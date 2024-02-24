using ab.context;
using ABLogWeb.Components;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using MQTTnet.Client;
using MudBlazor.Services;
using Newtonsoft.Json;
using NLog;
using NLog.Web;
using ServerLib;
using SharedLib;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using Telegram.Bot;
using Telegram.Bot.Services;

namespace ABLogWeb;

/// <summary>
/// Program
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("android")]
public class Program
{
    /// <summary>
    /// Main
    /// </summary>
    public static async Task Main(string[] args)
    {
        Logger logger = LogManager
                    .Setup()
                    .LoadConfigurationFromAppSettings()
                    .GetCurrentClassLogger();

        GlobalStatic.PefixDbFile = "-server";
        logger.Warn("init main");
        logger.Warn($"main-db: {GlobalStatic.MainDatabasePath}");
        logger.Warn($"storage-db: {GlobalStatic.ParametersStorageDatabasePath}");
        CheckContext();

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IWebHostEnvironment _env = builder.Environment;
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        //builder.Host.UseSystemd();

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
        builder.Services.AddMudServices();

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

        builder.Services.AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddDbContextFactory<ServerContext>(opt =>
            opt.UseSqlite($"Data Source={GlobalStatic.MainDatabasePath}"));

        builder.Services.AddDbContextFactory<ParametersContext>(opt =>
            opt.UseSqlite($"Data Source={GlobalStatic.ParametersStorageDatabasePath}"));

        MqttFactory mqttFactory = new();
        builder.Services.AddTransient(x => mqttFactory);
        IMqttClient mqttClient = mqttFactory.CreateMqttClient();
        builder.Services.AddSingleton(x => mqttClient);

        MqttConfigModel mqtt_settings = new();
        builder.Services.AddSingleton(x => mqtt_settings);

        WebApplication app = builder.Build();

        IDbContextFactory<ParametersContext> db_factory = app.Services.GetRequiredService<IDbContextFactory<ParametersContext>>();
        using ParametersContext _context = db_factory.CreateDbContext();

        mqtt_settings.LoadConfigFromJson(_context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue);

        string _json_config_raw = _context.GetStoredParameter(nameof(TelegramBotConfigModel), "").StoredValue;
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

        await using AsyncServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
        DbContextOptions<ServerContext> options = scope.ServiceProvider.GetRequiredService<DbContextOptions<ServerContext>>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
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