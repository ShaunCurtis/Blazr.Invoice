# Aggregates

The aggregate concept is a fundimental building block of applications. 

> An aggregate is a set of objects bound by a set of application rules.  The purpose of the aggregate is to apply those rules to maintain application consisistency.  
 

Editing an object where the consequences of that change are restricted to the object is simple.  Editing a customer's email has no direct consequences on the rest of the application.  You may need to resend invoices because the old one was wrong, but that's part of a process, not the integrity of the data within the application.

Editing an invoice line item object has consequences.  Change the unit cost or quantity and the invoice object now has the wrong total value. 

Aggregates address this problem.  

An aggregate is a black box.  All changes are applied to the black box, not the individual objects within it.  The black box contains the logic to ensure application consistency.  An aggregate only has purpose when you change an object to which those rules apply.  

Delete a line item through the aggregate, and it tracks the deletion of the item, calculates the new total amount and updates the invoice.  Persist the aggregate and it provides the persistance layer update/add/delete information to apply the changes as a *Unit of Work*.

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

```csharp
public DmoCustomer
{
    public CustomerId CustomerId { get; init; } = new(Guid.Empty);
    public string CustomerName { get; init; } = string.Empty;
}
```

```csharp
public record Invoice
{
    public InvoiceId InvoiceId { get; init; } = new(Guid.Empty);
    public CustomerId CustomerId { get; init; } = new(Guid.Empty);
    public decimal TotalAmount { get; init; }
    public DateOnly Date { get; init; }
}
```
 
```csharp
public record DmoInvoiceItem
{
    public InvoiceItemId InvoiceItemId { get; init; } = new(Guid.Empty);
    public InvoiceId InvoiceId { get; init; } = new(Guid.Empty);
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
}
```

## The Composite

A composite is a wrapper.  The invoice [the aggreagate root] is an object within the composite. 

```csharp
public class InvoiceComposite
{
    public Invoice Invoice {get;}
    public IEnumerable<InvoiceItems> Items {get;}
}
```

`Invoice` is a `record` and `Items` an `IEnumerable` of `InvoiceItems`.  Everything is read only.

### Managing Mutation

Change is managed within the composite using `Blazor.Flux` which is a simple indexed *Flux* pattern library.

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