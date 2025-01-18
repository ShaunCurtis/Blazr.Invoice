using Blazr.App.Server.Components;
using Blazr.App.Infrastructure;
using Blazr.App.Infrastructure.Server;
using Blazr.App.Presentation;
using Blazr.App.Presentation.Bootstrap;
using Blazr.App.UI;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAppServerInfrastructureServices();
builder.Services.AddAppUIServices();
builder.Services.AddAppBootstrapPresentationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// get the DbContext factory and add the test data
var factory = app.Services.GetService<IDbContextFactory<InMemoryInvoiceTestDbContext>>();
if (factory is not null)
    InvoiceTestDataProvider.Instance().LoadDbContext<InMemoryInvoiceTestDbContext>(factory);

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Blazr.App.UI._Imports).Assembly);

app.Run();
