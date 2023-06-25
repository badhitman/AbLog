﻿using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using Newtonsoft.Json;
using MQTTnet.Client;
using ab.context;
using SharedLib;
using ServerLib;
using MQTTnet;

namespace RemoteClient
{
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

            string folder_exe = Path.GetDirectoryName(Environment.ProcessPath)!;
            string file_conf_path = Path.Combine(folder_exe, "wwwroot", "conf.json");
            using FileStream conf_file = File.Open(file_conf_path, FileMode.Open);
            builder.Configuration.AddJsonStream(conf_file);

            ClientConfigModel settings = new();
            builder.Configuration.Bind(settings);
            builder.Services.AddSingleton(settings);

            builder.Services.AddSingleton<IParametersStorageService, ParametersStorageLocalService>();
            builder.Services.AddSingleton<IHardwaresService, HardwaresMqttService>();

            using ParametersContext _context = new();
            string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
            MqttConfigModel mqtt_settings = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig) ?? new();
            builder.Services.AddSingleton(x => mqtt_settings);

            MqttFactory mqttFactory = new();
            builder.Services.AddTransient(x => mqttFactory);
            IMqttClient mqttClient = mqttFactory.CreateMqttClient();
            builder.Services.AddSingleton(x => mqttClient);

            builder.Services.AddScoped<IToolsService, ToolsLocalService>();
            builder.Services.AddSingleton<IMqttBaseService, MqttClientService>();

            MauiApp maui_app = builder.Build();

            if (mqtt_settings.AutoStart && mqtt_settings.IsConfigured)
            {
                IMqttBaseService _mqtt_cli_srv = maui_app.Services.GetRequiredService<IMqttBaseService>();
                Task.Run(async () => { await _mqtt_cli_srv.StartService(); });
            }

            return maui_app;
        }
    }
}