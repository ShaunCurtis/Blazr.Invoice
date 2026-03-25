/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.App.EntityFramework;
using Blazr.Diode.Mediator;
using Blazr.Gallium;
using System.Reflection;

namespace Blazr.App.Wasm.Server;

public static class AppServices
{
    public static void AddBlazorWASMServerAppServices(this IServiceCollection services)
    {
        services.AddAppEFServices();

        // Add Blazor Mediator Service
        services.AddMediator(new Assembly[] {
                typeof(Blazr.App.EntityFramework.AppEFServerServices).Assembly
        });

        // Add the Gallium Message Bus Server services
        //services.AddScoped<IMessageBus, MessageBus>();

        // InMemory Scoped State Store 
        //services.AddScoped<ScopedStateProvider>();

        // Add the QuickGrid Entity Framework Adapter
        //services.AddQuickGridEntityFrameworkAdapter();
    }
}
