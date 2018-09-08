using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;

namespace ScanningServices.Models
{

    public class SQLFunctionsVFR
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get VFR Server Information
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultsVFR GetVFRInfoByJobID(int jobID)
        {
            GlobalVars.VFR vfr = new GlobalVars.VFR();
            GlobalVars.ResultsVFR resultVFR = new GlobalVars.ResultsVFR()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = vfr,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetVFRInfoByJobID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Vfr.FirstOrDefault(x => x.JobId == jobID);
                    if (results != null)
                    {
                        resultVFR.RecordsCount = 1;
                        vfr.CADIUrl = results.Cadiurl;
                        vfr.CaptureTemplate = results.CaptureTemplate;
                        vfr.InstanceName = results.InstanceName;
                        vfr.JobID = results.JobId;
                        vfr.SettingID = results.SettingId;
                        vfr.Password = results.Password;
                        vfr.UserName = results.UserName;
                        vfr.RepositoryName = results.RepositoryName;
                        vfr.QueryField = results.QueryField;
                    }
                    else
                    {
                        //There is no record in the database
                    }
                }
                resultVFR.ReturnValue = vfr;
                resultVFR.Message = "GetVFRInfoByJobID transaction completed successfully. Number of records found: " + resultVFR.RecordsCount;
                logger.Debug(resultVFR.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultVFR.ReturnCode = -2;
                resultVFR.Message = e.Message;
                var baseException = e.GetBaseException();
                resultVFR.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetVFRInfoByJobID Method ...");
            return resultVFR;
        }

        /// <summary>
        /// The Update Method creates a new records if it does not exist
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateVFRInfo(GlobalVars.VFR vfr)
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
                logger.Trace("Entering into UpdateVFRInfo Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Customer Names must be unique in the Database. The Name could be change but it must be unique
                    Vfr Matching_Result = DB.Vfr.FirstOrDefault(x => x.JobId == vfr.JobID);
                    Vfr record = new Vfr();
                    record.Cadiurl = vfr.CADIUrl;
                    record.CaptureTemplate = vfr.CaptureTemplate;
                    record.InstanceName = vfr.InstanceName;
                    record.JobId = vfr.JobID;
                    //record.SettingId = vfr.SettingID;
                    record.Password = vfr.Password;
                    record.UserName = vfr.UserName;
                    record.RepositoryName = vfr.RepositoryName;
                    record.QueryField = vfr.QueryField;
                    if (Matching_Result == null)
                    {
                        DB.Vfr.Add(record);
                        DB.SaveChanges();
                        result.Message = "There was not information associated to an VFR Server, so new records was created successfully.";
                    }
                    else
                    {
                        // Means --> table has a record and it will be updated
                        Matching_Result.Cadiurl = vfr.CADIUrl;
                        Matching_Result.CaptureTemplate = vfr.CaptureTemplate;
                        Matching_Result.InstanceName = vfr.InstanceName;
                        Matching_Result.JobId = vfr.JobID;
                        //Matching_Result.SettingId = vfr.SettingID;
                        Matching_Result.Password = vfr.Password;
                        Matching_Result.UserName = vfr.UserName;
                        Matching_Result.RepositoryName = vfr.RepositoryName;
                        Matching_Result.QueryField = vfr.QueryField;

                        DB.SaveChanges();
                        result.Message = "VFR Inforation was updated successfully.";
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
            logger.Trace("Leaving UpdateVFRInfo Method ...");
            return result;
        }

        /// <summary>
        /// Remove VFR Info formation a Given Job
        /// </summary>
        /// <param name="jobID"></param>
        static public GlobalVars.ResultGeneric DeleteVFRInfo(int jobID)
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
                logger.Trace("Entering into DeleteVFRInfo Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    Vfr Matching_Result = DB.Vfr.FirstOrDefault(x => x.JobId == jobID);
                    if (Matching_Result != null)
                    {
                        DB.Vfr.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteVFRInfo transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Job ID" + jobID + " does not exist. DeleteVFRInfo transaction ignore.";
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
            logger.Trace("Leaving DeleteVFRInfo Method ...");
            return result;
        }

    }
}
