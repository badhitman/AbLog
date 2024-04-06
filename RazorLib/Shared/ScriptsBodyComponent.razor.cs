using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MudBlazor;
using SharedLib;

namespace RazorLib.Shared;

/// <summary>
/// Скриты с командами
/// </summary>
public partial class ScriptsBodyComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Logger
    /// </summary>
    [Inject]
    public required ILogger<ScriptsBodyComponent> Logger { get; set; }

    /// <summary>
    /// Scripts
    /// </summary>
    [Inject]
    public required IScriptsService ScriptsService { get; set; }

    /// <summary>
    /// Snackbar
    /// </summary>
    [Inject]
    public required ISnackbar Snackbar { get; set; }


    List<ScriptModelDB> ScriptsAll = [];
    ScriptModelDB CurrentScript = new();

    /// <summary>
    /// Form ref
    /// </summary>
    protected EditForm? _formRef;

    /// <summary>
    /// TextBox ref
    /// </summary>
    protected InputRichTextComponent? _textBoxRef;

    CommandsListComponent? _commandsListRef;
    ScriptContentionComponent? _contentionsRef;

    /// <summary>
    /// Скрипт можно сохранить: заполнено имя скрипта, а так же скрипт изменён или только что создан и ещё не сохранён
    /// </summary>
    bool CanSaveScript
    {
        get
        {
            return !string.IsNullOrWhiteSpace(CurrentScript.Name) &&
            (CurrentScript.Id < 1 || script_name_orign != CurrentScript.Name || script_desc_orign != CurrentScript.Description);
        }
    }

    bool script_init_del = false;
    string script_name_orign = string.Empty;
    string? script_desc_orign;

    async Task DeleteScript()
    {
        if (!script_init_del)
        {
            script_init_del = true;
            return;
        }
        string msg;
        if (CurrentScript.Id > 0)
        {
            IsBusyProgress = true;
            ScriptsResponseModel rest = await ScriptsService.ScriptDelete(CurrentScript.Id, CancellationToken);
            Snackbar.ShowMessagesResponse(rest.Messages);
            if (!rest.IsSuccess)
                return;

            if (rest.Scripts is null)
            {
                msg = $"rest.Content is null error {{0D331AB6-20AE-43DB-8078-12B13EEAB3B0}}";
                Logger.LogError(msg);
                Snackbar.Add(msg, Severity.Error, c => c.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
                return;
            }

            ScriptsAll.RemoveAt(ScriptsAll.FindIndex(x => x.Id == CurrentScript.Id));
            IsBusyProgress = false;
        }
        else
        {
            ScriptsAll.RemoveAt(ScriptsAll.FindIndex(x => x.Id == CurrentScript.Id));
        }
        CurrentScript = ScriptsAll.FirstOrDefault() ?? new();

        script_name_orign = CurrentScript.Name;
        script_desc_orign = CurrentScript.Description;

        script_init_del = false;
    }

    /// <summary>
    /// Сброс состояния формы
    /// </summary>
    /// <param name="mode">Режимы сброса состояния формы</param>
    void ResetFormState(ResetFormModesEnum mode)
    {
        script_init_del = false;
        if (mode == ResetFormModesEnum.EditToOrign)
        {
            script_name_orign = CurrentScript.Name;
            script_desc_orign = CurrentScript.Description;
        }
        else
        {
            CurrentScript.Name = script_name_orign;
            CurrentScript.Description = script_desc_orign;
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        IsBusyProgress = true;
        ScriptsResponseModel res = await ScriptsService.ScriptsGetAll(CancellationToken);
        Snackbar.ShowMessagesResponse(res.Messages);
        if (!res.IsSuccess)
            return;

        if (res.Scripts is null)
        {
            Snackbar.Add("res.Content?.Scripts is null ошибка {EB6B7427-DB32-4B76-8F85-D4B10E5B2488}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        ScriptsAll = res.Scripts;

        if (ScriptsAll.Count != 0)
            CurrentScript = ScriptsAll.First();

        ResetFormState(ResetFormModesEnum.EditToOrign);
        IsBusyProgress = false;
    }

    void AddNewScript()
    {
        string new_name = string.Empty;
        foreach (int i in Enumerable.Range(1, int.MaxValue))
        {
            new_name = $"Новый скрипт {i}";
            if (!ScriptsAll.Any(x => x.Name.Equals(new_name, StringComparison.CurrentCultureIgnoreCase)))
                break;
        }

        int _id = ScriptsAll.Count != 0 ? ScriptsAll.Min(x => x.Id) - 1 : 0;
        if (_id > 0)
            _id = 0;

        ScriptsAll.Add(new() { Id = _id, Commands = [], Name = new_name });
        CurrentScript = ScriptsAll.Last();
    }

    async Task SaveScriptHandle()
    {
        IsBusyProgress = true;
        ScriptsResponseModel rest = await ScriptsService.ScriptUpdateOrCreate(new EntryDescriptionModel() { Id = CurrentScript.Id, Name = CurrentScript.Name, Description = CurrentScript.Description }, CancellationToken);
        Snackbar.ShowMessagesResponse(rest.Messages);
        if (!rest.IsSuccess)
            return;

        if (rest.Scripts is null)
        {
            Snackbar.Add("rest.Content?.Scripts is null ошибка {093F4C03-E89C-40F9-931D-EDB389FF56A6}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        ScriptModelDB src = rest.Scripts.First(x => x.Name == CurrentScript.Name);
        CurrentScript.Id = src.Id;
        CurrentScript.Name = src.Name;
        CurrentScript.Description = src.Description;
        ResetFormState(ResetFormModesEnum.EditToOrign);

        int[] rows_for_del = ScriptsAll.Where(x => x.Id > 0 && !rest.Scripts.Any(y => y.Id == x.Id)).Select(x => x.Id).Distinct().ToArray();
        while (ScriptsAll.Any(x => rows_for_del.Contains(x.Id)))
        {
            ScriptsAll.RemoveAt(ScriptsAll.FindIndex(x => rows_for_del.Contains(x.Id)));
        }
        ScriptModelDB[] rows_for_add = rest.Scripts.Where(x => x.Id != src.Id && !ScriptsAll.Any(y => y.Id == x.Id)).ToArray();
        if (rows_for_add.Length != 0)
        {
            ScriptsAll.AddRange(rows_for_add);
        }
        IsBusyProgress = false;
    }
}
