/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

/// <summary>
/// Mediator Handler to get an Invoice Entity - A DmoInvoice object
/// </summary>
public record InvoiceRecordHandler : IRequestHandler<InvoiceRequests.InvoiceRecordRequest, Result<DmoInvoice>>
{
    private IRecordRequestBroker _broker;

    public InvoiceRecordHandler(IRecordRequestBroker broker)
    {
        _broker = broker;
    }

    public async Task<Result<DmoInvoice>> Handle(InvoiceRequests.InvoiceRecordRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<DboInvoice, bool>> findExpression = (item) =>
            item.InvoiceID == request.Id.Value;

        var query = new RecordQueryRequest<DboInvoice>(findExpression);

        var result = await _broker.ExecuteAsync<DboInvoice>(query);

        if (!result.HasSucceeded(out DboInvoice? record))
            return result.ConvertFail<DmoInvoice>();

        var returnItem = DboInvoiceMap.Map(record);

        return Result<DmoInvoice>.Success(returnItem);
    }
}
