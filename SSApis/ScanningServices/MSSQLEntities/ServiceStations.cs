using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class ServiceStations
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public string PdfstationFlag { get; set; }
        public int? WatchFolderId { get; set; }
        public int? TargetFolderId { get; set; }
        public int? BackupFolderId { get; set; }
        public int? MaxNumberBatches { get; set; }
        public string WeekendFlag { get; set; }
        public string WorkdayFlag { get; set; }
        public string WeekendStartTime { get; set; }
        public string WeekendEndTime { get; set; }
        public string WorkdayStartTime { get; set; }
        public string WorkdayEndTime { get; set; }
    }
}
