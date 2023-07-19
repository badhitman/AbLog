////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Net.NetworkInformation;
using System.Xml.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Versioning;

namespace SharedLib;

/// <summary>
/// global static
/// </summary>
public static class GlobalStatic
{
    static string PersonalFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ab-log-droid");

    /// <summary>
    /// Путь к папке с файлами базы данных
    /// </summary>
    public static string RootDatabasePathBase
    {
        get
        {
            string basePath = Path.Combine(PersonalFolderPath, "sqlite-db");
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }
    }

    /// <summary>
    /// Путь к папке логов MailKit
    /// </summary>
    public static string RootMailPathBase
    {
        get
        {
            string basePath = Path.Combine(PersonalFolderPath, "mail.kit");
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }
    }

    /// <summary>
    /// Путь к папке с файлами web службы
    /// </summary>
    public static string WebPathBase
    {
        get
        {
            string full_www_path = Path.Combine(PersonalFolderPath, "www");
            if (!Directory.Exists(full_www_path))
                Directory.CreateDirectory(full_www_path);

            return full_www_path;
        }
    }


    /// <summary>
    /// имя файла логов SMTP
    /// </summary>
    private const string LogsSmtpFilename = "smtp.logs";
    /// <summary>
    /// путь к файлу логов SMTP
    /// </summary>
    public static string LogsSmtpPath => Path.Combine(RootMailPathBase, LogsSmtpFilename);

    /// <summary>
    /// 
    /// </summary>
    public static string PefixDbFile = "";
    /// <summary>
    /// имя файла основной базы данны
    /// </summary>
    private static string MainDatabaseFilename => $"main-db{PefixDbFile}.sqlite";
    /// <summary>
    /// путь к файлу основной базы данных
    /// </summary>
    public static string MainDatabasePath => Path.Combine(RootDatabasePathBase, MainDatabaseFilename);

    /// <summary>
    /// имя файла базы данных хранения параметров
    /// </summary>
    private static string ParametersStorageDatabaseFilename => $"parameters-db{PefixDbFile}.sqlite";
    /// <summary>
    /// путь к файлу базы данных хранения параметров
    /// </summary>
    public static string ParametersStorageDatabasePath => Path.Combine(RootDatabasePathBase, ParametersStorageDatabaseFilename);

    /// <summary>
    /// IP-адрес локальной машины
    /// </summary>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("android")]
    public static IPAddress IpAddress => Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last();

    /// <summary>
    /// Текущие сетевые интерфесы (локальные)
    /// </summary>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("android")]
    public static IEnumerable<NetworkInterfaceModel> GetNetworkInterfaces()
    {
        NetworkInterfaceType[] filter = new NetworkInterfaceType[] { NetworkInterfaceType.Loopback, NetworkInterfaceType.Unknown };
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces()
            .Where(c => !filter.Contains(c.NetworkInterfaceType) && c.OperationalStatus == OperationalStatus.Up)
            .OrderBy(x => x.NetworkInterfaceType)
            .ToArray();

        if (!nics.Any())
            yield break;

        foreach (NetworkInterface e in nics)
        {
            IPInterfaceProperties props = e.GetIPProperties();
            // get first IPV4 address assigned to this interface
            IPAddress? firstIpV4Address = props.UnicastAddresses
                .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(c => c.Address)
                .FirstOrDefault();
            if (firstIpV4Address is null)
                yield break;

            yield return new(e, firstIpV4Address);
        }
    }
    /// <summary>
    /// Сетевой интерфейс (локальный)
    /// </summary>
    public record struct NetworkInterfaceModel(NetworkInterface Interface, IPAddress? Address);

    /// <summary>
    /// Преобразовать размер файла в человекочитаемы вид
    /// </summary>
    public static string SizeDataAsString(long SizeFile)
    {
        if (SizeFile < 1024)
            return SizeFile.ToString() + " bytes";
        else if (SizeFile < 1024 * 1024)
            return Math.Round((double)SizeFile / 1024, 2).ToString() + " KB";
        else if (SizeFile < 1024 * 1024 * 1024)
            return Math.Round((double)SizeFile / 1024 / 1024, 2).ToString() + " MB";
        else
            return Math.Round((double)SizeFile / 1024 / 1024 / 1024, 3).ToString() + " GB";
    }

    /// <summary>
    /// Имя парамтра хранения токена Telegram бота
    /// </summary>
    public const string TELEGRAM_TOKEN = "ab.action.TELEGRAM_TELEGRAM_TOKEN";

    /// <summary>
    /// Строковое представление перечисления
    /// </summary>
    public static string ToFriendlyString(this SecureSocketOptionsEnum me)
    {
        return me switch
        {
            SecureSocketOptionsEnum.None => "Без SSL/TLS",
            SecureSocketOptionsEnum.Auto => "Автоматически",
            SecureSocketOptionsEnum.SslOnConnect => "SSL/TLS",
            SecureSocketOptionsEnum.StartTls => "StartTLS",
            SecureSocketOptionsEnum.StartTlsWhenAvailable => "Если доступно StartTLS",
            SecureSocketOptionsEnum.SelfSignedSsl => "SSL/TLS без валидации",
            _ => "<ошибка>",
        };
    }

