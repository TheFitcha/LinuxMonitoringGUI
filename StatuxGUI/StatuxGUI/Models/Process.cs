using System;
using System.Collections.Generic;
using System.Text;

namespace StatuxGUI.Models
{
    public class Process
    {
        public string Id { get; set; }
        public int ProcessIdSystem { get; set; }
        public string Name { get; set; }
        public string MachineId { get; set; }
    }
}
