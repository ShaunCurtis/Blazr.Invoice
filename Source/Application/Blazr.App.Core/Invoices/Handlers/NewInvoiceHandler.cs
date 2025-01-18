/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public record NewInvoiceHandler : IRequestHandler<NewInvoiceRequest, Result<Invoice>>
{
    private IRecordFactory<DmoInvoice> _recordFactory;

    public NewInvoiceHandler(IRecordFactory<DmoInvoice> newRecordProvider)
    {
         _recordFactory = newRecordProvider;
    }

    public Task<Result<Invoice>> Handle(NewInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoiceRecord = _recordFactory.NewRecord();

        var invoiceComposite = new Invoice(invoiceRecord, Enumerable.Empty<DmoInvoiceItem>());

        return Task.FromResult( Result<Invoice>.Success(invoiceComposite));
    }
}
