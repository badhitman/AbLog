////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Text.Json.Serialization;
using System.Runtime.Versioning;
using Telegram.Bot.Services;
using Newtonsoft.Json;
using MQTTnet.Client;
using Telegram.Bot;
using ab.context;
using SharedLib;
using ServerLib;
using NLog.Web;
using MQTTnet;
using NLog;

namespace AbLogServer;

#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
[SupportedOSPlatform("iOS")]
[SupportedOSPlatform("MacCatalyst")]
public class Program
{
    public static async Task Main(string[] args)
    {
        GlobalStatic.PefixDbFile = "-server";
        Logger logger = LogManager
            .Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();

        logger.Warn("init main");
        logger.Warn($"main-db: {GlobalStatic.MainDatabasePath}");
        logger.Warn($"storage-db: {GlobalStatic.ParametersStorageDatabasePath}");
        CheckContext();
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            IWebHostEnvironment _env = builder.Environment;

            builder.Services.AddHttpClient<ToolsLocalService>();
            builder.Services.AddHttpClient<MqttServerService>();

            builder.Services.AddSingleton<ISystemCommandsService, SystemCommandsLocalService>();
            builder.Services.AddScoped<IParametersStorageService, ParametersStorageLocalService>();
            builder.Services.AddSingleton<IHardwaresService, HardwaresLocalService>();
            builder.Services.AddSingleton<IMqttBaseService, MqttServerService>();
            builder.Services.AddScoped<ICamerasService, FlashCamLocalService>();
            builder.Services.AddSingleton<IEmailService, EmailLocalService>();
            builder.Services.AddSingleton<IUsersService, UsersLocalService>();
            builder.Services.AddScoped<IToolsService, ToolsLocalService>();

            builder.Services.AddSingleton<ITelegramBotFormFillingServive, TelegramBotFormFillingServive>();
            builder.Services.AddSingleton<ITelegramBotHardwareViewServive, TelegramBotHardwareViewServive>();

            builder.Services.AddControllersWithViews()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.AddRazorPages();

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            builder.Host.UseSystemd();

            logger.Warn($"load configs: *.{_env.EnvironmentName}.json");
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddJsonFile($"appsettings.{_env.EnvironmentName}.json");

            builder.Services.AddSingleton<IServiceCollection, ServiceCollection>();

            // Secrets
            string secretPath = Path.Combine("..", "..", "secrets");
            FileInfo fi = new(secretPath);
            logger.Warn($"load secrets from {secretPath} ({fi.FullName}).");
            if (Directory.Exists(secretPath))
            {
                foreach (string secret in Directory.GetFiles(secretPath, $"*.json"))
                    logger.Warn($"exist secret: {secret}");

                foreach (string secret in Directory.GetFiles(secretPath, $"*.{_env.EnvironmentName}.json"))
                    builder.Configuration.AddJsonFile(Path.GetFullPath(secret), optional: true, reloadOnChange: true);
            }

            using ParametersContext _context = new();
            string _json_config_raw = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
            MqttConfigModel mqtt_settings = JsonConvert.DeserializeObject<MqttConfigModel>(_json_config_raw) ?? new();
            builder.Services.AddSingleton(x => mqtt_settings);

            MqttFactory mqttFactory = new();
            builder.Services.AddTransient(x => mqttFactory);
            IMqttClient mqttClient = mqttFactory.CreateMqttClient();
            builder.Services.AddSingleton(x => mqttClient);

            _json_config_raw = _context.GetStoredParameter(nameof(TelegramBotConfigModel), "").StoredValue;
            TelegramBotConfigModel tbot_settings = JsonConvert.DeserializeObject<TelegramBotConfigModel>(_json_config_raw) ?? new();

            if (tbot_settings.IsConfigured && tbot_settings.AutoStart && !string.IsNullOrEmpty(tbot_settings.TelegramBotToken))
            {
                builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    //BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    TelegramBotClientOptions options = new(tbot_settings.TelegramBotToken);
                    return new TelegramBotClient(options, httpClient);
                });

                builder.Services.AddScoped<UpdateHandler>();
                builder.Services.AddScoped<ReceiverService>();
                builder.Services.AddHostedService<PollingService>();
            }

            WebApplication app = builder.Build();

            if (mqtt_settings.AutoStart && mqtt_settings.IsConfigured)
            {
                IMqttBaseService _mqtt_cli_srv = app.Services.GetRequiredService<IMqttBaseService>();
                ResponseBaseModel res = await _mqtt_cli_srv.StartService();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
        catch (Exception exception)
        {
            // NLog: catch setup errors
            logger.Error(exception, "Stopped program because of exception");
            throw;
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
        }
    }

    static void CheckContext()
    {
        using ServerContext db = new();
#if DEMO
        db.DemoSeed();
#endif
        db.ReorderScriptsCommands();
    }
}
#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена