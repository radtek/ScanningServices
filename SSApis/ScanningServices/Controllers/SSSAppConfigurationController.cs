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

namespace ScanningServices.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class SSSAppConfigurationController : Controller
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Scanning Services Clien Configurtaion 
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Configuration File in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultBatches))]
        [ActionName("GetSSSAppConfiguration")]
        public ActionResult GetSSSAppConfiguration()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //SSSClientGlobalVars.ConfigSTR SSConfig = new SSSClientGlobalVars.ConfigSTR();
            SSSClientGlobalVars.ResultSSSConfig resultSSSConfig = new SSSClientGlobalVars.ResultSSSConfig();

            try
            {                
                logger.Info("GetSSSAppConfiguration API Request.");
                resultSSSConfig = SSSAppConfigurationFileBuilder.GetSSSConfig();
                switch (resultSSSConfig.ReturnCode)
                {
                    case 0:
                        logger.Info("GetSSSAppConfiguration API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetSSSAppConfiguration API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultSSSConfig, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetSSSAppConfiguration API Request ends with a Fatal Error.");
                resultSSSConfig.ReturnCode = -2;
                resultSSSConfig.Message = e.Message;
                var baseException = e.GetBaseException();
                resultSSSConfig.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultSSSConfig, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultSSSConfig.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultSSSConfig, Formatting.Indented));
            var messaje = JsonConvert.SerializeObject(resultSSSConfig, Formatting.Indented);
            logger.Info("Leaving GetSSSAppConfiguration API.");
            //return Json(messaje);
            return Content(messaje);
        }

    }
}