/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

/// <summary>
/// This object should not be used in DI.
/// Create an instance through the Factory
/// </summary>
public class CustomerReadPresenter : ReadPresenter<DmoCustomer, CustomerId>
{
    public CustomerReadPresenter(IMediator dataBroker, IRecordFactory<DmoCustomer> newRecordProvider) : base(dataBroker, newRecordProvider)  { }

    protected override Task<Result<DmoCustomer>> GetItemAsync(CustomerId id)
    {
        return _dataBroker.Send(new CustomerItemRequest(id));
    }
}