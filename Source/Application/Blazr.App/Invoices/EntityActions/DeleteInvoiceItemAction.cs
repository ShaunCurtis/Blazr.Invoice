/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public record DeleteInvoiceItemAction
{
    private readonly DmoInvoiceItem _invoiceItem;

    public DeleteInvoiceItemAction(DmoInvoiceItem invoiceItem)
        => _invoiceItem = invoiceItem;

    public Result<InvoiceEntity> ExecuteAction(InvoiceEntity entity)
        => entity
            // Get the invoice Item
            .GetInvoiceItem(_invoiceItem.Id)
            // Create a new immutable list with the item removed
            .Map(item => entity.InvoiceItems.Remove(item))
            // Create and return a new Entity with the new list
            .Map(items => InvoiceEntity.Mutate(entity.InvoiceRecord, items));

    public static DeleteInvoiceItemAction Create(DmoInvoiceItem invoiceItem)
        => new DeleteInvoiceItemAction(invoiceItem);
}
