/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.Gallium;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace Blazr.App.Presentation;

public readonly record struct UpdateGridFilterRequest<TRecord>(Expression<Func<TRecord, bool>> Filter);

public readonly record struct UpdateGridPagingRequest<TRecord>(int StartIndex, int PageSize, Expression<Func<TRecord, object>>? Sorter) : IRequest<Result>
{
    public static UpdateGridPagingRequest<TRecord> Create(GridItemsProviderRequest<TRecord> request)
    {
        var column = request.SortByColumn as DataSetPropertyColumn<TRecord>;

        return new()
        {
            StartIndex = request.StartIndex,
            PageSize = request.Count ?? 0,
            Sorter = column?.SortExpression ?? null,
        };
    }
}

public sealed record UpdateGridPagingHandler : IRequestHandler<UpdateGridPagingRequest<TRecord>, Result>
{
    private readonly KeyedStateStore _store;

    public UpdateGridPagingHandler(KeyedStateStore keyedStateStore)
    {
        _store = keyedStateStore;
    }

    public Task<Result> Handle(UpdateGridPagingRequest request, CancellationToken cancellationToken)
    {
        var item = _store.GetState<GridState>(request.Key) ?? new();
        var updatedItem = item with { PageSize = request.PageSize, StartIndex = request.StartIndex, Sorter = request.Sorter };

        _store.Dispatch(request.Key, updatedItem);
        return Task.FromResult(Result.Success());
    }
}

public readonly record struct ResetGridRequest(Guid Key, int StartIndex, int PageSize, SortDefinition? Sorter, FilterDefinition? Filter) : IRequest<Result>;

public sealed record ResetGridHandler : IRequestHandler<ResetGridRequest, Result>
{
    private readonly KeyedStateStore _store;

    public ResetGridHandler(KeyedStateStore keyedStateStore)
    {
        _store = keyedStateStore;
    }

    public Task<Result> Handle(ResetGridRequest request, CancellationToken cancellationToken)
    {
        var item = _store.GetState<GridState>(request.Key) ?? new();
        var updatedItem = item with { PageSize = request.PageSize, StartIndex = request.StartIndex, Sorter = request.Sorter, Filter = request.Filter };

        _store.Dispatch(request.Key, updatedItem);
        return Task.FromResult(Result.Success());
    }
}
