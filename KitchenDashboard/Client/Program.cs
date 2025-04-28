using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KitchenDashboard.Client.Services;
using KitchenDashboard.Client;
using MudBlazor.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ChoreService>();
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-AU");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-AU");

builder.Services.AddMudServices();

await builder.Build().RunAsync();