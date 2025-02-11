/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.FluxGate;
using Blazr.Gallium;

namespace Blazr.App.Presentation.Bootstrap;

public class InvoiceGridPresenter : GridPresenter<DmoInvoice>
{
    public InvoiceGridPresenter(
        IMediator mediator, 
        IMessageBus messageBus, 
        KeyedFluxGateStore<GridState, Guid> keyedFluxGateStore)
        : base(mediator, messageBus, keyedFluxGateStore)
    { }

    protected override async Task<Result<ListResult<DmoInvoice>>> GetItemsAsync(GridState state)
    {
        // Get the list request from the Flux Context and get the result
        var listRequest = new InvoiceRequests.InvoiceListRequest()
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
