using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Projects
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CustomerId { get; set; }
    }
}
