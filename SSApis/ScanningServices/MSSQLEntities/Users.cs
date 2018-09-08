using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        public string CreateBatches { get; set; }
        public string ApproveBatches { get; set; }
        public string RejectBatches { get; set; }
        public string ExportBatches { get; set; }
        public string ViewBatchInformation { get; set; }
        public string ViewDocumentInformation { get; set; }
        public string GenerateBatchReport { get; set; }
        public string GenerateOperationReports { get; set; }
        public string Description { get; set; }
        public string UploadBatches { get; set; }
        public string ScanBatches { get; set; }
        public string QualityControl { get; set; }
        public string ReturnBatches { get; set; }
        public string ActiveFlag { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
