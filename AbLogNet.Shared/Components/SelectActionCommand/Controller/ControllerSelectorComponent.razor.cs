using Microsoft.AspNetCore.Components;
using SharedLib;

namespace AbLogNet.Shared.Components.SelectActionCommand.Controller
{
    public partial class ControllerSelectorComponent : ComponentBase
    {
        /// <summary>
        /// Перечень доступных контролеров вместе со своими портами
        /// </summary>
        [Parameter, EditorRequired]
        public required IEnumerable<EntryNestedModel> EntriesTreeHW { get; set; }

        [Parameter, EditorRequired]
        public required Action ControllerActionHandler { get; set; }

        /// <summary>
        /// Команда
        /// </summary>
        [Parameter, EditorRequired]
        public required CommandModelDB CurrentCommand { get; set; }

        public int SelectedControllerId
        {
            get
            {
                return CurrentCommand?.CommandType switch
                {
                    TypesCommandsEnum.Controller => CurrentCommand.Execution,
                    TypesCommandsEnum.Port => EntriesTreeHW.FirstOrDefault(x => x.Childs.Any(y => y.Id == CurrentCommand.Execution))?.Id ?? 0,
                    _ => 0
                };
            }
            set
            {
                if (CurrentCommand?.CommandType != TypesCommandsEnum.Controller && CurrentCommand?.CommandType != TypesCommandsEnum.Port)
                    return;

                bool is_upd = CurrentCommand.Execution != value;
                CurrentCommand.Execution = value;
                if (is_upd)
                {
                    if (ControllerActionHandler is not null)
                        ControllerActionHandler();
                    else
                        StateHasChanged();
                }
            }
        }

        PortHardwareModel? CurrentPort;
        EntryModel? CurrentController;

        /// <summary>
        /// Ссылка на вью компонента выбора порта
        /// </summary>
        public PortSelectorComponent? SelectorPortRef { get; private set; }
        /// <summary>
        /// Ссылка на вью компонента ввода команды (низкоуровневая) контроллера.
        /// i: Актуальна в случае, если
        /// </summary>
        public ControllerCmdComponent? CommandControllerRef { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            if (CurrentCommand is null)
                return;

            if (CurrentCommand.Execution > 0)
                switch (CurrentCommand.CommandType)
                {
                    case TypesCommandsEnum.Controller:
                        await RestByControllerId(CurrentCommand.Execution);
                        break;
                    case TypesCommandsEnum.Port:
                        await RestByPortId(CurrentCommand.Execution);
                        break;
                    default:
                        break;
                }
        }

        async Task RestByPortId(int port_id)
        {
            TResponseModel<PortHardwareModel> rest = await _hardwires.HardwarePortGet(port_id);
            CurrentPort = rest.Response;
            CurrentController = CurrentPort?.Hardware;
            await RestByControllerId(CurrentController!.Id);
        }

        async Task RestByControllerId(int controller_id)
        {
            TResponseModel<HardwareBaseModel> rest = await _hardwires.HardwareGet(controller_id);
            CurrentController = rest.Response;
        }
    }
}
