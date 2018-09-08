using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class ReportParameters
    {
        public int ReportId { get; set; }
        public int TemplateId { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }
    }
}
