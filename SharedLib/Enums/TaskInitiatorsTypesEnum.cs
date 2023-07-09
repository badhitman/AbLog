////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Типы инициаторов выполнения сценриев
/// </summary>
public enum TaskInitiatorsTypesEnum
{
    /// <summary>
    /// Ручной запуск из приложения
    /// </summary>
    Manual,

    /// <summary>
    /// Автозапуск по событию/триггеру
    /// </summary>
    Trigger
}