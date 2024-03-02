////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using ab.context;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MudBlazor.Services;
using Newtonsoft.Json;
using ServerLib;
using SharedLib;
using System.Reflection;

namespace ab_log_rc;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        GlobalStatic.PefixDbFile = "-client";
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
        //
        Assembly assembly = Assembly.GetExecutingAssembly();
        string[] ManifestResourceNames = assembly.GetManifestResourceNames();
        string resourceName = ManifestResourceNames.First(s => s.EndsWith("conf.json", StringComparison.CurrentCultureIgnoreCase));

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }
            builder.Configuration.AddJsonStream(stream);
        }


        ClientConfigModel settings = new();
        builder.Configuration.Bind(settings);
        builder.Services.AddSingleton(settings);
        builder.Services.AddHttpClient<ToolsRemoteService>();

        builder.Services.AddSingleton<IParametersStorageService, ParametersStorageRemoteService>();
        builder.Services.AddSingleton<ISystemCommandsService, SystemCommandsMqttService>();
        builder.Services.AddSingleton<IHardwaresService, HardwaresMqttService>();
        builder.Services.AddSingleton<IMqttBaseService, MqttClientService>();
        builder.Services.AddSingleton<IEmailService, EmailLocalService>();
        builder.Services.AddSingleton<IUsersService, UsersMqttService>();
        builder.Services.AddScoped<IToolsService, ToolsRemoteService>();


        using ParametersContext _context = new();
        string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
        MqttConfigModel mqtt_settings = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig) ?? new();
        builder.Services.AddSingleton(x => mqtt_settings);

        MqttFactory mqttFactory = new();
        builder.Services.AddTransient(x => mqttFactory);
        IMqttClient mqttClient = mqttFactory.CreateMqttClient();
        builder.Services.AddSingleton(x => mqttClient);

        MauiApp maui_app = builder.Build();

        if (mqtt_settings.AutoStart && mqtt_settings.IsConfigured)
        {
            IMqttBaseService _mqtt_cli_srv = maui_app.Services.GetRequiredService<IMqttBaseService>();
            Task.Run(async () => { await _mqtt_cli_srv.StartService(); });
        }

        return maui_app;
    }
}