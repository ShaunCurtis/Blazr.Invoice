/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Blazr.App.EntityFramework;
using Blazr.App.Server.API;
using Blazr.App.Wasm.Server;

var builder = WebApplication.CreateBuilder(args);

//builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddBlazorWASMServerAppServices();

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

//app.MapDefaultEndpoints();

app.AddAppAPIEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Services.AddInvoiceTestData();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<Blazr.App.Wasm.Server.Components.App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Blazr.App.Wasm.Client._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(Blazr.App.UI.CustomerEditForm).Assembly);

app.Run();
