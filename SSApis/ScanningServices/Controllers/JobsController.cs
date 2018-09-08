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
using System.Text.RegularExpressions;

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class JobsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Kodak Jobs 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Jobs in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultJobs))]
        [ActionName("GetJobs")]
        public ActionResult GetJobs()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
            try
            {
                logger.Info("GetJobs API Request.");
                resultJobs = SQLFunctionsJobs.GetJobs();
                switch (resultJobs.ReturnCode)
                {
                    case 0:
                        logger.Info("GetJobs API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetJobs API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetJobs API Request ends with a Fatal Error.");
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultJobs.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
            resultJobs.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultJobs, Formatting.Indented);
            logger.Info("Leaving GetJobs API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Job Info by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Jobs in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultJobs))]
        [ActionName("GetJobByID")]
        public ActionResult GetJobByID(int jobID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
            try
            {
                logger.Info("GetJobByID API Request. Job ID: " + jobID);
                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultJobs.ReturnCode = -1;
                    resultJobs.Message = "Missing argument Job ID";
                    logger.Warn("GetJobByID API Request ends with an Error.");
                    logger.Warn(resultJobs.Message);
                }
                else
                {
                    resultJobs = SQLFunctionsJobs.GetJobByID(jobID);
                    switch (resultJobs.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetJobByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetJobByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetJobByID API Request ends with a Fatal Error.");
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultJobs.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
            resultJobs.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultJobs, Formatting.Indented);
            logger.Info("Leaving GetJobByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Jobs by a given Project ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="projectID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Jobs in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultJobs))]
        [ActionName("GetJobsByProjectID")]
        public ActionResult GetJobsByProjectID(int projectID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
            try
            {
                logger.Info("GetJobsByProjectID API Request. Project ID: " + projectID);
                if (projectID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultJobs.ReturnCode = -1;
                    resultJobs.Message = "Missing argument Project ID";
                    logger.Warn("GetJobsByProjectID API Request ends with an Error.");
                    logger.Warn(resultJobs.Message);
                }
                else
                {
                    resultJobs = SQLFunctionsJobs.GetJobByProjectID(projectID);
                    switch (resultJobs.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetJobsByProjectID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetJobsByProjectID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetJobsByProjectID API Request ends with a Fatal Error.");
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultJobs.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
            resultJobs.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultJobs, Formatting.Indented);
            logger.Info("Leaving GetJobsByProjectID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Job Info by Name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobName">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Job info in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultJobs))]
        [ActionName("GetJobByName")]
        public ActionResult GetJobByName(string jobName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
            try
            {
                logger.Info("GetJobByName API Request. Job Name: " + jobName);
                if (string.IsNullOrEmpty(jobName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultJobs.ReturnCode = -1;
                    resultJobs.Message = "Missing argument Job Name";
                    logger.Warn("GetJobByName API Request ends with an Error.");
                    logger.Warn(resultJobs.Message);
                }
                else
                {
                    resultJobs = SQLFunctionsJobs.GetJobByName(jobName);
                    switch (resultJobs.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetJobByName API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetJobByName API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetJobByName API Request ends with a Fatal Error.");
                resultJobs.ReturnCode = -2;
                resultJobs.Message = e.Message;
                var baseException = e.GetBaseException();
                resultJobs.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultJobs.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultJobs, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultJobs.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultJobs, Formatting.Indented);
            logger.Info("Leaving GetJobByName API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update a given Job. 
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{JobID: 12,ProjectID: 6,JobName: Test Job,CPScanDirectory: "C:\\\\ScanDirectory",CPOutputDirectory: "C:\\\\CPDirectory",PDFLoadBalanceDirectory: "C:\\\\LoadBalnaceDirectory",PDFWatchFolder: "C:\\\\PDFWatchFolder",FinalBatchesDirectory: "C:\\\\FinalBatchLocation",ExportClassName: ,DepartmentName: Finance}'
        /// </remarks>
        /// <param name="jobJS">
        /// Required Field. Job information in a Json Format.
        /// The Job Name must be unique inthe entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateJob")]
        public ActionResult UpdateJob([FromBody]string jobJS)
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
                if (jobJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument jobJS";
                    logger.Warn("UpdateJob API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Job job = JsonConvert.DeserializeObject<GlobalVars.Job>(jobJS);
                    logger.Info("UpdateJob API Request.");
                    if (!string.IsNullOrEmpty(job.JobName))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(job, Formatting.Indented));

                        result = SQLFunctionsJobs.UpdateJob(job);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateJob API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("UpdateJob API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateJob API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "JobName value is not valid.";
                        logger.Warn("UpdateJob API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateJob API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateJob API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Create a new Job
        /// </summary>
        /// /// <remarks>
        /// Sample Request:
        /// '{JobID: 0,ProjectID: 6,JobName: Test Job,CPScanDirectory: "C:\\\\ScanDirectory",CPOutputDirectory: "C:\\\\CPDirectory",PDFLoadBalanceDirectory: "C:\\\\LoadBalnaceDirectory",PDFWatchFolder: "C:\\\\PDFWatchFolder",FinalBatchesDirectory: "C:\\\\FinalBatchLocation",ExportClassName: ,DepartmentName: Finance}'
        /// </remarks>
        /// <param name="jobJS">
        /// Required Field. Job information must be provided in Json Format.
        /// The Job Name must be unique in the entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewJob")]
        public ActionResult NewJob([FromBody]string jobJS)
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
                if (jobJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument jobJS";
                    logger.Warn("NewJob API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Job job = JsonConvert.DeserializeObject<GlobalVars.Job>(jobJS);
                    logger.Info("NewJob API Request.");
                    if (!string.IsNullOrEmpty(job.JobName) & job.ProjectID != 0)
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(job, Formatting.Indented));

                        result = SQLFunctionsJobs.NewJob(job);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewJob API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewJob API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewJob API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Job Name and/or Project ID value is not valid.";
                        logger.Warn("NewJob API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewJob API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewJob API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Delete Job by a given ID
        /// </summary>
        /// <remarks>
        /// If the given Job ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="jobID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteJob")]
        public ActionResult DeleteJob(int jobID)
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
                // Job ID is a Required Field
                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Job ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteJob API Request. Job ID: " + jobID);
                    result = SQLFunctionsJobs.DeleteJob(jobID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteJob ends successfully
                            logger.Info("Job ID:  " + jobID + " was Deleted Successfully.");
                            result.Message = "Job ID:  " + jobID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteJob API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteProject ends with a Fatal error
                            logger.Fatal("DeleteJob API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteJob API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteJob API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Page Sizes for a given Job ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Job's Page Sizes in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultFields))]
        [ActionName("GetPageSizesByJobID")]
        public ActionResult GetPageSizesByJobID(int jobID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultJobPageSizes resulJobPageSizes = new GlobalVars.ResultJobPageSizes();
            try
            {
                logger.Info("GetPageSizesByJobID API Request. Job ID: " + jobID.ToString());
                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resulJobPageSizes.ReturnCode = -1;
                    resulJobPageSizes.Message = "Missing argument Job ID";
                    logger.Warn("GetPageSizesByJobID API Request ends with an Error.");
                    logger.Warn(resulJobPageSizes.Message);
                }
                else
                {
                    resulJobPageSizes = SQLFunctionsJobs.GetPageSizesByJobID(jobID);
                    switch (resulJobPageSizes.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetPageSizesByJobID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetPageSizesByJobID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resulJobPageSizes, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetPageSizesByJobID API Request ends with a Fatal Error.");
                resulJobPageSizes.ReturnCode = -2;
                resulJobPageSizes.Message = e.Message;
                var baseException = e.GetBaseException();
                resulJobPageSizes.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resulJobPageSizes, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resulJobPageSizes.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resulJobPageSizes, Formatting.Indented));
            resulJobPageSizes.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resulJobPageSizes, Formatting.Indented);
            logger.Info("Leaving GetPageSizesByJobID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update a Job's Page Size
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{"ID": 1, "JobID": 18, "CategoryName": "Grp4", "High": 16, "Width": 20 }'
        /// </remarks>
        /// <param name="jobPageSizeJS">
        /// Required Field. Page Size information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateJobPageSize")]
        public ActionResult UpdateJobPageSize([FromBody]string jobPageSizeJS)
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
                if (jobPageSizeJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument fieldJS";
                    logger.Warn("UpdateJobPageSize API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.JobPageSize jobPageSize = JsonConvert.DeserializeObject<GlobalVars.JobPageSize>(jobPageSizeJS);
                    logger.Info("UpdateJobPageSize API Request.");
                    if (!string.IsNullOrEmpty(jobPageSize.CategoryName))
                    {
                        if (jobPageSize.JobID == 0 || jobPageSize.High == 0 || jobPageSize.Width == 0)
                        {
                            // High and/or Width values can not be Zero
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            result.ReturnCode = -1;
                            result.Message = "Job ID , High and/or Width values cannot be Zero.";
                            logger.Warn("UpdateJobPageSize API Request ends with an Error.");
                            logger.Warn(result.Message);
                        }
                        else
                        {
                            logger.Debug("Parameter:" + JsonConvert.SerializeObject(jobPageSize, Formatting.Indented));

                            result = SQLFunctionsJobs.UpdateJobPageSize(jobPageSize);
                            switch (result.ReturnCode)
                            {
                                case 0:
                                    logger.Info("UpdateJobPageSize API Request was executed Successfully.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -1:
                                    logger.Info("UpdateJobPageSize API Request ends with a warning.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -2:
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    logger.Fatal("UpdateJobPageSize API Request ends with a Fatal Error.");
                                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    break;
                            }
                        }                        
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Category Name value is not valid.";
                        logger.Warn("UpdateJobPageSize API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateJobPageSize API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateJobPageSize API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Create a new Job's Page Size definition
        /// </summary>
        /// /// <remarks>
        /// Sample Request:
        /// '{"ID": 0, "JobID": 18, "CategoryName": "Grp4", "High": 16, "Width": 20 }'
        /// </remarks>
        /// <param name="jobPageSizeJS">
        /// Required Field. Page Sizze information must be provided in Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewJobPageSize")]
        public ActionResult NewJobPageSize([FromBody]string jobPageSizeJS)
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
                if (jobPageSizeJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument fieldJS";
                    logger.Warn("NewJobPageSize API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.JobPageSize jobPageSize = JsonConvert.DeserializeObject<GlobalVars.JobPageSize>(jobPageSizeJS);
                    logger.Info("NewJobPageSize API Request.");


                    if (!string.IsNullOrEmpty(jobPageSize.CategoryName))
                    {
                        if (jobPageSize.JobID == 0 || jobPageSize.High == 0 || jobPageSize.Width == 0)
                        {
                            // High and/or Width values can not be Zero
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            result.ReturnCode = -1;
                            result.Message = "Job ID , High and/or Width values cannot be Zero.";
                            logger.Warn("NewJobPageSize API Request ends with an Error.");
                            logger.Warn(result.Message);
                        }
                        else
                        {
                            logger.Debug("Parameter:" + JsonConvert.SerializeObject(jobPageSize, Formatting.Indented));

                            result = SQLFunctionsJobs.NewJobPageSize(jobPageSize);
                            switch (result.ReturnCode)
                            {
                                case 0:
                                    logger.Info("NewJobPageSize API Request was executed Successfully.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -1:
                                    logger.Info("NewJobPageSize API Request ends with a warning.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -2:
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    logger.Fatal("NewJobPageSize API Request ends with a Fatal Error.");
                                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Category Name value is not valid.";
                        logger.Warn("NewJobPageSize API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewJobPageSize API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewJobPageSize API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Delete Page Size definition 
        /// </summary>
        /// <remarks>
        /// If the given Filed ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="pageSizeID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteJobPageSize")]
        public ActionResult DeleteJobPageSize(int pageSizeID)
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
                // Page Size ID is a Required Field
                if (pageSizeID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Page Size ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteJobPageSize API Request. Page Zise ID: " + pageSizeID);
                    result = SQLFunctionsJobs.DeleteJobPageSize(pageSizeID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteJob ends successfully
                            logger.Info("Page Size ID:  " + pageSizeID + " was Deleted Successfully.");
                            result.Message = "Page Size ID:  " + pageSizeID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteJobPageSize API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteProject ends with a Fatal error
                            logger.Fatal("DeleteJobPageSize API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteJobPageSize API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteJobPageSize API");
            //return Json(messaje);
            return Content(messaje);
        }
    }
}