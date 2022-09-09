using MvvmHelpers;
using StatuxGUI.Models;
using System.Threading.Tasks;

namespace StatuxGUI.Services
{
    public interface IProcessService
    {
        Task<ObservableRangeCollection<Process>> GetAllProcesses(string machineId);
        Task<Process> GetProcessByID(string processID);
        Task<ObservableRangeCollection<ProcessStatus>> GetProcessDetailsById(string processId);
    }
}