/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation;

public class InvoiceReadPresenterFactory : IReadPresenterFactory<DmoInvoice, InvoiceId>
{
    private IServiceProvider _serviceProvider;
    public InvoiceReadPresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async ValueTask<IReadPresenter<DmoInvoice, InvoiceId>> GetPresenterAsync(InvoiceId id)
    {
        var presenter = ActivatorUtilities.CreateInstance<InvoiceReadPresenter>(_serviceProvider);
        ArgumentNullException.ThrowIfNull(presenter, nameof(presenter));
        await presenter.LoadAsync(id);

        return presenter;
    }
}

public class InvoiceEditPresenterFactory
{
    private IServiceProvider _serviceProvider;
    public InvoiceEditPresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public InvoiceEditPresenter GetPresenter(Invoice Invoice)
    {
        var presenter = ActivatorUtilities.CreateInstance<InvoiceEditPresenter>(_serviceProvider, new[] { Invoice });
        ArgumentNullException.ThrowIfNull(presenter, nameof(presenter));
        return presenter;
    }
}

