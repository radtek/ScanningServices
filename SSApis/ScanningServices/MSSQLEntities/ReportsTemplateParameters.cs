using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class ReportsTemplateParameters
    {
        public int TemplateId { get; set; }
        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string RequiredFlag { get; set; }
        public string Description { get; set; }
    }
}
