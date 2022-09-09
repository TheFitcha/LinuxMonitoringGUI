using StatuxGUI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StatuxGUI.Services
{
    public interface IMachineService
    {
        Task<Machine> GetMachineById(string id);
        Task<ObservableCollection<Machine>> GetMachines();
    }
}