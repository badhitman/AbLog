////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.Net.NetworkInformation;
using System.Xml.Serialization;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Versioning;

namespace SharedLib
{
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
        /// имя файла базы данных логов
        /// </summary>
        private const string LogsDatabaseFilename = "logs.sqlite";
        /// <summary>
        /// путь к файлу базы данных логов
        /// </summary>
        public static string LogsDatabasePath => Path.Combine(RootDatabasePathBase, LogsDatabaseFilename);


        /// <summary>
        /// имя файла логов SMTP
        /// </summary>
        private const string LogsSmtpFilename = "smtp.logs";
        /// <summary>
        /// путь к файлу логов SMTP
        /// </summary>
        public static string LogsSmtpPath => Path.Combine(RootMailPathBase, LogsSmtpFilename);


        /// <summary>
        /// имя файла логов IMAP
        /// </summary>
        private const string LogsImapFilename = "imap.logs";
        /// <summary>
        /// путь к файлу логов IMAP
        /// </summary>
        public static string LogsImapPath => Path.Combine(RootMailPathBase, LogsImapFilename);


        /// <summary>
        /// имя файла основной базы данны
        /// </summary>
        private const string MainDatabaseFilename = "server.sqlite";
        /// <summary>
        /// путь к файлу основной базы данных
        /// </summary>
        public static string MainDatabasePath => Path.Combine(RootDatabasePathBase, MainDatabaseFilename);

        /// <summary>
        /// имя файла базы данных пользователей и их сообщений
        /// </summary>
        private const string UsersDatabaseFilename = "users.sqlite";
        /// <summary>
        /// путь к файлу базы данных пользователей и их сообщений
        /// </summary>
        public static string UsersDatabasePath => Path.Combine(RootDatabasePathBase, UsersDatabaseFilename);

        /// <summary>
        /// имя файла базы данных хранения параметров
        /// </summary>
        private const string ParametersStorageDatabaseFilename = "parameters.sqlite";
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
            else
                return Math.Round((double)SizeFile / 1024 / 1024, 2).ToString() + " MB";
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
        /// 
        /// </summary>
        public static class HttpRoutes
        {
            /// <summary>
            /// 
            /// </summary>
            public const string Hardwares = "hardwares";

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
            public const string LIST = "list";

            /// <summary>
            /// 
            /// </summary>
            public const string ENTRIES = "entries";

            /// <summary>
            /// 
            /// </summary>
            public const string NESTED_ENTRIES = "nested-entries";

            #region HARDWARE

            /*/// <summary>
            /// Запрос всех устройств
            /// </summary>
            public const string GET_ALL_HARDWARES = "get-all-hardwares";*/
            //
            /// <summary>
            /// Запрос всех устройств в виде лёгкого перечня Enumerable&gt;EntryModel&lt; {int:id, string:name}
            /// </summary>
            public const string GET_ALL_HARDWARES_ENTRIES = "get-all-hardwares-entries";

            /// <summary>
            /// Получить устройство
            /// </summary>
            public const string GET_HARDWARE = "get-hardware";

            /// <summary>
            /// Получить устройства с портами
            /// </summary>
            public const string GET_TREE_HARDWARES_NESTED_ENTRIES = "get-tree-hardwares-nested-entries";
            //public const string UPDATE_HARDWARE = "update-hardware";
            //public const string DELETE_HARDWARE = "delete-hardware";

            #endregion

            #region PORT

            /// <summary>
            /// Получить порт устройтва
            /// </summary>
            public const string GET_PORT_HARDWARE = "get-port-hardware";

            #endregion

            #region SCRIPT

            /// <summary>
            /// Запрос всех скриптов
            /// </summary>
            public const string GET_ALL_SCRIPTS = "get-all-scripts";

            /// <summary>
            /// Обновить/создать скрипт
            /// </summary>
            public const string UPDATE_SCRIPT = "update-script";

            /// <summary>
            /// Удалить скрипт
            /// </summary>
            public const string DELETE_SCRIPT = "delete-script";

            /// <summary>
            /// Включить/выключить скрипт
            /// </summary>
            public const string SET_SCRIPT_ENABLE = "set-script-enable";
            //public const string GET_SCRIPT = "get-script";

            #endregion

            #region CONDITION

