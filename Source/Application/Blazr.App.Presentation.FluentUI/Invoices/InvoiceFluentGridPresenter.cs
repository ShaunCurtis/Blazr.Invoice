/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation.FluentUI;

public sealed class InvoiceFluentGridPresenter : IFluentGridPresenter<DmoInvoice>
{
    private readonly IMediator _mediator;
    
    public InvoiceFluentGridPresenter(
        IMediator mediator,
        IMessageBus messageBus,
        KeyedFluxGateStore<GridState, Guid> keyedFluxGateStore)
    {
        _mediator = mediator;
    }

    public async ValueTask<GridItemsProviderResult<DmoInvoice>> GetItemsAsync(GridItemsProviderRequest<DmoInvoice> request)
    {
        // Get the list request from the Flux Context and get the result
        var listRequest = new InvoiceRequests.InvoiceListRequest()
        {
            PageSize = request.Count ?? 10,
            StartIndex = request.StartIndex,
        };

        var result = await _mediator.Send(listRequest);

        if (!result.HasSucceeded(out ListResult<DmoInvoice> listResult))
            return GridItemsProviderResult.From(new List<DmoInvoice>(), 0);

        return GridItemsProviderResult.From(listResult.Items.ToList(), listResult.TotalCount);
    }
}
