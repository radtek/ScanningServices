using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using System.Linq.Dynamic.Core;

// Chec this out
// Dynamic LINQ (Part 1: Using the LINQ Dynamic Query Library)
// https://weblogs.asp.net/scottgu/dynamic-linq-part-1-using-the-linq-dynamic-query-library

namespace ScanningServices.Models
{

        /// <summary>
    /// 
    /// </summary>
    public class SQLFunctionsBatches
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get the next available work order for a given Job Type
        /// 
        /// </summary>
        /// <param name="jobType"></param>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric GetNextAvailableWorkOrder(string jobType)
        {
            int maxBatchesPerWorkOrder = 15; // By default the max number is set to 15 Batches
            int workOrderToReturn = 0;
            int batchCounter = 0;
            int currentWorkOrder = 0;

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()           
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetNextAvailableWorkOrder Method. ");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Get Job Type Max Baches for Work Order
                    var result_maxBatchesPerWorkOrder = DB.Jobs.FirstOrDefault(x => x.JobName == jobType);
                    if (result_maxBatchesPerWorkOrder == null)
                    {
                        logger.Trace("The Job type " + jobType + " could not be found.");
                        result.Message = "The Job type " + jobType + " could not be found.";
                    }
                    else
                    {
                        maxBatchesPerWorkOrder = Convert.ToInt32(result_maxBatchesPerWorkOrder.MaxBatchesPerWorkOrder);
                        if (maxBatchesPerWorkOrder == 0) maxBatchesPerWorkOrder = 15;
                        // Get Approved Batches for a given Job Type
                        var result_approved_batches = from b in DB.BatchControl
                                                      where b.JobType == jobType && b.StatusFlag == "Approved"
                                                      orderby b.LotNumber descending
                                                      select b.LotNumber;

                        if (result_approved_batches.Count() > 0)
                        {
                            // Count the number of Batches for the higher Work Order
                            batchCounter = 0;
                            currentWorkOrder = 0;
                            foreach (var x in result_approved_batches)
                            {
                                if (currentWorkOrder != x.Value)
                                {
                                    if (currentWorkOrder == 0)
                                    {
                                        currentWorkOrder = x.Value;
                                        batchCounter = batchCounter + 1;
                                    }
                                    else
                                    {
                                        // Means that we are now in a different Work Order
                                        break;
                                    }
                                }
                                else
                                {
                                    batchCounter = batchCounter + 1;
                                }
                            }

                            // Make a decission of what Work Order to Use
                            if (batchCounter < maxBatchesPerWorkOrder)
                            {
                                // We can use the higher Jot Tyoe Work Order in use
                                workOrderToReturn = currentWorkOrder;
                            }
                            else
                            {
                                // Means that we need to get the next available Work order in the system
                                // In order words, max work order in used + 1
                                var result_maxWorkOrder = DB.BatchControl.Max(x => x.LotNumber);
                                workOrderToReturn = result_maxWorkOrder.Value + 1;
                            }
                        }
                        else
                        {
                            // Means that we need to get the next available Work order in the system
                            // In order words, max work order in used + 1
                            var result_maxWorkOrder = DB.BatchControl.Max(x => x.LotNumber);
                            workOrderToReturn = result_maxWorkOrder.Value + 1;
                        }
                        logger.Trace("Transaction was executed successfuly.");
                        result.Message = "Transaction was executed successfuly.";
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                //return resultHosts;
            }
            result.IntegerNumberReturnValue = workOrderToReturn;
            logger.Trace("Leaving GetNextAvailableWorkOrder Method ...");
            return result;
        }

        /// <summary>
        /// Totals and Averages for various field in the BatchDoc Database table
        /// a filter is used to norrow down the search
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A summary of Totals and Averages</returns>
        static public GlobalVars.ResultBatchSummary GetBatchSummary(string filter)
        {
            List<GlobalVars.BatchSummary> batchesSummary = new List<GlobalVars.BatchSummary>();
            GlobalVars.ResultBatchSummary resultBatchSummary = new GlobalVars.ResultBatchSummary()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetBatchSummary Method. ");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    if (string.IsNullOrWhiteSpace(filter))
                    {
                        // If not filter, use the following condition to get all records from the Database table
                        filter = "BatchNumber <> \"\"";
                    }

                    // The group by (i=> 1) is used to enable to the summary for all the differents fields
                    var results = DB.BatchControl.Where(filter).GroupBy(i => 1).Select(nd => 
                                 new {
                                        TotalRecords = nd.Count(),
                                        TotalNumberOfDocs = nd.Sum(d => d.NumberOfDocuments) ,
                                        AvgNumOfDocs = nd.Average(d => d.NumberOfDocuments),
                                        TotalNumberOfScannedPages = nd.Sum(d => d.NumberOfScannedPages),
                                        AvgNumOfScannedPages = nd.Average(d => d.NumberOfScannedPages),
                                        TotalNumberOfPages = nd.Sum(d => d.NumberOfPages),
                                        AvgNumOfPages = nd.Average(d => d.NumberOfPages),
                                        TotalNumKestrokes = nd.Sum(d => d.KeysStrokes),
                                        AvgNumKeystrokes = nd.Average(d => d.KeysStrokes),
                                        AvgScanningTime = nd.Average(d => d.ScanningTime)
                                 });
                    
                    if (results.Count() >= 1)
                    {
                        resultBatchSummary.RecordsCount = results.Count();
                        foreach (var x in results)
                        {
                            GlobalVars.BatchSummary batchSummary = new GlobalVars.BatchSummary()
                            {
                                Count = x.TotalRecords,
                                TotalNumOfDocumnets = Convert.ToInt32(x.TotalNumberOfDocs),
                                AvgNumOfDocumnets = Math.Round(Convert.ToDouble(x.AvgNumOfDocs),2), 
                                TotalNumOfScannedPages = Convert.ToInt32(x.TotalNumberOfScannedPages),
                                AvgNumOfScannedPages = Math.Round(Convert.ToDouble(x.AvgNumOfScannedPages),2),
                                TotalNumOfPages = Convert.ToInt32(x.TotalNumberOfPages),
                                AvgNumOfPages = Math.Round(Convert.ToDouble(x.AvgNumOfPages),2),
                                TotalNumOfKeystrokes = Convert.ToInt32(x.TotalNumKestrokes),
                                AvgNumOfKeystrokes = Math.Round(Convert.ToDouble(x.AvgNumKeystrokes),2),
                                AvgScanningTime = Math.Round(Convert.ToDouble(x.AvgScanningTime), 2)
                            };
                            batchesSummary.Add(batchSummary);
                        }
                     }                    
                }
                resultBatchSummary.ReturnValue = batchesSummary;
                resultBatchSummary.Message = "GetBatchSummary transaction completed successfully. Number of records found: " + resultBatchSummary.RecordsCount;
                logger.Debug(resultBatchSummary.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultBatchSummary.ReturnCode = -2;
                resultBatchSummary.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchSummary.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetBatchSummary Method ...");
            return resultBatchSummary;
        }

