/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

public record CustomerCommandHandler : IRequestHandler<CustomerCommandRequest, Result<CustomerId>>
{
    private ICommandHandler _handler;
    private IMessageBus _messageBus;

    public CustomerCommandHandler(ICommandHandler handler, IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _handler = handler;
    }

    public async Task<Result<CustomerId>> Handle(CustomerCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.ExecuteAsync<DboCustomer>(new CommandRequest<DboCustomer>(
            Item: DboCustomerMap.Map(request.Item),
            State: request.State,
            Cancellation: cancellationToken
        ));

        if (!result.HasSucceeded(out DboCustomer? record))
            return result.ConvertFail<CustomerId>();

        _messageBus.Publish<DmoCustomer>(DboCustomerMap.Map(record));

        return Result<CustomerId>.Success(new CustomerId(record.CustomerID));
    }
}
