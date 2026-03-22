# Factories, Extensions and Internal

Why do you need them?

Let's look at the `InvoiceEntity` and `InvoiceEntityMutor` to demonstrate their purpose.

## Object Creation Control

Creating an `InvoiceEntityMutor` is a two step process:

1. Create the object.
2. Asynchronously load the `InvoiceEntity` from the data pipeline [and report and deal with problems].

You can't use `new`.  It's purely synchronous.

Let's look at the `InvoiceEntityMutor` new method:

```csharp
    public InvoiceEntityMutor(IMediatorBroker mediator, IMessageBus messageBus, InvoiceId id)
    {
        // Get some services
        _mediator = mediator;
        _messageBus = messageBus;
        //Set some initial values so the mutor is in a valid state until the LoadAsync finishes
        this.BaseEntity = InvoiceEntity.Create();
        this.InvoiceEntity = this.BaseEntity;
        // Load the Invoice Entity
        this.LoadTask = this.LoadAsync(id);
    }
```

It's looks like a *DI* constructor, but how do you provide `id`?

The answer is a factory.  Here's the `InvoiceMutorFactory`.

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

This is a *DI* Service.  `GetInvoiceEntityMutorAsync` is the factory method to construct an `InvoiceEntityMutor`.  It uses the `ActivatorUtilities` utility to create an instance of `InvoiceEntityMutor` within the DI context, provides the id as an additional parameter and then awaits the `LoadTask`.

## Extensions and Internal

Extensions provide a mechanism to add extra functionality where it's needed, and hide it from where it's not.

Consider `DboCustomer`.  It's the DTO that maps to the Customer database table record.

It's a DTO so:

1. It has no functionality.  
1. It's declared `internal` so it's use is confined to the *Infrastructure Domain* project.
1. It's declared `sealed` because there's no reason for inheritance

```csharp
internal sealed record DboCustomer : ICommandEntity
{
    [Key] public Guid CustomerID { get; init; } = Guid.Empty;
    public string CustomerName { get; init; } = string.Empty;
}
```

However, it's also the most convenient place to hang *mappers* so we can write something like this:

```csharp
var customers = provider.Items.Select(item => item.MapToDmo)
```

So we can add an extension method to `DboCustomer` like this:

```csharp
internal static class CustomerMapExtensions
{
    extension(DboCustomer item)
    {
        public DmoCustomer MapToDmo => new DmoCustomer()
        {
            Id = CustomerId.Load(item.CustomerID),
            Name = new(item.CustomerName ?? Title.DefaultValue)
        };
    }
}
```
