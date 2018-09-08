using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using System.Linq.Dynamic.Core;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLFunctionsJobs
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// This method is used to get the the Path from a Working Folder List based on a given Folder ID
        /// </summary>
        /// <returns></returns>
        static public string GetFolderPath(int folderID, GlobalVars.ResultWorkingFolders workingFolder)
        {
            string path = "";
            try
            {
                foreach (GlobalVars.WorkingFolder folder in workingFolder.ReturnValue)
                {
                    if (folder.FolderID == folderID)
                    {
                        path = folder.Path.Trim();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
            }           
            return path;
        }

        /// <summary>
        /// Get List of Jobs
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultJobsExtended GetJobs()
        {
            string AutoImportWatchFolder = "";
            string ScanningFolder = "";
            string PostValidationWatchFolder = "";            
            string LoadBalancerWatchFolder = "";
            string BackupFolder = "";
            string FileConversionWatchFolder = "";
            string BatchDeliveryWatchFolder = "";
            string RestingLocation = "";
            string VFRRenamerWatchFolder = "";
            string VFRDuplicateRemoverWatchFolder = "";
            string VFRBatchUploaderWatchFolder = "";
            string VFRBatchMonitorFolder = "";
            string QCOutputFolder = "";
            List<GlobalVars.JobExtended> jobs = new List<GlobalVars.JobExtended>();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = jobs,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetJobs Method ...");
                // Get working folders. We will use the result to get the names of the working foldrs by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = from j in DB.Jobs
                                  join p in DB.Projects on j.ProjectId equals p.ProjectId
                                  join c in DB.Customers on p.CustomerId equals c.CustomerId
                                  select new { j, p.ProjectName, c.CustomerName };

                    //var results = DB.Jobs;
                    resultJobs.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {                        
                        foreach (var x in results)
                        {
                            // Get the Paths based on Folder IDs
                            AutoImportWatchFolder = GetFolderPath(Convert.ToInt32(x.j.AutoImportWatchFolderId), workingFolder);
                            ScanningFolder = GetFolderPath(x.j.ScanningFolderId, workingFolder);
                            PostValidationWatchFolder = GetFolderPath(Convert.ToInt32(x.j.PostValidationWatchFolderId), workingFolder);
                            LoadBalancerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.LoadBalancerWatchFolderId), workingFolder);
                            BackupFolder = GetFolderPath(Convert.ToInt32(x.j.BackupFolderId), workingFolder);
                            FileConversionWatchFolder = GetFolderPath(Convert.ToInt32(x.j.FileConversionWatchFolderId), workingFolder);
                            BatchDeliveryWatchFolder = GetFolderPath(Convert.ToInt32(x.j.BatchDeliveryWatchFolderId), workingFolder);
                            RestingLocation = GetFolderPath(x.j.RestingLocationId, workingFolder);
                            VFRRenamerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrrenamerWatchFolderId), workingFolder);
                            VFRDuplicateRemoverWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId), workingFolder);
                            VFRBatchUploaderWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchUploaderWatchFolderId), workingFolder);
                            VFRBatchMonitorFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchMonitorFolderId), workingFolder);
                            QCOutputFolder = GetFolderPath(Convert.ToInt32(x.j.QcoutputFolderId), workingFolder);

                            GlobalVars.JobExtended job = new GlobalVars.JobExtended()
                            {
                                JobID = x.j.JobId,
                                ProjectID = x.j.ProjectId,
                                JobName = (x.j.JobName ?? "").Trim(),
                                ExportClassName = (x.j.ExportClassName ?? "").Trim(),
                                DepartmentName = (x.j.DepartmentName ?? "").Trim(),
                                CustomerName = (x.CustomerName ?? "").Trim(),
                                ProjectName = (x.ProjectName ?? "").Trim(),
                                AutoImportWatchFolderID = Convert.ToInt32(x.j.AutoImportWatchFolderId),
                                ScanningFolderID = x.j.ScanningFolderId,
                                PostValidationWatchFolderID = Convert.ToInt32(x.j.ScanningFolderId),                                
                                LoadBalancerWatchFolderID = Convert.ToInt32(x.j.LoadBalancerWatchFolderId),
                                BackupFolderID = Convert.ToInt32(x.j.BackupFolderId),
                                FileConversionWatchFolderID = Convert.ToInt32(x.j.FileConversionWatchFolderId),
                                BatchDeliveryWatchFolderID = Convert.ToInt32(x.j.BatchDeliveryWatchFolderId),
                                RestingLocationID = x.j.RestingLocationId,
                                VFRRenamerWatchFolderID = Convert.ToInt32(x.j.VfrrenamerWatchFolderId),
                                VFRDuplicateRemoverWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchUploaderWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchMonitorFolderID = Convert.ToInt32(x.j.VfrbatchMonitorFolderId),
                                AutoImportEnableFlag = Convert.ToBoolean(x.j.AutoImportEnableFlag),
                                PostValidationEnableFlag = Convert.ToBoolean(x.j.PostValidationEnableFlag),
                                //LoadBalancerEnableFlag = Convert.ToBoolean(x.j.LoadBalancerEnableFlag),
                                //BatchDeliveryEnableFlag = Convert.ToBoolean(x.j.BatchDeliveryEnableFlag),
                                FileConversionEnableFlag = Convert.ToBoolean(x.j.FileConversionFlag),
                                VFREnableFlag = Convert.ToBoolean(x.j.VfrenableFlag),
                                OutputFileType = (x.j.OutputFileType ?? "").Trim(),
                                MultiPageFlag = Convert.ToBoolean(x.j.MultiPageFlag),
                                // Additional information
                                AutoImportWatchFolder = AutoImportWatchFolder,
                                ScanningFolder = ScanningFolder,
                                PostValidationWatchFolder = PostValidationWatchFolder,                               
                                LoadBalancerWatchFolder = LoadBalancerWatchFolder,
                                BackupFolder = BackupFolder,
                                FileConversionWatchFolder = FileConversionWatchFolder,
                                BatchDeliveryWatchFolder = BatchDeliveryWatchFolder,
                                RestingLocation = RestingLocation,
                                VFRRenamerWatchFolder = VFRRenamerWatchFolder,
                                VFRDuplicateRemoverWatchFolder = VFRDuplicateRemoverWatchFolder,
                                VFRBatchUploaderWatchFolder = VFRBatchUploaderWatchFolder,
                                VFRBatchMonitorFolder = VFRBatchMonitorFolder,
                                QCOuputFolderID = Convert.ToInt32(x.j.QcoutputFolderId),
                                QCOuputFolder = QCOutputFolder,
                                MaxBatchesPerWorkOrder = Convert.ToInt32(x.j.MaxBatchesPerWorkOrder),
                                BatchCleanupFlag = Convert.ToBoolean(x.j.BatchCleanupFlag)
                            };
                            jobs.Add(job);
                        }
                    }
                }
                resultJobs.ReturnValue = jobs;
                resultJobs.Message = "Get Jobs transaction completed successfully. Number of records found: " + resultJobs.RecordsCount;
                logger.Debug(resultJobs.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetJobs Method ...");
            return resultJobs;
        }

        /// <summary>
        /// Get Job by Name
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultJobsExtended GetJobByName(string jobName)
        {
            string AutoImportWatchFolder = "";
            string ScanningFolder = "";
            string PostValidationWatchFolder = "";
            string LoadBalancerWatchFolder = "";
            string BackupFolder = "";
            string FileConversionWatchFolder = "";
            string BatchDeliveryWatchFolder = "";
            string RestingLocation = "";
            string VFRRenamerWatchFolder = "";
            string VFRDuplicateRemoverWatchFolder = "";
            string VFRBatchUploaderWatchFolder = "";
            string VFRBatchMonitorFolder = "";
            string QCOutputFolder = "";
            List<GlobalVars.JobExtended> jobs = new List<GlobalVars.JobExtended>();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = jobs,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetJobByName Method ...");
                // Get working folders. We will use the result to get the names of the working foldrs by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = from j in DB.Jobs
                                  join p in DB.Projects on j.ProjectId equals p.ProjectId
                                  join c in DB.Customers on p.CustomerId equals c.CustomerId
                                  where j.JobName == jobName
                                  select new { j, p.ProjectName, c.CustomerName };

                    //var results = DB.Jobs.Where(x => x.JobName == jobName);
                    resultJobs.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            // Get the Paths based on Folder IDs
                            AutoImportWatchFolder = GetFolderPath(Convert.ToInt32(x.j.AutoImportWatchFolderId), workingFolder);
                            ScanningFolder = GetFolderPath(x.j.ScanningFolderId, workingFolder);
                            PostValidationWatchFolder = GetFolderPath(Convert.ToInt32(x.j.PostValidationWatchFolderId), workingFolder);
                            LoadBalancerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.LoadBalancerWatchFolderId), workingFolder);
                            BackupFolder = GetFolderPath(Convert.ToInt32(x.j.BackupFolderId), workingFolder);
                            FileConversionWatchFolder = GetFolderPath(Convert.ToInt32(x.j.FileConversionWatchFolderId), workingFolder);
                            BatchDeliveryWatchFolder = GetFolderPath(Convert.ToInt32(x.j.BatchDeliveryWatchFolderId), workingFolder);
                            RestingLocation = GetFolderPath(x.j.RestingLocationId, workingFolder);
                            VFRRenamerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrrenamerWatchFolderId), workingFolder);
                            VFRDuplicateRemoverWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId), workingFolder);
                            VFRBatchUploaderWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchUploaderWatchFolderId), workingFolder);
                            VFRBatchMonitorFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchMonitorFolderId), workingFolder);
                            QCOutputFolder = GetFolderPath(Convert.ToInt32(x.j.QcoutputFolderId), workingFolder);

                            GlobalVars.JobExtended job = new GlobalVars.JobExtended()
                            {
                                JobID = x.j.JobId,
                                ProjectID = x.j.ProjectId,
                                JobName = (x.j.JobName ?? "").Trim(),
                                ExportClassName = (x.j.ExportClassName ?? "").Trim(),
                                DepartmentName = (x.j.DepartmentName ?? "").Trim(),
                                CustomerName = (x.CustomerName ?? "").Trim(),
                                ProjectName = (x.ProjectName ?? "").Trim(),
                                AutoImportWatchFolderID = Convert.ToInt32(x.j.AutoImportWatchFolderId),
                                ScanningFolderID = x.j.ScanningFolderId,
                                PostValidationWatchFolderID = Convert.ToInt32(x.j.ScanningFolderId),
                                LoadBalancerWatchFolderID = Convert.ToInt32(x.j.LoadBalancerWatchFolderId),
                                BackupFolderID = Convert.ToInt32(x.j.BackupFolderId),
                                FileConversionWatchFolderID = Convert.ToInt32(x.j.FileConversionWatchFolderId),
                                BatchDeliveryWatchFolderID = Convert.ToInt32(x.j.BatchDeliveryWatchFolderId),
                                RestingLocationID = x.j.RestingLocationId,
                                VFRRenamerWatchFolderID = Convert.ToInt32(x.j.VfrrenamerWatchFolderId),
                                VFRDuplicateRemoverWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchUploaderWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchMonitorFolderID = Convert.ToInt32(x.j.VfrbatchMonitorFolderId),
                                AutoImportEnableFlag = Convert.ToBoolean(x.j.AutoImportEnableFlag),
                                PostValidationEnableFlag = Convert.ToBoolean(x.j.PostValidationEnableFlag),
                                //LoadBalancerEnableFlag = Convert.ToBoolean(x.j.LoadBalancerEnableFlag),
                                //BatchDeliveryEnableFlag = Convert.ToBoolean(x.j.BatchDeliveryEnableFlag),
                                FileConversionEnableFlag = Convert.ToBoolean(x.j.FileConversionFlag),
                                VFREnableFlag = Convert.ToBoolean(x.j.VfrenableFlag),
                                OutputFileType = (x.j.OutputFileType ?? "").Trim(),
                                MultiPageFlag = Convert.ToBoolean(x.j.MultiPageFlag),
                                // Additional information
                                AutoImportWatchFolder = AutoImportWatchFolder,
                                ScanningFolder = ScanningFolder,
                                PostValidationWatchFolder = PostValidationWatchFolder,
                                LoadBalancerWatchFolder = LoadBalancerWatchFolder,
                                BackupFolder = BackupFolder,
                                FileConversionWatchFolder = FileConversionWatchFolder,
                                BatchDeliveryWatchFolder = BatchDeliveryWatchFolder,
                                RestingLocation = RestingLocation,
                                VFRRenamerWatchFolder = VFRRenamerWatchFolder,
                                VFRDuplicateRemoverWatchFolder = VFRDuplicateRemoverWatchFolder,
                                VFRBatchUploaderWatchFolder = VFRBatchUploaderWatchFolder,
                                VFRBatchMonitorFolder = VFRBatchMonitorFolder,
                                QCOuputFolderID = Convert.ToInt32(x.j.QcoutputFolderId),
                                QCOuputFolder = QCOutputFolder,
                                MaxBatchesPerWorkOrder = Convert.ToInt32(x.j.MaxBatchesPerWorkOrder),
                                BatchCleanupFlag = Convert.ToBoolean(x.j.BatchCleanupFlag)
                            };
                            jobs.Add(job);
                        }
                    }
                }
                resultJobs.ReturnValue = jobs;
                resultJobs.Message = "GetJobByName transaction completed successfully. Number of records found: " + resultJobs.RecordsCount;
                logger.Debug(resultJobs.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetJobByName Method ...");
            return resultJobs;
        }

        /// <summary>
        /// Get Job by ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultJobsExtended GetJobByID(int jobID)
        {
            string AutoImportWatchFolder = "";
            string ScanningFolder = "";
            string PostValidationWatchFolder = "";
            string LoadBalancerWatchFolder = "";
            string BackupFolder = "";
            string FileConversionWatchFolder = "";
            string BatchDeliveryWatchFolder = "";
            string RestingLocation = "";
            string VFRRenamerWatchFolder = "";
            string VFRDuplicateRemoverWatchFolder = "";
            string VFRBatchUploaderWatchFolder = "";
            string VFRBatchMonitorFolder = "";
            string QCOutputFolder = "";
            List<GlobalVars.JobExtended> jobs = new List<GlobalVars.JobExtended>();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = jobs,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetJobByID Method ...");
                // Get working folders. We will use the result to get the names of the working foldrs by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    //Check this out
                    var results = from j in DB.Jobs
                                  join p in DB.Projects on j.ProjectId equals p.ProjectId
                                  join c in DB.Customers on p.CustomerId equals c.CustomerId
                                  where j.JobId == jobID
                                  select new { j, p.ProjectName, c.CustomerName };

                    //var results = DB.Jobs.Where(x => x.JobId == jobID);
                    resultJobs.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            // Get the Paths based on Folder IDs
                            AutoImportWatchFolder = GetFolderPath(Convert.ToInt32(x.j.AutoImportWatchFolderId), workingFolder);
                            ScanningFolder = GetFolderPath(x.j.ScanningFolderId, workingFolder);
                            PostValidationWatchFolder = GetFolderPath(Convert.ToInt32(x.j.PostValidationWatchFolderId), workingFolder);
                            LoadBalancerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.LoadBalancerWatchFolderId), workingFolder);
                            BackupFolder = GetFolderPath(Convert.ToInt32(x.j.BackupFolderId), workingFolder);
                            FileConversionWatchFolder = GetFolderPath(Convert.ToInt32(x.j.FileConversionWatchFolderId), workingFolder);
                            BatchDeliveryWatchFolder = GetFolderPath(Convert.ToInt32(x.j.BatchDeliveryWatchFolderId), workingFolder);
                            RestingLocation = GetFolderPath(x.j.RestingLocationId, workingFolder);
                            VFRRenamerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrrenamerWatchFolderId), workingFolder);
                            VFRDuplicateRemoverWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId), workingFolder);
                            VFRBatchUploaderWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchUploaderWatchFolderId), workingFolder);
                            VFRBatchMonitorFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchMonitorFolderId), workingFolder);
                            QCOutputFolder = GetFolderPath(Convert.ToInt32(x.j.QcoutputFolderId), workingFolder);

                            GlobalVars.JobExtended job = new GlobalVars.JobExtended()
                            {
                                JobID = x.j.JobId,
                                ProjectID = x.j.ProjectId,
                                JobName = (x.j.JobName ?? "").Trim(),
                                ExportClassName = (x.j.ExportClassName ?? "").Trim(),
                                DepartmentName = (x.j.DepartmentName ?? "").Trim(),
                                CustomerName = (x.CustomerName ?? "").Trim(),
                                ProjectName = (x.ProjectName ?? "").Trim(),
                                AutoImportWatchFolderID = Convert.ToInt32(x.j.AutoImportWatchFolderId),
                                ScanningFolderID = x.j.ScanningFolderId,
                                PostValidationWatchFolderID = Convert.ToInt32(x.j.ScanningFolderId),
                                LoadBalancerWatchFolderID = Convert.ToInt32(x.j.LoadBalancerWatchFolderId),
                                BackupFolderID = Convert.ToInt32(x.j.BackupFolderId),
                                FileConversionWatchFolderID = Convert.ToInt32(x.j.FileConversionWatchFolderId),
                                BatchDeliveryWatchFolderID = Convert.ToInt32(x.j.BatchDeliveryWatchFolderId),
                                RestingLocationID = x.j.RestingLocationId,
                                VFRRenamerWatchFolderID = Convert.ToInt32(x.j.VfrrenamerWatchFolderId),
                                VFRDuplicateRemoverWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchUploaderWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchMonitorFolderID = Convert.ToInt32(x.j.VfrbatchMonitorFolderId),
                                AutoImportEnableFlag = Convert.ToBoolean(x.j.AutoImportEnableFlag),
                                PostValidationEnableFlag = Convert.ToBoolean(x.j.PostValidationEnableFlag),
                                //LoadBalancerEnableFlag = Convert.ToBoolean(x.j.LoadBalancerEnableFlag),
                                //BatchDeliveryEnableFlag = Convert.ToBoolean(x.j.BatchDeliveryEnableFlag),
                                FileConversionEnableFlag = Convert.ToBoolean(x.j.FileConversionFlag),
                                VFREnableFlag = Convert.ToBoolean(x.j.VfrenableFlag),
                                OutputFileType = (x.j.OutputFileType ?? "").Trim(),
                                MultiPageFlag = Convert.ToBoolean(x.j.MultiPageFlag),
                                // Additional information
                                AutoImportWatchFolder = AutoImportWatchFolder,
                                ScanningFolder = ScanningFolder,
                                PostValidationWatchFolder = PostValidationWatchFolder,
                                LoadBalancerWatchFolder = LoadBalancerWatchFolder,
                                BackupFolder = BackupFolder,
                                FileConversionWatchFolder = FileConversionWatchFolder,
                                BatchDeliveryWatchFolder = BatchDeliveryWatchFolder,
                                RestingLocation = RestingLocation,
                                VFRRenamerWatchFolder = VFRRenamerWatchFolder,
                                VFRDuplicateRemoverWatchFolder = VFRDuplicateRemoverWatchFolder,
                                VFRBatchUploaderWatchFolder = VFRBatchUploaderWatchFolder,
                                VFRBatchMonitorFolder = VFRBatchMonitorFolder,
                                QCOuputFolderID = Convert.ToInt32(x.j.QcoutputFolderId),
                                QCOuputFolder = QCOutputFolder,
                                MaxBatchesPerWorkOrder = Convert.ToInt32(x.j.MaxBatchesPerWorkOrder),
                                BatchCleanupFlag = Convert.ToBoolean(x.j.BatchCleanupFlag)
                            };
                            jobs.Add(job);
                        }
                    }
                }
                resultJobs.ReturnValue = jobs;
                resultJobs.Message = "GetJobByID transaction completed successfully. Number of records found: " + resultJobs.RecordsCount;
                logger.Debug(resultJobs.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetJobByID Method ...");
            return resultJobs;
        }

        /// <summary>
        /// Get Job by a given Project ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultJobsExtended GetJobByProjectID(int projectID)
        {
            string AutoImportWatchFolder = "";
            string ScanningFolder = "";
            string PostValidationWatchFolder = "";
            string LoadBalancerWatchFolder = "";
            string BackupFolder = "";
            string FileConversionWatchFolder = "";
            string BatchDeliveryWatchFolder = "";
            string RestingLocation = "";
            string VFRRenamerWatchFolder = "";
            string VFRDuplicateRemoverWatchFolder = "";
            string VFRBatchUploaderWatchFolder = "";
            string VFRBatchMonitorFolder = "";
            string QCOutputFolder = "";
            List<GlobalVars.JobExtended> jobs = new List<GlobalVars.JobExtended>();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = jobs,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetJobByProjectID Method ...");
                // Get working folders. We will use the result to get the names of the working foldrs by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = from j in DB.Jobs
                                  join p in DB.Projects on j.ProjectId equals p.ProjectId
                                  join c in DB.Customers on p.CustomerId equals c.CustomerId
                                  where j.ProjectId == projectID
                                  select new { j, p.ProjectName, c.CustomerName };

                    //var results = DB.Jobs.Where(x => x.ProjectId == projectID);
                    resultJobs.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            // Get the Paths based on Folder IDs
                            AutoImportWatchFolder = GetFolderPath(Convert.ToInt32(x.j.AutoImportWatchFolderId), workingFolder);
                            ScanningFolder = GetFolderPath(x.j.ScanningFolderId, workingFolder);
                            PostValidationWatchFolder = GetFolderPath(Convert.ToInt32(x.j.PostValidationWatchFolderId), workingFolder);
                            LoadBalancerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.LoadBalancerWatchFolderId), workingFolder);
                            BackupFolder = GetFolderPath(Convert.ToInt32(x.j.BackupFolderId), workingFolder);
                            FileConversionWatchFolder = GetFolderPath(Convert.ToInt32(x.j.FileConversionWatchFolderId), workingFolder);
                            BatchDeliveryWatchFolder = GetFolderPath(Convert.ToInt32(x.j.BatchDeliveryWatchFolderId), workingFolder);
                            RestingLocation = GetFolderPath(x.j.RestingLocationId, workingFolder);
                            VFRRenamerWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrrenamerWatchFolderId), workingFolder);
                            VFRDuplicateRemoverWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId), workingFolder);
                            VFRBatchUploaderWatchFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchUploaderWatchFolderId), workingFolder);
                            VFRBatchMonitorFolder = GetFolderPath(Convert.ToInt32(x.j.VfrbatchMonitorFolderId), workingFolder);
                            QCOutputFolder = GetFolderPath(Convert.ToInt32(x.j.QcoutputFolderId), workingFolder);

                            GlobalVars.JobExtended job = new GlobalVars.JobExtended()
                            {
                                JobID = x.j.JobId,
                                ProjectID = x.j.ProjectId,
                                JobName = (x.j.JobName ?? "").Trim(),
                                ExportClassName = (x.j.ExportClassName ?? "").Trim(),
                                DepartmentName = (x.j.DepartmentName ?? "").Trim(),
                                CustomerName = (x.CustomerName ?? "").Trim(),
                                ProjectName = (x.ProjectName ?? "").Trim(),
                                AutoImportWatchFolderID = Convert.ToInt32(x.j.AutoImportWatchFolderId),
                                ScanningFolderID = x.j.ScanningFolderId,
                                PostValidationWatchFolderID = Convert.ToInt32(x.j.ScanningFolderId),
                                LoadBalancerWatchFolderID = Convert.ToInt32(x.j.LoadBalancerWatchFolderId),
                                BackupFolderID = Convert.ToInt32(x.j.BackupFolderId),
                                FileConversionWatchFolderID = Convert.ToInt32(x.j.FileConversionWatchFolderId),
                                BatchDeliveryWatchFolderID = Convert.ToInt32(x.j.BatchDeliveryWatchFolderId),
                                RestingLocationID = x.j.RestingLocationId,
                                VFRRenamerWatchFolderID = Convert.ToInt32(x.j.VfrrenamerWatchFolderId),
                                VFRDuplicateRemoverWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchUploaderWatchFolderID = Convert.ToInt32(x.j.VfrduplicateRemoverWatchFolderId),
                                VFRBatchMonitorFolderID = Convert.ToInt32(x.j.VfrbatchMonitorFolderId),
                                AutoImportEnableFlag = Convert.ToBoolean(x.j.AutoImportEnableFlag),
                                PostValidationEnableFlag = Convert.ToBoolean(x.j.PostValidationEnableFlag),
                                //LoadBalancerEnableFlag = Convert.ToBoolean(x.j.LoadBalancerEnableFlag),
                                //BatchDeliveryEnableFlag = Convert.ToBoolean(x.j.BatchDeliveryEnableFlag),
                                FileConversionEnableFlag = Convert.ToBoolean(x.j.FileConversionFlag),
                                VFREnableFlag = Convert.ToBoolean(x.j.VfrenableFlag),
                                OutputFileType = (x.j.OutputFileType ?? "").Trim(),
                                MultiPageFlag = Convert.ToBoolean(x.j.MultiPageFlag),
                                // Additional information
                                AutoImportWatchFolder = AutoImportWatchFolder,
                                ScanningFolder = ScanningFolder,
                                PostValidationWatchFolder = PostValidationWatchFolder,
                                LoadBalancerWatchFolder = LoadBalancerWatchFolder,
                                BackupFolder = BackupFolder,
                                FileConversionWatchFolder = FileConversionWatchFolder,
                                BatchDeliveryWatchFolder = BatchDeliveryWatchFolder,
                                RestingLocation = RestingLocation,
                                VFRRenamerWatchFolder = VFRRenamerWatchFolder,
                                VFRDuplicateRemoverWatchFolder = VFRDuplicateRemoverWatchFolder,
                                VFRBatchUploaderWatchFolder = VFRBatchUploaderWatchFolder,
                                VFRBatchMonitorFolder = VFRBatchMonitorFolder,
                                QCOuputFolderID = Convert.ToInt32(x.j.QcoutputFolderId),
                                QCOuputFolder = QCOutputFolder,
                                MaxBatchesPerWorkOrder = Convert.ToInt32(x.j.MaxBatchesPerWorkOrder),
                                BatchCleanupFlag = Convert.ToBoolean(x.j.BatchCleanupFlag)
                            };
                            jobs.Add(job);
                        }
                    }
                }
                resultJobs.ReturnValue = jobs;
                resultJobs.Message = "GetJobByProjectID transaction completed successfully. Number of records found: " + resultJobs.RecordsCount;
                logger.Debug(resultJobs.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetJobByProjectID Method ...");
            return resultJobs;
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistJobName(string jobName)
        {
            List<GlobalVars.Job> jobs = new List<GlobalVars.Job>();

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistJobName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Jobs.Where(x => x.JobName == jobName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Job Name" + jobName + " already exist.";
                    }
                    else
                    {
                        result.Message = "Job Name" + jobName + " doest not exist.";
                    }
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving ExistJobName Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistJobID(int jobID)
        {
            List<GlobalVars.Job> jobs = new List<GlobalVars.Job>();

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistJobID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Jobs.Where(x => x.JobId == jobID);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Job ID " + jobID + " already exist.";
                    }
                    else
                    {
                        result.Message = "Job ID " + jobID + " doest not exist.";
                    }
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving ExistJobID Method ...");
            return result;
        }



        private static GlobalVars.ResultGeneric WorkingFolderIDsValidation(GlobalVars.Job job)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };

            result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.ScanningFolderID);
            if (result.RecordsCount == 0)
            {
                result.Message = "Invalid Scanning Folder ID";
                result.ReturnCode = -1;
                return result;
            }

            result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.RestingLocationID);
            if (result.RecordsCount == 0)
            {
                result.Message = "Invalid Batch Resting Location Folder ID";
                result.ReturnCode = -1;
                return result;
            }

            if (job.AutoImportEnableFlag)
            {
                result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.AutoImportWatchFolderID);
                if (result.RecordsCount == 0)
                {
                    result.Message = "Invalid Auto Import Folder ID";
                    result.ReturnCode = -1;
                    return result;
                }
            }

            if (job.PostValidationEnableFlag)
            {
                result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.PostValidationWatchFolderID);
                if (result.RecordsCount == 0)
                {
                    result.Message = "Invalid Post Validation Folder ID";
                    result.ReturnCode = -1;
                    return result;
                }
            }

            if (job.FileConversionEnableFlag)
            {
                result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.LoadBalancerWatchFolderID);
                if (result.RecordsCount == 0)
                {
                    result.Message = "Invalid Load Balancer Folder ID";
                    result.ReturnCode = -1;
                    return result;
                }

                if (job.BackupFolderID > 0)
                {
                    result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.BackupFolderID);
                    if (result.RecordsCount == 0)
                    {
                        result.Message = "Invalid Backup Folder ID";
                        result.ReturnCode = -1;
                        return result;
                    }
                }

            }

                //if (job.BatchDeliveryEnableFlag)
                //{
                //    result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.BatchDeliveryWatchFolderID);
                //    if (result.RecordsCount == 0)
                //    {
                //        result.Message = "Invalid Batch Delivery Folder ID";
                //        result.ReturnCode = -1;
                //        return result;
                //    }
                //}

            if (job.VFREnableFlag)
            {
                result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.VFRRenamerWatchFolderID);
                if (result.RecordsCount == 0)
                {
                    result.Message = "Invalid VFR Renamer Watch Folder ID";
                    result.ReturnCode = -1;
                    return result;
                }
                result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.VFRDuplicateRemoverWatchFolderID);
                if (result.RecordsCount == 0)
                {
                    result.Message = "Invalid VFR Duplicate Remover Watch Folder ID";
                    result.ReturnCode = -1;
                    return result;
                }
                result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.VFRBatchUploaderWatchFolderID);
                if (result.RecordsCount == 0)
                {
                    result.Message = "Invalid VFR Batch Uploader Watch Folder ID";
                    result.ReturnCode = -1;
                    return result;
                }

                if (job.VFRBatchMonitorFolderID > 0)
                {
                    result = SQLFunctionsGeneralSettings.ExistWorkingFolderID(job.VFRBatchMonitorFolderID);
                    if (result.RecordsCount == 0)
                    {
                        result.Message = "Invalid Backup Folder ID";
                        result.ReturnCode = -1;
                        return result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Create a new Job
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewJob(GlobalVars.Job job)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into NewJob Method ...");

                // Chedck if given Project ID does exist
                result = SQLFunctionsProjects.ExistProjectID(job.ProjectID);
                if (result.RecordsCount != 0)
                {
                    // Check if Job Name Exist
                    result = ExistJobName(job.JobName);
                    if (result.RecordsCount == 0)
                    {
                        // Check if required and given folder IDs are correct and exist in the Database
                        result = WorkingFolderIDsValidation(job);
                        if (result.ReturnCode == 0)
                        {
                            // Create new Job
                            using (ScanningDBContext DB = new ScanningDBContext())
                            {
                                Jobs New_Record = new Jobs();
                                New_Record.JobName = job.JobName;
                                New_Record.ProjectId = job.ProjectID;
                                New_Record.ExportClassName = job.ExportClassName;
                                New_Record.DepartmentName = job.DepartmentName;

                                New_Record.AutoImportWatchFolderId = job.AutoImportWatchFolderID;
                                New_Record.ScanningFolderId = job.ScanningFolderID;
                                New_Record.PostValidationWatchFolderId = job.PostValidationWatchFolderID;
                                New_Record.LoadBalancerWatchFolderId = job.LoadBalancerWatchFolderID;
                                New_Record.BackupFolderId = job.BackupFolderID;
                                New_Record.FileConversionWatchFolderId = job.FileConversionWatchFolderID;
                                New_Record.BatchDeliveryWatchFolderId = job.BatchDeliveryWatchFolderID;
                                New_Record.RestingLocationId = job.RestingLocationID;
                                New_Record.QcoutputFolderId = job.QCOuputFolderID;

                                New_Record.VfrrenamerWatchFolderId = job.VFRRenamerWatchFolderID;
                                New_Record.VfrduplicateRemoverWatchFolderId = job.VFRDuplicateRemoverWatchFolderID;
                                New_Record.VfrbatchUploaderWatchFolderId = job.VFRBatchUploaderWatchFolderID;
                                New_Record.VfrbatchMonitorFolderId = job.VFRBatchMonitorFolderID;

                                New_Record.OutputFileType = job.OutputFileType;
                                New_Record.MultiPageFlag = Convert.ToString(job.MultiPageFlag);

                                New_Record.AutoImportEnableFlag = Convert.ToString(job.AutoImportEnableFlag);
                                New_Record.PostValidationEnableFlag = Convert.ToString(job.PostValidationEnableFlag);
                                //New_Record.LoadBalancerEnableFlag = Convert.ToString(job.LoadBalancerEnableFlag);
                                New_Record.FileConversionFlag = Convert.ToString(job.FileConversionEnableFlag);
                                //New_Record.BatchDeliveryEnableFlag = Convert.ToString(job.BatchDeliveryEnableFlag);
                                New_Record.VfrenableFlag = Convert.ToString(job.VFREnableFlag);

                                New_Record.MaxBatchesPerWorkOrder = job.MaxBatchesPerWorkOrder;
                                New_Record.BatchCleanupFlag = Convert.ToString(job.BatchCleanupFlag);

                                DB.Jobs.Add(New_Record);
                                DB.SaveChanges();
                            }
                            result.Message = "NewJob transaction completed successfully. One Record added.";
                        }
                        else
                        {
                            // one or more of the given folder id does not exist
                        }                        
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Job " + job.JobName + " already exist. NewJob transaction will be ignored.";
                    }
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Project ID " + job.ProjectID + " does not exist. NewJob transaction will be ignored.";
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving ExistJob Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateJob(GlobalVars.Job job)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into UpdateJob Method ...");

                // Check if required and given folder IDs are correct and exist in the Database
                result = WorkingFolderIDsValidation(job);
                if (result.ReturnCode == 0)
                {
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        // Job Names must be unique in the Database. The Name could be change but it must be unique
                        Jobs Matching_Result = DB.Jobs.FirstOrDefault(x => x.JobName == job.JobName & x.JobId != job.JobID);
                        if (Matching_Result == null)
                        {

                            Matching_Result = DB.Jobs.FirstOrDefault(x => x.JobId == job.JobID);
                            if (Matching_Result != null)
                            {
                                Matching_Result.JobName = job.JobName;
                                Matching_Result.ExportClassName = job.ExportClassName;
                                Matching_Result.DepartmentName = job.DepartmentName;

                                Matching_Result.AutoImportWatchFolderId = job.AutoImportWatchFolderID;
                                Matching_Result.ScanningFolderId = job.ScanningFolderID;
                                Matching_Result.PostValidationWatchFolderId = job.PostValidationWatchFolderID;
                                Matching_Result.LoadBalancerWatchFolderId = job.LoadBalancerWatchFolderID;
                                Matching_Result.BackupFolderId = job.BackupFolderID;
                                Matching_Result.FileConversionWatchFolderId = job.FileConversionWatchFolderID;
                                Matching_Result.BatchDeliveryWatchFolderId = job.BatchDeliveryWatchFolderID;
                                Matching_Result.RestingLocationId = job.RestingLocationID;
                                Matching_Result.QcoutputFolderId = job.QCOuputFolderID;

                                Matching_Result.VfrrenamerWatchFolderId = job.VFRRenamerWatchFolderID;
                                Matching_Result.VfrduplicateRemoverWatchFolderId = job.VFRDuplicateRemoverWatchFolderID;
                                Matching_Result.VfrbatchUploaderWatchFolderId = job.VFRBatchUploaderWatchFolderID;
                                Matching_Result.VfrbatchMonitorFolderId = job.VFRBatchMonitorFolderID;

                                Matching_Result.OutputFileType = job.OutputFileType;
                                Matching_Result.MultiPageFlag = Convert.ToString(job.MultiPageFlag);

                                Matching_Result.AutoImportEnableFlag = Convert.ToString(job.AutoImportEnableFlag);
                                Matching_Result.PostValidationEnableFlag = Convert.ToString(job.PostValidationEnableFlag);
                                //Matching_Result.LoadBalancerEnableFlag = Convert.ToString(job.LoadBalancerEnableFlag);
                                Matching_Result.FileConversionFlag = Convert.ToString(job.FileConversionEnableFlag);
                                //Matching_Result.BatchDeliveryEnableFlag = Convert.ToString(job.BatchDeliveryEnableFlag);
                                Matching_Result.VfrenableFlag = Convert.ToString(job.VFREnableFlag);

                                Matching_Result.MaxBatchesPerWorkOrder = job.MaxBatchesPerWorkOrder;
                                Matching_Result.BatchCleanupFlag = Convert.ToString(job.BatchCleanupFlag);

                                DB.SaveChanges();
                                result.Message = "UpdateJob transaction completed successfully. One Record Updated.";
                            }
                            else
                            {
                                // Means --> cannot update a Job that does not exist
                                result.ReturnCode = -1;
                                result.Message = "Job " + job.JobName + " does not exist. UpdateJob transaction ignore.";
                            }
                        }
                        else
                        {
                            // Means --> the name already exist for some othe JobID
                            result.ReturnCode = -1;
                            result.Message = "Job " + job.JobName + " already exist. UpdateJob transaction ignore.";
                        }
                    }
                }
                else
                {
                    // one or more of the given folder id does not exist
                }

                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving UpdateJob Method ...");
            return result;
        }

        /// <summary>
        /// Remove Job and associated information from Database
        /// </summary>
        /// <param name="jobID"></param>
        static public GlobalVars.ResultGeneric DeleteJob(int jobID)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into DeleteJob Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    Jobs Matching_Result = DB.Jobs.FirstOrDefault(x => x.JobId == jobID);
                    if (Matching_Result != null)
                    {
                        DB.Jobs.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteJob transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Job ID" + jobID + " does not exist. DeleteJob transaction ignore.";
                    }
                    logger.Debug(result.Message);
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving DeleteJob Method ...");
            return result;
        }

        /// <summary>
        /// Get List of Page Sizes that has bee defined for a given Job for a Job
        /// We will add an especial Page Size to account for Images out of the ranges that has been defined for a particular Job
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultJobPageSizes GetPageSizesByJobID(int jobID)
        {
            List<GlobalVars.JobPageSize> jobPageSizes = new List<GlobalVars.JobPageSize>();
            GlobalVars.ResultJobPageSizes resultJobPageSizes = new GlobalVars.ResultJobPageSizes()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = jobPageSizes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetPageSizesByJobID Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.JobPageSizes.Where(x => x.JobId == jobID);
                    resultJobPageSizes.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                         foreach (var x in results)
                         {
                            GlobalVars.JobPageSize jobPageSize = new GlobalVars.JobPageSize()
                            {
                                ID = x.Id,
                                CategoryName = (x.CategoryName ?? "").Trim(),
                                High = x.High,
                                Width = x.Width,
                                JobID = x.JobId,
                                Area = x.Width * x.High
                            };
                            jobPageSizes.Add(jobPageSize);                            
                        }
                        // Add a Not Define Page Size
                        GlobalVars.JobPageSize jobPageSizeND = new GlobalVars.JobPageSize()
                        {
                            ID = 0,
                            CategoryName = "Unknown",
                            High = 0,
                            Width = 0,
                            JobID = 0,
                            Area = 0
                        };
                        jobPageSizes.Add(jobPageSizeND);
                    }
                }
                resultJobPageSizes.ReturnValue = jobPageSizes;
                resultJobPageSizes.Message = "Get Job Page Sizes transaction completed successfully. Number of records found: " + resultJobPageSizes.RecordsCount;
                logger.Debug(resultJobPageSizes.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultJobPageSizes.ReturnCode = -2;
                resultJobPageSizes.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobPageSizes.Exception = baseException.ToString();
                return resultJobPageSizes;
            }
            logger.Trace("Leaving GetPageSizesByJobID Method ...");
            return resultJobPageSizes;
        }

        /// <summary>
        /// Update Page Size Information for a given Job
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateJobPageSize(GlobalVars.JobPageSize jobPageSize)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into UpdateJobPageSize Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    // Check if Category Name for this Job has been already taken
                    JobPageSizes Matching_Result = DB.JobPageSizes.FirstOrDefault(x => x.JobId == jobPageSize.JobID && x.Id != jobPageSize.ID
                                                                    && x.CategoryName == jobPageSize.CategoryName);
                    if (Matching_Result != null)
                    {
                        result.ReturnCode = -1;
                        result.Message = "There is another record with category Name " + jobPageSize.CategoryName+ " for this Job in the Database. UpdateJobPageSize transaction ignore.";
                    }
                    else
                    {
                        // Check if Records exist in the Database
                        Matching_Result = DB.JobPageSizes.FirstOrDefault(x => x.JobId == jobPageSize.JobID && x.Id == jobPageSize.ID);

                        if (Matching_Result != null)
                        {
                            // Means that the record exist in the Database
                            Matching_Result.CategoryName = jobPageSize.CategoryName;
                            Matching_Result.High = (float)jobPageSize.High;
                            Matching_Result.Width = (float)jobPageSize.Width;

                            DB.SaveChanges();
                            result.Message = "UpdateJobPageSize transaction completed successfully. One Record Updated.";
                        }
                        else
                        {
                            // Means --> cannot update a Job that does not exist
                            result.ReturnCode = -1;
                            result.Message = "Job ID  " + jobPageSize.JobID.ToString() + " and/or Page Size ID : " + jobPageSize.ID.ToString() +
                                            " does not exist. UpdateJobPageSize transaction ignore.";
                        }
                    }
                }            
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving UpdateJobPageSize Method ...");
            return result;
        }

        /// <summary>
        /// Remove Jobs Page Size for a given Page Size ID
        /// </summary>
        /// <param name="pageSizeID"></param>
        static public GlobalVars.ResultGeneric DeleteJobPageSize(int pageSizeID)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into DeleteJobPageSize Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    JobPageSizes Matching_Result = DB.JobPageSizes.FirstOrDefault(x => x.Id == pageSizeID);
                    if (Matching_Result != null)
                    {
                        DB.JobPageSizes.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteJobPageSize transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Page Size ID" + pageSizeID + " does not exist. DeleteJobPageSize transaction ignore.";
                    }
                    logger.Debug(result.Message);
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving DeleteJobPageSize Method ...");
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistPageSize(int jobID, string categoryName)
        {
            List<GlobalVars.Field> fields = new List<GlobalVars.Field>();

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistPageSie Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.JobPageSizes.Where(x => x.JobId == jobID && x.CategoryName == categoryName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Page Size with Job ID: " + jobID.ToString() + " and Category Name: " + categoryName + " already exist.";
                    }
                    else
                    {
                        result.Message = "Page Size with Job ID: " + jobID.ToString() + " and Category Name: " + categoryName + " does not exist.";
                    }
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving ExistPageSie Method ...");
            return result;
        }

        /// <summary>
        /// Create a new Page Sie for a Job
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewJobPageSize(GlobalVars.JobPageSize jobPageSize)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into NewJobPageSize Method ...");

                // Chedck if given Page Sie ID Field does exist
                result = SQLFunctionsJobs.ExistPageSize(jobPageSize.JobID, jobPageSize.CategoryName);
                if (result.RecordsCount == 0)
                {
                    // Create new Field
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        JobPageSizes New_Record = new JobPageSizes();
                        New_Record.JobId = jobPageSize.JobID;
                        New_Record.CategoryName = jobPageSize.CategoryName;
                        New_Record.High = (float)jobPageSize.High;
                        New_Record.Width = (float)jobPageSize.Width;

                        DB.JobPageSizes.Add(New_Record);
                        DB.SaveChanges();
                    }
                    result.Message = "NewJobPageSize transaction completed successfully. One Record added.";
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Page Size with Job ID: " + jobPageSize.JobID.ToString() + " and Category Name: " + jobPageSize.CategoryName + " already exist. NewJobPageSize transaction will be ignored.";
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving NewJobPageSize Method ...");
            return result;
        }
    }
}
