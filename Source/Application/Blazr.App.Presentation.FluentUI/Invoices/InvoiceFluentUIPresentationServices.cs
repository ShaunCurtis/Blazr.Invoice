/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation.FluentUI;

public static class InvoiceFluentUIPresentationServices
{
    public static void AddInvoiceFluentUIPresentationServices(this IServiceCollection services)
    {
        services.AddTransient<IFluentGridPresenter<DmoInvoice>, InvoiceFluentGridPresenter>();
        services.AddTransient<IReadPresenterFactory<DmoInvoice, InvoiceId>, InvoiceReadPresenterFactory>();

        services.AddTransient<InvoiceAggregatePresenterFactory>();
    }
}
