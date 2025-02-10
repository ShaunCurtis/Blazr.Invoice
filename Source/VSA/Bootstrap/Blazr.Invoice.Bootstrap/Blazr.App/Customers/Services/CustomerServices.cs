/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.App.Presentation.Bootstrap;
using Blazr.App.Presentation;
using Blazr.App.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.App.Infrastructure;

public static class CustomerServices
{
    public static void AddCustomerServices(this IServiceCollection services)
    {
        services.AddScoped<IEntityProvider<DmoCustomer, CustomerId>, CustomerEntityProvider>();
        services.AddSingleton<IUIEntityService<DmoCustomer>, CustomerUIEntityService>();

        //services.AddScoped<IRecordIdProvider<DmoCustomer, CustomerId>, CustomerIdProvider>();
        //services.AddScoped<IRecordFactory<DmoCustomer>, CustomerRecordFactory>();

        services.AddTransient<IGridPresenter<DmoCustomer>, CustomerGridPresenter>();
        //services.AddTransient<IEditPresenterFactory<CustomerEditContext, CustomerId>, CustomerEditPresenterFactory>();
        //services.AddTransient<IReadPresenterFactory<DmoCustomer, CustomerId>, CustomerReadPresenterFactory>();
    }
}
