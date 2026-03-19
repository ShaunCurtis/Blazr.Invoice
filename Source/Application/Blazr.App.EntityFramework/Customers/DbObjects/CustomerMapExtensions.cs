/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure;

internal static class CustomerMapExtensions
{
    extension(DmoCustomer item)
    {
        public DboCustomer MapToDbo => new DboCustomer()
        {
            CustomerID = item.Id.Value,
            CustomerName = item.Name.Value
        };
    }

    extension(DboCustomer item)
    {
        public DmoCustomer MapToDmo => new DmoCustomer()
        {
            Id = CustomerId.Load(item.CustomerID),
            Name = new(item.CustomerName ?? Title.DefaultValue)
        };

        public FkoCustomer MapToFko => new FkoCustomer(
            Id: CustomerId.Load(item.CustomerID),
            Name: new(item.CustomerName ?? Title.DefaultValue)
        );
    }

    extension(DvoCustomer item)
    {
        public DmoCustomer MapToDmo => new DmoCustomer()
        {
            Id = CustomerId.Load(item.CustomerID),
            Name = new(item.CustomerName ?? Title.DefaultValue)
        };
    }
}
