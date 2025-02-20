﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation.FluentUI;

public static class CustomerFluentUIPresentationServices
{
    public static void AddCustomerFluentUIPresentationServices(this IServiceCollection services)
    {
        services.AddTransient<IFluentGridPresenter<DmoCustomer>, CustomerFluentGridPresenter>();
        services.AddTransient<IEditPresenter<CustomerEditContext, CustomerId>, EditPresenter<DmoCustomer, CustomerEditContext, CustomerId>>();
        services.AddTransient<IReadPresenter<DmoCustomer, CustomerId>, ReadPresenter<DmoCustomer, CustomerId>>();
    }
}
