/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using System.Collections.Immutable;

namespace Blazr.App.Core;

/// <summary>
/// Invoice Entity for the InvoiceMutor
/// </summary>
public sealed record InvoiceEntity : IEquatable<InvoiceEntity>
{
    public DmoInvoice InvoiceRecord { get; private init; }
    public ImmutableList<DmoInvoiceItem> InvoiceItems { get; private init; }

    private InvoiceEntity(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceInvoiceItems)
    {
        this.InvoiceRecord = invoice;
        this.InvoiceItems = invoiceInvoiceItems.ToImmutableList();
    }

    public bool IsDirty(InvoiceEntity control) => !this.Equals(control);

    public Result<DmoInvoiceItem> GetInvoiceItem(InvoiceItemId id)
        => ResultT.Read(
            value: this.InvoiceItems.SingleOrDefault(_item => _item.Id == id),
            exceptionMessage: $"The record with id {id} does not exist in the Invoice Items");

    public InvoiceEntity Mutate(DmoInvoice invoice)
        => InvoiceEntity.Load(invoice, this.InvoiceItems)
            .ApplyEntityRules();

    public InvoiceEntity Mutate(IEnumerable<DmoInvoiceItem> invoiceItems)
        => InvoiceEntity.Load(this.InvoiceRecord, invoiceItems)
            .ApplyEntityRules();

    public Result<InvoiceEntity> CheckEntityRules()
    => this.InvoiceItems.Sum(item => item.Amount.Value) == this.InvoiceRecord.TotalAmount.Value
        ? this.ToResult
        : ResultT.Fail<InvoiceEntity>("The Invoice Total Amount is incorrect.");

    public InvoiceEntity ApplyEntityRules()
        => InvoiceEntity.Load(
            invoice: this.InvoiceRecord with { TotalAmount = new(this.InvoiceItems.Sum(item => item.Amount.Value)) },
            invoiceItems: this.InvoiceItems);

    public bool Equals(InvoiceEntity? other)
        => other is not null
            && this.InvoiceItems.OrderBy(e => e.Id.Value).SequenceEqual(other.InvoiceItems.OrderBy(e => e.Id.Value))
            && this.InvoiceRecord.Equals(other.InvoiceRecord);

    public override int GetHashCode()
        => base.GetHashCode();

    public static InvoiceEntity Load(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceItems)
        => new InvoiceEntity(invoice, invoiceItems);

    public static Result<InvoiceEntity> TryLoad(DmoInvoice invoice, IEnumerable<DmoInvoiceItem> invoiceItems) =>
        InvoiceEntity.Load(invoice, invoiceItems)
            .CheckEntityRules();

    public static InvoiceEntity Create() =>
        InvoiceEntity.Load(DmoInvoice.CreateNew(), Enumerable.Empty<DmoInvoiceItem>());

    public static InvoiceEntity Create(DmoInvoice invoice) =>
        InvoiceEntity.Load(invoice, Enumerable.Empty<DmoInvoiceItem>());
}
