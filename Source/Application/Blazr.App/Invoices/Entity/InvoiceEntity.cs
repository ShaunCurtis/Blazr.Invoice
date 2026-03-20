/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Collections.Immutable;

namespace Blazr.App.Core;

public sealed record InvoiceEntity : IEquatable<InvoiceEntity>
{
    public DmoInvoice InvoiceRecord { get; private init; }
    public ImmutableList<DmoInvoiceItem> InvoiceItems { get; private init; }

    public bool IsValid =>
        this.InvoiceItems.Sum(item => item.Amount.Value) == this.InvoiceRecord.TotalAmount.Value;

    private InvoiceEntity(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceInvoiceItems)
    {
        this.InvoiceRecord = invoice;
        this.InvoiceItems = invoiceInvoiceItems.ToImmutableList();
    }

    public bool Equals(InvoiceEntity? other)
        => other is not null
            && this.InvoiceItems.OrderBy(e => e.Id.Value).SequenceEqual(other.InvoiceItems.OrderBy(e => e.Id.Value))
            && this.InvoiceRecord.Equals(other.InvoiceRecord);

    public override int GetHashCode()
        => base.GetHashCode();

    public static InvoiceEntity Load(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceItems)
        => new InvoiceEntity(invoice, invoiceItems);

    public static InvoiceEntity Create() =>
        new InvoiceEntity(DmoInvoice.CreateNew(), Enumerable.Empty<DmoInvoiceItem>());

    public static InvoiceEntity Create(DmoInvoice invoice) =>
        new InvoiceEntity(invoice, Enumerable.Empty<DmoInvoiceItem>());
}
