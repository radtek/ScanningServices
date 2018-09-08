using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Vfr
    {
        public int SettingId { get; set; }
        public int JobId { get; set; }
        public string Cadiurl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string InstanceName { get; set; }
        public string RepositoryName { get; set; }
        public string CaptureTemplate { get; set; }
        public string QueryField { get; set; }
    }
}
