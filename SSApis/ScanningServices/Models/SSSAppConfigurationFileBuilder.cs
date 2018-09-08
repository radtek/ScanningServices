using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using System;


namespace ScanningServices.Models
{
    /// <summary>
    ///     /// 
    /// </summary>
    public class SSSAppConfigurationFileBuilder
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Scanning Services Application Configuration File
        /// </summary>
        /// <returns></returns>
        static public SSSClientGlobalVars.ResultSSSConfig GetSSSConfig()
        {
            ScanningServicesDataObjects.SSSClientGlobalVars.ConfigSTR SSSConfig = new ScanningServicesDataObjects.SSSClientGlobalVars.ConfigSTR();
            ScanningServicesDataObjects.SSSClientGlobalVars.ResultSSSConfig resultSSSConfig = new ScanningServicesDataObjects.SSSClientGlobalVars.ResultSSSConfig()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = SSSConfig,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    //Get General Inforamtion, including Database information
                    GlobalVars.GeneralSettings generalSettings = new GlobalVars.GeneralSettings();
                    var resultGeneralSettings = DB.GeneralSettings.FirstOrDefault();
                    if (resultGeneralSettings != null)
                    {   
                        SSSConfig.Debug = Convert.ToBoolean(resultGeneralSettings.DebugFlag);
                        SSSConfig.CaptureApplication = resultGeneralSettings.CpapplicationFilePath;
                        SSSConfig.ImageViewer = resultGeneralSettings.ImageViewerFilePath;

                        SSSClientGlobalVars.DatabaseSTR database = new SSSClientGlobalVars.DatabaseSTR();
                        database.Server = resultGeneralSettings.Dbserver;
                        database.UserName = resultGeneralSettings.DbuserName;
                        database.Password = resultGeneralSettings.Dbpassword;
                        database.Provider = resultGeneralSettings.Dbprovider;
                        database.RDBMS = resultGeneralSettings.Dbrdbms;
                        database.DatabaseName = resultGeneralSettings.Dbname;

                        // Add Database information to the SSSConfig Data Structure
                        SSSConfig.Database = database;
                    }
                    else
                    {
                        //There is no record for SMTP information in the database
                    }

                    //Get SMTP Information
                    GlobalVars.SMTP smtp = new GlobalVars.SMTP();
                    var resulSMTP = DB.Smtp.FirstOrDefault();
                    if (resulSMTP != null)
                    {
                        SSSClientGlobalVars.EmailSTR email = new SSSClientGlobalVars.EmailSTR();
                        email.Host = resulSMTP.HostName;
                        email.Port = Convert.ToString(resulSMTP.PortNumber);
                        email.SenderAddress = resulSMTP.SenderEmailAddress;
                        email.EnableSSL = Convert.ToBoolean(resulSMTP.EnableSslflag);
                        email.SenderName = resulSMTP.SenderName;
                        email.UserName = resulSMTP.UserName;
                        email.Password = resulSMTP.Password;
                        // Add Email information to the SSSConfig Data Structure
                        SSSConfig.Email = email;
                    }
                    else
                    {
                        //There is no record for SMTP information in the database
                    }
                    
                    //Get Kodak Information
                    List<GlobalVars.JobExtended> jobs = new List<GlobalVars.JobExtended>();
                    int count = 0;
                    var resultJobs = SQLFunctionsJobs.GetJobs(); //DB.Jobs;
                    if (resultJobs != null)
                    {
                        SSSClientGlobalVars.KodakSTR kodak = new SSSClientGlobalVars.KodakSTR();
                        kodak.JobTypes = new List<string>();
                        kodak.StationsID = new List<string>();
                        kodak.JobOutpts = new List<string>();
                        kodak.CaptureProScanDirectories = new List<string>();
                        foreach (var job in resultJobs.ReturnValue)
                        {
                            count ++;                            
                            kodak.JobTypes.Add(job.JobName);
                            kodak.CaptureProScanDirectories.Add(job.ScanningFolder);
                            kodak.JobOutpts.Add(job.PostValidationWatchFolder);
                            kodak.StationsID.Add(Convert.ToString(count)); // Check if we are using Station ID for anything in SSS
                        }
                        // Add Kodak information to the SSSConfig Data Structure
                        SSSConfig.Kodak = kodak;
                    }
                    else
                    {
                        //There is no record for Jobs in the database
                    }

                    //Get Customer information
                   GlobalVars.ResultWorkingFolders resultWorkingFolders = new GlobalVars.ResultWorkingFolders();

                    List<GlobalVars.Customer> customers = new List<GlobalVars.Customer>();
                    var resultCustomers = DB.Customers;
                    if (resultCustomers != null)
                    {
                        SSSConfig.Customers = new List<SSSClientGlobalVars.CustomerSTR>();
                        foreach (var cust in resultCustomers)
                        {
                            SSSClientGlobalVars.CustomerSTR customer = new SSSClientGlobalVars.CustomerSTR();
                            customer.Name = cust.CustomerName;
  
                            // Get Jobs for a giben Customer ID
                            var customerJobs = from j in DB.Jobs
                                         join p in DB.Projects on j.ProjectId equals p.ProjectId
                                         join c in DB.Customers on p.CustomerId equals c.CustomerId
                                         where c.CustomerId == cust.CustomerId
                                         select new { j, c.CustomerId, c.CustomerName, p.ProjectName };
                            if (resultJobs != null)
                            {
                                customer.Projects = new List<SSSClientGlobalVars.CustomerProjectSTR>();
                                foreach (var job in customerJobs)
                                {
                                    resultWorkingFolders = SQLFunctionsGeneralSettings.GetWorkingFolderByID(job.j.RestingLocationId);
                                    if (resultWorkingFolders.RecordsCount > 0)
                                        customer.BatchesLocation = resultWorkingFolders.ReturnValue[0].Path;
                                    else
                                        customer.BatchesLocation = "";

                                    // We will use projects as Jobs
                                    SSSClientGlobalVars.CustomerProjectSTR customerProject = new SSSClientGlobalVars.CustomerProjectSTR();
                                    customerProject.Name = job.ProjectName;
                                    customerProject.ExportClass = job.j.ExportClassName;

                                    // Get Job Fields to deterimine which one are Keystrokes fields
                                    var jobFields = DB.JobsFields.Where(x => x.JobId == job.j.JobId);
                                    if (resultCustomers != null)
                                    {
                                        customerProject.KeyStrokesExcludedFields = new List<string>();
                                        foreach (var field in jobFields)
                                        {
                                            if (field.KeyStrokeExcludeFlag.ToLower() == "true")
                                            {
                                                customerProject.KeyStrokesExcludedFields.Add(field.CpfieldName);
                                            }
                                        }                                        
                                    }
                                    else
                                    {
                                    }

                                    // Get VFR Seetings
                                    customerProject.VFR = new SSSClientGlobalVars.VFRSTR();
                                    var vfrSetting = DB.Vfr.FirstOrDefault(x => x.JobId == job.j.JobId);
                                    if (vfrSetting != null)
                                    { 
                                        customerProject.VFR.CADIWebServiceURL = vfrSetting.Cadiurl;
                                        customerProject.VFR.UserName = vfrSetting.UserName;
                                        customerProject.VFR.Password = vfrSetting.Password;
                                        customerProject.VFR.InstanceName = vfrSetting.InstanceName;
                                        customerProject.VFR.RepositoryName = vfrSetting.RepositoryName;
                                        customerProject.VFR.QueryField = vfrSetting.QueryField;
                                    }
                                    else
                                    {

                                    }
                                     // Add Job information to the SSSConfig Data Structure
                                     customer.Projects.Add(customerProject);
                                }

                            }
                            else
                            {
                            }

                            // Get Customer Reports
                            var reports = from r in DB.Reports
                                          join t in DB.ReportsTemplate on r.TemplateId equals t.TemplateId
                                          join c in DB.Customers on r.CustomerId equals c.CustomerId
                                          where c.CustomerName == customer.Name
                                          select new { r, t.Name };

                            if (reports.Count() >= 1)
                            {
                                customer.Reports = new List<SSSClientGlobalVars.ReportSTR>();
                                foreach (var record in reports)
                                {
                                    SSSClientGlobalVars.ReportSTR report = new SSSClientGlobalVars.ReportSTR();
                                    report.Name = record.Name;

                                    report.Recipients = new List<string>();
                                    if (!string.IsNullOrEmpty(record.r.EmailRecipients))
                                    {
                                        string[] recipients = record.r.EmailRecipients.Split(",");
                                        foreach (string recipient in recipients)
                                        {
                                            report.Recipients.Add(recipient);
                                        }
                                    }

                                    report.Subject = record.r.EmailSubject;
                                    report.Enable = Convert.ToBoolean(record.r.EnableFlag);

                                    SSSClientGlobalVars.ReportTitleSTR title1 = new SSSClientGlobalVars.ReportTitleSTR();
                                    title1.Content = record.r.TitleContent1;
                                    title1.FontBold = record.r.TitleFontBoldFlag1;
                                    title1.FontColor = record.r.TitleFontColor1;
                                    title1.FontSize = Convert.ToString(record.r.TitleFontSize1);
                                    report.Title1 = title1;

                                    SSSClientGlobalVars.ReportTitleSTR title2 = new SSSClientGlobalVars.ReportTitleSTR();
                                    title2.Content = record.r.TitleContent2;
                                    title2.FontBold = record.r.TitleFontBoldFlag2;
                                    title2.FontColor = record.r.TitleFontColor2;
                                    title2.FontSize = Convert.ToString(record.r.TitleFontSize2);
                                    report.Title2 = title2;

                                    SSSClientGlobalVars.ReportTitleSTR title3 = new SSSClientGlobalVars.ReportTitleSTR();
                                    title3.Content = record.r.TitleContent3;
                                    title3.FontBold = record.r.TitleFontBoldFlag3;
                                    title3.FontColor = record.r.TitleFontColor3;
                                    title3.FontSize = Convert.ToString(record.r.TitleFontSize3);
                                    report.Title3 = title3;

                                    SSSClientGlobalVars.ReportTableSTR table = new SSSClientGlobalVars.ReportTableSTR();
                                    table.HeaderBackGroundColor = record.r.TableHeaderBackColor;
                                    table.HeaderFontBold = record.r.TableHeaderFontBoldFlag;
                                    table.HeaderFontColor = record.r.TableHeaderFontColor;
                                    table.HeaderFontSize = Convert.ToString(record.r.TableHeaderFontSize.Value);
                                    //TableColumnNamesBackColor = record.r.TableColumnNamesBackColor,
                                    //TableColumnNamesFontBoldFlag = Convert.ToBoolean(record.r.TableColumnNamesFontBoldFlag),
                                    //TableColumnNamesFontColor = record.r.TableColumnNamesFontColor,
                                    //TableColumnNamesFontSize = record.r.TableColumnNamesFontSize.Value,
                                    report.ReportTable = table;

                                    var parameters = from rp in DB.ReportParameters
                                                     join rtp in DB.ReportsTemplateParameters on rp.ParameterId equals rtp.ParameterId
                                                     where (rp.ReportId == record.r.ReportId && rp.TemplateId == record.r.TemplateId)
                                                     select new { rp, rtp.Name };
                                                                       
                                    if (parameters.Count() >= 1)
                                    {
                                        report.Parameters = new List<SSSClientGlobalVars.ReportParameterSTR>();
                                        foreach (var p in parameters)
                                        {
                                            SSSClientGlobalVars.ReportParameterSTR parameter = new SSSClientGlobalVars.ReportParameterSTR();
                                            parameter.Name = p.Name;
                                            parameter.Value = p.rp.Value;
                                            report.Parameters.Add(parameter);
                                        }
                                    }
                                    customer.Reports.Add(report);
                                }
                            }

                            // Add Customer information to the SSSConfig Data Structure
                            SSSConfig.Customers.Add(customer);
                        }                        
                    }
                    else
                    {
                        //There is no record for Jobs in the database
                    }

                    // Add System Reports to the SSSConfig Data Structure
                    // Get Customer Reports
                    var systemReports = from r in DB.Reports
                                        join t in DB.ReportsTemplate on r.TemplateId equals t.TemplateId
                                        where r.CustomerId == 0
                                        select new { r, t.Name };

                    if (systemReports.Count() >= 1)
                    {
                        SSSConfig.Reports = new List<SSSClientGlobalVars.ReportSTR>();
                        foreach (var record in systemReports)
                        {
                            SSSClientGlobalVars.ReportSTR report = new SSSClientGlobalVars.ReportSTR();
                            report.Name = record.Name;

                            report.Recipients = new List<string>();
                            if (!string.IsNullOrEmpty(record.r.EmailRecipients))
                            {
                                string[] recipients = record.r.EmailRecipients.Split(",");
                                foreach (string recipient in recipients)
                                {
                                    report.Recipients.Add(recipient);
                                }
                            }

                            report.Subject = record.r.EmailSubject;
                            report.Enable = Convert.ToBoolean(record.r.EnableFlag);

                            SSSClientGlobalVars.ReportTitleSTR title1 = new SSSClientGlobalVars.ReportTitleSTR();
                            title1.Content = record.r.TitleContent1;
                            title1.FontBold = record.r.TitleFontBoldFlag1;
                            title1.FontColor = record.r.TitleFontColor1;
                            title1.FontSize = Convert.ToString(record.r.TitleFontSize1);
                            report.Title1 = title1;

                            SSSClientGlobalVars.ReportTitleSTR title2 = new SSSClientGlobalVars.ReportTitleSTR();
                            title2.Content = record.r.TitleContent2;
                            title2.FontBold = record.r.TitleFontBoldFlag2;
                            title2.FontColor = record.r.TitleFontColor2;
                            title2.FontSize = Convert.ToString(record.r.TitleFontSize2);
                            report.Title2 = title2;

                            SSSClientGlobalVars.ReportTitleSTR title3 = new SSSClientGlobalVars.ReportTitleSTR();
                            title3.Content = record.r.TitleContent3;
                            title3.FontBold = record.r.TitleFontBoldFlag3;
                            title3.FontColor = record.r.TitleFontColor3;
                            title3.FontSize = Convert.ToString(record.r.TitleFontSize3);
                            report.Title3 = title3;

                            SSSClientGlobalVars.ReportTableSTR table = new SSSClientGlobalVars.ReportTableSTR();
                            table.HeaderBackGroundColor = record.r.TableHeaderBackColor;
                            table.HeaderFontBold = record.r.TableHeaderFontBoldFlag;
                            table.HeaderFontColor = record.r.TableHeaderFontColor;
                            table.HeaderFontSize = Convert.ToString(record.r.TableHeaderFontSize.Value);
                            //TableColumnNamesBackColor = record.r.TableColumnNamesBackColor,
                            //TableColumnNamesFontBoldFlag = Convert.ToBoolean(record.r.TableColumnNamesFontBoldFlag),
                            //TableColumnNamesFontColor = record.r.TableColumnNamesFontColor,
                            //TableColumnNamesFontSize = record.r.TableColumnNamesFontSize.Value,
                            report.ReportTable = table;

                            var parameters = from rp in DB.ReportParameters
                                             join rtp in DB.ReportsTemplateParameters on rp.ParameterId equals rtp.ParameterId
                                             where (rp.ReportId == record.r.ReportId && rp.TemplateId == record.r.TemplateId)
                                             select new { rp, rtp.Name };

                            if (parameters.Count() >= 1)
                            {
                                report.Parameters = new List<SSSClientGlobalVars.ReportParameterSTR>();
                                foreach (var p in parameters)
                                {
                                    SSSClientGlobalVars.ReportParameterSTR parameter = new SSSClientGlobalVars.ReportParameterSTR();
                                    parameter.Name = p.Name;
                                    parameter.Value = p.rp.Value;
                                    report.Parameters.Add(parameter);
                                }
                            }
                            SSSConfig.Reports.Add(report);
                        }
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultSSSConfig.ReturnCode = -2;
                resultSSSConfig.Message = e.Message;
                var baseException = e.GetBaseException();
                resultSSSConfig.Exception = baseException.ToString();
                return resultSSSConfig;
            }

            logger.Trace("Leaving GetCustomerByID Method ...");
            return resultSSSConfig;
          
       }
    }
}
