/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.App.Core;
using Blazr.Diode.Mediator;
using Blazr.Manganese;
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.Test;

public partial class InvoiceTests
{

    [Fact]
    public async Task GetAnInvoice()
    {
        // Get a fully stocked DI container
        var provider = GetServiceProvider();

        var mediator = provider.GetRequiredService<IMediatorBroker>()!;

        // Get the test item and it's Id from the Test Provider

        // Get a test item and it's Id from the Test Provider
        var controlInvoice = _testDataProvider.GetTestInvoice();
        var controlId = controlInvoice.Id;

        var controlInvoiceItems = _testDataProvider.GetInvoiceItems(controlId).ToList();

        var entityResult = await mediator.DispatchAsync(new InvoiceEntityRequest(controlId));
        Assert.IsType<SuccessResult<InvoiceEntity>>(entityResult);

        var entity = ((SuccessResult<InvoiceEntity>)entityResult).Value;

        Assert.Equal(controlInvoiceItems.Count, entity.InvoiceItems.Count);
        Assert.Contains(entity.InvoiceItems.First(), controlInvoiceItems);
    }
}
