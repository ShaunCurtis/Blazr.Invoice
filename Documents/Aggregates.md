# Aggregate Entities in Functional Programing

Updating objects where the consequences of the change are limited to the object are simple.  Changing an object with business rules that encompass other objects is complex.  In OOP the aggregate pattern sets out to address this problem.  

> An aggregate entity is a group of objects bound by one or more application rules.  The purpose of the aggregate is to ensure those rules are applied, and cannot be broken.  
 
The OOP pattern treats the aggregate as a black box.  All changes are submitted to the black box, not the individual objects within it.  The black box applies the changes and runs the logic to ensure consistency.

A really important point to note is:

> An aggregate only makes sense in a mutation context: you don't need aggregates to list or display data.  

An invoice is a good example of an aggregate. Delete a line item, and the aggregate needs to track the deletion of the item, calculate the new total amount and update the invoice.  Persist the aggregate to the data store, and the aggregate needs to hold the necessary state information to apply the appropriate update/add/delete actions as a *Unit of Work* to the data store.

## The Problems with Aggregates

Conceptually coding aggregates seems plain sailing.  The problem is in the detail.  It's easy to slip the boundary, include more related objects.  Complex aggregate entities quickly grow: they become god classes.

## The Functional Challenge

At first glance the aggregate concept and Functional Programming's demand for immutability see at odds: how do you fit and square peg in a round hole?
Let's look at how we can overcome it.

### A Note Before We Start

Much of the code is written in *Functional Programming style*.  You will see a lot of small, sometimes one line functions.  Many are `static.  There's extensive use of extension methods to add *local* functionality to existing objects.  Chaining is common to *compose* more complex functions from small code snippets. 

### Invoice Entity

The `InvoiceEntity` code skeleton:

```csharp
public sealed record InvoiceEntity : IEquatable<InvoiceEntity>
{
    public DmoInvoice InvoiceRecord { get; private init; }
    public ImmutableList<DmoInvoiceItem> InvoiceItems { get; private init; }

    private InvoiceEntity(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceInvoiceItems);

    public bool IsDirty(InvoiceEntity control);
    public Result<DmoInvoiceItem> GetInvoiceItem(InvoiceItemId id);
    public InvoiceEntity Mutate(DmoInvoice invoice);
    public InvoiceEntity Mutate(IEnumerable<DmoInvoiceItem> invoiceItems);
    public Result<InvoiceEntity> CheckEntityRules();
    public InvoiceEntity ApplyEntityRules();
    public bool Equals(InvoiceEntity? other);
    public override int GetHashCode();
    public static InvoiceEntity Load(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceItems);
    public static Result<InvoiceEntity> TryLoad(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceItems);
    public static InvoiceEntity Create();
    public static InvoiceEntity Create(DmoInvoice invoice);
}
```

The objects data is immutable: `records` and an `ImmutableList` of `records`.

Most of the code is self explanatory. Notes:

1. The constructor is private.  Object instances must be created using one of the static `Load` or `Create` factory methods.
2. `TryLoad` applies the entity rules and returns a `FailureResult` if the rule check fails.
3. The custom `Equals` uses `SequenceEqual` with two ordered collections to test equality.

### Invoice Entity Mutor

`InvoiceEntityMutor` is the mutable object we use to manage Invoice mutation.

The skeleton class:

```csharp
public sealed class InvoiceEntityMutor
{
    public InvoiceEntity BaseEntity { get; private set; }
    public InvoiceEntity InvoiceEntity { get; private set; }
    public Result LastResult { get; private set; }
    public bool IsNew  {get;}
    public Task LoadTask { get; private set; }
    public bool IsDirty  {get;}
    public RecordState State {get;}

    public InvoiceEntityMutor(IMediatorBroker mediator, IMessageBus messageBus, InvoiceId id);

    public Result Dispatch(Func<InvoiceEntity, Result<InvoiceEntity>> dispatcher);
    public async Task<Result> SaveAsync();
    public async Task<Result> DeleteAsync();
    public InvoiceItemRecordMutor GetInvoiceItemRecordMutor(InvoiceItemId id);
    public InvoiceItemRecordMutor GetNewInvoiceItemRecordMutor();
    public Result Reset();
}
```

Note the complex constructor.  `InvoiceEntityMutor` instances should only be obtained from the DI registered `InvoiceMutorFactory`. 

```csharp
public sealed class InvoiceMutorFactory
{
    private IServiceProvider _serviceProvider;

