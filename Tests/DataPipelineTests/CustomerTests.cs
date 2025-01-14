using Blazr.App.Core;
using Blazr.App.Infrastructure;
using Blazr.OneWayStreet.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataPipelineTests
{
    public class CustomerTests
    {
        private TestDataProvider _testDataProvider;

        public CustomerTests()
            => _testDataProvider = TestDataProvider.Instance();

        private IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            //services.AddAppServerInfrastructureServices();
            services.AddAppServerMappedInfrastructureServices();
            services.AddLogging(builder => builder.AddDebug());

            var provider = services.BuildServiceProvider();

            // get the DbContext factory and add the test data
            var factory = provider.GetService<IDbContextFactory<InMemoryTestDbContext>>();
            if (factory is not null)
                TestDataProvider.Instance().LoadDbContext<InMemoryTestDbContext>(factory);

            return provider!;
        }

        [Fact]
        public async Task LoadACustomer()
        {
            var provider = GetServiceProvider();
            var dataBroker = provider.GetService<IDataBroker>()!;

            var testItem = _testDataProvider.Customers.First();
            var testUid = testItem.CustomerID;

            var request = new ItemQueryRequest(testUid);
            var loadResult = await dataBroker.ExecuteQueryAsync<DmoCustomer>(request);

            var composite = loadResult.Item;

            Assert.True(loadResult.Successful);
        }

        //[Fact]
        //public async Task TestUpdatingRoot()
        //{
        //    var provider = GetServiceProvider();
        //    var dataBroker = provider.GetService<IDataBroker>()!;

        //    var testItem = _testDataProvider.Invoices.First();
        //    var testUid = testItem.InvoiceID;

        //    var request = new ItemQueryRequest(testUid);
        //    var loadResult = await dataBroker.ExecuteQueryAsync<InvoiceComposite>(request);
        //    Assert.True(loadResult.Successful);

        //    var composite = loadResult.Item!;

        //    var invoiceEditContext = new DmoInvoiceEditContext(composite.Invoice);
        //    Assert.NotNull(invoiceEditContext);

        //    var newDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));
        //    invoiceEditContext.Date = newDate;

        //    var updateResult = composite.UpdateInvoice(invoiceEditContext.Mutate);
        //    Assert.True(updateResult.Successful);
        //    Assert.Equal(newDate, composite.Invoice.Date);

        //    var command = new CommandRequest<InvoiceComposite>(composite, CommandState.Update);
        //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
        //    Assert.True(executeResult.Successful);

        //    var testResult = await dataBroker.ExecuteQueryAsync<InvoiceComposite>(request);
        //    Assert.True(testResult.Successful);

        //    var testComposite = testResult.Item!;
        //    Assert.Equal(newDate, testComposite.Invoice.Date);
        //}

        //[Fact]
        //public async Task TestUpdatingInvoiceItem()
        //{
        //    var provider = GetServiceProvider();
        //    var dataBroker = provider.GetService<IDataBroker>()!;

        //    var testItem = _testDataProvider.Invoices.First();
        //    var testUid = testItem.InvoiceID;

        //    var request = new ItemQueryRequest(testUid);
        //    var loadResult = await dataBroker.ExecuteQueryAsync<InvoiceComposite>(request);
        //    Assert.True(loadResult.Successful);

        //    var composite = loadResult.Item!;

        //    var testInvoiceItemUid = composite.InvoiceItems.First().InvoiceItemId;

        //    var invoiceItem = composite.GetInvoiceItem(testInvoiceItemUid);
        //    Assert.NotNull(invoiceItem);

        //    var itemEditContext = new DmoInvoiceItemEditContext(invoiceItem);
        //    Assert.NotNull(itemEditContext);

        //    var newDescription = "Fokker F27";
        //    var newAmount = 5;
        //    itemEditContext.Description = newDescription;
        //    itemEditContext.Amount = newAmount;

        //    var result = composite.UpdateInvoiceItem(itemEditContext.Id, itemEditContext.Mutate);
        //    Assert.True(result.Successful);

        //    var command = new CommandRequest<InvoiceComposite>(composite, CommandState.Update);
        //    var executeResult = await dataBroker.ExecuteCommandAsync(command);
        //    Assert.True(executeResult.Successful);

        //    var testResult = await dataBroker.ExecuteQueryAsync<InvoiceComposite>(request);
        //    Assert.True(testResult.Successful);

        //    var testComposite = testResult.Item!;

        //    var testInvoiceItem = composite.GetInvoiceItem(testInvoiceItemUid);
        //    Assert.NotNull(testInvoiceItem);

        //    Assert.Equal(newDescription, testInvoiceItem!.Description);
        //    Assert.Equal(newAmount, testInvoiceItem!.Amount);
        //}
    }
}