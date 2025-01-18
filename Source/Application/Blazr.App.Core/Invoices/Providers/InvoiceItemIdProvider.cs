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
        if (key is Guid value)
            return new(value);

        throw new InvalidKeyProviderException("Object provided is not a WeatherForecastId Value");
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