    public InvoiceMutorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<InvoiceEntityMutor> GetInvoiceEntityMutorAsync(InvoiceId id)
    {
        var mutor = ActivatorUtilities.CreateInstance<InvoiceEntityMutor>(_serviceProvider, new object[] { id });
        await mutor.LoadTask;
        return mutor;
    }

    public async Task<InvoiceEntityMutor> CreateInvoiceEntityMutorAsync()
    {
        var mutor = ActivatorUtilities.CreateInstance<InvoiceEntityMutor>(_serviceProvider, new object[] { InvoiceId.NewId });
        await mutor.LoadTask;
        return mutor;
    }
}
```

The key mutation method is `Dispatch`.  It's a *Monadic Function* and `Dispatch` is a `Bind` operation. 

```csharp
public Return Dispatch(Func<InvoiceEntity, Return<InvoiceEntity>> dispatcher)
{
    InvoiceEntity = dispatcher.Invoke(InvoiceEntity)
        .WriteReturn(ret => LastResult = ret)
        .Write(defaultValue: this.InvoiceEntity);

    _messageBus.Publish<InvoiceEntity>(this.InvoiceEntity.InvoiceRecord.Id);

    return this.LastResult;
}
```

## Usage

The simplest way to understand usage is through *Tests*.

As an example let's look in detail at the `UpdateAnInvoiceItem` test.

First call the helpers to get the DI service container, mediator service and the Mutor Factory.  *You can see these in detail in the Repo*.

```csharp
// Get a fully stocked DI container
var provider = GetServiceProvider();
var mediator = provider.GetRequiredService<IMediatorBroker>()!;
var mutorFactory = provider.GetRequiredService<InvoiceMutorFactory>()!;
```

Get a valid `InvoiceId` for an invoice to edit.

```csharp
var entity = await this.GetASampleEntityAsync(mediator);
var Id = entity.InvoiceRecord.Id;
```

Get the entity Mutor for the Invoice Id.

```csharp
var entityMutor = await mutorFactory.GetInvoiceEntityMutorAsync(entity.InvoiceRecord.Id);
```

This uses the factory to instanciate the `InvoiceEntityMutor` and load the data from the data pipeline asynchronously. 

```csharp
public async Task<InvoiceEntityMutor> GetInvoiceEntityMutorAsync(InvoiceId id)
{
    var mutor = ActivatorUtilities.CreateInstance<InvoiceEntityMutor>(_serviceProvider, new object[] { id });
    await mutor.LoadTask;
    return mutor;
}
```
Next we create an `InvoiceItemRecordMutor` from the first item record

```csharp
var itemMutor = InvoiceItemRecordMutor.Load(entityMutor.InvoiceEntity.InvoiceItems.First());
```

And simulate updating the value through a UI edit form:

```csharp
itemMutor.Amount = 59;
```

When we click save on the item we update the Entity Mutor by passing the itemMutor's Dispatcher to the entity mutor's dispatcher.

```csharp
var actionResult = entityMutor.Dispatch(itemMutor.Dispatcher);
Assert.True(actionResult.Success);
```

The action dispatcher, builds a `SaveInvoiceItemAction` Action and calls it's dispatcher:

```csharp
public Func<InvoiceEntity, Result<InvoiceEntity>> Dispatcher =>
    entity => this.IsDirty
        ? SaveInvoiceItemAction.Create(this.Record).Dispatcher(entity)
        : ResultT.Read(entity);
```

`SaveInvoiceItemAction`:

```csharp
public record SaveInvoiceItemAction
{
    private readonly DmoInvoiceItem _invoiceItem;

    public SaveInvoiceItemAction(DmoInvoiceItem invoiceItem)
        => _invoiceItem = invoiceItem;

