using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SharedLib;

namespace RazorLib.Shared.settings;

/// <summary>
/// TelegramUsersListComponent
/// </summary>
public partial class TelegramUsersListComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Пользователи
    /// </summary>
    [Inject]
    public required IUsersService UsersService { get; set; }

    /// <summary>
    /// Snackbar
    /// </summary>
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    private int totalItems;
    private MudTable<UserModelDB>? table;

    UserModelDB elementBeforeEdit = new();
    IEnumerable<UserModelDB> Users = [];

    async Task Reload()
    {
        if (table is not null)
            await table.ReloadServerData();
    }

    void SaveElement(MouseEventArgs element)
    {
        UserModelDB? sender = Users?.FirstOrDefault(x => x.TelegramId == elementBeforeEdit.TelegramId);

        if (sender is null)
        {
            Snackbar.Add("sender is null. error {71B240C3-2A60-45AB-B7AF-C0A8F4CF7CAB}", Severity.Error, conf => conf.DuplicatesBehavior = SnackbarDuplicatesBehavior.Allow);
            return;
        }

        _ = InvokeAsync(async () =>
        {
            IsBusyProgress = true;
            StateHasChanged();
            ResponseBaseModel upd_command = await UsersService.UpdateUser(new UpdateUserModel()
            {
                TelegramId = sender.TelegramId,
                Email = sender.Email,
                IsDisabled = sender.IsDisabled,
                AllowSystemCommands = sender.AllowSystemCommands,
                AllowAlerts = sender.AlarmSubscriber,
                AllowHardwareControl = sender.CommandsAllowed,
                AllowChangeMqttConfig = sender.AllowChangeMqttConfig
            });
            Snackbar.ShowMessagesResponse(upd_command.Messages);
            if (!upd_command.IsSuccess)
            {
                IsBusyProgress = false;
                StateHasChanged();
                return;
            }
            elementBeforeEdit = GlobalStatic.CreateDeepCopy(sender);
            await Reload();
            IsBusyProgress = false;
            StateHasChanged();
        });
    }

    void BeckupEditItem(object element)
    {
        UserModelDB sender = (UserModelDB)element;
        elementBeforeEdit = GlobalStatic.CreateDeepCopy<UserModelDB>(sender);
        StateHasChanged();
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    private async Task<TableData<UserModelDB>> ServerReload(TableState state)
    {
        IsBusyProgress = true;
        StateHasChanged();
        UsersPaginationResponseModel rest = await UsersService.UsersGetList(new UserListGetModel() { PageNum = state.Page, PageSize = state.PageSize, IncludeDisabledUsers = true });
        Snackbar.ShowMessagesResponse(rest.Messages);
        totalItems = rest.TotalRowsCount;
        IsBusyProgress = false;
        Users = rest.Users ?? [];
        StateHasChanged();

        return new TableData<UserModelDB>() { TotalItems = totalItems, Items = rest.Users };
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((UserModelDB)element).Reload(elementBeforeEdit);
        StateHasChanged();
    }

    private int selectedRowNumber = -1;
    private string SelectedRowClassFunc(UserModelDB element, int rowNumber)
    {
        if (selectedRowNumber == rowNumber)
        {
            selectedRowNumber = -1;
            return string.Empty;
        }
        else if (table?.SelectedItem is not null && table.SelectedItem.Equals(element))
        {
            selectedRowNumber = rowNumber;
            return "selected";
        }

        return string.Empty;
    }
}