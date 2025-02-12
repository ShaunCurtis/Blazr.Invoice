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
        public const string WeatherHttpClient = "WeatherHttpClient";
        public const string Id = "Id";
    }

    public static class WeatherForecast
    {
        public const string WeatherForecastId = "WeatherForecastId";
        public const string WeatherForecastID = "WeatherForecastID";
        public const string Date = "Date";
        public const string Temperature = "Temperature";
        public const string TemperatureC = "TemperatureC";
        public const string Summary = "Summary";

        public const string WeatherForecastListAPIUrl = "/API/WeatherForecast/GetItems";
        public const string WeatherForecastItemAPIUrl = "/API/WeatherForecast/GetItem";
        public const string WeatherForecastCommandAPIUrl = "/API/WeatherForecast/Command";
    }
    public static class Customer
    {
        public const string CustomerId = "Id";
        public const string CustomerID = "CustomerID";
        public const string CustomerName = "CustomerName";
    }

    public static class Invoice
    {
        public const string InvoiceId = "Id";
        public const string InvoiceID = "InvoiceID";
        public const string Date = "Date";
        public const string TotalAmount = "TotalAmount";
    }

    public static class InvoiceItem
    {
        public const string InvoiceItemId = "Id";
        public const string InvoiceItemID = "InvoiceItemID";
        public const string Description = "Description";
        public const string Amount = "Amount";
    }
}
