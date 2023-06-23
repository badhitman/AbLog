using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using MQTTnet.Client;
using ServicesLib;
using BlazorLib;
using SharedLib;

namespace RemoteClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            builder.Services.AddMudServices();

            string folder_exe = Path.GetDirectoryName(Environment.ProcessPath)!;
            string file_conf_path = Path.Combine(folder_exe, "wwwroot", "conf.json");
            using FileStream conf_file = File.Open(file_conf_path, FileMode.Open);
            builder.Configuration.AddJsonStream(conf_file);

            ClientConfigModel settings = new();
            builder.Configuration.Bind(settings);
            builder.Services.AddSingleton(settings);

            builder.Services.AddSingleton<IParametersStorageService, ParametersStorageRefitService>();
            builder.Services.AddSingleton<IHardwaresService, HardwaresMqttService>();

            builder.Services.AddScoped<IMqttClient, MqttClient>();

            return builder.Build();
        }
    }
}