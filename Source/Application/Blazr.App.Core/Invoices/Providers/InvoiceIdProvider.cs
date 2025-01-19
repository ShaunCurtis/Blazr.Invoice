/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class InvoiceIdProvider : IRecordIdProvider<DmoInvoice, InvoiceId>
{
    public InvoiceId GetKey(object key)
    {
        return key switch
        {
            InvoiceId id => id,
            Guid guid => new InvoiceId(guid),
            _ => InvoiceId.Default
        };
    }

    public InvoiceId GetKey(DmoInvoice record)
    {
        return record.Id;
    }

    public object GetValueObject(InvoiceId key)
    {
        return key.Value;
    }
}
