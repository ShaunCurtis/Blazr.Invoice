# Aggregates

Aggregates are a fundimental building block of applications.  Aggregates provide a framework for managing complex domain entities. 

> An aggregate is a group of objects bound by one or more application rules.  The purpose of the aggregate is to ensure  those rules are applied to maintain application consisistency.  
 

Editing an object where the consequences of that change are restricted to the object is simple.  Editing a customer's email has no direct consequences on the rest of the application.  You may need to resend invoices because the old one was wrong, but that's part of a process, not the integrity of the data within the application.

Editing an invoice line item object has consequences.  Change the unit cost or quantity and the invoice object now has the wrong total value. 

Aggregates address this problem.  

An aggregate is a black box.  All changes are applied to the black box, not the individual objects within it.  The black box contains the logic to maintain entity integrity and consistency.  

Delete a line item through the aggregate, and it tracks the deletion of the item, calculates the new total amount and updates the invoice.  Persist the aggregate and it provides the persistance layer update/add/delete information to apply the changes as a *Unit of Work*.

An important point to understand is you only need aggregeates when you need to change data.  If you're just reading data, you don't need aggregates, you can use standard simple patterns to get the data you need.

The aggregate provides the invoice and line items as read only objects.  No modifications allowed.


## The Classic Aggregate

The classic aggregate looks like this:

```csharp
public class InvoiceAggregate
{
    public int InvoiceID {get; set;}
    //...
    public ReadOnlyList<InvoiceItems> Items {get; private set;}

    //...  methods to change items
}
```

The Invoice is the aggregate root.  I'll discuss why I don't like this design later.

## The Classic Invoice Example

The rest of this article uses the Invoice example. The objects are minimal to keep things simple.

Note that the objects are immutable.  Good practice in aggregates to ensure consistency and changes can only be applied through the aggregate.

```csharp
public sealed record DmoCustomer : ICommandEntity
{
    public CustomerId Id { get; init; } = CustomerId.Default;
    public string CustomerName { get; init; } = string.Empty;
}
```

```csharp
public sealed record DmoInvoice
{
    public InvoiceId Id { get; init; } = InvoiceId.Default;
    public CustomerId CustomerId { get; init; } = CustomerId.Default;
    public string CustomerName { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public DateOnly Date { get; init; }
}
```
 
```csharp
public sealed record DmoInvoiceItem
{
    public InvoiceItemId Id { get; init; } = InvoiceItemId.Default;
    public InvoiceId InvoiceId { get; init; } = InvoiceId.Default;
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
}
```

## The Aggregate

The aggregate is a wrapper containing all objects to which the rule/s apply.  In out case the invoice [the aggreagate root] and the collection of invoice items. 

```csharp
public class InvoiceComposite
{
    public DmoInvoice InvoiceRecord { get; private set; }
    public IEnumerable<InvoiceItem> InvoiceItems
        => this.Items.AsEnumerable();

    private List<InvoiceItem> Items { get; set; }
        = new List<InvoiceItem>();
}
```

`Invoice` is a `record` and `Items` an `IEnumerable` of `InvoiceItems`.  Everything is read only.

The rule is simple.  The `TotalAmount` of the `Invoice` is the sum of the `Amount` of all `InvoiceItems`.  It's applied like this:
```csharp
private void Process()
{
    decimal total = 0m;
    foreach (var item in Items)
        total += item.Amount;

    if (total != this.TotalAmount)
    {
        this.InvoiceRecord = this.InvoiceRecord with { TotalAmount = total };
        this.State = State.AsDirty;
    }
    this.StateHasChanged?.Invoke(this, this.InvoiceRecord.Id);
}
```

### Managing Mutation

Change is managed within the aggregate by implementing a *Flux* style pattern.  Each mutation is defined in an action and passed through a *Dispatcher* method into the aggregate.

For example, the invoice is updated by creating an `UpdateInvoiceAction`:

```csharp
    public readonly record struct UpdateInvoiceAction(DmoInvoice Item);
```
And passing it to the `Dispatcher` method:

```csharp
    public Result Dispatch(UpdateInvoiceAction action)
    {
        this.UpdateInvoice(action.Item);
        return Result.Success();
    }

    private void UpdateInvoice(DmoInvoice invoice)
    {
        this.InvoiceRecord = invoice;
        this.State = State.AsDirty;
        this.Process();
    }
```

```csharp
``` 

   composite using `Blazor.Flux` which is a simple indexed *Flux* pattern library.

We define our Flux contexts:

```csharp
    private FluxContext<InvoiceId, DmoInvoice> _invoice;
    private List<FluxContext<InvoiceItemId, DmoInvoiceItem>> _invoiceItems = new();
```

And then link our public properties to them.

```csharp
    public DmoInvoice Invoice => _invoice.Item;
    public FluxState State => _invoice.State;
    public IEnumerable<DmoInvoiceItem> InvoiceItems => _invoiceItems.Select(item => item.Item).AsEnumerable();
``` 





## Composite

A composite is a wrapper that maintains consistency within a set of intimately related entities.

Some important points:

1. All mutations are applied to the composite which the consistency logic.
2. All objects obtained from the composite are read only.
3. The composite maintains state for persistance and the persistance transaction is a unit of work.  Changes are persisted or discarded as a unit.
4. Composites are only used when you need to create/update/delete data. 

A composite differs from an aggregate root in that it is purely a wrapper.  The aggregate root is an object within the composite.

An Invoice aggregate would look like this:

```csharp
public class InvoiceAggregate
{
    public int InvoiceID {get; set;}
    //...
    public List<InvoiceItems> Items {get; private set;}

    //...  methods to change items
}
```

While an Invoice Composite would look like this:

```csharp
public class InvoiceComposite
{
    public Invoice Invoice {get;}
    public List<InvoiceItems> Items {get;}
}
```

## The Boundary Decision

The most difficult decision to make in designing aggregates or composites is the boundary.  What objects are within and outside the wrapper.  It's very easy to add too much.

In our classic example we have `Invoice`, `InvoiceItem` and `Customer` objects.  They are all related, so do all three belong with the composite?

An `InvoiceItem` is intrinsically linked to an invoice.  It has no context outside the `Invoice`.  Changing the `Amount` on an `InvoiceItem` changes the `TotalAmount` in the `Invoice`.

On the other hand a `Customer` is a stand alone item.  Changing data on the invoice doesn't affect the integrity of the `Customer` object.  It doesn't belong inside the aggregate/composite.

## The Aggregate Root

It's very easy to fall into the trap of making the Invoice the aggregate root.  In the classic example above that's exactly what we've done.  `InvoiceID`, `InvoiceDate`, `InvoiceAmount` all become properties of the aggregate.

If you step back and apply the *Single Responsibility Principle*  you realise you're giving the aggregate root two responsibilities maintaining the root data and applying the business rules to the whole aggregate.

The aggregate is the fascade for applying consistency/business rules to the objects within the aggregate boundary.

So in my aggregates, the Invoice is just another object within the aggregate.