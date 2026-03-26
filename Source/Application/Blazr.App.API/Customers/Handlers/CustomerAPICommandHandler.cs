/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.API;

/// <summary>
/// Mediator Handler for executing record commands against a Customer Entity in an API Context
/// </summary>
public sealed class CustomerAPICommandHandler : IRequestHandler<CustomerCommandRequest, Result<CustomerId>>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerAPICommandHandler(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<Result<CustomerId>> HandleAsync(CustomerCommandRequest request, CancellationToken cancellationToken)
    {
        using var http = _httpClientFactory.CreateClient(AppDictionary.Common.AppHttpClient);

        var httpResult = await http.PostAsJsonAsync<CustomerCommandRequest>(AppDictionary.Customer.CustomerCommandApiUrl, request, cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        if (!httpResult.IsSuccessStatusCode)
            return ResultT.Fail<CustomerId>($"The server returned a status code of : {httpResult.StatusCode}");

        var listResult = await httpResult.Content.ReadFromJsonAsync<Result<CustomerId>>()
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return listResult ?? ResultT.Fail<CustomerId>($"The result was null");
    }
}
