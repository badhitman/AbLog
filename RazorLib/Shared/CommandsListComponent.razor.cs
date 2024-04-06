using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using SharedLib;

namespace RazorLib.Shared;

/// <summary>
/// Список команд
/// </summary>
public partial class CommandsListComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Logger
    /// </summary>
    [Inject]
    public required ILogger<CommandsListComponent> Logger { get; set; }

    /// <summary>
    /// Snackbar
    /// </summary>
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    /// <summary>
    /// Commands service
    /// </summary>
    [Inject]
    public required ICommandsService CommandsService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter, EditorRequired]
    public int ScriptId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter, EditorRequired]
    public required IEnumerable<ScriptModelDB> AllScripts { get; set; }

    /// <summary>
    /// Команды
    /// </summary>
    protected List<EntrySortingModel> Commands = [];

    /// <summary>
    /// Move command - Action
    /// </summary>
    protected void MoveCommandAction(VerticalDirectionsEnum direction, int command_id)
    {
        if (Commands.Any(x => x.Id < 1))
        {
            Snackbar.Add("Некоторые команды не записаны. Для перемещения команд требуется что бы все команды были записаны", Severity.Warning, (opt) => { opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow; });
            return;
        }
        int index_com = Commands.FindIndex(x => x.Id == command_id);

        if (index_com < 0)
        {
            Snackbar.Add($"Ошибка. Не найден объект команды #{command_id}", Severity.Error, (opt) => { opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow; });
            return;
        }

        if ((direction == VerticalDirectionsEnum.Up && index_com == 0) || (direction == VerticalDirectionsEnum.Down && index_com == Commands.Count - 1))
        {
            Snackbar.Add($"Ошибка. Нельзя сдвинуть команду в этом направлении", Severity.Error, (opt) => { opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow; });
            return;
        }

        if (direction == VerticalDirectionsEnum.Up)
        {
            _ = InvokeAsync(async () => await MoveCommandRest(new IdsPairModel() { Id1 = command_id, Id2 = Commands[index_com - 1].Id }));
        }
        else
        {
            _ = InvokeAsync(async () => await MoveCommandRest(new IdsPairModel() { Id1 = command_id, Id2 = Commands[index_com + 1].Id }));
        }

    }

    /// <summary>
    /// Move command - Rest
    /// </summary>
    protected async Task MoveCommandRest(IdsPairModel req)
    {
        IsBusyProgress = true;
        StateHasChanged();

        EntriesSortingResponseModel rest = await CommandsService.CommandSortingSet(req);
        Snackbar.ShowMessagesResponse(rest.Messages);
        if (rest.Entries is null)
        {
            Snackbar.Add("rest.Content?.Entries is null ошибка {D73FEEE5-38BC-4AFF-B1FA-E00CE50673F4}", Severity.Error, opt => opt.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        if (!rest.IsSuccess)
            return;

        EntrySortingModel[] for_add = rest.Entries.Where(x => !Commands.Any(y => y.Id == x.Id)).ToArray();
        int[] for_del = Commands.Where(x => !rest.Entries.Any(y => y.Id == x.Id)).Select(x => x.Id).ToArray();
        if (for_add.Length != 0)
        {
            Commands.AddRange(for_add);
        }
        if (for_del.Any())
        {
            Commands = Commands.Where(x => !for_del.Contains(x.Id)).ToList();
        }
        Commands.ForEach(x => { EntrySortingModel? y = rest.Entries.FirstOrDefault(z => z.Id == x.Id); if (y is not null) { x.Sorting = y.Sorting; } });
        Commands = Commands.OrderBy(x => x.Sorting).ToList();
        IsBusyProgress = false;
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await Rest(ScriptId);
    }

    /// <summary>
    /// Add command
    /// </summary>
    protected void AddCommand(int position_num)
    {
        if (Commands.Count == 0)
        {
            Commands.Add(new EntrySortingModel() { Id = -1, Sorting = 1, Name = "cmd-1" });
            StateHasChanged();
            return;
        }

        int new_tmp_id = Commands.Min(x => x.Id);
        new_tmp_id = new_tmp_id > 0 ? -1 : new_tmp_id - 1;
        double new_position_num;
        if (Commands.Count == position_num)
        {
            new_position_num = Math.Ceiling(Commands.Max(x => x.Sorting) + 1);
        }
        else if (position_num == 0)
        {
            new_position_num = Math.Floor(Commands.Min(x => x.Sorting) - 1);
        }
        else
        {
            Commands.Sort((x, y) => x.Sorting.CompareTo(y.Sorting));
            double prev_sorting_val = Commands[position_num - 1].Sorting;
            double next_sorting_val = Commands[position_num].Sorting;
            new_position_num = prev_sorting_val + ((next_sorting_val - prev_sorting_val) / 2.0);
        }

        string new_name = "new-cmd";
        for (int i = 1; i < int.MaxValue; i++)
        {
            new_name = $"cmd-{i}";
            if (!Commands.Any(x => x.Name == new_name))
                break;
        }

        Commands.Add(new EntrySortingModel() { Id = new_tmp_id, Sorting = new_position_num, Name = new_name });
        Commands.Sort((x, y) => x.Sorting.CompareTo(y.Sorting));
        StateHasChanged();
    }

    /// <summary>
    /// Delete command
    /// </summary>
    protected void DeleteCommand(int command_id)
    {
        int index_row = Commands.FindIndex(x => x.Id == command_id);
        if (index_row < 0)
            return;

        if (Commands[index_row].Id < 0)
        {
            Commands.RemoveAt(index_row);
            StateHasChanged();
            return;
        }

    }

    /// <summary>
    /// Update command
    /// </summary>
    protected void UpdateCommand(CommandModelDB sender)
    {
        int index_row = Commands.FindIndex(x => x.Id == sender.Id);
        if (index_row > -1)
        {
            Commands.RemoveAt(index_row);
        }

        _ = InvokeAsync(async () => await Rest(ScriptId, false));
    }

    /// <summary>
    /// Rest
    /// </summary>
    public async Task Rest(int id, bool hard_reload = true)
    {
        IsBusyProgress = true;
        EntriesSortingResponseModel rest = await CommandsService.GetCommandsEntriesByScript(id);
        if (hard_reload)
        {
            Commands = [.. rest.Entries ?? []];
        }
        else
        {
            List<EntrySortingModel> cashe_commands = new(rest.Entries ?? []);

            IEnumerable<EntrySortingModel> commands_add = cashe_commands.Where(x => !Commands.Any(y => y.Id == x.Id)).ToArray();
            if (commands_add.Any())
            {
                Commands.AddRange(commands_add);
            }
        }

        IsBusyProgress = false;
        StateHasChanged();
    }
}