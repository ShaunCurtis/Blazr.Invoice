namespace Blazr.App.Core;

public sealed partial class Invoice
{
    private bool _isDirty 
        => this.State != CommandState.None;

    public CommandState State { get; private set; } 
        = CommandState.None;

    public DmoInvoice InvoiceRecord { get; private set; }

    public IEnumerable<InvoiceItem> InvoiceItems
        => this.Items.AsEnumerable();

    public IEnumerable<InvoiceItem> InvoiceItemsBin
        => this.ItemsBin.AsEnumerable();

    public bool IsDirty
        => _isDirty ? true : this.Items.Any(item => item.IsDirty);

    public decimal TotalAmount
        => this.InvoiceRecord.TotalAmount;

    public event EventHandler<InvoiceId>? StateHasChanged;

    public static Invoice Default
        => new Invoice(new DmoInvoice(), Enumerable.Empty<DmoInvoiceItem>());

    private List<InvoiceItem> Items { get; set; }
        = new List<InvoiceItem>();

    private List<InvoiceItem> ItemsBin { get; set; }
        = new List<InvoiceItem>();

    public Invoice(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> items)
    {
        // We create new records for the Invoice and InvoiceItems
        InvoiceRecord = invoice with { };

        foreach (var item in items)
        {
            Items.Add(new InvoiceItem(item with { }, this.ItemUpdated));
        }
    }

    private void UpdateInvoice(DmoInvoice invoice)
    {
        this.InvoiceRecord = invoice;
        this.State = State.AsDirty;
    }

    private void ItemUpdated(InvoiceItem item)
    {
        this.Process();
    }

    private void Process()
    {
        decimal total = 0m;
        foreach (var item in Items)
            total += item.Amount;

        if (total != this.TotalAmount)
        {
            this.InvoiceRecord = this.InvoiceRecord with { TotalAmount = total };
            this.State = State.AsDirty;
        }
        this.StateHasChanged?.Invoke(this, this.InvoiceRecord.Id);
    }
}
