using Microsoft.AspNetCore.Components;
using RazorLib;

namespace ab_log_rc.Pages;

public partial class HardwareConfigPage : BlazorBusyComponentBaseModel
{
    [Parameter, EditorRequired]
    public int Id { get; set; }
}