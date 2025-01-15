/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;
public sealed class InvoiceItem
{
    public DmoInvoiceItem InvoiceItemRecord { get; private set; }
    public bool IsDirty { get; private set; }
    public InvoiceItem(DmoInvoiceItem item)
    {
        InvoiceItemRecord = item; 
    }

    internal void UpdateInvoice(DmoInvoiceItem invoiceItem)
    {
        this.InvoiceItemRecord = invoiceItem;
        this.IsDirty = true;
    }

}
