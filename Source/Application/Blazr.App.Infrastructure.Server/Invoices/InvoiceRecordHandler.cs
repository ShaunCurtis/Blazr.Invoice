/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

public record InvoiceItemHandler : IRequestHandler<InvoiceRequests.InvoiceRecordRequest, Result<DmoInvoice>>
{
    private IItemRequestHandler _handler;

    public InvoiceItemHandler(IItemRequestHandler handler)
    {
        _handler = handler;
    }

    public async Task<Result<DmoInvoice>> Handle(InvoiceRequests.InvoiceRecordRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<DboInvoice, bool>> findExpression = (item) =>
            item.InvoiceID == request.Id.Value;

        var query = new ItemQueryRequest<DboInvoice>(findExpression);

        var result = await _handler.ExecuteAsync<DboInvoice>(query);

        if (!result.HasSucceeded(out DboInvoice? record))
            return result.ConvertFail<DmoInvoice>();

        var returnItem = DboInvoiceMap.Map(record);

        return Result<DmoInvoice>.Success(returnItem);
    }
}
