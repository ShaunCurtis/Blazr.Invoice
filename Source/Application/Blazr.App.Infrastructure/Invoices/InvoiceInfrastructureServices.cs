/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public static class InvoiceInfrastructureServices
{
    public static void AddInvoiceInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IEntityProvider<DmoInvoice, InvoiceId>, InvoiceEntityProvider>();
    }
}
