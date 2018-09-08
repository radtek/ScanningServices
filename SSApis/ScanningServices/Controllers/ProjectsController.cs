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

namespace ScanningServices.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ProjectsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Projects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <returns>List of Projects in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProjects))]
        [ActionName("GetProjects")]
        public ActionResult GetProjects()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects();
            try
            {
                logger.Info("GetProjects API Request.");
                resultProjects = SQLFunctionsProjects.GetProjects();
                switch (resultProjects.ReturnCode)
                {
                    case 0:
                        logger.Info("GetProjects API Request was executed Successfully.");
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        break;

                    case -2:
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        logger.Fatal("GetProjects API Request ends with a Fatal Error.");
                        logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProjects API Request ends with a Fatal Error.");
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProjects.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultProjects.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProjects, Formatting.Indented);
            logger.Info("Leaving GetProjects API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Projects by a given Customer ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Projects in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProjects))]
        [ActionName("GetProjectsByCustomerID")]
        public ActionResult GetProjectsByCustomerID(int customerID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects();
            try
            {
                logger.Info("GetProjectsByCustomerID API Request. Customer ID: " + customerID);
                if (customerID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProjects.ReturnCode = -1;
                    resultProjects.Message = "Missing argument Customer ID";
                    logger.Warn("GetProjectsByCustomerID API Request ends with an Error.");
                    logger.Warn(resultProjects.Message);
                }
                else
                {
                    resultProjects = SQLFunctionsProjects.GetProjectByCustomerID(customerID);
                    switch (resultProjects.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetProjectsByCustomerID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetProjectsByCustomerID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }                
            }
            catch (Exception e)
            {
                logger.Fatal("GetProjectsByCustomerID API Request ends with a Fatal Error.");
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProjects.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultProjects.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProjects, Formatting.Indented);
            logger.Info("Leaving GetProjectsByCustomerID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Projects Info by ID
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="projectID">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Projects in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProjects))]
        [ActionName("GetProjectByID")]
        public ActionResult GetProjectByID(int projectID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects();
            try
            {
                logger.Info("GetProjectsByID API Request. Project ID: " + projectID);
                if (projectID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProjects.ReturnCode = -1;
                    resultProjects.Message = "Missing argument Project ID";
                    logger.Warn("GetProjectsByID API Request ends with an Error.");
                    logger.Warn(resultProjects.Message);
                }
                else
                {
                    resultProjects = SQLFunctionsProjects.GetProjectByID(projectID);
                    switch (resultProjects.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetProjectsByID API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetProjectsByID API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProjectsByID API Request ends with a Fatal Error.");
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProjects.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultProjects.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProjects, Formatting.Indented);
            logger.Info("Leaving GetProjectsByID API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Get Projects Info by Name
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="projectName">
        /// Required Field.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns>List of Projects in Json format</returns>
        [HttpGet]
        [DefaultValue(false)]
        [Produces("application/json", Type = typeof(GlobalVars.ResultProjects))]
        [ActionName("GetProjectByName")]
        public ActionResult GetProjectByName(string projectName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects();
            try
            {
                logger.Info("GetProjectsByName API Request. Project Name: " + projectName);
                if (string.IsNullOrEmpty(projectName))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    resultProjects.ReturnCode = -1;
                    resultProjects.Message = "Missing argument Project Name";
                    logger.Warn("GetProjectsByName API Request ends with an Error.");
                    logger.Warn(resultProjects.Message);
                }
                else
                {
                    resultProjects = SQLFunctionsProjects.GetProjectByName(projectName);
                    switch (resultProjects.ReturnCode)
                    {
                        case 0:
                            logger.Info("GetProjectsByName API Request was executed Successfully.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            logger.Fatal("GetProjectsByName API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("GetProjectsByName API Request ends with a Fatal Error.");
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
                logger.Fatal("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            Response.ContentType = "application/json";
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            elapsedMs = elapsedMs / 1000;
            resultProjects.ElapsedTime = elapsedMs.ToString();
            logger.Debug("Returned value:" + JsonConvert.SerializeObject(resultProjects, Formatting.Indented));
            //var messaje = JsonConvert.SerializeObject(resultCustomers, Formatting.Indented);
            resultProjects.HttpStatusCode = Response.StatusCode.ToString();
            var messaje = JsonConvert.SerializeObject(resultProjects, Formatting.Indented);
            logger.Info("Leaving GetProjectsByName API.");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Create a new Project
        /// </summary>
        /// /// <remarks>
        /// Sample Request:
        /// '{CustomerID: "1", ProjectName: "Demo Project"}'
        /// </remarks>
        /// <param name="projectJS">
        /// Required Field. Project information must be provided in Json Format.
        /// The Project Name must be unique in the entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ActionName("NewProject")]
        public ActionResult NewProject([FromBody]string projectJS)
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
                if (projectJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument projectJS";
                    logger.Warn("NewProject API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Project project = JsonConvert.DeserializeObject<GlobalVars.Project>(projectJS);
                    logger.Info("NewProject API Request.");
                    if (!string.IsNullOrEmpty(project.ProjectName) & project.CustomerID != 0)
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(project, Formatting.Indented));

                        result = SQLFunctionsProjects.NewProject(project);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("NewProject API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("NewProject API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("NewProject API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "Project Name and/or Customer ID value is not valid.";
                        logger.Warn("NewProject API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("NewProject API Request ends with a Fatal Error.");
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
            logger.Info("Leaving NewProject API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Delete Project by a given ID
        /// </summary>
        /// <remarks>
        /// If the given Project ID does not exist, the Delete request will be ignored.
        /// </remarks>
        /// <param name="projectID">Required Field. ("0" is not a valid value)</param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("DeleteProject")]
        public ActionResult DeleteProject(int projectID)
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
                // Projectr ID is a Required Field
                if (projectID == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Project ID is a required field.";
                    logger.Warn(result.Message);
                }
                else
                {
                    logger.Info("DeleteProject API Request. Project ID: " + projectID);
                    result = SQLFunctionsProjects.DeleteProject(projectID);

                    switch (result.ReturnCode)
                    {
                        case 0:
                            // DeleteProject ends successfully
                            logger.Info("Project ID:  " + projectID + " was Deleted Successfully.");
                            result.Message = "Project ID:  " + projectID + " was Deleted Successfully.";
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -1:
                            logger.Info("DeleteProject API Request ends with a warning.");
                            Response.StatusCode = (int)HttpStatusCode.OK;
                            break;

                        case -2:
                            // DeleteProject ends with a Fatal error
                            logger.Fatal("DeleteProject API Request ends with a Fatal Error.");
                            logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("DeleteProject API Request ends with a Fatal Error.");
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
            logger.Info("Leaving DeleteProject API");
            //return Json(messaje);
            return Content(messaje);
        }

        /// <summary>
        /// Update a given Project. 
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// '{ProjectID: "9",CustomerName: "COMPU-DATA"}'
        /// </remarks>
        /// <param name="projectJS">
        /// Required Field. Project information in a Json Format.
        /// The Project Name must be unique inthe entire system.
        /// </param>
        /// <response code="200">Ok</response>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UpdateProject")]
        public ActionResult UpdateProject([FromBody]string projectJS)
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
                if (projectJS == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.ReturnCode = -1;
                    result.Message = "Missing argument projectJS";
                    logger.Warn("UpdateProject API Request ends with an Error.");
                    logger.Warn(result.Message);
                }
                else
                {
                    GlobalVars.Project project = JsonConvert.DeserializeObject<GlobalVars.Project>(projectJS);
                    logger.Info("UpdateProject API Request.");
                    if (!string.IsNullOrEmpty(project.ProjectName))
                    {
                        logger.Debug("Parameter:" + JsonConvert.SerializeObject(project, Formatting.Indented));

                        result = SQLFunctionsProjects.UpdateProject(project);
                        switch (result.ReturnCode)
                        {
                            case 0:
                                logger.Info("UpdateProject API Request was executed Successfully.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -1:
                                logger.Info("UpdateProject API Request ends with a warning.");
                                Response.StatusCode = (int)HttpStatusCode.OK;
                                break;

                            case -2:
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                logger.Fatal("UpdateProject API Request ends with a Fatal Error.");
                                logger.Debug("Returned value:" + JsonConvert.SerializeObject(result, Formatting.Indented));
                                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                break;
                        }
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result.ReturnCode = -1;
                        result.Message = "UpdateProject value is not valid.";
                        logger.Warn("UpdateProject API Request ends with an Error.");
                        logger.Warn(result.Message);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal("UpdateProject API Request ends with a Fatal Error.");
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
            logger.Info("Leaving UpdateProject API");
            //return Json(messaje);
            return Content(messaje);
        }
    }
}