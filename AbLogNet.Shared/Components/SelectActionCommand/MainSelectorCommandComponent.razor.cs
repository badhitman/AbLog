using AbLogNet.Shared.Components.SelectActionCommand.Controller;
using AbLogNet.Shared.Components.SelectActionCommand.Exit;
using Microsoft.AspNetCore.Components;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbLogNet.Shared.Components.SelectActionCommand
{
    public partial class MainSelectorCommandComponent : BlazorBusyComponentBaseModel
    {
        /// <summary>
        /// Устройства
        /// </summary>
        [Parameter, EditorRequired]
        public required IEnumerable<EntryNestedModel> EntriesTreeHW { get; set; }

        /// <summary>
        /// Выбранная/текущая команда перехода (если null, то выход)
        /// </summary>
        [Parameter, EditorRequired]
        public required CommandModelDB CurrentCommand { get; set; }

        [Parameter, EditorRequired]
        public required Action<CommandModelDB> RootSelectorHandler { get; set; }

        [Parameter, EditorRequired]
        public required IEnumerable<ScriptModelDB> AllScripts { get; set; }

        /// <summary>
        /// Типы команд:
        /// Команда контролера, порт или выход/переход
        /// </summary>
        public TypesCommandsEnum CommandType
        {
            get => CurrentCommand?.CommandType ?? TypesCommandsEnum.Exit;
            private set
            {
                if (CurrentCommand is null)
                    return;

                CurrentCommand.CommandType = value;

                SelectHandlerAction();
            }
        }

        public ControllerSelectorComponent? ControllerSelectorRef { get; private set; }

        public ExitSelectorComponent? ExitSelectorRef { get; private set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender && CurrentCommand?.Execution == 0 && EntriesTreeHW.Any())
            {
                switch (CommandType)
                {
                    case TypesCommandsEnum.Controller:
                        if (EntriesTreeHW.Any())
                        {
                            CurrentCommand.Execution = EntriesTreeHW.First().Id;
                        }
                        break;
                    case TypesCommandsEnum.Port:
                        if (EntriesTreeHW.Any(x => x.Childs.Any()))
                        {
                            CurrentCommand.Execution = EntriesTreeHW.First(x => x.Childs.Any()).Childs.First().Id;
                        }
                        break;
                    case TypesCommandsEnum.Exit:
                        break;
                }
                SelectHandlerAction();
            }
        }

        void SelectHandlerAction() => RootSelectorHandler(CurrentCommand);

        protected override void OnInitialized()
        {
            if (CurrentCommand?.Execution == 0)
            {
                switch (CommandType)
                {
                    case TypesCommandsEnum.Controller:
                        if (EntriesTreeHW.Any())
                        {
                            CurrentCommand.Execution = EntriesTreeHW.First().Id;
                        }
                        break;
                    case TypesCommandsEnum.Port:
                        if (EntriesTreeHW.Any(x => x.Childs.Any()))
                        {
                            CurrentCommand.Execution = EntriesTreeHW.First(x => x.Childs.Any()).Childs.First().Id;
                        }
                        break;
                    case TypesCommandsEnum.Exit:
                        break;
                }
            }
        }
    }
}