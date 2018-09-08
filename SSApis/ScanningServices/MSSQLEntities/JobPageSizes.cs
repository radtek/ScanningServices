using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class JobPageSizes
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string CategoryName { get; set; }
        public float Width { get; set; }
        public float High { get; set; }
    }
}
