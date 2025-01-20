/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.App.Presentation;
using Blazr.App.Presentation.FluentUI;
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.App.UI.FluentUI;

public static class ApplicationFluentUIServices
{
    public static void AddAppFluentUIServices(this IServiceCollection services)
    {
        services.AddScoped<IAppToastService, FluentUIToastService>();
    }
}