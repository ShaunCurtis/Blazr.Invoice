/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazored.Toast;
using Blazr.App.EntityFramework;
using Blazr.App.Blazor;
using Blazr.Diode.Mediator;
using Blazr.Gallium;
using System.Reflection;

namespace Blazr.App.Infrastructure.Server;

public static class BlazorServerAppServices
{
    public static void AddBlazorServerAppServices(this IServiceCollection services)
    {
        services.AddAppEntityFrameworkServices();
        services.AddAppBlazorServices();

        // Add Blazor Mediator Service
        services.AddMediator(new Assembly[] {
                typeof(Blazr.App.EntityFramework.AppEntityFrameworkServices).Assembly
        });

        // Add the Gallium Message Bus Server services
        services.AddScoped<IMessageBus, MessageBus>();

        // InMemory Scoped State Store 
        services.AddScoped<ScopedStateProvider>();

        // Add the QuickGrid Entity Framework Adapter
        services.AddQuickGridEntityFrameworkAdapter();
    }
}
