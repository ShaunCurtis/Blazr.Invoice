using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.App.Infrastructure.Server;

public record SaveInvoiceHandler : IRequestHandler<SaveInvoiceRequest, Result>
{
    private readonly IMessageBus _messageBus;
    private readonly IDbContextFactory<InMemoryInvoiceTestDbContext> _factory;

    public SaveInvoiceHandler(IDbContextFactory<InMemoryInvoiceTestDbContext> dbContextFactory, IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _factory = dbContextFactory;
    }

    public async Task<Result> Handle(SaveInvoiceRequest request, CancellationToken cancellationToken)
    {
    }
}
