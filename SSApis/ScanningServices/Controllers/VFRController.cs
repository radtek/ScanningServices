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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class VFRController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get VFR Information
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobID">
        /// Required Field. SMTP information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>VFR Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultSMTP))]
        [ActionName("GetVFRInfoByJobID")]
        public ActionResult GetVFRInfoByJobID(int jobID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultsVFR resultVFR = new GlobalVars.ResultsVFR();
            try
            {
                logger.Info("GetVFRInfoByJobID API Request. Job ID: " + jobID);
                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultVFR.ReturnCode = -1;
                    resultVFR.Message = "Missing argument Job ID";
                    logger.Warn("GetJobsByProjectID API Request ends with an Error.");
                    logger.Warn(resultVFR.Message);
                }
                else
                {
                    resultVFR = SQLFunctionsVFR.GetVFRInfoByJobID(jobID);
                    switch (resultVFR.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetVFRInfoByJobID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetVFRInfoByJobID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultVFR, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetVFRInfoByJobID API Request ends with a Fatal Error.");
                resultVFR.ReturnCode = -2;
                resultVFR.Message = e.Message;
                var baseException = e.GetBaseException();
                resultVFR.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultVFR, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultVFR.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultVFR, Formatting.Indented));
            resultVFR.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultVFR, Formatting.Indented);
            logger.Info("Leaving GetVFRInfoByJobID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update VFR Infor for a given Job or create a new one if it does not exist. 
        /// </summary>
        /// <remarks>
        /// The record is created if itr does not exist in the Database
        /// Sample Request:
        /// '{CADIUrl: "http://Cadi",SettingID: 0,JobID: 1,UserName: "lcarbone",Password: "qpwoeiruty",InstanceName: "Demo",RepositoryName: "DemoRepository",CaptureTemplate: "DemoTemplate",QueryField: "BatchName"}'
        /// </remarks>
        /// <param name="vfrJS">
        /// Required Field. VFR information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateVFRInfo")]
        public ActionResult UpdateVFRInfo([FromBody]string vfrJS)
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
                if (vfrJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument vfrJS";
                    logger.Warn("UpdateVFRInfo API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.VFR vfr = JsonConvert.DeserializeObject<GlobalVars.VFR>(vfrJS);
                    logger.Info("UpdateVFRInfo API Request.");
                    if (vfr.JobID != 0)
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(vfr, Formatting.Indented));

                        result = SQLFunctionsVFR.UpdateVFRInfo(vfr);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateVFRInfo API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("UpdateVFRInfo API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateVFRInfo API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Job ID value is not valid.";
                        logger.Warn("UpdateVFRInfo API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateVFRInfo API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateVFRInfo API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Delete VFR Infor for given Job
        /// </summary>
        /// <remarks>
        /// If the given Job ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="jobID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteVFRInfo")]
        public ActionResult DeleteVFRInfo(int jobID)
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
                // Job ID ID is a Required Field
                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Field ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteVFRInfo API Request. Job ID: " + jobID);
                    result = SQLFunctionsFields.DeleteField(jobID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteJob ends successfully
                            logger.Info("Job ID:  " + jobID + " was Deleted Successfully.");
                            result.Message = "Job ID:  " + jobID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteVFRInfo API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteProject ends with a Fatal error
                            logger.Fatal("DeleteVFRInfo API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteVFRInfo API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteVFRInfo API");
            //return Json(messaje);
            return Content(messaje);
        }
    }
}
