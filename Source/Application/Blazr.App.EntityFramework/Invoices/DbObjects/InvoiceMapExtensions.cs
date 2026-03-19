/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.EntityFramework;

internal static class InvoiceMapExtensions
{
    extension(DmoInvoice item)
    {
        public DboInvoice MapToDbo => new DboInvoice
        {
            InvoiceID = item.Id.Value,
            CustomerID = item.Customer.Id.Value,
            TotalAmount = item.TotalAmount.Value,
            Date = item.Date.ToDateTime
        };
    }

    extension(DvoInvoice item)
    {
        public DmoInvoice MapToDmo => new DmoInvoice
        {
            Id = InvoiceId.Load(item.InvoiceID),
            Customer = new FkoCustomer(CustomerId.Load(item.CustomerID), new(item.CustomerName)),
            TotalAmount = new(item.TotalAmount),
            Date = new(item.Date)
        };

        public Result<DmoInvoice> MapToResultT =>
            ResultT.Read(item.MapToDmo);
    }

    extension(DmoInvoiceItem item)
    {
        public DboInvoiceItem MapToDbo => new DboInvoiceItem
        {
            InvoiceItemID = item.Id.Value,
            InvoiceID = item.InvoiceId.Value,
            Amount = item.Amount.Value,
            Description = item.Description.Value
        };
    }

    extension(DvoInvoiceItem item)
    {
        public DmoInvoiceItem MapToDmo => new DmoInvoiceItem
        {
            Id = InvoiceItemId.Load(item.InvoiceItemID),
            InvoiceId = InvoiceId.Load(item.InvoiceID),
            Amount = new(item.Amount),
            Description = new(item.Description),
        };
    }

    extension(DboInvoiceItem item)
    {
        public DvoInvoiceItem MapToDvo => new DvoInvoiceItem
        {
            InvoiceID = item.InvoiceID,
            InvoiceItemID = item.InvoiceItemID,
            Amount = item.Amount,
            Description = item.Description,
        };
    }
}
