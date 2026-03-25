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
public static class CustomerApiEndpoints
{
    internal static void AddCustomerApiEndpoints(this WebApplication app)
    {
        app.MapPost(AppDictionary.Customer.CustomerListApiUrl, GetListAsync);
        app.MapPost(AppDictionary.Customer.CustomerRecordApiUrl, GetRecordAsync);
        app.MapPost(AppDictionary.Customer.CustomerCommandApiUrl, ExecuteCommandAsync);
    }

    internal static async Task<IResult> GetListAsync(
        CustomerListRequest request,
        IMediatorBroker mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.DispatchAsync(request, cancellationToken);
        return Results.Ok(result);
    }

    internal static async Task<IResult> GetRecordAsync(
        CustomerRecordRequest request,
        IMediatorBroker mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.DispatchAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    internal static async Task<IResult> ExecuteCommandAsync(
        CustomerCommandRequest request,
        IMediatorBroker mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.DispatchAsync(request, cancellationToken);
        return Results.Ok(result);
    }
}
