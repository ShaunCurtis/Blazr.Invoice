/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.App.Core.Invoices;

namespace Blazr.App.Presentation;

public sealed class InvoiceItemRecordMutor : RecordMutor<DmoInvoiceItem> ,IRecordMutor<DmoInvoiceItem>
{
    [TrackState] public string Description { get; set; } = string.Empty;
    [TrackState] public decimal Amount { get; set; }

    private InvoiceItemRecordMutor(DmoInvoiceItem record)
    {
        this.BaseRecord = record;
        this.SetFields();
    }

    private void SetFields()
    {
        this.Description = this.BaseRecord.Description.Value;
        this.Amount = this.BaseRecord.Amount.Value;
    }

    public override DmoInvoiceItem Record => this.BaseRecord with
    {
        Description = new(this.Description),
        Amount = new(this.Amount)
    };

    public override void Reset()
        => this.SetFields();

    public Func<InvoiceEntity, Result<InvoiceEntity>> Dispatcher =>
        entity => this.IsDirty
            ? SaveInvoiceItemAction.Create(this.Record).ExecuteAction(entity)
            : ResultT.Read(entity);

    public override bool IsNew => BaseRecord.Id.IsNew;

    public static InvoiceItemRecordMutor Load(DmoInvoiceItem record)
        => new InvoiceItemRecordMutor(record);

    public static InvoiceItemRecordMutor Load(InvoiceEntity entity, InvoiceItemId id)
        => entity.GetInvoiceItem(id)
            .Map(value => InvoiceItemRecordMutor.Load(value))
            .Write(failureValue: InvoiceItemRecordMutor.NewMutor(entity.InvoiceRecord.Id));

    public static InvoiceItemRecordMutor NewMutor(InvoiceId invoiceId)
        => new InvoiceItemRecordMutor(DmoInvoiceItem.CreateNew(invoiceId));
}
