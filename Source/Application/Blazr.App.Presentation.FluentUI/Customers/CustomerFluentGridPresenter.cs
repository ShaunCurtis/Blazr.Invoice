/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation.FluentUI;

public interface IFluentGridPresenter<TGridItem>
{
    public ValueTask<GridItemsProviderResult<TGridItem>> GetItemsAsync(GridItemsProviderRequest<TGridItem> request);
}

public class CustomerFluentGridPresenter : IFluentGridPresenter<DmoCustomer>
{
    IMediator _mediator;
    public CustomerFluentGridPresenter(
        IMediator mediator,
        IMessageBus messageBus,
        KeyedFluxGateStore<GridState, Guid> keyedFluxGateStore)
    {
        _mediator = mediator;
    }

    public async ValueTask<GridItemsProviderResult<DmoCustomer>> GetItemsAsync(GridItemsProviderRequest<DmoCustomer> request)
    {
        // Get the list request from the Flux Context and get the result
        var listRequest = new CustomerListRequest()
        {
            PageSize = request.Count ?? 10,
            StartIndex = request.StartIndex,
        };

        var result = await _mediator.Send(listRequest);

        if (!result.HasSucceeded(out ListResult<DmoCustomer> listResult))
            return GridItemsProviderResult.From(new List<DmoCustomer>(), 0);

        return GridItemsProviderResult.From(listResult.Items.ToList(), listResult.TotalCount);
    }
}
