/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Server.API;

public static class AppAPIServices
{
    public static void AddAppAPIEndpoints(this WebApplication app)
    {
        app.AddCustomerApiEndpoints();
    }
}
