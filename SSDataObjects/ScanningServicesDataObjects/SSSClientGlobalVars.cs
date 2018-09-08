using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ScanningServicesDataObjects
{
    public class SSSClientGlobalVars
    {

        public class DatabaseSTR
        {
            public string Server { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Provider { get; set; }
            public string DatabaseName { get; set; }
            public string RDBMS { get; set; }
        }

        public class EmailSTR
        {
            public string Host { get; set; }
            public string Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public Boolean EnableSSL { get; set; }
            public string SenderAddress { get; set; }
            public string SenderName { get; set; }
            public List<string> Recipients { get; set; }
        }

        public class KodakSTR
        {
            public List<string> JobTypes { get; set; }
            public List<string> CaptureProScanDirectories { get; set; }
            public List<string> JobOutpts { get; set; }
            public List<string> StationsID { get; set; }

        }

        public class ReportTitleSTR
        {
            public string Content { get; set; }
            public string FontSize { get; set; }
            public string FontColor { get; set; }
            public string FontBold { get; set; }
        }

        public class ReportTableSTR
        {
            public string HeaderBackGroundColor { get; set; }
            public string HeaderFontColor { get; set; }
            public string HeaderFontSize { get; set; }
            public string HeaderFontBold { get; set; }
            public string RowFontColor { get; set; }
            public string RowFontSize { get; set; }
        }

        public class ReportParameterSTR
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class ReportSTR
        {
            public string Name { get; set; }
            public Boolean Enable { get; set; }
            public string Subject { get; set; }
            public ReportTitleSTR Title1 { get; set; }
            public ReportTitleSTR Title2 { get; set; }
            public ReportTitleSTR Title3 { get; set; }
            public ReportTableSTR ReportTable { get; set; }
            public List<string> Recipients { get; set; }
            public List<ReportParameterSTR> Parameters { get; set; }
        }

        public class VFRSTR
        {
            public string CADIWebServiceURL { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string InstanceName { get; set; }
            public string RepositoryName { get; set; }
            public string QueryField { get; set; }
        }

        public class StageSTR
        {
            public string Name { get; set; }
            public Boolean Enable { get; set; }
        }

        public class CustomerProjectSTR
        {
            public string Name { get; set; }
            public List<string> KeyStrokesExcludedFields { get; set; }
            public string ExportClass { get; set; }
            public VFRSTR VFR { get; set; }
            public List<StageSTR> Stages { get; set; }
        }

        public class CustomerSTR
        {
            public string Name { get; set; }
            public string BatchesLocation { get; set; }
            public List<CustomerProjectSTR> Projects { get; set; }
            public List<ReportSTR> Reports { get; set; }
        }

        public class ConfigSTR
        {
            public Boolean Debug { get; set; }
            public string LogFile { get; set; }
            public string CaptureApplication { get; set; }
            public string ImageViewer { get; set; }
            public DatabaseSTR Database { get; set; }
            public EmailSTR Email { get; set; }
            public KodakSTR Kodak { get; set; }
            public List<CustomerSTR> Customers { get; set; }
            public List<ReportSTR> Reports { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        public class ResultSSSConfig
        {
            /// <summary>
            /// This is the Response Code of a Http API Call
            /// </summary>
            public string HttpStatusCode { get; set; }
            /// <summary>
            /// Return Value { success = 0, failure = -1}
            /// </summary>
            public int ReturnCode { get; set; }
            /// <summary>
            /// Any Infofrmation 
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// "true"/"false"
            /// </summary>
            public ConfigSTR ReturnValue { get; set; }
            /// <summary>
            /// Exception Message
            /// </summary>
            public string Exception { get; set; }
            /// <summary>
            /// Mesure the Method Elapsed time in seconds
            /// </summary>
            public string ElapsedTime { get; set; }
            /// <summary>
            /// Return Value { success = 0, failure = -1}
            /// </summary>
            public int RecordsCount { get; set; }
        }

    }
}

