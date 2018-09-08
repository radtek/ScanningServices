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
    public class SQLFunctionsCustomers
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get List of Customers
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultCustomers GetCustomers()
        {
            List<GlobalVars.Customer> customers = new List<GlobalVars.Customer>();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = customers,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetCustomers Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Customers;
                    resultCustomers.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Customer customer = new GlobalVars.Customer()
                            {
                                CustomerID = x.CustomerId,
                                CustomerName = (x.CustomerName ?? "").Trim()
                            };
                            customers.Add(customer);
                        }
                    }
                }
                resultCustomers.ReturnValue = customers;
                resultCustomers.Message = "GetCustomers transaction completed successfully. Number of records found: " + resultCustomers.RecordsCount;
                logger.Debug(resultCustomers.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultCustomers.ReturnCode = -2;
                resultCustomers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomers.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetCustomers Method ...");
            return resultCustomers;
        }

        /// <summary>
        /// Get Customer by Name
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultCustomers GetCustomerByName(string customerName)
        {
            List<GlobalVars.Customer> customers = new List<GlobalVars.Customer>();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = customers,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetCustomerByName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Customers.Where(x => x.CustomerName == customerName);
                    resultCustomers.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Customer customer = new GlobalVars.Customer()
                            {
                                CustomerID = x.CustomerId,
                                CustomerName = (x.CustomerName ?? "").Trim()
                            };
                            customers.Add(customer);
                        }
                    }
                }
                resultCustomers.ReturnValue = customers;
                resultCustomers.Message = "GetCustomerByName transaction completed successfully. Number of records found: " + resultCustomers.RecordsCount;
                logger.Debug(resultCustomers.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultCustomers.ReturnCode = -2;
                resultCustomers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomers.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetCustomerByName Method ...");
            return resultCustomers;
        }

        /// <summary>
        /// Get Customer by ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultCustomers GetCustomerByID(int customerID)
        {
            List<GlobalVars.Customer> customers = new List<GlobalVars.Customer>();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = customers,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetCustomerByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Customers.Where(x => x.CustomerId == customerID);
                    resultCustomers.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Customer customer = new GlobalVars.Customer()
                            {
                                CustomerID = x.CustomerId,
                                CustomerName = (x.CustomerName ?? "").Trim()
                            };
                            customers.Add(customer);
                        }
                    }
                }
                resultCustomers.ReturnValue = customers;
                resultCustomers.Message = "GetCustomerByID transaction completed successfully. Number of records found: " + resultCustomers.RecordsCount;
                logger.Debug(resultCustomers.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultCustomers.ReturnCode = -2;
                resultCustomers.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomers.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetCustomerByID Method ...");
            return resultCustomers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistCustomerID(int customerID)
        {
            List<GlobalVars.Customer> customers = new List<GlobalVars.Customer>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistCustomerID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Customers.Where(x => x.CustomerId == customerID);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Customer ID " + customerID + " already exist.";
                    }
                    else
                    {
                        result.Message = "Customer ID " + customerID + " doest not exist.";
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
            logger.Trace("Leaving ExistCustomerID Method ...");
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistCustomerName(string customerName)
        {
            List<GlobalVars.Customer> customers = new List<GlobalVars.Customer>();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistCustomerName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Customers.Where(x => x.CustomerName == customerName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Customer " + customerName + " already exist.";
                    }
                    else
                    {
                        result.Message = "Customer " + customerName + " doest not exist.";
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
            logger.Trace("Leaving ExistCustomerName Method ...");
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewCustomer(GlobalVars.Customer customer)
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
                logger.Trace("Entering into NewCustomer Method ...");

                // Check if Customer Exist
                result = ExistCustomerName(customer.CustomerName);
                if (result.RecordsCount == 0)
                {
                    // Create new Customer
                    using (ScanningDBContext DB = new ScanningDBContext())
                    {
                        Customers New_Record = new Customers();
                        New_Record.CustomerName = customer.CustomerName;

                        DB.Customers.Add(New_Record);
                        DB.SaveChanges();
                    }
                    result.Message = "NewCustomer transaction completed successfully. One Record added.";
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Customer " + customer.CustomerName + " already exist. NewCustomer transaction ignore.";
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
            logger.Trace("Leaving NewCustomer Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateCustomer(GlobalVars.Customer customer)
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
                logger.Trace("Entering into UpdateCustomer Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Customer Names must be unique in the Database. The Name could be change but it must be unique
                    Customers Matching_Result = DB.Customers.FirstOrDefault(x => x.CustomerName == customer.CustomerName);
                    if (Matching_Result == null)
                    {
                        // Means --> this is a new name
                        Matching_Result = DB.Customers.FirstOrDefault(x => x.CustomerId == customer.CustomerID);
                        if (Matching_Result != null)
                        {
                            Matching_Result.CustomerName = customer.CustomerName;
                            DB.SaveChanges();
                            result.Message = "UpdateCustomer transaction completed successfully. One Record Updated.";
                        }
                        else
                        {
                            // Means --> cannot update a Customer that does not exist
                            result.ReturnCode = -1;
                            result.Message = "Customer " + customer.CustomerName + " does not exist. UpdateCustomer transaction ignore.";
                        }
                    }
                    else
                    {
                        // Means --> the name already exist
                        result.ReturnCode = -1;
                        result.Message = "Customer " + customer.CustomerName + " already exist. UpdateCustomer transaction ignore.";
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
            logger.Trace("Leaving UpdateCustomer Method ...");
            return result;
        }

        /// <summary>
        /// Remove Customer and associated information from Database
        /// </summary>
        /// <param name="customerID"></param>
        static public GlobalVars.ResultGeneric DeleteCustomer(int customerID)
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
                logger.Trace("Entering into DeleteCustomer Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    Customers Matching_Result = DB.Customers.FirstOrDefault(x => x.CustomerId == customerID);
                    if (Matching_Result != null)
                    { 
                        DB.Customers.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteCustomer transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Customer ID" + customerID + " does not exist. DeleteCustomer transaction ignore.";
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
            logger.Trace("Leaving DeleteCustomer Method ...");
            return result;
        }


    }


}
