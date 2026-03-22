/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public record SaveInvoiceItemAction
{
    private readonly DmoInvoiceItem _invoiceItem;

    public SaveInvoiceItemAction(DmoInvoiceItem invoiceItem)
        => _invoiceItem = invoiceItem;

    public Result<InvoiceEntity> ExecuteAction(InvoiceEntity entity)
        => entity.InvoiceItems.Any(_item => _invoiceItem.Id.Equals(_item.Id))
            // if the item exists, update it
            ? Update(entity, _invoiceItem)
            // otherwise add it
            : Add(entity, _invoiceItem);

    private static Result<InvoiceEntity> Add(InvoiceEntity entity, DmoInvoiceItem invoiceItem)
        => entity.InvoiceItems.Add(invoiceItem)
        .ToResult
        .Map(items => InvoiceEntity.Mutate(entity.InvoiceRecord, items));

    private static Result<InvoiceEntity> Update(InvoiceEntity entity, DmoInvoiceItem invoiceItem)
        => entity.GetInvoiceItem(invoiceItem.Id)
            .Map(item => entity.InvoiceItems.Replace(item, invoiceItem))
            .Map(items => InvoiceEntity.Mutate(entity.InvoiceRecord, items));

    public static SaveInvoiceItemAction Create(DmoInvoiceItem invoiceItem)
        => new SaveInvoiceItemAction(invoiceItem);
}
