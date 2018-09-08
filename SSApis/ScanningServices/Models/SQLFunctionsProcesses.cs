using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using Newtonsoft.Json;
using static ScanningServicesDataObjects.GlobalVars;
using CronExpressionDescriptor;
using System.IO;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLFunctionsProcesses
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Processes Types
        /// </summary>
        /// <returns>Indexer, Synchronizer, Batch Load Balancer, Batch Delivery, etc</returns>
        static public GlobalVars.ResultProcessTypes GetProcessTypes()
        {
            List<GlobalVars.ProcessType> processTypes = new List<GlobalVars.ProcessType>();
            GlobalVars.ResultProcessTypes resultProcessTypes = new GlobalVars.ResultProcessTypes()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processTypes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessTypes Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ProcessesTypes;
                    resultProcessTypes.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ProcessType process = new GlobalVars.ProcessType()
                            {
                                ProcessID = x.Id,
                                Name = (x.Name ?? "").Trim()                                
                            };
                            processTypes.Add(process);
                        }
                    }
                }
                resultProcessTypes.ReturnValue= processTypes;
                resultProcessTypes.Message = "GetProcesses transaction completed successfully. Number of records found: " + resultProcessTypes.RecordsCount;
                logger.Debug(resultProcessTypes.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcessTypes.ReturnCode = -2;
                resultProcessTypes.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcessTypes.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessTypes Method ...");
            return resultProcessTypes;
        }

        /// <summary>
        /// Get Process Type by a given Process Name
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcessTypes GetProcessTypeByName(string processName)
        {
            List<GlobalVars.ProcessType> processTypes = new List<GlobalVars.ProcessType>();
            GlobalVars.ResultProcessTypes resultProcessTypes = new GlobalVars.ResultProcessTypes()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processTypes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessTypeByName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    //var results = DB.ProcessesTypes.FirstOrDefault(x => x.Name == processName);
                    var results = from pt in DB.ProcessesTypes
                                  where pt.Name == processName
                                  select new { pt};

                    resultProcessTypes.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ProcessType process = new GlobalVars.ProcessType()
                            {
                                ProcessID = x.pt.Id,
                                Name = (x.pt.Name ?? "").Trim()
                            };
                            processTypes.Add(process);
                        }
                    }
                }
                resultProcessTypes.ReturnValue = processTypes;
                resultProcessTypes.Message = "GetProcessTypeByName transaction completed successfully. Number of records found: " + resultProcessTypes.RecordsCount;
                logger.Debug(resultProcessTypes.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcessTypes.ReturnCode = -2;
                resultProcessTypes.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcessTypes.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessTypeByName Method ...");
            return resultProcessTypes;
        }

       
       /// <summary>
       /// Check if a Process Name has been already defined in the Database
       /// Only process listes in the ProcessType Database Table, can be used to create Jobs
       /// </summary>
       /// <param name="processName"></param>
       /// <returns></returns>
        static public GlobalVars.ResultGeneric ValidProcessName(string processName)
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
                logger.Trace("Entering into ValidProcessName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ProcessesTypes.Where(x => x.Name == processName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Process " + processName + " already exist.";
                    }
                    else
                    {
                        result.Message = "Process  " + processName + " doest not exist.";
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
            logger.Trace("Leaving ValidProcessName Method ...");
            return result;
        }

        /// <summary>
        /// Get Processes
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcesses GetProcesses()
        {
            List<GlobalVars.Process> processes = new List<GlobalVars.Process>();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ResultProcesses Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = from p in DB.Processes
                                  select new { p };

                    //var results = DB.JobsProcesses;
                    resultProcesses.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            // Retrieve the process Name
                            string processName = "";
                            ProcessesTypes Matching_ProcessType = DB.ProcessesTypes.FirstOrDefault(y => y.Id == x.p.ProcessId);
                            if (Matching_ProcessType != null)
                            {
                                processName = Matching_ProcessType.Name;
                            }

                            // Retrieve the Station Name
                            string stationName = "";
                            ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.p.StationId);
                            if (Matching_ServiceStation != null)
                            {
                                stationName = Matching_ServiceStation.StationName;
                            }

                            // Retrieve Job Name
                            string jobName = "";
                            Jobs Matching_Job = DB.Jobs.FirstOrDefault(y => y.JobId == x.p.JobId);
                            if (Matching_Job != null)
                            {
                                jobName = Matching_Job.JobName;
                            }

                            // Retrieve the PDFStation Name
                            string pdfStationName = "";
                            ServiceStations Matching_PDFServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.p.PdfstationId);
                            if (Matching_PDFServiceStation != null)
                            {
                                pdfStationName = Matching_PDFServiceStation.StationName;
                            }

                            var result = ExpressionDescriptor.GetDescription(GeneralTools.scheduleStringBuilder(x.p.Schedule), new Options());
                            GlobalVars.Process process = new GlobalVars.Process()
                            {
                                ProcessID = x.p.ProcessId,
                                JobID = x.p.JobId,
                                StationID = Convert.ToInt32(x.p.StationId),
                                PDFStationID = x.p.PdfstationId,
                                ProcessName = (processName ?? "").Trim(),
                                JobName = (jobName ?? "").Trim(),
                                StationName = (stationName ?? "").Trim(),
                                PDFStationName = (pdfStationName ?? "").Trim(),
                                Description = (x.p.Description ?? "").Trim(),                                
                                Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(x.p.Schedule),
                                ScheduleCronFormat = GeneralTools.scheduleStringBuilder(x.p.Schedule),   
                                CronDescription = result.ToString(),
                                EnableFlag = Convert.ToBoolean(x.p.EnableFlag)
                            };
                            processes.Add(process);
                        }
                    }
                }
                resultProcesses.ReturnValue = processes;
                resultProcesses.Message = "ResultProcesses transaction completed successfully. Number of records found: " + resultProcesses.RecordsCount;
                logger.Debug(resultProcesses.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
            }
            logger.Trace("Leaving ResultProcesses Method ...");
            return resultProcesses;
        }

        /// <summary>
        /// Get Processes by a given Process ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcesses GetProcessesByID(int processID)
        {
            List<GlobalVars.Process> processes = new List<GlobalVars.Process>();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessesByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    // Retrieve the process Name
                    string processName = "";
                    ProcessesTypes Matching_ProcessType = DB.ProcessesTypes.FirstOrDefault(y => y.Id == processID);
                    if (Matching_ProcessType != null)
                    {
                        processName = Matching_ProcessType.Name;

                        var results = from jp in DB.Processes
                                      where jp.ProcessId == processID
                                      select new { jp };

                        //var results = DB.JobsProcesses.Where(x => x.ProcessId == processID);
                        resultProcesses.RecordsCount = results.Count();
                        if (results.Count() >= 1)
                        {
                            foreach (var x in results)
                            {
                                // Retrieve the Station Name
                                string stationName = "";
                                ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.StationId);
                                if (Matching_ServiceStation != null)
                                {
                                    stationName = Matching_ServiceStation.StationName;
                                }

                                // Retrieve Job Name
                                string jobName = "";
                                Jobs Matching_Job = DB.Jobs.FirstOrDefault(y => y.JobId == x.jp.JobId);
                                if (Matching_Job != null)
                                {
                                    jobName = Matching_Job.JobName;
                                }

                                // Retrieve the PDFStation Name
                                string pdfStationName = "";
                                ServiceStations Matching_PDFServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.PdfstationId);
                                if (Matching_PDFServiceStation != null)
                                {
                                    pdfStationName = Matching_PDFServiceStation.StationName;
                                }

                                var result = ExpressionDescriptor.GetDescription(GeneralTools.scheduleStringBuilder(x.jp.Schedule), new Options());
                                GlobalVars.Process process = new GlobalVars.Process()
                                {
                                    ProcessID = x.jp.ProcessId,
                                    JobID = x.jp.JobId,
                                    StationID = Convert.ToInt32(x.jp.StationId),
                                    PDFStationID = x.jp.PdfstationId,
                                    ProcessName = (processName ?? "").Trim(),
                                    JobName = (jobName ?? "").Trim(),
                                    StationName = (stationName ?? "").Trim(),
                                    PDFStationName = (pdfStationName ?? "").Trim(),
                                    Description = (x.jp.Description ?? "").Trim(),
                                    Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(x.jp.Schedule),
                                    ScheduleCronFormat = GeneralTools.scheduleStringBuilder(x.jp.Schedule),
                                    CronDescription = result.ToString(),
                                    EnableFlag = Convert.ToBoolean(x.jp.EnableFlag),
                                };
                                processes.Add(process);
                            }
                        }
                    }

                    
                }
                resultProcesses.ReturnValue = processes;
                resultProcesses.Message = "GetProcessesByID transaction completed successfully. Number of records found: " + resultProcesses.RecordsCount;
                logger.Debug(resultProcesses.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessesByID Method ...");
            return resultProcesses;
        }

        /// <summary>
        /// Get Processes by a given Job ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcesses GetProcessesByJobID(int jobID)
        {
            List<GlobalVars.Process> processes = new List<GlobalVars.Process>();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessesByJobID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    // Retrieve the process Name
                    Jobs Matching_ProcessType = DB.Jobs.FirstOrDefault(y => y.JobId == jobID);
                    if (Matching_ProcessType != null)
                    {
                        //processName = Matching_ProcessType.Name;

                        var results = from jp in DB.Processes
                                      where jp.JobId == jobID
                                      select new { jp };

                        //var results = DB.JobsProcesses.Where(x => x.ProcessId == processID);
                        resultProcesses.RecordsCount = results.Count();
                        if (results.Count() >= 1)
                        {
                            foreach (var x in results)
                            {
                                string processName = "";
                                ProcessesTypes Matching_ProcessName = DB.ProcessesTypes.FirstOrDefault(y => y.Id == x.jp.ProcessId);
                                if (Matching_ProcessName != null)
                                {
                                    processName = Matching_ProcessName.Name;
                                }

                                // Retrieve the Station Name
                                string stationName = "";
                                ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.StationId);
                                if (Matching_ServiceStation != null)
                                {
                                    stationName = Matching_ServiceStation.StationName;
                                }

                                // Retrieve Job Name
                                string jobName = "";
                                Jobs Matching_Job = DB.Jobs.FirstOrDefault(y => y.JobId == x.jp.JobId);
                                if (Matching_Job != null)
                                {
                                    jobName = Matching_Job.JobName;
                                }

                                // Retrieve the PDFStation Name
                                string pdfStationName = "";
                                ServiceStations Matching_PDFServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.PdfstationId);
                                if (Matching_PDFServiceStation != null)
                                {
                                    pdfStationName = Matching_PDFServiceStation.StationName;
                                }

                                var result = ExpressionDescriptor.GetDescription(GeneralTools.scheduleStringBuilder(x.jp.Schedule), new Options());
                                GlobalVars.Process process = new GlobalVars.Process()
                                {
                                    ProcessID = x.jp.ProcessId,
                                    JobID = x.jp.JobId,
                                    StationID = Convert.ToInt32(x.jp.StationId),
                                    PDFStationID = x.jp.PdfstationId,
                                    ProcessName = (processName ?? "").Trim(),
                                    JobName = (jobName ?? "").Trim(),
                                    StationName = (stationName ?? "").Trim(),
                                    PDFStationName = (pdfStationName ?? "").Trim(),
                                    Description = (x.jp.Description ?? "").Trim(),
                                    Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(x.jp.Schedule),
                                    ScheduleCronFormat = GeneralTools.scheduleStringBuilder(x.jp.Schedule),
                                    CronDescription = result.ToString(),
                                    EnableFlag = Convert.ToBoolean(x.jp.EnableFlag),
                                };
                                processes.Add(process);
                            }
                        }
                    }
                }
                resultProcesses.ReturnValue = processes;
                resultProcesses.Message = "GetProcessesByJobID transaction completed successfully. Number of records found: " + resultProcesses.RecordsCount;
                logger.Debug(resultProcesses.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessesByJobID Method ...");
            return resultProcesses;
        }

        /// <summary>
        /// Get Processes by a Process, Job, Station, and PDFStation IDs
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcesses GetProcessesByIDs(int processID, int jobID, int stationID, int pdfStationID)
        {
            List<GlobalVars.Process> processes = new List<GlobalVars.Process>();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessesByIDs Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = from p in DB.Processes
                                    where p.ProcessId == processID && p.JobId == jobID && p.StationId == stationID && p.PdfstationId == pdfStationID
                                    select new { p };

                    resultProcesses.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            // Retrieve the process Name
                            string processName = "";
                            ProcessesTypes Matching_ProcessType = DB.ProcessesTypes.FirstOrDefault(y => y.Id == x.p.ProcessId);
                            if (Matching_ProcessType != null)
                            {
                                processName = Matching_ProcessType.Name;
                            }

                            // Retrieve the Station Name
                            string stationName = "";
                            ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.p.StationId);
                            if (Matching_ServiceStation != null)
                            {
                                stationName = Matching_ServiceStation.StationName;
                            }

                            // Retrieve Job Name
                            string jobName = "";
                            Jobs Matching_Job = DB.Jobs.FirstOrDefault(y => y.JobId == x.p.JobId);
                            if (Matching_Job != null)
                            {
                                jobName = Matching_Job.JobName;
                            }

                            // Retrieve the PDFStation Name
                            string pdfStationName = "";
                            ServiceStations Matching_PDFServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.p.PdfstationId);
                            if (Matching_PDFServiceStation != null)
                            {
                                pdfStationName = Matching_PDFServiceStation.StationName;
                            }
                            var result = ExpressionDescriptor.GetDescription(GeneralTools.scheduleStringBuilder(x.p.Schedule), new Options());
                            GlobalVars.Process process = new GlobalVars.Process()
                            {
                                ProcessID = x.p.ProcessId,
                                JobID = x.p.JobId,
                                StationID = Convert.ToInt32(x.p.StationId),
                                PDFStationID = x.p.PdfstationId,
                                ProcessName = (processName ?? "").Trim(),
                                JobName = (jobName ?? "").Trim(),
                                StationName = (stationName ?? "").Trim(),
                                PDFStationName = (pdfStationName ?? "").Trim(),
                                Description = (x.p.Description ?? "").Trim(),
                                Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(x.p.Schedule),
                                ScheduleCronFormat = GeneralTools.scheduleStringBuilder(x.p.Schedule),
                                CronDescription = result.ToString(),
                                EnableFlag = Convert.ToBoolean(x.p.EnableFlag)
                            };
                            processes.Add(process);
                        }
                    }
                }
                resultProcesses.ReturnValue = processes;
                resultProcesses.Message = "GetProcessesByIDs transaction completed successfully. Number of records found: " + resultProcesses.RecordsCount;
                logger.Debug(resultProcesses.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessesByIDs Method ...");
            return resultProcesses;
        }


        /// <summary>
        /// Get Processes by a given Name
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcesses GetProcessesByName(string processName)
        {
            List<GlobalVars.Process> processes = new List<GlobalVars.Process>();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessesByName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Get the process ID from Process Types Entity Table for a given process name
                    ProcessesTypes Matching_Result = DB.ProcessesTypes.FirstOrDefault(x => x.Name == processName);
                    if (Matching_Result != null)
                    {
                        int processID = Matching_Result.Id;

                        // Get Master Process Information frm JobsProcesses table
                        var results = from jp in DB.Processes
                                      join pt in DB.ProcessesTypes on jp.ProcessId equals pt.Id
                                      where jp.ProcessId == processID
                                      select new { jp, pt.Name};

                        resultProcesses.RecordsCount = results.Count();
                        if (results.Count() >= 1)
                        {
                            foreach (var x in results)
                            {
                                // Retrieve the Station Name
                                string stationName = "";
                                ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.StationId);
                                if (Matching_ServiceStation != null)
                                {
                                    stationName = Matching_ServiceStation.StationName;
                                }

                                // Retrieve Job Name
                                string jobName = "";
                                Jobs Matching_Job = DB.Jobs.FirstOrDefault(y => y.JobId == x.jp.JobId);
                                if (Matching_Job != null)
                                {
                                    jobName = Matching_Job.JobName;
                                }

                                // Retrieve the PDFStation Name
                                string pdfStationName = "";
                                ServiceStations Matching_PDFServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.PdfstationId);
                                if (Matching_PDFServiceStation != null)
                                {
                                    pdfStationName = Matching_PDFServiceStation.StationName;
                                }

                                var result = ExpressionDescriptor.GetDescription(GeneralTools.scheduleStringBuilder(x.jp.Schedule), new Options());
                                GlobalVars.Process process = new GlobalVars.Process()
                                {
                                    ProcessID = x.jp.ProcessId,
                                    JobID = x.jp.JobId,
                                    StationID = Convert.ToInt32(x.jp.StationId),
                                    PDFStationID = x.jp.PdfstationId,
                                    ProcessName = (processName ?? "").Trim(),
                                    JobName = (jobName ?? "").Trim(),
                                    StationName = (stationName ?? "").Trim(),
                                    PDFStationName = (pdfStationName ?? "").Trim(),
                                    Description = (x.jp.Description ?? "").Trim(),
                                    Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(x.jp.Schedule),
                                    ScheduleCronFormat = GeneralTools.scheduleStringBuilder(x.jp.Schedule),
                                    CronDescription = result.ToString(),
                                    EnableFlag = Convert.ToBoolean(x.jp.EnableFlag),
                                };
                                processes.Add(process);
                            }
                        }
                    }
                }
                resultProcesses.ReturnValue = processes;
                resultProcesses.Message = "GetProcessesByName transaction completed successfully. Number of records found: " + resultProcesses.RecordsCount;
                logger.Debug(resultProcesses.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessesByName Method ...");
            return resultProcesses;
        }

        /// <summary>
        /// Get Processes by a given Station ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcesses GetProcessesByStationID(int stationID)
        {
            List<GlobalVars.Process> processes = new List<GlobalVars.Process>();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessesByStationID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                   var results = from jp in DB.Processes
                                  where jp.StationId == stationID
                                  select new { jp };

                    resultProcesses.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            // Retrieve the process Name
                            string processName = "";
                            ProcessesTypes Matching_ProcessType = DB.ProcessesTypes.FirstOrDefault(y => y.Id == x.jp.ProcessId);
                            if (Matching_ProcessType != null)
                            {
                                processName = Matching_ProcessType.Name;
                            }

                            // Retrieve the Station Name
                            string stationName = "";
                            ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.StationId);
                            if (Matching_ServiceStation != null)
                            {
                                stationName = Matching_ServiceStation.StationName;
                            }

                            // Retrieve Job Name
                            string jobName = "";
                            Jobs Matching_Job = DB.Jobs.FirstOrDefault(y => y.JobId == x.jp.JobId);
                            if (Matching_Job != null)
                            {
                                jobName = Matching_Job.JobName;
                            }

                            // Retrieve the PDFStation Name
                            string pdfStationName = "";
                            ServiceStations Matching_PDFServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.PdfstationId);
                            if (Matching_PDFServiceStation != null)
                            {
                                pdfStationName = Matching_PDFServiceStation.StationName;
                            }

                            var result = ExpressionDescriptor.GetDescription(GeneralTools.scheduleStringBuilder(x.jp.Schedule), new Options());
                            GlobalVars.Process process = new GlobalVars.Process()
                            {
                                ProcessID = x.jp.ProcessId,
                                JobID = x.jp.JobId,
                                StationID = Convert.ToInt32(x.jp.StationId),
                                PDFStationID = x.jp.PdfstationId,
                                ProcessName = (processName ?? "").Trim(),
                                JobName = (jobName ?? "").Trim(),
                                StationName = (stationName ?? "").Trim(),
                                PDFStationName = (pdfStationName ?? "").Trim(),
                                Description = (x.jp.Description ?? "").Trim(),
                                Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(x.jp.Schedule),
                                ScheduleCronFormat = GeneralTools.scheduleStringBuilder(x.jp.Schedule),                               
                                CronDescription = result.ToString(),
                                EnableFlag = Convert.ToBoolean(x.jp.EnableFlag),
                            };
                            processes.Add(process);
                        }
                    }

                }
                resultProcesses.ReturnValue = processes;
                resultProcesses.Message = "GetProcessesByStationID transaction completed successfully. Number of records found: " + resultProcesses.RecordsCount;
                logger.Debug(resultProcesses.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessesByStationID Method ...");
            return resultProcesses;
        }


        /// <summary>
        /// Get Processes by a given PDF Station ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProcesses GetProcessesByPDFStationID(int pdfStationID)
        {
            List<GlobalVars.Process> processes = new List<GlobalVars.Process>();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = processes,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProcessesByPDFStationID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = from jp in DB.Processes
                                  where jp.PdfstationId == pdfStationID
                                  select new { jp };

                    resultProcesses.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            // Retrieve the process Name
                            string processName = "";
                            ProcessesTypes Matching_ProcessType = DB.ProcessesTypes.FirstOrDefault(y => y.Id == x.jp.ProcessId);
                            if (Matching_ProcessType != null)
                            {
                                processName = Matching_ProcessType.Name;
                            }

                            // Retrieve the Station Name
                            string stationName = "";
                            ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.StationId);
                            if (Matching_ServiceStation != null)
                            {
                                stationName = Matching_ServiceStation.StationName;
                            }

                            // Retrieve Job Name
                            string jobName = "";
                            Jobs Matching_Job = DB.Jobs.FirstOrDefault(y => y.JobId == x.jp.JobId);
                            if (Matching_Job != null)
                            {
                                jobName = Matching_Job.JobName;
                            }

                            // Retrieve the PDFStation Name
                            string pdfStationName = "";
                            ServiceStations Matching_PDFServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == x.jp.PdfstationId);
                            if (Matching_PDFServiceStation != null)
                            {
                                pdfStationName = Matching_PDFServiceStation.StationName;
                            }

                            var result = ExpressionDescriptor.GetDescription(GeneralTools.scheduleStringBuilder(x.jp.Schedule), new Options());
                            GlobalVars.Process process = new GlobalVars.Process()
                            {
                                ProcessID = x.jp.ProcessId,
                                JobID = x.jp.JobId,                               
                                StationID = Convert.ToInt32(x.jp.StationId),
                                PDFStationID = x.jp.PdfstationId,
                                ProcessName = (processName ?? "").Trim(),
                                JobName = (jobName ?? "").Trim(),
                                StationName = (stationName ?? "").Trim(),
                                PDFStationName = (pdfStationName ?? "").Trim(),
                                Description = (x.jp.Description ?? "").Trim(),
                                Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(x.jp.Schedule),
                                ScheduleCronFormat = GeneralTools.scheduleStringBuilder(x.jp.Schedule),                               
                                EnableFlag = Convert.ToBoolean(x.jp.EnableFlag),
                                CronDescription = result.ToString()
                            };
                            processes.Add(process);
                        }
                    }
                }
                resultProcesses.ReturnValue = processes;
                resultProcesses.Message = "GetProcessesByPDFStationID transaction completed successfully. Number of records found: " + resultProcesses.RecordsCount;
                logger.Debug(resultProcesses.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProcessesByPDFStationID Method ...");
            return resultProcesses;
        }

        /// <summary>
        /// Creates a new Process or Instance of a Master Process
        /// An instance of a Process is a record that identify a Process than runs on a particular Service Station.
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateProcess(GlobalVars.ProcessUpdate process)
        {
            Boolean continueProcessing = true;
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into UpdateProcess Method ...");
                
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    //Check if process id  is valid
                    ProcessesTypes Matching_ProcessType = DB.ProcessesTypes.FirstOrDefault(y => y.Id == process.ProcessID);
                    if (Matching_ProcessType == null)
                    {
                        continueProcessing = false;
                        result.ReturnCode = -1;
                        result.Message = "Process ID " + process.ProcessID.ToString() + " does not exist in the Database.";
                    }

                    if (continueProcessing)
                    {
                        //Check if station id exist in the Database
                        ServiceStations Matching_ServiceStation = DB.ServiceStations.FirstOrDefault(y => y.StationId == process.StationID);
                        if (Matching_ServiceStation == null)
                        {
                            continueProcessing = false;
                            result.ReturnCode = -1;
                            result.Message = "Service Station ID " + process.StationID.ToString() + " does not exist in the Database.";
                        }
                    }

                    if (continueProcessing)
                    {
                        //Check schedule is provided
                        if (process.Schedule == null)
                        {
                            continueProcessing = false;
                            result.ReturnCode = -1;
                            result.Message = "Schule information is missing.";
                        }                        
                    }

                    if (continueProcessing)
                    {
                        //Check if Backup folder id exist in the Database, if backup id is provided inthe call 
                        //The Batch Delivery Process will look for the Backup Folder of the Load Balancer
                    }

                    if (continueProcessing)
                    {
                        string processJS = JsonConvert.SerializeObject(process.Schedule, Newtonsoft.Json.Formatting.Indented);

                        Processes record = new Processes();
                        record.ProcessId = process.ProcessID;
                        record.StationId = process.StationID;
                        record.EnableFlag = Convert.ToString(process.EnableFlag);
                        record.JobId = process.JobID;
                        record.PdfstationId = process.PDFStationID;
                        record.Description = process.Description;
                        record.Schedule = processJS;
                        //record.SleepTime = process.SleepTime;
                        
                        // Initialy, I have this condition but the Methid was creating multiple entries for different Stations IDs and we only allow one
                        // We should consider to take the Station ID as Key in the Process Datbase Table
                        // Combination of Station Id and Process ID must be unique in the Dabatase
                        // Processes Matching_Result = DB.Processes.FirstOrDefault(x => (x.ProcessId == process.ProcessID && x.StationId == process.StationID && x.JobId == process.JobID && x.PdfstationId == process.PDFStationID));

                        // Combination of Process Id and Job ID must be unique in the Dabatase
                        Processes Matching_Result = DB.Processes.FirstOrDefault(x => (x.ProcessId == process.ProcessID && x.JobId == process.JobID && x.PdfstationId == process.PDFStationID));
                        if (Matching_Result == null)
                        {
                            // Means --> the Process given was not found in the Database so a new record will be created
                            DB.Processes.Add(record);
                            DB.SaveChanges();
                            result.Message = "There was not information associated to the given Process, so new records was created successfully.";
                        }
                        else
                        {
                            // Means --> given Process/Job is in the Database, so we need to delete the exiisting Record and add a new one
                            // Remove Record fromthe Database
                            DB.Processes.Remove(Matching_Result);
                            DB.SaveChanges();
                            // Then add record with new information into the Database
                            DB.Processes.Add(record);
                            DB.SaveChanges();

                            //Matching_Result.EnableFlag = Convert.ToString(process.EnableFlag);
                            //Matching_Result.Description = process.Description;
                            //Matching_Result.Schedule = processJS;
                            //Matching_Result.StationId = process.StationID;
                            //DB.SaveChanges();
                            result.Message = "UpdateProcess transaction completed successfully. One Record added.";
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
            logger.Trace("Leaving UpdateProcess Method ...");
            return result;
        }

        /// <summary>
        /// Get Next Available File Conversion Station
        /// This Method browses the Watch Folder for the File Conversion Stations that are available
        /// The folder selection is based only in the number of available Batched for a File Conversion Station, so
        /// the station that has more capacity will be selected.
        /// If there is more that one fileconversion station with the same number of available batches, one of them will be selected        /// 
        /// </summary>
        /// <returns> Selected Watch Folder in StringReturnValue, and Number of available capacity in IntegerNumberReturnValue</returns>
        static public GlobalVars.ResultGeneric GetNextAvailableFileConversionStation()
        {
            GlobalVars.ResultGeneric resultStations = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                StringReturnValue = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            DateTime currentDT;
            DateTime oldestDT;
            DateTime oldestFolder;
            int minFoldersCount;
            string fileConversionFolder;
            int currentAvailableBatches;
            int stationFoldersCount;
            try
            {
                logger.Trace("Entering into GetNextAvailableFileConversionStation Method ...");

                // Get Jobs
                GlobalVars.ResultJobsExtended resultJobs = new ResultJobsExtended();
                resultJobs = SQLFunctionsJobs.GetJobs();

                // Get Services stations
                //List<GlobalVars.ServiceStationExtended> stations = new List<GlobalVars.ServiceStationExtended>();
                GlobalVars.ResultServiceStationsExtended resultFileConversionStations = new GlobalVars.ResultServiceStationsExtended();
                resultFileConversionStations = SQLFunctionsGeneralSettings.GetPDFServiceStations();

                DateTime dayToday = DateTime.Now;
                DateTime startTime;
                DateTime endTime;
                oldestDT = dayToday;
                oldestFolder = dayToday;
                fileConversionFolder = "";
                minFoldersCount = 100000;
                currentAvailableBatches = 0;

                foreach (GlobalVars.ServiceStationExtended station in resultFileConversionStations.ReturnValue)
                {
                    // Filter Stations that are enable only
                    if (station.PDFStationFlag)
                    {
                        stationFoldersCount = 0;
                        foreach (GlobalVars.JobExtended job in resultJobs.ReturnValue)
                        {
                            logger.Trace("  Anlizing Job :" + job.JobName);
                            logger.Trace("      Directory :" + station.WatchFolder + "\\" + job.JobName.Trim());

                            if (!string.IsNullOrEmpty(station.WatchFolder + "\\" + job.JobName.Trim()))
                            {
                                if (Directory.Exists(station.WatchFolder + "\\" + job.JobName.Trim()))
                                {
                                    string[] stationBatchFolders = System.IO.Directory.GetDirectories(station.WatchFolder + "\\" + job.JobName.Trim(), "*", System.IO.SearchOption.TopDirectoryOnly);
                                    stationFoldersCount = stationFoldersCount + stationBatchFolders.Count();
                                    logger.Trace("      Folders count in Directoy: " + stationFoldersCount);
                                }
                            }
                            else
                            {
                                logger.Trace("      Directory Does not exist.");
                            }
                        }

                        //
                        // Checking if we are in a Weekend or Workday
                        if ((dayToday.DayOfWeek == DayOfWeek.Saturday) || (dayToday.DayOfWeek == DayOfWeek.Sunday))
                        {
                            logger.Trace("      Today is Operational Weekend day: " + dayToday.DayOfWeek);
                            // Filter by Weekend or Work day
                            if (station.WeekendFlag)
                            {
                                logger.Trace("      Station has Weekend Flag.");
                                // Check if time is withing the time frame
                                startTime = Convert.ToDateTime(station.WeekendStartTime);
                                endTime = Convert.ToDateTime(station.WeenkendEndTime);
                                if (dayToday.TimeOfDay >= startTime.TimeOfDay && dayToday.TimeOfDay < endTime.TimeOfDay)
                                {
                                    logger.Trace("      Station within Operational Hours: " + startTime.TimeOfDay.ToString() + " -- " + endTime.TimeOfDay.ToString());
                                    // Analize File Conversion Station Watch Folder                                   
                                    //string[] folders = System.IO.Directory.GetDirectories(station.WatchFolder, "*", System.IO.SearchOption.TopDirectoryOnly);
                                    //DirectoryInfo dInfo = new DirectoryInfo(station.WatchFolder);
                                    currentAvailableBatches = station.MaxNumberBatches - stationFoldersCount;
                                    logger.Trace("      Current available Batches is Operational: " + currentAvailableBatches.ToString());
                                    if (currentAvailableBatches > 0)
                                    {
                                        if (currentAvailableBatches <= minFoldersCount)
                                        {
                                            if (currentAvailableBatches < minFoldersCount)
                                            {
                                                // Means this is the station that has more Batch capacity 
                                                minFoldersCount = currentAvailableBatches;
                                                fileConversionFolder = station.WatchFolder;
                                                logger.Trace("          File Conversion Folder candidate: " + fileConversionFolder);
                                            }
                                            else
                                            {
                                                // Means Batch Capavity is the same , so go by which one has the the oldest existing folder
                                                // The oldest existing folder translate to -- > more probability that the file conversion ends first
                                                // NOTE: We could also go by the number of files within the folder to make the selection more robust
                                                minFoldersCount = currentAvailableBatches;
                                                if (fileConversionFolder == "")
                                                {
                                                    // THis will happen the first time
                                                    fileConversionFolder = station.WatchFolder;
                                                    logger.Trace("          File Conversion Folder candidate: " + fileConversionFolder);
                                                }
                                                //else
                                                //{
                                                //    foreach (var folder in folders)
                                                //    {
                                                //        currentDT = System.IO.Directory.GetLastWriteTime(folder);
                                                //        if (currentDT < oldestFolder)
                                                //        {
                                                //            oldestFolder = currentDT;
                                                //            fileConversionFolder = station.WatchFolder;
                                                //        }
                                                //    }
                                                //}                                                
                                            }
                                        }
                                        else
                                        {
                                            // This section could be removed
                                            minFoldersCount = currentAvailableBatches;
                                            fileConversionFolder = station.WatchFolder;
                                            logger.Trace("          File Conversion Folder candidate: " + fileConversionFolder);
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            logger.Trace("      Today is Operational Workday day: " + dayToday.DayOfWeek);
                            // Check if today's day is a Work Day
                            if (station.WorkdayFlag)
                            {
                                logger.Trace("      Station has Workday Flag.");
                         
                                // Check if time is withing the time frame
                                startTime = Convert.ToDateTime(station.WorkdayStartTime);
                                endTime = Convert.ToDateTime(station.WorkdayEndTime);
                                if (dayToday.TimeOfDay >= startTime.TimeOfDay && dayToday.TimeOfDay < endTime.TimeOfDay)
                                {
                                    logger.Trace("      Station within Operational Hours: " + startTime.TimeOfDay.ToString() + " -- " + endTime.TimeOfDay.ToString());
                                    // Analize File Conversion Station Watch Folder
                                    //string[] folders = System.IO.Directory.GetDirectories(station.WatchFolder, "*", System.IO.SearchOption.TopDirectoryOnly);
                                    //DirectoryInfo dInfo = new DirectoryInfo(station.WatchFolder);
                                    currentAvailableBatches = station.MaxNumberBatches - stationFoldersCount;
                                    logger.Trace("      Current available Batches is Operational: " + currentAvailableBatches.ToString());
                                    if (currentAvailableBatches > 0)
                                    {
                                        if (currentAvailableBatches <= minFoldersCount)
                                        {
                                            if (currentAvailableBatches < minFoldersCount)
                                            {
                                                // Means this is the station that has more Batch capacity 
                                                minFoldersCount = currentAvailableBatches;
                                                fileConversionFolder = station.WatchFolder;
                                                logger.Trace("          File Conversion Folder candidate: " + fileConversionFolder);
                                            }
                                            else
                                            {
                                                // Means Batch Capavity is the same , so go by which one has the the oldest existing folder
                                                // The oldest existing folder translate to -- > more probability that the file conversion ends first
                                                // NOTE: We could also go by the number of files within the folder to make the selection more robust
                                                minFoldersCount = currentAvailableBatches;
                                                if (fileConversionFolder == "")
                                                {
                                                    // THis will happen the first time
                                                    fileConversionFolder = station.WatchFolder;
                                                    logger.Trace("          File Conversion Folder candidate: " + fileConversionFolder);
                                                }
                                                //else
                                                //{
                                                //    foreach (var folder in folders)
                                                //    {
                                                //        currentDT = System.IO.Directory.GetLastWriteTime(folder);
                                                //        if (currentDT < oldestFolder)
                                                //        {
                                                //            oldestFolder = currentDT;
                                                //            fileConversionFolder = station.WatchFolder;
                                                //        }
                                                //    }
                                                //}                                                
                                            }
                                        }
                                        else
                                        {
                                            // This section could be removed
                                            minFoldersCount = currentAvailableBatches;
                                            fileConversionFolder = station.WatchFolder;
                                            logger.Trace("          File Conversion Folder candidate: " + fileConversionFolder);
                                        }
                                    }
                                }
                            }
                        }                        
                    }
                }

                // Perform step #4
                resultStations.StringReturnValue = fileConversionFolder;
                resultStations.IntegerNumberReturnValue = currentAvailableBatches;
                resultStations.Message = "GetNextAvailableFileConversionStation transaction completed successfully. Number of records found: " + resultStations.RecordsCount;
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
            logger.Trace("Leaving GetNextAvailableFileConversionStation Method ...");
            return resultStations;
        }
    }
}
