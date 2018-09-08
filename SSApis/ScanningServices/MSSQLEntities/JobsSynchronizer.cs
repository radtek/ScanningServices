using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class JobsSynchronizer
    {
        public int JobId { get; set; }
        public string EnableFlag { get; set; }
        public string LogFileName { get; set; }
    }
}
