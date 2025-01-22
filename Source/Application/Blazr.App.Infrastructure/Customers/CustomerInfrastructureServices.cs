/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

public static class CustomerInfrastructureServices
{
    public static void AddCustomerInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IEntityProvider<DmoCustomer, CustomerId>, CustomerEntityProvider>();

        services.AddScoped<IRecordIdProvider<DmoCustomer, CustomerId>, CustomerIdProvider>();
        services.AddScoped<IRecordFactory<DmoCustomer>, CustomerRecordFactory>();
    }
}
