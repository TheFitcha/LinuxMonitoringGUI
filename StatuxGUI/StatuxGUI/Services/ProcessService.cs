using MvvmHelpers;
using Newtonsoft.Json;
using StatuxGUI.Models;
using StatuxGUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(ProcessService))]
namespace StatuxGUI.Services
{
    public class ProcessService : IProcessService
    {
        private HttpClient client;
        public ProcessService()
        {
            client = HelperMethods.CreateHttpClient();
        }

        public async Task<ObservableRangeCollection<Process>> GetAllProcesses(string machineId)
        {
            try
            {
                var jsonProcesses = await client.GetStringAsync($"processList/{machineId}");
                var processes = JsonConvert.DeserializeObject<IEnumerable<Process>>(jsonProcesses);
                return new ObservableRangeCollection<Process>(processes);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
        }

        public async Task<Process> GetProcessByID(string processID)
        {
            try
            {
                var jsonProcess = await client.GetStringAsync($"process/{processID}");
                var process = JsonConvert.DeserializeObject<IEnumerable<Process>>(jsonProcess);
                return process.FirstOrDefault();
            }
            catch (TaskCanceledException)
            {
                throw;
            }
        }

        public async Task<ObservableRangeCollection<ProcessStatus>> GetProcessDetailsById(string processId)
        {
            try
            {
                var jsonProcessStatus = await client.GetStringAsync($"processStatus/{processId}");
                var statuses = JsonConvert.DeserializeObject<IEnumerable<ProcessStatus>>(jsonProcessStatus);
                return new ObservableRangeCollection<ProcessStatus>(statuses);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }
    }
}
