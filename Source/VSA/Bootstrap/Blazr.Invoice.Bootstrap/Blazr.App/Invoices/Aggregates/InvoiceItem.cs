/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public sealed class InvoiceItem : IDisposable
{
    private Action<InvoiceItem>? UpdateCallback;
    
    public CommandState State { get; internal set; } 
        = CommandState.None;

    public DmoInvoiceItem InvoiceItemRecord { get; private set; }
    
    public bool IsDirty 
        => this.State != CommandState.None;

    public InvoiceItem(DmoInvoiceItem item, Action<InvoiceItem> callback, bool isNew = false)
    {
        InvoiceItemRecord = item;
        this.UpdateCallback = callback;

        if (isNew || item.Id.IsDefault)
            this.State = CommandState.Add;
    }

    public InvoiceItemId Id => this.InvoiceItemRecord.Id;
    public string Description => this.InvoiceItemRecord.Description;
    public decimal Amount => this.InvoiceItemRecord.Amount;

    internal void UpdateInvoiceItem(DmoInvoiceItem invoiceItem)
    {
        this.InvoiceItemRecord = invoiceItem;
        this.State = this.State.AsDirty;
        UpdateCallback?.Invoke(this);
    }

    public void Dispose()
    {
        this.UpdateCallback = null;
    }
}
