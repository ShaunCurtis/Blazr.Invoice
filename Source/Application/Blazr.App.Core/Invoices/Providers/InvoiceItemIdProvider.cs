/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class InvoiceItemIdProvider : IRecordIdProvider<DmoInvoiceItem, InvoiceItemId>
{
    public InvoiceItemId GetKey(object key)
    {
        return key switch
        {
            InvoiceItemId id => id,
            Guid guid => new InvoiceItemId(guid),
            _ => InvoiceItemId.Default
        };
    }

    public InvoiceItemId GetKey(DmoInvoiceItem record)
    {
        return record.Id;
    }

    public object GetValueObject(InvoiceItemId key)
    {
        return key.Value;
    }
}
