/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.Diode.Mediator;

namespace Blazr.App.Server.API;

/// <summary>
/// API Endpoints
/// </summary>
public static class FKApiEndpoints
{
    internal static void AddFKApiEndpoints(this WebApplication app)
    {
        app.MapPost(AppDictionary.Shared.CustomerFkApiUrl, GetCustomerFKListAsync);
    }

    internal static async Task<IResult> GetCustomerFKListAsync(
        CustomerFKRequest request,
        IMediatorBroker mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.DispatchAsync(request, cancellationToken);
        return Results.Ok(result);
    }
}
