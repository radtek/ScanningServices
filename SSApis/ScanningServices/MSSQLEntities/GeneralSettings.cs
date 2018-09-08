using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class GeneralSettings
    {
        public int Id { get; set; }
        public string DebugFlag { get; set; }
        public string CpapplicationFilePath { get; set; }
        public string ImageViewerFilePath { get; set; }
        public string Dbserver { get; set; }
        public string DbuserName { get; set; }
        public string Dbpassword { get; set; }
        public string Dbprovider { get; set; }
        public string Dbname { get; set; }
        public string Dbrdbms { get; set; }
        public string CdiWebUrl { get; set; }
        public int? AutoImportWatchFolderId { get; set; }
        public int? ScanningFolderId { get; set; }
        public int? PostValidationWatchFolderId { get; set; }
        public int? LoadBalancerWatchFolderId { get; set; }
        public int? BackupFolderId { get; set; }
        public int? FileConversionWatchFolderId { get; set; }
        public int? BatchDeliveryWatchFolderId { get; set; }
        public int? RestingLocationId { get; set; }
        public int? VfrrenamerWatchFolderId { get; set; }
        public int? VfrduplicateRemoverWatchFolderId { get; set; }
        public int? VfrbatchUploaderWatchFolderId { get; set; }
        public int? VfrbatchMonitorFolderId { get; set; }
        public int? QcoutputFolderId { get; set; }
    }
}
