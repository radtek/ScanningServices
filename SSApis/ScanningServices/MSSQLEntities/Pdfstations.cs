using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Pdfstations
    {
        public int PdfstationId { get; set; }
        public int? WatchFolderId { get; set; }
        public int? TargetFolderId { get; set; }
        public int? BackupFolderId { get; set; }
        public int? MaxNumberBatches { get; set; }
        public string WeekendFlag { get; set; }
        public string WeekDayFlag { get; set; }
        public TimeSpan? WeekendStartTime { get; set; }
        public TimeSpan? WeekendEndTime { get; set; }
        public TimeSpan? WeekDayStartTime { get; set; }
        public TimeSpan? WeekDayEndTime { get; set; }
        public string EnableFlag { get; set; }
    }
}