    /// <summary>
    /// Клон объекта (через сереализацию)
    /// </summary>
    public static T CreateDeepCopy<T>(T obj)
    {
        using MemoryStream ms = new();
        XmlSerializer serializer = new XmlSerializer(obj!.GetType());
        serializer.Serialize(ms, obj);
        ms.Seek(0, SeekOrigin.Begin);
        return (T)serializer.Deserialize(ms)!;
    }

    /// <summary>
    /// С заглавной буквы
    /// </summary>
    public static string UpperFirstChar(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return $"{char.ToUpper(input[0])}{input[1..]}";
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Routes
    {
        /// <summary>
        /// 
        /// </summary>
        public const string System = "system";
        /// <summary>
        /// 
        /// </summary>
        public const string Users = "users";
        /// <summary>
        /// 
        /// </summary>
        public const string Hardwares = "hardwares";
        /// <summary>
        /// 
        /// </summary>
        public const string Hardware = "hardware";
        /// <summary>
        /// 
        /// </summary>
        public const string Cameras = "cameras";
        /// <summary>
        /// 
        /// </summary>
        public const string Commands = "commands";
        /// <summary>
        /// 
        /// </summary>
        public const string Conditions = "conditions";
        /// <summary>
        /// 
        /// </summary>
        public const string Contentions = "contentions";
        /// <summary>
        /// 
        /// </summary>
        public const string Ports = "ports";
        /// <summary>
        /// 
        /// </summary>
        public const string Port = "port";
        /// <summary>
        /// 
        /// </summary>
        public const string Scripts = "scripts";
        /// <summary>
        /// 
        /// </summary>
        public const string Storage = "storage";
        /// <summary>
        /// 
        /// </summary>
        public const string Triggers = "triggers";
        /// <summary>
        /// 
        /// </summary>
        public const string Tools = "tools";
        /// <summary>
        /// 
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// 
        /// </summary>
        public const string Mqtt = "mqtt";

        /// <summary>
        /// 
        /// </summary>
        public const string SORTING = "sorting";
        /// <summary>
        /// 
        /// </summary>
        public const string ENABLE = "enable";
        /// <summary>
        /// 
        /// </summary>
        public const string DELETE = "delete";
        /// <summary>
        /// 
        /// </summary>
        public const string UPDATE = "update";
        /// <summary>
        /// 
        /// </summary>
        public const string LIST = "list";
        /// <summary>
        /// 
        /// </summary>
        public const string START = "start";
        /// <summary>
        /// 
        /// </summary>
        public const string STOP = "stop";
        /// <summary>
        /// 
        /// </summary>
        public const string STATUS = "status";
        /// <summary>
        /// 
        /// </summary>
        public const string BY_OWNER = "by-owner";
        /// <summary>
        /// 
        /// </summary>
        public const string ENTRIES = "entries";
        /// <summary>
        /// 
        /// </summary>
        public const string NESTED_ENTRIES = "nested-entries";

        /// <summary>
        /// html
        /// </summary>
        public const string HTML = "html";

        /// <summary>
        /// main
        /// </summary>
        public const string MAIN = "main";

        /// <summary>
        /// check
        /// </summary>
        public const string CHECK = "check";

        /// <summary>
        /// 
        /// </summary>
        public const string PUBLISH = "publish";

        /// <summary>
        /// check
        /// </summary>
        public const string GET = "get";

        /// <summary>
        /// Системный топик MQTT на который будут подписаны все
        /// </summary>
        public const string AB_LOG_SYSTEM = "ab-log-system";

        /// <summary>
        /// http
        /// </summary>
        public const string HTTP = "http";

        /// <summary>
        /// shot
        /// </summary>
        public const string SHOT = "shot";
        /// <summary>
        /// Telegram bot
        /// </summary>
        public const string TelegramBot = "telegram-bot";
    }

    #region forms maps

    /// <summary>
    /// 
    /// </summary>
    public static Dictionary<string, FormMapModel> FormsMaps => new()
    {
        { nameof(MqttConfigModel) , MqttConfigForm }
    };

    /// <summary>
    /// 
    /// </summary>
    static FormMapModel MqttConfigForm => new()
    {
        Name = "Конфигурация MQTT",
        Properties = new[]
        {
            new FormPropertyModel() { Code = nameof(MqttConfigModel.Username), Name = "Логин" },
            new FormPropertyModel() { Code = nameof(MqttConfigModel.Password), Name = "Пароль" },
            new FormPropertyModel() { Code = nameof(MqttConfigModel.Server), Name = "Сервер" },
            new FormPropertyModel() { Code = nameof(MqttConfigModel.Port), Name = "Порт" }
        }
    };

    #endregion
}