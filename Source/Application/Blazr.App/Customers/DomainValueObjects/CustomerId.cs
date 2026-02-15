/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

/// <summary>
/// 
/// CustomerId is a Strongly Typed Id
/// It has two "States":
///   - New - where it has an Id that it has created.  This signals the data store that saving is an Add operation
///   - Existing - where the Id was inserted as part of the data pipeline read process.
///          This signals the data store that saving is an Update operation
///   
/// It implements custom:
/// -  ToString methods
/// -  Equality checking to compare only the Guids and ignore IsNew.
/// 
/// </summary>
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
