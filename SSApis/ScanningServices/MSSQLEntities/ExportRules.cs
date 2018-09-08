using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class ExportRules
    {
        public int JobId { get; set; }
        public string DirectoryFormat { get; set; }
        public string FileNameFormat { get; set; }
        public string DirectoryReplaceRule { get; set; }
        public string FileNameReplaceRule { get; set; }
        public string FieldsReplaceRule { get; set; }
        public string OutputFileFormat { get; set; }
        public string OutputFileDelimeter { get; set; }
        public string OutputFields { get; set; }
        public string MetadataFileName { get; set; }
        public string MetadataOutputDirectoryFormat { get; set; }
        public string UseQuotationAroundFlag { get; set; }
        public string IncludeHeaderFlag { get; set; }
    }
}
