using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using BlazorLib;
using SharedLib;
using ABLog;
using Refit;
using SharedLib.IServices;
using RazorLib;

namespace AbLog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices();

            builder.Services.AddRefitClient<IRefitHardwaresService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddRefitClient<IRefitCommandsService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddRefitClient<IRefitConditionsService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddRefitClient<IRefitContentionsService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddRefitClient<IRefitScriptsService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddRefitClient<IRefitStorageService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddRefitClient<IRefitTriggersService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddSingleton<IParametersStorageService, ParametersStorageRefitService>();
            builder.Services.AddSingleton<IHardwaresService, HardwaresRefitService>();

            await builder.Build().RunAsync();
        }
    }
}