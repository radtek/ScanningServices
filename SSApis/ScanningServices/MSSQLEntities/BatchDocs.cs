using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class BatchDocs
    {
        public string DocumentLocation { get; set; }
        public string DocumentId { get; set; }
        public int ImageCountInDocument { get; set; }
        public int PageCountInDocument { get; set; }
        public string BatchName { get; set; }
        public string BatchLocation { get; set; }
        public double BatchSize { get; set; }
        public DateTime CreatedDateAndTime { get; set; }
        public string CreatedStationId { get; set; }
        public string CreatedStationName { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime OutputDateAndTime { get; set; }
        public string OutputStationId { get; set; }
        public string OutputStationName { get; set; }
        public string OutputUserId { get; set; }
        public DateTime LastModifiedDateAndTime { get; set; }
        public string LastModifiedStationId { get; set; }
        public string LastModifiedStationName { get; set; }
        public string LastModifiedUserId { get; set; }
        public string StartingDocumentId { get; set; }
        public string FirstDocumentId { get; set; }
        public string LastDocumentId { get; set; }
        public int DocumentCountInBatch { get; set; }
        public int PageCountInBatch { get; set; }
        public int ImageCountInBatch { get; set; }
        public int BlackAndWhiteImageCount { get; set; }
        public int ColorImageCount { get; set; }
        public int GrayscaleImageCount { get; set; }
        public int ImagesCapturedFront { get; set; }
        public int ImagesRescannedFront { get; set; }
        public int ImagesRemovedForBlankFront { get; set; }
        public int ImageDeletedFront { get; set; }
        public int ImagesCapturedBack { get; set; }
        public int ImagesRescannedBack { get; set; }
        public int ImagesRemovedForBlankBack { get; set; }
        public int ImageDeletedBack { get; set; }
        public string DocumentSequenceNumber { get; set; }
        public string DocumentFilenameWithFullPath { get; set; }
        public string DocumentFilename { get; set; }
        public string DocumentSize { get; set; }
        public string CustomerField1 { get; set; }
        public string CustomerField2 { get; set; }
        public string CustomerField3 { get; set; }
        public string CustomerField4 { get; set; }
        public string CustomerField5 { get; set; }
        public string CustomerField6 { get; set; }
        public string CustomerField7 { get; set; }
        public string CustomerField8 { get; set; }
        public string CustomerField9 { get; set; }
        public string CustomerField10 { get; set; }
        public string Customer { get; set; }
        public int? Keystrokes { get; set; }
        public string CustomerField11 { get; set; }
        public string CustomerField12 { get; set; }
        public string CustomerField13 { get; set; }
        public string CustomerField14 { get; set; }
        public string CustomerField15 { get; set; }
    }
}
