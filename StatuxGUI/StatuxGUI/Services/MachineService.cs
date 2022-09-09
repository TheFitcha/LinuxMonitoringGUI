using Newtonsoft.Json;
using StatuxGUI.Models;
using StatuxGUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly:Dependency(typeof(MachineService))]
namespace StatuxGUI.Services
{
    public class MachineService : IMachineService
    {
        private readonly HttpClient client;
        public MachineService()
        {
            client = HelperMethods.CreateHttpClient();
        }

        public async Task<ObservableCollection<Machine>> GetMachines()
        {
            try
            {
                var jsonMachines = await client.GetStringAsync("machine");
                var machines = JsonConvert.DeserializeObject<IEnumerable<Machine>>(jsonMachines);
                return new ObservableCollection<Machine>(machines);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Machine> GetMachineById(string id)
        {
            try
            {
                var jsonMachine = await client.GetStringAsync($"machine/{id}");
                return JsonConvert.DeserializeObject<Machine>(jsonMachine);
            }
            catch(Exception)
            {
                throw;
            }       
        }
    }
}
