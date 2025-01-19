/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class CustomerIdProvider : IRecordIdProvider<DmoCustomer, CustomerId>
{
    public CustomerId GetKey(object key)
    {
        if (key is Guid value)
            return new(value);

        throw new InvalidKeyProviderException("Object provided is not a CustomerId Value");
    }

    public CustomerId GetKey(DmoCustomer record)
    {
        return record.Id;
    }

    public object GetValueObject(CustomerId key)
    {
        return key.Value;
    }
}
