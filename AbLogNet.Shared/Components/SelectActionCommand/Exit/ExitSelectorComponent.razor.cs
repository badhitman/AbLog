using Microsoft.AspNetCore.Components;
using SharedLib;

namespace AbLogNet.Shared.Components.SelectActionCommand.Exit;

public partial class ExitSelectorComponent : ComponentBase
{
    /// <summary>
    /// Команда
    /// </summary>
    [Parameter, EditorRequired]
    public CommandModelDB? CurrentCommand { get; set; }

    [Parameter, EditorRequired]
    public Action? SelectDirectHandler { get; set; }

    [Parameter, EditorRequired]
    public IEnumerable<ScriptModelDB>? AllScripts { get; set; }

    int orign_command_id;
    public int CurrentCommandId
    {
        get => CurrentCommand?.Execution ?? 0;
        private set
        {
            if (CurrentCommand is null)
                return;

            CurrentCommand!.Execution = value;
            if (value != default && orign_command_id == default)
                orign_command_id = value;

            if (SelectDirectHandler is not null)
            {
                SelectDirectHandler();
            }
        }
    }
}
