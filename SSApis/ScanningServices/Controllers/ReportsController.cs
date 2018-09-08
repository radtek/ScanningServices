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
    public class ReportsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Get System Reports Template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of System Reports Templates in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultReportsTemplate))]
        [ActionName("GetSystemReportsTemplate")]
        public ActionResult GetSystemReportsTemplate()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultReportsTemplate resultReportsTemplate = new GlobalVars.ResultReportsTemplate();
            try
            {
                logger.Info("GetSystemReportsTemplate API Request.");
                
                resultReportsTemplate = SQLFunctionsReports.GetReportsTemplate("System");
                switch (resultReportsTemplate.ReturnCode)
                {
                    case 0:
                        logger.Info("GetSystemReportsTemplate API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetSystemReportsTemplate API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

            }
            catch (Exception e)
            {
                logger.Fatal("GetSystemReportsTemplate API Request ends with a Fatal Error.");
                resultReportsTemplate.ReturnCode = -2;
                resultReportsTemplate.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReportsTemplate.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultReportsTemplate.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultReportsTemplate.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented);
            logger.Info("Leaving GetSystemReportsTemplate API.");
            //return Json(messaje);
            return Content(messaje);
        }

       

        /// <summary>
        /// Get Reports by a given Customer Name
        /// </summary>
        /// <param name="customerName">Required Field</param>
        /// <response code="200">Ok</response>
        /// <returns>Customer's Reports Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultReports))]
        [ActionName("GetReportsByCustomerName")]
        public ActionResult GetReportsByCustomerName(string customerName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultReports resultReports = new GlobalVars.ResultReports();
            try
            {
                logger.Info("GetReportsByCustomerName API Request.");

                if (string.IsNullOrEmpty(customerName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultReports.ReturnCode = -1;
                    resultReports.Message = "Missing argument customerName";
                    logger.Warn("GetReportsByCustomerName API Request ends with an Error.");
                    logger.Warn(resultReports.Message);
                }
                else
                {
                    resultReports = SQLFunctionsReports.GetReportsByCustomerName(customerName);
                    switch (resultReports.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetReportsByCustomerName API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetReportsByCustomerName API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReports, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetReportsByCustomerName API Request ends with a Fatal Error.");
                resultReports.ReturnCode = -2;
                resultReports.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReports.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultReports, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultReports.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReports, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultReports.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultReports, Formatting.Indented);
            logger.Info("Leaving GetReportsByCustomerName API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Report Template by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="templateID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Report Template Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultReportsTemplate))]
        [ActionName("GetReportTemplateByID")]
        public ActionResult GetReportTemplateByID(int templateID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultReportsTemplate resultReportsTemplate = new GlobalVars.ResultReportsTemplate();
            try
            {
                logger.Info("GetReportTemplateByID API Request. Template ID: " + templateID);
                
                if (templateID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultReportsTemplate.ReturnCode = -1;
                    resultReportsTemplate.Message = "Missing argument Template ID";
                    logger.Warn("GetReportTemplateByID API Request ends with an Error.");
                    logger.Warn(resultReportsTemplate.Message);
                }
                else
                {
                    resultReportsTemplate = SQLFunctionsReports.GetReportTemplateByID(templateID);
                    switch (resultReportsTemplate.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetReportTemplateByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetReportTemplateByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }   
            }
            catch (Exception e)
            {
                logger.Fatal("GetReportTemplateByID API Request ends with a Fatal Error.");
                resultReportsTemplate.ReturnCode = -2;
                resultReportsTemplate.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReportsTemplate.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultReportsTemplate.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultReportsTemplate.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented);
            logger.Info("Leaving GetReportTemplateByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Reports
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>Report Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.Report))]
        [ActionName("GetReports")]
        public ActionResult GetReports()
        {
            // The following section is just a test for the reporting
            //GlobalVars.ResultGeneric rg = new GlobalVars.ResultGeneric();
            // rg = SQLFunctionsReports.OverallBatchStatusReport(reportID);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultReports resultReport = new GlobalVars.ResultReports();
            try
            {
                logger.Info("GetReports API Request.");

                resultReport = SQLFunctionsReports.GetReports();
                switch (resultReport.ReturnCode)
                {
                    case 0:
                        logger.Info("GetReports API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetReports API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetReports API Request ends with a Fatal Error.");
                resultReport.ReturnCode = -2;
                resultReport.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReport.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultReport.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultReport.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultReport, Formatting.Indented);
            logger.Info("Leaving GetReports API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Report by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="reportID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Report Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.Report))]
        [ActionName("GetReportByID")]
        public ActionResult GetReportByID(int reportID)
        {
            // The following section is just a test for the reporting
            //GlobalVars.ResultGeneric rg = new GlobalVars.ResultGeneric();
            // rg = SQLFunctionsReports.OverallBatchStatusReport(reportID);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultReports resultReport = new GlobalVars.ResultReports();
            try
            {
                logger.Info("GetReportByID API Request. Report ID: " + reportID);
                
                if (reportID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultReport.ReturnCode = -1;
                    resultReport.Message = "Missing argument Report ID";
                    logger.Warn("GetReportByID API Request ends with an Error.");
                    logger.Warn(resultReport.Message);
                }
                else
                {
                    resultReport = SQLFunctionsReports.GetReportByID(reportID);
                    switch (resultReport.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetReportByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetReportByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetReportByID API Request ends with a Fatal Error.");
                resultReport.ReturnCode = -2;
                resultReport.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReport.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultReport.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultReport.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultReport, Formatting.Indented);
            logger.Info("Leaving GetReportByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Report by Customer and Template ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerID">
        /// Required Field.
        /// </param>
        /// <param name="templateID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Report Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.Report))]
        [ActionName("GetReportByCustomerAndTemplateID")]
        public ActionResult GetReportByCustomerAndTemplateID(int customerID, int templateID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultReports resultReport = new GlobalVars.ResultReports();
            try
            {
                logger.Info("GetReportByCustomerAndTemplateID API Request. Customer ID: " + customerID + " and Template ID: " + templateID);
                // CustomerID == 0 is a valid entry
                if (templateID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultReport.ReturnCode = -1;
                    resultReport.Message = "Missing argument Template ID";
                    logger.Warn("GetReportByCustomerAndTemplateID API Request ends with an Error.");
                    logger.Warn(resultReport.Message);
                }
                else
                {
                    resultReport = SQLFunctionsReports.GetReportByCustomerAndTemplateID(customerID, templateID);
                    switch (resultReport.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetReportByCustomerAndTemplateID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetReportByCustomerAndTemplateID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                logger.Fatal("GetReportByCustomerAndTemplateID API Request ends with a Fatal Error.");
                resultReport.ReturnCode = -2;
                resultReport.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReport.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultReport.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReport, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultReport.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultReport, Formatting.Indented);
            logger.Info("Leaving GetReportByCustomerAndTemplateID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Generate a Report for a given Report ID and Template ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="reportID">
        /// Required Field.
        /// </param>
        /// <param name="templateID">
        /// Required Field.
        /// </param>
        ///  /// <param name="sendEmail">
        /// Required Field { true or False }. Force to send the report via email.
        /// </param>
        /// ///  /// <param name="attachPDF">
        /// Required Field { true or False }. Use "true" if you wan tto recieve the content of the email as an attachment PDF File.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Report in HTML Format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultGeneric))]
        [ActionName("GenerateReport")]
        public ActionResult GenerateReport(int reportID, int templateID, Boolean sendEmail, Boolean attachPDF)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric();
            try
            {
                logger.Info("GenerateReport API Request. Report ID: " + reportID + " and Template ID: " + templateID);
                // CustomerID == 0 is a valid entry
                if (reportID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument Report ID";
                    logger.Warn("GenerateReport API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    if (templateID == 0)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Missing argument Template ID";
                        logger.Warn("GenerateReport API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                    else
                    {
                        if (Convert.ToString(sendEmail) == "True" || Convert.ToString(sendEmail) == "False" || string.IsNullOrWhiteSpace(Convert.ToString(sendEmail)))
                        {
                            if (string.IsNullOrWhiteSpace(Convert.ToString(sendEmail)))
                            {
                                sendEmail = false;
                            }
                            result = SQLFunctionsReports.GenerateReport(reportID, templateID, sendEmail, attachPDF);
                            switch (result.ReturnCode)
                            {
                                case 0:
                                    logger.Info("GenerateReport API Request was executed Successfully.");
                                    Response.StatusCode = (int)HttpStatusCode.OK;
                                    break;

                                case -2:
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    logger.Fatal("GenerateReport API Request ends with a Fatal Error.");
                                    logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    break;
                            }
                        }
                        else
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            result.ReturnCode = -1;
                            result.Message = "Invalid argument sendEmail. You must enter true/false.";
                            logger.Warn("GenerateReport API Request ends with an Error.");
                            logger.Warn(result.Message);
                        }                        
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GenerateReport API Request ends with a Fatal Error.");
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
            logger.Info("Leaving GenerateReport API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Generate a Work Order Reports for a given work orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="workOrders">
        /// Required Field. This a string of work orders numbers separated by comma
        /// </param>
        /// <param name="sendEmail">
        /// Required Field { true or False }. Force to send the report via email.
        /// </param>
        /// ///  /// <param name="attachPDF">
        /// Required Field { true or False }. Use "true" if you wan to recieve the content of the email as an attachment PDF File.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Report in HTML Format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultGeneric))]
        [ActionName("GenerateWorkOrderReport")]
        public ActionResult GenerateWorkOrderReport(string workOrders, Boolean sendEmail, Boolean attachPDF)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric();
            try
            {
                logger.Info("GenerateWorkOrderReport API Request. Work Orders to include in the Report: " + workOrders);
                logger.Info("   Send Email: " + sendEmail.ToString());
                logger.Info("   Attache Reports as PDF: " + attachPDF.ToString());
                // CustomerID == 0 is a valid entry
                if (string.IsNullOrEmpty(workOrders))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument WorkOrders (String of Numbers separated by comma)";
                    logger.Warn("GenerateWorkOrderReport API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {                    
                    if (Convert.ToString(sendEmail) == "True" || Convert.ToString(sendEmail) == "False" || string.IsNullOrWhiteSpace(Convert.ToString(sendEmail)))
                    {
                        if (string.IsNullOrWhiteSpace(Convert.ToString(sendEmail)))
                        {
                            sendEmail = false;
                        }
                        result = SQLFunctionsReports.GenerateWorkOrdersReport(workOrders, sendEmail, attachPDF);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("GenerateWorkOrderReport API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("GenerateWorkOrderReport API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Invalid argument sendEmail. You must enter true/false.";
                        logger.Warn("GenerateWorkOrderReport API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GenerateWorkOrderReport API Request ends with a Fatal Error.");
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
            logger.Info("Leaving GenerateWorkOrderReport API.");
            //return Json(messaje);
            return Content(messaje);
        }
        /// <summary>
        /// Get Customer Reports Template
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Customer Reports Template in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultReportsTemplate))]
        [ActionName("GetCustomerReportsTemplate")]
        public ActionResult GetCustomerReportsTemplate()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultReportsTemplate resultReportsTemplate = new GlobalVars.ResultReportsTemplate();
            try
            {
                logger.Info("GetCustomerReportsTemplate API Request.");

                resultReportsTemplate = SQLFunctionsReports.GetReportsTemplate("Customer");
                switch (resultReportsTemplate.ReturnCode)
                {
                    case 0:
                        logger.Info("GetCustomerReportsTemplate API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetCustomerReportsTemplate API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

            }
            catch (Exception e)
            {
                logger.Fatal("GetCustomerReportsTemplate API Request ends with a Fatal Error.");
                resultReportsTemplate.ReturnCode = -2;
                resultReportsTemplate.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReportsTemplate.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultReportsTemplate.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultReportsTemplate.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultReportsTemplate, Formatting.Indented);
            logger.Info("Leaving GetCustomerReportsTemplate API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update Report info for a given Report or create a new one if Reports does not exist.
        /// <remarks>
        /// Sample Request:
        /// '{ReportID: 0,TemplateID: 9,CustomerID: 3,EnableFlag: false,EmailSubject: "This is the subject line",TitleContent1: "COMPU - DATA International, LLC",TitleContent2: "Scanning Services System - REPORT NAME",TitleContent3: "DESCRIBE WHAT YOU EXPECT TO SEE IN THE TABLE",TitleFontColor1: "#993300",TitleFontColor2: "#000000",TitleFontColor3: "#993300",TitleFontSize1: 6,TitleFontSize2: 5,TitleFontSize3: 4,TableHeaderFontColor: "#2980B9",TableHeaderFontBoldFlag: true,TableHeaderBackColor: "#FDF5E6",TableHeaderFontSize: 4,TableColumnNamesFontColor: "#000000",TableColumnNamesFontBoldFlag: true,TableColumnNamesBackColor: "#FDF5E6",TableColumnNamesFontSize: 3,TitleFontBoldFlag1: true,TitleFontBoldFlag2: false,TitleFontBoldFlag3: false,TableHeaderBackgroundColor: null,EmailRecipients: "lcarbone@cdlac.com,rking@cdlac.com",Parameters: [{"TemplateID: 9,"ParameterID: 1,"ReportID: 9,"Value: "20"},{"TemplateID: 9,"ParameterID: 2,"ReportID: 9,"Value: "true"},{"TemplateID: 9,"ParameterID: 3,"ReportID: 9,"Value: "10:30"},{"TemplateID: 9,"ParameterID: 4,"ReportID: 9,"Value: "06:50"},{"TemplateID: 9,"ParameterID: 5,"ReportID: 9,"Value: ""},{"TemplateID: 9,"ParameterID: 6,"ReportID: 9,"Value: ""},{"TemplateID: 9,"ParameterID: 7,"ReportID: 9,"Value: ""}]'
        /// </remarks>
        /// <param name="reportJS">
        /// Required Field. Report information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateReport")]
        public ActionResult UpdateReport([FromBody]string reportJS)
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
                if (reportJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument reportJS";
                    logger.Warn("UpdateReport API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Report report = JsonConvert.DeserializeObject<GlobalVars.Report>(reportJS);
                    logger.Info("UpdateReport API Request.");
                    logger.Debug("Parameter:" + JsonConvert.SerializeObject(reportJS, Formatting.Indented));

                    //Rules:
                    // 1- Template ID not null
                    // 2- Customer ID  0 for System Reports. Any other number for Customer Report
                    if (report.TemplateID == 0)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "You must provide a Valid Template ID.";
                        logger.Warn("UpdateReport API Request ends with a Fatal Error.");
                        logger.Warn(result.Message);
                    }
                    else
                    {
                        result = SQLFunctionsReports.UpdateReport(report);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateReport API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateReport API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateReport API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateReport API");
            //return Json(messaje);
            return Content(messaje);
        }
    }
}