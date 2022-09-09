using System;
using System.Collections.Generic;
using System.Text;

namespace StatuxGUI.Models
{
    public class ProcessStatus
    {
        public string ProcessId { get; set; }
        public DateTime Time { get; set; }
        public string State { get; set; }
        public float CpuUtil { get; set; }
        public int Threads { get; set; }
    }
}
