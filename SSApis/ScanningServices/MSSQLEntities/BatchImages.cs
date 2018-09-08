using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class BatchImages
    {
        public string BatchName { get; set; }
        public string DocumentId { get; set; }
        public string PageId { get; set; }
        public string ImageSequenceNumber { get; set; }
        public string ImageLocation { get; set; }
        public string ImageFilenameWithFullPath { get; set; }
        public string ImageFilename { get; set; }
    }
}
