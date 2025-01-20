/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public static class InvoiceItemInfrastructureServices
{
    public static void AddInvoiceItemInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IRecordFactory<DmoInvoiceItem>, InvoiceItemRecordFactory>();

        services.AddScoped<IRecordIdProvider<DmoInvoiceItem, InvoiceItemId>, InvoiceItemIdProvider>();
    }
}
