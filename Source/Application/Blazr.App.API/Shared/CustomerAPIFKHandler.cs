/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.API;

public record CustomerAPIFKHandler : IRequestHandler<CustomerFKRequest, Result<List<FkoCustomer>>>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerAPIFKHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<List<FkoCustomer>>> HandleAsync(CustomerFKRequest request, CancellationToken cancellationToken)
    {
        using var http = _httpClientFactory.CreateClient(AppDictionary.Common.AppHttpClient);

        var httpResult = await http.PostAsJsonAsync<CustomerFKRequest>(AppDictionary.Shared.CustomerFkApiUrl, request, cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        if (!httpResult.IsSuccessStatusCode)
            return ResultT.Fail<List<FkoCustomer>>($"The server returned a status code of : {httpResult.StatusCode}");

        var listResult = await httpResult.Content.ReadFromJsonAsync<Result<List<FkoCustomer>>>()
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return listResult ?? ResultT.Fail<List<FkoCustomer>>($"The result was null");
    }
}
