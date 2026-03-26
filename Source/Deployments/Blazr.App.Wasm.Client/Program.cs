using Blazr.App.Blazor;
using Blazr.App.Wasm.Client;
using Blazr.Diode.Mediator;
using Blazr.Gallium;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorWasmAppServices();

await builder.Build().RunAsync();
