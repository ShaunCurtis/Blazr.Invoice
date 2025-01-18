/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

public static class ApplicationServerInfrastructureServices
{
    public static void AddAppServerInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContextFactory<InMemoryInvoiceTestDbContext>(options
            => options.UseInMemoryDatabase($"TestDatabase-{Guid.NewGuid().ToString()}"));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                typeof(DmoCustomer).Assembly,
                typeof(DboCustomer).Assembly,
                typeof(CustomerCommandHandler).Assembly
                ));

        services.AddScoped<IMessageBus, MessageBus>();

        // Add the standard handlers
        services.AddScoped<IListRequestHandler, ListRequestServerHandler<InMemoryInvoiceTestDbContext>>();
        services.AddScoped<IItemRequestHandler, ItemRequestServerHandler<InMemoryInvoiceTestDbContext>>();
        services.AddScoped<ICommandHandler, CommandServerHandler<InMemoryInvoiceTestDbContext>>();

        // Add Custom Handlers
        services.AddScoped<ICommandHandler<Invoice>, InvoiceCommandServerHandler<InMemoryInvoiceTestDbContext>>();

        // Add any individual entity services
        services.AddCustomerInfrastructureServices();
        services.AddInvoiceInfrastructureServices();
        services.AddInvoiceItemInfrastructureServices();
   }

    public static void AddTestData(IServiceProvider provider)
    {
        var factory = provider.GetService<IDbContextFactory<InMemoryInvoiceTestDbContext>>();

        if (factory is not null)
            InvoiceTestDataProvider.Instance().LoadDbContext<InMemoryInvoiceTestDbContext>(factory);
    }
}
