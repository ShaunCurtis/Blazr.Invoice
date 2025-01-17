/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

/// <summary>
/// This class implements the "standard" Server Command Handler
/// against an EF `TDbContext`
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public sealed class InvoiceCommandServerHandler<TDbContext>
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;

    public InvoiceCommandServerHandler(IDbContextFactory<TDbContext> factory)
    {
        _factory = factory;
    }

    public async ValueTask<Result> ExecuteAsync(SaveInvoiceRequest request, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommandAsync(request, cancellationToken);
    }

    private async ValueTask<Result> ExecuteCommandAsync(SaveInvoiceRequest request, CancellationToken cancellationToken)
    {
        using var dbContext = _factory.CreateDbContext();

        var invoiceRecord = DboInvoiceMap.Map(request.Invoice.InvoiceRecord);

        if (request.Invoice.State == CommandState.Update)
            dbContext.Update<DboInvoice>(invoiceRecord);

        if (request.Invoice.State == CommandState.Add)
            dbContext.Add<DboInvoice>(invoiceRecord);

        if (request.Invoice.State == CommandState.Delete)
            dbContext.Remove<DboInvoice>(invoiceRecord);

        foreach (var invoiceItem in request.Invoice.InvoiceItems)
        {
            var invoiceItemRecord = DboInvoiceItemMap.Map(invoiceItem.InvoiceItemRecord);

            if (invoiceItem.State == CommandState.Update)
                dbContext.Update<DboInvoiceItem>(invoiceItemRecord);

            if (invoiceItem.State == CommandState.Add)
                dbContext.Update<DboInvoiceItem>(invoiceItemRecord);
        }

        foreach (var invoiceItem in request.Invoice.InvoiceItemsBin)
        {
            if (invoiceItem.State != CommandState.Add)
            {
                var invoiceItemRecord = DboInvoiceItemMap.Map(invoiceItem.InvoiceItemRecord);

                dbContext.Remove<DboInvoiceItem>(invoiceItemRecord);
            }
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        return result > 0
            ? Result.Success()
            : Result.Fail(new CommandException("Error adding Invoice Composite to the data store."));
    }
}