            /// <summary>
            /// Обновить условие/ограничение
            /// </summary>
            public const string UPDATE_CONDITION = "update-condition";

            /// <summary>
            /// Удалить условие/ограничение
            /// </summary>
            public const string DELETE_CONDITION = "delete-condition";

            /// <summary>
            /// Получить условия/ограничения
            /// </summary>
            public const string GET_CONDITIONS = "get-conditions";

            #endregion

            #region COMMAND

            /// <summary>
            /// Получить команду
            /// </summary>
            public const string GET_COMMAND = "get-command";

            /// <summary>
            /// Получить команды по ид скрипта (лёгкая модель ответа [Entry])
            /// </summary>
            public const string GET_COMMANDS_ENTRIES_BY_SCRIPT_ID = "get-commands-entries-by-script-id";

            /// <summary>
            /// Обновить команду
            /// </summary>
            public const string UPDATE_COMMAND = "update-command";

            /// <summary>
            /// Установить команде позицию в сортировке
            /// </summary>
            public const string SET_COMMAND_SORTING = "set-command-sorting";

            /// <summary>
            /// Удалить команду
            /// </summary>
            public const string DELETE_COMMAND = "delete-command";

            #endregion

            #region CONTENTIONS (конкуренция/взаимоблокировка выполнения скриптов)

            /// <summary>
            /// Получить настройки режима конкуренции выполнения скриптов
            /// </summary>
            public const string GET_CONTENTIONS = "get-contentions";

            /// <summary>
            /// Обновить объект настройки режима конкуренции выполнения скриптов
            /// </summary>
            public const string UPD_CONTENTION = "upd-contention";

            #endregion

            #region TRIGGERS

            /// <summary>
            /// Получить тригеры
            /// </summary>
            public const string GET_TRIGGERS = "get-triggers";

            /// <summary>
            /// Обновить тригер
            /// </summary>
            public const string UPD_TRIGGER = "upd-trigger";

            /// <summary>
            /// Удалить тригер
            /// </summary>
            public const string DEL_TRIGGER = "del-trigger";
            #endregion

            #region ParametersStorage

            /// <summary>
            /// Получить конфигурацию Email (imap+smtp)
            /// </summary>
            public const string GET_EMAIL_CONFIG = "get-email-config";

            /// <summary>
            /// Сохранить конфигурацию Email (imap+smtp)
            /// </summary>
            public const string SAVE_EMAIL_CONFIG = "save-email-config";

            /// <summary>
            /// Проверить подключение к Email (конфигурация imap+smtp)
            /// </summary>
            public const string TEST_CONNECTION_EMAIL_CONFIG = "test-email-connection-config";

            #endregion
        }

        /// <summary>
        /// Команды (имена вложенных файлов)
        /// </summary>
        public static class Commands
        {
            /// <summary>
            /// Префикс имени файла ЗАПРОСА
            /// </summary>
            public const string REQUEST_PREFIX = "request-";

            /// <summary>
            /// Префикс имени файла РЕЗУЛЬТАТА обработки запроса
            /// </summary>
            public const string RESULT_PREFIX = "result-";



            /// <summary>
            /// Запрос: HTTP команда
            /// </summary>
            public const string REQUEST_HTTP_COMMAND = $"{REQUEST_PREFIX}http-command.json";

            /// <summary>
            /// Ответ: на HTTP команду
            /// </summary>
            public const string RESULT_HTTP_COMMAND = $"{RESULT_PREFIX}http-command.json";

            /// <summary>
            /// Запрос: перечень доступных камер
            /// </summary>
            public const string REQUEST_AVAILABLE_CAMERAS_COMMAND = $"{REQUEST_PREFIX}available-cameras.json";

            /// <summary>
            /// Ответ: перечень доступных камер
            /// </summary>
            public const string RESULT_AVAILABLE_CAMERAS_COMMAND = $"{RESULT_PREFIX}available-cameras.json";

            /// <summary>
            /// Запрос: фотография с камеры
            /// </summary>
            public const string REQUEST_SHOT_CAMERA_COMMAND = $"{REQUEST_PREFIX}one-shot-camera.json";

            /// <summary>
            /// Ответ: фотография с камеры
            /// </summary>
            public const string RESULT_SHOT_CAMERA_COMMAND = $"{RESULT_PREFIX}one-shot-camera.json";
        }
    }
}