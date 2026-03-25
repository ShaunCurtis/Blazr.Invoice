# Item Requests

Item requests may seem easy, but how do you know what the entity identity data type is?

To abstract that problem away we can define a request as follows:

```csharp
public record RecordQueryRequest<TRecord>(
    Expression<Func<TRecord, bool>> FindExpression,
    CancellationToken? Cancellation = null
);
```

## The CQS Handler

We can define a generic CQS handler extension to `DbContext` like this to handle the provided expression:

```csharp
extension (DbContext dbContext)
{
    public async Task<Result<TRecord>> GetRecordFromDatastoreAsync<TRecord>(RecordQueryRequest<TRecord> request)
        where TRecord : class
    {
        dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        var record = await dbContext.Set<TRecord>()
            .FirstOrDefaultAsync(request.FindExpression)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        if (record is null)
            return ResultT.Fail<TRecord>($"No record retrieved with the Key provided");

        return ResultT.Read(record);
    }
}
```

## The Mediator Request

So, how do we code this into the front end?

Here's the Customer Record Request:

```csharp
public readonly record struct CustomerRecordRequest(CustomerId Id) 
    : IRequest<Result<DmoCustomer>>
{
    public static CustomerRecordRequest Create(CustomerId Id)
        => new(Id);
}
```

And then the EntityFramework Mediator Handlertakes the provided Id and creates the query expression.

```csharp
public sealed class CustomerRecordHandler : IRequestHandler<CustomerRecordRequest, Result<DmoCustomer>>
{
    private IDbContextFactory<InMemoryInvoiceTestDbContext> _factory;

    public CustomerRecordHandler(IDbContextFactory<InMemoryInvoiceTestDbContext> dbContextFactory)
        => _factory = dbContextFactory;

    public async Task<Result<DmoCustomer>> HandleAsync(CustomerRecordRequest request, CancellationToken cancellationToken)
    {
        using var dbContext = _factory.CreateDbContext();

        return await dbContext
            .GetRecordFromDatastoreAsync<DvoCustomer>(new RecordQueryRequest<DvoCustomer>(item => item.CustomerID == request.Id.Value))
            .MapAsync(item => item.MapToDmo);
    }
}
```

The Mediator Handler and the CQS handler are all back end.  The UI or front end API passes a `CustomerRecordRequest` to the configured Mediator Handler.


In an API context the client handler would look like this:

```csharp
public sealed class CustomerAPIRecordHandler : IRequestHandler<CustomerRecordRequest, Result<DmoCustomer>>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerAPIRecordHandler(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<Result<DmoCustomer>> HandleAsync(CustomerRecordRequest request, CancellationToken cancellationToken)
    {
        using var http = _httpClientFactory.CreateClient(AppDictionary.Common.AppHttpClient);

        var httpResult = await http.PostAsJsonAsync<CustomerRecordRequest>(AppDictionary.Customer.CustomerRecordApiUrl, request, cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        if (!httpResult.IsSuccessStatusCode)
            return ResultT.Fail<DmoCustomer>($"The server returned a status code of : {httpResult.StatusCode}");

        var listResult = await httpResult.Content.ReadFromJsonAsync<Result<DmoCustomer>>()
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return listResult ?? ResultT.Fail<DmoCustomer>($"The result was null");
    }
}
```

And the server-side API endpoint:

```csharp
public static class CustomerApiEndpoints
{
    internal static void AddCustomerApiEndpoints(this WebApplication app)
    {
        app.MapPost(AppDictionary.Customer.CustomerRecordApiUrl, GetRecordAsync);
    }

    internal static async Task<IResult> GetRecordAsync(
        CustomerRecordRequest request,
        IMediatorBroker mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.DispatchAsync(request, cancellationToken);

        return Results.Ok(result);
    }
}
```