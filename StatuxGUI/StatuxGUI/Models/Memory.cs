using System;
using System.Collections.Generic;
using System.Text;

namespace StatuxGUI.Models
{
    public class Memory
    {
        public string MachineId { get; set; }
        public ulong TotalPhysicalMemoryKb { get; set; }    
        public ulong TotalSwapMemoryKb { get; set; }
        public ulong FreePhysicalMemoryKb { get; set; }
        public ulong FreeSwapMemoryKb { get; set; }
    }
}
