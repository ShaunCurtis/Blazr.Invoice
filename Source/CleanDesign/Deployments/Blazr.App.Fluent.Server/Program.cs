global using Blazr.App.Infrastructure;
global using Blazr.App.Presentation;
using Blazr.App.Fluent.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Blazr.App.Infrastructure.Server;
using Blazr.App.Presentation.FluentUI;
using Blazr.App.UI.FluentUI;



var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLogging(builder => builder.AddConsole());

builder.Services.AddFluentUIComponents();

builder.Services.AddAppServerInfrastructureServices();
builder.Services.AddAppFluentUIPresentationServices();
builder.Services.AddAppFluentUIServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// get the DbContext factory and add the test data
var factory = app.Services.GetService<IDbContextFactory<InMemoryInvoiceTestDbContext>>();
if (factory is not null)
    InvoiceTestDataProvider.Instance().LoadDbContext<InMemoryInvoiceTestDbContext>(factory);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies([typeof(Blazr.App.UI.FluentUI.CustomerListPage).Assembly]);

app.Run();

