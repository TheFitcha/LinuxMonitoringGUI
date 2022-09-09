using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using StatuxGUI.Models;
using StatuxGUI.Services;
using StatuxGUI.Views;
using Xamarin.Forms;

namespace StatuxGUI.ViewModels
{
    public class MachinesListViewModel : BaseViewModel
    {
        private ObservableCollection<Machine> allMachines;
        private ObservableCollection<Machine> machines;
        public ObservableCollection<Machine> Machines { 
            get => machines;
            set => SetProperty(ref machines, value);
        }

        private string filterEntry = "";
        public string FilterEntry
        {
            get => filterEntry;
            set => SetProperty(ref filterEntry, value);
        }

        private Machine selectedMachine;
        public Machine SelectedMachine
        {
            get => selectedMachine;
            set => SetProperty(ref selectedMachine, value);
        }

        public AsyncCommand GetMachinesCommand { get; private set; }
        public AsyncCommand GetMachinesByIdCommand { get; private set; }
        public AsyncCommand RefreshCommand { get; private set; }
        public AsyncCommand FilterMachinesCommand { get; private set; }
        public AsyncCommand<Machine> SelectedCommand { get; private set; }

        private IMachineService _machineService;

        private string backgroundColor;
        public string BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }

        public MachinesListViewModel()
        {
            Title = "Machines view";
            allMachines = new ObservableCollection<Machine>();
            machines = new ObservableCollection<Machine>();
            GetMachinesCommand = new AsyncCommand(GetMachinesFromService);
            GetMachinesByIdCommand = new AsyncCommand(GetMachinesByIdFromService);
            FilterMachinesCommand = new AsyncCommand(FilterMachineList);
            SelectedCommand = new AsyncCommand<Machine>(Selected);

            RefreshCommand = new AsyncCommand(InitMethod);

            try
            {
                _machineService = DependencyService.Get<IMachineService>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            BackgroundColor = AppSettingsManager.Settings["background_color"];
        }

        private async Task InitMethod()
        {
            IsBusy = true;

            //HelperMethods.CheckDNS();

            if(_machineService == null)
            {
                try
                {
                    _machineService = DependencyService.Get<IMachineService>();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            await GetMachinesFromService();
            
            IsBusy = false;
        }

        private async Task GetMachinesFromService()
        {
            try
            {
                Machines = allMachines = await _machineService.GetMachines();
                FilterEntry = "";
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Server error", ex.ToString(), "OK");
            }   
        }

        private async Task GetMachinesByIdFromService()
        {
            //await processService.GetMachineById(id);
        }

        private async Task FilterMachineList()
        {
            Machines = (FilterEntry == "") ? allMachines : new ObservableCollection<Machine>(allMachines.Where(x => x.Name.Contains(FilterEntry)));
        }

        async Task Selected(Machine machine)
        {
            if (machine == null)
                return;

            if(machine.Id == null)
            {
                Debug.WriteLine("Machine GUID is null while opening MachineDetailsPage.");
                return;
            }        

            var detailsRoute = $"{nameof(MachineDetailsPage)}?SelectedMachineID={machine.Id}";
            await Shell.Current.GoToAsync(detailsRoute);

            //baca exception iz nekog razloga
            //SelectedMachine = null;
        }
    }
}
