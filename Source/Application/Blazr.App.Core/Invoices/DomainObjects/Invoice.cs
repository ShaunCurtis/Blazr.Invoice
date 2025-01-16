﻿namespace Blazr.App.Core;

public sealed partial class Invoice
{
    private bool _isDirty => this.State != CommandState.None;

    public CommandState State { get; private set; } = CommandState.None;

    public DmoInvoice InvoiceRecord { get; private set; }

    public IEnumerable<InvoiceItem> InvoiceItems
        => this.Items.AsEnumerable();

    public bool IsDirty
        => _isDirty ? true : this.Items.Any(item => item.IsDirty);

    public decimal TotalAmount
        => this.InvoiceRecord.TotalAmount;

    private List<InvoiceItem> Items { get; set; }
        = new List<InvoiceItem>();

    private List<InvoiceItem> ItemsBin { get; set; }
    = new List<InvoiceItem>();

    public Invoice(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> items)
    {
        InvoiceRecord = invoice;

        foreach (var item in items)
        {
            Items.Add(new InvoiceItem(item, this.ItemUpdated));
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
    }
}
