using MvvmHelpers;
using StatuxGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace StatuxGUI.Services
{
    public interface IMachineDetailsService
    {
        Task<Processor> GetProcessorInfo(string machineId);
        Task<ObservableRangeCollection<Core>> GetCoresInfo(string processorId);
        Task<Memory> GetMemoryInfo(string machineId);
    }
}
