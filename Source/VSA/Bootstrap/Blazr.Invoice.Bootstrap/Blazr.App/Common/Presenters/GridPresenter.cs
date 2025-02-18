/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Azure.Core;
using Blazr.Gallium;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.VisualBasic;
using System;

namespace Blazr.App.Presentation;

public abstract class GridPresenter<TRecord>
    : IGridPresenter<TRecord>, IDisposable
    where TRecord : class, new()
{
    // Services
    protected readonly IMessageBus _messageBus;
    private readonly KeyedStateStore _gridStateStore;

    public Guid ContextUid { get; private set; } = Guid.NewGuid();
    public GridState GridState { get; private set; } = new();

    public readonly Guid ComponentInstanceId = Guid.NewGuid();

    public IDataResult LastResult { get; protected set; } = DataResult.Failure("New");

    public event EventHandler<EventArgs>? StateChanged;

    public GridPresenter(IMediator mediator, IMessageBus messageBus, KeyedStateStore keyedFluxGateStore)
    {
        _dataBroker = mediator;
        _messageBus = messageBus;
        _gridStateStore = keyedFluxGateStore;

        _messageBus.Subscribe<TRecord>(this.OnStateChanged);
    }

    public void SetContext(Guid context)
    {
        this.ContextUid = context;
        if (_gridStateStore.TryGetState<GridState<TRecord>>(context, out GridState<TRecord>? state))
        this.GridState = state;

        this.GridState = new GridState<TRecord>();
    }

    public IDataResult DispatchGridStateChange(UpdateGridFilterRequest<TRecord> request)
    {
        var item = _gridStateStore.GetState<GridState<TRecord>>(this.ContextUid) ?? new();
        var updatedItem = item with { Filter= request.Filter };

        _gridStateStore.Dispatch(this.ContextUid, updatedItem);

        return DataResult.Success() as;
    }

    public async Task<IDataResult> DispatchGridStateChange(UpdateGridPagingRequest<TRecord> request)
    {
        var item = _store.GetState<GridState>(request.Key) ?? new();
        var updatedItem = item with { PageSize = request.PageSize, StartIndex = request.StartIndex, Sorter = request.Sorter };

        _store.Dispatch(request.Key, updatedItem);
        return Task.FromResult(Result.Success());

    }
    public async Task<IDataResult> DispatchGridStateChange(ResetGridRequest request)
    {
        var result = await _dataBroker.Send(request);

        return result.ToDataResult;
    }

    protected abstract Task<Result<ListResult<TRecord>>> GetItemsAsync(GridState state);

    public async ValueTask<GridItemsProviderResult<TRecord>> GetItemsAsync()
    {
        var result = await this.GetItemsAsync(this.GridState);
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
