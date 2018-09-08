using System;
using System.Collections.Generic;

namespace ScanningServices.MSSQLEntities
{
    public partial class Reports
    {
        public int ReportId { get; set; }
        public int TemplateId { get; set; }
        public int CustomerId { get; set; }
        public string EnableFlag { get; set; }
        public string EmailSubject { get; set; }
        public string TitleContent1 { get; set; }
        public string TitleContent2 { get; set; }
        public string TitleContent3 { get; set; }
        public string TitleFontColor1 { get; set; }
        public string TitleFontColor2 { get; set; }
        public string TitleFontColor3 { get; set; }
        public int? TitleFontSize1 { get; set; }
        public int? TitleFontSize2 { get; set; }
        public int? TitleFontSize3 { get; set; }
        public string TitleFontBoldFlag1 { get; set; }
        public string TitleFontBoldFlag2 { get; set; }
        public string TitleFontBoldFlag3 { get; set; }
        public string EmailRecipients { get; set; }
        public string TableHeaderFontColor { get; set; }
        public string TableHeaderFontBoldFlag { get; set; }
        public string TableHeaderBackColor { get; set; }
        public int? TableHeaderFontSize { get; set; }
        public string TableColumnNamesFontColor { get; set; }
        public string TableColumnNamesFontBoldFlag { get; set; }
        public string TableColumnNamesBackColor { get; set; }
        public int? TableColumnNamesFontSize { get; set; }
        public string ScheduleTime { get; set; }
        public string StationName { get; set; }
    }
}
