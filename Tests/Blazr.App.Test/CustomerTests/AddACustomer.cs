/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Blazr.App.Core;
using Blazr.Diode;
using Blazr.Diode.Mediator;
using Blazr.Manganese;
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.Test;

public partial class CustomerTests
{
    [Fact]
    public async Task AddACustomer()
    {
        var provider = GetServiceProvider();
        var mediator = provider.GetRequiredService<IMediatorBroker>()!;

        // Create a new customer record.  Note the IsNew flag is set in the record
        var newCustomer = DmoCustomer.NewCustomer() with { Name = new("Alaskan") };

        // newCustomer has the isNew flag set in the record so we need to fix that to make a comparison
        var compareCustomer = newCustomer with { Id = CustomerId.Load(newCustomer.Id.Value) };

        var customerAddResult = await mediator.DispatchAsync(CustomerCommandRequest.Create(newCustomer, RecordState.CreateAsNewState));
        Assert.IsType<SuccessResult<CustomerId>>(customerAddResult);

        var customerResult = await mediator.DispatchAsync(new CustomerRecordRequest(newCustomer.Id));
        Assert.IsType<SuccessResult<DmoCustomer>>(customerResult);

        // check it matches the test record
        Assert.Equivalent(compareCustomer, ((SuccessResult<DmoCustomer>)customerResult).Value);
    }
}
