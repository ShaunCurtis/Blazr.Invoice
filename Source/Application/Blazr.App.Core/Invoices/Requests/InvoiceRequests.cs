/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

public readonly record struct SaveInvoiceRequest(Invoice Invoice)
    : IRequest<Result>;

public readonly record struct NewInvoiceRequest()
    : IRequest<Result<Invoice>>;

public readonly record struct GetInvoiceRequest(InvoiceId Id)
    : IRequest<Result<Invoice>>;
