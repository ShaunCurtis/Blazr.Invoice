/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public record NewInvoiceHandler : IRequestHandler<NewInvoiceRequest, Result<Invoice>>
{
    private INewRecordProvider<DmoInvoice> _recordProvider;

    public NewInvoiceHandler(INewRecordProvider<DmoInvoice> newRecordProvider)
    {
         _recordProvider = newRecordProvider;
    }

    public Task<Result<Invoice>> Handle(NewInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoiceRecord = _recordProvider.NewRecord();

        var invoiceComposite = new Invoice(invoiceRecord, Enumerable.Empty<DmoInvoiceItem>());

        return Task.FromResult( Result<Invoice>.Success(invoiceComposite));
    }
}
