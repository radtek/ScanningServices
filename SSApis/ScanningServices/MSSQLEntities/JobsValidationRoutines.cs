using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class JobsValidationRoutines
    {
        public int JobId { get; set; }
        public int RoutineId { get; set; }
        public string OperatorType { get; set; }
        public string RoutineName { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationLevel { get; set; }
        public string RoutineType { get; set; }
    }
}
