/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.Gallium;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace Blazr.App.Presentation;

public readonly record struct UpdateGridFiltersRequest(Guid Key, FilterDefinition Filter) : IRequest<Result>;

public sealed record UpdateGridFiltersHandler : IRequestHandler<UpdateGridFiltersRequest, Result>
{
    private readonly KeyedStateStore _store;

    public UpdateGridFiltersHandler(KeyedStateStore keyedStateStore)
    {
        _store = keyedStateStore;
    }

    public Task<Result> Handle(UpdateGridFiltersRequest request, CancellationToken cancellationToken)
    {
        var item = _store.GetState<GridState>(request.Key) ?? new();
        var updatedItem = item with { Filter = request.Filter };

        _store.Dispatch(request.Key, updatedItem);

        return Task.FromResult(Result.Success());
    }
}

public readonly record struct UpdateGridPagingRequest(Guid Key, int StartIndex, int PageSize, SortDefinition? Sorter) : IRequest<Result>
{
    public static UpdateGridPagingRequest Create<TRecord>(Guid Key, GridItemsProviderRequest<TRecord> request)
    {
        List<SortDefinition> sortDefinitions = new();

        var column = request.SortByColumn as DataSetPropertyColumn<TRecord>;

        if (column is not null)
            sortDefinitions.Add(new(column.DataSetName, !request.SortByAscending));
        else
        {
            var definedSorters = request.GetSortByProperties();
            if (definedSorters is not null)
                sortDefinitions = definedSorters.Select(item => new SortDefinition(SortField: item.PropertyName, SortDescending: item.Direction == SortDirection.Descending)).ToList();
        }

        return new()
        {
            Key = Key,
            StartIndex = request.StartIndex,
            PageSize = request.Count ?? 0,
            Sorter = sortDefinitions.FirstOrDefault(),
        };
    }
}

public sealed record UpdateGridPagingHandler : IRequestHandler<UpdateGridPagingRequest, Result>
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
