/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public static class InvoiceEntityActionExtensions
{
    extension(InvoiceEntity @this)
    {
        public Result<DmoInvoiceItem> GetInvoiceItem(InvoiceItemId id)
            => ResultT.Read(
                value: @this.InvoiceItems.SingleOrDefault(_item => _item.Id == id),
                exceptionMessage: $"The record with id {id} does not exist in the Invoice Items");

        public InvoiceEntity ApplyEntityRules()
            => InvoiceEntity.Load(
                invoice: @this.InvoiceRecord with { TotalAmount = new(@this.InvoiceItems.Sum(item => item.Amount.Value)) },
                invoiceItems: @this.InvoiceItems);
    }
}
