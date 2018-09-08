using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class BatchTracking
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; }
        public DateTime Date { get; set; }
        public string InitialStatus { get; set; }
        public string FinalStatus { get; set; }
        public string OperatorId { get; set; }
        public string StationId { get; set; }
        public string Event { get; set; }
    }
}
