using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class ReportsTemplate
    {
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        public string EnableSchedulerFlag { get; set; }
    }
}
