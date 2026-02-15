# Strongly Typed Identifiers

A source of bugs in an application is Id mismatching.  Let's look at an example to demonstrate.

```csharp
public record WeatherForecast
{
	public Guid Id {get; init;}
	//...
}

public record WeatherStation
{
	public Guid Id {get; init;}
	//...
}
```

There's nothing to stop you doing this inadvertently.

```csharp
var forecast = new WeatherForecast();
var station = GetStaton(forecast.Id);
```

The solution is *Strongly Typed IDs*: each Id is defined as a unique type.

Here's the CustomerId.

```csharp
public readonly record struct CustomerId : IEntityId, IEquatable<CustomerId>
{
    public Guid Value { get; private init; }
    public bool IsNew { get; private init; }

    public CustomerId()
    {
        Value = Guid.CreateVersion7();
        IsNew = true;
    }

    public CustomerId(Guid guid)
        => Value = guid == Guid.Empty
            ? throw new InvalidGuidIdException()
            : guid;

    public static CustomerId Load(Guid id)
        => id == Guid.Empty
            ? throw new InvalidGuidIdException()
            : new CustomerId(id);

    public static CustomerId NewId => new(Guid.CreateVersion7()) { IsNew = true };

    public override string ToString()
        => Value.ToString();

    public string ToString(bool shortform)
        => Value.ToString().Substring(28);

    public bool Equals(CustomerId other)
        => this.Value == other.Value;

    public override int GetHashCode()
        => HashCode.Combine(this.Value);
}
```

Note:

1. You can only create a valid Guid: `Guid.Empty` is invalid.
1. The `ToString` and equality methods are overridden.
1. There is a mechanism for creating a new `CustomerId`.


