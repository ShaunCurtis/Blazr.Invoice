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
        return key switch
        {
            CustomerId id => id,
            Guid guid => new CustomerId(guid),
            _ => CustomerId.Default
        };
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
