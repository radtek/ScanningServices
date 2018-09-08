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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ProcessController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Scheduled Job Processes
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Scheduled Job Processes in Json format</returns>
        //[HttpGet]
        //[DefaultValue(false)]
        //[Produces("application/json", Type = typeof(GlobalVars.ResultJobProcesses))]
        //[ActionName("GetJobProcesses")]
        //public ActionResult GetJobProcesses()
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    GlobalVars.ResultJobProcesses resultProcesses = new GlobalVars.ResultJobProcesses();
        //    try
        //    {
        //        logger.Info("GetJobProcesses API Request.");

        //        resultProcesses = SQLFunctionsProcesses.GetJobProcesses();
        //        switch (resultProcesses.ReturnCode)
        //        {
        //            case 0:
        //                logger.Info("GetJobProcesses API Request was executed Successfully.");
        //                Response.StatusCode = (int)HttpStatusCode.OK;
        //                break;

        //            case -2:
        //                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                logger.Fatal("GetJobProcesses API Request ends with a Fatal Error.");
        //                logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcesses, Formatting.Indented));
        //                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                break;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Fatal("GetJobProcesses API Request ends with a Fatal Error.");
        //        resultProcesses.ReturnCode = -2;
        //        resultProcesses.Message = e.Message;
        //        var baseException = e.GetBaseException();
        //        resultProcesses.Exception = baseException.ToString();
        //        logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcesses, Formatting.Indented));
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    }
        //    Response.ContentType = "application/json";
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    elapsedMs = elapsedMs / 1000;
        //    resultProcesses.ElapsedTime = elapsedMs.ToString();
        //    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcesses, Formatting.Indented));
        //    //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
        //    resultProcesses.HttpStatusCode = Response.StatusCode.ToString();
        //    var messaje = JsonConvert.SerializeObject(resultProcesses, Formatting.Indented);
        //    logger.Info("Leaving GetJobProcesses API.");
        //    //return Json(messaje);
        //    return Content(messaje);
        //}

        


        /// <summary>
        /// Get Scheduled Job Process Info by Process ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="processID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Scheduled Job Process information in Json format</returns>
        //[HttpGet]
        //[DefaultValue(false)]
        //[Produces("application/json", Type = typeof(GlobalVars.ResultJobProcesses))]
        //[ActionName("GetJobProcessByID")]
        //public ActionResult GetJobProcessByID(int processID)
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    GlobalVars.ResultJobProcesses resultProcess = new GlobalVars.ResultJobProcesses();
        //    try
        //    {
        //        logger.Info("GetJobProcessByID API Request. Process ID: " + processID);
        //        if (processID == 0)
        //        {
        //            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            resultProcess.ReturnCode = -1;
        //            resultProcess.Message = "Missing argument Process ID";
        //            logger.Warn("GetJobProcessByID API Request ends with an Error.");
        //            logger.Warn(resultProcess.Message);
        //        }
        //        else
        //        {
        //            resultProcess = SQLFunctionsProcesses.GetJobProcessByID(processID);
        //            switch (resultProcess.ReturnCode)
        //            {
        //                case 0:
        //                    logger.Info("GetJobProcessByID API Request was executed Successfully.");
        //                    Response.StatusCode = (int)HttpStatusCode.OK;
        //                    break;

        //                case -2:
        //                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                    logger.Fatal("GetJobProcessByID API Request ends with a Fatal Error.");
        //                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Fatal("GetJobProcessByID API Request ends with a Fatal Error.");
        //        resultProcess.ReturnCode = -2;
        //        resultProcess.Message = e.Message;
        //        var baseException = e.GetBaseException();
        //        resultProcess.Exception = baseException.ToString();
        //        logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    }
        //    Response.ContentType = "application/json";
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    elapsedMs = elapsedMs / 1000;
        //    resultProcess.ElapsedTime = elapsedMs.ToString();
        //    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //    resultProcess.HttpStatusCode = Response.StatusCode.ToString();
        //    var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
        //    logger.Info("Leaving GetJobProcessByID API.");
        //    //return Json(messaje);
        //    return Content(messaje);
        //}

        /// <summary>
        /// Get Scheduled Job Processes Info by a given Process Name
        /// </summary>
        /// <remarks>
        /// This Method needs to be rebuilt since JobProcess Table not longer has Process Name
        /// The implemenation needs to traverse the Process Type Table and look for Process Name
        /// </remarks>
        /// <param name="processName">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Process information in Json format</returns>
        //[HttpGet]
        //[DefaultValue(false)]
        //[Produces("application/json", Type = typeof(GlobalVars.ResultJobProcesses))]
        //[ActionName("GetJobProcessByName")]
        //public ActionResult GetJobProcessByName(string processName)
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    GlobalVars.ResultJobProcesses resultProcess = new GlobalVars.ResultJobProcesses();
        //    try
        //    {
        //        logger.Info("GetJobProcessByName API Request. Process Name: " + processName);
        //        if (string.IsNullOrEmpty(processName))
        //        {
        //            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            resultProcess.ReturnCode = -1;
        //            resultProcess.Message = "Missing argument Process Name";
        //            logger.Warn("GetJobProcessByName API Request ends with an Error.");
        //            logger.Warn(resultProcess.Message);
        //        }
        //        else
        //        {
        //            resultProcess = SQLFunctionsProcesses.GetJobProcessByName(processName);
        //            switch (resultProcess.ReturnCode)
        //            {
        //                case 0:
        //                    logger.Info("GetJobProcessByName API Request was executed Successfully.");
        //                    Response.StatusCode = (int)HttpStatusCode.OK;
        //                    break;

        //                case -2:
        //                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                    logger.Fatal("GetJobProcessByName API Request ends with a Fatal Error.");
        //                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Fatal("GetJobProcessByName API Request ends with a Fatal Error.");
        //        resultProcess.ReturnCode = -2;
        //        resultProcess.Message = e.Message;
        //        var baseException = e.GetBaseException();
        //        resultProcess.Exception = baseException.ToString();
        //        logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    }
        //    Response.ContentType = "application/json";
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    elapsedMs = elapsedMs / 1000;
        //    resultProcess.ElapsedTime = elapsedMs.ToString();
        //    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //    resultProcess.HttpStatusCode = Response.StatusCode.ToString();
        //    var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
        //    logger.Info("Leaving GetJobProcessByName API.");
        //    //return Json(messaje);
        //    return Content(messaje);
        //}


        /// <summary>
        /// Get Process Info by Job ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Process information in Json format</returns>
        //[HttpGet]
        //[DefaultValue(false)]
        //[Produces("application/json", Type = typeof(GlobalVars.ResultJobProcesses))]
        //[ActionName("GetJobProcessByJobID")]
        //public ActionResult GetJobProcessByJobID(int jobID)
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    GlobalVars.ResultJobProcesses resultProcess = new GlobalVars.ResultJobProcesses();
        //    try
        //    {
        //        logger.Info("GetJobProcessByJobID API Request. Job ID: " + jobID);
        //        if (jobID == 0)
        //        {
        //            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            resultProcess.ReturnCode = -1;
        //            resultProcess.Message = "Missing argument Job ID";
        //            logger.Warn("GetJobProcessByJobID API Request ends with an Error.");
        //            logger.Warn(resultProcess.Message);
        //        }
        //        else
        //        {
        //            resultProcess = SQLFunctionsProcesses.GetJobProcessByJobID(jobID);
        //            switch (resultProcess.ReturnCode)
        //            {
        //                case 0:
        //                    logger.Info("GetJobProcessByJobID API Request was executed Successfully.");
        //                    Response.StatusCode = (int)HttpStatusCode.OK;
        //                    break;

        //                case -2:
        //                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                    logger.Fatal("GetJobProcessByJobID API Request ends with a Fatal Error.");
        //                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Fatal("GetJobProcessByJobID API Request ends with a Fatal Error.");
        //        resultProcess.ReturnCode = -2;
        //        resultProcess.Message = e.Message;
        //        var baseException = e.GetBaseException();
        //        resultProcess.Exception = baseException.ToString();
        //        logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    }
        //    Response.ContentType = "application/json";
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    elapsedMs = elapsedMs / 1000;
        //    resultProcess.ElapsedTime = elapsedMs.ToString();
        //    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //    resultProcess.HttpStatusCode = Response.StatusCode.ToString();
        //    var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
        //    logger.Info("Leaving GetJobProcessByJobID API.");
        //    //return Json(messaje);
        //    return Content(messaje);
        //}


        /// <summary>
        /// Get Process Info by Process Name and Job ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="processName">
        /// Required Field.
        /// </param>
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Process information in Json format</returns>
        //[HttpGet]
        //[DefaultValue(false)]
        //[Produces("application/json", Type = typeof(GlobalVars.ResultJobProcesses))]
        //[ActionName("GetJobProcessByNameAndJobID")]
        //public ActionResult GetJobProcessByNameAndJobID(string processName, int jobID)
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    GlobalVars.ResultJobProcesses resultProcess = new GlobalVars.ResultJobProcesses();
        //    try
        //    {
        //        logger.Info("GetJobProcessByNameAndJobID API Request. Process Name: " + processName);
        //        if (string.IsNullOrEmpty(processName))
        //        {
        //            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            resultProcess.ReturnCode = -1;
        //            resultProcess.Message = "Missing argument Process Name";
        //            logger.Warn("GetJobProcessByNameAndJobID API Request ends with an Error.");
        //            logger.Warn(resultProcess.Message);
        //        }
        //        else
        //        {
        //            logger.Info("GetJobProcessByNameAndJobID API Request. Job ID: " + jobID);
        //            if (jobID == 0)
        //            {
        //                Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //                resultProcess.ReturnCode = -1;
        //                resultProcess.Message = "Missing argument Job ID";
        //                logger.Warn("GetJobProcessByNameAndJobID API Request ends with an Error.");
        //                logger.Warn(resultProcess.Message);
        //            }
        //            else
        //            {
        //                resultProcess = SQLFunctionsProcesses.GetJobProcessByNameAndJobID(processName, jobID);
        //                switch (resultProcess.ReturnCode)
        //                {
        //                    case 0:
        //                        logger.Info("GetJobProcessByNameAndJobID API Request was executed Successfully.");
        //                        Response.StatusCode = (int)HttpStatusCode.OK;
        //                        break;

        //                    case -2:
        //                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                        logger.Fatal("GetJobProcessByNameAndJobID API Request ends with a Fatal Error.");
        //                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                        break;
        //                }
        //            }                    
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Fatal("GetJobProcessByNameAndJobID API Request ends with a Fatal Error.");
        //        resultProcess.ReturnCode = -2;
        //        resultProcess.Message = e.Message;
        //        var baseException = e.GetBaseException();
        //        resultProcess.Exception = baseException.ToString();
        //        logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    }
        //    Response.ContentType = "application/json";
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    elapsedMs = elapsedMs / 1000;
        //    resultProcess.ElapsedTime = elapsedMs.ToString();
        //    logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
        //    resultProcess.HttpStatusCode = Response.StatusCode.ToString();
        //    var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
        //    logger.Info("Leaving GetJobProcessByNameAndJobID API.");
        //    //return Json(messaje);
        //    return Content(messaje);
        //}


        /// <summary>
        /// Update Job Process info for a given Process or create a new one if the Job Process does not exist.
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{"ProcessID": 0,"JobID": 20,"Name": "Synchronizer","Description": null,"Schedule": {"dailyFlag": false,"recurEveryDays": "\","dayOfTheWeekFlag": true,"monday": false,"tuesday": false,"wednesday": false,"thursday": true,"friday": false,"saturday": true,"sunday": true,"repeatTaskFlag": true,"repeatEveryHoursFlag": true,"repeatEveryMinutesFlag": false,"repeatTaskTimes": "1","repeatTaskRange": true,"taskBeginHour": "1","taskEndHour": "5","startTaskAtFlag": false,"startTaskHour": "0","startTaskMinute": "0" },"ScheduleCronFormat": null,"WatchFolder": "\","TargetFolder": "\","EnableFlag": true,"StationName": "\"}'
        /// </remarks>
        /// <param name="processJS">
        /// Required Field. Process information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        //[HttpPost]
        //[ActionName("UpdateJobProcess")]
        //public ActionResult UpdateJobProcess([FromBody]string processJS)
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
        //    {
        //        Message = "",
        //        ReturnCode = 0,
        //        //ReturnValue = ""
        //    };
        //    try
        //    {
        //        if (processJS == null)
        //        {
        //            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            result.ReturnCode = -1;
        //            result.Message = "Missing argument processJS";
        //            logger.Warn("UpdateJobProcess API Request ends with a Fatal Error.");
        //            logger.Warn(result.Message);
        //        }
        //        else
        //        {
        //            GlobalVars.JobProcess process = JsonConvert.DeserializeObject<GlobalVars.JobProcess>(processJS);
        //            logger.Info("UpdateJobProcess API Request.");
        //            logger.Debug("Parameter:" + JsonConvert.SerializeObject(processJS, Formatting.Indented));

        //            //Rules:
        //            // 1- Process ID not Null
        //            // 2- Job ID not Null
        //            // 3- Process Name not Null
        //            //if (process.ProcessID == 0)
        //            //{
        //            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            //    result.ReturnCode = -1;
        //            //    result.Message = "You must provide a Valid Process ID.";
        //            //    logger.Warn("UpdateProcess API Request ends with a Fatal Error.");
        //            //    logger.Warn(result.Message);
        //            //}
        //            //else
        //            //{
        //                if (process.JobID == 0)
        //                {
        //                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //                    result.ReturnCode = -1;
        //                    result.Message = "You must provide a Valid Job ID.";
        //                    logger.Warn("UpdateJobProcess API Request ends with a Fatal Error.");
        //                    logger.Warn(result.Message);
        //                }
        //                else
        //                {
        //                    if (string.IsNullOrEmpty(process.Name))
        //                    {
        //                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //                        result.ReturnCode = -1;
        //                        result.Message = "You must provide a Valid Process Name.";
        //                        logger.Warn("UpdateJobProcess API Request ends with a Fatal Error.");
        //                        logger.Warn(result.Message);
        //                    }
        //                    else
        //                    {
        //                        result = SQLFunctionsProcesses.UpdateJobProcess(process);
        //                        switch (result.ReturnCode)
        //                        {
        //                            case 0:
        //                                logger.Info("UpdateJobProcess API Request was executed Successfully.");
        //                                Response.StatusCode = (int)HttpStatusCode.OK;
        //                                break;

        //                            case -2:
        //                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                                logger.Fatal("UpdateJobProcess API Request ends with a Fatal Error.");
        //                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
        //                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //                                break;
        //                        }
        //                    }                            
        //                }                        
        //            //}
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Fatal("UpdateJobProcess API Request ends with a Fatal Error.");
        //        result.ReturnCode = -2;
        //        result.Message = e.Message;
        //        var baseException = e.GetBaseException();
        //        result.Exception = baseException.ToString();
        //        logger.Fatal("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
        //        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    }
        //    Response.ContentType = "application/json";
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    elapsedMs = elapsedMs / 1000;
        //    result.ElapsedTime = elapsedMs.ToString();
        //    result.HttpStatusCode = Response.StatusCode.ToString();
        //    var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
        //    logger.Info("Leaving UpdateJobProcess API");
        //    //return Json(messaje);
        //    return Content(messaje);
        //}


        /// <summary>
        /// Get Process Types
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Process Types in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProcessTypes))]
        [ActionName("GetProcessTypes")]
        public ActionResult GetProcessTypes()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProcessTypes resultProcessTypes = new GlobalVars.ResultProcessTypes();
            try
            {
                logger.Info("GetProcessTypes API Request.");

                resultProcessTypes = SQLFunctionsProcesses.GetProcessTypes();
                switch (resultProcessTypes.ReturnCode)
                {
                    case 0:
                        logger.Info("GetProcessTypes API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetProcessTypes API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcessTypes, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProcessTypes API Request ends with a Fatal Error.");
                resultProcessTypes.ReturnCode = -2;
                resultProcessTypes.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcessTypes.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcessTypes, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProcessTypes.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcessTypes, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultProcessTypes.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProcessTypes, Formatting.Indented);
            logger.Info("Leaving GetProcessTypes API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Scheduled Master Processes
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Scheduled Master Processes in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProcesses))]
        [ActionName("GetProcesses")]
        public ActionResult GetProcesses()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProcesses resultProcesses = new GlobalVars.ResultProcesses();
            try
            {
                logger.Info("GetProcesses API Request.");

                resultProcesses = SQLFunctionsProcesses.GetProcesses();
                switch (resultProcesses.ReturnCode)
                {
                    case 0:
                        logger.Info("GetProcesses API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetProcesses API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcesses, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProcesses API Request ends with a Fatal Error.");
                resultProcesses.ReturnCode = -2;
                resultProcesses.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcesses.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcesses, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProcesses.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcesses, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultProcesses.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProcesses, Formatting.Indented);
            logger.Info("Leaving GetProcesses API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Scheduled Processes Info by Process ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="processID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns> Scheduled Process information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProcesses))]
        [ActionName("GetProcessesByID")]
        public ActionResult GetProcessesByID(int processID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProcesses resultProcess = new GlobalVars.ResultProcesses();
            try
            {
                logger.Info("GetProcessesByID API Request. Process ID: " + processID);
                if (processID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProcess.ReturnCode = -1;
                    resultProcess.Message = "Missing argument Process ID";
                    logger.Warn("GetProcessesByID API Request ends with an Error.");
                    logger.Warn(resultProcess.Message);
                }
                else
                {
                    resultProcess = SQLFunctionsProcesses.GetProcessesByID(processID);
                    switch (resultProcess.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetProcessesByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetProcessesByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProcessesByID API Request ends with a Fatal Error.");
                resultProcess.ReturnCode = -2;
                resultProcess.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcess.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProcess.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
            resultProcess.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
            logger.Info("Leaving GetProcessesByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Scheduled Processes Info by Job ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="jobID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns> Scheduled Process information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProcesses))]
        [ActionName("GetProcessesByJobID")]
        public ActionResult GetProcessesByJobID(int jobID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProcesses resultProcess = new GlobalVars.ResultProcesses();
            try
            {
                logger.Info("GetProcessesByID API Request. Job ID: " + jobID);
                if (jobID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProcess.ReturnCode = -1;
                    resultProcess.Message = "Missing argument Job ID";
                    logger.Warn("GetProcessesByJobID API Request ends with an Error.");
                    logger.Warn(resultProcess.Message);
                }
                else
                {
                    resultProcess = SQLFunctionsProcesses.GetProcessesByJobID(jobID);
                    switch (resultProcess.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetProcessesByJobID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetProcessesByJobID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProcessesByJobID API Request ends with a Fatal Error.");
                resultProcess.ReturnCode = -2;
                resultProcess.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcess.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProcess.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
            resultProcess.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
            logger.Info("Leaving GetProcessesByJobID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Scheduled Processes Info by IDs
        /// jobID = 0 means Any Job or all Jobs
        /// pdfStationID = 0 is used when there a record does not has pdf station
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="processID">
        /// <param name="jobID">
        /// <param name="stationID">
        /// <param name="pdfStationID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns> Scheduled Process information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProcesses))]
        [ActionName("GetProcessesByIDs")]
        public ActionResult GetProcessesByIDs(int processID, int jobID, int stationID, int pdfStationID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProcesses resultProcess = new GlobalVars.ResultProcesses();
            try
            {
                logger.Info("GetProcessesByIDs API Request. Process ID: " + processID);
                if (processID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProcess.ReturnCode = -1;
                    resultProcess.Message = "Missing argument Process ID";
                    logger.Warn("GetProcessesByIDs API Request ends with an Error.");
                    logger.Warn(resultProcess.Message);
                }
                else
                {
                    if (stationID == 0)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        resultProcess.ReturnCode = -1;
                        resultProcess.Message = "Missing argument Station ID";
                        logger.Warn("GetProcessesByIDs API Request ends with an Error.");
                        logger.Warn(resultProcess.Message);
                    }
                    else
                    {
                        resultProcess = SQLFunctionsProcesses.GetProcessesByIDs(processID, jobID, stationID, pdfStationID);
                        switch (resultProcess.ReturnCode)
                        {
                            case 0:
                                logger.Info("GetProcessesByIDs API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("GetProcessesByIDs API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProcessesByIDs API Request ends with a Fatal Error.");
                resultProcess.ReturnCode = -2;
                resultProcess.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcess.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProcess.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
            resultProcess.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
            logger.Info("Leaving GetProcessesByIDs API.");
            //return Json(messaje);
            return Content(messaje);
        }
        /// <summary>
        /// Get Scheduled Processes Info by a given Process Name
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="processName">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Process information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProcesses))]
        [ActionName("GetProcessesByName")]
        public ActionResult GetProcessesByName(string processName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProcesses resultProcess = new GlobalVars.ResultProcesses();
            try
            {
                logger.Info("GetProcessesByName API Request. Process Name: " + processName);
                if (string.IsNullOrEmpty(processName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProcess.ReturnCode = -1;
                    resultProcess.Message = "Missing argument Process Name";
                    logger.Warn("GetProcessesByName API Request ends with an Error.");
                    logger.Warn(resultProcess.Message);
                }
                else
                {
                    resultProcess = SQLFunctionsProcesses.GetProcessesByName(processName);
                    switch (resultProcess.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetProcessesByName API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetProcessesByName API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProcessesByName API Request ends with a Fatal Error.");
                resultProcess.ReturnCode = -2;
                resultProcess.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcess.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProcess.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
            resultProcess.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
            logger.Info("Leaving GetProcessesByName API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Process Info by Station ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="stationID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Process information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProcesses))]
        [ActionName("GetProcessesByStationID")]
        public ActionResult GetProcessesByStationID(int stationID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProcesses resultProcess = new GlobalVars.ResultProcesses();
            try
            {
                logger.Info("GetProcessesByStationID API Request. Station ID: " + stationID);
                if (stationID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProcess.ReturnCode = -1;
                    resultProcess.Message = "Missing argument Job ID";
                    logger.Warn("GetProcessesByStationID API Request ends with an Error.");
                    logger.Warn(resultProcess.Message);
                }
                else
                {
                    resultProcess = SQLFunctionsProcesses.GetProcessesByStationID(stationID);
                    switch (resultProcess.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetProcessesByStationID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetProcessesByStationID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProcessesByStationID API Request ends with a Fatal Error.");
                resultProcess.ReturnCode = -2;
                resultProcess.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProcess.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProcess.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProcess, Formatting.Indented));
            resultProcess.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProcess, Formatting.Indented);
            logger.Info("Leaving GetProcessesByStationID API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Next Available File Conversion Station
        /// </summary>
        /// <remarks>
        /// <para> - The selection is based on the available Capacity of a File Conversion Station. The one that has the most capacity will be selected. If there is more than one Station with the same capacity, one of them will be selected. Future implementation will consider the number of files each one of these File Conversion Stations needs to process, so the one that has less, will be the one selected.</para>
        /// <para> - The StringReturnValue tag returns the File Conversion's Watch Folder selected. This is the base location to be used for the Load Balancer. If an empty value is returned on this field, means that there is not File Conversion Stataion that can process a new request at the moment.</para>
        /// <para> - The IntegerNumberReturnValue tag returns the current capacity of the File Conversion Station.</para>
        /// <para> - Use GlobalVars.ResultGeneric data object to Serialize the information returned by this method.</para>
        /// </remarks>
        /// <param>
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Process information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultGeneric))]
        [ActionName("GetNextAvailableFileConversionStation")]
        public ActionResult GetNextAvailableFileConversionStation()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric();
            try
            {
                logger.Info("GetNextAvailableFileConversionStation API Request. ");

                result = SQLFunctionsProcesses.GetNextAvailableFileConversionStation();
                switch (result.ReturnCode)
                {
                    case 0:
                        logger.Info("GetNextAvailableFileConversionStation API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetNextAvailableFileConversionStation API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

            }
            catch (Exception e)
            {
                logger.Fatal("GetNextAvailableFileConversionStation API Request ends with a Fatal Error.");
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
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving GetNextAvailableFileConversionStation API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update Process info for a given Process or create a new one if the Master Process does not exist.
        /// </summary>
        /// <remarks>
        /// TO DO .... We should consider to take the Station ID out as Key from the Process Database Table
        /// Sample Request:
        /// '{"ProcessID": 0,"JobID": 20,"Name": "Synchronizer","Description": null,"Schedule": {"dailyFlag": false,"recurEveryDays": "\","dayOfTheWeekFlag": true,"monday": false,"tuesday": false,"wednesday": false,"thursday": true,"friday": false,"saturday": true,"sunday": true,"repeatTaskFlag": true,"repeatEveryHoursFlag": true,"repeatEveryMinutesFlag": false,"repeatTaskTimes": "1","repeatTaskRange": true,"taskBeginHour": "1","taskEndHour": "5","startTaskAtFlag": false,"startTaskHour": "0","startTaskMinute": "0" },"ScheduleCronFormat": null,"WatchFolder": "\","TargetFolder": "\","EnableFlag": true,"StationName": "\"}'
        /// </remarks>
        /// <param name="masterProcessJS">
        /// Required Field. Process information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateProcess")]
        public ActionResult UpdateProcess([FromBody]string masterProcessJS)
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
                if (masterProcessJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument masterProcessJS";
                    logger.Warn("UpdateProcess API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.ProcessUpdate process = JsonConvert.DeserializeObject<GlobalVars.ProcessUpdate>(masterProcessJS);
                    logger.Info("UpdateProcess API Request.");
                    logger.Debug("Parameter:" + JsonConvert.SerializeObject(masterProcessJS, Formatting.Indented));

                    result = SQLFunctionsProcesses.UpdateProcess(process);
                    switch (result.ReturnCode)
                    {
                        case 0:
                            logger.Info("UpdateProcess API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("UpdateProcess API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateProcess API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateProcess API");
            //return Json(messaje);
            return Content(messaje);
        }
    }
}
