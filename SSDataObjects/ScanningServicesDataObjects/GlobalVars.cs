using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ScanningServicesDataObjects
{
    public class GlobalVars
    {

        /// <summary>
        /// To be used to stored the connection string value especified in th eappsettings.json file
        /// </summary>
        public static string connectionString { get; set; }

        public class PageSizeInfo
        {
            public string Category { get; set; }
            public int ImageCount { get; set; }
        }

        public class WorkOrderJobType
        {
            public string JobName { get; set; }
            public List<Batch> batches { get; set; }
            public int NumberBoxes { get; set; }
            public int NumberDocs { get; set; }
            public int NumberScannedImages { get; set; }
            public int NumberKeystrokes { get; set; }
            public int NumberBlankImages { get; set; }
            public double PrepTime { get; set; }
        }

        public class WorkOrderCustomer
        {
            public string CustomerName { get; set; }
            public List<WorkOrderJobType> JobTypeNames { get; set; }
            public int NumberBoxes { get; set; }
            public int NumberDocs { get; set; }
            public int NumberScannedImages { get; set; }
            public int NumberKeystrokes { get; set; }
            public int NumberBlankImages { get; set; }
            public double PrepTime { get; set; }
        }

        public class ReportParameter
        {
            public int TemplateID { get; set; }
            public int ParameterID { get; set; }
            public int ReportID { get; set; }
            public string ParameterName { get; set; }
            public string Value { get; set; }
        }

        public class Report
        {
            public string ReportName { get; set; }
            public int ReportID { get; set; }
            public int TemplateID { get; set; }
            public int CustomerID { get; set; }
            public string CustomerName { get; set; }
            public Boolean EnableFlag { get; set; }
            public ScheduleTime Schedule { get; set; }
            public string ScheduleCronFormat { get; set; }
            public string StationName { get; set; }
            public string EmailSubject { get; set; }
            public string TitleContent1 { get; set; }
            public string TitleContent2 { get; set; }
            public string TitleContent3 { get; set; }
            public string TitleFontColor1 { get; set; }
            public string TitleFontColor2 { get; set; }
            public string TitleFontColor3 { get; set; }
            public int TitleFontSize1 { get; set; }
            public int TitleFontSize2 { get; set; }
            public int TitleFontSize3 { get; set; }
            public string TableHeaderFontColor { get; set; }
            public Boolean TableHeaderFontBoldFlag { get; set; }
            public string TableHeaderBackColor { get; set; }
            public int TableHeaderFontSize { get; set; }
            public string TableColumnNamesFontColor { get; set; }
            public Boolean TableColumnNamesFontBoldFlag { get; set; }
            public string TableColumnNamesBackColor { get; set; }
            public int TableColumnNamesFontSize { get; set; }
            public Boolean TitleFontBoldFlag1 { get; set; }
            public Boolean TitleFontBoldFlag2 { get; set; }
            public Boolean TitleFontBoldFlag3 { get; set; }
            public string EmailRecipients { get; set; }
            public List<ReportParameter> Parameters { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultReports
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
            public List<Report> ReturnValue { get; set; }
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

        public class UIFunctionality
        {
            public int FunctionalityID { get; set; }
            public string Description { get; set; }
        }

        public class ResultUIFunctionalities
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
            public List<UIFunctionality> ReturnValue { get; set; }
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

        public class User
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Title { get; set; }
            public Boolean ActiveFlag { get; set; }
            public List<UIFunctionality> UIFunctionality { get; set; }
            public string Email { get; set; }
        }

        public class ResultUsers
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
            public List<User> ReturnValue { get; set; }
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

        public class JobPageSize
        {
            public int ID { get; set; }
            public int JobID { get; set; }
            public string CategoryName { get; set; }
            public double High { get; set; }
            public double Width { get; set; }
            public double Area { get; set; }
            public int TotalCounnt { get; set; }
        }

        public class ResultJobPageSizes
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
            public List<JobPageSize> ReturnValue { get; set; }
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


        public class ReportTemplateParameter
        {
            public int TemplateID { get; set; }
            public int ParameterID { get; set; }
            public string ParameterName { get; set; }
            public string Name { get; set; }
            public Boolean RequiredFlag { get; set; }
            public string Description { get; set; }
            public string DataType { get; set; }
        }

        public class ReportTemplate
        {
            public int TemplateID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Notes { get; set; }
            public string Type { get; set; }
            public List<ReportTemplateParameter> Parameters { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultReportsTemplate
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
            public List<ReportTemplate> ReturnValue { get; set; }
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
        
        public class Field
        {
            public int JobID { get; set; }
            public int FieldID { get; set; }
            public string CPFieldName { get; set; }
            public string VFRFieldName { get; set; }
            public Boolean ExcludeFromKeystrokesCount { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultFields
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
            public List<Field> ReturnValue { get; set; }
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

        public class EMAIL
        {
            //public string SenderEmailAddress { get; set; }
            public string RecipientsEmailAddress { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public string SenderName { get; set; }
            public string SenderEmailAddress { get; set; }
            public byte[] attachment { get; set; }
            public Boolean HasAttachment { get; set; }
        }

        public class SMTP
        {
            public string HostName { get; set; }            
            public int PortNumber { get; set; }
            public Boolean EnableSSLFlag { get; set; }
            public string SenderEmailAddress { get; set; }
            public string SenderName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultSMTP
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
            public SMTP ReturnValue { get; set; }
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

        public class VFR
        {
            public string CADIUrl { get; set; }
            public int SettingID { get; set; }
            public int JobID { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string InstanceName { get; set; }
            public string RepositoryName { get; set; }
            public string CaptureTemplate { get; set; }
            public string QueryField { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultsVFR
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
            public VFR ReturnValue { get; set; }
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

        public class Customer
        {
            public int CustomerID { get; set; }
            [Required()]
            public string CustomerName { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultCustomers
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
            public List<Customer> ReturnValue { get; set; }
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

        public class Project
        {
            public int ProjectID { get; set; }
            public string ProjectName { get; set; }
            public int CustomerID { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultProjects
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
            public List<GlobalVars.Project> ReturnValue { get; set; }
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

        public class JobExtended
        {
            public int JobID;
            [Required]
            public int ProjectID { get; set; }
            [Required]
            public string JobName { get; set; }
            public string ExportClassName { get; set; }
            public string DepartmentName { get; set; }
            public string ProjectName { get; set; }
            public string CustomerName { get; set; }
            public int AutoImportWatchFolderID { get; set; }
            public int ScanningFolderID { get; set; }
            public int QCOuputFolderID { get; set; }
            public int PostValidationWatchFolderID { get; set; }
            public int LoadBalancerWatchFolderID { get; set; }
            public int BackupFolderID { get; set; }
            public int FileConversionWatchFolderID { get; set; }
            public int BatchDeliveryWatchFolderID { get; set; }
            public int RestingLocationID { get; set; }
            public int VFRRenamerWatchFolderID { get; set; }
            public int VFRDuplicateRemoverWatchFolderID { get; set; }
            public int VFRBatchUploaderWatchFolderID { get; set; }
            public int VFRBatchMonitorFolderID { get; set; }
            public Boolean AutoImportEnableFlag { get; set; }
            public Boolean PostValidationEnableFlag { get; set; }
            //public Boolean LoadBalancerEnableFlag { get; set; }
            //public Boolean BatchDeliveryEnableFlag { get; set; }
            public Boolean FileConversionEnableFlag { get; set; }
            public Boolean VFREnableFlag { get; set; }
            public Boolean MultiPageFlag { get; set; }
            public string OutputFileType { get; set; } // PDF , Searchabel PDF ,or TIF
            public string AutoImportWatchFolder { get; set; }
            public string ScanningFolder { get; set; }
            public string QCOuputFolder { get; set; }
            public string PostValidationWatchFolder { get; set; }
            public string LoadBalancerWatchFolder { get; set; }
            public string BackupFolder { get; set; }
            public string FileConversionWatchFolder { get; set; }
            public string BatchDeliveryWatchFolder { get; set; }
            public string RestingLocation { get; set; }
            public string VFRRenamerWatchFolder { get; set; }
            public string VFRDuplicateRemoverWatchFolder { get; set; }
            public string VFRBatchUploaderWatchFolder { get; set; }
            public string VFRBatchMonitorFolder { get; set; }
            public int MaxBatchesPerWorkOrder { get; set; }
            public Boolean BatchCleanupFlag { get; set; }
        }

        public class ResultJobsExtended
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
            public List<GlobalVars.JobExtended> ReturnValue { get; set; }
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

        public class Job
        {
            public int JobID;
            [Required]
            public int ProjectID { get; set; }
            [Required]
            public string JobName { get; set; }
            public string ExportClassName { get; set; }
            public string DepartmentName { get; set; }
            public string ProjectName { get; set; }
            public string CustomerName { get; set;}
            public int ScanningFolderID { get; set; }
            public int AutoImportWatchFolderID { get; set; }
            public int QCOuputFolderID { get; set; }
            public int PostValidationWatchFolderID { get; set; }
            public int LoadBalancerWatchFolderID { get; set; }
            public int BackupFolderID { get; set; }
            public int FileConversionWatchFolderID { get; set; }
            public int BatchDeliveryWatchFolderID { get; set; }
            public int RestingLocationID { get; set; }
            public int VFRRenamerWatchFolderID { get; set; }
            public int VFRDuplicateRemoverWatchFolderID { get; set; }
            public int VFRBatchUploaderWatchFolderID { get; set; }
            public int VFRBatchMonitorFolderID { get; set; }
            public Boolean AutoImportEnableFlag { get; set; }
            public Boolean PostValidationEnableFlag { get; set; }
            //public Boolean LoadBalancerEnableFlag { get; set; }
            //public Boolean BatchDeliveryEnableFlag { get; set; }
            public Boolean FileConversionEnableFlag { get; set; }
            public Boolean VFREnableFlag { get; set; }
            public Boolean MultiPageFlag { get; set; }
            public string OutputFileType { get; set; } // PDF , Searchabel PDF ,or TIF
            public int MaxBatchesPerWorkOrder { get; set; }
            public Boolean BatchCleanupFlag { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultJobs
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
            public List<GlobalVars.Job> ReturnValue { get; set; }
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

        /// <summary>
        /// General Data structure to be used to return the result of Methods
        /// </summary>
        public class ResultGeneric
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
            public Boolean BooleanReturnValue { get; set; }
            /// <summary>
            /// any string
            /// </summary>
            public string StringReturnValue { get; set; }
            /// <summary>
            /// any integer
            /// </summary>
            public int IntegerNumberReturnValue { get; set; }
            /// <summary>
            /// any double
            /// </summary>
            public double DoubleNumberReturnValue { get; set; }
            /// <summary>
            /// any string
            /// </summary>
            public List<string> stringListReturnValue { get; set; }
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
        
        public class JobSynchronizer
        {
            public int JobID { get; set; }
            public Boolean EnableFlag { get; set; }
            public string LogFileName { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultJobSynchronizer
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
            public List<GlobalVars.JobSynchronizer> ReturnValue { get; set; }
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
        
        public class BatchSummary
        {
            public int Count { get; set; }
            public int TotalNumOfDocumnets { get; set; }
            public double AvgNumOfDocumnets { get; set; }
            public int TotalNumOfPages { get; set; }
            public double AvgNumOfPages { get; set; }
            public int TotalNumOfScannedPages { get; set; }
            public double AvgNumOfScannedPages { get; set; }
            public int TotalNumOfKeystrokes { get; set; }
            public double AvgNumOfKeystrokes { get; set; }
            public double AvgBatchSize { get; set; }
            public int TotalImageCountGrayscale { get; set; }
            public double AvgImageCountGrayscale { get; set; }
            public int TotalImageCountBlackWhite { get; set; }
            public double AvgImageCountBlackWhite { get; set; }
            public int TotalImageCountGrayscaleBack { get; set; }
            public double AvgImageCountGrayscaleBack { get; set; }
            public int TotalImageCountGrayscaleFront { get; set; }
            public double AvgImageCountGrayscaleFront { get; set; }
            public int TotalImageCountBlackWhiteBack { get; set; }
            public double AvgImageCountBlackWhiteBack { get; set; }
            public int TotalImageCountBlackWhiteFront { get; set; }
            public double AvgImageCountBlackWhiteFront { get; set; }
            public double AvgCaptureTime { get; set; }
            public double AvgScanningStageTime { get; set; }
            public double AvgQCStageTime { get; set; }
            public double AvgScanningTime { get; set; }
            public double AvgQCTime { get; set; }
            public double AvgPrepTime { get; set; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public class ResultBatchSummary
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
            public List<GlobalVars.BatchSummary> ReturnValue { get; set; }
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

        public class BatchTrackingExtended
        {
            public int ID { get; set; }
            public string BatchNumber { get; set; }
            public DateTime Date { get; set; }
            public string InitialStatus { get; set; }
            public string FinalStatus { get; set; }
            public string OperatorName { get; set; }
            public string StationName { get; set; }
            public string Event { get; set; }
            public string JobType { get; set; }
            public double totalQcMinutes { get; set; }
            public int totalDocuments { get; set; }
        }

        public class ResultBatchTrackingExtended
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
            public List<GlobalVars.BatchTrackingExtended> ReturnValue { get; set; }
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

        public class BatchTracking
        {
            public int ID { get; set; }
            public string BatchNumber { get; set; }
            public DateTime Date { get; set; }
            public string InitialStatus { get; set; }
            public string FinalStatus { get; set; }
            public string OperatorName { get; set; }
            public string StationName { get; set; }            
            public string Event { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultBatchTracking
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
            public List<GlobalVars.BatchTracking> ReturnValue { get; set; }
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

        public class Batch
        {
            public string BatchNumber { get; set; }
            public int LotNumber { get; set; }
            public int BlockNumber { get; set; }
            public int NumberOfDocuments { get; set; }
            public string SubmittedBy { get; set; }
            public DateTime SubmittedDate { get; set; }
            public string StatusFlag { get; set; }
            public string ScanOperator { get; set; }
            public DateTime ApprovedDate { get; set; }
            public int RejectedTimes { get; set; }
            public DateTime LastTimeRejected { get; set; }
            public string ApprovedBy { get; set; }
            public int NumberOfPages { get; set; }
            public string RejectedBy { get; set; }
            public string RejectionReason { get; set; }
            public int NumberOfScannedPages { get; set; }
            public string DepName { get; set; }
            public string ProjectName { get; set; }
            public string DocumentPath { get; set; }
            public DateTime ExportedDate { get; set; }
            public string ExportedBy { get; set; }
            public double BatchSize { get; set; }
            public int ExportedTimes { get; set; }
            public string SubDepName { get; set; }
            public string FileStatus { get; set; }
            public DateTime ScannedDate { get; set; }
            public string QCBy { get; set; }
            public string QCStation { get; set; }
            public DateTime QCDate { get; set; }
            public string OutputBy { get; set; }
            public DateTime OutputDate { get; set; }
            public string OutputStation { get; set; }
            public string ScanStation { get; set; }
            public string KodakStatus { get; set; }
            public string JobType { get; set; }
            public string ModifiedStation { get; set; }
            public DateTime ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
            public string KodakErrorState { get; set; }
            public string Comments { get; set; }
            public int ScannedPagesReturned { get; set; }
            public int CaptureTime { get; set; }
            public int ScanningStageTime { get; set; }
            public int QCStageTime { get; set; }
            public int ScanningTime { get; set; }
            public int QCTime { get; set; }
            public DateTime ScanningEndTime { get; set; }
            public DateTime QCEndTime { get; set; }
            public DateTime QCStartTime { get; set; }
            public string TaskOrder { get; set; }
            public string PrepUserName { get; set; }
            public DateTime PrepDate { get; set; }
            public string QARFlag { get; set; }
            public int RecallTimes { get; set; }
            public DateTime RecallDate { get; set; }
            public string RecallBy { get; set; }
            public string RecallReason { get; set; }
            public string Customer { get; set; }
            public int KeysStrokes { get; set; }
            public string BatchAlias { get; set; }
            public DateTime VFRUploadeDate { get; set; } 
            public DateTime VFRUploadModiffiedDate { get; set; }
            public Double PrepTime { get; set; }
             // ----------------------------------------------
            public int InitialNumberOfDocuments { get; set; }
            public int InitialNumberOfPages { get; set; }
            public int InitialNumberOfScannedPages { get; set; }
            public int ImageCountGrayscale { get; set; }
            public int ImageCountBlackWhite { get; set; }
            public int ImageCountGrayscaleBack { get; set; }
            public int ImageCountGrayscaleFront { get; set; }
            public int ImageCountBlackWhiteBack { get; set; }
            public int ImageCountBlackWhiteFront { get; set; }
            public int FrontsCaptured  { get; set; }
            public int FrontsRemoved  { get; set; }
            public int FrontsDeleted  { get; set; }
            public int FrontsRescanned  { get; set; }
            public int BacksCaptured { get; set; }
            public int BacksRemoved { get; set; }
            public int BacksDeleted { get; set; }
            public int BacksRescanned { get; set; }
            //------ VFR Related fields
            public DateTime VFRUploadRequestedDate { get; set; }
            public DateTime VFRUploadCompletedDate { get; set; }
            public string VFRStatusFlag { get; set; }
            public string PageSizesCount { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultBatches
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
            public List<GlobalVars.Batch> ReturnValue { get; set; }
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

        public enum MetadataScope
        {
            [Description("Batch")]
            Batch = 1,
            [Description("Document")]
            Document = 2
        }

        public class Metadata
        {
            public MetadataScope Scope { get; set; }
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
        }

        public class VFRDocs
        {
            public string BatchNumber { get; set; }
            public int DocumentID { get; set; }
            public string StatusFlag { get; set; }
            public DateTime CreationDate { get; set; }
            public int JobID { get; set; }
        }

        public class ResultVFRDocs
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
            public List<GlobalVars.VFRDocs> ReturnValue { get; set; }
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

        public class BatchDocs
        {
            public List<Metadata> DocMetadata {get; set; }
            public string DocumentLocation { get; set; }
            public int DocumentID { get; set; }
            public int ImageCountInDocument { get; set; }
            public int PageCountInDocument { get; set; }
            public string BatchName { get; set; }
            public string BatchLocation { get; set; }
            public double BatchSize { get; set; }
            public DateTime CreateDateAndTime { get; set; }
            public string CreateStationID { get; set; }
            public string CreateStationName { get; set; }
            public string CreateUserID { get; set; }
            public DateTime OutputDateAndTime { get; set; }
            public string OutoutStationID { get; set; }
            public string OutputStationName { get; set; }
            public string OutputUserID { get; set; }
            public DateTime LastModifiedDateAndTime { get; set; }
            public string LastModifiedStationID { get; set; }
            public string LastModifiedStationName { get; set; }
            public string LastModifiedUserID { get; set; }
            public string StartingDocumentID { get; set; }
            public string FirstDocumentID { get; set; }
            public string LastDocumentID { get; set; }
            public int DocumentCountInBatch { get; set; }
            public int PageCountInBatch { get; set; }
            public int ImageCountInBatch { get; set; }
            public int BlackAndWhiteImageCount { get; set; }
            public int ColorImageCount { get; set; }
            public int GrayscaleImageCount { get; set; }
            public int ImageCaptureFront { get; set; }
            public int ImagesRescannedFront { get; set; }
            public int ImagesRemovedForBlankFront { get; set; }
            public int ImagesDeletedFront { get; set; }
            public int ImagesCaptureBack { get; set; }
            public int ImagesRescannedBack { get; set; }
            public int ImagesRemovedForBlankBack { get; set; }
            public int ImagesDeletedBack { get; set; }
            public string DocumentSequenceNumber { get; set; }
            public string DocumentFileNameWithFullPath { get; set; }
            public string DocumentFileName { get; set; }
            public string DocumentSize { get; set; }
            public string CustonmerField1 { get; set; }
            public string CustonmerField2 { get; set; }
            public string CustonmerField3 { get; set; }
            public string CustonmerField4 { get; set; }
            public string CustonmerField5 { get; set; }
            public string CustonmerField6 { get; set; }
            public string CustonmerField7 { get; set; }
            public string CustonmerField8 { get; set; }
            public string CustonmerField9 { get; set; }
            public string CustonmerField10 { get; set; }
            public string CustonmerField11 { get; set; }
            public string CustonmerField12 { get; set; }
            public string CustonmerField13 { get; set; }
            public string CustonmerField14 { get; set; }
            public string CustonmerField15 { get; set; }
            public string Customer { get; set; }
            public int keystrokes { get; set; }
            public string InDqatabase { get; set; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public class ResultBatchDocs
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
            public List<GlobalVars.BatchDocs> ReturnValue { get; set; }
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

        public class GeneralSettingsExtended
        {
            public Boolean DebugFlag { get; set; }
            public string CPApplicationFilePath { get; set; }
            public string ImageViewerFilePath { get; set; }
            public string DBServer { get; set; }
            public string DBUserName { get; set; }
            public string DBPassword { get; set; }
            public string DBProvider { get; set; }
            public string DBName { get; set; }
            public string DBRDBMS { get; set; }
            public string CdiWebUrl { get; set; }
            public int AutoImportWatchFolderID { get; set; }
            public int ScanningFolderID { get; set; }
            public int PostValidationWatchFolderID { get; set; }
            public int LoadBalancerWatchFolderID { get; set; }
            public int BackupFolderID { get; set; }
            public int FileConversionWatchFolderID { get; set; }
            public int BatchDeliveryWatchFolderID { get; set; }
            public int RestingLocationID { get; set; }
            public int VFRRenamerWatchFolderID { get; set; }
            public int VFRDuplicateRemoverWatchFolderID { get; set; }
            public int VFRBatchUploaderWatchFolderID { get; set; }
            public int VFRBatchMonitorFolderID { get; set; }
            public string AutoImportWatchFolder { get; set; }
            public string ScanningFolder { get; set; }
            public string PostValidationWatchFolder { get; set; }
            public string LoadBalancerWatchFolder { get; set; }
            public string BackupFolder { get; set; }
            public string FileConversionWatchFolder { get; set; }
            public string BatchDeliveryWatchFolder { get; set; }
            public string RestingLocation { get; set; }
            public string VFRRenamerWatchFolder { get; set; }
            public string VFRDuplicateRemoverWatchFolder { get; set; }
            public string VFRBatchUploaderWatchFolder { get; set; }
            public string VFRBatchMonitorFolder { get; set; }
            public int QCOutputFolderID { get; set; }
            public string QCOutputFolder { get; set; }
        }

        public class ResultGeneralSettingsExtended
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
            public GeneralSettingsExtended ReturnValue { get; set; }
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
        public class GeneralSettings
        {
            public Boolean DebugFlag { get; set; }
            public string CPApplicationFilePath { get; set; }
            public string ImageViewerFilePath { get; set; }
            public string DBServer { get; set; }
            public string DBUserName { get; set; }
            public string DBPassword { get; set; }
            public string DBProvider { get; set; }
            public string DBName { get; set; }
            public string DBRDBMS { get; set; }
            public string CdiWebUrl { get; set; }
            public int AutoImportWatchFolderID { get; set; }
            public int ScanningFolderID { get; set; }
            public int PostValidationWatchFolderID { get; set; }
            public int LoadBalancerWatchFolderID { get; set; }
            public int BackupFolderID { get; set; }
            public int FileConversionWatchFolderID { get; set; }
            public int BatchDeliveryWatchFolderID { get; set; }
            public int RestingLocationID { get; set; }
            public int VFRRenamerWatchFolderID { get; set; }
            public int VFRDuplicateRemoverWatchFolderID { get; set; }
            public int VFRBatchUploaderWatchFolderID { get; set; }
            public int VFRBatchMonitorFolderID { get; set; }
            public int QCOutputFolderID { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultGeneralSettings
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
            public GeneralSettings ReturnValue { get; set; }
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
        
        /// <summary>
        /// 
        /// </summary>
        public class ScheduleTime
        {
            // Daily
            public Boolean dailyFlag { get; set; }
            public string recurEveryDays { get; set; }

            // By day of the Week
            public Boolean dayOfTheWeekFlag { get; set; }
            public Boolean monday { get; set; }
            public Boolean tuesday { get; set; }
            public Boolean wednesday { get; set; }
            public Boolean thursday { get; set; }
            public Boolean friday { get; set; }
            public Boolean saturday { get; set; }
            public Boolean sunday { get; set; }

            // Repeat Task
            public Boolean repeatTaskFlag { get; set; }
            public Boolean repeatEveryHoursFlag { get; set; }
            public Boolean repeatEveryMinutesFlag { get; set; }
            public string repeatTaskTimes { get; set; }
            public Boolean repeatTaskRange { get; set; }
            public string taskBeginHour { get; set; }
            public string taskEndHour { get; set; }

            // Start at
            public Boolean startTaskAtFlag { get; set; }
            public string startTaskHour { get; set; }
            public string startTaskMinute { get; set; }
        }
        
        // Data structure used to support Batch Export Functionality
        public class ReplaceToken
        {
            public string Name { get; set; }
            public string Pattern { get; set; }
            public string ReplaceBy { get; set; }
        }

        public class ExportRules
        {
            public int JobID { get; set; }
            public string JobName { get; set; }
            public List<string> DirectoryFormat { get; set; }
            public List<string> FileNameFormat { get; set; }
            public List<ReplaceToken> DirectoryReplaceRule { get; set; }
            public List<ReplaceToken> FileNameReplaceRule { get; set; }
            public List<ReplaceToken> FieldsReplaceRule { get; set; }
            public string OutputFileFormat { get; set; }
            public string OutputFileDelimeter { get; set; }
            public List<string> OutputFields { get; set; }
            public string MetadataFileName { get; set; }
            public List<string> MetadataDirectoryFormat { get; set; }
            public Boolean UseQuotationAroundFlag { get; set; }
            public Boolean IncludeHeaderFlag { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultExportRules
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
            public List<GlobalVars.ExportRules> ReturnValue { get; set; }
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
        
        public enum OutputFileFormatype
        {
            [Description("XML")]
            DocumentXML = 1,
            [Description("CSV")]
            CSV = 2
        }

        public class ExportDocs
        {
            public string FileName { get; set; }
            public string FileLocation { get; set; }           
            public string TargetFilename { get; set; }
            public string TargetFileLocation { get; set; }
            public List<Metadata> Fields { get; set; }
        }

        public class ExportBatches
        {
            public string BatchName { get; set; }            
            public List<ExportDocs> Documents { get; set; }

        }

        public class MetadataFiles
        {
            public string OutputFileName { get; set; }
            public string OutputFileLocation { get; set; }
            public string Content { get; set; }

        }

        public class ExportTransactionsJob
        {   
            public string CustomerName { get; set; }
            public string ProjectName { get; set; }
            public string JobType { get; set; }
            public string WorkOrder { get; set; }
            public ExportRules ExportRule { get; set; }
            public List<ExportBatches> Batches { get; set; }
            public List<MetadataFiles> OutputFiles { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultExportTransactionsJob
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
            public ExportTransactionsJob ReturnValue { get; set; }
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
        // --------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public class JobProcess
        {
            public int ProcessID { get; set; }
            public int JobID { get; set; }
            public int StationID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public ScheduleTime Schedule { get; set;  }
            public string ScheduleCronFormat { get; set; }
            public string CronDescription { get; set; }
            public int WatchFolderID { get; set; }
            public int TargetFolderID { get; set; }
            public int BackupFolderID { get; set; }
            public Boolean EnableFlag { get; set; }      
            public string JobName { get; set; }
            public Boolean supersedeFlag { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultJobProcesses
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
            public List<JobProcess> ReturnValue { get; set; }
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
        // --------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public class ProcessType
        {
            public int ProcessID { get; set; }
            public string Name { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultProcessTypes
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
            public List<ProcessType> ReturnValue { get; set; }
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


        // --------------------------------------------------------------


        // --------------------------------------------------------------
        /// <summary>
        /// Use this data structure for New and Update Transactions
        /// </summary>
        public class ProcessUpdate
        {
            public int ProcessID { get; set; }
            public int JobID { get; set; }
            public int StationID { get; set; }
            public int PDFStationID { get; set; }
            public string Description { get; set; }
            public ScheduleTime Schedule { get; set; }
            public Boolean EnableFlag { get; set; }
            //public int WatchFolderID { get; set; }
            //public int TargetFolderID { get; set; }
            //public int BackupFolderID { get; set; }
            //public int SleepTime { get; set; }
        }

        /// <summary>
        /// Use this data Structure for Get Transactions
        /// </summary>
        public class Process
        {
            public int ProcessID { get; set; }
            public int JobID { get; set; }
            public int StationID { get; set; }
            public int PDFStationID { get; set; }
            public string ProcessName { get; set; }           
            public string JobName { get; set; }
            public string StationName { get; set; }
            public string PDFStationName { get; set; }
            public string Description { get; set; }
            public ScheduleTime Schedule { get; set; }
            public string ScheduleCronFormat { get; set; }
            public string CronDescription { get; set; }
            public Boolean EnableFlag { get; set; }
            //public int WatchFolderID { get; set; }
            //public int TargetFolderID { get; set; }
            //public int BackupFolderID { get; set; }
            //public string WatchFolder { get; set; }
            //public string TargetFolder { get; set; }
            //public string BackupFolder { get; set; }
            //public int SleepTime { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultProcesses
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
            public List<Process> ReturnValue { get; set; }
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

        // --------------------------------------------------------------
        
        public class ServiceStationExtended
        {
            public int StationID { get; set; }
            public string StationName { get; set; }
            public Boolean PDFStationFlag { get; set; }
            public int WatchFolderID { get; set; }
            public int TargetFolderID { get; set; }
            public int BackupFolderID { get; set; }
            public int MaxNumberBatches { get; set; }
            public Boolean WeekendFlag { get; set; }
            public Boolean WorkdayFlag { get; set; }
            public string WeekendStartTime { get; set; }
            public string WeenkendEndTime { get; set; }
            public string WorkdayStartTime { get; set; }
            public string WorkdayEndTime { get; set; }
            public Boolean EnableFlag { get; set; }
            public string WatchFolder { get; set; }
            public string TargetFolder { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultServiceStationsExtended
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
            public List<ServiceStationExtended> ReturnValue { get; set; }
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

        /// <summary>
        /// 
        /// </summary>
        public class ServiceStation
        {
            public int StationID { get; set; }
            public string StationName { get; set; }
            public Boolean PDFStationFlag { get; set; }
            public int WatchFolderID { get; set; }
            public int TargetFolderID { get; set; }
            public int BackupFolderID { get; set; }
            public int MaxNumberBatches { get; set; }
            public Boolean WeekendFlag { get; set; }
            public Boolean WorkdayFlag { get; set; }
            public string WeekendStartTime { get; set; }
            public string WeenkendEndTime { get; set; }
            public string WorkdayStartTime { get; set; }
            public string WorkdayEndTime { get; set; }
            public Boolean EnableFlag { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultServiceStations
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
            public List<ServiceStation> ReturnValue { get; set; }
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

        // --------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public class WorkingFolder
        {
            public int FolderID { get; set; }
            public string Path { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ResultWorkingFolders
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
            public List<WorkingFolder> ReturnValue { get; set; }
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

        // ------------------------------------------------------------------
        public void xyz()
        {
            ResultCustomers resultTest = new ResultCustomers();
            
            resultTest.Exception = null;
                            resultTest.ElapsedTime = "0";
                            resultTest.HttpStatusCode = "200";
                            resultTest.HttpStatusCode = "GetCustomers transaction completed successfully. Number of records found: 1";
                            resultTest.RecordsCount = 1;
                            resultTest.ReturnCode = 0;
                            Customer cust = new Customer();
            cust.CustomerID = 3;
                            cust.CustomerName = "COH-SWM";
                            List<Customer> customers = new List<Customer>();
            customers.Add(cust);
                            resultTest.ReturnValue = customers;
        }

    }
}
