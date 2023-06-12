using Refit;
using SharedLib;

namespace BlazorLib
{
    /// <summary>
    /// Работа с бэком
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface IRefitService
    {
        #region HARDWARE`s +PORT`s

        /// <summary>
        /// Получить все устройства (с портами)
        /// </summary>
        /// <returns>Все устройства (с портами)</returns>
        [Get($"/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.LIST}")]
        public Task<ApiResponse<HardwaresResponseModel>> HardwaresGetAll();

        /// <summary>
        /// Получить все устройства (данные в лёгкой форме)
        /// </summary>
        /// <returns>Все устройства (данные в лёгкой форме)</returns>
        [Get($"/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.ENTRIES}")]
        public Task<ApiResponse<EntriesResponseModel>> HardwaresGetAllAsEntries();

        /// <summary>
        /// Получить все устройства с портами
        /// </summary>
        [Get($"/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.NESTED_ENTRIES}")]
        public Task<ApiResponse<EntriesNestedResponseModel>> HardwaresGetTreeNestedEntries();

        /// <summary>
        /// Получить информацию об устройстве
        /// </summary>
        [Get($"/{GlobalStatic.HttpRoutes.Hardwares}/{{hardware_id}}")]
        public Task<ApiResponse<HardwareResponseModel>> HardwareGet(int hardware_id);

        /// <summary>
        /// Получить порт устройства
        /// </summary>
        [Get($"/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.Ports}/{{port_id}}")]
        public Task<ApiResponse<PortHardwareResponseModel>> HardwarePortGet(int port_id);

        #endregion

        #region SCRIPT`s

        /// <summary>
        /// Получить все скрипты
        /// </summary>
        /// <returns>Все скрипты</returns>
        [Get($"/{GlobalStatic.HttpRoutes.Scripts}/{GlobalStatic.HttpRoutes.LIST}")]
        public Task<ApiResponse<ScriptsResponseModel>> ScriptsGetAll();

        /// <summary>
        /// Удалить скрипт
        /// </summary>
        /// <param name="script_id">Идентификатор срипта</param>
        /// <returns>Актуальный перечень скриптов (после удаления)</returns>
        [Delete($"/{GlobalStatic.HttpRoutes.Scripts}/{{script_id}}")]
        public Task<ApiResponse<ScriptsResponseModel>> ScriptDelete(int script_id);

        /// <summary>
        /// Обновить/создать скрипт
        /// </summary>
        /// <param name="script">Скрипт для создания</param>
        /// <returns>Актуальный перечень скриптов (после обновления)</returns>
        [Post($"/{GlobalStatic.HttpRoutes.Scripts}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<ScriptsResponseModel>> ScriptUpdateOrCreate(EntryDescriptionModel script);

        /// <summary>
        /// Установить скрипту признак включения/отключения
        /// </summary>
        [Put($"/{GlobalStatic.HttpRoutes.Scripts}/{GlobalStatic.HttpRoutes.ENABLE}/{{script_id}}")]
        public Task<ApiResponse<ResponseBaseModel>> ScriptEnableSet([AliasAs("script_id")] int script_id, bool is_enable);

        #endregion

        #region COMMAND`s

        /// <summary>
        /// Получить команды скрипта
        /// </summary>
        /// <param name="script_id">Идентификатор скрипта</param>
        /// <returns>Команды скрипта</returns>
        [Get($"/{GlobalStatic.HttpRoutes.Commands}/{GlobalStatic.HttpRoutes.BY_OWNER}/{{script_id}}")]
        public Task<ApiResponse<EntriesSortingResponseModel>> GetCommandsEntriesByScript(int script_id);

        /// <summary>
        /// Получить команду
        /// </summary>
        /// <param name="command_id">Идентификатор команды</param>
        [Get($"/{GlobalStatic.HttpRoutes.Commands}/{{command_id}}")]
        public Task<ApiResponse<CommandResponseModel>> CommandGet(int command_id);

        /// <summary>
        /// Обновитьсоздать команду
        /// </summary>
        /// <param name="command">Команда обновления</param>
        [Post($"/{GlobalStatic.HttpRoutes.Commands}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<ResponseBaseModel>> CommandUpdateOrCreate(CommandModelDB command);

        /// <summary>
        /// Установить сортировку элементов путём обмена их местами между собой (объекты обмениваются значениями [sorting])
        /// </summary>
        /// <param name="commands_for_move">Идентификаторы команд, которые нужно поменять месстами в сортировке</param>
        /// <returns>Новый (актуальный) перечень команд</returns>
        [Put($"/{GlobalStatic.HttpRoutes.Commands}/{GlobalStatic.HttpRoutes.SORTING}")]
        public Task<ApiResponse<EntriesSortingResponseModel>> CommandSortingSet(IdsPairModel commands_for_move);

        /// <summary>
        /// Удалить команду
        /// </summary>
        /// <param name="id_command">Идентификатор команды для удаления</param>
        /// <returns></returns>
        [Delete($"/{GlobalStatic.HttpRoutes.Commands}/{{id_command}}")]
        public Task<ApiResponse<ResponseBaseModel>> CommandDelete(int id_command);

        #endregion

        #region CONDITION`s

        /// <summary>
        /// Получить условия/ограничения по владельцу (тригшер или команда)
        /// </summary>
        /// <returns>Объект ограничений/условий</returns>
        [Get($"/{GlobalStatic.HttpRoutes.Conditions}/{{owner_id}}/{{condition_type}}")]
        public Task<ApiResponse<ConditionsAnonimResponseModel>> ConditionsGetByOwner([AliasAs("owner_id")] int condition_id, [AliasAs("condition_type")] ConditionsTypesEnum condition_type);

        /// <summary>
        /// Обновить объект условия/ограничения в БД
        /// </summary>
        /// <param name="req">Объект обновления условия/ограничения</param>
        /// <returns>Полный (обновлённый) перечень условий контекстного типа (например для тригера, команды и т.п.)</returns>
        [Post($"/{GlobalStatic.HttpRoutes.Conditions}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<ConditionsAnonimResponseModel>> ConditionUpdateOrCreate(UpdateConditionRequestModel req);

        /// <summary>
        /// Удалить объект условия/ограничения
        /// </summary>
        /// <returns>Полный (обновлённый) перечень условий (после удаления) контекстного типа (например для тригера, команды и т.п.)</returns>
        [Delete($"/{GlobalStatic.HttpRoutes.Conditions}/{{condition_id}}/{{condition_type}}")]
        public Task<ApiResponse<ConditionsAnonimResponseModel>> ConditionDelete([AliasAs("condition_id")] int condition_id, [AliasAs("condition_type")] ConditionsTypesEnum condition_type);

        #endregion

        #region CONTENTION`s (конкуренция/взаимоблокировка выполнения скриптов)

        /// <summary>
        /// Получить идентификаторы скриптов, которым запрещён автозапуск во время выполенния  конкурирующего скрипта script_id (ScriptMasterId)
        /// </summary>
        /// <param name="script_id">script_id/ScriptMasterId</param>
        /// <returns>Перечень Id скриптов, которым запрещён автозапуск во время выполенния конкурирующего скрипта script_id (ScriptMasterId)</returns>
        [Get($"/{GlobalStatic.HttpRoutes.Contentions}/{{script_id}}")]
        public Task<ApiResponse<IdsResponseModel>> ContentionsGetByScript(int script_id);

        /// <summary>
        /// Установить признак конкурирующей взаимоблокировки
        /// </summary>
        /// <param name="contention_upd">Запрос</param>
        /// <returns>Перечень Id скриптов, которым запрещён автозапуск во время выполенния конкурирующего скрипта ScriptMasterId</returns>
        [Post($"/{GlobalStatic.HttpRoutes.Contentions}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<IdsResponseModel>> ContentionSet(UpdateContentionRequestModel contention_upd);

        #endregion

        #region TRIGGER`s

        /// <summary>
        /// Получить все тригеры
        /// </summary>
        [Get($"/{GlobalStatic.HttpRoutes.Triggers}/{GlobalStatic.HttpRoutes.LIST}")]
        public Task<ApiResponse<TriggersResponseModel>> TriggersGetAll();

        /// <summary>
        /// Обновить тригер
        /// </summary>
        /// <param name="trigger">Тригер обновления</param>
        /// <returns>Все тригеры</returns>
        [Post($"/{GlobalStatic.HttpRoutes.Triggers}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<TriggersResponseModel>> TriggerUpdateOrCreate(TrigerModelDB trigger);

        /// <summary>
        /// Удалить тригер
        /// </summary>
        /// <param name="trigger_id">Идентификатор тригшера для удаления</param>
        /// <returns>Все тригеры</returns>
        [Delete($"/{GlobalStatic.HttpRoutes.Triggers}/{{trigger_id}}")]
        public Task<ApiResponse<TriggersResponseModel>> TriggerDelete(int trigger_id);

        #endregion

        #region PARAMETER`s STORAGE

        /// <summary>
        /// Получить конфигурацию Email (imap+smtp)
        /// </summary>
        [Get($"/{GlobalStatic.HttpRoutes.GET_EMAIL_CONFIG}")]
        public Task<ApiResponse<EmailConfigModel>> EmailConfigGet();

        /// <summary>
        /// Сохранить конфигурацию Email (imap+smtp)
        /// </summary>
        [Post($"/{GlobalStatic.HttpRoutes.SAVE_EMAIL_CONFIG}")]
        public Task<ApiResponse<ResponseBaseModel>> EmailConfigSave(EmailConfigModel email_conf);

        /// <summary>
        /// Проверить подключение к Email (конфигурация imap+smtp)
        /// </summary>
        [Post($"/{GlobalStatic.HttpRoutes.TEST_CONNECTION_EMAIL_CONFIG}")]
        public Task<ApiResponse<ResponseBaseModel>> EmailConfigTestSmtpConnection(EmailConfigModel? email_conf);

        #endregion
    }
}