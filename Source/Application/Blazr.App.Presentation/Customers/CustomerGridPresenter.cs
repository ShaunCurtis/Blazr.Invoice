/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

public class CustomerGridPresenter : GridPresenter<DmoCustomer>
{
    public CustomerGridPresenter(
        IMediator mediator, 
        IMessageBus messageBus, 
        KeyedFluxGateStore<GridState, Guid> keyedFluxGateStore)
        : base(mediator, messageBus, keyedFluxGateStore)
    { }

    protected override async Task<Result<ListResult<DmoCustomer>>> GetItemsAsync(GridState state)
    {
        // Get the list request from the Flux Context and get the result
        var listRequest = new CustomerListRequest()
        {
            PageSize = state.PageSize,
            StartIndex = state.StartIndex,
            SortColumn = state.Sorter?.SortField,
            SortDescending = state.Sorter?.SortDescending ?? false
        };

        var result = await _dataBroker.Send(listRequest);

        return result;
    }
}
