using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class JobsProcessesDelete
    {
        public int ProcessId { get; set; }
        public int JobId { get; set; }
        public int? StationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Schedule { get; set; }
        public string EnableFlag { get; set; }
        public string SupersedeFlag { get; set; }
        public int? WatchFolderId { get; set; }
        public int? TargetFolderId { get; set; }
        public int? BackupFolderId { get; set; }
    }
}
