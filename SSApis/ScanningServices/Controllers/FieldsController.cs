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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanningServices.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class FieldsController : Controller
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Fields by a given Job ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Job's Fields in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultFields))]
        [ActionName("GetFieldsByJobID")]
        public ActionResult GetFieldsByJobID(int jobID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultFields resultFields = new GlobalVars.ResultFields();
            try
            {
                logger.Info("GetFieldsByJobID API Request. Job ID: " + jobID);
                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultFields.ReturnCode = -1;
                    resultFields.Message = "Missing argument Job ID";
                    logger.Warn("GetJobsByProjectID API Request ends with an Error.");
                    logger.Warn(resultFields.Message);
                }
                else
                {
                    resultFields = SQLFunctionsFields.GetFieldsByJobID(jobID);
                    switch (resultFields.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetFieldsByJobID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetFieldsByJobID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetFieldsByJobID API Request ends with a Fatal Error.");
                resultFields.ReturnCode = -2;
                resultFields.Message = e.Message;
                var baseException = e.GetBaseException();
                resultFields.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultFields.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
            resultFields.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultFields, Formatting.Indented);
            logger.Info("Leaving GetFieldsByJobID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Field Information b y Field Name and Job ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="fieldName">
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Job's Fields in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultFields))]
        [ActionName("GetFieldsByNameAndJobID")]
        public ActionResult GetFieldsByNameAndJobID(string fieldName, int jobID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultFields resultFields = new GlobalVars.ResultFields();
            try
            {
                logger.Info("GetFieldsByNameAndJobID API Request. Field Name: " + fieldName + " and Job ID: " + jobID);
                if (jobID == 0 || string.IsNullOrEmpty(fieldName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultFields.ReturnCode = -1;
                    resultFields.Message = "Missing argument Job ID and/or Capture Pro Field Name";
                    logger.Warn("GetFieldsByNameAndJobID API Request ends with an Error.");
                    logger.Warn(resultFields.Message);
                }
                else
                {
                    resultFields = SQLFunctionsFields.GetFieldsByNameAndJobID(fieldName, jobID);
                    switch (resultFields.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetFieldsByNameAndJobID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetFieldsByNameAndJobID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetFieldsByNameAndJobID API Request ends with a Fatal Error.");
                resultFields.ReturnCode = -2;
                resultFields.Message = e.Message;
                var baseException = e.GetBaseException();
                resultFields.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultFields.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
            resultFields.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultFields, Formatting.Indented);
            logger.Info("Leaving GetFieldsByNameAndJobID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Fields by a given Field ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="fieldID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Job's Fields in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultFields))]
        [ActionName("GetFieldByID")]
        public ActionResult GetFieldByID(int fieldID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultFields resultFields = new GlobalVars.ResultFields();
            try
            {
                logger.Info("GetFieldByID API Request. Job ID: " + fieldID);
                if (fieldID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultFields.ReturnCode = -1;
                    resultFields.Message = "Missing argument Field ID";
                    logger.Warn("GetFieldByID API Request ends with an Error.");
                    logger.Warn(resultFields.Message);
                }
                else
                {
                    resultFields = SQLFunctionsFields.GetFieldByID(fieldID);
                    switch (resultFields.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetFieldByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetFieldByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetFieldByID API Request ends with a Fatal Error.");
                resultFields.ReturnCode = -2;
                resultFields.Message = e.Message;
                var baseException = e.GetBaseException();
                resultFields.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultFields.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultFields, Formatting.Indented));
            resultFields.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultFields, Formatting.Indented);
            logger.Info("Leaving GetFieldByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update a given Field. 
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{JobID: 1,FieldID: 5,CPFieldName: "Address",VFRFieldName: "Address",ExcludeFromKeystrokesCount: false}'
        /// </remarks>
        /// <param name="fieldJS">
        /// Required Field. Field information in a Json Format.
        /// The Field Name must be unique inthe entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateField")]
        public ActionResult UpdateField([FromBody]string fieldJS)
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
                if (fieldJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument fieldJS";
                    logger.Warn("UpdateField API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Field field = JsonConvert.DeserializeObject<GlobalVars.Field>(fieldJS);
                    logger.Info("UpdateField API Request.");
                    if (!string.IsNullOrEmpty(field.CPFieldName))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(field, Formatting.Indented));

                        result = SQLFunctionsFields.UpdateField(field);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateField API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("UpdateField API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateField API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "CPFielName value is not valid.";
                        logger.Warn("UpdateField API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateField API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateField API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Create a new Job's Field
        /// </summary>
        /// /// <remarks>
        /// Sample Request:
        /// '{JobID: "1",FieldID: "0",CPFieldName:"BatchName", VFRFieldName: "VFRDoc",ExcludeFromKeystrokesCount: "false"}'
        /// </remarks>
        /// <param name="fieldJS">
        /// Required Field. Field information must be provided in Json Format.
        /// The Field Name must be unique withing the Job.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewField")]
        public ActionResult NewField([FromBody]string fieldJS)
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
                if (fieldJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument fieldJS";
                    logger.Warn("NewField API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Field field = JsonConvert.DeserializeObject<GlobalVars.Field>(fieldJS);
                    logger.Info("NewField API Request.");
                    if (!string.IsNullOrEmpty(field.CPFieldName) & field.FieldID == 0)
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(field, Formatting.Indented));

                        result = SQLFunctionsFields.NewField(field);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewField API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewField API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewField API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Field Name and/or Field ID value is not valid.";
                        logger.Warn("NewField API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewField API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewField API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Delete Job's Field by a given ID
        /// </summary>
        /// <remarks>
        /// If the given Filed ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="fieldID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteField")]
        public ActionResult DeleteField(int fieldID)
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
                // Field ID is a Required Field
                if (fieldID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Field ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteField API Request. Job ID: " + fieldID);
                    result = SQLFunctionsFields.DeleteField(fieldID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteJob ends successfully
                            logger.Info("Field ID:  " + fieldID + " was Deleted Successfully.");
                            result.Message = "Field ID:  " + fieldID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteField API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteProject ends with a Fatal error
                            logger.Fatal("DeleteField API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteField API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteField API");
            //return Json(messaje);
            return Content(messaje);
        }

    }
}
