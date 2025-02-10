/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.Antimony.Infrastructure.Server;
using Blazr.App.Presentation;
using Blazr.FluxGate;
using Blazr.Gallium;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blazr.App.Infrastructure.Server;

public static class ApplicationServerServices
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddDbContextFactory<InMemoryInvoiceTestDbContext>(options
            => options.UseInMemoryDatabase($"TestDatabase-{Guid.NewGuid().ToString()}"));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                typeof(DmoCustomer).Assembly
                ));

        services.AddScoped<IMessageBus, MessageBus>();

        // Add the standard Antimony Server handlers
        // These are used for simple entities
        services.AddScoped<IListRequestBroker, ListRequestServerBroker<InMemoryInvoiceTestDbContext>>();
        services.AddScoped<IRecordRequestBroker, RecordRequestServerBroker<InMemoryInvoiceTestDbContext>>();
        services.AddScoped<ICommandBroker, CommandServerBroker<InMemoryInvoiceTestDbContext>>();

        // Add Custom Handlers
        services.AddScoped<ICommandBroker<Invoice>, InvoiceCommandServerBroker<InMemoryInvoiceTestDbContext>>();

        // GridState inMemory Store 
        services.AddScoped<KeyedFluxGateStore<GridState, Guid>>();
        services.AddTransient<FluxGateDispatcher<GridState>, GridStateDispatcher>();

        // Presenter Factories
        services.AddScoped<ILookupPresenterFactory, LookupPresenterFactory>();
        services.AddScoped<IEditPresenterFactory, EditPresenterFactory>();
        services.AddTransient<IReadPresenterFactory, ReadPresenterFactory>();

        services.AddQuickGridEntityFrameworkAdapter();

        // Add any individual entity services
        services.AddCustomerServices();
        //services.AddInvoiceInfrastructureServices();
        //services.AddInvoiceItemInfrastructureServices();
   }

    public static void AddTestData(IServiceProvider provider)
    {
        var factory = provider.GetService<IDbContextFactory<InMemoryInvoiceTestDbContext>>();

        if (factory is not null)
            InvoiceTestDataProvider.Instance().LoadDbContext<InMemoryInvoiceTestDbContext>(factory);
    }
}
