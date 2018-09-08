using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Processes
    {
        public int ProcessId { get; set; }
        public int JobId { get; set; }
        public int StationId { get; set; }
        public int PdfstationId { get; set; }
        public string EnableFlag { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }
    }
}
