using System.Text.Json.Serialization;
using Newtonsoft.Json;
using MQTTnet.Client;
using ServicesLib;
using System.Text;
using ab.context;
using SharedLib;
using ServerLib;
using NLog.Web;
using MQTTnet;
using NLog;

namespace AbLogServer;

#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
public class Program
{
    public static async Task Main(string[] args)
    {
        Logger logger = LogManager
            .Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();

        logger.Warn("init main");
        CheckContext();
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            IWebHostEnvironment _env = builder.Environment;

            builder.Services.AddScoped<IParametersStorageService, ParametersStorageLocalService>();
            builder.Services.AddScoped<IHardwaresService, HardwaresLocalService>();
            builder.Services.AddScoped<ICamerasService, FlashCamLocalService>();

            builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddRazorPages();

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            builder.Host.UseSystemd();

            logger.Warn($"load configs: *.{_env.EnvironmentName}.json");
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddJsonFile($"appsettings.{_env.EnvironmentName}.json");

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

            using ServerContext _context = new();
            string _mqttConfig = _context.GetStoredParameter(nameof(MqttConfigModel), "").StoredValue;
            MqttConfigModel mqtt_settings = JsonConvert.DeserializeObject<MqttConfigModel>(_mqttConfig) ?? new();
            builder.Services.AddSingleton(x => mqtt_settings);
            MqttFactory mqttFactory = new();
            builder.Services.AddTransient(x => mqttFactory);
            IMqttClient mqttClient = mqttFactory.CreateMqttClient();
            builder.Services.AddSingleton(x => mqttClient);

            if (mqtt_settings.AutoStart && mqtt_settings.IsConfigured)
            {
                try
                {
                    // Create TCP based options using the builder.
                    MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTls()
                    .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                    .WithClientId(mqtt_settings.ClientId)
                    .WithTcpServer(mqtt_settings.Server, mqtt_settings.Port)
                    .WithCredentials(mqtt_settings.Username, mqtt_settings.Password)
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                    .Build();

                    mqttClient.DisconnectedAsync += async e =>
                    {
                        logger.Warn($"mqttClient.DisconnectedAsync => ClientWasConnected:{e.ClientWasConnected}");
                        if (e.ClientWasConnected)
                            await mqttClient.ConnectAsync(mqttClient.Options);
                    };
                    mqttClient.ApplicationMessageReceivedAsync += e =>
                    {
                        logger.Info($"client:{e.ClientId}; topic:{e.ApplicationMessage.Topic};{e.ApplicationMessage.UserProperties}");
                        if (e.ApplicationMessage.PayloadSegment.Array is not null)
                            logger.Info(Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.Array));

                        return Task.CompletedTask;
                    };

                    MqttClientConnectResult ccr = await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                    MqttClientSubscribeOptions mqttSubscribeOptions = mqttFactory
                        .CreateSubscribeOptionsBuilder()
                        .WithTopicFilter(f => { f.WithTopic(mqtt_settings.Topic); })
                        .Build();

                    MqttClientSubscribeResult csr = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                    MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(mqtt_settings.Topic)
                    .WithUserProperty("test_prop_name", "test_prop_value")
                    .WithPayload("19.5")
                    .Build();

                    MqttClientPublishResult cpr = await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                    builder.Services.AddSingleton(mqttClient);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }

            WebApplication app = builder.Build();

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