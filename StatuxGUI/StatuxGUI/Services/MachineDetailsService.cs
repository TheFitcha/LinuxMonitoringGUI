using MvvmHelpers;
using Newtonsoft.Json;
using StatuxGUI.Models;
using StatuxGUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(MachineDetailsService))]
namespace StatuxGUI.Services
{
    public class MachineDetailsService : IMachineDetailsService
    {
        private readonly HttpClient client;
        public MachineDetailsService()
        {
            client = HelperMethods.CreateHttpClient();
        }

        public async Task<ObservableRangeCollection<Core>> GetCoresInfo(string processorId)
        {
            try
            {
                var jsonCores = await client.GetStringAsync($"cpuCore/{processorId}");
                var cores = JsonConvert.DeserializeObject<IEnumerable<Core>>(jsonCores);
                return new ObservableRangeCollection<Core>(cores);
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

        public async Task<Processor> GetProcessorInfo(string machineId)
        {
            try
            {
                var jsonProcessor = await client.GetStringAsync($"cpuInfo/{machineId}");
                if (jsonProcessor.Contains("QueryResultError")) //naci bolji nacin za provjeriti je li error
                {
                    return new Processor
                    {
                        Name = "Processor is not registered with this machine!"
                    };
                }
                var processor = JsonConvert.DeserializeObject<Processor>(jsonProcessor);
                return processor;
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (JsonException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Memory> GetMemoryInfo(string machineId)
        {
            try
            {
                var jsonMemory = await client.GetStringAsync($"memory/{machineId}");
                if (jsonMemory.Contains("QueryResultError")) //naci bolji nacin za provjeriti je li error
                {
                    return new Memory();
                }
                var memory = JsonConvert.DeserializeObject<Memory>(jsonMemory);
                return memory;
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (JsonException)
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
