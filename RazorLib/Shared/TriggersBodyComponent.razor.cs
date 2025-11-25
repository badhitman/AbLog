using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MudBlazor;
using SharedLib;

namespace RazorLib.Shared;

/// <summary>
/// 
/// </summary>
public partial class TriggersBodyComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Scripts service
    /// </summary>
    [Inject]
    public required IScriptsService ScriptsService { get; set; }

    /// <summary>
    /// Hardwires service
    /// </summary>
    [Inject]
    public required IHardwiresService HardwiresService { get; set; }

    /// <summary>
    /// Triggers service
    /// </summary>
    [Inject]
    public required ITriggersService TriggersService { get; set; }

    /// <summary>
    /// Logger
    /// </summary>
    [Inject]
    public required ILogger<TriggersBodyComponent> Logger { get; set; }

    /// <summary>
    /// Snackbar
    /// </summary>
    [Inject]
    public required ISnackbar SnackBar { get; set; }

    /// <summary>
    /// Entries tree hardwires
    /// </summary>
    public List<EntryNestedModel> EntriesTreeHW { get; set; } = [];

    ConditionsComponent? conditions_ref;

    /// <inheritdoc/>
    protected MudList<object>? _listRef;

    object _selVal = 0;
    object SelectedValue
    {
        get
        {
            if (_selVal.ToString() == "0" && _curTrig.Id != 0)
            {
                return _curTrig.Id;
            }
            if (_selVal.ToString() == "0" && Triggers.Count != 0)
            {
                _curTrig = Triggers.First();
                return _curTrig.Id;
            }
            return _selVal;
        }
        set
        {
            ResetFormToOrignState();
            _selVal = value;
            if (_selVal.ToString() != _curTrig.Id.ToString() && int.TryParse(_selVal.ToString(), out int selected_id))
            {
                _curTrig = Triggers.FirstOrDefault(x => x.Id == selected_id) ?? new();
            }
            if (conditions_ref is not null)
                conditions_ref?.Rest(_curTrig.Id, ConditionsTypesEnum.Trigger);

            trigger_name_orign = CurrentTrigger.Name;
            trigger_desc_orign = CurrentTrigger.Description ?? "";
            trigger_is_off = CurrentTrigger.IsDisable;
            trigger_script = CurrentTrigger.ScriptId;
        }
    }

    TrigerModelDB _curTrig = new();
    TrigerModelDB CurrentTrigger
    {
        get
        {
            if (_curTrig.Id == 0 && _selVal.ToString() != "0" && int.TryParse(_selVal.ToString(), out int selected_id))
            {
                _curTrig = Triggers.FirstOrDefault(x => x.Id == selected_id) ?? new();
            }

            return _curTrig;
        }
        set
        {
            _curTrig = value;

            if (_selVal.ToString() != _curTrig.Id.ToString() && int.TryParse(_selVal.ToString(), out int selected_id))
            {
                _selVal = Triggers.FirstOrDefault(x => x.Id == selected_id)?.Id ?? 0;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected EditForm? form;
    InputRichTextComponent? _textBox;

    List<TrigerModelDB> Triggers = [];
    List<ScriptModelDB> Scripts = [];

    bool CanSaveTrigger
    {
        get
        {
            return !string.IsNullOrWhiteSpace(CurrentTrigger.Name) &&
            (CurrentTrigger.Id < 1 || trigger_script != CurrentTrigger.ScriptId || trigger_is_off != CurrentTrigger.IsDisable || trigger_name_orign != CurrentTrigger.Name || trigger_desc_orign != CurrentTrigger.Description);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        IsBusyProgress = true;

        TResponseModel<List<ScriptModelDB>> rest_all_scripts = await ScriptsService.ScriptsGetAll(CancellationToken);
        SnackBar.ShowMessagesResponse(rest_all_scripts.Messages);
        if (!rest_all_scripts.IsSuccess)
        {
            IsBusyProgress = false;
            return;
        }

        if (rest_all_scripts.Response is null)
        {
            IsBusyProgress = false;
            SnackBar.Add("rest_all_scripts.Scripts is null ошибка {5B557F50-0F44-40CD-A5B4-10A58F78D701}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        Scripts = rest_all_scripts.Response;

        TResponseModel<List<TrigerModelDB>> rest_all_triggers = await TriggersService.TriggersGetAll(CancellationToken);
        SnackBar.ShowMessagesResponse(rest_all_triggers.Messages);
        if (!rest_all_triggers.IsSuccess)
        {
            IsBusyProgress = false;
            return;
        }

        if (rest_all_triggers.Response is null)
        {
            IsBusyProgress = false;
            SnackBar.Add("rest_all_triggers.Triggers is null ошибка {4334BFB1-481E-460D-9E21-B8782DEA3DCA}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        Triggers = new(rest_all_triggers.Response.OrderBy(x => x.Name));
        if (Triggers.Count != 0)
            _curTrig = Triggers.First();

        TResponseModel<List<EntryNestedModel>> rest_hw_tree = await HardwiresService.HardwiresGetTreeNestedEntries(CancellationToken);
        SnackBar.ShowMessagesResponse(rest_hw_tree.Messages);
        if (!rest_hw_tree.IsSuccess)
        {
            IsBusyProgress = false;
            return;
        }

        if (rest_hw_tree.Response is null)
        {
            IsBusyProgress = false;
            SnackBar.Add("rest_hw_tree.Entries is null ошибка {7A0F6B2C-8286-4CB5-84AB-7F954831FE94}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }
        EntriesTreeHW = rest_hw_tree.Response;

        trigger_name_orign = _curTrig.Name;
        trigger_desc_orign = _curTrig.Description ?? "";
        trigger_is_off = _curTrig.IsDisable;
        trigger_script = _curTrig.ScriptId;

        ResetFormToOrignState();
        IsBusyProgress = false;
    }

    string trigger_name_orign = string.Empty;
    string trigger_desc_orign = string.Empty;
    bool trigger_is_off = false;
    int trigger_script;
    //
    bool trigger_init_del = false;

    void AddNewTrigger()
    {
        string new_name = string.Empty;
        foreach (int i in Enumerable.Range(1, int.MaxValue))
        {
            new_name = $"тригер new:{i}";
            if (!Triggers.Any(x => x.Name.Equals(new_name, StringComparison.CurrentCultureIgnoreCase)))
                break;
        }

        int _id = Triggers.Count != 0 ? Triggers.Min(x => x.Id) - 1 : -1;
        if (_id > -1)
            _id = -1;

        Triggers.Add(new() { Id = _id, Name = new_name, Conditions = [] });
        SelectedValue = Triggers.Last().Id;
    }

    void ResetFormToOrignState()
    {
        trigger_init_del = false;
        CurrentTrigger.Name = trigger_name_orign;
        CurrentTrigger.Description = trigger_desc_orign;
        CurrentTrigger.IsDisable = trigger_is_off;
        CurrentTrigger.ScriptId = trigger_script;
        _textBox?.SetValue(trigger_desc_orign);
    }

    async Task DeleteTrigger()
    {
        if (!trigger_init_del)
        {
            trigger_init_del = true;
            return;
        }
        IsBusyProgress = true;
        if (CurrentTrigger.Id > 0)
        {
            TResponseModel<List<TrigerModelDB>> rest = await TriggersService.TriggerDelete(CurrentTrigger.Id, CancellationToken);
            SnackBar.ShowMessagesResponse(rest.Messages);
            if (!rest.IsSuccess)
                return;

            Triggers.RemoveAt(Triggers.FindIndex(x => x.Id == CurrentTrigger.Id));
        }
        else
        {
            Triggers.RemoveAt(Triggers.FindIndex(x => x.Id == CurrentTrigger.Id));
        }

        SelectedValue = Triggers.FirstOrDefault()?.Id ?? 0;
        ResetFormToOrignState();
        IsBusyProgress = false;
    }

    async Task SaveTriggerHandle()
    {
        IsBusyProgress = true;
        TResponseModel<List<TrigerModelDB>> rest = await TriggersService.TriggerUpdateOrCreate(CurrentTrigger, CancellationToken);
        IsBusyProgress = false;
        SnackBar.ShowMessagesResponse(rest.Messages);
        if (!rest.IsSuccess)
            return;

        if (rest.Response is null)
        {
            SnackBar.Add("rest.Triggers is null. error {F73EA570-0594-41B1-A2E0-7CEB33B07370}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        TrigerModelDB src = rest.Response.First(x => x.Name == CurrentTrigger.Name);
        _curTrig.Id = src.Id;
        _curTrig.Name = src.Name;
        _curTrig.Description = src.Description;
        _curTrig.IsDisable = src.IsDisable;
        _curTrig.ScriptId = src.ScriptId;

        trigger_name_orign = src.Name;
        trigger_desc_orign = src.Description ?? "";
        trigger_is_off = src.IsDisable;
        trigger_script = src.ScriptId;
        ResetFormToOrignState();

        int[] rows_for_del = Triggers.Where(x => x.Id > 0 && !rest.Response.Any(y => y.Id == x.Id)).Select(x => x.Id).Distinct().ToArray();
        while (Triggers.Any(x => rows_for_del.Contains(x.Id)))
        {
            Triggers.RemoveAt(Triggers.FindIndex(x => rows_for_del.Contains(x.Id)));
        }
        TrigerModelDB[] rows_for_add = rest.Response.Where(x => x.Id != src.Id && !Triggers.Any(y => y.Id == x.Id)).ToArray();
        if (rows_for_add.Length != 0)
        {
            Triggers.AddRange(rows_for_add);
        }

        Triggers.RemoveAt(Triggers.FindIndex(x => x.Id == src.Id));
        Triggers.Add(src);
        SelectedValue = src.Id;
    }
}