/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.FluxGate;
using Blazr.Gallium;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace Blazr.App.Presentation;

public abstract class GridPresenter<TRecord>
    : IGridPresenter<TRecord>, IDisposable
    where TRecord : class, new()
{
    // Services
    protected readonly IMediator _dataBroker;
    protected readonly IMessageBus _messageBus;
    private readonly KeyedFluxGateStore<GridState, Guid> _gridStateStores;

    // State Management
    protected FluxGateStore<GridState> _gridStateStore;
    public Guid ContextUid { get; private set; } = Guid.NewGuid();
    public GridState GridState => _gridStateStore.Item;

    public readonly Guid ComponentInstanceId = Guid.NewGuid();

    public IDataResult LastResult { get; protected set; } = DataResult.Failure("New");

    public event EventHandler<EventArgs>? StateChanged;

    public GridPresenter(IMediator mediator, IMessageBus messageBus, KeyedFluxGateStore<GridState, Guid> keyedFluxGateStore)
    {
        _dataBroker = mediator;
        _messageBus = messageBus;
        _gridStateStores = keyedFluxGateStore;

        _gridStateStore = _gridStateStores.GetOrCreateStore(ContextUid);

        _messageBus.Subscribe<TRecord>(this.OnStateChanged);
    }

    public void SetContext(Guid context)
    {
        _gridStateStores.RemoveStore(ContextUid);

        ContextUid = context;

        _gridStateStore = _gridStateStores.GetOrCreateStore(ContextUid);
    }

    public IDataResult DispatchGridStateChange(IFluxGateAction action)
    {
        return _gridStateStore.Dispatch(action).ToDataResult();
    }

    protected abstract Task<Result<ListResult<TRecord>>> GetItemsAsync(GridState state);

    public async ValueTask<GridItemsProviderResult<TRecord>> GetItemsAsync()
    {
        var result = await this.GetItemsAsync(_gridStateStore.Item);
        this.LastResult = result.ToDataResult;

        if (!result.HasSucceeded(out ListResult<TRecord> listResult))
            return GridItemsProviderResult.From<TRecord>(new List<TRecord>(), 0);

        // return a new GridItemsProviderResult created from the ListQueryResult
        return GridItemsProviderResult.From<TRecord>(listResult.Items.ToList(), listResult.TotalCount); ;
    }

    public void OnStateChanged(object? message)
    {
        this.StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _messageBus.UnSubscribe<TRecord>(this.OnStateChanged);
    }
}
