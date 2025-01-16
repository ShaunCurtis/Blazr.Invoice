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
public class CustomerEditPresenter : EditPresenter<DmoCustomer, CustomerEditContext, CustomerId>
{
    public CustomerEditPresenter(IMediator mediator, IRecordIdProvider<DmoCustomer, CustomerId> keyProvider, INewRecordProvider<DmoCustomer> newRecordProvider)
        : base(mediator, keyProvider, newRecordProvider) { }

    protected override Task<Result<DmoCustomer>> GetItemAsync()
    {
        return this.Databroker.Send(new CustomerItemRequest(this.EntityId));
    }

    protected override Task<Result<CustomerId>> UpdateAsync(DmoCustomer record, CommandState state)
    {
        return Databroker.Send(new CustomerCommandRequest(record, state));
    }
}

