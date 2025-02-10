﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

/// <summary>
/// Mediatr Handler to return a new Invoice 
/// </summary>
public record NewInvoiceHandler : IRequestHandler<InvoiceRequests.InvoiceNewRequest, Result<Invoice>>
{
    private IEntityProvider<DmoInvoice, InvoiceId> _entityProvider;

    public NewInvoiceHandler(IEntityProvider<DmoInvoice, InvoiceId> entityProvider)
    {
        _entityProvider = entityProvider;
    }

    public Task<Result<Invoice>> Handle(InvoiceRequests.InvoiceNewRequest request, CancellationToken cancellationToken)
    {
        var invoiceRecord = _entityProvider.NewRecord();

        var invoiceComposite = new Invoice(invoiceRecord, Enumerable.Empty<DmoInvoiceItem>());

        return Task.FromResult( Result<Invoice>.Success(invoiceComposite));
    }
}
