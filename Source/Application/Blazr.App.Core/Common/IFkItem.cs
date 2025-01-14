/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

/// <summary>
/// IFkItem defines the common interface for all Foreign key objects
/// These are normally used in UI select controls.  Choose a Name and the
/// GUID is the foreign key.
/// </summary>
public interface IFkItem
{
    public Guid Id { get; }
    public string Name { get; }
}

public abstract record FkItem
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "[NOT SET]";
}
