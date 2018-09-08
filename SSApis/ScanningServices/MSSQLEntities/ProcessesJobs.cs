using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class ProcessesJobs
    {
        public int ProcessId { get; set; }
        public int JobId { get; set; }
        public string EnableFlag { get; set; }
    }
}
