/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Core;

public static class AppDictionary
{
    public static class Common
    {
        public const string AppHttpClient = "AppHttpClient";
    }

    public static class Shared
    {
        public const string CustomerFkApiUrl = "api/fk/customer";
    }

    public static class Customer
    {
        public const string CustomerId = "Id";
        public const string CustomerID = "CustomerID";
        public const string CustomerName = "Name";

        public const string CustomerListApiUrl = "api/customer/list";
        public const string CustomerRecordApiUrl = "api/customer/record";
        public const string CustomerCommandApiUrl = "api/customer/Command";

    }

    public static class Invoice
    {
        public const string InvoiceId = "Id";
        public const string InvoiceID = "InvoiceID";
        public const string Date = "Date";
        public const string TotalAmount = "TotalAmount";

        public const string InvoiceListApiUrl = "api/invoice/liat";
        public const string InvoiceRecordApiUrl = "api/invoice/record";
        public const string InvoiceEntityApiUrl = "api/invoice/entity";
        public const string CustomerCommandApiUrl = "api/invoice/Command";
    }

    public static class InvoiceItem
    {
        public const string InvoiceItemId = "Id";
        public const string InvoiceItemID = "InvoiceItemID";
        public const string Description = "Description";
        public const string Amount = "Amount";

        public const string InvoiceItemListApiUrl = "api/invoice/liat";
        public const string InvoiceItemRecordApiUrl = "api/invoice/record";
    }
}
