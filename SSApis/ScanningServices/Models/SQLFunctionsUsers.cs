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
    public class SQLFunctionsUsers
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get List of Users
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultUsers GetUsers()
        {
            List<GlobalVars.User> users = new List<GlobalVars.User>();
            GlobalVars.ResultUsers resultUsers = new GlobalVars.ResultUsers()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = users,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetUsers Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Users;
                    resultUsers.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.User user = new GlobalVars.User()
                            {
                                UserID = x.UserId,
                                UserName = (x.UserName ?? "").Trim(),
                                Title = x.Title,
                                Email = x.Email,
                                ActiveFlag = Convert.ToBoolean(x.ActiveFlag)
                            };

                            var result_aux = from u in DB.UserUifunctionality
                                             join f in DB.Sssfunctionality on u.FunctionalityId equals f.FunctionalityId
                                             where u.UserId == x.UserId
                                             select new { f.FunctionalityId, f.Functionality };                            

                            List<GlobalVars.UIFunctionality> funtionalities = new List<GlobalVars.UIFunctionality>();
                            if (result_aux.Count() >= 1)
                            {                                
                                foreach (var y in result_aux)
                                {
                                    GlobalVars.UIFunctionality functionality = new GlobalVars.UIFunctionality();
                                    functionality.FunctionalityID = y.FunctionalityId;
                                    functionality.Description = y.Functionality;
                                    funtionalities.Add(functionality); 
                                }                               
                            }
                            user.UIFunctionality = funtionalities;
                            users.Add(user);
                        }
                    }
                }
                resultUsers.ReturnValue = users;
                resultUsers.Message = "GetUsers transaction completed successfully. Number of records found: " + resultUsers.RecordsCount;
                logger.Debug(resultUsers.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultUsers.ReturnCode = -2;
                resultUsers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUsers.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetUsers Method ...");
            return resultUsers;
        }

        /// <summary>
        /// Get User Information by ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultUsers GetUserByID(int userID)
        {
            List<GlobalVars.User> users = new List<GlobalVars.User>();
            GlobalVars.ResultUsers resultUsers = new GlobalVars.ResultUsers()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = users,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetUserByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Users.Where(x => x.UserId == userID);
                    resultUsers.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.User user = new GlobalVars.User()
                            {
                                UserID = x.UserId,
                                UserName = (x.UserName ?? "").Trim(),
                                Title = x.Title,
                                Email = x.Email,
                                ActiveFlag = Convert.ToBoolean(x.ActiveFlag)
                            };
                            var result_aux = from u in DB.UserUifunctionality
                                             join f in DB.Sssfunctionality on u.FunctionalityId equals f.FunctionalityId
                                             where u.UserId == x.UserId
                                             select new { f.FunctionalityId, f.Functionality };

                            List<GlobalVars.UIFunctionality> funtionalities = new List<GlobalVars.UIFunctionality>();
                            if (result_aux.Count() >= 1)
                            {
                                foreach (var y in result_aux)
                                {
                                    GlobalVars.UIFunctionality functionality = new GlobalVars.UIFunctionality();
                                    functionality.FunctionalityID = y.FunctionalityId;
                                    functionality.Description = y.Functionality;
                                    funtionalities.Add(functionality);
                                }
                            }
                            user.UIFunctionality = funtionalities;
                            users.Add(user);
                        }
                    }
                }
                resultUsers.ReturnValue = users;
                resultUsers.Message = "GetUserByID transaction completed successfully. Number of records found: " + resultUsers.RecordsCount;
                logger.Debug(resultUsers.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultUsers.ReturnCode = -2;
                resultUsers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUsers.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetUserByID Method ...");
            return resultUsers;
        }

        /// <summary>
        /// Get User Information by UserName
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultUsers GetUserByName(string userName)
        {
            List<GlobalVars.User> users = new List<GlobalVars.User>();
            GlobalVars.ResultUsers resultUsers = new GlobalVars.ResultUsers()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = users,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetUserByName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Users.Where(x => x.UserName == userName);
                    resultUsers.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.User user = new GlobalVars.User()
                            {
                                UserID = x.UserId,
                                UserName = (x.UserName ?? "").Trim(),
                                Title = x.Title,
                                Email = x.Email,
                                ActiveFlag = Convert.ToBoolean(x.ActiveFlag)
                            };
                            var result_aux = from u in DB.UserUifunctionality
                                             join f in DB.Sssfunctionality on u.FunctionalityId equals f.FunctionalityId
                                             where u.UserId == x.UserId
                                             select new { f.FunctionalityId, f.Functionality };                            

                            List<GlobalVars.UIFunctionality> funtionalities = new List<GlobalVars.UIFunctionality>();
                            if (result_aux.Count() >= 1)
                            {                                
                                foreach (var y in result_aux)
                                {
                                    GlobalVars.UIFunctionality functionality = new GlobalVars.UIFunctionality();
                                    functionality.FunctionalityID = y.FunctionalityId;
                                    functionality.Description = y.Functionality;
                                    funtionalities.Add(functionality); 
                                }                               
                            }
                            user.UIFunctionality = funtionalities;
                            users.Add(user);
                        }
                    }
                }
                resultUsers.ReturnValue = users;
                resultUsers.Message = "GetUserByName transaction completed successfully. Number of records found: " + resultUsers.RecordsCount;
                logger.Debug(resultUsers.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultUsers.ReturnCode = -2;
                resultUsers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUsers.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetUserByName Method ...");
            return resultUsers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistUserName(string userName)
        {
            List<GlobalVars.User> users = new List<GlobalVars.User>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistUserName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Users.Where(x => x.UserName == userName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "User " + userName + " already exist.";
                    }
                    else
                    {
                        result.Message = "User " + userName + " doest not exist.";
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
            logger.Trace("Leaving ExistUserName Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewUser(GlobalVars.User user)
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
                logger.Trace("Entering into NewUser Method ...");

                // Check if User Exist
                result = ExistUserName(user.UserName);
                if (result.RecordsCount == 0)
                {
                    // Create new User ...
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        Users New_Record = new Users();
                        New_Record.UserName = user.UserName;
                        New_Record.Title = user.Title;
                        New_Record.ActiveFlag = Convert.ToString(user.ActiveFlag);
                        New_Record.Email = user.Email;

                        DB.Users.Add(New_Record);
                        DB.SaveChanges();
                    }

                    // Get the user ID created above, so it can be useed in UserUI Functionality Table below
                    GlobalVars.ResultUsers users = new GlobalVars.ResultUsers();
                    users = GetUserByName(user.UserName);

                    // Add User Functionalities ...
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        UserUifunctionality New_Record = new UserUifunctionality();

                        foreach (GlobalVars.UIFunctionality functionlaity in user.UIFunctionality)
                        {
                            New_Record.FunctionalityId = functionlaity.FunctionalityID;
                            New_Record.UserId = user.UserID;
                            DB.UserUifunctionality.Add(New_Record);
                            DB.SaveChanges();
                        }
                    }
                    result.Message = "NewUser transaction completed successfully. One Record added.";
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "User " + user.UserName + " already exist. NewUser transaction ignore.";
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
            logger.Trace("Leaving NewUser Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateUser(GlobalVars.User user)
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
                logger.Trace("Entering into UpdateUser Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // User Name must be unique in the Database. The Name could be change but it must be unique
                    Users Matching_Result = DB.Users.FirstOrDefault(x => x.UserName == user.UserName);
                    if (Matching_Result != null)
                    {
                        // Means --> this is a new name
                        Matching_Result = DB.Users.FirstOrDefault(x => x.UserId == user.UserID);
                        if (Matching_Result != null)
                        {
                            Matching_Result.UserName = user.UserName;
                            Matching_Result.Email = user.Email;
                            Matching_Result.ActiveFlag = Convert.ToString(user.ActiveFlag);
                            Matching_Result.Title = user.Title;
                            DB.SaveChanges();

                            // Update User UI Functionlaities
                            // 1- Delete existing all existimg UI Functinalities for this user
                            DB.UserUifunctionality.RemoveRange(DB.UserUifunctionality.Where(x => x.UserId == user.UserID));
                            DB.SaveChanges();

                            // 2- Add User Functionalities
                            UserUifunctionality New_Record = new UserUifunctionality();
                            
                            foreach (GlobalVars.UIFunctionality functionlaity in user.UIFunctionality)
                            {
                                New_Record.FunctionalityId = functionlaity.FunctionalityID;
                                New_Record.UserId = user.UserID;
                                DB.UserUifunctionality.Add(New_Record);
                                DB.SaveChanges();
                            }
                            result.Message = "UpdateUser transaction completed successfully. One Record Updated.";
                        }
                        else
                        {
                            // Means --> cannot update a Customer that does not exist
                            result.ReturnCode = -1;
                            result.Message = "User " + user.UserName + " does not exist. UpdateUser transaction ignore.";
                        }
                    }
                    else
                    {
                        // Means --> the name already exist
                        result.ReturnCode = -1;
                        result.Message = "User " + user.UserName + " already exist. UpdateUser transaction ignore.";
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
            logger.Trace("Leaving UpdateUser Method ...");
            return result;
        }

        /// <summary>
        /// Remove User and associated information from Database
        /// </summary>
        /// <param name="userID"></param>
        static public GlobalVars.ResultGeneric DeleteUser(int userID)
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
                logger.Trace("Entering into DeleteUser Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    Users Matching_Result = DB.Users.FirstOrDefault(x => x.UserId == userID);
                    if (Matching_Result != null)
                    {
                        DB.Users.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteUser transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "User ID" + userID + " does not exist. DeleteUser transaction ignore.";
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
            logger.Trace("Leaving DeleteUser Method ...");
            return result;
        }

        /// <summary>
        /// Get List of UI Functionalities
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultUIFunctionalities GetUIFunctionalities()
        {
            List<GlobalVars.UIFunctionality> uiFunctionalities = new List<GlobalVars.UIFunctionality>();
            GlobalVars.ResultUIFunctionalities resultUIFunctionalities = new GlobalVars.ResultUIFunctionalities()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = uiFunctionalities,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetUIFunctionalities Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Sssfunctionality;
                    resultUIFunctionalities.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.UIFunctionality uiFunctionality = new GlobalVars.UIFunctionality()
                            {
                                FunctionalityID = x.FunctionalityId,
                                Description = (x.Functionality ?? "").Trim(),

                            };
                            uiFunctionalities.Add(uiFunctionality);
                        }
                    }
                }
                resultUIFunctionalities.ReturnValue = uiFunctionalities;
                resultUIFunctionalities.Message = "GetUIFunctionalities transaction completed successfully. Number of records found: " + resultUIFunctionalities.RecordsCount;
                logger.Debug(resultUIFunctionalities.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultUIFunctionalities.ReturnCode = -2;
                resultUIFunctionalities.Message = e.Message;
                var baseException = e.GetBaseException();
                resultUIFunctionalities.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetUIFunctionalities Method ...");
            return resultUIFunctionalities;
        }
    }
}
