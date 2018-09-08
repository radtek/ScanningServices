using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Vfrdocs
    {
        public string BatchNumber { get; set; }
        public string DocumentId { get; set; }
        public string StatusFlag { get; set; }
        public DateTime CreationDate { get; set; }
        public string JobId { get; set; }
    }
}
