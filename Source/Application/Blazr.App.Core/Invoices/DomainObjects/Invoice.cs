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

    internal List<InvoiceItem> Items { get; private set; } 
        = new List<InvoiceItem>();

    public Invoice(DmoInvoice invoice, DmoCustomer customer, IEnumerable<DmoInvoiceItem> items)
    {
        InvoiceRecord = invoice;
        Customer = customer;

        foreach (var item in items)
        {
            Items.Add(new InvoiceItem(item));
        }
    }

    internal void UpdateInvoice(DmoInvoice invoice)
    {
        this.InvoiceRecord = invoice;
        _isDirty = true;
    }
}
