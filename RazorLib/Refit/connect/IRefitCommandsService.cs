using SharedLib;
using Refit;

namespace BlazorLib
{
    /// <summary>
    /// Работа с бэком: Hardwares
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface IRefitCommandsService
    {
        /// <summary>
        /// Получить команды скрипта
        /// </summary>
        /// <param name="script_id">Идентификатор скрипта</param>
        /// <returns>Команды скрипта</returns>
        [Get($"/api/{GlobalStatic.HttpRoutes.Commands}/{GlobalStatic.HttpRoutes.BY_OWNER}/{{script_id}}")]
        public Task<ApiResponse<EntriesSortingResponseModel>> GetCommandsEntriesByScript(int script_id);

        /// <summary>
        /// Получить команду
        /// </summary>
        /// <param name="command_id">Идентификатор команды</param>
        [Get($"/api/{GlobalStatic.HttpRoutes.Commands}/{{command_id}}")]
        public Task<ApiResponse<CommandResponseModel>> CommandGet(int command_id);

        /// <summary>
        /// Обновитьсоздать команду
        /// </summary>
        /// <param name="command">Команда обновления</param>
        [Post($"/api/{GlobalStatic.HttpRoutes.Commands}/{GlobalStatic.HttpRoutes.UPDATE}")]
        public Task<ApiResponse<ResponseBaseModel>> CommandUpdateOrCreate(CommandModelDB command);

        /// <summary>
        /// Установить сортировку элементов путём обмена их местами между собой (объекты обмениваются значениями [sorting])
        /// </summary>
        /// <param name="commands_for_move">Идентификаторы команд, которые нужно поменять месстами в сортировке</param>
        /// <returns>Новый (актуальный) перечень команд</returns>
        [Put($"/api/{GlobalStatic.HttpRoutes.Commands}/{GlobalStatic.HttpRoutes.SORTING}")]
        public Task<ApiResponse<EntriesSortingResponseModel>> CommandSortingSet(IdsPairModel commands_for_move);

        /// <summary>
        /// Удалить команду
        /// </summary>
        /// <param name="id_command">Идентификатор команды для удаления</param>
        /// <returns></returns>
        [Delete($"/api/{GlobalStatic.HttpRoutes.Commands}/{{id_command}}")]
        public Task<ApiResponse<ResponseBaseModel>> CommandDelete(int id_command);
    }
}