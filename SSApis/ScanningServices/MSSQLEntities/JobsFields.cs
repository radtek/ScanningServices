using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class JobsFields
    {
        public string CpfieldName { get; set; }
        public string VfrfieldName { get; set; }
        public int FieldId { get; set; }
        public int JobId { get; set; }
        public string KeyStrokeExcludeFlag { get; set; }
    }
}
