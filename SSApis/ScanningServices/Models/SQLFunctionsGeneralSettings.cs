using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using System;
using System.IO;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLFunctionsGeneralSettings
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get General SSS General Settings
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneralSettingsExtended GetGeneralSettings()
        {
            GlobalVars.GeneralSettingsExtended generalSettings = new GlobalVars.GeneralSettingsExtended();
            GlobalVars.ResultGeneralSettingsExtended resultGeneralSettings = new GlobalVars.ResultGeneralSettingsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = generalSettings,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetGeneralSettings Method ...");
                // Get working folders. We will use the result to get the names of the working foldrs by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.GeneralSettings.FirstOrDefault();
                    if (results != null)
                    {
                        resultGeneralSettings.RecordsCount = 1;
                        generalSettings.DebugFlag = Convert.ToBoolean(results.DebugFlag);
                        if (string.IsNullOrEmpty(results.CpapplicationFilePath))
                            generalSettings.CPApplicationFilePath = "";
                        else
                            generalSettings.CPApplicationFilePath = results.CpapplicationFilePath;
                        if (string.IsNullOrEmpty(results.ImageViewerFilePath))
                            generalSettings.ImageViewerFilePath = "";
                        else
                            generalSettings.ImageViewerFilePath = results.ImageViewerFilePath;
                        if (string.IsNullOrEmpty(results.Dbserver))
                            generalSettings.DBServer = "";
                        else
                            generalSettings.DBServer = results.Dbserver;
                        if (string.IsNullOrEmpty(results.DbuserName))
                            generalSettings.DBUserName = "";
                        else
                            generalSettings.DBUserName = results.DbuserName;
                        if (string.IsNullOrEmpty(results.Dbpassword))
                            generalSettings.DBPassword = "";
                        else
                            generalSettings.DBPassword = results.Dbpassword;
                        if (string.IsNullOrEmpty(results.Dbprovider))
                            generalSettings.DBProvider = "";
                        else
                            generalSettings.DBProvider = results.Dbprovider;
                        if (string.IsNullOrEmpty(results.Dbname))
                            generalSettings.DBName = "";
                        else
                            generalSettings.DBName = results.Dbname;
                        if (string.IsNullOrEmpty(results.Dbrdbms))
                            generalSettings.DBRDBMS = "";
                        else
                            generalSettings.DBRDBMS = results.Dbrdbms;

                        if (string.IsNullOrEmpty(results.CdiWebUrl))
                            generalSettings.CdiWebUrl = "";
                        else
                            generalSettings.CdiWebUrl = results.CdiWebUrl;
                        
                        generalSettings.AutoImportWatchFolderID = Convert.ToInt32(results.AutoImportWatchFolderId);
                        generalSettings.AutoImportWatchFolder= GetFolderPath(generalSettings.AutoImportWatchFolderID, workingFolder);
                        generalSettings.ScanningFolderID = Convert.ToInt32(results.ScanningFolderId);
                        generalSettings.ScanningFolder = GetFolderPath(generalSettings.ScanningFolderID, workingFolder);
                        generalSettings.PostValidationWatchFolderID = Convert.ToInt32(results.PostValidationWatchFolderId);
                        generalSettings.PostValidationWatchFolder = GetFolderPath(generalSettings.PostValidationWatchFolderID, workingFolder);
                        generalSettings.LoadBalancerWatchFolderID = Convert.ToInt32(results.LoadBalancerWatchFolderId);
                        generalSettings.LoadBalancerWatchFolder = GetFolderPath(generalSettings.LoadBalancerWatchFolderID, workingFolder);
                        generalSettings.BackupFolderID = Convert.ToInt32(results.BackupFolderId);
                        generalSettings.BackupFolder = GetFolderPath(generalSettings.BackupFolderID, workingFolder);
                        generalSettings.FileConversionWatchFolderID = Convert.ToInt32(results.FileConversionWatchFolderId);
                        generalSettings.FileConversionWatchFolder = GetFolderPath(generalSettings.FileConversionWatchFolderID, workingFolder);
                        generalSettings.BatchDeliveryWatchFolderID = Convert.ToInt32(results.BatchDeliveryWatchFolderId);
                        generalSettings.BatchDeliveryWatchFolder = GetFolderPath(generalSettings.BatchDeliveryWatchFolderID, workingFolder);
                        generalSettings.RestingLocationID = Convert.ToInt32(results.RestingLocationId);
                        generalSettings.RestingLocation = GetFolderPath(generalSettings.RestingLocationID, workingFolder);
                        generalSettings.VFRRenamerWatchFolderID = Convert.ToInt32(results.VfrrenamerWatchFolderId);
                        generalSettings.VFRRenamerWatchFolder = GetFolderPath(generalSettings.VFRRenamerWatchFolderID, workingFolder);
                        generalSettings.VFRDuplicateRemoverWatchFolderID = Convert.ToInt32(results.VfrduplicateRemoverWatchFolderId);
                        generalSettings.VFRDuplicateRemoverWatchFolder = GetFolderPath(generalSettings.VFRDuplicateRemoverWatchFolderID, workingFolder);
                        generalSettings.VFRBatchUploaderWatchFolderID = Convert.ToInt32(results.VfrbatchUploaderWatchFolderId);
                        generalSettings.VFRBatchUploaderWatchFolder = GetFolderPath(generalSettings.VFRBatchUploaderWatchFolderID, workingFolder);
                        generalSettings.VFRBatchMonitorFolderID = Convert.ToInt32(results.VfrbatchMonitorFolderId);
                        generalSettings.VFRBatchMonitorFolder = GetFolderPath(generalSettings.VFRBatchMonitorFolderID, workingFolder);
                        generalSettings.QCOutputFolderID = Convert.ToInt32(results.QcoutputFolderId);
                        generalSettings.QCOutputFolder = GetFolderPath(generalSettings.QCOutputFolderID, workingFolder);
                    }
                    else
                    {
                        //There is no record in the database
                    }
                }
                resultGeneralSettings.ReturnValue = generalSettings;
                resultGeneralSettings.Message = "GetGeneralSettings transaction completed successfully. Number of records found: " + resultGeneralSettings.RecordsCount;
                logger.Debug(resultGeneralSettings.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultGeneralSettings.ReturnCode = -2;
                resultGeneralSettings.Message = e.Message;
                var baseException = e.GetBaseException();
                resultGeneralSettings.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetGeneralSettings Method ...");
            return resultGeneralSettings;
        }


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
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateGeneralSettings(GlobalVars.GeneralSettings generalSettings)
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
                logger.Trace("Entering into UpdateGeneralSettings Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                   
                    GeneralSettings Matching_Result = DB.GeneralSettings.FirstOrDefault();
                    GeneralSettings record = new GeneralSettings();
                    record.DebugFlag = Convert.ToString(generalSettings.DebugFlag);
                    record.CpapplicationFilePath = generalSettings.CPApplicationFilePath;
                    record.ImageViewerFilePath = generalSettings.ImageViewerFilePath;
                    record.Dbserver = generalSettings.DBServer;
                    record.DbuserName = generalSettings.DBUserName;
                    record.Dbpassword = generalSettings.DBPassword;
                    record.Dbprovider = generalSettings.DBProvider;
                    record.Dbname = generalSettings.DBName;
                    record.Dbrdbms = generalSettings.DBRDBMS;

                    record.AutoImportWatchFolderId = generalSettings.AutoImportWatchFolderID;
                    record.ScanningFolderId = generalSettings.ScanningFolderID;
                    record.PostValidationWatchFolderId = generalSettings.PostValidationWatchFolderID;
                    record.LoadBalancerWatchFolderId = generalSettings.LoadBalancerWatchFolderID;
                    record.BackupFolderId = generalSettings.BackupFolderID;
                    record.BatchDeliveryWatchFolderId = generalSettings.BatchDeliveryWatchFolderID;
                    record.FileConversionWatchFolderId = generalSettings.FileConversionWatchFolderID;
                    record.RestingLocationId = generalSettings.RestingLocationID;
                    record.VfrrenamerWatchFolderId = generalSettings.VFRRenamerWatchFolderID;
                    record.VfrduplicateRemoverWatchFolderId = generalSettings.VFRDuplicateRemoverWatchFolderID;
                    record.VfrbatchUploaderWatchFolderId = generalSettings.VFRDuplicateRemoverWatchFolderID;
                    record.VfrbatchMonitorFolderId = generalSettings.VFRBatchMonitorFolderID;
                    record.QcoutputFolderId = generalSettings.QCOutputFolderID;

                    if (Matching_Result == null)
                    {
                        // DB.Smtp.Add(record);
                        DB.GeneralSettings.Add(record);
                        DB.SaveChanges();
                        result.Message = "There was not information associated to a General Seeting in the Database, so new records was created successfully.";
                    }
                    else
                    {
                        // Means --> table has a record and it will be updated
                        Matching_Result.DebugFlag = Convert.ToString(generalSettings.DebugFlag);
                        Matching_Result.CpapplicationFilePath = generalSettings.CPApplicationFilePath;
                        Matching_Result.ImageViewerFilePath = generalSettings.ImageViewerFilePath;
                        Matching_Result.Dbserver = generalSettings.DBServer;
                        Matching_Result.DbuserName = generalSettings.DBUserName;
                        Matching_Result.Dbpassword = generalSettings.DBPassword;
                        Matching_Result.Dbprovider = generalSettings.DBProvider;
                        Matching_Result.Dbname = generalSettings.DBName;
                        Matching_Result.Dbrdbms = generalSettings.DBRDBMS;
                      
                        Matching_Result.AutoImportWatchFolderId = generalSettings.AutoImportWatchFolderID;
                        Matching_Result.ScanningFolderId = generalSettings.ScanningFolderID;
                        Matching_Result.PostValidationWatchFolderId = generalSettings.PostValidationWatchFolderID;
                        Matching_Result.LoadBalancerWatchFolderId = generalSettings.LoadBalancerWatchFolderID;
                        Matching_Result.BackupFolderId = generalSettings.BackupFolderID;
                        Matching_Result.FileConversionWatchFolderId = generalSettings.FileConversionWatchFolderID;
                        Matching_Result.BatchDeliveryWatchFolderId = generalSettings.BatchDeliveryWatchFolderID;
                        Matching_Result.RestingLocationId = generalSettings.RestingLocationID;
                        Matching_Result.VfrrenamerWatchFolderId = generalSettings.VFRRenamerWatchFolderID;
                        Matching_Result.VfrduplicateRemoverWatchFolderId = generalSettings.VFRDuplicateRemoverWatchFolderID;
                        Matching_Result.VfrbatchUploaderWatchFolderId = generalSettings.VFRDuplicateRemoverWatchFolderID;
                        Matching_Result.VfrbatchMonitorFolderId = generalSettings.VFRBatchMonitorFolderID;
                        Matching_Result.QcoutputFolderId = generalSettings.QCOutputFolderID;

                        DB.SaveChanges();
                        result.Message = "UpdateGeneralSettings Inforation was updated successfully.";
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
            logger.Trace("Leaving UpdateGeneralSettings Method ...");
            return result;
        }


        /// <summary>
        /// Get List of Stations
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultServiceStationsExtended GetServiceStations()
        {
            List<GlobalVars.ServiceStationExtended> stations = new List<GlobalVars.ServiceStationExtended>();
            GlobalVars.ResultServiceStationsExtended resultStations = new GlobalVars.ResultServiceStationsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = stations,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetServiceStations Method ...");
                
                // Get working folders. We will use the result to get the names of the working folders by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ServiceStations;
                    resultStations.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ServiceStationExtended serviceStation = new GlobalVars.ServiceStationExtended()
                            {
                                StationID = x.StationId,
                                StationName = x.StationName,
                                PDFStationFlag = Convert.ToBoolean(x.PdfstationFlag),
                                WatchFolderID = Convert.ToInt32(x.WatchFolderId),
                                TargetFolderID = Convert.ToInt32(x.TargetFolderId),
                                BackupFolderID = Convert.ToInt32(x.BackupFolderId),
                                MaxNumberBatches = Convert.ToInt32(x.MaxNumberBatches),
                                WeekendFlag = Convert.ToBoolean(x.WeekendFlag),
                                WorkdayFlag = Convert.ToBoolean(x.WorkdayFlag),
                                WorkdayStartTime = x.WorkdayStartTime,
                                WorkdayEndTime = x.WorkdayEndTime,
                                WeekendStartTime = x.WeekendStartTime,
                                WeenkendEndTime = x.WeekendEndTime
                            };
                            serviceStation.WatchFolder = GetFolderPath(Convert.ToInt32(serviceStation.WatchFolderID), workingFolder);
                            serviceStation.TargetFolder = GetFolderPath(Convert.ToInt32(serviceStation.TargetFolderID), workingFolder);
                            stations.Add(serviceStation);
                        }
                    }
                }
                resultStations.ReturnValue = stations;
                resultStations.Message = "GetServiceStations transaction completed successfully. Number of records found: " + resultStations.RecordsCount;
                logger.Debug(resultStations.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultStations.ReturnCode = -2;
                resultStations.Message = e.Message;
                var baseException = e.GetBaseException();
                resultStations.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetServiceStations Method ...");
            return resultStations;
        }

        /// <summary>
        /// Get Service Station By ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultServiceStationsExtended GetServiceStationByID(int stationID)
        {
            List<GlobalVars.ServiceStationExtended> stations = new List<GlobalVars.ServiceStationExtended>();
            GlobalVars.ResultServiceStationsExtended resultStations = new GlobalVars.ResultServiceStationsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = stations,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetServiceStationByID Method ...");

                // Get working folders. We will use the result to get the names of the working folders by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ServiceStations.Where(x => x.StationId == stationID);
                    resultStations.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ServiceStationExtended serviceStation = new GlobalVars.ServiceStationExtended()
                            {
                                StationID = x.StationId,
                                StationName = x.StationName,
                                PDFStationFlag = Convert.ToBoolean(x.PdfstationFlag),
                                WatchFolderID = Convert.ToInt32(x.WatchFolderId),
                                TargetFolderID = Convert.ToInt32(x.TargetFolderId),
                                BackupFolderID = Convert.ToInt32(x.BackupFolderId),
                                MaxNumberBatches = Convert.ToInt32(x.MaxNumberBatches),
                                WeekendFlag = Convert.ToBoolean(x.WeekendFlag),
                                WorkdayFlag = Convert.ToBoolean(x.WorkdayFlag),
                                WorkdayStartTime = x.WorkdayStartTime,
                                WorkdayEndTime = x.WorkdayEndTime,
                                WeekendStartTime = x.WeekendStartTime,
                                WeenkendEndTime = x.WeekendEndTime
                            };
                            serviceStation.WatchFolder = GetFolderPath(Convert.ToInt32(serviceStation.WatchFolderID), workingFolder);
                            serviceStation.TargetFolder = GetFolderPath(Convert.ToInt32(serviceStation.TargetFolderID), workingFolder);
                            stations.Add(serviceStation);
                        }
                    }
                }
                resultStations.ReturnValue = stations;
                resultStations.Message = "GetServiceStationByID transaction completed successfully. Number of records found: " + resultStations.RecordsCount;
                logger.Debug(resultStations.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultStations.ReturnCode = -2;
                resultStations.Message = e.Message;
                var baseException = e.GetBaseException();
                resultStations.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetServiceStationByID Method ...");
            return resultStations;
        }


        /// <summary>
        /// Get Service Station By Name
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultServiceStationsExtended GetServiceStationByBName(string stationName)
        {
            List<GlobalVars.ServiceStationExtended> stations = new List<GlobalVars.ServiceStationExtended>();
            GlobalVars.ResultServiceStationsExtended resultStations = new GlobalVars.ResultServiceStationsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = stations,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetServiceStationByBName Method ...");

                // Get working folders. We will use the result to get the names of the working folders by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ServiceStations.Where(x => x.StationName == stationName);
                    resultStations.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ServiceStationExtended serviceStation = new GlobalVars.ServiceStationExtended()
                            {
                                StationID = x.StationId,
                                StationName = x.StationName,
                                PDFStationFlag = Convert.ToBoolean(x.PdfstationFlag),
                                WatchFolderID = Convert.ToInt32(x.WatchFolderId),
                                TargetFolderID = Convert.ToInt32(x.TargetFolderId),
                                BackupFolderID = Convert.ToInt32(x.BackupFolderId),
                                MaxNumberBatches = Convert.ToInt32(x.MaxNumberBatches),
                                WeekendFlag = Convert.ToBoolean(x.WeekendFlag),
                                WorkdayFlag = Convert.ToBoolean(x.WorkdayFlag),
                                WorkdayStartTime = x.WorkdayStartTime,
                                WorkdayEndTime = x.WorkdayEndTime,
                                WeekendStartTime = x.WeekendStartTime,
                                WeenkendEndTime = x.WeekendEndTime
                            };
                            serviceStation.WatchFolder = GetFolderPath(Convert.ToInt32(serviceStation.WatchFolderID), workingFolder);
                            serviceStation.TargetFolder = GetFolderPath(Convert.ToInt32(serviceStation.TargetFolderID), workingFolder);
                            stations.Add(serviceStation);
                        }
                    }
                }
                resultStations.ReturnValue = stations;
                resultStations.Message = "GetServiceStationByBName transaction completed successfully. Number of records found: " + resultStations.RecordsCount;
                logger.Debug(resultStations.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultStations.ReturnCode = -2;
                resultStations.Message = e.Message;
                var baseException = e.GetBaseException();
                resultStations.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetServiceStationByBName Method ...");
            return resultStations;
        }

        /// <summary>
        /// Get PDF Stations
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultServiceStationsExtended GetPDFServiceStations()
        {
            List<GlobalVars.ServiceStationExtended> stations = new List<GlobalVars.ServiceStationExtended>();
            GlobalVars.ResultServiceStationsExtended resultStations = new GlobalVars.ResultServiceStationsExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = stations,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetPDFServiceStations Method ...");
                
                // Get working folders. We will use the result to get the names of the working folders by ID
                GlobalVars.ResultWorkingFolders workingFolder = new GlobalVars.ResultWorkingFolders();
                workingFolder = SQLFunctionsGeneralSettings.GetWorkingFolders();

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ServiceStations.Where(x => x.PdfstationFlag == "true");
                    resultStations.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ServiceStationExtended serviceStation = new GlobalVars.ServiceStationExtended()
                            {
                                StationID = x.StationId,
                                StationName = x.StationName,
                                PDFStationFlag = Convert.ToBoolean(x.PdfstationFlag),
                                WatchFolderID = Convert.ToInt32(x.WatchFolderId),
                                TargetFolderID = Convert.ToInt32(x.TargetFolderId),
                                BackupFolderID = Convert.ToInt32(x.BackupFolderId),
                                MaxNumberBatches = Convert.ToInt32(x.MaxNumberBatches),
                                WeekendFlag = Convert.ToBoolean(x.WeekendFlag),
                                WorkdayFlag = Convert.ToBoolean(x.WorkdayFlag),
                                WorkdayStartTime = x.WorkdayStartTime,
                                WorkdayEndTime = x.WorkdayEndTime,
                                WeekendStartTime = x.WeekendStartTime,
                                WeenkendEndTime = x.WeekendEndTime
                            };
                            serviceStation.WatchFolder = GetFolderPath(Convert.ToInt32(serviceStation.WatchFolderID), workingFolder);
                            serviceStation.TargetFolder = GetFolderPath(Convert.ToInt32(serviceStation.TargetFolderID), workingFolder);
                            stations.Add(serviceStation);
                        }
                    }
                }
                resultStations.ReturnValue = stations;
                resultStations.Message = "GetPDFServiceStations transaction completed successfully. Number of records found: " + resultStations.RecordsCount;
                logger.Debug(resultStations.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultStations.ReturnCode = -2;
                resultStations.Message = e.Message;
                var baseException = e.GetBaseException();
                resultStations.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetPDFServiceStations Method ...");
            return resultStations;
        }


        

        /// <summary>
        /// Use to determine if a given Service Station ID Exist in the Database
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistServiceStation(string stationName)
        {
            List<GlobalVars.ServiceStation> serviceStations = new List<GlobalVars.ServiceStation>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistServiceStation Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ServiceStations.Where(x => x.StationName == stationName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Service Station Name" + stationName + " already exist.";
                    }
                    else
                    {
                        result.Message = "Service Station Name" + stationName + "  doest not exist.";
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
            logger.Trace("Leaving ExistServiceStation Method ...");
            return result;
        }


        /// <summary>
        /// Create a new Service Station
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewServiceStation(GlobalVars.ServiceStation serviceStation)
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
                logger.Trace("Entering into NewServiceStation Method ...");

                // Check if given Station Name does exist
                result = SQLFunctionsGeneralSettings.ExistServiceStation(serviceStation.StationName);
                if (result.RecordsCount == 0)
                {                  
                    // Create new Field
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        ServiceStations New_Record = new ServiceStations();
                        New_Record.StationName = serviceStation.StationName;
                        New_Record.PdfstationFlag = Convert.ToString(serviceStation.PDFStationFlag);
                        New_Record.WatchFolderId = serviceStation.WatchFolderID;
                        New_Record.TargetFolderId = serviceStation.TargetFolderID;
                        New_Record.BackupFolderId = serviceStation.BackupFolderID;
                        New_Record.MaxNumberBatches = serviceStation.MaxNumberBatches;
                        New_Record.WeekendFlag = Convert.ToString(serviceStation.WeekendFlag);
                        New_Record.WeekendStartTime = serviceStation.WeekendStartTime;
                        New_Record.WeekendEndTime = serviceStation.WeenkendEndTime;
                        New_Record.WorkdayFlag = Convert.ToString(serviceStation.WorkdayFlag);
                        New_Record.WorkdayStartTime = serviceStation.WeekendStartTime;
                        New_Record.WorkdayEndTime = serviceStation.WorkdayEndTime;
                        DB.ServiceStations.Add(New_Record);
                        DB.SaveChanges();
                    }
                    result.Message = "NewServiceStation transaction completed successfully. One Record added.";                   
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Station Name " + serviceStation.StationName + " does exist. NewServiceStation transaction will be ignored.";
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
            logger.Trace("Leaving NewServiceStation Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateServiceStation(GlobalVars.ServiceStation serviceStation)
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
                logger.Trace("Entering into UpdateServiceStation Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Service Station ID must exist
                    ServiceStations Matching_Result = DB.ServiceStations.FirstOrDefault(x => x.StationId == serviceStation.StationID);
                    if (Matching_Result != null)
                    {
                        Matching_Result.PdfstationFlag = Convert.ToString(serviceStation.PDFStationFlag);
                        Matching_Result.WatchFolderId = serviceStation.WatchFolderID;
                        Matching_Result.TargetFolderId = serviceStation.TargetFolderID;
                        Matching_Result.MaxNumberBatches = serviceStation.MaxNumberBatches;
                        Matching_Result.BackupFolderId = serviceStation.BackupFolderID;
                        Matching_Result.WeekendFlag = Convert.ToString(serviceStation.WeekendFlag);
                        Matching_Result.WeekendStartTime = serviceStation.WeekendStartTime;
                        Matching_Result.WeekendEndTime = serviceStation.WeenkendEndTime;
                        Matching_Result.WorkdayFlag = Convert.ToString(serviceStation.WorkdayFlag);
                        Matching_Result.WorkdayStartTime = serviceStation.WorkdayStartTime;
                        Matching_Result.WorkdayEndTime = serviceStation.WorkdayEndTime;

                        DB.SaveChanges();
                            result.Message = "UpdateServiceStation transaction completed successfully. One Record Updated.";
                    }
                    else
                    {
                        // Means --> the name already exist
                        result.ReturnCode = -1;
                        result.Message = "Station ID " + Convert.ToString(serviceStation.StationID) + " does not exist in Database. UpdateServiceStation transaction ignore.";
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
            logger.Trace("Leaving UpdateServiceStation Method ...");
            return result;
        }


        /// <summary>
        /// Remove Service Station and associated information from Database
        /// </summary>
        /// <param name="stationID"></param>
        static public GlobalVars.ResultGeneric DeleteServiceStation(int stationID)
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
                logger.Trace("Entering into DeleteServiceStation Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    ServiceStations Matching_Result = DB.ServiceStations.FirstOrDefault(x => x.StationId == stationID);
                    if (Matching_Result != null)
                    {
                        DB.ServiceStations.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteServiceStation transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Station ID" + stationID + " does not exist. DeleteServiceStation transaction ignore.";
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
            logger.Trace("Leaving DeleteServiceStation Method ...");
            return result;
        }

        /// <summary>
        /// Get List of Working Folders
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultWorkingFolders GetWorkingFolders()
        {
            List<GlobalVars.WorkingFolder> folders = new List<GlobalVars.WorkingFolder>();
            GlobalVars.ResultWorkingFolders resultWorkingFolders = new GlobalVars.ResultWorkingFolders()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = folders,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetWorkingFolders Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.WorkingFolders;
                    resultWorkingFolders.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.WorkingFolder workingFolder = new GlobalVars.WorkingFolder()
                            {
                                FolderID = x.FolderId,
                                Path = x.Path
                            };
                            folders.Add(workingFolder);
                        }
                    }
                }
                resultWorkingFolders.ReturnValue = folders;
                resultWorkingFolders.Message = "GetWorkingFolders transaction completed successfully. Number of records found: " + resultWorkingFolders.RecordsCount;
                logger.Debug(resultWorkingFolders.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultWorkingFolders.ReturnCode = -2;
                resultWorkingFolders.Message = e.Message;
                var baseException = e.GetBaseException();
                resultWorkingFolders.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetWorkingFolders Method ...");
            return resultWorkingFolders;
        }

        /// <summary>
        /// Get Workimg Folder By ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultWorkingFolders GetWorkingFolderByID(int folderID)
        {
            List<GlobalVars.WorkingFolder> folders = new List<GlobalVars.WorkingFolder>();
            GlobalVars.ResultWorkingFolders resultWorkingFolders = new GlobalVars.ResultWorkingFolders()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = folders,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetWorkingFolderByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.WorkingFolders.Where(x => x.FolderId == folderID);
                    resultWorkingFolders.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.WorkingFolder workingFolder = new GlobalVars.WorkingFolder()
                            {
                                FolderID = x.FolderId,
                                Path = x.Path
                            };
                            folders.Add(workingFolder);
                        }
                    }
                }
                resultWorkingFolders.ReturnValue = folders;
                resultWorkingFolders.Message = "GetWorkingFolderByID transaction completed successfully. Number of records found: " + resultWorkingFolders.RecordsCount;
                logger.Debug(resultWorkingFolders.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultWorkingFolders.ReturnCode = -2;
                resultWorkingFolders.Message = e.Message;
                var baseException = e.GetBaseException();
                resultWorkingFolders.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetWorkingFolderByID Method ...");
            return resultWorkingFolders;
        }

        /// <summary>
        /// Use to determine if a given Working Folder  Exist in the Database
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistWorkingFolder(string path)
        {
            Boolean pathFound = false;
            List<GlobalVars.WorkingFolder> folders = new List<GlobalVars.WorkingFolder>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistWorkingFolder Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.WorkingFolders;
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var record in results)
                        {
                            if (record.Path == path)
                            {                               
                                pathFound = true;
                                break;
                            }
                        }
                        //result.Message = "Path " + path + " already exist.";
                    }
                    if (pathFound)
                    {
                        result.Message = "Path " + path + " already exist.";
                        result.RecordsCount = 1;
                    }
                    else
                    {
                        result.Message = "Path " + path + "  doest not exist.";
                        result.RecordsCount = 0;
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
            logger.Trace("Leaving ExistWorkingFolder Method ...");
            return result;
        }

        /// <summary>
        /// Use to determine if a given Working Folder ID Exist in the Database
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistWorkingFolderID(int folderID)
        {
            Boolean pathFound = false;
            List<GlobalVars.WorkingFolder> folders = new List<GlobalVars.WorkingFolder>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistWorkingFolderID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.WorkingFolders;
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var record in results)
                        {
                            if (record.FolderId == folderID)
                            {
                                pathFound = true;
                                break;
                            }
                        }
                        //result.Message = "Path " + path + " already exist.";
                    }
                    if (pathFound)
                    {
                        result.Message = "Folder ID  " + folderID.ToString() + " already exist.";
                        result.RecordsCount = 1;
                    }
                    else
                    {
                        result.Message = "Folder ID " + folderID.ToString() + "  doest not exist.";
                        result.RecordsCount = 0;
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
            logger.Trace("Leaving ExistWorkingFolderID Method ...");
            return result;
        }

        /// <summary>
        /// Create a new Working Folder
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewWorkingFolder(GlobalVars.WorkingFolder folder)
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
                logger.Trace("Entering into NewWorkingFolder Method ...");

                // Check if given Working Folder does exist
                result = SQLFunctionsGeneralSettings.ExistWorkingFolder(folder.Path);
                if (result.RecordsCount == 0)
                {
                    // Create new Working Folder
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        WorkingFolders New_Record = new WorkingFolders();
                        New_Record.Path = folder.Path;
                        DB.WorkingFolders.Add(New_Record);
                        DB.SaveChanges();
                    }
                    result.Message = "NewWorkingFolder transaction completed successfully. One Record added.";
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Working Folder " + folder.Path + " does exist. NewWorkingFolder transaction will be ignored.";
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
            logger.Trace("Leaving NewWorkingFolder Method ...");
            return result;
        }

        /// <summary>
        /// Remove Working Folder and associated information from Database
        /// </summary>
        /// <param name="folderID"></param>
        static public GlobalVars.ResultGeneric DeleteWorkingFolder(int folderID)
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
                logger.Trace("Entering into DeleteWorkingFolder Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    WorkingFolders Matching_Result = DB.WorkingFolders.FirstOrDefault(x => x.FolderId == folderID);
                    if (Matching_Result != null)
                    {
                        DB.WorkingFolders.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteWorkingFolder transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Folder ID" + folderID + " does not exist. DeleteWorkingFolder transaction ignore.";
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
            logger.Trace("Leaving DeleteWorkingFolder Method ...");
            return result;
        }
    }
}
