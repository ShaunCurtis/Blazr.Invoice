/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class InvoiceItemRecordFactory : IRecordFactory<DmoInvoiceItem>
{
    public InvoiceId InvoiceId { get; private set; } = new(Guid.Empty);

    public void SetInvoiceContext(InvoiceId invoiceId)
        => this.InvoiceId = invoiceId;

    public DmoInvoiceItem NewRecord()
    {
        return new DmoInvoiceItem() 
        {
            Id = new InvoiceItemId(Guid.CreateVersion7()),
            InvoiceId = this.InvoiceId 
        };
    }

    public DmoInvoiceItem DefaultRecord()
    {
        return new DmoInvoiceItem
        {
            Id = InvoiceItemId.Default
        };
    }
}