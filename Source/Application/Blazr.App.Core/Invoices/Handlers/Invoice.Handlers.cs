/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

namespace Blazr.App.Core;

//public record UpdateInvoiceHandler : IRequestHandler<UpdateInvoiceAction, Result<Invoice>>
//{
//    public Task<Result<Invoice>> Handle(UpdateInvoiceAction request, CancellationToken cancellationToken)
//    {
//        var invoice = request.Invoice;
//        invoice.UpdateInvoice(request.Item);
//        return Task.FromResult(Result<Invoice>.Success(invoice));
//    }
//}

//public record DeleteInvoiceHandler : IRequestHandler<DeleteInvoiceAction, Result<Invoice>>
//{
//    public Task<Result<Invoice>> Handle(DeleteInvoiceAction request, CancellationToken cancellationToken)
//    {
//        var invoice = request.Invoice;
//        invoice.State = CommandState.Delete;
//        return Task.FromResult(Result<Invoice>.Success(invoice));
//    }
//}

//public record DeleteInvoiceItemHandler : IRequestHandler<DeleteInvoiceItemAction, Result<Invoice>>
//{
//    public Task<Result<Invoice>> Handle(DeleteInvoiceItemAction request, CancellationToken cancellationToken)
//    {
//        var invoice = request.Invoice;

//        var invoiceItem = invoice.Items.FirstOrDefault(item => item.InvoiceItemRecord == request.Item);
//        if (invoiceItem != null)
//        {
//            invoiceItem.State = CommandState.Delete;
//            invoice.ItemsBin.Add(invoiceItem);
//            invoice.Items.Remove(invoiceItem);
//        }

//        return Task.FromResult(Result<Invoice>.Success(invoice));
//    }
//}

//public record UpdateInvoiceItemHandler : IRequestHandler<UpdateInvoiceItemAction, Result<Invoice>>
//{
//    public Task<Result<Invoice>> Handle(UpdateInvoiceItemAction request, CancellationToken cancellationToken)
//    {
//        var invoice = request.Invoice;

//        var invoiceItem = invoice.Items.FirstOrDefault(item => item.InvoiceItemRecord == request.Item);
//        if (invoiceItem != null)
//        {
//            invoiceItem.State = invoiceItem.State.AsDirty;
//            invoiceItem.UpdateInvoiceItem(request.Item);
//        }

//        return Task.FromResult(Result<Invoice>.Success(invoice));
//    }
//}

//public record AddInvoiceItemHandler : IRequestHandler<AddInvoiceItemAction, Result<Invoice>>
//{
//    public Task<Result<Invoice>> Handle(AddInvoiceItemAction request, CancellationToken cancellationToken)
//    {
//        var invoice = request.Invoice;

//        var invoiceItem = new InvoiceItem(request.Item, request.IsNew);
//        var invoiceItem = invoice.Items.FirstOrDefault(item => item.InvoiceItemRecord == request.Item);
//        if (invoiceItem != null)
//        {
//            invoiceItem.State = invoiceItem.State.AsDirty;
//            invoiceItem.UpdateInvoiceItem(request.Item);
//        }

//        return Task.FromResult(Result<Invoice>.Success(invoice));
//    }
//}
