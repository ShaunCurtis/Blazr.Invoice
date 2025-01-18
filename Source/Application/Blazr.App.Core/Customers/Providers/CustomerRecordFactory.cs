/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public class CustomerRecordFactory : IRecordFactory<DmoCustomer>
{
    public DmoCustomer NewRecord()
    {
        return new DmoCustomer()
        {
            Id = CustomerId.Create,
        };
    }
    public DmoCustomer DefaultRecord()
    {
        return new DmoCustomer { Id = CustomerId.Default };
    }
}