/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

public record CustomerItemHandler : IRequestHandler<CustomerItemRequest, Result<DmoCustomer>>
{
    private IItemRequestHandler _handler;

    public CustomerItemHandler(IItemRequestHandler handler)
    {
        _handler = handler;
    }

    public async Task<Result<DmoCustomer>> Handle(CustomerItemRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<DboCustomer, bool>> findExpression = (item) =>
            item.CustomerID == request.Id.Value;

        var query = new ItemQueryRequest<DboCustomer>(findExpression);

        var result = await _handler.ExecuteAsync<DboCustomer>(query);

        if (!result.HasSucceeded(out DboCustomer? record))
            return result.ConvertFail<DmoCustomer>();

        var returnItem = DboCustomerMap.Map(record);

        return Result<DmoCustomer>.Success(returnItem);
    }
}
