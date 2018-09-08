using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScanningServicesDataObjects;
using NLog;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Net;
using ScanningServices.Models;


namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class BatchesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Batches filter by Status
        /// </summary>
        /// <param name="status">Required Field</param>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Batches in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultBatches))]
        [ActionName("GetBatchesByStatus")]
        public ActionResult GetBatchesByStatus(string status)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
            try
            {
                if (status == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultBatches.ReturnCode = -1;
                    resultBatches.Message = "Status is a required parameter.";
                    logger.Warn("GetBatchesByStatus API Request ends with an Error.");
                    logger.Warn(resultBatches.Message);
                }
                else
                {
                    logger.Info("GetBatchesByStatus API Request. Status: " + status);
                    resultBatches = SQLFunctionsBatches.GetBatchesByStatus(status);
                    switch (resultBatches.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetBatchesByStatus API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetBatchesByStatus API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatches, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetBatchesByStatus API Request ends with a Fatal Error.");
                resultBatches.ReturnCode = -2;
                resultBatches.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatches.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultBatches, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultBatches.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatches, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(resultBatches, Formatting.Indented);
            logger.Info("Leaving GetBatchesByStatus API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Batch(es) Information.
        /// </summary>
        /// <param name="filter">Optional Field. If not value is provided, the API will returns all records in the Database. Sample 1: StatusFlag="Ready to Scan". Sample 2: StatusFlag="Ready to Scan" And  NumberOfDocuments=0. Sample 3" (ScannedDate >= "02/26/2017 00:00 AM") AND (ScannedDate &lt;= "02/28/2017 00:00 AM")</param>
        /// <param name="orderBy">Optional Field. Sample 1: "Category.CategoryName, UnitPrice descending" </param>
        /// <remarks>
        /// <para> - The Method does not validate the syntax of the Filters/orderby, so you need to  make sure you are using the correct Database Field Names with the appropiate operators and conditions.</para>
        /// <para> - Use the filter to norrow down your search. If no filter is provided, this method returns all the existing batches in the Database.</para>
        /// <para> - If sortby condition is no provided, this this will return bathes order by BatchNumber ascending.</para>
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Batches Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultBatches))]
        [ActionName("GetBatchesInformation")]
        public ActionResult GetBatchesInformation(string filter, string orderBy)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
            try
            {               
                logger.Info("GetBatchesInformation API Request. Status: " + filter);
                resultBatches = SQLFunctionsBatches.GetBatchesInformation(filter, orderBy);
                switch (resultBatches.ReturnCode)
                {
                    case 0:
                        logger.Info("GetBatchesInformation API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetBatchesInformation API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatches, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetBatchesInformation API Request ends with a Fatal Error.");
                resultBatches.ReturnCode = -2;
                resultBatches.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatches.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultBatches, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultBatches.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatches, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(resultBatches, Formatting.Indented);
            logger.Info("Leaving GetBatchesInformation API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Batch(es) Docs Information.
        /// </summary>
        /// <param name="filter">Optional Field. If not value is provided, the API will returns all records in the Database.Sample 1: "BatchName = "KC-1"</param>
        /// <param name="orderBy">Optional Field. Sample 1: "Category.CategoryName, UnitPrice descending" </param>
        /// <remarks>
        /// <para> - The Method does not validate the syntax of the Filters/orderby, so you need to  make sure you are using the correct Database Field Names with the appropiate operators and conditions.</para>
        /// <para> - Use the filter to norrow down your search. If no filter is provided, this method returns all the existing batches in the Database.</para>
        /// <para> - If sortby condition is no provided, this this will return bathes order by BatchName ascending.</para>
        /// <para> - List of values for the Metadata scope tag:</para>
        /// <para />
        /// <para>    scope = 1 translates to "Batch"</para>
        /// <para />
        /// <para>    scope = 2 translates to "Document"</para>
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Batches Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultBatchDocs))]
        [ActionName("GetBatchDocsInformation")]
        public ActionResult GetBatchDocsInformation(string filter, string orderBy)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultBatchDocs resultBatchDocs = new GlobalVars.ResultBatchDocs();
            try
            {
                logger.Info("GetBatchDocsInformation API Request. Status: " + filter);
                resultBatchDocs = SQLFunctionsBatches.GetBatchDocInformation(filter, orderBy);
                switch (resultBatchDocs.ReturnCode)
                {
                    case 0:
                        logger.Info("GetBatchDocsInformation API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetBatchDocsInformation API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchDocs, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetBatchDocsInformation API Request ends with a Fatal Error.");
                resultBatchDocs.ReturnCode = -2;
                resultBatchDocs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchDocs.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultBatchDocs, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultBatchDocs.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchDocs, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(resultBatchDocs, Formatting.Indented);
            logger.Info("Leaving GetBatchDocsInformation API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Batches Summary Information based on a Filter
        /// </summary>
        /// <param name="filter">Optional Field. If not value is provided, the API will returns all records in the Database.
        /// Sample 1: StatusFlag="Ready to Scan"
        /// Sample 2: Customer="Methodist"
        /// Sample 3" (ScannedDate >= "02/26/2017 00:00 AM") AND (ScannedDate &lt;= "02/28/2017 00:00 AM")
        /// </param>
        /// <remarks>
        /// <para> - The Method does not validate the syntax of the Filters so you need to  make sure you are using the correct Database Field Names with the appropiate operators and conditions. </para>
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Batches Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.BatchSummary))]
        [ActionName("GetBatchesSummary")]
        public ActionResult GetBatchesSummary(string filter)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultBatchSummary resultBatchSummary = new GlobalVars.ResultBatchSummary();
            try
            {
                logger.Info("GetBatchesSummary API Request. Status: " + filter);
                resultBatchSummary = SQLFunctionsBatches.GetBatchSummary(filter);
                switch (resultBatchSummary.ReturnCode)
                {
                    case 0:
                        logger.Info("GetBatchesSummary API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetBatchesSummary API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchSummary, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
              }
            catch (Exception e)
            {
                logger.Fatal("GetBatchesSummary API Request ends with a Fatal Error.");
                resultBatchSummary.ReturnCode = -2;
                resultBatchSummary.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchSummary.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultBatchSummary, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultBatchSummary.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchSummary, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(resultBatchSummary, Formatting.Indented);
            logger.Info("Leaving GetBatchesSummary API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Batch Tracking Information based on a given filter.
        /// </summary>
        /// <param name="filter">Optional Field. If not value is provided, the API will returns all records in the Database. 
        /// Sample 1: StatusFlag="Ready to Scan". 
        /// Sample 2: StatusFlag="Ready to Scan" And  NumberOfDocuments=0. 
        /// Sample 3" (ScannedDate >= "02/26/2017 00:00 AM") AND (ScannedDate &lt;= "02/28/2017 00:00 AM")</param>
        /// <param name="orderBy">Optional Field. By default it will sort by ID ascending. 
        /// Sample 1: "Category.CategoryName, UnitPrice descending" </param>
        /// <remarks>
        /// The Method does not validate the syntax of the Filters so you need to  make sure you are using the correct Database Field Names
        /// with the appropiate operators and conditions.
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Batch Tracking Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultBatchTracking))]
        [ActionName("GetBatchTrackingInformation")]
        public ActionResult GetBatchTrackingInformation(string filter, string orderBy)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultBatchTracking resultBatchTracking = new GlobalVars.ResultBatchTracking();
            try
            {                
                logger.Info("GetBatchTrackingInformation API Request. Status: " + filter);
                resultBatchTracking = SQLFunctionsBatches.GetBatchTrackingInformation(filter, orderBy);
                switch (resultBatchTracking.ReturnCode)
                {
                    case 0:
                        logger.Info("GetBatchTrackingInformation API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetBatchTrackingInformation API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetBatchTrackingInformation API Request ends with a Fatal Error.");
                resultBatchTracking.ReturnCode = -2;
                resultBatchTracking.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchTracking.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultBatchTracking.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented);
            logger.Info("Leaving GetBatchTrackingInformation API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Next Available Work Order for a Given Job TYpe.
        /// </summary>
        /// <param name="jobType">Required Field.</param>
        /// <remarks>
        /// The Method look for the highest Work Order Number that can be assigned to a Batch based on Job Type it belongs to.
        /// The default value for the Maximun Number of Batches for a Work Order is 15 unless the Job Type definition has a 
        /// different value.
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Batch Tracking Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultGeneric))]
        [ActionName("GetNextAvailableWorkOrder")]
        public ActionResult GetNextAvailableWorkOrder(string jobType)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric();
            try
            {
                logger.Info("GetNextAvailableWorkOrder API Request. Job Type: " + jobType);
                result = SQLFunctionsBatches.GetNextAvailableWorkOrder(jobType);
                switch (result.ReturnCode)
                {
                    case 0:
                        logger.Info("GetNextAvailableWorkOrder API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetNextAvailableWorkOrder API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetNextAvailableWorkOrder API Request ends with a Fatal Error.");
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            result.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving GetNextAvailableWorkOrder API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Batch Tracking Information to be used for Daily Operation Reports
        /// </summary>
        /// <param name="dateFrom">
        /// Required field. If not value is provided, the API will use toda's day at 00:00 AM 
        /// Sample:  "02/26/2017 00:00 AM"
        /// </param>
        /// <param name="dateTo">
        /// Required field. If not value is porvided, the API will use today's day at 23:59 PM 
        /// Sample: "02/26/2017 03:00 PM"
        /// </param>
        /// <remarks>
        /// <para> - This method query the Batch Tracking Database Table and filter records that were tagged as 'QC Started' or  'QC Completed' in a given time frame (Date-From , Date-To). </para>
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Batch Tracking Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultBatchTrackingExtended))]
        [ActionName("GetInformationForDailyOperationReport")]
        public ActionResult GetInformationForDailyOperationReport(DateTime dateFrom, DateTime dateTo)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultBatchTrackingExtended resultBatchTracking = new GlobalVars.ResultBatchTrackingExtended();
            try
            {
                logger.Info("GetInformationForDailyOperationReport API Request. From : " + dateFrom.ToString() + " to: " + dateTo.ToString());
                if (dateFrom == DateTime.MinValue || dateFrom == DateTime.MinValue)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultBatchTracking.ReturnCode = -1;
                    resultBatchTracking.Message = "Wrong Date-From and/or DateTo argument(s).";
                    logger.Warn("GetInformationForDailyOperationReport API Request ends with an Error.");
                    logger.Warn(resultBatchTracking.Message);
                }
                else
                {
                    resultBatchTracking = SQLFunctionsBatches.GetInformationForDailyOperationReport(dateFrom, dateTo);
                    switch (resultBatchTracking.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetInformationForDailyOperationReport API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetInformationForDailyOperationReport API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }                
            }
            catch (Exception e)
            {
                logger.Fatal("GetInformationForDailyOperationReport API Request ends with a Fatal Error.");
                resultBatchTracking.ReturnCode = -2;
                resultBatchTracking.Message = e.Message;
                var baseException = e.GetBaseException();
                resultBatchTracking.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultBatchTracking.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(resultBatchTracking, Formatting.Indented);
            logger.Info("Leaving GetInformationForDailyOperationReport API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Use for Batch Registration.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="batchJS">
        /// Required field.</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("BatchRegistration")]
        public ActionResult BatchRegistration([FromBody]string batchJS)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                Message = "",
                ReturnCode = 0,
            };
            try
            {
                logger.Info("BatchRegistration API Request.");
                if (batchJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument batchJS";
                    logger.Warn("NewJob API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Batch batch = JsonConvert.DeserializeObject<GlobalVars.Batch>(batchJS);
                    if (string.IsNullOrEmpty(batch.StatusFlag) || string.IsNullOrEmpty(batch.Customer) || 
                        string.IsNullOrEmpty(batch.ProjectName) || string.IsNullOrEmpty(batch.JobType))
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Missing argument StatusFlag, Customer, ProjectName, or JobType";
                        logger.Warn("BatchRegistration API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(batch.BatchNumber) && string.IsNullOrEmpty(batch.BatchAlias))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            result.ReturnCode = -1;
                            result.Message = "Missing argument Batch Number or Batch Alias";
                            logger.Warn("BatchRegistration API Request ends with an Error.");
                            logger.Warn(result.Message);
                        }
                        else
                        {
                            logger.Debug("Parameters");
                            logger.Debug("  Batch Number: " + batch.BatchNumber);
                            logger.Debug("  Batch Alias: " + batch.BatchAlias);
                            logger.Debug("  Status Flag: " + batch.StatusFlag);
                            logger.Debug("  Customer: " + batch.Customer);
                            logger.Debug("  Project Name: " + batch.ProjectName);
                            logger.Debug("  Job Type: " + batch.JobType);
                            logger.Debug("  Department: " + batch.DepName);
                            logger.Debug("  Submitted by: " + batch.SubmittedBy);
                          
                            batch.SubmittedDate = DateTime.Now;
                            result = SQLFunctionsBatches.BatchRegistration(batch);
                            switch (result.ReturnCode)
                            {
                                case 0:
                                    logger.Info("BatchRegistration API Request was executed Successfully.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -1:
                                    logger.Info("BatchRegistration API Request ends with a warning.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -2:
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    logger.Fatal("BatchRegistration API Request ends with a Fatal Error.");
                                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    break;
                            }

                        }
                    }
                }                
            }
            catch (Exception e)
            {
                logger.Fatal("BatchRegistration API Request ends with a Fatal Error.");
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            result.ElapsedTime = elapsedMs.ToString();
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving BatchRegistration API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Update a given batch. 
        /// </summary>
        /// <remarks>
        /// When batch status is chenged to Approved, the Methid return the assinged work order in the IntegerNumberReturnValue variable.        /// 
        /// </remarks>
        /// <param name="batchJS">
        /// Required Field. Batch information in a Json Format. 
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateBatchInformation")]
        public ActionResult UpdateBatchInformation([FromBody]string batchJS)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                Message = "",
                ReturnCode = 0,
                //ReturnValue = ""
            };
            try
            {
                if (batchJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument batchJS";
                    logger.Warn("UpdateBatchInformation API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Batch batch = JsonConvert.DeserializeObject<GlobalVars.Batch>(batchJS);
                    logger.Info("UpdateBatchInformation API Request.");
                    if (!string.IsNullOrEmpty(batch.BatchNumber))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(batch, Formatting.Indented));

                        result = SQLFunctionsBatches.UpdateBatchInformation(batch);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateBatchInformation API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("UpdateBatchInformation API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateBatchInformation API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Batch Number value is not valid.";
                        logger.Warn("UpdateBatchInformation API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateBatchInformation API Request ends with a Fatal Error.");
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            result.ElapsedTime = elapsedMs.ToString();
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving UpdateBatchInformation API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Delete Batch Documents from the Databaase
        /// </summary>
        /// <remarks>
        /// This method remove associated documents to a given Batch  from the Datbasase 
        /// </remarks>
        /// <param name="batchName">Required Field.</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteBatchDocuments")]
        public ActionResult DeleteBatchDocuments(string batchName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                Message = "",
                ReturnCode = 0,
                //ReturnValue = ""
            };
            try
            {
                // Batch Name is a Required Field
                if (string.IsNullOrEmpty(batchName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Batch Name is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteBatchDocuments API Request. Batch Name: " + batchName);
                    result = SQLFunctionsBatches.DeleteBatchDocuments(batchName);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteBatchDocuments ends successfully
                            logger.Info("Documents for Batch Name:  " + batchName + " were Deleted Successfully.");
                            result.Message = "Documents for Batch Name:  " + batchName + " were Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteBatchDocuments API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteCustomer ends with a Fatal error
                            logger.Fatal("DeleteBatchDocuments API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteBatchDocuments API Request ends with a Fatal Error.");
                System.Diagnostics.Trace.WriteLineIf(true, "Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            result.ElapsedTime = elapsedMs.ToString();
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving DeleteBatchDocuments API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Delete Batch from the Databaase
        /// </summary>
        /// <remarks>
        /// This method remove a given Batch  from the Datbasase 
        /// </remarks>
        /// <param name="batchName">Required Field.</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteBatch")]
        public ActionResult DeleteBatch(string batchName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                Message = "",
                ReturnCode = 0,
                //ReturnValue = ""
            };
            try
            {
                // Batch Name is a Required Field
                if (string.IsNullOrEmpty(batchName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Batch Name is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteBatch API Request. Batch Name: " + batchName);
                    result = SQLFunctionsBatches.DeleteBatch(batchName);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteBatch ends successfully
                            logger.Info("Batch Name:  " + batchName + " was Deleted Successfully.");
                            result.Message = "Batch Name:  " + batchName + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteBatch API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteCustomer ends with a Fatal error
                            logger.Fatal("DeleteBatch API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteBatch API Request ends with a Fatal Error.");
                System.Diagnostics.Trace.WriteLineIf(true, "Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            result.ElapsedTime = elapsedMs.ToString();
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving DeleteBatch API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Used for tracking propose 
        /// This Method must be use for any event in Scanning Services that produce a change in the Batch Status.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="batchTrackingJS">
        /// Required field.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("NewBatchEvent")]
        public ActionResult NewBatchEvent([FromBody]string batchTrackingJS)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                Message = "",
                ReturnCode = 0,
            };
            try
            {
                logger.Info("NewBatchEvent API Request.");
                if (batchTrackingJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument batchTrackingJS";
                    logger.Warn("NewJob API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.BatchTracking batchTracking = JsonConvert.DeserializeObject<GlobalVars.BatchTracking>(batchTrackingJS);

                    if (string.IsNullOrEmpty(batchTracking.InitialStatus) || string.IsNullOrEmpty(batchTracking.FinalStatus) || string.IsNullOrEmpty(batchTracking.OperatorName) ||
                    string.IsNullOrEmpty(batchTracking.StationName) || string.IsNullOrEmpty(batchTracking.Event))
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Missing argument initialStatus, finalStatus, operatorName, stationName, or eventDescription";
                        logger.Warn("NewBatchEvent API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(batchTracking.BatchNumber))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            result.ReturnCode = -1;
                            result.Message = "Missing argument Batch Number or Batch Alias";
                            logger.Warn("NewBatchEvent API Request ends with an Error.");
                            logger.Warn(result.Message);
                        }
                        else
                        {
                            logger.Debug("Parameters");
                            logger.Debug("  Batch Number: " + batchTracking.BatchNumber);
                            logger.Debug("  Initial Status: " + batchTracking.InitialStatus);
                            logger.Debug("  Final Status: " + batchTracking.FinalStatus);
                            logger.Debug("  Operator Name: " + batchTracking.OperatorName);
                            logger.Debug("  Station Name: " + batchTracking.StationName);
                            logger.Debug("  Event Description: " + batchTracking.Event);

                            result = SQLFunctionsBatches.NewBatchEvent(batchTracking.BatchNumber, batchTracking.InitialStatus, batchTracking.FinalStatus,
                                                                        batchTracking.OperatorName, batchTracking.StationName, batchTracking.Event);
                            switch (result.ReturnCode)
                            {
                                case 0:
                                    logger.Info("NewBatchEvent API Request was executed Successfully.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -1:
                                    logger.Info("NewBatchEvent API Request ends with a warning.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -2:
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    logger.Fatal("NewBatchEvent API Request ends with a Fatal Error.");
                                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    break;
                            }
                        }
                    }
                }                
            }
            catch (Exception e)
            {
                logger.Fatal("NewBatchEvent API Request ends with a Fatal Error.");
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            result.ElapsedTime = elapsedMs.ToString();
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving NewBatchEvent API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Add Batch Documents 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="batchDocumentsJS">
        /// Required field.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("NewBatchDocument")]
        public ActionResult NewBatchDocument([FromBody]string batchDocumentsJS)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                Message = "",
                ReturnCode = 0,
            };
            try
            {
                logger.Info("NewBatchDocument API Request.");
                if (batchDocumentsJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument batchDocumentsJS";
                    logger.Warn("NewBatchDocument API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.BatchDocs batchDocument = JsonConvert.DeserializeObject<GlobalVars.BatchDocs>(batchDocumentsJS);
                   
                    if (string.IsNullOrEmpty(batchDocument.BatchName))
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Missing argument Batch Name";
                        logger.Warn("NewBatchDocument API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                    else
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(batchDocument, Formatting.Indented));

                        result = SQLFunctionsBatches.NewBatchDocument(batchDocument);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewBatchEvent API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewBatchDocument API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewBatchDocuments API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }                
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewBatchDocument API Request ends with a Fatal Error.");
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            result.ElapsedTime = elapsedMs.ToString();
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving NewBatchDocument API");
            //return Json(messaje);
            return Content(messaje);
        }

    }
}