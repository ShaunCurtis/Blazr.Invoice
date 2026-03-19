/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core.Invoices;

/// <summary>
/// Invoice Entity methods used exclusively within the Core Domain
/// By the Actions
/// </summary>
public static class InvoiceEntityExtensions
{
    extension(InvoiceEntity @this)
    {
        public bool IsDirty(InvoiceEntity control) => !@this.Equals(control);

        public InvoiceEntity Map(Func<InvoiceEntity, InvoiceEntity> func)
            => func.Invoke(@this);

        public Result<DmoInvoiceItem> GetInvoiceItem(InvoiceItemId id)
            => ResultT.Read(
                value: @this.InvoiceItems.SingleOrDefault(_item => _item.Id == id),
                exceptionMessage: $"The record with id {id} does not exist in the Invoice Items");

        public InvoiceEntity Mutate(DmoInvoice invoice)
            => InvoiceEntity.Load(invoice, @this.InvoiceItems)
                .Map(ApplyEntityRules);

        public InvoiceEntity Mutate(IEnumerable<DmoInvoiceItem> invoiceItems)
            => InvoiceEntity.Load(@this.InvoiceRecord, invoiceItems)
                .Map(ApplyEntityRules);

        public Result<InvoiceEntity> CheckEntityRules()
        => @this.InvoiceItems.Sum(item => item.Amount.Value) == @this.InvoiceRecord.TotalAmount.Value
            ? ResultT.Read(@this)
            : ResultT.Fail<InvoiceEntity>("The Invoice Total Amount is incorrect.");

        public InvoiceEntity ApplyEntityRules()
            => InvoiceEntity.Load(
                invoice: @this.InvoiceRecord with { TotalAmount = new(@this.InvoiceItems.Sum(item => item.Amount.Value)) },
                invoiceItems: @this.InvoiceItems);
    }
}
