/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.App.Presentation;

//public sealed class InvoiceEditPresenterFactory
//{
//    private IServiceProvider _serviceProvider;
//    public InvoiceEditPresenterFactory(IServiceProvider serviceProvider)
//    {
//        _serviceProvider = serviceProvider;
//    }

//    public InvoiceEditPresenter GetPresenter(Invoice Invoice)
//    {
//        var presenter = ActivatorUtilities.CreateInstance<InvoiceEditPresenter>(_serviceProvider, new[] { Invoice });
//        ArgumentNullException.ThrowIfNull(presenter, nameof(presenter));
//        return presenter;
//    }
//}

public sealed class InvoiceItemEditPresenterFactory
{
    private IServiceProvider _serviceProvider;
    public InvoiceItemEditPresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public InvoiceItemEditPresenter GetPresenter(Invoice invoice, InvoiceItemId invoiceItemId)
    {
        var presenter = ActivatorUtilities.CreateInstance<InvoiceItemEditPresenter>(_serviceProvider, new object[] {invoice , invoiceItemId });
        ArgumentNullException.ThrowIfNull(presenter, nameof(presenter));
        return presenter;
    }
}


