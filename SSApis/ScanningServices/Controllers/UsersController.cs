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
    public class UsersController : Controller
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Get Scanning Services Application User Interface Functionalities
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Functionality in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultUIFunctionalities))]
        [ActionName("GetUIFunctionalities")]
        public ActionResult GetUIFunctionalities()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultUIFunctionalities resultUIFunctionalities = new GlobalVars.ResultUIFunctionalities();
            try
            {
                logger.Info("GetUIFunctionalities API Request.");
                resultUIFunctionalities = SQLFunctionsUsers.GetUIFunctionalities();
                switch (resultUIFunctionalities.ReturnCode)
                {
                    case 0:
                        logger.Info("GetUIFunctionalities API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetUIFunctionalities API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUIFunctionalities, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetUIFunctionalities API Request ends with a Fatal Error.");
                resultUIFunctionalities.ReturnCode = -2;
                resultUIFunctionalities.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUIFunctionalities.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultUIFunctionalities, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultUIFunctionalities.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUIFunctionalities, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultUIFunctionalities.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultUIFunctionalities, Formatting.Indented);
            logger.Info("Leaving GetUIFunctionalities API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Users
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Users in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultUsers))]
        [ActionName("GetUsers")]
        public ActionResult GetUsers()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultUsers resultUsers = new GlobalVars.ResultUsers();
            try
            {
                logger.Info("GetUsers API Request.");
                resultUsers = SQLFunctionsUsers.GetUsers();
                switch (resultUsers.ReturnCode)
                {
                    case 0:
                        logger.Info("GetUsers API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetUsers API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetUsers API Request ends with a Fatal Error.");
                resultUsers.ReturnCode = -2;
                resultUsers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUsers.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultUsers.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultUsers.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultUsers, Formatting.Indented);
            logger.Info("Leaving GetUsers API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get User Info By Name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userName">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>User Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultUsers))]
        [ActionName("GetUserByName")]
        public ActionResult GetUserByName(string userName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultUsers resultUsers = new GlobalVars.ResultUsers();
            try
            {
                logger.Info("GetUserByName API Request. User Name: " + userName);

                if (string.IsNullOrEmpty(userName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultUsers.ReturnCode = -1;
                    resultUsers.Message = "Missing argument User Name";
                    logger.Warn("NewJob API Request ends with an Error.");
                    logger.Warn(resultUsers.Message);
                }
                else
                {
                    resultUsers = SQLFunctionsUsers.GetUserByName(userName);
                    switch (resultUsers.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetUserByName API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetUserByName API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetUserByName API Request ends with a Fatal Error.");
                resultUsers.ReturnCode = -2;
                resultUsers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUsers.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultUsers.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultUsers.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultUsers, Formatting.Indented);
            logger.Info("Leaving GetUserByName API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get User Info By ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        ///<param name="userID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>User inforamtion in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultUsers))]
        [ActionName("GetUserByID")]
        public ActionResult GetUserByID(int userID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultUsers resultUsers = new GlobalVars.ResultUsers();
            try
            {
                logger.Info("GetUserByID API Request. User ID: " + userID);

                if (userID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultUsers.ReturnCode = -1;
                    resultUsers.Message = "Missing argument Customer ID";
                    logger.Warn("GetUserByID API Request ends with an Error.");
                    logger.Warn(resultUsers.Message);
                }
                else
                {
                    resultUsers = SQLFunctionsUsers.GetUserByID(userID);
                    switch (resultUsers.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetUserByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;
                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetUserByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetUserByID API Request ends with a Fatal Error.");
                resultUsers.ReturnCode = -2;
                resultUsers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUsers.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultUsers.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultUsers, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultUsers.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultUsers, Formatting.Indented);
            logger.Info("Leaving GetUserByID API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Create a new User
        /// </summary>
        /// /// <remarks>
        /// Sample Request:
        /// '{UserName: "jsmith"}'
        /// </remarks>
        /// <param name="userJS">
        /// Required Field. User information must be provided in Json Format.
        /// The User Name must be unique inthe entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewUser")]
        public ActionResult NewUser([FromBody]string userJS)
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
                if (userJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument customerJS";
                    logger.Warn("NewUser API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.User user = JsonConvert.DeserializeObject<GlobalVars.User>(userJS);
                    logger.Info("NewUser API Request.");
                    if (!string.IsNullOrEmpty(user.UserName))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(user, Formatting.Indented));

                        result = SQLFunctionsUsers.NewUser(user);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewUser API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewUser API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewUser API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "NewUser value is not valid.";
                        logger.Warn("NewUser API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewUser API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewUser API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Delete User by a given ID
        /// </summary>
        /// <remarks>
        /// If the given User ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="userID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteUser")]
        public ActionResult DeleteUser(int userID)
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
                // User ID is a Required Field
                if (userID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "User ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteUser API Request. User ID: " + userID);
                    result = SQLFunctionsUsers.DeleteUser(userID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // Delete User ends successfully
                            logger.Info("User ID:  " + userID + " was Deleted Successfully.");
                            result.Message = "User ID:  " + userID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteUser API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteCustomer ends with a Fatal error
                            logger.Fatal("DeleteUser API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteUser API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteUser API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Update a given User. 
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{UserID: "9",UserName: "jsmith", Email: "jsmith@cdlac.com", Active: "true"}'
        /// </remarks>
        /// <param name="userJS">
        /// Required Field. User information in a Json Format.
        /// The User Name must be unique inthe entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateUser")]
        public ActionResult UpdateUser([FromBody]string userJS)
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
                if (userJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument userJS";
                    logger.Warn("UpdateUser API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.User user = JsonConvert.DeserializeObject<GlobalVars.User>(userJS);
                    logger.Info("UpdateUser API Request.");
                    if (!string.IsNullOrEmpty(user.UserName))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(user, Formatting.Indented));

                        result = SQLFunctionsUsers.UpdateUser(user);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateUser API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("UpdateUser API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateUser API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "UpdateUser value is not valid.";
                        logger.Warn("UpdateUser API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateUser API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateUser API");
            //return Json(messaje);
            return Content(messaje);
        }
    }
}