using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using System;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLFunctionsProjects
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get List of Projects
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProjects GetProjects()
        {
            List<GlobalVars.Project> projects = new List<GlobalVars.Project>();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = projects,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProjects Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Projects;
                    resultProjects.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Project project = new GlobalVars.Project()
                            {
                                ProjectID = x.ProjectId,
                                CustomerID = x.CustomerId,
                                ProjectName = x.ProjectName
                            };
                            projects.Add(project);
                        }
                    }
                }
                resultProjects.ReturnValue = projects;
                resultProjects.Message = "GetProjects transaction completed successfully. Number of records found: " + resultProjects.RecordsCount;
                logger.Debug(resultProjects.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProjects Method ...");
            return resultProjects;
        }

        /// <summary>
        /// Get Project by Name
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProjects GetProjectByName(string projectName)
        {
            List<GlobalVars.Project> projects = new List<GlobalVars.Project>();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = projects,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProjectByName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Projects.Where(x => x.ProjectName == projectName);
                    resultProjects.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Project project = new GlobalVars.Project()
                            {
                                ProjectID = x.ProjectId,
                                CustomerID = x.CustomerId,
                                ProjectName = x.ProjectName
                            };
                            projects.Add(project);
                        }
                    }
                }
                resultProjects.ReturnValue = projects;
                resultProjects.Message = "GetProjectByName transaction completed successfully. Number of records found: " + resultProjects.RecordsCount;
                logger.Debug(resultProjects.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProjectByName Method ...");
            return resultProjects;
        }

        /// <summary>
        /// Get Project by ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProjects GetProjectByID(int projectID)
        {
            List<GlobalVars.Project> projects = new List<GlobalVars.Project>();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = projects,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProjectByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Projects.Where(x => x.ProjectId == projectID);
                    resultProjects.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Project project = new GlobalVars.Project()
                            {
                                ProjectID = x.ProjectId,
                                CustomerID = x.CustomerId,
                                ProjectName = x.ProjectName
                            };
                            projects.Add(project);
                        }
                    }
                }
                resultProjects.ReturnValue = projects;
                resultProjects.Message = "GetProjectByID transaction completed successfully. Number of records found: " + resultProjects.RecordsCount;
                logger.Debug(resultProjects.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProjectByID Method ...");
            return resultProjects;
        }

        
        /// <summary>
        /// Get Project by Customer ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultProjects GetProjectByCustomerID(int customerID)
        {
            List<GlobalVars.Project> projects = new List<GlobalVars.Project>();
            GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = projects,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetProjectByCustomerID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Projects.Where(x => x.CustomerId == customerID);
                    resultProjects.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Project project = new GlobalVars.Project()
                            {
                                ProjectID = x.ProjectId,
                                CustomerID = x.CustomerId,
                                ProjectName = x.ProjectName
                            };
                            projects.Add(project);
                        }
                    }
                }
                resultProjects.ReturnValue = projects;
                resultProjects.Message = "GetProjectByCustomerID transaction completed successfully. Number of records found: " + resultProjects.RecordsCount;
                logger.Debug(resultProjects.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultProjects.ReturnCode = -2;
                resultProjects.Message = e.Message;
                var baseException = e.GetBaseException();
                resultProjects.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetProjectByCustomerID Method ...");
            return resultProjects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistProjectName(string projectName)
        {
            List<GlobalVars.Project> projects = new List<GlobalVars.Project>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistProjectName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Projects.Where(x => x.ProjectName == projectName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Project Name " + projectName + " already exist.";
                    }
                    else
                    {
                        result.Message = "Project Name " + projectName + " doest not exist.";
                    }
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving ExistProjectName Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistProjectID(int projectID)
        {
            List<GlobalVars.Project> projects = new List<GlobalVars.Project>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistProjectID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Projects.Where(x => x.ProjectId == projectID);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Project ID " + projectID + " already exist.";
                    }
                    else
                    {
                        result.Message = "Project ID " + projectID + " doest not exist.";
                    }
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving ExistProjectID Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewProject(GlobalVars.Project project)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into NewProject Method ...");

                // Check if Customer Exist
                result = SQLFunctionsCustomers.ExistCustomerID(project.CustomerID);
                if (result.RecordsCount != 0)
                {
                    // Check if Project Name Exist
                    result = SQLFunctionsProjects.ExistProjectName(project.ProjectName);
                    if (result.RecordsCount == 0)
                    {
                        // Create new Project
                        using (ScanningDBContext DB = new ScanningDBContext())
                        {
                            Projects New_Record = new Projects();
                            New_Record.ProjectName = project.ProjectName;
                            New_Record.CustomerId = project.CustomerID;

                            DB.Projects.Add(New_Record);
                            DB.SaveChanges();
                        }
                        result.Message = "NewProject transaction completed successfully. One Record added.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Project " + project.ProjectName + " already exist. NewProject transaction will be ignore.";
                    }                        
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Customer ID " + project.CustomerID + " does not exist. NewProject transaction will be ignore.";
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving NewProject Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateProject(GlobalVars.Project project)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into UpdateProject Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Project Names must be unique in the Database. The Name could be change but it must be unique
                    Projects Matching_Result = DB.Projects.FirstOrDefault(x => x.ProjectName == project.ProjectName);
                    if (Matching_Result == null)
                    {
                        // Means --> this is a new name
                        Matching_Result = DB.Projects.FirstOrDefault(x => x.ProjectId == project.ProjectID);
                        if (Matching_Result != null)
                        {
                            Matching_Result.ProjectName = project.ProjectName;
                            DB.SaveChanges();
                            result.Message = "UpdateCustomer transaction completed successfully. One Record Updated.";
                        }
                        else
                        {
                            // Means --> cannot update a Customer that does not exist
                            result.ReturnCode = -1;
                            result.Message = "Project " + project.ProjectName + " does not exist. UpdateProject transaction ignore.";
                        }
                    }
                    else
                    {
                        // Means --> the name already exist
                        result.ReturnCode = -1;
                        result.Message = "Project " + project.ProjectName + " already exist. UpdateCustomer transaction ignore.";
                    }
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving UpdateProject Method ...");
            return result;
        }

        /// <summary>
        /// Remove Project and associated information from Database
        /// </summary>
        /// <param name="projectID"></param>
        static public GlobalVars.ResultGeneric DeleteProject(int projectID)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into DeleteProject Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    Projects Matching_Result = DB.Projects.FirstOrDefault(x => x.ProjectId == projectID);
                    if (Matching_Result != null)
                    {
                        DB.Projects.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteProject transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Project ID" + projectID + " does not exist. DeleteProject transaction ignore.";
                    }
                    logger.Debug(result.Message);
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving DeleteProject Method ...");
            return result;
        }

    }
}
