# List Requests

Generically we can define a request for a list like this:

```csharp
ListResult GetItemsAsync(ListRequest request);
```

## The Challenges

Lists present the greatest challenges in data retrieval.

Why can't you just do:

```csharp
var list = await dbContext.Set<TRecord>().ToListAsync();
```

Consider the following:

1. Do you really want to retrieve all the records at once.
2. What are you going to do with 2000 retrieved records?  Display them on a single page?
3. Can you gracefully handle retrieving a million records?
4. What order do you want the data set in?
5. Do you want to filter the data set?

## The CQS Request

Building these requirments into the design, we can define a struct like this for the data pipleine server side backend:

```csharp
public record ListQueryRequest<TRecord>
{
    public int StartIndex { get; init; }
    public int PageSize { get; init; }
    public Expression<Func<TRecord, bool>>? FilterExpression { get; init; }
    public Expression<Func<TRecord, object>>? SortExpression { get; init; }
    public bool SortDescending { get; init; } = true;
    public CancellationToken Cancellation { get; init; }

    public ListQueryRequest()
    {
        StartIndex = 0;
        PageSize = 1000;
        FilterExpression = null;
        SortExpression = null;
        Cancellation = new();
    }
    
    public static ListQueryRequest<TRecord> CreateWithDefaultValues(CancellationToken? cancellationToken = null)
        => new ListQueryRequest<TRecord>() { Cancellation = cancellationToken ?? new() };
}
```

`StartIndex` and `PageSize` define the data chunk we want to retrieve.  We can apply the values as `Skip` and `Take` actions in the `IQueryable` query.

Sorting is defined by an `Expression<Func<TRecord, object>>` and a `SortDescending` flag. 

Filtering is defined by an `Expression<Func<TRecord, bool>>`.  This is a little more complex and we'll see how we handle this shortly.

## The Mediator Request

The UI request may take several forms, based on the UI grid control or table you use.  This needs to be translated in the Presentation layer into a Mediator request.

The base implementation:

```csharp
public record BaseListRequest
{
    public int StartIndex { get; init; } = 0;
    public int PageSize { get; init; } = 1000;
    public string? SortColumn { get; init; } = null;
    public bool SortDescending { get; init; } = false;
}
```

Each request is record specific.  The *Customer* list request looks like this:

```csharp
public record CustomerListRequest : BaseListRequest, IRequest<Result<ListItemsProvider<DmoCustomer>>>
{
    public static Result<CustomerListRequest> FromGridState(GridState<DmoCustomer> state)
        => ResultT.Read<CustomerListRequest>(new CustomerListRequest()
        {
            PageSize = state.PageSize,
            StartIndex = state.StartIndex,
            SortColumn = state.SortField,
            SortDescending = state.SortDescending
        });
}
```

Requests may contain additional information such as filter values.

This is a weather forecast request with a `Summary` filter:

```csharp
public record WeatherForecastListRequest
    : BaseListRequest, IRequest<Result<ListItemsProvider<DmoWeatherForecast>>>
{
    public string? Summary { get; init; }
}
```

## The Mediator Handler

The handler provides the interface into the data store.  It implements the `IRequestHandler` that takes an `IRequest` request and returns a `Result<ListResult<DmoCustomer>>`.  The handler is responsible for getting the data from the database and returning it in the requested format.

It builds a `ListQueryRequest` object from the request and passes it to the `GetItemsAsync` method on a `DbContext` instance.  This returns a `ListResult` object that is mapped to the `DmoCustomer` object and returned as a `Result<ListResult<DmoCustomer>>`.  The `GetSorter` and `GetFilter` methods build the `SortExpression` and `FilterExpression` objects from data provided in the list request.