    public Result<InvoiceEntity> Dispatcher(InvoiceEntity entity)
        => entity.InvoiceItems.Any(_item => _invoiceItem.Id.Equals(_item.Id))
            // if the item exists, update it
            ? Update(entity, _invoiceItem)
            // otherwise add it
            : Add(entity, _invoiceItem);

    private static Result<InvoiceEntity> Add(InvoiceEntity entity, DmoInvoiceItem invoiceItem)
        => ResultT.Read(entity.InvoiceItems.Add(invoiceItem))
            .Map(items => entity.Mutate(items));

    private static Result<InvoiceEntity> Update(InvoiceEntity entity, DmoInvoiceItem invoiceItem)
        => entity.GetInvoiceItem(invoiceItem.Id)
            .Map(item => entity.InvoiceItems.Replace(item, invoiceItem))
            .Map(items => entity.Mutate(items));

    public static SaveInvoiceItemAction Create(DmoInvoiceItem invoiceItem)
        => (new SaveInvoiceItemAction(invoiceItem));
}
```

When save the entity by calling `SaveAsync` on the entity mutor:

```csharp
var commandResult = await entityMutor.SaveAsync();
```

This dispatches an `InvoiceCommandRequest` to Mediator.

```csharp
public async Task<Return> SaveAsync()
    => await _mediator.DispatchAsync(new InvoiceCommandRequest(this.InvoiceEntity, EditState.Dirty, Guid.NewGuid()))
        .WriteReturnAsync(ret => this.LastResult = ret);
```

Finally we test by getting the entity from the data store and comparing it against the new entity.

```csharp
var entityResult = await mediator.DispatchAsync(new InvoiceEntityRequest(Id));

Assert.True(entityResult.HasValue);

// Get the Mutor Entities
var updatedEntity = entityMutor.InvoiceEntity;
var dbEntity = entityResult.Value!;

// Check the stored data is the same as the edited entity
Assert.Equivalent(updatedEntity, dbEntity);
```

### UpdateInvoiceItemAction in Detail

`UpdateInvoiceItemAction` is a simple record:

```csharp
using Blazr.App.Core.Invoices;
namespace Blazr.App.Core;
public record UpdateInvoiceItemAction
{
    private readonly DmoInvoiceItem _invoiceItem;

    public UpdateInvoiceItemAction(DmoInvoiceItem invoiceItem)
        => _invoiceItem = invoiceItem;

    public Return<InvoiceEntity> Dispatcher(InvoiceEntity entity)
        => entity.ReplaceInvoiceItem(_invoiceItem);

    public static UpdateInvoiceItemAction Create(DmoInvoiceItem invoiceItem)
        => (new UpdateInvoiceItemAction(invoiceItem));
}
```

The relevant `InvoiceEntity` extension methods:

```csharp
internal Return<InvoiceEntity> ReplaceInvoiceItem(DmoInvoiceItem record)
    => @this.GetInvoiceItem(record)
        .Bind(item => @this.MutateWithEntityRulesApplied(@this.InvoiceItems.Replace(item, record)));

private Return<DmoInvoiceItem> GetInvoiceItem(DmoInvoiceItem item)
    => @this.GetInvoiceItem(item.Id);

internal Return<DmoInvoiceItem> GetInvoiceItem(InvoiceItemId id)
    => Return<DmoInvoiceItem>.Read(
        value: @this.InvoiceItems.SingleOrDefault(_item => _item.Id == id),
        errorMessage: "The record does not exist in the Invoice Items");

private Return<InvoiceEntity> MutateWithEntityRulesApplied(IEnumerable<DmoInvoiceItem> invoiceItems) 
    => InvoiceEntityFactory.ApplyEntityRules(InvoiceEntity.Read(@this.InvoiceRecord, invoiceItems)).ToReturnT;

```

And Factory methods:

```csharp
internal static InvoiceEntity ApplyEntityRules(InvoiceEntity entity)
    => InvoiceEntity.Read(
        invoice: entity.InvoiceRecord with { TotalAmount = new(entity.InvoiceItems.Sum(item => item.Amount.Value)) },
        invoiceItems: entity.InvoiceItems);
```

Note that many of the methods are `Internal` to hide them from the UI projects assemblies.

