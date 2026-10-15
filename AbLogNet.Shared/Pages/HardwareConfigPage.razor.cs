using AbLogNet.Shared.Components;
using Microsoft.AspNetCore.Components;

namespace AbLogNet.Shared.Pages;

public partial class HardwareConfigPage : BlazorBusyComponentBaseModel
{
    [Parameter, EditorRequired]
    public int Id { get; set; }
}