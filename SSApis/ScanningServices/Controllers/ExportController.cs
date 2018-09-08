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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ExportController : Controller
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Job Export Rules for a given Job ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Report Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ExportRules))]
        [ActionName("GetExportRulesByJobID")]
        public ActionResult GetExportRulesByJobID(int jobID)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultExportRules resultExportRules = new GlobalVars.ResultExportRules();
            try
            {
                logger.Info("GetExportRulesByJobID API Request. Report ID: " + jobID);

                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultExportRules.ReturnCode = -1;
                    resultExportRules.Message = "Missing argument Job ID";
                    logger.Warn("GetReportByID API Request ends with an Error.");
                    logger.Warn(resultExportRules.Message);
                }
                else
                {
                    resultExportRules = SQLFunctionsExportRules.GetExportRulesByJobID(jobID);
                    switch (resultExportRules.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetExportRulesByJobID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetExportRulesByJobID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultExportRules, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetExportRulesByJobID API Request ends with a Fatal Error.");
                resultExportRules.ReturnCode = -2;
                resultExportRules.Message = e.Message;
                var baseException = e.GetBaseException();
                resultExportRules.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultExportRules, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultExportRules.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultExportRules, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultExportRules.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultExportRules, Formatting.Indented);
            logger.Info("Leaving GetExportRulesByJobID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Export Transaction Job for a given Work Order or Batch Name
        /// </summary>
        /// <remarks>
        /// This method only works for Batches with the following status: "Waiting for Approval", "Approved" or "Exported"
        /// You must provide a value for one of teh following parameters: Work Order or the Batch Name. If both are submitted, the API will only check for Work Order
        /// </remarks>
        /// <param name="jobID">Required Field.</param>
        /// <param name="workOrder">Required Field if Batch Name is empty.</param>
        /// <param name="batchName">Work roder must be empty for this filter to work.</param>
        /// <param name="baseOutputDirectory">Required Field.</param>
        /// <response code="200">Ok</response>
        /// <returns>Generate Export Transaction Jobs in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultExportTransactionsJob))]
        [ActionName("GetExportTransactionsJob")]
        public ActionResult GetExportTransactionsJob(int jobID, string workOrder, string batchName, string baseOutputDirectory)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultExportTransactionsJob resultExportTransactionsJob = new GlobalVars.ResultExportTransactionsJob();
            try
            {
                logger.Info("GetExportTransactionsJob API Request.");
                logger.Info("   Job ID: " + jobID.ToString());
                logger.Info("   Work Order: " + workOrder);
                logger.Info("   Batch Name: " + batchName);
                logger.Info("   Base Output Directory: " + baseOutputDirectory);

                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultExportTransactionsJob.ReturnCode = -1;
                    resultExportTransactionsJob.Message = "Missing argument Job ID";
                    logger.Warn("GetExportTransactionsJob API Request ends with an Error.");
                    logger.Warn(resultExportTransactionsJob.Message);
                }
                else
                {
                    if (string.IsNullOrEmpty(baseOutputDirectory))
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        resultExportTransactionsJob.ReturnCode = -1;
                        resultExportTransactionsJob.Message = "Missing baseOutputDirectory.";
                        logger.Warn("GetExportTransactionsJob API Request ends with an Error.");
                        logger.Warn(resultExportTransactionsJob.Message);
                    }
                    else
                    {
                        //check if the base directory is a valid path
                        if (!IsValidPath(baseOutputDirectory))
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            resultExportTransactionsJob.ReturnCode = -1;
                            resultExportTransactionsJob.Message = "Invalid  baseOutputDirectory value.";
                            logger.Warn("GetExportTransactionsJob API Request ends with an Error.");
                            logger.Warn(resultExportTransactionsJob.Message);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(workOrder) && string.IsNullOrEmpty(batchName))
                            {
                                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                resultExportTransactionsJob.ReturnCode = -1;
                                resultExportTransactionsJob.Message = "Mising Work Order or Batch Name.";
                                logger.Warn("GetExportTransactionsJob API Request ends with an Error.");
                                logger.Warn(resultExportTransactionsJob.Message);
                            }
                            else
                            {
                                resultExportTransactionsJob = SQLFunctionsExportRules.GetExportTransactionsJob(jobID, workOrder, batchName, baseOutputDirectory);
                                switch (resultExportTransactionsJob.ReturnCode)
                                {
                                    case 0:
                                        logger.Info("GetExportTransactionsJob API Request was executed Successfully.");
                                        Response.StatusCode = (int)HttpStatusCode.OK;
                                        break;

                                    case -2:
                                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                        logger.Fatal("GetExportTransactionsJob API Request ends with a Fatal Error.");
                                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultExportTransactionsJob, Formatting.Indented));
                                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                        break;
                                }
                            }
                        }                        
                    }                   
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetExportTransactionsJob API Request ends with a Fatal Error.");
                resultExportTransactionsJob.ReturnCode = -2;
                resultExportTransactionsJob.Message = e.Message;
                var baseException = e.GetBaseException();
                resultExportTransactionsJob.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultExportTransactionsJob, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultExportTransactionsJob.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultExportTransactionsJob, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultExportTransactionsJob.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultExportTransactionsJob, Formatting.Indented);
            logger.Info("Leaving GetExportRulesByJobID API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Update Job Export Rules info for a given Job ID or create a new one if Export Rules does not exist.
        /// </summary>
        /// <remarks> 
        /// </remarks>
        /// <param name="exportRulesJS">
        /// Required Field. Report information in a Json Format.
        /// Sample Request:
        /// {"JobID": 1,"JobName": "COH-SWM","DirectoryFormat": ["[Customer Name] "," \\ "," [Job Type] "," \\ "," [Work Order] "," \\ "," \"ok\""],"FileNameFormat": [""],
        /// "PreserveDocXMLFileFlag": false,"DuplicateFileAction": "Replace ","DirectoryReplaceRule": [],"FileNameReplaceRule": [],
        /// "FieldsReplaceRule": [{"Name": "First Name","Pattern": "Leonardo","ReplaceBy": "Juan "},{"Name": " Last Name","Pattern": "Carbone","ReplaceBy": "Celaya"}],
        /// "OutputFileFormat": "Document XML ","OutputFileDelimeter": null,"OutputFields": [""],"MetadataFileName": "Batch Name","MetadataFileScope": "One per Batch"}
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateJobExportRules")]
        public ActionResult UpdateJobExportRules([FromBody]string exportRulesJS)
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
                if (exportRulesJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument exportRulesJS";
                    logger.Warn("UpdateJobExportRules API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.ExportRules exportRules = JsonConvert.DeserializeObject<GlobalVars.ExportRules>(exportRulesJS);
                    logger.Info("UpdateJobExportRules API Request.");
                    logger.Debug("Parameter:" + JsonConvert.SerializeObject(exportRulesJS, Formatting.Indented));

                    //Rules:
                    // 1- Job ID not null
                    if (exportRules.JobID == 0)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "You must provide a Valid Job ID.";
                        logger.Warn("UpdateJobExportRules API Request ends with a Fatal Error.");
                        logger.Warn(result.Message);
                    }
                    else
                    {
                        result = SQLFunctionsExportRules.UpdateExportRule(exportRules);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateJobExportRules API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateJobExportRules API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateJobExportRules API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateJobExportRules API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public bool IsValidPath(string path)
        {
            Regex r = new Regex(@"^(?:[\w]\:|\\)(\\[a-zA-Z_\-\ –\s0-9\.\\]+)$");
            //Regex r = new Regex(@"^[a - zA - Z]:\\(((? ![<>:"/\\|?*]).)+((?<![ .])\\)?)*$");
            //Regex r = new Regex(@"^(?:[\w]\:|\\)(\\[a-z_\-\s0-9\.]+)+\.(?i)(txt|gif|pdf|doc|docx|xls|xlsx)$");
            return r.IsMatch(path.ToLower());
        }
    }
}