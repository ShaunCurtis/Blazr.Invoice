namespace Blazr.App.Core;

public sealed class Invoice
{
    private bool _isDirty;
    public DmoInvoice InvoiceRecord { get; private set; }

    public DmoCustomer Customer { get; private set; }

    public IEnumerable<InvoiceItem> InvoiceItems 
        => this.Items.AsEnumerable();

    public bool IsDirty 
        => _isDirty ? true : this.Items.Any(item => item.IsDirty) ;
    
    public decimal TotalAmount 
        => this.InvoiceRecord.TotalAmount;

    internal List<InvoiceItem> Items { get; private set; } 
        = new List<InvoiceItem>();

    public Invoice(DmoInvoice invoice, DmoCustomer customer, IEnumerable<DmoInvoiceItem> items)
    {
        InvoiceRecord = invoice;
        Customer = customer;

        foreach (var item in items)
        {
            Items.Add(new InvoiceItem(item,this.ItemUpdated));
        }
    }

    internal void UpdateInvoice(DmoInvoice invoice)
    {
        this.InvoiceRecord = invoice;
        _isDirty = true;
    }

    private void ItemUpdated(InvoiceItem item)
    {
        this.Process();
    }

    internal void Process()
    {
        decimal total = 0m;
        foreach (var item in Items)
            total += item.Amount;

        if(total != this.TotalAmount)
        {
            this.InvoiceRecord = this.InvoiceRecord with { TotalAmount = total };
            _isDirty = true;
        }
    }
}
