/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.App.Presentation;

public sealed class InvoiceAggregatePresenterFactory
{
    private readonly IServiceProvider _serviceProvider;

    public InvoiceAggregatePresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<InvoiceAggregatePresenter> CreateAsync(InvoiceId invoiceId)
    {
        var presenter = ActivatorUtilities.CreateInstance<InvoiceAggregatePresenter>(_serviceProvider);

        ArgumentNullException.ThrowIfNull(presenter);

        await presenter.LoadAsync(invoiceId);

        return presenter!;
    }
}


public sealed class InvoiceAggregatePresenter
{
    private readonly IMediator _dataBroker;

    public IDataResult LastResult { get; private set; } = DataResult.Success();

    public Invoice Invoice { get; private set; }

    public IQueryable<DmoInvoiceItem> InvoiceItems => this.Invoice.InvoiceItems.Select(item => item.InvoiceItemRecord).AsQueryable();

    public InvoiceAggregatePresenter(IMediator dataBroker)
    {
        _dataBroker = dataBroker;

        // Get a default Invoice
        this.Invoice = new(new DmoInvoice(), Enumerable.Empty<DmoInvoiceItem>());
    }

    internal async Task LoadAsync(InvoiceId id)
    {
        this.LastResult = DataResult.Success();

        // if we have an empty guid them we go with the new context created in the constructor
        if (id.Value != Guid.Empty)
        {
            var request = new InvoiceRequests.InvoiceRequest(id);
            var result = await _dataBroker.Send(request);

            LastResult = result.ToDataResult;

            if (result.HasSucceeded(out Invoice? invoice))
                this.Invoice = invoice!;

            return;
        }
    }

    public async ValueTask<Result> SaveAsync()
    {
        var result = await _dataBroker.Send(new InvoiceRequests.InvoiceSaveRequest(this.Invoice));

        LastResult = result.ToDataResult;

        if (result.IsFailure)
            return result;

        return this.Invoice.Dispatch(new InvoiceActions.SetAsPersistedAction());
    }

    public Result FakePersistenceToAllowExit()
    {
        return this.Invoice.Dispatch(new InvoiceActions.SetAsPersistedAction());
    }
}
