using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;

namespace ScanningServices.Models
{
    public class SQLFunctionsFields
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get List of Fields for a given Job ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultFields GetFieldsByJobID(int jobID)
        {
            List<GlobalVars.Field> fields = new List<GlobalVars.Field>();
            GlobalVars.ResultFields resultFields = new GlobalVars.ResultFields()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = fields,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetFieldsByJobID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.JobsFields.Where(x => x.JobId == jobID);
                    resultFields.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Field field = new GlobalVars.Field()
                            {
                                JobID = x.JobId,
                                FieldID = x.FieldId,
                                CPFieldName = (x.CpfieldName ?? "").Trim(),
                                VFRFieldName = (x.VfrfieldName ?? "").Trim(),
                                ExcludeFromKeystrokesCount = Convert.ToBoolean(x.KeyStrokeExcludeFlag)
                            };
                            fields.Add(field);
                        }
                    }
                }
                resultFields.ReturnValue = fields;
                resultFields.Message = "GetFieldsByJobID transaction completed successfully. Number of records found: " + resultFields.RecordsCount;
                logger.Debug(resultFields.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultFields.ReturnCode = -2;
                resultFields.Message = e.Message;
                var baseException = e.GetBaseException();
                resultFields.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetFieldsByJobID Method ...");
            return resultFields;
        }

        /// <summary>
        /// Get List of Fields for a given Job ID and FieldName
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultFields GetFieldsByNameAndJobID(string fieldName, int jobID)
        {
            List<GlobalVars.Field> fields = new List<GlobalVars.Field>();
            GlobalVars.ResultFields resultFields = new GlobalVars.ResultFields()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = fields,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetFieldsByNameAndJobID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.JobsFields.Where(x => x.JobId == jobID & x.CpfieldName == fieldName);
                    resultFields.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Field field = new GlobalVars.Field()
                            {
                                JobID = x.JobId,
                                FieldID = x.FieldId,
                                CPFieldName = (x.CpfieldName ?? "").Trim(),
                                VFRFieldName = (x.VfrfieldName ?? "").Trim(),
                                ExcludeFromKeystrokesCount = Convert.ToBoolean(x.KeyStrokeExcludeFlag)
                            };
                            fields.Add(field);
                        }
                    }
                }
                resultFields.ReturnValue = fields;
                resultFields.Message = "GetFieldsByNameAndJobID transaction completed successfully. Number of records found: " + resultFields.RecordsCount;
                logger.Debug(resultFields.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultFields.ReturnCode = -2;
                resultFields.Message = e.Message;
                var baseException = e.GetBaseException();
                resultFields.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetFieldsByNameAndJobID Method ...");
            return resultFields;
        }

        /// <summary>
        /// Get Field Information for a given Field ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultFields GetFieldByID(int fieldID)
        {
            List<GlobalVars.Field> fields = new List<GlobalVars.Field>();
            GlobalVars.ResultFields resultFields = new GlobalVars.ResultFields()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = fields,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetFieldByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.JobsFields.Where(x => x.FieldId == fieldID);
                    resultFields.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.Field field = new GlobalVars.Field()
                            {
                                JobID = x.JobId,
                                FieldID = x.FieldId,
                                CPFieldName = (x.CpfieldName ?? "").Trim(),
                                VFRFieldName = (x.VfrfieldName ?? "").Trim(),
                                ExcludeFromKeystrokesCount = Convert.ToBoolean(x.KeyStrokeExcludeFlag)
                            };
                            fields.Add(field);
                        }
                    }
                }
                resultFields.ReturnValue = fields;
                resultFields.Message = "GetFieldByID transaction completed successfully. Number of records found: " + resultFields.RecordsCount;
                logger.Debug(resultFields.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultFields.ReturnCode = -2;
                resultFields.Message = e.Message;
                var baseException = e.GetBaseException();
                resultFields.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetFieldByID Method ...");
            return resultFields;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistFieldName(int jobID, string fieldName)
        {
            List<GlobalVars.Field> fields = new List<GlobalVars.Field>();

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistJobFieldName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.JobsFields.Where(x => x.JobId == jobID & x.CpfieldName == fieldName);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Field Name" + fieldName + " for Job ID " + jobID + " already exist.";
                    }
                    else
                    {
                        result.Message = "Field Name" + fieldName + " for Job ID " + jobID + "  doest not exist.";
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
            logger.Trace("Leaving ExistJobFieldName Method ...");
            return result;
        }

        /// <summary>
        /// Create a new Field
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric NewField(GlobalVars.Field field)
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
                logger.Trace("Entering into NewJobField Method ...");

                // Chedck if given Job ID Field does exist
                result = SQLFunctionsJobs.ExistJobID(field.JobID);
                if (result.RecordsCount != 0)
                {
                    // Check if Job's Field Name Exist
                    result = ExistFieldName(field.JobID, field.CPFieldName);
                    if (result.RecordsCount == 0)
                    {
                        // Create new Field
                        using (ScanningDBContext DB = new ScanningDBContext())
                        {
                            JobsFields New_Record = new JobsFields();
                            New_Record.CpfieldName = field.CPFieldName;
                            New_Record.VfrfieldName = field.VFRFieldName;
                            New_Record.KeyStrokeExcludeFlag = field.ExcludeFromKeystrokesCount.ToString();
                            New_Record.JobId = field.JobID;

                            DB.JobsFields.Add(New_Record);
                            DB.SaveChanges();
                        }
                        result.Message = "NewJobField transaction completed successfully. One Record added.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Field " + field.CPFieldName.Trim()  + " for Job id " + field.JobID.ToString().Trim() + " already exist. NewJobField transaction will be ignored.";
                    }
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "JobID " + field.JobID.ToString().Trim() + " does not exist. NewJobField transaction will be ignored.";
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
            logger.Trace("Leaving NewJobField Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateField(GlobalVars.Field field)
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
                logger.Trace("Entering into UpdateField Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Chedck if record exist in the Database
                    JobsFields Matching_Result = DB.JobsFields.FirstOrDefault(x => x.JobId == field.JobID & x.FieldId == field.FieldID);
                    if (Matching_Result != null)
                    {
                        // Means that Field exist in the Database. 
                        // then, check if the field Name has changed
                        Matching_Result = DB.JobsFields.FirstOrDefault(x => x.JobId == field.JobID & x.FieldId == field.FieldID & x.CpfieldName == field.CPFieldName);
                        if (Matching_Result != null)
                        {
                            // Means that Field Name remain the same
                            Matching_Result.VfrfieldName = field.VFRFieldName;
                            Matching_Result.KeyStrokeExcludeFlag = field.ExcludeFromKeystrokesCount.ToString();
                            DB.SaveChanges();
                            result.Message = "UpdateField transaction completed successfully. One Record Updated.";
                        }
                        else
                        {
                            // Means that the Field Name has Changed, so check if t he name has already been taking by another field
                            Matching_Result = DB.JobsFields.FirstOrDefault(x => x.JobId == field.JobID & x.FieldId != field.FieldID & x.CpfieldName == field.CPFieldName);
                            if (Matching_Result == null)
                            {
                                // Means that Field Name has changed
                                // Look for the record in the datbase so it can be updated
                                Matching_Result = DB.JobsFields.FirstOrDefault(x => x.JobId == field.JobID & x.FieldId == field.FieldID);
                                if (Matching_Result != null)
                                {
                                    Matching_Result.CpfieldName = field.CPFieldName;
                                    Matching_Result.VfrfieldName = field.VFRFieldName;
                                    Matching_Result.KeyStrokeExcludeFlag = field.ExcludeFromKeystrokesCount.ToString();
                                    DB.SaveChanges();
                                    result.Message = "UpdateField transaction completed successfully. One Record Updated.";
                                }   
                            }
                            else
                            {
                                result.ReturnCode = -1;
                                result.Message = "Field name " + field.CPFieldName.Trim() + " with Job ID " + field.JobID.ToString().Trim() + " already exists for this Job. UpdateField transaction ignore.";
                            }                            
                        }
                    }
                    else
                    {
                        // Means --> The field does not exist in the Database
                        result.ReturnCode = -1;
                        result.Message = "Field " + field.CPFieldName.Trim() + " with Job ID " + field.JobID.ToString().Trim() + " does not exist. UpdateField transaction ignore.";
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
            logger.Trace("Leaving UpdateField Method ...");
            return result;
        }

        /// <summary>
        /// Remove Field and associated information from Database
        /// </summary>
        /// <param name="fieldID"></param>
        static public GlobalVars.ResultGeneric DeleteField(int fieldID)
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
                logger.Trace("Entering into DeleteField Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    JobsFields Matching_Result = DB.JobsFields.FirstOrDefault(x => x.FieldId == fieldID);
                    if (Matching_Result != null)
                    {
                        DB.JobsFields.Remove(Matching_Result);
                        DB.SaveChanges();
                        result.Message = "DeleteField transaction completed successfully. One Record Deleted.";
                    }
                    else
                    {
                        result.ReturnCode = -1;
                        result.Message = "Fild ID" + fieldID + " does not exist. DeleteField transaction ignore.";
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
            logger.Trace("Leaving DeleteField Method ...");
            return result;
        }
    }
}
