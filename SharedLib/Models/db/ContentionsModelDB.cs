////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib
{
    /// <summary>
    /// Конкуренция между скриптами.
    /// Запрет запуска Slave скрипта пока выполняется Master скрипт
    /// </summary>
    [Index(nameof(MasterScriptId), nameof(SlaveScriptId), IsUnique = true, Name = "IX_ScriptJoinLink"), Index(nameof(MasterScriptId), Name = "IX_ContentionsMaster"), Index(nameof(SlaveScriptId), Name = "IX_ContentionsSlave")]
    public class ContentionsModelDB
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// FK: Ведущий/старший скрипт
        /// </summary>
        public int MasterScriptId { get; set; }
        /// <summary>
        /// Ведущий/старший скрипт
        /// </summary>
        [ForeignKey(nameof(MasterScriptId))]
        public ScriptModelDB? MasterScript { get; set; }

        /// <summary>
        /// FK: Ведомый/младший скрипт
        /// </summary>
        public int SlaveScriptId { get; set; }
        /// <summary>
        /// Ведомый/младший скрипт
        /// </summary>
        [ForeignKey(nameof(SlaveScriptId))]
        public ScriptModelDB? SlaveScript { get; set; }
    }
}