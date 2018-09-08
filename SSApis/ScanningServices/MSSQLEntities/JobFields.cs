using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class JobFields
    {
        public string CpfieldName { get; set; }
        public string VfrfieldName { get; set; }
        public int FieldId { get; set; }
        public int JobId { get; set; }
        public string KeyStrokeInclude { get; set; }
    }
}
