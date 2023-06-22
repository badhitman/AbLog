using System.Text.Json.Serialization;
using SharedLib.IServices;
using MQTTnet.Client;
using ServicesLib;
using System.Text;
using ab.context;
using SharedLib;
using ServerLib;
using NLog.Web;
using MQTTnet;
using NLog;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace AbLogServer
{
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
                MqttFactory mqttFactory = new();
                builder.Services.AddScoped(x => mqttFactory);

                if (mqtt_settings.AutoStart)
                {
                    IMqttClient mqttClient = mqttFactory.CreateMqttClient();

                    // Create TCP based options using the builder.
                    MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTls()
                    .WithClientId(mqtt_settings.ClientId)
                    .WithTcpServer(mqtt_settings.Server, mqtt_settings.Port)
                    .WithCredentials(mqtt_settings.Username, mqtt_settings.Password)
                    .WithCleanSession()
                    .Build();

                    mqttClient.DisconnectedAsync += async e =>
                    {
                        logger.Warn($"mqttClient.DisconnectedAsync => ClientWasConnected:{e.ClientWasConnected}");
                        if (e.ClientWasConnected)
                        {
                            // Use the current options as the new options.
                            await mqttClient.ConnectAsync(mqttClient.Options);
                        }
                    };
                    mqttClient.ApplicationMessageReceivedAsync += e =>
                    {
                        logger.Info($"client:{e.ClientId}; topic:{e.ApplicationMessage.Topic};");
                        if (e.ApplicationMessage.PayloadSegment.Array is not null)
                            logger.Info(Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.Array));
                        return Task.CompletedTask;
                    };
                    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                    MqttClientSubscribeOptions mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic("samples");
                        })
                    .Build();

                    await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                    //var applicationMessage = new MqttApplicationMessageBuilder()
                    //    .WithTopic("samples")
                    //    .WithPayload("19.5")
                    //    .Build();

                    //await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                    //await mqttClient.DisconnectAsync();
                    builder.Services.AddSingleton(mqttClient);
                }


                var app = builder.Build();

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
}