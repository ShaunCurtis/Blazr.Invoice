/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;
public sealed class InvoiceItem : IDisposable
{
    private Action<InvoiceItem>? UpdateCallback;

    public DmoInvoiceItem InvoiceItemRecord { get; private set; }
    public bool IsDirty { get; private set; }

    public InvoiceItem(DmoInvoiceItem item, Action<InvoiceItem> callback)
    {
        InvoiceItemRecord = item;
        this.UpdateCallback = callback;
    }

    public decimal Amount => this.InvoiceItemRecord.Amount;

    internal void UpdateInvoice(DmoInvoiceItem invoiceItem)
    {
        this.InvoiceItemRecord = invoiceItem;
        this.IsDirty = true;
        UpdateCallback?.Invoke(this);
    }

    public void Dispose()
    {
        this.UpdateCallback = null;
    }

}
