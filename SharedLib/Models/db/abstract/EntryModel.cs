////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib
{
    /// <summary>
    /// Простейшая entry-db модель
    /// </summary>
    public class EntryModel
    {
        /// <summary>
        /// Ключ/идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public virtual string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Базовая вложенная модель
    /// </summary>
    public class EntryNestedModel : EntryModel
    {
        /// <summary>
        /// Вложеные объекты/наследники
        /// </summary>
        public EntryModel[] Childs { get; set; } = Array.Empty<EntryModel>();
    }

    /// <summary>
    /// Базовая модель entry+description db модель
    /// </summary>
    public class EntryDescriptionModel : EntryModel
    {
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// Базовая модель entry с сортировкой db модель
    /// </summary>
    public class EntrySortingModel : EntryModel
    {
        /// <summary>
        /// Индекс сортировки
        /// </summary>
        public double Sorting { get; set; }
    }
}