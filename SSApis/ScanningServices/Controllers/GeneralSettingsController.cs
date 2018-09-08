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

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class GeneralSettingsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get General Settings Information
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>General Setting Information in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultGeneralSettingsExtended))]
        [ActionName("GetGeneralSettingsInfo")]
        public ActionResult GetGeneralSettingsInfo()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultGeneralSettingsExtended resultGeneralSettings = new GlobalVars.ResultGeneralSettingsExtended();
            try
            {
                logger.Info("GetGeneralSettingsInfo API Request.");
                resultGeneralSettings = SQLFunctionsGeneralSettings.GetGeneralSettings();
                switch (resultGeneralSettings.ReturnCode)
                {
                    case 0:
                        logger.Info("GetGeneralSettingsInfo API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetGeneralSettingsInfo API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultGeneralSettings, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetGeneralSettingsInfo API Request ends with a Fatal Error.");
                resultGeneralSettings.ReturnCode = -2;
                resultGeneralSettings.Message = e.Message;
                var baseException = e.GetBaseException();
                resultGeneralSettings.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultGeneralSettings, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultGeneralSettings.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultGeneralSettings, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultGeneralSettings.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultGeneralSettings, Formatting.Indented);
            logger.Info("Leaving GetGeneralSettingsInfo API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update General Setings Information. 
        /// </summary>
        /// <remarks>
        /// '{"??????"}'
        /// </remarks>
        /// <param name="generalSettingsJS">
        /// Required Field. SMTP information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateGeneralSettings")]
        public ActionResult UpdateGeneralSettings([FromBody]string generalSettingsJS)
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
                if (generalSettingsJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument generalSettingsJS";
                    logger.Warn("UpdateGeneralSettings API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.GeneralSettings generalSettings = JsonConvert.DeserializeObject<GlobalVars.GeneralSettings>(generalSettingsJS);
                    logger.Info("UpdateGeneralSettings API Request.");
                    logger.Debug("Parameter:" + JsonConvert.SerializeObject(generalSettings, Formatting.Indented));

                    result = SQLFunctionsGeneralSettings.UpdateGeneralSettings(generalSettings);
                    switch (result.ReturnCode)
                    {
                        case 0:
                            logger.Info("UpdateGeneralSettings API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("UpdateGeneralSettings API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateGeneralSettings API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateGeneralSettings API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Service Stations that are used to run SSS Scheduled processes
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param>
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Service Stations in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultServiceStationsExtended))]
        [ActionName("GetServiceStations")]
        public ActionResult GetServiceStations()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultServiceStationsExtended resultServiceStations = new GlobalVars.ResultServiceStationsExtended();
            try
            {
                resultServiceStations = SQLFunctionsGeneralSettings.GetServiceStations();
                switch (resultServiceStations.ReturnCode)
                {
                    case 0:
                        logger.Info("GetServiceStations API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetServiceStations API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetServiceStations API Request ends with a Fatal Error.");
                resultServiceStations.ReturnCode = -2;
                resultServiceStations.Message = e.Message;
                var baseException = e.GetBaseException();
                resultServiceStations.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultServiceStations.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
            resultServiceStations.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented);
            logger.Info("Leaving GetServiceStations API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Service Station by a given Station ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="stationID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Return the Station information in Json Format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultServiceStationsExtended))]
        [ActionName("GetServiceStationByID")]
        public ActionResult GetServiceStationByID(int stationID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultServiceStationsExtended resultServiceStations = new GlobalVars.ResultServiceStationsExtended();
            try
            {
                logger.Info("GetServiceStationByID API Request. Station ID: " + stationID);
                if (stationID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultServiceStations.ReturnCode = -1;
                    resultServiceStations.Message = "Missing argument Station ID";
                    logger.Warn("GetServiceStationByID API Request ends with an Error.");
                    logger.Warn(resultServiceStations.Message);
                }
                else
                {
                    resultServiceStations = SQLFunctionsGeneralSettings.GetServiceStationByID(stationID);
                    switch (resultServiceStations.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetServiceStationByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetServiceStationByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetServiceStationByID API Request ends with a Fatal Error.");
                resultServiceStations.ReturnCode = -2;
                resultServiceStations.Message = e.Message;
                var baseException = e.GetBaseException();
                resultServiceStations.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultServiceStations.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
            resultServiceStations.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented);
            logger.Info("Leaving GetServiceStationByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Service Station by Name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="stationName">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Return the Station information in Json Format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultServiceStationsExtended))]
        [ActionName("GetServiceStationByName")]
        public ActionResult GetServiceStationByName(string stationName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultServiceStationsExtended resultServiceStations = new GlobalVars.ResultServiceStationsExtended();
            try
            {
                logger.Info("GetServiceStationByName API Request. Station Name: " + stationName);
                if (string.IsNullOrEmpty(stationName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultServiceStations.ReturnCode = -1;
                    resultServiceStations.Message = "Missing argument Station Name";
                    logger.Warn("GetServiceStationByName API Request ends with an Error.");
                    logger.Warn(resultServiceStations.Message);
                }
                else
                {
                    resultServiceStations = SQLFunctionsGeneralSettings.GetServiceStationByBName(stationName);
                    switch (resultServiceStations.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetServiceStationByName API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetServiceStationByName API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetServiceStationByName API Request ends with a Fatal Error.");
                resultServiceStations.ReturnCode = -2;
                resultServiceStations.Message = e.Message;
                var baseException = e.GetBaseException();
                resultServiceStations.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultServiceStations.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented));
            resultServiceStations.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultServiceStations, Formatting.Indented);
            logger.Info("Leaving GetServiceStationByName API.");
            //return Json(messaje);
            return Content(messaje);
        }
        /// <summary>
        /// Create a new Service Station
        /// </summary>
        /// <remarks>
        /// Station is name of a machine that is used to run SSS Services
        /// </remarks>
        /// <param name="stationJS">
        /// Required Field.
        /// The Station information in Jason Format
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewServiceStation")]
        public ActionResult NewServiceStation([FromBody] string stationJS)
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
                if (stationJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument stationJS";
                    logger.Warn("NewServiceStation API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.ServiceStation serviceStation = JsonConvert.DeserializeObject<GlobalVars.ServiceStation>(stationJS);
                    logger.Info("NewServiceStation API Request.");
                    if (!string.IsNullOrEmpty(serviceStation.StationName))
                    {
                        logger.Debug("Parameter:" + serviceStation.StationName);
                        serviceStation.StationName = serviceStation.StationName;
                        result = SQLFunctionsGeneralSettings.NewServiceStation(serviceStation);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewServiceStation API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewServiceStation API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewServiceStation API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Station Name value is not valid.";
                        logger.Warn("NewServiceStation API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewServiceStation API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewServiceStation API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update a Service Station. 
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{}'
        /// </remarks>
        /// <param name="serviceStationJS">
        /// Required Field. Field information in a Json Format.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateServiceStation")]
        public ActionResult UpdateServiceStation([FromBody]string serviceStationJS)
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
                if (serviceStationJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument fieldJS";
                    logger.Warn("UpdateServiceStation API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.ServiceStation serviceStation = JsonConvert.DeserializeObject<GlobalVars.ServiceStation>(serviceStationJS);
                    logger.Info("UpdateServiceStation API Request.");
                   
                    logger.Debug("Parameter:" + JsonConvert.SerializeObject(serviceStation, Formatting.Indented));

                    result = SQLFunctionsGeneralSettings.UpdateServiceStation(serviceStation);
                    switch (result.ReturnCode)
                    {
                        case 0:
                            logger.Info("UpdateServiceStation API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("UpdateServiceStation API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("UpdateServiceStation API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateServiceStation API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateServiceStation API");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Delete Service Station by a given ID
        /// </summary>
        /// <remarks>
        /// This method is under construction.
        /// The methid is missing the validation code necessary to check if the Station ID is been used.
        /// This method must only allow the delete transaction if and only if the station is not been used by any process.
        /// If the given Service Station ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="stationID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteServiceStation")]
        public ActionResult DeleteServiceStation(int stationID)
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
                // Station ID is a Required Field
                if (stationID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Service Station ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteServiceStation API Request. Service Station ID: " + stationID);
                    result = SQLFunctionsGeneralSettings.DeleteServiceStation(stationID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteCustomer ends successfully
                            logger.Info("Service Station ID:  " + stationID + " was Deleted Successfully.");
                            result.Message = "Service Station ID:  " + stationID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteServiceStation API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteCustomer ends with a Fatal error
                            logger.Fatal("DeleteServiceStation API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteServiceStation API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteServiceStation API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Working Folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param>
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Working Folders in Json Format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultWorkingFolders))]
        [ActionName("GetWorkingFolders")]
        public ActionResult GetWorkingFolders()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultWorkingFolders resultWorkingFolders = new GlobalVars.ResultWorkingFolders();
            try
            {
                resultWorkingFolders = SQLFunctionsGeneralSettings.GetWorkingFolders();
                switch (resultWorkingFolders.ReturnCode)
                {
                    case 0:
                        logger.Info("GetWorkingFolders API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetWorkingFolders API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetWorkingFolders API Request ends with a Fatal Error.");
                resultWorkingFolders.ReturnCode = -2;
                resultWorkingFolders.Message = e.Message;
                var baseException = e.GetBaseException();
                resultWorkingFolders.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultWorkingFolders.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented));
            resultWorkingFolders.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented);
            logger.Info("Leaving GetWorkingFolders API.");
            //return Json(messaje);
            return Content(messaje);
        }


        /// <summary>
        /// Get Working Folder by a given Folder ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folderID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>Return the Station information in Json Format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultWorkingFolders))]
        [ActionName("GetWorkingFolderByID")]
        public ActionResult GetWorkingFolderByID(int folderID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultWorkingFolders resultWorkingFolders = new GlobalVars.ResultWorkingFolders();
            try
            {
                logger.Info("GetWorkingFolderByID API Request. Folder ID: " + folderID);
                if (folderID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultWorkingFolders.ReturnCode = -1;
                    resultWorkingFolders.Message = "Missing argument Folder ID";
                    logger.Warn("GetWorkingFolderByID API Request ends with an Error.");
                    logger.Warn(resultWorkingFolders.Message);
                }
                else
                {
                    resultWorkingFolders = SQLFunctionsGeneralSettings.GetWorkingFolderByID(folderID);
                    switch (resultWorkingFolders.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetWorkingFolderByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetWorkingFolderByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetWorkingFolderByID API Request ends with a Fatal Error.");
                resultWorkingFolders.ReturnCode = -2;
                resultWorkingFolders.Message = e.Message;
                var baseException = e.GetBaseException();
                resultWorkingFolders.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultWorkingFolders.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented));
            resultWorkingFolders.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultWorkingFolders, Formatting.Indented);
            logger.Info("Leaving GetWorkingFolderByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Create a new Working Folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="workingFolderJS">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewWorkingFolder")]
        public ActionResult NewWorkingFolder([FromBody] string workingFolderJS)
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
                if (workingFolderJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument stationJS";
                    logger.Warn("NewWorkingFolder API Request ends with a Fatal Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.WorkingFolder workingFolder = JsonConvert.DeserializeObject<GlobalVars.WorkingFolder>(workingFolderJS);
                    logger.Info("NewWorkingFolder API Request.");
                    if (!string.IsNullOrEmpty(workingFolder.Path))
                    {
                        logger.Debug("Parameter:" + workingFolder.Path);                        
                        result = SQLFunctionsGeneralSettings.NewWorkingFolder(workingFolder);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewWorkingFolder API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewWorkingFolder API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewWorkingFolder API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Path value is not valid.";
                        logger.Warn("NewWorkingFolder API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewWorkingFolder API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewWorkingFolder API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Delete Working Folder by a given ID
        /// </summary>
        /// <remarks>
        /// This method is under construction.
        /// The method is missing the validation code necessary to check if the Working Folder ID is been used.
        /// This method must only allow the delete transaction if and only if the Working Folder is not been used by any process.
        /// If the given Working Folder ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="folderID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteWorkingFolder")]
        public ActionResult DeleteWorkingFolder(int folderID)
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
                // Station ID is a Required Field
                if (folderID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Working Folder ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteWorkingFolder API Request. Working Folder ID: " + folderID);
                    result = SQLFunctionsGeneralSettings.DeleteWorkingFolder(folderID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteWorkingFolder ends successfully
                            logger.Info("Working Folder ID:  " + folderID + " was Deleted Successfully.");
                            result.Message = "Working Folder ID:  " + folderID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteWorkingFolder API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteCustomer ends with a Fatal error
                            logger.Fatal("DeleteWorkingFolder API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteWorkingFolder API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteWorkingFolder API");
            //return Json(messaje);
            return Content(messaje);
        }

        
    }
}