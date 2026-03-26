/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.API;

/// <summary>
/// Mediator Handler for executing record list requests to get a Customer Entity in an API Context
/// </summary>
public sealed class CustomerAPIListHandler : IRequestHandler<CustomerListRequest, Result<ListItemsProvider<DmoCustomer>>>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerAPIListHandler(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<Result<ListItemsProvider<DmoCustomer>>> HandleAsync(CustomerListRequest request, CancellationToken cancellationToken)
    {
        using var http = _httpClientFactory.CreateClient(AppDictionary.Common.AppHttpClient);

        var httpResult = await http.PostAsJsonAsync<CustomerListRequest>(AppDictionary.Customer.CustomerListApiUrl, request, cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        if (!httpResult.IsSuccessStatusCode)
            return ResultT.Fail<ListItemsProvider<DmoCustomer>>($"The server returned a status code of : {httpResult.StatusCode}");

        var listResult = await httpResult.Content.ReadFromJsonAsync<Result<ListItemsProvider<DmoCustomer>>>()
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return listResult ?? ResultT.Fail<ListItemsProvider<DmoCustomer>>($"The result was null");
    }
}
