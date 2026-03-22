/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Diode.Infrastructure.EntityFramework;

public static class DbContextExtensions
{
    extension (DbContext dbContext)
    {
        public async Task<Result<TRecord>> ExecuteCommandOnDatastoreAsync<TRecord>(CommandRequest<TRecord> request, CancellationToken cancellationToken = new())
            where TRecord : class
        {
            if ((request.Item is not ICommandEntity))
                return ResultT.Fail<TRecord>($"{request.Item.GetType().Name} Does not implement ICommandEntity and therefore you can't Update/Add/Delete it directly.");

            return request.State switch
            {
                RecordState.New => await AddAsync(dbContext, request, cancellationToken),
                RecordState.Deleted => await DeleteAsync(dbContext, request, cancellationToken),
                RecordState.Dirty => await UpdateAsync(dbContext, request, cancellationToken),
                _ => ResultT.Fail<TRecord>("Nothing executed.  Unrecognised State.")
            };
        }

        public async Task<Result<ListItemsProvider<TRecord>>> GetItemsFromDatastoreAsync<TRecord>(ListQueryRequest<TRecord> request)
            where TRecord : class
        {
            int totalRecordCount;

            // Turn off tracking.  We're only querying, no changes
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            // Get the IQueryable DbSet for TRecord
            IQueryable<TRecord> query = dbContext.Set<TRecord>();

            // If we have a filter defined, apply the predicate delegate to the IQueryable instance
            if (request.FilterExpression is not null)
                query = query.Where(request.FilterExpression).AsQueryable();

            // Get the total record count after applying the filters
            totalRecordCount = query is IAsyncEnumerable<TRecord>
                ? await query.CountAsync(request.Cancellation).ConfigureAwait(ConfigureAwaitOptions.None)
                : query.Count();

            // If we have a sorter, apply the sorter to the IQueryable instance
            if (request.SortExpression is not null)
            {
                query = request.SortDescending
                    ? query.OrderByDescending(request.SortExpression)
                    : query.OrderBy(request.SortExpression);
            }

            // Apply paging to the filtered and sorted IQueryable
            if (request.PageSize > 0)
                query = query
                    .Skip(request.StartIndex)
                    .Take(request.PageSize);

            // Finally materialize the list from the data source
            var list = query is IAsyncEnumerable<TRecord>
                ? await query.ToListAsync().ConfigureAwait(ConfigureAwaitOptions.None)
                : query.ToList();

            return ResultT.Read(new ListItemsProvider<TRecord>(list, totalRecordCount));
        }

        public async Task<Result<TRecord>> GetRecordFromDatastoreAsync<TRecord>(RecordQueryRequest<TRecord> request)
            where TRecord : class
        {
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var record = await dbContext.Set<TRecord>()
                .FirstOrDefaultAsync(request.FindExpression)
                .ConfigureAwait(ConfigureAwaitOptions.None);

            if (record is null)
                return ResultT.Fail<TRecord>($"No record retrieved with the Key provided");

            return ResultT.Read(record);
        }
    }

    private async static Task<Result<TRecord>> AddAsync<TRecord>(DbContext dbContext, CommandRequest<TRecord> request, CancellationToken cancellationToken)
         where TRecord : class
    {
        dbContext.Add<TRecord>(request.Item);
        var result = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        return result == 1
            ? ResultT.Read(request.Item)
            : ResultT.Fail<TRecord>("Error adding Record");

    }

    private async static Task<Result<TRecord>> DeleteAsync<TRecord>(DbContext dbContext, CommandRequest<TRecord> request, CancellationToken cancellationToken)
        where TRecord : class
    {
        dbContext.Remove<TRecord>(request.Item);
        var result = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        return result == 1
            ? ResultT.Read(request.Item)
            : ResultT.Fail<TRecord>("Error deleting Record");
    }

    private async static Task<Result<TRecord>> UpdateAsync<TRecord>(DbContext dbContext, CommandRequest<TRecord> request, CancellationToken cancellationToken)
        where TRecord : class
    {
        dbContext.Update<TRecord>(request.Item);
        var result = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        return result == 1
            ? ResultT.Read(request.Item)
            : ResultT.Fail<TRecord>("Error updating Record");
    }
}

