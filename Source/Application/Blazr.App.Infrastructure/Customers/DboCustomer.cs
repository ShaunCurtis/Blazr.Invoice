﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public sealed record DboCustomer : ICommandEntity
{
    [Key] public Guid CustomerID { get; init; }
    public string CustomerName { get; init; } = string.Empty;
}
