/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public record UpdateInvoiceAction
{
    private readonly DmoInvoice _invoice;

    public UpdateInvoiceAction(DmoInvoice invoice)
        => _invoice = invoice;

    public Result<InvoiceEntity> ExecuteAction(InvoiceEntity entity)
        => InvoiceEntity.Load(_invoice, entity.InvoiceItems).ToResult;

    public static UpdateInvoiceAction Create(DmoInvoice invoice)
            => new UpdateInvoiceAction(invoice);
}

