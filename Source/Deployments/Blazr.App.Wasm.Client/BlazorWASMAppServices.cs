/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazored.Toast;
using Blazr.App.Blazor;
using Blazr.Diode.Mediator;
using Blazr.Gallium;
using System.Reflection;

namespace Blazr.App.Wasm.Client;

public static class BlazorWASMAppServices
{
    public static void AddBlazorWasmAppServices(this IServiceCollection services)
    {
        services.AddAppBlazorServices();

        // Add Blazor Mediator Service
        var assemblies = new Assembly[] { typeof(Blazr.App.API.CustomerAPIListHandler).Assembly };
        services.AddMediator(assemblies);

        // Add the Gallium Message Bus Server services
        services.AddScoped<IMessageBus, MessageBus>();

        // InMemory Scoped State Store 
        services.AddScoped<ScopedStateProvider>();

        // Add the Blazored Toast services
        services.AddBlazoredToast();

        // Add the QuickGrid Entity Framework Adapter
        //services.AddQuickGridEntityFrameworkAdapter();
    }
}
