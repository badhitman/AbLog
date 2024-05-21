namespace RazorLib.Shared.hardwires;

/// <summary>
/// Hardware translate Main (LayerComponent)
/// </summary>
public partial class HardwareTranslateMainLayerComponent : ReloadPageComponentBaseModel
{
    /// <inheritdoc/>
    protected override async Task OnInitializedAsync() => await GetData();
}