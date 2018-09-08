using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Jobs
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int ProjectId { get; set; }
        public string ExportClassName { get; set; }
        public string DepartmentName { get; set; }
        public int ScanningFolderId { get; set; }
        public int? AutoImportWatchFolderId { get; set; }
        public int? QcoutputFolderId { get; set; }
        public int? PostValidationWatchFolderId { get; set; }
        public int? LoadBalancerWatchFolderId { get; set; }
        public int? BackupFolderId { get; set; }
        public int? FileConversionWatchFolderId { get; set; }
        public int? BatchDeliveryWatchFolderId { get; set; }
        public int RestingLocationId { get; set; }
        public int? VfrrenamerWatchFolderId { get; set; }
        public int? VfrduplicateRemoverWatchFolderId { get; set; }
        public int? VfrbatchUploaderWatchFolderId { get; set; }
        public int? VfrbatchMonitorFolderId { get; set; }
        public string AutoImportEnableFlag { get; set; }
        public string PostValidationEnableFlag { get; set; }
        public string DeleteLoadBalancerEnableFlag { get; set; }
        public string FileConversionFlag { get; set; }
        public string DeleteBatchDeliveryEnableFlag { get; set; }
        public string VfrenableFlag { get; set; }
        public string OutputFileType { get; set; }
        public string MultiPageFlag { get; set; }
        public int? MaxBatchesPerWorkOrder { get; set; }
        public string BatchCleanupFlag { get; set; }
    }
}
