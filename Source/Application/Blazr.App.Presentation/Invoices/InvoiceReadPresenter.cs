/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

/// <summary>
/// This object should not be used in DI.
/// Create an instance through the Factory
/// </summary>
public class InvoiceReadPresenter : ReadPresenter<DmoInvoice, InvoiceId>
{
    public InvoiceReadPresenter(IMediator dataBroker, IRecordFactory<DmoInvoice> newRecordProvider) : base(dataBroker, newRecordProvider)  { }

    protected override Task<Result<DmoInvoice>> GetItemAsync(InvoiceId id)
    {
        return _dataBroker.Send(new InvoiceRequests.InvoiceRecordRequest(id));
    }
}