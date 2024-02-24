using BlazorLib;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RazorLib;
using Refit;
using SharedLib;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
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
builder.Services.AddRefitClient<IRefitCamerasService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddRefitClient<IRefitToolsService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddRefitClient<IRefitSystemCommandsService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddRefitClient<IRefitUsersService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddSingleton<IParametersStorageService, ParametersStorageRefitService>();
builder.Services.AddSingleton<IHardwaresService, HardwaresRefitService>();
builder.Services.AddSingleton<ISystemCommandsService, SystemCommandsRefitService>();
builder.Services.AddSingleton<IUsersService, UsersRefitService>();
builder.Services.AddSingleton<IToolsService, ToolsRefitService>();
var http = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};
builder.Services.AddScoped(sp => http);

using HttpResponseMessage conf_response = await http.GetAsync("conf.json");
using Stream conf_stream = await conf_response.Content.ReadAsStreamAsync();
builder.Configuration.AddJsonStream(conf_stream);

ClientConfigModel settings = new();
builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);

await builder.Build().RunAsync();
