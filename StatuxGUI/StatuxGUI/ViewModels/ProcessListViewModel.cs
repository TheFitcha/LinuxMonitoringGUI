using MvvmHelpers;
using MvvmHelpers.Commands;
using StatuxGUI.Models;
using StatuxGUI.Services;
using StatuxGUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatuxGUI.ViewModels
{
    public class ProcessListViewModel : BaseViewModel
    {
        private ObservableCollection<Machine> availableMachines;
        public ObservableCollection<Machine> AvailableMachines
        {
            get => availableMachines;
            set => SetProperty(ref availableMachines, value);
        }

        private ObservableRangeCollection<Models.Process> processes;
        public ObservableRangeCollection<Models.Process> Processes
        {
            get => processes;
            set => SetProperty(ref processes, value);
        }

        private Machine selectedMachine;
        public Machine SelectedMachine
        {
            get => selectedMachine;
            set => SetProperty(ref selectedMachine, value);
        }

        private Models.Process selectedProcess;
        public Models.Process SelectedProcess
        {
            get => selectedProcess;
            set => SetProperty(ref selectedProcess, value);
        }

        private readonly IMachineService _machineService; 
        private readonly IProcessService _processService;

        public AsyncCommand RefreshMachinesCommand { get; private set; }
        public AsyncCommand RefreshProcessesCommand { get; private set; }
        public AsyncCommand RefreshFullCommand { get; private set; }
        public AsyncCommand<Models.Process>SelectedProcessCommand { get; private set; }

        private string backgroundColor;
        public string BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }
        public ProcessListViewModel()
        {
            _machineService = DependencyService.Get<IMachineService>();
            _processService = DependencyService.Get<IProcessService>();

            RefreshMachinesCommand = new AsyncCommand(RefreshMachines);
            RefreshProcessesCommand = new AsyncCommand(RefreshProcesses);
            RefreshFullCommand = new AsyncCommand(RefreshFull);
            SelectedProcessCommand = new AsyncCommand<Models.Process>(SelectedProcessMethod);

            BackgroundColor = AppSettingsManager.Settings["background_color"];
        }

        private async Task RefreshFull()
        {
            IsBusy = true;
            await RefreshProcesses();
            await RefreshMachines();
            IsBusy = false; 
        }

        private async Task RefreshMachines()
        {
            try
            {
                AvailableMachines = await _machineService.GetMachines();
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Cannot get machines!", "Ok");
            }
            
            //Processes = null;
        }
        private async Task RefreshProcesses()
        {
            if(selectedMachine?.Id != null)
            {
                Processes = await _processService.GetAllProcesses(selectedMachine.Id);
            }
            else
            {
                Processes = null;
            }
        }

        private async Task SelectedProcessMethod(Models.Process process)
        {
            if (process == null)
                return;

            if(process.Id == null)
            {
                Debug.WriteLine("Selected process does not contain id!");
                return;
            }

            var detailsRoute = $"{nameof(ProcessDetailsPage)}?SelectedProcessID={process.Id}";
            await Shell.Current.GoToAsync(detailsRoute);

            //SelectedProcess = null;
        }

    }
}