        // UNDER CONSTRUCTION
        /// <summary>
        /// Average of a given field in a Batch List
        /// </summary>
        /// <param name="batches"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric GetBatchFieldAvg(List<GlobalVars.Batch> batches, string fieldName)
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
                logger.Trace("Entering into GetBatchFieldSum Method. ");
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetBatchFieldSum Method ...");
            return result;
        }

        /// <summary>
        /// Get the Transaction records based on a given Filter
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultBatches GetBatchesByStatus(string status)
        {
            List<GlobalVars.Batch> batches = new List<GlobalVars.Batch>();
            GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = batches,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                //Northwind db = new Northwind(connString);
                //db.Log = Console.Out;

                //var query =
                //    db.Customers.Where("City == @0 and Orders.Count >= @1", "London", 10).
                //    OrderBy("CompanyName").
                //    Select("New(CompanyName as Name, Phone)");

                logger.Trace("Entering into GetBatchesByStatus Method. Filter by Status : " + status);
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    //Smaple Code using Dynamic Query
                    //var query_aux = DB.Jobs.Where("JobName = @0 and JobId = @1", "Production Job  1",1);
                    //var query_aux = DB.Jobs.Where("JobName = \"Production Job  1\" AND JobId = 1");

                    //var query = DB.BatchControl.Where("StatusFlag = 'Ready to Scan");
                    var results = DB.BatchControl.Where(x => x.StatusFlag == status);
                
                    if (results.Count() >= 1)
                    {
                        resultBatches.RecordsCount = results.Count();
                        foreach (var x in results)
                        {
                            GlobalVars.Batch batch = new GlobalVars.Batch()
                            {
                                BatchNumber = x.BatchNumber,
                                LotNumber = Convert.ToInt32(x.LotNumber),
                                BlockNumber = Convert.ToInt32(x.BlockNumber),
                                NumberOfDocuments = Convert.ToInt32(x.NumberOfDocuments),
                                SubmittedBy = x.SubmittedBy,
                                SubmittedDate = Convert.ToDateTime(x.SubmittedDate),
                                StatusFlag = x.StatusFlag,
                                ScanOperator = x.ScanOperator,
                                ApprovedDate = Convert.ToDateTime(x.ApprovedDate),
                                RejectedTimes = Convert.ToInt32(x.RejectedTimes),
                                LastTimeRejected = Convert.ToDateTime(x.LastTimeRejected),
                                ApprovedBy = x.ApprovedBy,
                                NumberOfPages = Convert.ToInt32(x.NumberOfPages),
                                RejectedBy = x.RejectedBy,
                                RejectionReason = x.RejectionReason,
                                NumberOfScannedPages = Convert.ToInt32(x.NumberOfScannedPages),
                                DepName = x.DeptName,
                                ProjectName = x.ProjectName,
                                DocumentPath = x.DocumentPath,
                                ExportedDate = Convert.ToDateTime(x.ExportedDate),
                                ExportedBy = x.ExportedBy,
                                BatchSize = Convert.ToDouble(x.BatchSize),
                                ExportedTimes = Convert.ToInt32(x.ExportedTimes),
                                SubDepName = x.SubDeptName,
                                FileStatus = x.FileStatus,
                                ScannedDate = Convert.ToDateTime(x.ScannedDate),
                                QCBy = x.Qcby,
                                QCStation = x.Qcstation,
                                QCDate = Convert.ToDateTime(x.Qcdate),
                                OutputBy = x.OutputBy,
                                OutputDate = Convert.ToDateTime(x.OutputDate),
                                OutputStation = x.OutputStation,
                                ScanStation = x.ScanStation,
                                KodakStatus = x.KodakStatus,
                                JobType = x.JobType,
                                ModifiedStation = x.ModifiedStation,
                                ModifiedDate = Convert.ToDateTime(x.ModifiedDate),
                                ModifiedBy = x.ModifiedBy,
                                KodakErrorState = x.KodakErrorState,
                                Comments = x.Comments,
                                ScannedPagesReturned = Convert.ToInt32(x.ScannedPagesReturned),
                                CaptureTime = Convert.ToInt32(x.CaptureTime),
                                ScanningStageTime = Convert.ToInt32(x.ScanningStageTime),
                                QCStageTime = Convert.ToInt32(x.QcstageTime),
                                ScanningTime = Convert.ToInt32(x.ScanningTime),
                                QCTime = Convert.ToInt32(x.Qctime),
                                ScanningEndTime = Convert.ToDateTime(x.ScanningEndTime),
                                QCEndTime = Convert.ToDateTime(x.QcendTime),
                                QCStartTime = Convert.ToDateTime(x.QcstartTime),
                                TaskOrder = x.TaskOrder,
                                PrepUserName = x.PrepUserName,
                                PrepDate = Convert.ToDateTime(x.PrepDate),
                                QARFlag = x.Qarflag,
                                RecallTimes = Convert.ToInt32(x.RecallTimes),
                                RecallDate = Convert.ToDateTime(x.RecallDate),
                                RecallBy = x.RecallBy,
                                RecallReason = x.RecallReason,
                                Customer = x.Customer,
                                KeysStrokes = Convert.ToInt32(x.KeysStrokes),
                                BatchAlias = x.BatchAlias,
                                VFRUploadeDate = Convert.ToDateTime(x.VfruploadDate),
                                VFRUploadModiffiedDate = Convert.ToDateTime(x.VfruploadModifiedDate),
                                PrepTime = Convert.ToDouble(x.PrepTime),

                                InitialNumberOfDocuments = Convert.ToInt32(x.InitialNumberOfDocuments),
                                InitialNumberOfPages = Convert.ToInt32(x.InitialNumberOfPages),
                                InitialNumberOfScannedPages = Convert.ToInt32(x.InitialNumberOfScannedPages),
                                ImageCountGrayscale = Convert.ToInt32(x.ImageCountGrayscale),
                                ImageCountBlackWhite = Convert.ToInt32(x.ImageCountBlackWhite),
                                ImageCountGrayscaleBack = Convert.ToInt32(x.ImageCountGrayscaleBack),
                                ImageCountGrayscaleFront = Convert.ToInt32(x.ImageCountGrayscaleFront),
                                ImageCountBlackWhiteBack = Convert.ToInt32(x.ImageCountBlackWhiteBack),
                                ImageCountBlackWhiteFront = Convert.ToInt32(x.ImageCountBlackWhiteFront),
                                FrontsCaptured = Convert.ToInt32(x.FrontsCaptured),
                                FrontsRemoved = Convert.ToInt32(x.FrontsRemoved),
                                FrontsDeleted = Convert.ToInt32(x.FrontsDeleted),
                                FrontsRescanned = Convert.ToInt32(x.FrontsRescanned),
                                BacksCaptured = Convert.ToInt32(x.BacksCaptured),
                                BacksRemoved = Convert.ToInt32(x.BacksRemoved),
                                BacksDeleted = Convert.ToInt32(x.BacksDeleted),
                                BacksRescanned = Convert.ToInt32(x.BacksRescanned)
                            };
                            batches.Add(batch);
                        }
                    }
                }
                resultBatches.ReturnValue = batches;
                resultBatches.Message = "GetBatchesByStatus transaction completed successfully. Number of records found: " + resultBatches.RecordsCount;
                logger.Debug(resultBatches.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultBatches.ReturnCode = -2;
                resultBatches.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatches.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetBatchesByStatus Method ...");
            return resultBatches;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultBatchTracking GetBatchTrackingInformation(string filter, string orderBy)
        {
            List<GlobalVars.BatchTracking> batchTracking = new List<GlobalVars.BatchTracking>();
            GlobalVars.ResultBatchTracking resultBatchTracking = new GlobalVars.ResultBatchTracking()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = batchTracking,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetBatchTrackingInformation Method. Filter by: " + filter);
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    if (string.IsNullOrWhiteSpace(filter))
                    {
                        // Make the filter to retrieve all records in the database
                        filter = "ID >= 0";
                    }
                    if (string.IsNullOrWhiteSpace(orderBy))
                    {
                        // Make the order By  Date ascending by default
                        orderBy = "Date ascending";
                    }
                    

                    var results = DB.BatchTracking.Where(filter).OrderBy(orderBy);
                    if (results.Count() >= 1)
                    {
                        resultBatchTracking.RecordsCount = results.Count();
                        foreach (var x in results)
                        {
                            GlobalVars.BatchTracking record = new GlobalVars.BatchTracking()
                            {
                                ID = x.Id,
                                BatchNumber = x.BatchNumber,
                                Date = Convert.ToDateTime(x.Date),
                                InitialStatus = x.InitialStatus,                     
                                FinalStatus = x.FinalStatus,
                                OperatorName = x.OperatorId,
                                StationName = x.StationId,
                                Event = x.Event
                            };
                            batchTracking.Add(record);
                        }
                    }
                }
                resultBatchTracking.ReturnValue = batchTracking;
                resultBatchTracking.Message = "GetBatchTrackingInformation transaction completed successfully. Number of records found: " + resultBatchTracking.RecordsCount;
                logger.Debug(resultBatchTracking.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultBatchTracking.ReturnCode = -2;
                resultBatchTracking.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchTracking.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetBatchTrackingInformation Method ...");
            return resultBatchTracking;
        }


        /// <summary>
        ///  This method is used to generate Daily report by Operator
        ///  A time Frame is required to execute this Method
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultBatchTrackingExtended GetInformationForDailyOperationReport(DateTime dateFrom , DateTime dateTo)
        {
            string currentOperator = "";
            int numberOfOperators = 0;
            string currentBatchName = "";
            int numberOfBatches = 0;
            DateTime startTime;
            DateTime endTime;
            Boolean WasBatchOuptutToday;

            GlobalVars.ResultBatchDocs resultBatchDocs = new GlobalVars.ResultBatchDocs();
            GlobalVars.BatchDocs batchDocs = new GlobalVars.BatchDocs();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
            List<GlobalVars.JobExtended> jobs = new List<GlobalVars.JobExtended>();
            List<GlobalVars.BatchTrackingExtended> batchTracking = new List<GlobalVars.BatchTrackingExtended>();
            GlobalVars.ResultBatchTrackingExtended resultBatchTracking = new GlobalVars.ResultBatchTrackingExtended()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = batchTracking,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {                
                resultJobs = SQLFunctionsJobs.GetJobs();
                jobs = resultJobs.ReturnValue;

                logger.Trace("Entering into GetInformationForDailyOperationReport Method. From  " + dateFrom.ToString() + " to " + dateTo.ToString());
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    //var results = from x in
                    //                (from events in DB.BatchTracking
                    //                 where (events.Date >= dateFrom && events.Date <= dateTo) && (events.Event == "QC Started" || events.Event == "QC Completed")
                    //                 select new { events.OperatorId, events.BatchNumber, events.Date, events.Event }
                    //                )
                    //             orderby new { x.OperatorId, x.BatchNumber, x.Date, x.Event } ascending
                    //             group x by new { x.OperatorId, x.BatchNumber, x.Date, x.Event } into g                             
                    //             select new { g.Key.OperatorId, g.Key.BatchNumber, g.Key.Date, g.Key.Event };

                    var results = from x in
                                    (from events in DB.BatchTracking
                                     join b in DB.BatchControl on events.BatchNumber equals b.BatchNumber
                                     where (events.Date >= dateFrom && events.Date <= dateTo) && (events.Event == "QC Started" || events.Event == "QC Completed")
                                     select new { events.OperatorId, events.BatchNumber, events.Date, events.Event , b.JobType}
                                    )
                                  orderby new { x.OperatorId, x.BatchNumber, x.Date, x.Event , x.JobType} ascending
                                  group x by new { x.OperatorId, x.BatchNumber, x.Date, x.Event , x.JobType} into g
                                  select new { g.Key.OperatorId, g.Key.BatchNumber, g.Key.Date, g.Key.Event , g.Key.JobType};

                    if (results.Count() >= 1)
                    {
                        resultBatchTracking.RecordsCount = results.Count();
                        startTime = DateTime.MinValue;
                        foreach (var x in results)
                        {
                            GlobalVars.BatchTrackingExtended record = new GlobalVars.BatchTrackingExtended();

                            if (x.OperatorId != currentOperator)
                            {
                                logger.Trace("      Operator: " + x.OperatorId);
                                logger.Trace("      First ocurrance of this Operator.");
                                numberOfOperators = numberOfOperators + 1;
                                currentOperator = x.OperatorId;
                                currentBatchName = "";
                                numberOfBatches = -1;
                                record.OperatorName = x.OperatorId;
                            }
                            logger.Trace("      Analizing Batch: " + x.BatchNumber + " with status: " + x.Event);
                            if (currentBatchName != x.BatchNumber && x.Event == "QC Started")
                            {
                                currentBatchName = x.BatchNumber;
                                startTime = x.Date;
                            }
                            else
                            {
                                if (x.BatchNumber == currentBatchName && x.Event == "QC Completed")
                                {
                                    logger.Trace("      QC Completed Status found");
                                    endTime = x.Date;

                                    // Find the filter to be used to determine if a Batch was output today
                                    WasBatchOuptutToday = false;
                                    foreach (GlobalVars.JobExtended job in jobs)
                                    {
                                        if (x.JobType == job.JobName)
                                        {
                                            if (job.PostValidationEnableFlag)
                                            {
                                                var results_aux1 = from events in DB.BatchTracking
                                                                   where (events.Date >= dateFrom && events.Date <= dateTo) && (events.Event == "Validation Passed" || events.Event == "Validation Failed")
                                                                   select new { events.OperatorId, events.BatchNumber, events.Date, events.Event };
                                                if (results_aux1.Count() > 0)
                                                {
                                                    WasBatchOuptutToday = true;
                                                }
                                            }
                                            else
                                            {
                                                if (job.FileConversionEnableFlag)
                                                {
                                                    var results_aux2 = from events in DB.BatchTracking
                                                                       where (events.Date >= dateFrom && events.Date <= dateTo) && (events.Event == "Batch Sent for File Conversion")
                                                                       select new { events.OperatorId, events.BatchNumber, events.Date, events.Event };
                                                    if (results_aux2.Count() > 0)
                                                    {
                                                        WasBatchOuptutToday = true;
                                                    }
                                                }
                                                else
                                                {
                                                    var results_aux3 = from events in DB.BatchTracking
                                                                       where (events.Date >= dateFrom && events.Date <= dateTo) && (events.Event == "Batch Delivered to Resting Location")
                                                                       select new { events.OperatorId, events.BatchNumber, events.Date, events.Event };
                                                    if (results_aux3.Count() > 0)
                                                    {
                                                        WasBatchOuptutToday = true;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    if (WasBatchOuptutToday)
                                    {
                                        numberOfBatches = numberOfBatches + 1;
                                        record.BatchNumber = currentBatchName;
                                        record.totalQcMinutes = endTime.Subtract(startTime).TotalMinutes;
                                        record.JobType = x.JobType;
                                        resultBatchDocs = SQLFunctionsBatches.GetBatchDocInformation("BatchName = \"" + record.BatchNumber + "\"", "");
                                        record.totalDocuments = resultBatchDocs.RecordsCount;
                                        batchTracking.Add(record);
                                    }                                   
                                }
                            }
                        }
                    }
                }
                resultBatchTracking.ReturnValue = batchTracking;
                resultBatchTracking.Message = "GetInformationForDailyOperationReport transaction completed successfully. Number of records found: " + resultBatchTracking.RecordsCount;
                logger.Debug(resultBatchTracking.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultBatchTracking.ReturnCode = -2;
                resultBatchTracking.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchTracking.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetInformationForDailyOperationReport Method ...");
            return resultBatchTracking;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultBatchDocs GetBatchDocInformation(string filter, string orderBy)
        {
            List<GlobalVars.BatchDocs> batchDocs = new List<GlobalVars.BatchDocs>();
            GlobalVars.ResultBatchDocs resultBatchDocs = new GlobalVars.ResultBatchDocs()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = batchDocs,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
           
            try
            {
               // batchDocs[0].DocMetadat[0].Scope = GlobalVars.MetadataSope.BATCH;
                logger.Trace("Entering into GetBatchDocumentsInformation Method. Filter by: " + filter);
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    if (string.IsNullOrWhiteSpace(filter))
                    {
                        // If not filter, use the following condition to get all records from the Database table
                        filter = "BatchName <> \"\"";
                    }
                    if (string.IsNullOrWhiteSpace(orderBy))
                    {
                        // If not sortby, use the following criteria for sorting
                        orderBy = "BatchName ascending";
                    }

                    var results = DB.BatchDocs.Where(filter).OrderBy(orderBy);

                    if (results.Count() >= 1)
                    {
                        resultBatchDocs.RecordsCount = results.Count();
                        foreach (var x in results)
                        {
                            GlobalVars.BatchDocs batchDoc = new GlobalVars.BatchDocs()
                            {
                                DocumentLocation = (x.DocumentLocation ?? "").Trim(),
                                BatchName = (x.BatchName ?? "").Trim(),
                                DocumentID = Convert.ToInt32(x.DocumentId),
                                ImageCountInDocument = x.ImageCountInDocument,
                                PageCountInDocument = x.PageCountInDocument,
                                BatchLocation = (x.BatchLocation ?? "").Trim(),
                                BatchSize = Convert.ToDouble(x.BatchSize),
                                CreateDateAndTime = Convert.ToDateTime(x.CreatedDateAndTime),
                                CreateStationID = (x.CreatedStationId ?? "").Trim(),
                                CreateStationName = (x.CreatedStationName ?? "").Trim(),
                                CreateUserID = (x.CreatedUserId ?? "").Trim(),
                                OutputDateAndTime = Convert.ToDateTime(x.OutputDateAndTime),
                                OutoutStationID = (x.OutputStationId ?? "").Trim(),
                                OutputStationName = (x.OutputStationName ?? "").Trim(),
                                OutputUserID = (x.OutputUserId ?? "").Trim(),
                                LastModifiedDateAndTime = Convert.ToDateTime(x.LastModifiedDateAndTime),
                                LastModifiedStationID = (x.LastModifiedStationId ?? "").Trim(),
                                LastModifiedStationName = (x.LastModifiedStationName ?? "").Trim(),
                                LastModifiedUserID = (x.LastModifiedUserId ?? "").Trim(),
                                StartingDocumentID = (x.StartingDocumentId ?? "").Trim(),
                                FirstDocumentID = (x.FirstDocumentId ?? "").Trim(),
                                LastDocumentID = (x.LastDocumentId ?? "").Trim(),
                                DocumentCountInBatch = x.DocumentCountInBatch,
                                PageCountInBatch = x.PageCountInBatch ,
                                ImageCountInBatch = x.ImageCountInBatch,
                                BlackAndWhiteImageCount = x.BlackAndWhiteImageCount,
                                ColorImageCount = x.ColorImageCount,
                                GrayscaleImageCount = x.GrayscaleImageCount,
                                ImageCaptureFront = x.ImagesCapturedFront,
                                ImagesRescannedFront = x.ImagesRescannedFront,
                                ImagesRemovedForBlankBack = x.ImagesRemovedForBlankBack,
                                ImagesDeletedBack = x.ImageDeletedBack,
                                DocumentSequenceNumber = (x.DocumentSequenceNumber ?? "").Trim(),
                                DocumentFileNameWithFullPath = (x.DocumentFilenameWithFullPath ?? "").Trim(),
                                DocumentFileName = (x.DocumentFilename ?? "").Trim(),
                                ImagesCaptureBack = x.ImagesCapturedBack,
                                ImagesDeletedFront = x.ImageDeletedFront,
                                ImagesRemovedForBlankFront = x.ImagesRemovedForBlankFront,
                                ImagesRescannedBack = x.ImagesRescannedBack,
                                DocumentSize = (x.DocumentSize ?? "").Trim(),                                
                                CustonmerField1 = (x.CustomerField1 ?? "").Trim(),
                                CustonmerField2 = (x.CustomerField2 ?? "").Trim(),
                                CustonmerField3 = (x.CustomerField3 ?? "").Trim(),
                                CustonmerField4 = (x.CustomerField4 ?? "").Trim(),
                                CustonmerField5 = (x.CustomerField5 ?? "").Trim(),
                                CustonmerField6 = (x.CustomerField6 ?? "").Trim(),
                                CustonmerField7 = (x.CustomerField7 ?? "").Trim(),
                                CustonmerField8 = (x.CustomerField8 ?? "").Trim(),
                                CustonmerField9 = (x.CustomerField9 ?? "").Trim(),
                                CustonmerField10 = (x.CustomerField10 ?? "").Trim(),
                                CustonmerField11 = (x.CustomerField11 ?? "").Trim(),
                                CustonmerField12 = (x.CustomerField12 ?? "").Trim(),
                                CustonmerField13 = (x.CustomerField13 ?? "").Trim(),
                                CustonmerField14 = (x.CustomerField14 ?? "").Trim(),
                                CustonmerField15 = (x.CustomerField15 ?? "").Trim(),
                                Customer = (x.Customer ?? "").Trim(),
                                keystrokes = Convert.ToInt32(x.Keystrokes)                                
                            };

                            List<GlobalVars.Metadata> docMetadata = new List<GlobalVars.Metadata>();
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField1))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField1));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField2))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField2));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField3))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField3));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField4))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField4));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField5))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField5));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField6))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField6));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField7))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField7));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField8))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField8));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField9))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField9));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField10))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField10));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField11))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField11));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField12))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField12));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField13))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField13));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField14))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField14));
                            if (!string.IsNullOrEmpty(batchDoc.CustonmerField15))
                                docMetadata.Add(BuildMetadata(batchDoc.CustonmerField15));

                            batchDoc.DocMetadata = docMetadata;
                            batchDocs.Add(batchDoc);
                        }
                    }
                }
                resultBatchDocs.ReturnValue = batchDocs;
                resultBatchDocs.Message = "GetBatchDocumentsInformation transaction completed successfully. Number of records found: " + resultBatchDocs.RecordsCount;
                logger.Debug(resultBatchDocs.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultBatchDocs.ReturnCode = -2;
                resultBatchDocs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchDocs.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetBatchDocumentsInformation Method ...");
            return resultBatchDocs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInformation"></param>
        /// <returns></returns>
        static public GlobalVars.Metadata BuildMetadata(string fieldInformation)
        {
            var tokens = fieldInformation.Split('|');
            GlobalVars.Metadata metadata = new GlobalVars.Metadata();
            metadata.FieldName = tokens[0];
            metadata.FieldValue = tokens[2];
            if (tokens[1] == "D")
                metadata.Scope = GlobalVars.MetadataScope.Document;
            else
                metadata.Scope = GlobalVars.MetadataScope.Batch;
            return metadata;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultBatches GetBatchesInformation(string filter, string orderBy)
        {
            List<GlobalVars.Batch> batches = new List<GlobalVars.Batch>();
            GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = batches,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                //Northwind db = new Northwind(connString);
                //db.Log = Console.Out;

                //var query =
                //    db.Customers.Where("City == @0 and Orders.Count >= @1", "London", 10).
                //    OrderBy("CompanyName").
                //    Select("New(CompanyName as Name, Phone)");
                //var results = DB.BatchControl.Where(filter).OrderBy("RIGHT(REPLICATE('0', 8 - LEN([Batchnumber])) + [Batchnumber], 20) descending");
                //sample code using sortorder
                //    products.OrderBy("Category.CategoryName, UnitPrice descending");
                // ascending by default

                logger.Trace("Entering into GetBatchesInformation Method. Filter by: " + filter);
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    if (string.IsNullOrWhiteSpace(filter))
                    {
                        // If not filter, use the following condition to get all records from the Database table
                        filter = "BatchNumber <> \"\"";
                    }
                    if (string.IsNullOrWhiteSpace(orderBy))
                    {
                        // If not sortby, use the following criteria for sorting
                        orderBy = "BatchNumber ascending";
                    }

                    var results = DB.BatchControl.Where(filter).OrderBy(orderBy);
                    
                    if (results.Count() >= 1)
                    {
                        resultBatches.RecordsCount = results.Count();
                        foreach (var x in results)
                        {
                            GlobalVars.Batch batch = new GlobalVars.Batch()
                            {
                                BatchNumber = (x.BatchNumber ?? "").Trim(),
                                LotNumber = Convert.ToInt32(x.LotNumber),
                                BlockNumber = Convert.ToInt32(x.BlockNumber),
                                NumberOfDocuments = Convert.ToInt32(x.NumberOfDocuments),
                                SubmittedBy = (x.SubmittedBy ?? "").Trim(),
                                SubmittedDate = Convert.ToDateTime(x.SubmittedDate),
                                StatusFlag = (x.StatusFlag ?? "").Trim(),
                                ScanOperator = (x.ScanOperator ?? "").Trim(),
                                ApprovedDate = Convert.ToDateTime(x.ApprovedDate),
                                RejectedTimes = Convert.ToInt32(x.RejectedTimes),
                                LastTimeRejected = Convert.ToDateTime(x.LastTimeRejected),
                                ApprovedBy = (x.ApprovedBy ?? "").Trim(),
                                NumberOfPages = Convert.ToInt32(x.NumberOfPages),
                                RejectedBy = (x.RejectedBy ?? "").Trim(),
                                RejectionReason = (x.RejectionReason ?? "").Trim(),
                                NumberOfScannedPages = Convert.ToInt32(x.NumberOfScannedPages),
                                DepName = (x.DeptName ?? "").Trim(),
                                ProjectName = (x.ProjectName ?? "").Trim(),
                                DocumentPath = (x.DocumentPath ?? "").Trim(),
                                ExportedDate = Convert.ToDateTime(x.ExportedDate),
                                ExportedBy = (x.ExportedBy ?? "").Trim(),
                                BatchSize = Convert.ToDouble(x.BatchSize),
                                ExportedTimes = Convert.ToInt32(x.ExportedTimes),
                                SubDepName = (x.SubDeptName ?? "").Trim(),
                                FileStatus = (x.FileStatus ?? "").Trim(),
                                ScannedDate = Convert.ToDateTime(x.ScannedDate),
                                QCBy = (x.Qcby ?? "").Trim(),
                                QCStation = (x.Qcstation ?? "").Trim(),
                                QCDate = Convert.ToDateTime(x.Qcdate),
                                OutputBy = (x.OutputBy ?? "").Trim(),
                                OutputDate = Convert.ToDateTime(x.OutputDate),
                                OutputStation = (x.OutputStation ?? "").Trim(),
                                ScanStation = (x.ScanStation ?? "").Trim(),
                                KodakStatus = (x.KodakStatus ?? "").Trim(),
                                JobType = (x.JobType ?? "").Trim(),
                                ModifiedStation = (x.ModifiedStation ?? "").Trim(),
                                ModifiedDate = Convert.ToDateTime(x.ModifiedDate),
                                ModifiedBy = (x.ModifiedBy ?? "").Trim(),
                                KodakErrorState = (x.KodakErrorState ?? "").Trim(),
                                Comments = (x.Comments ?? "").Trim(),
                                ScannedPagesReturned = Convert.ToInt32(x.ScannedPagesReturned),
                                CaptureTime = Convert.ToInt32(x.CaptureTime),
                                ScanningStageTime = Convert.ToInt32(x.ScanningStageTime),
                                QCStageTime = Convert.ToInt32(x.QcstageTime),
                                ScanningTime = Convert.ToInt32(x.ScanningTime),
                                QCTime = Convert.ToInt32(x.Qctime),
                                ScanningEndTime = Convert.ToDateTime(x.ScanningEndTime),
                                QCEndTime = Convert.ToDateTime(x.QcendTime),
                                QCStartTime = Convert.ToDateTime(x.QcstartTime),
                                TaskOrder = (x.TaskOrder ?? "").Trim(),
                                PrepUserName = (x.PrepUserName ?? "").Trim(),
                                PrepDate = Convert.ToDateTime(x.PrepDate),
                                QARFlag = (x.Qarflag ?? "").Trim(),
                                RecallTimes = Convert.ToInt32(x.RecallTimes),
                                RecallDate = Convert.ToDateTime(x.RecallDate),
                                RecallBy = (x.RecallBy ?? "").Trim(),
                                RecallReason = (x.RecallReason ?? "").Trim(),
                                Customer = (x.Customer ?? "").Trim(),
                                KeysStrokes = Convert.ToInt32(x.KeysStrokes),
                                BatchAlias = (x.BatchAlias ?? "").Trim(),
                                VFRUploadeDate = Convert.ToDateTime(x.VfruploadDate),
                                VFRUploadModiffiedDate = Convert.ToDateTime(x.VfruploadModifiedDate),
                                PrepTime = Convert.ToDouble(x.PrepTime),
                                InitialNumberOfDocuments = Convert.ToInt32(x.InitialNumberOfDocuments),
                                InitialNumberOfPages = Convert.ToInt32(x.InitialNumberOfPages),
                                InitialNumberOfScannedPages = Convert.ToInt32(x.InitialNumberOfScannedPages),
                                ImageCountGrayscale = Convert.ToInt32(x.ImageCountGrayscale),
                                ImageCountBlackWhite = Convert.ToInt32(x.ImageCountBlackWhite),
                                ImageCountGrayscaleBack = Convert.ToInt32(x.ImageCountGrayscaleBack),
                                ImageCountGrayscaleFront = Convert.ToInt32(x.ImageCountGrayscaleFront),
                                ImageCountBlackWhiteBack = Convert.ToInt32(x.ImageCountBlackWhiteBack),
                                ImageCountBlackWhiteFront = Convert.ToInt32(x.ImageCountBlackWhiteFront),
                                FrontsCaptured = Convert.ToInt32(x.FrontsCaptured),
                                FrontsRemoved = Convert.ToInt32(x.FrontsRemoved),
                                FrontsDeleted = Convert.ToInt32(x.FrontsDeleted),
                                FrontsRescanned = Convert.ToInt32(x.FrontsRescanned),
                                BacksCaptured = Convert.ToInt32(x.BacksCaptured),
                                BacksRemoved = Convert.ToInt32(x.BacksRemoved),
                                BacksDeleted = Convert.ToInt32(x.BacksDeleted),
                                BacksRescanned = Convert.ToInt32(x.BacksRescanned),
                                VFRStatusFlag = (x.StatusFlag ?? "").Trim(),
                                VFRUploadRequestedDate = Convert.ToDateTime(x.VfruploadRequestedDate),
                                VFRUploadCompletedDate = Convert.ToDateTime(x.VfruploadCompletedDate),
                                PageSizesCount = (x.PageSizesCount ?? "").Trim()
                            };
                            logger.Trace("      Batch NUmber:" + batch.BatchNumber);
                            logger.Trace("          Page Size:" + batch.PageSizesCount);
                            logger.Trace("          Number of Documents:" + batch.NumberOfDocuments);
                            batches.Add(batch);
                        }
                    }
                }
                resultBatches.ReturnValue = batches;
                resultBatches.Message = "GetBatchesInformation transaction completed successfully. Number of records found: " + resultBatches.RecordsCount;
                logger.Debug(resultBatches.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultBatches.ReturnCode = -2;
                resultBatches.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatches.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetBatchesInformation Method ...");
            return resultBatches;
        }

        /// <summary>
        /// Used for a New Batch Registration
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric BatchRegistration(GlobalVars.Batch batch)
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
                logger.Trace("Entering into BatchRegistration Method ...");

                // Check if Batch Exist
                GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                resultBatches =  GetBatchesInformation("BatchNumber = \"" + batch.BatchNumber.Trim() + "\" OR BatchAlias = \"" + batch.BatchAlias.Trim() + "\"", "");
                if (resultBatches.RecordsCount == 0)
                {
                    // Create new Batch
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        BatchControl New_Record = new BatchControl();
                        New_Record.BatchNumber = batch.BatchNumber;
                        New_Record.BatchAlias = batch.BatchAlias;
                        New_Record.SubmittedBy = batch.SubmittedBy;
                        New_Record.SubmittedDate = batch.SubmittedDate;
                        New_Record.StatusFlag = batch.StatusFlag;
                        New_Record.DeptName = batch.DepName;
                        New_Record.ProjectName = batch.ProjectName;
                        New_Record.Customer = batch.Customer;
                        New_Record.JobType = batch.JobType;

                        DB.BatchControl.Add(New_Record);
                        DB.SaveChanges();
                    }
                    result.Message = "BatchRegistration transaction completed successfully. One Record added.";
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Batch Number  " + batch.BatchNumber + " or Batch Alias " + batch.BatchAlias + " already exist. BatchRegistration transaction ignore.";
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
            logger.Trace("Leaving BatchRegistration Method ...");
            return result;
        }
        
        /// <summary>
        /// Add Batch Document Record to Database
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewBatchDocument(GlobalVars.BatchDocs batchDocument)
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
                logger.Trace("Entering into NewBatchDocument Method ...");
                
                // Create new Batch
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    BatchDocs New_Record = new BatchDocs();
                    New_Record.BatchName = batchDocument.BatchName;
                    New_Record.BatchLocation = batchDocument.BatchLocation;
                    New_Record.BatchSize = batchDocument.BatchSize;
                    New_Record.BlackAndWhiteImageCount = batchDocument.BlackAndWhiteImageCount;
                    New_Record.ColorImageCount = batchDocument.ColorImageCount;
                    New_Record.CreatedDateAndTime = batchDocument.CreateDateAndTime;
                    New_Record.CreatedStationId = batchDocument.CreateStationID;
                    New_Record.CreatedStationName = batchDocument.CreateStationID;
                    New_Record.CreatedUserId = batchDocument.CreateUserID;
                    New_Record.Customer = batchDocument.Customer;
                    New_Record.CustomerField1 = batchDocument.CustonmerField1;
                    New_Record.CustomerField2 = batchDocument.CustonmerField2;
                    New_Record.CustomerField3 = batchDocument.CustonmerField3;
                    New_Record.CustomerField4 = batchDocument.CustonmerField4;
                    New_Record.CustomerField5 = batchDocument.CustonmerField5;
                    New_Record.CustomerField6 = batchDocument.CustonmerField6;
                    New_Record.CustomerField7 = batchDocument.CustonmerField7;
                    New_Record.CustomerField8 = batchDocument.CustonmerField8;
                    New_Record.CustomerField9 = batchDocument.CustonmerField9;
                    New_Record.CustomerField10 = batchDocument.CustonmerField10;
                    New_Record.CustomerField11 = batchDocument.CustonmerField11;
                    New_Record.CustomerField12 = batchDocument.CustonmerField12;
                    New_Record.CustomerField13 = batchDocument.CustonmerField13;
                    New_Record.CustomerField14 = batchDocument.CustonmerField14;
                    New_Record.CustomerField15 = batchDocument.CustonmerField15;                       
                    New_Record.DocumentCountInBatch = batchDocument.DocumentCountInBatch;
                    New_Record.DocumentFilename = batchDocument.DocumentFileName;
                    New_Record.DocumentFilenameWithFullPath = batchDocument.DocumentFileNameWithFullPath;
                    New_Record.DocumentId = batchDocument.DocumentID.ToString();
                    New_Record.DocumentLocation = batchDocument.DocumentLocation;
                    New_Record.DocumentSequenceNumber = batchDocument.DocumentSequenceNumber;
                    New_Record.DocumentSize = batchDocument.DocumentSize;
                    New_Record.FirstDocumentId = batchDocument.FirstDocumentID;
                    New_Record.GrayscaleImageCount = batchDocument.GrayscaleImageCount;
                    New_Record.ImageCountInBatch = batchDocument.ImageCountInBatch;
                    New_Record.ImageCountInDocument = batchDocument.ImageCountInDocument;
                    New_Record.ImageDeletedBack = batchDocument.ImagesDeletedBack;
                    New_Record.ImageDeletedFront = batchDocument.ImagesDeletedFront;
                    New_Record.ImagesCapturedBack = batchDocument.ImagesCaptureBack;
                    New_Record.ImagesCapturedFront = batchDocument.ImageCaptureFront;
                    New_Record.ImagesRemovedForBlankBack = batchDocument.ImagesRemovedForBlankBack;
                    New_Record.ImagesRemovedForBlankFront = batchDocument.ImagesRemovedForBlankFront;
                    New_Record.ImagesRescannedBack = batchDocument.ImagesRescannedBack;
                    New_Record.ImagesRescannedFront = batchDocument.ImagesRescannedFront;
                    New_Record.Keystrokes = batchDocument.keystrokes;
                    New_Record.LastDocumentId = batchDocument.LastDocumentID;
                    New_Record.LastModifiedDateAndTime = batchDocument.LastModifiedDateAndTime;
                    New_Record.LastModifiedStationId = batchDocument.LastModifiedStationID;
                    New_Record.LastModifiedStationName = batchDocument.LastModifiedStationName;
                    New_Record.LastModifiedUserId = batchDocument.LastModifiedUserID;
                    New_Record.OutputDateAndTime = batchDocument.OutputDateAndTime;
                    New_Record.OutputStationId = batchDocument.OutoutStationID;
                    New_Record.OutputStationName = batchDocument.OutputStationName;
                    New_Record.OutputUserId = batchDocument.OutputUserID;
                    New_Record.PageCountInBatch = batchDocument.PageCountInBatch;
                    New_Record.PageCountInDocument = batchDocument.PageCountInDocument;
                    New_Record.StartingDocumentId = batchDocument.StartingDocumentID;

                    DB.BatchDocs.Add(New_Record);
                    DB.SaveChanges();
                }
                result.Message = "NewBatchDocument transaction completed successfully. One Record added.";
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
            logger.Trace("Leaving NewBatchDocument Method ...");
            return result;
        }


        /// <summary>
        /// Verify if given Batch, using Batch Number or Alias Name, exist in the Database
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric BatchExist(string batchIdentifier)
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
                logger.Trace("Entering into BatchExist Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.BatchControl.Where(x => x.BatchNumber == batchIdentifier || x.BatchAlias == batchIdentifier);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Batch " + batchIdentifier + " exist in the Database.";
                    }
                    else
                    {
                        result.Message = "Batch " + batchIdentifier + " doest not exist in the Database.";
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
            logger.Trace("Leaving BatchExist Method ...");
            return result;
        }

        /// <summary>
        /// Update Batch Information 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateBatchInformation(GlobalVars.Batch batch)
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
                logger.Trace("Entering into UpdateBatchInformation Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    BatchControl Matching_Result = null;
                    // Job Names must be unique in the Database. The Name could be change but it must be unique
                    if (!string.IsNullOrEmpty(batch.BatchNumber))
                    {
                        // Search by Batch Number
                        Matching_Result = DB.BatchControl.FirstOrDefault(x => x.BatchNumber == batch.BatchNumber);
                    }
                    
                    if (Matching_Result != null)
                    {
                        Matching_Result.ApprovedBy = batch.ApprovedBy;
                        if (batch.ApprovedDate == DateTime.MinValue)
                             Matching_Result.ApprovedDate = null;
                        else
                            Matching_Result.ApprovedDate = batch.ApprovedDate;
                        Matching_Result.BacksCaptured = batch.BacksCaptured;
                        Matching_Result.BacksDeleted = batch.BacksDeleted;
                        Matching_Result.BacksRemoved = batch.BacksRemoved;
                        Matching_Result.BacksRescanned = batch.BacksRescanned;
                        //Matching_Result.BatchAlias 
                        //Matching_Result.BatchNumber
                        Matching_Result.BatchSize = (float)batch.BatchSize;
                        Matching_Result.BlockNumber = batch.BlockNumber;
                        Matching_Result.CaptureTime = batch.CaptureTime;
                        Matching_Result.Comments = batch.Comments;
                        Matching_Result.DeptName = batch.DepName;
                        Matching_Result.DocumentPath = batch.DocumentPath;
                        Matching_Result.ExportedBy = batch.ExportedBy;
                        if (batch.ExportedDate == DateTime.MinValue)
                            Matching_Result.ExportedDate = null;
                        else
                            Matching_Result.ExportedDate = batch.ExportedDate;
                        Matching_Result.ExportedTimes = batch.ExportedTimes;
                        Matching_Result.FileStatus = batch.FileStatus;
                        Matching_Result.FrontsCaptured = batch.FrontsCaptured;
                        Matching_Result.FrontsDeleted = batch.FrontsDeleted;
                        Matching_Result.FrontsRemoved = batch.FrontsRemoved;
                        Matching_Result.FrontsRescanned = batch.FrontsRescanned;
                        Matching_Result.ImageCountBlackWhite = batch.ImageCountBlackWhite;
                        Matching_Result.ImageCountBlackWhiteBack = batch.ImageCountBlackWhiteBack;
                        Matching_Result.ImageCountBlackWhiteFront = batch.ImageCountBlackWhiteFront;
                        Matching_Result.ImageCountGrayscale = batch.ImageCountGrayscale;
                        Matching_Result.ImageCountGrayscaleBack = batch.ImageCountGrayscaleBack;
                        Matching_Result.ImageCountGrayscaleFront = batch.ImageCountGrayscaleFront;
                        Matching_Result.InitialNumberOfDocuments = batch.InitialNumberOfDocuments;
                        Matching_Result.InitialNumberOfPages = batch.InitialNumberOfPages;
                        Matching_Result.InitialNumberOfScannedPages = batch.InitialNumberOfScannedPages;
                        Matching_Result.JobType = batch.JobType;
                        Matching_Result.KeysStrokes = batch.KeysStrokes;
                        Matching_Result.KodakErrorState = batch.KodakErrorState;
                        Matching_Result.KodakStatus = batch.KodakStatus;
                        if (batch.LastTimeRejected == DateTime.MinValue)
                            Matching_Result.LastTimeRejected = null;
                        else
                            Matching_Result.LastTimeRejected = batch.LastTimeRejected;
                        Matching_Result.LotNumber = batch.LotNumber;
                        Matching_Result.ModifiedBy = batch.ModifiedBy;
                        if (batch.ModifiedDate == DateTime.MinValue)
                            Matching_Result.ModifiedDate = null;
                        else
                            Matching_Result.ModifiedDate = batch.ModifiedDate;
                        Matching_Result.ModifiedStation = batch.ModifiedStation;
                        Matching_Result.NumberOfDocuments = batch.NumberOfDocuments;
                        Matching_Result.NumberOfPages = batch.NumberOfPages;
                        Matching_Result.NumberOfScannedPages = batch.NumberOfScannedPages;
                        Matching_Result.OutputBy = batch.OutputBy;
                        if (batch.OutputDate == DateTime.MinValue)
                            Matching_Result.OutputDate = null;
                        else
                            Matching_Result.OutputDate = batch.OutputDate;
                        Matching_Result.OutputStation = batch.OutputStation;
                        if (batch.PrepDate == DateTime.MinValue)
                            Matching_Result.PrepDate = null;
                        else
                            Matching_Result.PrepDate = batch.PrepDate;
                        Matching_Result.PrepTime = (float)batch.PrepTime;
                        Matching_Result.PrepUserName = batch.PrepUserName;
                        Matching_Result.ProjectName = batch.ProjectName;
                        Matching_Result.Qarflag = batch.QARFlag;
                        Matching_Result.Qcby = batch.QARFlag;
                        if (batch.QCDate == DateTime.MinValue)
                            Matching_Result.Qcdate = null;
                        else
                            Matching_Result.Qcdate = batch.QCDate;
                        if (batch.QCEndTime == DateTime.MinValue)
                            Matching_Result.QcendTime = null;
                        else
                            Matching_Result.QcendTime = batch.QCEndTime;
                        Matching_Result.QcstageTime = batch.QCStageTime;
                        if (batch.QCStartTime == DateTime.MinValue)
                            Matching_Result.QcstartTime = null;
                        else
                            Matching_Result.QcstartTime = batch.QCStartTime;
                        Matching_Result.Qcstation = batch.QCStation;
                        Matching_Result.Qctime = batch.QCTime;
                        Matching_Result.RecallBy = batch.RecallBy;
                        if (batch.RecallDate == DateTime.MinValue)
                            Matching_Result.RecallDate = null;
                        else
                            Matching_Result.RecallDate = batch.RecallDate;
                        Matching_Result.RecallReason = batch.RecallReason;
                        Matching_Result.RecallTimes = batch.RecallTimes;
                        Matching_Result.RejectedBy = batch.RejectedBy;
                        Matching_Result.RejectedTimes = batch.RejectedTimes;
                        Matching_Result.RejectionReason = batch.RejectionReason;
                        if (batch.ScannedDate == DateTime.MinValue)
                            Matching_Result.ScannedDate = null;
                        else
                            Matching_Result.ScannedDate = batch.ScannedDate;
                        Matching_Result.ScannedPagesReturned = batch.ScannedPagesReturned;
                        if (batch.ScanningEndTime == DateTime.MinValue)
                            Matching_Result.ScanningEndTime = null;
                        else
                            Matching_Result.ScanningEndTime = batch.ScanningEndTime;
                        Matching_Result.ScanningStageTime = batch.ScanningStageTime;
                        Matching_Result.ScanningTime = batch.ScanningTime;                       
                        Matching_Result.ScanStation = batch.ScanStation;
                        Matching_Result.StatusFlag = batch.StatusFlag;
                        Matching_Result.SubDeptName = batch.SubDepName;
                        Matching_Result.SubmittedBy = batch.SubmittedBy;
                        // Submitted Date is not null
                        Matching_Result.SubmittedDate = batch.SubmittedDate;
                        Matching_Result.TaskOrder = batch.TaskOrder;
                        if (batch.VFRUploadeDate == DateTime.MinValue)
                            Matching_Result.VfruploadDate = null;
                        else
                            Matching_Result.VfruploadDate = batch.VFRUploadeDate;
                        if (batch.VFRUploadModiffiedDate == DateTime.MinValue)
                            Matching_Result.VfruploadModifiedDate = null;
                        else
                            Matching_Result.VfruploadModifiedDate = batch.VFRUploadModiffiedDate;

                        if (batch.VFRUploadCompletedDate == DateTime.MinValue)
                            Matching_Result.VfruploadCompletedDate = null;
                        else
                            Matching_Result.VfruploadCompletedDate = batch.VFRUploadCompletedDate;
                        if (batch.VFRUploadRequestedDate == DateTime.MinValue)
                            Matching_Result.VfruploadRequestedDate = null;
                        else
                            Matching_Result.VfruploadRequestedDate = batch.VFRUploadRequestedDate;
                        Matching_Result.VfrstatusFlag = batch.VFRStatusFlag;

                        // Assign Work Order if necessary
                        if (Matching_Result.StatusFlag == "Approved" && Matching_Result.LotNumber == 0)
                        {
                            // A work order is necessary for this batch
                            Matching_Result.LotNumber = GetNextAvailableWorkOrder(Matching_Result.JobType).IntegerNumberReturnValue;
                            result.IntegerNumberReturnValue = Convert.ToInt32(Matching_Result.LotNumber);
                        }
                        Matching_Result.PageSizesCount = batch.PageSizesCount;

                        DB.SaveChanges();
                        result.Message = "UpdateBatchInformation transaction completed successfully. One Record Updated.";
                    }
                    else
                    {
                            // Means --> cannot update the status of a Batch that does not exist in the Database
                            result.ReturnCode = -1;
                            result.Message = "Invalid Batch Number or Batch Alias Name. UpdateBatchInformation transaction ignored.";
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
            logger.Trace("Leaving UpdateBatchInformation Method ...");
            return result;
        }


        /// <summary>
        /// Used for Batch Tracking
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewBatchEvent(string batchNumber,  string initialStatus, string finalStatus, 
                                                             string operatorName, string stationName, string eventDescription)
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
                logger.Trace("Entering into NewBatchEvent Method ...");

                // Check if Batch Exist
                GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                resultBatches = GetBatchesInformation("BatchNumber = \"" + batchNumber.Trim() + "\"" , "");
                if (resultBatches.RecordsCount != 0)
                    //if (resultBatches.RecordsCount != 0)
                    {
                    // Create new Batch
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        BatchTracking New_Record = new BatchTracking();
                        New_Record.BatchNumber = batchNumber;
                        New_Record.Date = DateTime.Now;
                        New_Record.InitialStatus = initialStatus;
                        New_Record.FinalStatus = finalStatus;
                        New_Record.OperatorId = operatorName;
                        New_Record.StationId = stationName;
                        New_Record.Event = eventDescription;
                       
                        DB.BatchTracking.Add(New_Record);
                        DB.SaveChanges();
                    }
                    result.Message = "NewBatchEvent transaction completed successfully. One Record added.";
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Batch Number  " + batchNumber + " doest not exist in the Database. NewBatchEvent transaction ignore.";
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
            logger.Trace("Leaving NewBatchEvent Method ...");
            return result;
        }

        /// <summary>
        /// Remove a given Batch Name from the Database
        /// </summary>
        /// <param name="batchName"></param>
        static public GlobalVars.ResultGeneric DeleteBatch(string batchName)
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
                logger.Trace("Entering into DeleteBatch Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    BatchControl Matching_Result  = DB.BatchControl.FirstOrDefault(x => x.BatchNumber == batchName);
                    if (Matching_Result != null)
                    {
                        DB.BatchControl.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteBatch transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Batch Number" + batchName + " does not exist. DeleteBatch transaction ignore.";
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
            logger.Trace("Leaving DeleteBatch Method ...");
            return result;
        }

        /// <summary>
        /// Remove Documents associated to a given Batch Name in the Database
        /// </summary>
        /// <param name="batchName"></param>
        static public GlobalVars.ResultGeneric DeleteBatchDocuments(string batchName)
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
                logger.Trace("Entering into DeleteBatchDocuments Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.BatchDocs.Where(x => x.BatchName == batchName);
                    if (results.Count() >= 1)
                    {
                        DB.BatchDocs.RemoveRange(DB.BatchDocs.Where(x => x.BatchName == batchName));
                        DB.SaveChanges();
                        result.Message = "DeleteBatchDocuments transaction completed successfully. A total of " + results.Count().ToString() + " Documents were removed.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "No Documents found for Batch Name" + batchName + ". DeleteBatchDocuments transaction ignore.";
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
            logger.Trace("Leaving DeleteBatchDocuments Method ...");
            return result;
        }
    }
}
