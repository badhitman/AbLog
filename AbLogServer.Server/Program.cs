using System.Text.Json.Serialization;
using SharedLib.IServices;
using ServicesLib;
using ab.context;
using SharedLib;
using ServerLib;
using NLog.Web;
using NLog;

namespace AbLogServer
{
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    public class Program
    {
        public static void Main(string[] args)
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

                builder.Services.AddScoped<IParametersStorageService, ParametersStorageLocalSQLiteService>();
                builder.Services.AddScoped<IHardwaresService, HardwaresLocalSQLiteService>();
                builder.Services.AddScoped<ICamerasService, FlashCamLocalService>();

                builder.Services.AddControllersWithViews()
                    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
                builder.Services.AddRazorPages();

                builder.Logging.ClearProviders();
                builder.Host.UseNLog();

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