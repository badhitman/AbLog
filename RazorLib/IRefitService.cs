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
        #region HARDWARES

        /// <summary>
        /// Получить все устройства (с портами)
        /// </summary>
        /// <returns>Все устройства (с портами)</returns>
        [Get($"/api/{GlobalStatic.HttpRoutes.Hardwares}/{GlobalStatic.HttpRoutes.ALL}")]
        public Task<ApiResponse<HardwaresResponseModel>> GetAllHardwares();

        /// <summary>
        /// Получить информацию об устройстве
        /// </summary>
        /// <param name="hw_id"></param>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_HARDWARE}")]
        public Task<ApiResponse<HardwareResponseModel>> GetHardware(int hw_id);

        /// <summary>
        /// Получить все устройства (данные в лёгкой форме)
        /// </summary>
        /// <returns>Все устройства (данные в лёгкой форме)</returns>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_ALL_HARDWARES_ENTRIES}")]
        public Task<ApiResponse<EntriesResponseModel>> GetAllHardwaresEntries();

        /// <summary>
        /// Получить все устройства с портами
        /// </summary>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_TREE_HARDWARES_NESTED_ENTRIES}")]
        public Task<ApiResponse<EntriesNestedResponseModel>> GetTreeHardwaresNestedEntries();

        #endregion

        #region PORT (for hw)

        /// <summary>
        /// Получить порт устройства
        /// </summary>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_PORT_HARDWARE}")]
        public Task<ApiResponse<PortHardwareResponseModel>> GetPortHW(int port_id);

        #endregion

        #region SCRIPT

        /// <summary>
        /// Получить все скрипты
        /// </summary>
        /// <returns>Все скрипты</returns>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_ALL_SCRIPTS}")]
        public Task<ApiResponse<ScriptsResponseModel>> GetAllScripts();

        /// <summary>
        /// Обновить/создать скрипт
        /// </summary>
        /// <param name="script">Скрипт для создания</param>
        /// <returns>Актуальный перечень скриптов (после обновления)</returns>
        [Post($"/api/{GlobalStatic.HttpRoutes.UPDATE_SCRIPT}")]
        public Task<ApiResponse<ScriptsResponseModel>> UpdateScript(EntryDescriptionModel script);

        /// <summary>
        /// Удалить скрипт
        /// </summary>
        /// <param name="script_id">Идентификатор срипта</param>
        /// <returns>Актуальный перечень скриптов (после удаления)</returns>
        [Delete($"/api/{GlobalStatic.HttpRoutes.DELETE_SCRIPT}")]
        public Task<ApiResponse<ScriptsResponseModel>> DeleteScript(int script_id);

        /// <summary>
        /// Установить скрипту признак включения/отключения
        /// </summary>
        /// <param name="script_id">Идентификатор скрипта</param>
        /// <param name="set_enable">Признак включения</param>
        /// <returns></returns>
        [Get($"/api/{GlobalStatic.HttpRoutes.SET_SCRIPT_ENABLE}")]
        public Task<ApiResponse<ResponseBaseModel>> SetScriptEnable(int script_id, bool set_enable);

        #endregion

        #region COMMAND

        /// <summary>
        /// Получить команды скрипта
        /// </summary>
        /// <param name="script_id">Идентификатор скрипта</param>
        /// <returns>Команды скрипта</returns>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_COMMANDS_ENTRIES_BY_SCRIPT_ID}")]
        public Task<ApiResponse<EntriesSortingResponseModel>> GetCommandsEntriesByScript(int script_id);

        /// <summary>
        /// Получить команду
        /// </summary>
        /// <param name="command_id">Идентификатор команды</param>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_COMMAND}")]
        public Task<ApiResponse<CommandResponseModel>> GetCommand(int command_id);

        /// <summary>
        /// Обновить команду
        /// </summary>
        /// <param name="command">Команда обновления</param>
        [Post($"/api/{GlobalStatic.HttpRoutes.UPDATE_COMMAND}")]
        public Task<ApiResponse<ResponseBaseModel>> UpdateCommand(CommandModelDB command);

        /// <summary>
        /// Установить сортировку элементов путём обмена их местами между собой (объекты обмениваются значениями [sorting])
        /// </summary>
        /// <param name="commands_for_move">Идентификаторы команд, которые нужно поменять месстами в сортировке</param>
        /// <returns>Новый (актуальный) перечень команд</returns>
        [Post($"/api/{GlobalStatic.HttpRoutes.SET_COMMAND_SORTING}")]
        public Task<ApiResponse<EntriesSortingResponseModel>> SetCommandSorting(IdsPairModel commands_for_move);

        /// <summary>
        /// Удалить команду
        /// </summary>
        /// <param name="id_command">Идентификатор команды для удаления</param>
        /// <returns></returns>
        [Delete($"/api/{GlobalStatic.HttpRoutes.DELETE_COMMAND}/{{id_command}}")]
        public Task<ApiResponse<ResponseBaseModel>> DeleteCommand(int id_command);

        #endregion

        #region CONDITIONS

        /// <summary>
        /// Получить объект условия/ограничения
        /// </summary>
        /// <param name="req">Тип ограничения (триггер, скрипт и т.п.) и его идентификатор</param>
        /// <returns>Объект ограничения/условия</returns>
        [Patch($"/api/{GlobalStatic.HttpRoutes.GET_CONDITIONS}")]
        public Task<ApiResponse<ConditionsAnonimResponseModel>> GetConditions(ConditionRequestModel req);

        /// <summary>
        /// Обновить объект условия/ограничения в БД
        /// </summary>
        /// <param name="req">Объект обновления условия/ограничения</param>
        /// <returns>Полный (обновлённый) перечень условий контекстного типа (например для тригера, команды и т.п.)</returns>
        [Post($"/api/{GlobalStatic.HttpRoutes.UPDATE_CONDITION}")]
        public Task<ApiResponse<ConditionsAnonimResponseModel>> UpdateCondition(UpdateConditionRequestModel req);

        /// <summary>
        /// Удалить объект условия/ограничения
        /// </summary>
        /// <param name="req">Идентификатор условия (и его тип)</param>
        /// <returns>Полный (обновлённый) перечень условий (после удаления) контекстного типа (например для тригера, команды и т.п.)</returns>
        [Post($"/api/{GlobalStatic.HttpRoutes.DELETE_CONDITION}")]
        public Task<ApiResponse<ConditionsAnonimResponseModel>> DeleteCondition(ConditionRequestModel req);

        #endregion

        #region CONTENTION (конкуренция/взаимоблокировка выполнения скриптов)

        /// <summary>
        /// Получить идентификаторы скриптов, которым запрещён автозапуск во время выполенния  конкурирующего скрипта script_id (ScriptMasterId)
        /// </summary>
        /// <param name="script_id">script_id/ScriptMasterId</param>
        /// <returns>Перечень Id скриптов, которым запрещён автозапуск во время выполенния конкурирующего скрипта script_id (ScriptMasterId)</returns>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_CONTENTIONS}")]
        public Task<ApiResponse<IdsResponseModel>> GetContentions(int script_id);

        /// <summary>
        /// Установить признак конкурирующей взаимоблокировки
        /// </summary>
        /// <param name="contention_upd">Запрос</param>
        /// <returns>Перечень Id скриптов, которым запрещён автозапуск во время выполенния конкурирующего скрипта ScriptMasterId</returns>
        [Post($"/api/{GlobalStatic.HttpRoutes.UPD_CONTENTION}")]
        public Task<ApiResponse<IdsResponseModel>> SetContention(UpdateContentionRequestModel contention_upd);

        #endregion

        #region TRIGGERS

        /// <summary>
        /// Получить все тригеры
        /// </summary>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_TRIGGERS}")]
        public Task<ApiResponse<TriggersResponseModel>> GetAllTriggers();

        /// <summary>
        /// Обновить тригер
        /// </summary>
        /// <param name="trigger">Тригер обновления</param>
        /// <returns>Все тригеры</returns>
        [Post($"/api/{GlobalStatic.HttpRoutes.UPD_TRIGGER}")]
        public Task<ApiResponse<TriggersResponseModel>> UpdateTrigger(TrigerModelDB trigger);

        /// <summary>
        /// Удалить тригер
        /// </summary>
        /// <param name="trigger_id">Идентификатор тригшера для удаления</param>
        /// <returns>Все тригеры</returns>
        [Delete($"/api/{GlobalStatic.HttpRoutes.DEL_TRIGGER}")]
        public Task<ApiResponse<TriggersResponseModel>> DeleteTrigger(int trigger_id);

        #endregion

        #region PARAMETERS STORAGE

        /// <summary>
        /// Получить конфигурацию Email (imap+smtp)
        /// </summary>
        [Get($"/api/{GlobalStatic.HttpRoutes.GET_EMAIL_CONFIG}")]
        public Task<ApiResponse<EmailConfigModel>> GetEmailConfig();

        /// <summary>
        /// Сохранить конфигурацию Email (imap+smtp)
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.SAVE_EMAIL_CONFIG}")]
        public Task<ApiResponse<ResponseBaseModel>> SaveEmailConfig(EmailConfigModel email_conf);

        /// <summary>
        /// Проверить подключение к Email (конфигурация imap+smtp)
        /// </summary>
        [Post($"/api/{GlobalStatic.HttpRoutes.TEST_CONNECTION_EMAIL_CONFIG}")]
        public Task<ApiResponse<ResponseBaseModel>> TestEmailConfigSmtpConnection(EmailConfigModel? email_conf);

        #endregion
    }
}