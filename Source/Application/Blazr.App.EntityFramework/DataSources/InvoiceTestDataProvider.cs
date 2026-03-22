/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.EntityFramework;

public sealed class InvoiceTestDataProvider
{
    private IEnumerable<DboCustomer> Customers => _customers.AsEnumerable();
    private IEnumerable<DboInvoice> Invoices => _invoices.AsEnumerable();
    private IEnumerable<DboInvoiceItem> InvoiceItems => _invoiceItems.AsEnumerable();

    private List<DboCustomer> _customers = new List<DboCustomer>();
    private List<DboInvoice> _invoices = new List<DboInvoice>();
    private List<DboInvoiceItem> _invoiceItems = new List<DboInvoiceItem>();

    public FkoCustomer GetFkoCustomer(CustomerId id)
        => this.Customers.First(item => item.CustomerID == id.Value).MapToFko;

    public InvoiceEntity GetTestInvoiceEntity()
    {
        var invoice = this.Invoices.Skip(Random.Shared.Next(_invoices.Count)).First();
        var customer = this.Customers.First(item => item.CustomerID == invoice.CustomerID).MapToFko;
        var invoiceItems = this.InvoiceItems.Where(item => item.InvoiceID == invoice.InvoiceID).Select(item => item.MapToDvo.MapToDmo);
        return InvoiceEntity.Load(new DmoInvoice
        {
            Id = InvoiceId.Load(invoice.InvoiceID),
            Customer = customer,
            Date = new Date(invoice.Date),
            TotalAmount = new Money(invoice.TotalAmount),
        }, invoiceItems);
    }

    public DmoCustomer GetTestCustomer()
        => this.Customers.Skip(Random.Shared.Next(_customers.Count)).First().MapToDmo;

    public InvoiceId GetTestInvoiceId()
        => InvoiceId.Load(this.Invoices.Skip(Random.Shared.Next(_invoices.Count)).First().InvoiceID);

    public DmoInvoice GetTestInvoice()
    {
        var invoice = this.Invoices.Skip(Random.Shared.Next(_invoices.Count)).First();
        var customer = this.Customers.First(item => item.CustomerID == invoice.CustomerID).MapToFko;
        return new DmoInvoice
        {
            Id = InvoiceId.Load(invoice.InvoiceID),
            Customer = customer,
            Date = new Date(invoice.Date),
            TotalAmount = new Money(invoice.TotalAmount),
        };
    }

    public IEnumerable<DmoInvoiceItem> GetInvoiceItems(InvoiceId id)
        => this.InvoiceItems.Where(item => item.InvoiceID == id.Value).Select(item => item.MapToDvo.MapToDmo);

    public InvoiceTestDataProvider()
    {
        this.Loader();
    }

    private void Loader()
    {
        _customers = new();

        var id = Guid.CreateVersion7();
        DboCustomer customer = new()
        {
            CustomerID = id,
            CustomerName = "EasyJet"
        };
        _customers.Add(customer);

        {
            var _id = Guid.CreateVersion7();
            _invoices.Add(new()
            {
                InvoiceID = _id,
                CustomerID = id,
                Date = DateTime.Now.AddDays(-3),
                TotalAmount = 50
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A321",
                Amount = 15
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A350",
                Amount = 35
            });
        }

        id = Guid.CreateVersion7();
        customer = new()
        {
            CustomerID = id,
            CustomerName = "RyanAir"
        };
        _customers.Add(customer);

        {
            var _id = Guid.CreateVersion7();
            _invoices.Add(new()
            {
                InvoiceID = _id,
                CustomerID = id,
                Date = DateTime.Now.AddDays(-2),
                TotalAmount = 27
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A319",
                Amount = 12
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A321",
                Amount = 15
            });
        }

        id = Guid.CreateVersion7();
        customer = new()
        {
            CustomerID = id,
            CustomerName = "Air France"
        };
        _customers.Add(customer);

        {
            var _id = Guid.CreateVersion7();
            _invoices.Add(new()
            {
                InvoiceID = _id,
                CustomerID = id,
                Date = DateTime.Now.AddDays(-1),
                TotalAmount = 60
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A330",
                Amount = 25
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A350",
                Amount = 35
            });
        }

        id = Guid.CreateVersion7();
        customer = new()
        {
            CustomerID = id,
            CustomerName = "Sabena"
        };
        _customers.Add(customer);

        {
            var _id = Guid.CreateVersion7();
            _invoices.Add(new()
            {
                InvoiceID = _id,
                CustomerID = id,
                Date = DateTime.Now.AddDays(-1),
                TotalAmount = 60
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A330",
                Amount = 25
            });
            _invoiceItems.Add(new()
            {
                InvoiceItemID = Guid.CreateVersion7(),
                InvoiceID = _id,
                Description = "Airbus A330",
                Amount = 25
            });
        }

        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "TAP" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "American Airlines" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Quantas" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Virgin" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Lufhansa" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "SAS" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Emirates" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Etihad" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Cathay Pacific" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Singapore Airlines" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "United Airlines" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Alaskan" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Logan Air" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Qatar Airlines" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Air Egypt" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Iberia" });
        _customers.Add(new() { CustomerID = Guid.CreateVersion7(), CustomerName = "Alitalia" });

    }

    public void LoadDbContext<TDbContext>(IDbContextFactory<TDbContext> factory) where TDbContext : DbContext
    {
        using var dbContext = factory.CreateDbContext();

        var dboCustomers = dbContext.Set<DboCustomer>();
        var dboInvoices = dbContext.Set<DboInvoice>();
        var dboInvoiceItems = dbContext.Set<DboInvoiceItem>();

        // Check if we already have a full data set
        // If not clear down any existing data and start again
        if (dboCustomers.Count() == 0)
            dbContext.AddRange(_customers);

        if (dboInvoices.Count() == 0)
            dbContext.AddRange(_invoices);

        if (dboInvoiceItems.Count() == 0)
            dbContext.AddRange(_invoiceItems);

        dbContext.SaveChanges();
    }

    private static InvoiceTestDataProvider? _provider;

    public static InvoiceTestDataProvider Instance()
    {
        if (_provider is null)
            _provider = new InvoiceTestDataProvider();

        return _provider;
    }
}