using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using MvvmHelpers;
using MvvmHelpers.Commands;
using StatuxGUI.Models;
using StatuxGUI.Services;
using Xamarin.Forms;

namespace StatuxGUI.ViewModels
{
    [QueryProperty(nameof(SelectedMachineID), nameof(SelectedMachineID))]
    public class MachineDetailsViewModel : BaseViewModel
    {
        private string selectedMachineID;
        public string SelectedMachineID
        {
            get => selectedMachineID;
            set => SetProperty(ref selectedMachineID, value);
        }

        private Machine currentMachine;
        public Machine CurrentMachine
        {
            get => currentMachine;
            set => SetProperty(ref currentMachine, value);
        }

        private readonly IMachineDetailsService _machineDetailsService;
        private readonly IMachineService _machineService;
        private Processor currentProcessor;
        public Processor CurrentProcessor
        {
            get => currentProcessor;
            set => SetProperty(ref currentProcessor, value);
        }

        private ObservableRangeCollection<Core> currentCores;
        public ObservableRangeCollection<Core> CurrentCores
        {
            get => currentCores;
            set => SetProperty(ref currentCores, value);
        }

        private Memory currentMemory;
        public Memory CurrentMemory
        {
            get => currentMemory;
            set => SetProperty(ref currentMemory, value);
        }

        public AsyncCommand RefreshCommand { get; private set; }

        private string backgroundColor;
        public string BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }
        public MachineDetailsViewModel()
        {
            RefreshCommand = new AsyncCommand(Refresh);

            _machineDetailsService = DependencyService.Get<IMachineDetailsService>();
            _machineService = DependencyService.Get<IMachineService>();

            BackgroundColor = AppSettingsManager.Settings["background_color"];

            // ...
            // NOTE: use for debugging, not in released app code!
            var assembly = typeof(MachineDetailsViewModel).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
        }

        private async Task Refresh()
        {
            IsBusy = true;
            await GetMachineInfo();
            await GetCPUInfo();
            await GetCoresInfo();
            await GetMemoryInfo();
            InitPhyicalMemoryChartData();
            InitSwapMemoryChartData();
            IsBusy = false;
        }

        public async Task GetMachineInfo()
        {
            try
            {
                CurrentMachine = await _machineService.GetMachineById(SelectedMachineID);
            }
            catch (System.Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public async Task GetCPUInfo()
        {
            try
            {
                CurrentProcessor = await _machineDetailsService.GetProcessorInfo(selectedMachineID);
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public async Task GetCoresInfo() 
        {
            try
            {
                if (CurrentProcessor.Id != null)
                {
                    CurrentCores = await _machineDetailsService.GetCoresInfo(currentProcessor.Id);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public async Task GetMemoryInfo()
        {
            try
            {
                CurrentMemory = await _machineDetailsService.GetMemoryInfo(selectedMachineID);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private RadialGaugeChart physicalMemoryChart;
        public RadialGaugeChart PhysicalMemoryChart
        {
            get => physicalMemoryChart;
            set => SetProperty(ref physicalMemoryChart, value);
        }

        private void InitPhyicalMemoryChartData()
        {
            var entries = new List<ChartEntry>();
            entries.Add(new ChartEntry(currentMemory.TotalPhysicalMemoryKb)
            {
                Color = SkiaSharp.SKColor.Parse("#52d9ff"),
                ValueLabel = currentMemory.TotalPhysicalMemoryKb.ToString() + " kB",
                Label = "Total Memory"
            });
            entries.Add(new ChartEntry(currentMemory.FreePhysicalMemoryKb)
            {
                Color = SkiaSharp.SKColor.Parse("#184bc4"),
                ValueLabel = currentMemory.FreePhysicalMemoryKb.ToString() + " kB",
                Label = "Free Memory"
            });

            PhysicalMemoryChart = new RadialGaugeChart()
            {
                Entries = entries,
                LabelTextSize = 40f,
                BackgroundColor = SkiaSharp.SKColor.Parse("#00FFFFFF")
            };
        }

        private RadialGaugeChart swapMemoryChart;
        public RadialGaugeChart SwapMemoryChart
        {
            get => swapMemoryChart;
            set => SetProperty(ref swapMemoryChart, value);
        }
        private void InitSwapMemoryChartData()
        {
            var entries = new List<ChartEntry>();
            entries.Add(new ChartEntry(currentMemory.TotalSwapMemoryKb)
            {
                Color = SkiaSharp.SKColor.Parse("#40ffcc"),
                ValueLabel = currentMemory.TotalSwapMemoryKb.ToString() + " kB",
                Label = "Total Swap"
            });
            entries.Add(new ChartEntry(currentMemory.FreeSwapMemoryKb)
            {
                Color = SkiaSharp.SKColor.Parse("#1ad966"),
                ValueLabel = currentMemory.FreeSwapMemoryKb.ToString() + " kB",
                Label = "Free Swap"
            });

            SwapMemoryChart = new RadialGaugeChart()
            {
                Entries = entries,
                LabelTextSize = 40f,
                BackgroundColor = SkiaSharp.SKColor.Parse("#00FFFFFF")
            };
        }

    }
}
