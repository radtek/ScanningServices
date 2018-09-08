using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class BatchControl
    {
        public string BatchNumber { get; set; }
        public int? LotNumber { get; set; }
        public int? BlockNumber { get; set; }
        public int? NumberOfDocuments { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string StatusFlag { get; set; }
        public string ScanOperator { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? RejectedTimes { get; set; }
        public DateTime? LastTimeRejected { get; set; }
        public string ApprovedBy { get; set; }
        public int? NumberOfPages { get; set; }
        public string RejectedBy { get; set; }
        public string RejectionReason { get; set; }
        public int? NumberOfScannedPages { get; set; }
        public string DeptName { get; set; }
        public string ProjectName { get; set; }
        public string DocumentPath { get; set; }
        public DateTime? ExportedDate { get; set; }
        public string ExportedBy { get; set; }
        public float? BatchSize { get; set; }
        public int? ExportedTimes { get; set; }
        public string SubDeptName { get; set; }
        public string FileStatus { get; set; }
        public DateTime? ScannedDate { get; set; }
        public string Qcby { get; set; }
        public string Qcstation { get; set; }
        public DateTime? Qcdate { get; set; }
        public string OutputBy { get; set; }
        public DateTime? OutputDate { get; set; }
        public string OutputStation { get; set; }
        public string ScanStation { get; set; }
        public string KodakStatus { get; set; }
        public string JobType { get; set; }
        public string ModifiedStation { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string KodakErrorState { get; set; }
        public string Comments { get; set; }
        public int? ScannedPagesReturned { get; set; }
        public int CaptureTime { get; set; }
        public int? ScanningStageTime { get; set; }
        public int? QcstageTime { get; set; }
        public int? ScanningTime { get; set; }
        public int? Qctime { get; set; }
        public DateTime? ScanningEndTime { get; set; }
        public DateTime? QcendTime { get; set; }
        public DateTime? QcstartTime { get; set; }
        public string TaskOrder { get; set; }
        public string PrepUserName { get; set; }
        public DateTime? PrepDate { get; set; }
        public string Qarflag { get; set; }
        public int? RecallTimes { get; set; }
        public DateTime? RecallDate { get; set; }
        public string RecallBy { get; set; }
        public string RecallReason { get; set; }
        public string Customer { get; set; }
        public int? KeysStrokes { get; set; }
        public string BatchAlias { get; set; }
        public DateTime? VfruploadDate { get; set; }
        public int? InitialNumberOfDocuments { get; set; }
        public int? InitialNumberOfPages { get; set; }
        public int? InitialNumberOfScannedPages { get; set; }
        public DateTime? VfruploadModifiedDate { get; set; }
        public int? ImageCountGrayscale { get; set; }
        public int? ImageCountBlackWhite { get; set; }
        public int? ImageCountGrayscaleBack { get; set; }
        public int? ImageCountGrayscaleFront { get; set; }
        public int? ImageCountBlackWhiteBack { get; set; }
        public int? ImageCountBlackWhiteFront { get; set; }
        public int? FrontsCaptured { get; set; }
        public int? FrontsRemoved { get; set; }
        public int? FrontsDeleted { get; set; }
        public int? FrontsRescanned { get; set; }
        public int? BacksCaptured { get; set; }
        public int? BacksRemoved { get; set; }
        public int? BacksDeleted { get; set; }
        public int? BacksRescanned { get; set; }
        public float? PrepTime { get; set; }
        public DateTime? VfruploadRequestedDate { get; set; }
        public DateTime? VfruploadCompletedDate { get; set; }
        public string VfrstatusFlag { get; set; }
        public string PageSizesCount { get; set; }
    }
}