```csharp
public sealed class CustomerListHandler : IRequestHandler<CustomerListRequest, Result<ListItemsProvider<DmoCustomer>>>
{
    private readonly IDbContextFactory<InMemoryInvoiceTestDbContext> _factory;

    public CustomerListHandler(IDbContextFactory<InMemoryInvoiceTestDbContext> factory)
        => _factory = factory;

    public async Task<Result<ListItemsProvider<DmoCustomer>>> HandleAsync(CustomerListRequest request, CancellationToken cancellationToken)
    {
        using var dbContext = _factory.CreateDbContext();

        return await dbContext
            .GetItemsFromDatastoreAsync<DvoCustomer>(GetListRequest(request, cancellationToken))
            .MapAsync(MapToDomainEntitiyListProvider);
    }

    private ListQueryRequest<DvoCustomer> GetListRequest(CustomerListRequest request, CancellationToken cancellationToken)
        => new ListQueryRequest<DvoCustomer>()
        {
            PageSize = request.PageSize,
            StartIndex = request.StartIndex,
            SortDescending = request.SortDescending,
            SortExpression = this.GetSorter(request.SortColumn),
            FilterExpression = this.GetFilter(request),
            Cancellation = cancellationToken
        };

    private static ListItemsProvider<DmoCustomer> MapToDomainEntitiyListProvider(ListItemsProvider<DvoCustomer> provider)
        => new ListItemsProvider<DmoCustomer>(
            Items: provider.Items.Select(item => item.MapToDmo),
            TotalCount: provider.TotalCount);

    private Expression<Func<DvoCustomer, object>> GetSorter(string? field)
        => field switch
        {
            "Id" => (Item) => Item.CustomerID,
            "ID" => (Item) => Item.CustomerID,
            "Name" => (Item) => Item.CustomerName ?? "No Customer Name Set",
            _ => (item) => item.CustomerID // Default Sort by ID
        };

    // No Filter Defined
    private Expression<Func<DvoCustomer, bool>>? GetFilter(CustomerListRequest request)
    {
        return null;
    }
}
```

## The Result

The data pipeline returns a `ListResult`.  This object contains the requested list of records and the total number of records in the query.

```csharp
public readonly record struct  ListResult<TRecord>(IEnumerable<TRecord> Items, int TotalCount);
```

This is wrapped in a `Result` object returnd by Mediator that contains the list result and status of the request.

### CQS DBContext Extensions

`GetItemsFromDatastoreAsync` is an extension method on `DbContext`.

The method builds a `IQueryable` object from the request to execute against the `DBContext`.  Once to retrieve the total number of rows and then a second time with the paging applied.

```csharp
public async Task<Result<ListItemsProvider<TRecord>>> GetItemsFromDatastoreAsync<TRecord>(ListQueryRequest<TRecord> request)
    where TRecord : class
{
    int totalRecordCount;

    // Turn off tracking.  We're only querying, no changes
    dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

    // Get the IQueryable DbSet for TRecord
    IQueryable<TRecord> query = dbContext.Set<TRecord>();

    // If we have a filter defined, apply the predicate delegate to the IQueryable instance
    if (request.FilterExpression is not null)
        query = query.Where(request.FilterExpression).AsQueryable();

    // Get the total record count after applying the filters
    totalRecordCount = query is IAsyncEnumerable<TRecord>
        ? await query.CountAsync(request.Cancellation).ConfigureAwait(ConfigureAwaitOptions.None)
        : query.Count();

    // If we have a sorter, apply the sorter to the IQueryable instance
    if (request.SortExpression is not null)
    {
        query = request.SortDescending
            ? query.OrderByDescending(request.SortExpression)
            : query.OrderBy(request.SortExpression);
    }

    // Apply paging to the filtered and sorted IQueryable
    if (request.PageSize > 0)
        query = query
            .Skip(request.StartIndex)
            .Take(request.PageSize);

    // Finally materialize the list from the data source
    var list = query is IAsyncEnumerable<TRecord>
        ? await query.ToListAsync().ConfigureAwait(ConfigureAwaitOptions.None)
        : query.ToList();

    return ResultT.Read(new ListItemsProvider<TRecord>(list, totalRecordCount));
}
```

## Service Registration

No objects are manually registered.  Everything is handled by the Mediator implementation.

## Request in Action

Here's a test that retrieves paged data:

```csharp
    [Theory]
    [InlineData(0, 10)]
    [InlineData(0, 20)]
    [InlineData(5, 10)]
    public async Task GetCustomerGrid(int startIndex, int pageSize)
    {
        var provider = GetServiceProvider();
        var mediator = provider.GetRequiredService<IMediatorBroker>()!;

        // Get the total expected count and the first record of the page
        var testCount = _testDataProvider.Customers.Count();
        var testFirstRecord = _testDataProvider.Customers.Skip(startIndex).First();

        var customerListResult = await mediator.DispatchAsync(new CustomerListRequest()
        {
            PageSize = pageSize,
            StartIndex = startIndex,
            SortColumn = null,
            SortDescending = false
        });
        Assert.IsType<SuccessResult<ListItemsProvider<DmoCustomer>>>(customerListResult);
        var listResult = ((SuccessResult<ListItemsProvider<DmoCustomer>>)customerListResult).Value;

        Assert.Equal(testCount, listResult.TotalCount);
        Assert.Equal(pageSize, listResult.Items.Count());
    }
```
