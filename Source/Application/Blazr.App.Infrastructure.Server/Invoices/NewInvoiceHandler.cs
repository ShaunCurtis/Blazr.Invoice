/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

public record NewInvoiceHandler : IRequestHandler<NewInvoiceRequest, Result<Invoice>>
{
    private IListRequestHandler _listHandler;
    private IItemRequestHandler _itemHhandler;
    private IMessageBus _messageBus;

    public NewInvoiceHandler(IItemRequestHandler itemRequestHandler, IListRequestHandler listRequestHandler, IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _listHandler = listRequestHandler;
        _itemHhandler = itemRequestHandler;
    }

    public async Task<Result<Invoice>> Handle(NewInvoiceRequest request, CancellationToken cancellationToken)
    {
        DmoInvoice? invoice = null;
        
        {
            Expression<Func<DboInvoice, bool>> findExpression = (item) =>
                item.InvoiceID == request.Id.Value;

            var query = new ItemQueryRequest<DboInvoice>(findExpression);
            var result = await _itemHhandler.ExecuteAsync<DboInvoice>(query);
            
            if (!result.HasSucceeded(out DboInvoice? record))
                return result.ConvertFail<Invoice>();
            
            invoice = DboInvoiceMap.Map(record);
        }

        List<DmoInvoiceItem>? invoiceItems = new();
        {
            Expression<Func<DboInvoiceItem, bool>> filterExpression = (item) =>
                item.InvoiceID == request.Id.Value;
            
            var query = new ListQueryRequest<DboInvoiceItem>() { FilterExpression=filterExpression };
            var result = await _listHandler.ExecuteAsync<DboInvoiceItem>(query);
            
            if (!result.HasSucceeded(out ListResult<DboInvoiceItem> records))
                return result.ConvertFail<Invoice>();
            
            invoiceItems = records.Items.Select(item => DboInvoiceItemMap.Map(item)).ToList();
        }

        var invoiceComposite = new Invoice(invoice, invoiceItems);

        return Result<Invoice>.Success(invoiceComposite);
    }
}
