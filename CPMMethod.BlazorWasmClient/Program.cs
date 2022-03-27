using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CPMMethod.BlazorWasmClient;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider
    .RegisterLicense("NjAxMDY4QDMxMzkyZTM0MmUzMGx3N2c1NDNIQjZ2RnV1aVhram5iY090M2Vnc0J1c2pNc293dXFkUmZkRHM9");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });

await builder.Build().RunAsync();