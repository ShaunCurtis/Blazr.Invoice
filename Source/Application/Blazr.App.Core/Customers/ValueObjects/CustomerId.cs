﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

//[DebuggerDisplay("CustomerId : Value = {Value}")]
public readonly record struct CustomerId(Guid Value) : IEntityId
{
    public bool IsDefault => this == Default;
    public static CustomerId Create => new(Guid.CreateVersion7());
    public static CustomerId Default => new(Guid.Empty);

    public override string ToString()
    {
        return Value.ToString();
    }
}
