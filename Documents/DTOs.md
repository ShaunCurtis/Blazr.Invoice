# DTO's

A Data Transfer Object (DTO) is a design pattern used to transfer data between systems and layers within an application.

The key characteristic of DTOs is **Simplicity**:  DTOs are simple objects that contain no business logic. They typically consist of fields and properties to hold data.

In modern C# a DTO should be implemented as a  `record`  type to take advantage of immutability and value-based equality.  There's no valid reason to use a class for a DTO in C# anymore.

In the demo application we can see DTOs used in the Data Pipeline.

`DmoCustomer`that represents a customer in our system. While this domain object contains no business logic it uses value objects with built in validation.  It's a simple domain entity, not a DTO.

```csharp
public sealed record DmoCustomer
{
    public CustomerId Id { get; init; }
    public Title Name { get; init; }

    public static DmoCustomer NewCustomer()
        => new DmoCustomer() { Id = CustomerId.NewId };
}
```

Dtos are used within the infrastructure layer to map the domain object to the database.

The `DboCustomer` maps directly to the *Customer* table.  It's only used for commands i.e. writing to the database.  It consists of primitives with no validation.

```csharp
internal sealed record DboCustomer : ICommandEntity
{
    [Key] public Guid CustomerID { get; init; } = Guid.Empty;
    public string CustomerName { get; init; } = string.Empty;
}
```

The `DvoCustomer` is the query DTO that represents the customer in the database. It normally maps directly to a view.  It's only used for queries i.e. reading from the database.

```csharp
internal sealed record DvoCustomer
{
    [Key] public Guid CustomerID { get; init; } = Guid.Empty;
    public string? CustomerName { get; set; }
}
```

The mapping between the domain object and the DTOs is done using extension methods.  This keeps the mapping logic separate from the domain and infrastructure layers.

```csharp
internal static class CustomerMapExtensions
{
    extension(DmoCustomer item)
    {
        public DboCustomer MapToDbo => new DboCustomer()
        {
            CustomerID = item.Id.Value,
            CustomerName = item.Name.Value
        };
    }

    extension(DvoCustomer item)
    {
        public DmoCustomer MapToDmo => new DmoCustomer()
        {
            Id = CustomerId.Load(item.CustomerID),
            Name = new(item.CustomerName ?? Title.DefaultValue)
        };
    }
}
```

Note that everything is immutable and internal to the infrastructure layer.  This ensures that the DTOs are only used for their intended purpose and cannot be modified outside of the infrastructure layer.

