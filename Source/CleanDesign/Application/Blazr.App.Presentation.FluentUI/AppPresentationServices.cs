﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Presentation.FluentUI;

public static class AppFluentUIPresentationServices
{
    public static void AddAppFluentUIPresentationServices(this IServiceCollection services)
    {
        // GridState inMemory Store 
        services.AddScoped<KeyedFluxGateStore<GridState, Guid>>();
        services.AddTransient<FluxGateDispatcher<GridState>, GridStateDispatcher>();

        services.AddScoped<ILookupPresenterFactory, LookupPresenterFactory>();
        services.AddScoped<IEditPresenterFactory, EditPresenterFactory>();
        services.AddTransient<IReadPresenterFactory, ReadPresenterFactory>();

        services.AddQuickGridEntityFrameworkAdapter();

        services.AddCustomerFluentUIPresentationServices();
        services.AddInvoiceFluentUIPresentationServices();
    }
}
