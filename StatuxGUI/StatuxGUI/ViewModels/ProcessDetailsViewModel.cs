using Microcharts;
using MvvmHelpers;
using MvvmHelpers.Commands;
using StatuxGUI.Models;
using StatuxGUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StatuxGUI.ViewModels
{
    [QueryProperty(nameof(SelectedProcessID), nameof(SelectedProcessID))]
    public class ProcessDetailsViewModel : BaseViewModel
    {
        private string selectedProcessID;
        public string SelectedProcessID
        {
            get => selectedProcessID;
            set => SetProperty(ref selectedProcessID, value);
        }

        private Process selectedProcess;
        public Process SelectedProcess
        {
            get => selectedProcess;
            set => SetProperty(ref selectedProcess, value);
        }
        private readonly IProcessService _processService;

        private ObservableRangeCollection<ProcessStatus> processDetails;
        public ObservableRangeCollection<ProcessStatus> ProcessDetails
        {
            get => processDetails;
            set => SetProperty(ref processDetails, value);
        }

        private LineChart processPerformanceChart;
        public LineChart ProcessPerformanceChart
        {
            get => processPerformanceChart;
            set => SetProperty(ref processPerformanceChart, value);
        }

        public AsyncCommand RefreshCommand { get; private set; }
        public AsyncCommand FilterCommand { get; private set; }

        private string backgroundColor;
        public string BackgroundColor
        {
            get => backgroundColor;
            set => SetProperty(ref backgroundColor, value);
        }

        private string chartBackgroundColor;
        public string ChartBackgroundColor
        {
            get => chartBackgroundColor;
            set => SetProperty(ref chartBackgroundColor, value);
        }

        public ProcessDetailsViewModel()
        {
            _processService = DependencyService.Get<IProcessService>();

            RefreshCommand = new AsyncCommand(Refresh);
            FilterCommand = new AsyncCommand(FilterByDate);

            Device.StartTimer(new TimeSpan(0, 0, 10), () =>
            {
                Task.Run(async () =>
                {
                    await GetProcessInfo();
                    await GetProcessDetails();
                });  
                return true;
            });

            BackgroundColor = AppSettingsManager.Settings["background_color"];
            ChartBackgroundColor = AppSettingsManager.Settings["cpu_chart_background_color"];
        }

        public async Task GetProcessInfo()
        {
            try
            {
                SelectedProcess = await _processService.GetProcessByID(SelectedProcessID);
            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task GetProcessDetails()
        {
            ProcessDetails = await _processService.GetProcessDetailsById(SelectedProcessID);
            InitProcessChartData();
        }

        private async Task Refresh()
        {
            IsBusy = true;
            await GetProcessInfo();
            await GetProcessDetails();
            IsBusy = false;
        }

        private async Task FilterByDate()
        {
            //var beginDate = await App.Current.MainPage.DisplayPromptAsync("From Datetime");
        }

        private void InitProcessChartData()
        {
            //potencijalno rjesenje ako zelim izbjeci svako iteriranje kroz processStatus
            /*if (ProcessDetails == null)
            {
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    InitProcessChartData();
                    return false;
                });
                return;
            }*/

            var processDetailsEntries = new List<ChartEntry>();

            foreach(var dataEntry in ProcessDetails.Skip(Math.Max(0, ProcessDetails.Count - 6)))
            {
                processDetailsEntries.Add(new ChartEntry(dataEntry.CpuUtil)
                {
                    Color = SkiaSharp.SKColor.Parse("#037bfc"),
                    ValueLabel = dataEntry.CpuUtil.ToString() + " %",
                    Label = "CPU"
                });
            }

            ProcessPerformanceChart = new LineChart
            {
                Entries = processDetailsEntries,
                LabelTextSize = 20f,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SkiaSharp.SKColor.Parse(ChartBackgroundColor)
            };
        }
    }
}
