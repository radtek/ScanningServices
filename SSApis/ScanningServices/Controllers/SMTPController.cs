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
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class SMTPController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get SMTP Information
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>SMTP Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultSMTP))]
        [ActionName("GetSMTPInfo")]
        public ActionResult GetSMTPInfo()
        {
            // REMOVE THIS SECTION ONCE THE EMAIL NOTIFICATION HAS BEEN FULLY TESTED
            // Testing SendEmail
            //GlobalVars.EMAIL email = new GlobalVars.EMAIL();
            //email.Body = "This is a test message";
            //email.SenderEmailAddress = "lcarbone@cdlac.com";
            //email.RecipientsEmailAddress = "lcarbone@cdlac.com";
            //email.Subject = "Test Message sent from SSS Microservices";
            //GlobalVars.ResultGeneric resultGeneric = new GlobalVars.ResultGeneric();
            //resultGeneric = SQLFunctionsSMTP.SendEmail(email);
            // -------------------------------------------------------

            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultSMTP resultSMTP = new GlobalVars.ResultSMTP();
            try
            {
                logger.Info("GetSMTPInfo API Request.");
                resultSMTP = SQLFunctionsSMTP.GetSMTPInfo();
                switch (resultSMTP.ReturnCode)
                {
                    case 0:
                        logger.Info("GetSMTPInfo API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetSMTPInfo API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultSMTP, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetSMTPInfo API Request ends with a Fatal Error.");
                resultSMTP.ReturnCode = -2;
                resultSMTP.Message = e.Message;
                var baseException = e.GetBaseException();
                resultSMTP.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultSMTP, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultSMTP.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultSMTP, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultSMTP.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultSMTP, Formatting.Indented);
            logger.Info("Leaving GetSMTPInfo API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Send Email using a predefined SMTP Server information
        /// This Method assumes that the SMTP information was already set in the Database
        /// </summary>
        /// <remarks>
        /// '{RecipientsEmailAddress: "lcarbone@cdlac.com",Subject: "This is the subject of the Email", Body: "This is the content of the Email"}'
        /// </remarks>
        /// <param name="emailJS">Required Field. Email information in Json Format</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("SendEmail")]
        public ActionResult SendEmail([FromBody]string emailJS)      
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric();
            try
            {
                
                logger.Info("SendEmail API Request.");

                if (emailJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument emailJS";
                    logger.Warn("SendEmail API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);                   
                }
                else
                {
                    GlobalVars.EMAIL email = JsonConvert.DeserializeObject<GlobalVars.EMAIL>(emailJS.ToString());
                    if (!string.IsNullOrWhiteSpace(email.RecipientsEmailAddress))
                    {
                        result = SQLFunctionsSMTP.SendEmail(email);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("SendEmail API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("SendEmail API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Missing argument recipients";
                        logger.Warn("SendEmail API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }               
            }
            catch (Exception e)
            {
                logger.Fatal("SendEmail API Request ends with a Fatal Error.");
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
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            result.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(result, Formatting.Indented);
            logger.Info("Leaving SendEmail API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update SMTP Server Information. 
        /// </summary>
        /// <remarks>
        /// '{HostName: "smtp05.sherwebcloud.com",PortNumber: "587", EnableSslflag: "true", SenderEmailAddress: "lcarbone@cdlac.com", SenderName: "Leonardo Carbone", UserName: "lcarbone@cdlac.com", Password: "qpwoeiruty"}'
        /// </remarks>
        /// <param name="smtpJS">
        /// Required Field. SMTP information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateSMTP")]
        public ActionResult UpdateSMTP([FromBody]string smtpJS)
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
                if (smtpJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument smtpJS";
                    logger.Warn("UpdateSMTP API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.SMTP smtp = JsonConvert.DeserializeObject<GlobalVars.SMTP>(smtpJS);
                    logger.Info("UpdateSMTP API Request.");
                    logger.Debug("Parameter:" + JsonConvert.SerializeObject(smtp, Formatting.Indented));

                    //Rules:
                    // 1- HostName not null
                    // 2- PortNumber <> 0
                    // 3- EnableSslFlag not null
                    // 3- If SSL is true --> UserName and Password not null
                    // 4- Sender Email Address not null
                    // 5- Sender Name could be empty
                    if (!string.IsNullOrEmpty(smtp.HostName) & smtp.PortNumber != 0 & !string.IsNullOrEmpty(smtp.SenderEmailAddress) & !string.IsNullOrEmpty(smtp.EnableSSLFlag.ToString()))
                    {
                        if (smtp.EnableSSLFlag)
                        {
                            if (!string.IsNullOrEmpty(smtp.UserName) & !string.IsNullOrEmpty(smtp.Password))
                            {
                                // Good to go
                            }
                            else
                            {
                                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                result.ReturnCode = -1;
                                result.Message = "When SSL Flag is true, you must provide no empty values for UserName and Password";
                                logger.Warn("UpdateSMTP API Request ends with a Fatal Error.");
                                logger.Warn(result.Message);
                            }
                        }
                        else
                        {
                            // Good to go
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "HostName, PortNumber, and SenderEmailAddress must have a value. HostName not null, PortNumber > 0, SenderEmailAddress not null, and EnableSslflag {true, false}";
                        logger.Warn("UpdateSMTP API Request ends with a Fatal Error.");
                        logger.Warn(result.Message);
                    }
                    if (result.ReturnCode == 0)
                    {
                        result = SQLFunctionsSMTP.UpdateSMTP(smtp);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateSMTP API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateSMTP API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateSMTP API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateSMTP API");
            //return Json(messaje);
            return Content(messaje);
        }

    }
}
