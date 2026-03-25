/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.Diode.Mediator;
using Blazr.Manganese;

namespace Blazr.App.EntityFramework;

/// <summary>
/// Mediator Handler for executing record requests to get a Customer Entity in an API Context
/// </summary>
public sealed class CustomerAPIRecordHandler : IRequestHandler<CustomerRecordRequest, Result<DmoCustomer>>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerAPIRecordHandler(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;

    public async Task<Result<DmoCustomer>> HandleAsync(CustomerRecordRequest request, CancellationToken cancellationToken)
    {
        using var http = _httpClientFactory.CreateClient(AppDictionary.Common.AppHttpClient);

        var httpResult = await http.PostAsJsonAsync<CustomerRecordRequest>(AppDictionary.Customer.CustomerRecordApiUrl, request, cancellationToken)
            .ConfigureAwait(ConfigureAwaitOptions.None);

        if (!httpResult.IsSuccessStatusCode)
            return ResultT.Fail<DmoCustomer>($"The server returned a status code of : {httpResult.StatusCode}");

        var listResult = await httpResult.Content.ReadFromJsonAsync<Result<DmoCustomer>>()
            .ConfigureAwait(ConfigureAwaitOptions.None);

        return listResult ?? ResultT.Fail<DmoCustomer>($"The result was null");
    }
}
