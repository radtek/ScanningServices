using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Smtp
    {
        public string HostName { get; set; }
        public int PortNumber { get; set; }
        public string EnableSslflag { get; set; }
        public string SenderEmailAddress { get; set; }
        public string SenderName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
    }
}
