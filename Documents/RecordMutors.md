# Record Mutors

> A **Record Mutor** is a pattern for implementing mutaton of immutable record objects.

Record Mutors are classes with mutable data properties.  A Record Mutor is loaded from it's immutable record and provides a mutated copy of the record as output.  It's stateful: it maintains a copy of the original record and has `IsDirty` and `IsNew` properties.

The principle use of Record Mutors is as the `model` for edit forms.

Note: The mutors in the solution use the `[TrackState]` custom attribute to tell the `EditStateTracker` in the Blazor `EditForm` to track the individual property state.

## The Record Mutor

Consider the `Customer` domain record.

```csharp
public sealed record DmoCustomer
{
    public CustomerId Id { get; init; }
    public Title Name { get; init; }
}
```

The UI needs access to read/write fields representing the editable data in the record: in this case just `Name`.

This is where we use the *Mutor* pattern.  

First an Interface: this is used in boilerplate generic Form classes.

```csharp
public interface IRecordMutor<TRecord>
    where TRecord : class
{
    public TRecord BaseRecord { get; }
    public bool IsDirty { get; }
    public bool IsNew { get; }
    public TRecord Record { get; }
    public void Reset();
    public RecordState State { get; }
}
```

An abstract base class to implement common boilerplate code:

```csharp
public abstract class RecordMutor<TRecord>
    where TRecord : class
{
    public TRecord BaseRecord { get; protected set; }
    public bool IsDirty => !this.Record.Equals(BaseRecord);
    public virtual bool IsNew { get; }
    public virtual TRecord Record { get; } = default!;
    public abstract void Reset();

    protected RecordMutor(TRecord record)
    {
        this.BaseRecord = record;
        this.SetFields();
    }

    public RecordState State => (this.IsNew, this.IsDirty) switch
    {
        (true, _) => RecordState.NewState,
        (false, false) =>RecordState.CleanState,
        (false, true) => RecordState.DirtyState,
    };

    protected abstract void SetFields();
}
```

And the Customer record UI mutor:

```csharp
public sealed class CustomerRecordMutor : RecordMutor<DmoCustomer>, IRecordMutor<DmoCustomer>
{
    [TrackState] public string? Name { get; set; }
    public override bool IsNew => BaseRecord.Id.IsNew;

    private CustomerRecordMutor(DmoCustomer record)
        : base(record) { }

    protected override void SetFields()
    {
        this.Name = this.BaseRecord.Name.Value;
    }

    public override DmoCustomer Record => this.BaseRecord with
    {
        Name = new(this.Name ?? "No Name Set")
    };

    public override void Reset()
        => this.SetFields();

    public static CustomerRecordMutor Load(DmoCustomer record)
        => new CustomerRecordMutor(record);

    public static CustomerRecordMutor NewMutor()
        => new CustomerRecordMutor(DmoCustomer.NewCustomer());
}
```

It's locked down.

1. A `CustomerRecordMutor`can only be initialised through the static `Load` and `Create` methods.
1. The only editable field is `Name`.
1. The only method that changes the internal state is `Reset`.
1. The current state is available through `IsDirty`.
1. The *mutated record* (to save) is available through `Record`.

How do you use it?

In Blazor the `CustomerRecordMutor` instance is the `model` for the `EditContext`.  On `Save`, submit the record from the `Record` property into the data pipeline.

## Emulating the Process

We can emulate the UI process in a simple test.  See the inline commentary for details.

```csharp
[Fact]
public async Task UpdateACustomer()
{
    // Get a fully stocked DI container
    var provider = GetServiceProvider();
    var mediator = provider.GetRequiredService<IMediatorBroker>()!;

    // Get the test item and it's Id from the Test Provider
    var controlRecord = _testDataProvider.GetTestCustomer();
    var controlId = controlRecord.Id;

    // Get the record from the data pipeline
    var customerResult = await mediator.DispatchAsync(new CustomerRecordRequest(controlId));
    Assert.True(customerResult.HasSucceeded);

    // Load the mutor
    var mutor = CustomerRecordMutor.Load(customerResult.Write(DmoCustomer.NewCustomer()));

    // emulate a UI Edit
    mutor.Name = $"{mutor.Name} - Update";

    //emulate the Validation process 
    var validator = new CustomerRecordMutorValidator();
    var validateResult = validator.Validate(mutor);
    var editedRecord = mutor.Record;
    Assert.True(validateResult.IsValid);

    // Emulate the save button
    var customerUpdateResult = await mediator.DispatchAsync(CustomerCommandRequest.Create(mutor.Record, mutor.State));

    // Get the new record from the data pipeine
    customerResult = await mediator.DispatchAsync(new CustomerRecordRequest(controlId));

    // check it matches the test record
    Assert.True(customerResult.HasSucceeded);
    Assert.Equivalent(editedRecord, customerResult.Write(DmoCustomer.NewCustomer()));
}
```