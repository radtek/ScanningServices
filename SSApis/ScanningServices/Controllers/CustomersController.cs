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

/// INETERESTING :
/// If you want to have required fields in swagger, declare a public class like this, then use and instantietion of the class in the POST
/// Method
/// 
/// <summary>
/// 
/// </summary>
//public class SearchHistory
//{
//    [Required]
//    public int CurrentPageIndex { get; set; }
//    [Required]
//    public int PageSize { get; set; }
//    public DateTime? LastSyncDate { get; set; }
//}
//[HttpDelete]
//[ActionName("DeleteCustomer")]
//public ActionResult DeleteCustomer(int customerID, SearchHistory xyz)

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CustomersController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="responseFormat">
        /// Required Field {jason,xml}
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Customers in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        //[Produces("application/json", "application/xml")]
        [Produces("application/xml")] //, Type = typeof(GlobalVars.ResultCustomers))]
        [ActionName("XMLGetCustomers")]
        public ActionResult XMLGetCustomers(string responseFormat)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers();
            try
            {
                logger.Info("GetCustomers API Request.");
                resultCustomers = SQLFunctionsCustomers.GetCustomers();
                switch (resultCustomers.ReturnCode)
                {
                    case 0:
                        logger.Info("GetCustomers API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetCustomers API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetCustomers API Request ends with a Fatal Error.");
                resultCustomers.ReturnCode = -2;
                resultCustomers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomers.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/xml";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultCustomers.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultCustomers.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = XMLTools.GetXMLFromObject(resultCustomers);
            logger.Info("Leaving GetCustomers API.");
            return Content(messaje);
        }

       

        /// <summary>
        /// Get Customers
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Customers in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultCustomers))]
        [ActionName("GetCustomers")]
        public ActionResult GetCustomers()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers();
            try
            {
                logger.Info("GetCustomers API Request.");
                resultCustomers = SQLFunctionsCustomers.GetCustomers();
                switch (resultCustomers.ReturnCode)
                {
                    case 0:
                        logger.Info("GetCustomers API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetCustomers API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetCustomers API Request ends with a Fatal Error.");
                resultCustomers.ReturnCode = -2;
                resultCustomers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomers.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultCustomers.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultCustomers.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultCustomers,Formatting.Indented);
            logger.Info("Leaving GetCustomers API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Customer Info By Name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerName">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Customers in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultCustomers))]
        [ActionName("GetCustomerByName")]
        public ActionResult GetCustomerbyName(string customerName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers();
            try
            {
                logger.Info("GetCustomerbyName API Request. Customer Name: " + customerName);

                if (string.IsNullOrEmpty(customerName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultCustomers.ReturnCode = -1;
                    resultCustomers.Message = "Missing argument Customer Name";
                    logger.Warn("NewJob API Request ends with an Error.");
                    logger.Warn(resultCustomers.Message);
                }
                else
                {
                    resultCustomers = SQLFunctionsCustomers.GetCustomerByName(customerName);
                    switch (resultCustomers.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetCustomerbyName API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetCustomerbyName API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }                
            }
            catch (Exception e)
            {
                logger.Fatal("GetCustomerbyName API Request ends with a Fatal Error.");
                resultCustomers.ReturnCode = -2;
                resultCustomers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomers.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultCustomers.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultCustomers.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            logger.Info("Leaving GetCustomerbyName API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Customer Info By ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        ///<param name="customerID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Customers in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultCustomers))]
        [ActionName("GetCustomerByID")]
        public ActionResult GetCustomerByID(int customerID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers();
            try
            {
                logger.Info("GetCustomerByID API Request. Customer ID: " + customerID);

                if (customerID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultCustomers.ReturnCode = -1;
                    resultCustomers.Message = "Missing argument Customer ID";
                    logger.Warn("GetCustomerByID API Request ends with an Error.");
                    logger.Warn(resultCustomers.Message);
                }
                else
                {
                    resultCustomers = SQLFunctionsCustomers.GetCustomerByID(customerID);
                    switch (resultCustomers.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetCustomerByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;
                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetCustomerByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetCustomerByID API Request ends with a Fatal Error.");
                resultCustomers.ReturnCode = -2;
                resultCustomers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomers.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultCustomers.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultCustomers, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultCustomers.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            logger.Info("Leaving GetCustomerByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Create a new Customer
        /// </summary>
        /// /// <remarks>
        /// Sample Request:
        /// '{CustomerName: "COMPU-DATA"}'
        /// </remarks>
        /// <param name="customerJS">
        /// Required Field. Customer information must be provided in Json Format.
        /// The Customer Name must be unique inthe entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewCustomer")]
        //public ActionResult NewCustomer([FromBody]string customerJS)
        public ActionResult NewCustomer([FromBody]string customerJS)
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
                if (customerJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument customerJS";
                    logger.Warn("NewCustomer API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    //GlobalVars.Customer customer1 = new GlobalVars.Customer();
                    //customer1 = JsonConvert.SerializeObject(customer1);
                    GlobalVars.Customer customer = JsonConvert.DeserializeObject<GlobalVars.Customer>(customerJS);
                    logger.Info("NewCustomer API Request.");
                    if (!string.IsNullOrEmpty(customer.CustomerName))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(customer, Formatting.Indented));

                        result = SQLFunctionsCustomers.NewCustomer(customer);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewCustomer API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewCustomer API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewCustomer API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "CustomerName value is not valid.";
                        logger.Warn("NewCustomer API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewCustomer API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewCustomer API");
            //return Json(messaje);
            return Content(messaje);
        }
        
        /// <summary>
        /// Delete Customer by a given ID
        /// </summary>
        /// <remarks>
        /// If the given Customer ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="customerID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteCustomer")]
        public ActionResult DeleteCustomer(int customerID)
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
                // Customer ID is a Required Field
                if (customerID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Customer ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteCustomer API Request. Customer ID: " + customerID);
                    result = SQLFunctionsCustomers.DeleteCustomer(customerID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteCustomer ends successfully
                            logger.Info("Customer ID:  " + customerID + " was Deleted Successfully.");
                            result.Message = "Customer ID:  " + customerID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteCustomer API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteCustomer ends with a Fatal error
                            logger.Fatal("DeleteCustomer API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteCustomer API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteCustomer API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update a given Customer. 
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{CustomerID: "9",CustomerName: "COMPU-DATA"}'
        /// </remarks>
        /// <param name="customerJS">
        /// Required Field. Customer information in a Json Format.
        /// The Customer Name must be unique inthe entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateCustomer")]
        public ActionResult UpdateCustomer([FromBody]string customerJS)
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
                if (customerJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument customerJS";
                    logger.Warn("UpdateCustomer API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Customer customer = JsonConvert.DeserializeObject<GlobalVars.Customer>(customerJS);
                    logger.Info("UpdateCustomer API Request.");
                    if (!string.IsNullOrEmpty(customer.CustomerName))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(customer, Formatting.Indented));

                        result = SQLFunctionsCustomers.UpdateCustomer(customer);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateCustomer API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("UpdateCustomer API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateCustomer API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "CustomerName value is not valid.";
                        logger.Warn("UpdateCustomer API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateCustomer API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateCustomer API");
            //return Json(messaje);
            return Content(messaje);
        }

    }
}