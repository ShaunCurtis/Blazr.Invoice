/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

/// <summary>
/// Mediatr Server Handler that saves an Invoice Aggregate
/// It uses the custom Invoice Database Handler to interface with the database
/// </summary>
public record PersistInvoiceServerHandler : IRequestHandler<SaveInvoiceRequest, Result>
{
    private readonly IMessageBus _messageBus;
    private readonly ICommandHandler<Invoice> _commandHandler;

    public PersistInvoiceServerHandler(ICommandHandler<Invoice> commandHandler, IMessageBus messageBus)
    {
        _commandHandler = commandHandler;
        _messageBus = messageBus;
    }

    public async Task<Result> Handle(SaveInvoiceRequest request, CancellationToken cancellationToken)
    {
        var invoice = request.Invoice;
        
        var result = await _commandHandler.ExecuteAsync(new CommandRequest<Invoice>(
            Item: invoice,
            State: CommandState.None,
            Cancellation: cancellationToken));

        if (result.HasFailed(out Exception? exception))
            return Result.Fail(exception!);

        _messageBus.Publish<DmoInvoice>(invoice.InvoiceRecord);

        return Result.Success();
    }
}
