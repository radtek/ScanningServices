using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using static ScanningServices.Models.GeneralTools;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLFunctionsExportRules
    {
        static object lockObj = new object();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get Export Rules for a given Job ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultExportRules GetExportRulesByJobID(int jobID)
        {
            List<GlobalVars.ExportRules> exportRules = new List<GlobalVars.ExportRules>();
            GlobalVars.ResultExportRules resultExportRules = new GlobalVars.ResultExportRules()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = exportRules,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetExportRulesByJobID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = from je in DB.ExportRules
                                  join j in DB.Jobs on je.JobId equals j.JobId
                                  where je.JobId == jobID
                                  select new { je, j.JobName };

                    resultExportRules.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ExportRules exportRule = new GlobalVars.ExportRules()
                            {
                                JobID = x.je.JobId,
                                OutputFileFormat = x.je.OutputFileFormat,
                                OutputFileDelimeter = x.je.OutputFileDelimeter,
                                MetadataFileName = x.je.MetadataFileName,
                                UseQuotationAroundFlag = Convert.ToBoolean(x.je.UseQuotationAroundFlag),
                                IncludeHeaderFlag = Convert.ToBoolean(x.je.IncludeHeaderFlag),
                                JobName = x.JobName
                            };
                            if (!string.IsNullOrEmpty(x.je.DirectoryFormat))
                            {
                                exportRule.DirectoryFormat = x.je.DirectoryFormat.Split((char)248).ToList();
                            }
                            else
                            {
                                exportRule.DirectoryFormat = new List<string>();
                            }
                            if (!string.IsNullOrEmpty(x.je.FileNameFormat))
                            {
                                exportRule.FileNameFormat = x.je.FileNameFormat.Split((char)248).ToList();
                            }
                            else
                            {
                                exportRule.FileNameFormat = new List<string>();
                            }
                            //exportRule.MetadataDirectoryFormat = new List<string>();
                            if (!string.IsNullOrEmpty(x.je.MetadataOutputDirectoryFormat))
                            {
                                exportRule.MetadataDirectoryFormat = x.je.MetadataOutputDirectoryFormat.Split((char)248).ToList();
                            }
                            else
                            {
                                exportRule.MetadataDirectoryFormat = new List<string>();
                            }
                            if (!string.IsNullOrEmpty(x.je.OutputFields))
                            {
                                exportRule.OutputFields = x.je.OutputFields.Split((char)248).ToList();
                                List<string> fieldsTempList = new List<string>();
                                foreach (string fieldName in exportRule.OutputFields)
                                {
                                    fieldsTempList.Add(fieldName.Trim());
                                }
                                exportRule.OutputFields = fieldsTempList;
                            }
                            else
                            {
                                exportRule.OutputFields = new List<string>();
                            }
                            List<string> tokens = new List<string>();
                            exportRule.DirectoryReplaceRule = new List<GlobalVars.ReplaceToken>();
                            tokens = x.je.DirectoryReplaceRule.Split((char)248).ToList();
                            if (tokens[0].Length != 0)
                            {
                                foreach (var token in tokens)
                                {
                                    GlobalVars.ReplaceToken replaceToken = new GlobalVars.ReplaceToken();
                                    var subToken = token.Split((char)164).ToList();
                                    replaceToken.Name = "";
                                    replaceToken.Pattern = subToken[0].Replace("\"", "'");
                                    replaceToken.ReplaceBy = subToken[1].Replace("\"", "'");
                                    exportRule.DirectoryReplaceRule.Add(replaceToken);
                                }
                            }

                            exportRule.FileNameReplaceRule = new List<GlobalVars.ReplaceToken>();
                            tokens = x.je.FileNameReplaceRule.Split((char)248).ToList();
                            if (tokens[0].Length != 0)
                            {
                                foreach (var token in tokens)
                                {
                                    GlobalVars.ReplaceToken replaceToken = new GlobalVars.ReplaceToken();
                                    var subToken = token.Split((char)164).ToList();
                                    replaceToken.Name = "";
                                    replaceToken.Pattern = subToken[0].Replace("\"", "'");
                                    replaceToken.ReplaceBy = subToken[1].Replace("\"", "'");
                                    exportRule.FileNameReplaceRule.Add(replaceToken);
                                }
                            }

                            exportRule.FieldsReplaceRule = new List<GlobalVars.ReplaceToken>();
                            tokens = x.je.FieldsReplaceRule.Split((char)248).ToList();
                            if (tokens[0].Length != 0)
                            {
                                foreach (var token in tokens)
                                {
                                    GlobalVars.ReplaceToken replaceToken = new GlobalVars.ReplaceToken();
                                    var subToken = token.Split((char)164).ToList();
                                    replaceToken.Name = subToken[0];
                                    replaceToken.Pattern = subToken[1].Replace("\"", "'");
                                    replaceToken.ReplaceBy = subToken[2].Replace("\"", "'");
                                    exportRule.FieldsReplaceRule.Add(replaceToken);
                                }
                            }

                            exportRules.Add(exportRule);
                        }
                    }
                }
                resultExportRules.ReturnValue = exportRules;
                resultExportRules.Message = "GetExportRulesByJobID transaction completed successfully. Number of records found: " + resultExportRules.RecordsCount;
                logger.Debug(resultExportRules.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultExportRules.ReturnCode = -2;
                resultExportRules.Message = e.Message;
                var baseException = e.GetBaseException();
                resultExportRules.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetExportRulesByJobID Method ...");
            return resultExportRules;
        }

        /// <summary>
        /// Creates a new Export Rule for a Job
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateExportRule(GlobalVars.ExportRules exportRule)
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
                logger.Trace("Entering into UpdateExportRule Method ...");

                // List of Items we need to check
                // 1. If Job ID exist
                // 2. Duplicate File Action values { Replace, Ignore, Append Sequence }
                // 3. Output File Format values { Document XML, CSV }
                // 4. Metadata File Scope values { One per Batch, One per Export }

                //Check if Job ID is valid
                GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
                resultJobs = SQLFunctionsJobs.GetJobByID(exportRule.JobID);
                if (resultJobs.RecordsCount != 0)
                {
                    // Given Job ID exist .. continue ...
                    //  Duplicate File Action value
                    //  Output File Format value
                    if (exportRule.OutputFileFormat == "XML" || exportRule.OutputFileFormat == "CSV")
                    {
                        // Validation passed, continue
                        using (ScanningDBContext DB = new ScanningDBContext())
                        {
                            // Jobs's Export Rule  must be unique in the Database. 
                            ExportRules Matching_Result = DB.ExportRules.FirstOrDefault(x => x.JobId == exportRule.JobID);
                            ExportRules record = new ExportRules();
                            record.JobId = exportRule.JobID;

                            // Rules used to build tokens in Database Table
                            // 0. We will use to no writable caharcaters to operate with special fields in the database:
                            //    Code: 164: "¤" is used to separate token information
                            //    Code: 248: " ø " is used to separate tokens
                            // 1. DirectoryFormat and FileName Format fields in the database are made of a squence of tokens with the folloing form:
                            //      {String} + " ø  " + {string} + " ø  " + {string} + ..... + " ø  " + {string}
                            // 2. DirectoryReplaceRule and FileNameReplaceRule format fields in the databse are made of a sequence of tokens with the following form:
                            //      { Pattern String} + ¤ + {Replace by string} +  ..... + " ø  " + { Pattern String} + ¤ + {Replace by string}
                            // 3. FieldsReplaceRule format fiel in the Database is made of a sequence tokens with the following form:
                            //      { Field Name} + ¤ + { Pattern String} + ¤ + {Replace by string} + .... + " ø  " + { Field Name} + ¤ + { Pattern String} + ¤ + {Replace by string}

                            // Somem Samples:
                            //  "ABC¤XYZ" --> use to replace "ABC" pattern by "XYZ"
                            //  "ABC¤XYZ ø DEG¤WK" --> This string has 2 tokens: "ABC¤XYZ" and "DEG¤WK"
                            //  "First Name¤ABC¤XYZ" --> use to replace the patter "ABC" by the string "XYZ" in the content of a field name called "First Name"

                            record.MetadataOutputDirectoryFormat = "";
                            if (exportRule.MetadataDirectoryFormat != null)
                            {
                                foreach (string token in exportRule.MetadataDirectoryFormat)
                                {
                                    if (record.MetadataOutputDirectoryFormat.Length == 0)
                                        record.MetadataOutputDirectoryFormat = token;
                                    else
                                        record.MetadataOutputDirectoryFormat = record.MetadataOutputDirectoryFormat + " " + (char)248 + " " + token.Trim();
                                }
                            }                                

                            record.DirectoryFormat = "";
                            if (exportRule.DirectoryFormat != null)
                            {
                                foreach (string token in exportRule.DirectoryFormat)
                                {
                                    if (record.DirectoryFormat.Length == 0)
                                        record.DirectoryFormat = token;
                                    else
                                        record.DirectoryFormat = record.DirectoryFormat + " " + (char)248 + " " + token.Trim();
                                }
                            }                                

                            record.FileNameFormat = "";
                            if (exportRule.FileNameFormat != null)
                            {
                                foreach (string token in exportRule.FileNameFormat)
                                {
                                    if (record.FileNameFormat.Length == 0)
                                        record.FileNameFormat = token;
                                    else
                                        record.FileNameFormat = record.FileNameFormat + " " + (char)248 + " " + token.Trim();
                                }
                            }

                            record.DirectoryReplaceRule = "";
                            if (exportRule.DirectoryReplaceRule != null)
                            {
                                foreach (GlobalVars.ReplaceToken token in exportRule.DirectoryReplaceRule)
                                {
                                    if (record.DirectoryReplaceRule.Length == 0)
                                        record.DirectoryReplaceRule = token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                    else
                                        record.DirectoryReplaceRule = record.DirectoryReplaceRule + " " + (char)248 + " " + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                }
                            }                                

                            record.FileNameReplaceRule = "";
                            if (exportRule.FileNameReplaceRule != null)
                            {
                                foreach (GlobalVars.ReplaceToken token in exportRule.FileNameReplaceRule)
                                {
                                    if (record.FileNameReplaceRule.Length == 0)
                                        record.FileNameReplaceRule = token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                    else
                                        record.FileNameReplaceRule = record.FileNameReplaceRule + " " + (char)248 + " " + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                }
                            }                                                               

                            record.FieldsReplaceRule = "";
                            if (exportRule.FieldsReplaceRule != null)
                            {
                                foreach (GlobalVars.ReplaceToken token in exportRule.FieldsReplaceRule)
                                {
                                    if (record.FieldsReplaceRule.Length == 0)
                                        record.FieldsReplaceRule = token.Name.Trim() + (char)164 + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                    else
                                        record.FieldsReplaceRule = record.FieldsReplaceRule + " " + (char)248 + " " + token.Name.Trim() + (char)164 + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                }
                            }                                

                            record.OutputFields = "";
                            if (exportRule.OutputFields != null)
                            {
                                foreach (string token in exportRule.OutputFields)
                                {
                                    if (record.OutputFields.Length == 0)
                                        record.OutputFields = token;
                                    else
                                        record.OutputFields = record.OutputFields + " " + (char)248 + " " + token.Trim();
                                }
                            }

                            record.OutputFileFormat = exportRule.OutputFileFormat;
                            record.OutputFileDelimeter = exportRule.OutputFileDelimeter;
                            record.IncludeHeaderFlag = Convert.ToString(exportRule.IncludeHeaderFlag);
                            record.UseQuotationAroundFlag = Convert.ToString(exportRule.UseQuotationAroundFlag);
                            record.MetadataFileName = exportRule.MetadataFileName;

                            if (Matching_Result == null)
                            {
                                // Means --> the Job Export Rule given was not found in the Database so a new record will be created
                                DB.ExportRules.Add(record);
                                DB.SaveChanges();
                                result.Message = "There was not information associated to the given Job Export Rule, so a new record was created successfully.";
                            }
                            else
                            {
                                // Means --> table has a record and it will be updated
                                Matching_Result.MetadataOutputDirectoryFormat = "";
                                if (exportRule.MetadataDirectoryFormat != null)
                                {
                                    foreach (string token in exportRule.MetadataDirectoryFormat)
                                    {
                                        if (Matching_Result.MetadataOutputDirectoryFormat.Length == 0)
                                            Matching_Result.MetadataOutputDirectoryFormat = token;
                                        else
                                            Matching_Result.MetadataOutputDirectoryFormat = Matching_Result.MetadataOutputDirectoryFormat + " " + (char)248 + " " + token.Trim();
                                    }
                                }                                    

                                Matching_Result.DirectoryFormat = "";
                                if (exportRule.DirectoryFormat != null)
                                {
                                    foreach (string token in exportRule.DirectoryFormat)
                                    {
                                        if (Matching_Result.DirectoryFormat.Length == 0)
                                            Matching_Result.DirectoryFormat = token;
                                        else
                                            Matching_Result.DirectoryFormat = Matching_Result.DirectoryFormat + " " + (char)248 + " " + token.Trim();
                                    }
                                }
                                    
                                Matching_Result.FileNameFormat = "";
                                if (exportRule.FileNameFormat != null)
                                {
                                    foreach (string token in exportRule.FileNameFormat)
                                    {
                                        if (Matching_Result.FileNameFormat.Length == 0)
                                            Matching_Result.FileNameFormat = token;
                                        else
                                            Matching_Result.FileNameFormat = Matching_Result.FileNameFormat + " " + (char)248 + " " + token.Trim();
                                    }
                                }

                                Matching_Result.DirectoryReplaceRule = "";
                                if (exportRule.DirectoryReplaceRule != null)
                                {
                                    foreach (GlobalVars.ReplaceToken token in exportRule.DirectoryReplaceRule)
                                    {
                                        if (Matching_Result.DirectoryReplaceRule.Length == 0)
                                            Matching_Result.DirectoryReplaceRule = token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                        else
                                            Matching_Result.DirectoryReplaceRule = Matching_Result.DirectoryReplaceRule + " " + (char)248 + " " + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                    }
                                }                               

                                Matching_Result.FileNameReplaceRule = "";
                                if (exportRule.FileNameReplaceRule != null)
                                {
                                    foreach (GlobalVars.ReplaceToken token in exportRule.FileNameReplaceRule)
                                    {
                                        if (Matching_Result.FileNameReplaceRule.Length == 0)
                                            Matching_Result.FileNameReplaceRule = token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                        else
                                            Matching_Result.FileNameReplaceRule = Matching_Result.FileNameReplaceRule + " " + (char)248 + " " + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                    }
                                }                                        

                                Matching_Result.FieldsReplaceRule = "";
                                if (exportRule.FieldsReplaceRule != null)
                                {
                                    foreach (GlobalVars.ReplaceToken token in exportRule.FieldsReplaceRule)
                                    {
                                        if (Matching_Result.FieldsReplaceRule.Length == 0)
                                            Matching_Result.FieldsReplaceRule = token.Name.Trim() + (char)164 + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                        else
                                            Matching_Result.FieldsReplaceRule = Matching_Result.FieldsReplaceRule + " " + (char)248 + " " + token.Name.Trim() + (char)164 + token.Pattern.Trim() + (char)164 + token.ReplaceBy.Trim();
                                    }
                                }

                                Matching_Result.OutputFields = "";
                                if (exportRule.OutputFields != null)
                                {
                                    foreach (string token in exportRule.OutputFields)
                                    {
                                        if (Matching_Result.OutputFields.Length == 0)
                                            Matching_Result.OutputFields = token;
                                        else
                                            Matching_Result.OutputFields = Matching_Result.OutputFields + " " + (char)248 + " " + token.Trim();
                                    }
                                }

                                Matching_Result.OutputFileFormat = exportRule.OutputFileFormat;
                                Matching_Result.OutputFileDelimeter = exportRule.OutputFileDelimeter;
                                Matching_Result.IncludeHeaderFlag = Convert.ToString(exportRule.IncludeHeaderFlag);
                                Matching_Result.UseQuotationAroundFlag = Convert.ToString(exportRule.UseQuotationAroundFlag);
                                Matching_Result.MetadataFileName = exportRule.MetadataFileName;
                                DB.SaveChanges();
                                result.Message = "UpdateExportRule transaction completed successfully. One Record added.";
                            }
                        }
                            
                        }
                        else
                        {
                            result.ReturnCode = -1;
                            result.Message = "Output File Format field value is invaid. Use one of he followoing values { Document XML, CSV }";
                        }
                    
                }
                else
                {
                    result.ReturnCode = -1;
                    result.Message = "Could not find Job ID " + exportRule.JobID.ToString() + " in the Database.";
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
            logger.Trace("Leaving UpdateExportRule Method ...");
            return result;
        }

        /// <summary>
        /// THis method generates an Export Transactions File that a client can used to deliver Batches to Customers
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultExportTransactionsJob GetExportTransactionsJob(int jobID, string workOrder, string batchName, string baseOutputDirectory)
        {
            int totalNumDocuments = 0;
            //string sampleOututFile = "";
            List<string> filesNamesCheckList = new List<string>();
            List<string> outputFilesNamesCheckList = new List<string>();
            Boolean continueProcessing = true;
            GlobalVars.ExportTransactionsJob exportTransactions = new GlobalVars.ExportTransactionsJob();
            GlobalVars.ResultExportTransactionsJob result = new GlobalVars.ResultExportTransactionsJob()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetExportTransactionsJob Method ...");
                if (!baseOutputDirectory.EndsWith("\\"))
                {
                    baseOutputDirectory = baseOutputDirectory +"\\";
                }
                // 1. Getting Job Export Rules to be used to generated the transactions job 
                logger.Info("   Getting Job Export Rules to be used to generated the transactions job ...");
                GlobalVars.ResultExportRules resultExportRules = new GlobalVars.ResultExportRules();
                resultExportRules = SQLFunctionsExportRules.GetExportRulesByJobID(jobID);
                if (resultExportRules.RecordsCount != 0)
                {
                    exportTransactions.ExportRule = new GlobalVars.ExportRules();
                    exportTransactions.ExportRule = resultExportRules.ReturnValue[0];                    
                }
                else
                {
                    continueProcessing = false;
                    result.ReturnCode = -1;
                    result.Message = "Could not find Export Rules for Job ID " + jobID.ToString() + " in the Database.";
                    logger.Debug(result.Message);
                }

                // 2. Getting Job Information
                if (continueProcessing)
                {
                    logger.Info("    Getting Job Information for Job ID: " + jobID.ToString());
                    GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
                    resultJobs = SQLFunctionsJobs.GetJobByID(jobID);
                    if (resultExportRules.RecordsCount != 0)
                    {
                        exportTransactions.JobType = resultJobs.ReturnValue[0].JobName.Trim();
                        exportTransactions.ProjectName = resultJobs.ReturnValue[0].ProjectName.Trim();
                        exportTransactions.CustomerName = resultJobs.ReturnValue[0].CustomerName.Trim();                       
                    }
                    else
                    {
                        continueProcessing = false;
                        result.ReturnCode = -1;
                        result.Message = "Could not find Job Information for Job ID " + jobID.ToString() + " in the Database.";
                        logger.Debug(result.Message);
                    }
                }

                // 3. Getting Document Batch Information either by Work Order or Batch Name
                if (continueProcessing)
                {
                    logger.Info("    Getting Document Batch Information either by Work Order or Batch Name ... ");
                    // if the Work Order is given, query the database to get Batches for this Work Order
                    // Otherwise, if Batcn Name is given, query the Database to get just this Batch Information
                    if (!string.IsNullOrEmpty(workOrder))
                    {
                        logger.Info("       Work Order: " + workOrder);
                        exportTransactions.WorkOrder = workOrder;
                        GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                        resultBatches = SQLFunctionsBatches.GetBatchesInformation("LotNumber =\"" + workOrder + "\"", "BatchNumber");
                        result.RecordsCount = resultBatches.RecordsCount;
                        if (resultBatches.RecordsCount != 0)
                        {
                            exportTransactions.Batches = new List<GlobalVars.ExportBatches>();
                            GlobalVars.ResultBatchDocs resultBatchDocs = new GlobalVars.ResultBatchDocs();
                            foreach (GlobalVars.Batch batch in resultBatches.ReturnValue)
                            {
                                GlobalVars.ExportBatches exportBatch = new GlobalVars.ExportBatches();
                                exportBatch.BatchName = batch.BatchNumber.Trim();
                                logger.Info("           Getting Documents Information for Batch Namer: " + exportBatch.BatchName);
                                resultBatchDocs = SQLFunctionsBatches.GetBatchDocInformation("BatchName =\"" + exportBatch.BatchName + "\"", "BatchName");
                                logger.Info("           Number of Documents found: " + resultExportRules.RecordsCount.ToString());
                                if (resultExportRules.RecordsCount != 0)
                                {
                                    totalNumDocuments += resultBatchDocs.RecordsCount;
                                    exportBatch.Documents = new List<GlobalVars.ExportDocs>();
                                    foreach (GlobalVars.BatchDocs batchDoc in resultBatchDocs.ReturnValue)
                                    {
                                        GlobalVars.ExportDocs document = new GlobalVars.ExportDocs();
                                        document.Fields = batchDoc.DocMetadata;
                                        document.FileName = batchDoc.DocumentFileName;
                                        document.FileLocation = batchDoc.DocumentLocation;
                                        document.TargetFileLocation = "";
                                        document.TargetFilename = "";
                                        exportBatch.Documents.Add(document);
                                    }
                                }
                                else
                                {
                                    continueProcessing = false;
                                    result.ReturnCode = -1;
                                    result.Message = "Could not find Documents in Batch Name " + exportBatch.BatchName + " for Work Order " + workOrder + " in the Database.";
                                    logger.Debug(result.Message);
                                }                                
                                exportTransactions.Batches.Add(exportBatch);
                            }
                        }
                        else
                        {
                            continueProcessing = false;
                            result.ReturnCode = -1;
                            result.Message = "Could not find Batches for a given Work Order " + workOrder + " in the Database.";
                            logger.Debug(result.Message);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(batchName))
                        {
                            logger.Info("       Batch Name: " + batchName);
                            // Add the logic to retrieve one batch by Batch Name
                            GlobalVars.ExportBatches exportBatch = new GlobalVars.ExportBatches();
                            exportBatch.BatchName = batchName;

                            GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                            // Quey Batch Table to get Work order
                            resultBatches = SQLFunctionsBatches.GetBatchesInformation("BatchNumber =\"" + batchName + "\"", "BatchNumber");
                            result.RecordsCount = resultBatches.RecordsCount;
                            if (resultBatches.RecordsCount != 0)
                            {
                                exportTransactions.WorkOrder = Convert.ToString(resultBatches.ReturnValue[0].LotNumber);

                                GlobalVars.ResultBatchDocs resultBatchDocs = new GlobalVars.ResultBatchDocs();
                                logger.Info("           Getting Documents Information for Batch Namer ...");
                                resultBatchDocs = SQLFunctionsBatches.GetBatchDocInformation("BatchName =\"" + exportBatch.BatchName + "\"", "BatchName");
                                result.RecordsCount = resultBatchDocs.RecordsCount;
                                logger.Info("           Number of Documents found: " + resultExportRules.RecordsCount.ToString());
                                if (resultBatchDocs.RecordsCount != 0)
                                {
                                    totalNumDocuments += resultBatchDocs.RecordsCount;
                                    result.RecordsCount = 1;
                                    exportBatch.Documents = new List<GlobalVars.ExportDocs>();
                                    foreach (GlobalVars.BatchDocs batchDoc in resultBatchDocs.ReturnValue)
                                    {
                                        GlobalVars.ExportDocs document = new GlobalVars.ExportDocs();
                                        document.Fields = batchDoc.DocMetadata;
                                        document.FileName = batchDoc.DocumentFileName;
                                        document.FileLocation = batchDoc.DocumentLocation;
                                        document.TargetFileLocation = "";
                                        document.TargetFilename = "";
                                        exportBatch.Documents.Add(document);
                                    }
                                }
                                else
                                {
                                    continueProcessing = false;
                                    result.ReturnCode = -1;
                                    result.Message = "Could not find Documents for a given Batch Name " + exportBatch.BatchName + " in the Database.";
                                    logger.Debug(result.Message);
                                }
                                exportTransactions.Batches = new List<GlobalVars.ExportBatches>();
                                exportTransactions.Batches.Add(exportBatch);
                            }
                            else
                            {
                                continueProcessing = false;
                                result.ReturnCode = -1;
                                result.Message = "Could not find Documents in Batch Name " + batchName + " in the Database.";
                                logger.Debug(result.Message);
                            }                            
                        }
                    }
                }

                // 4- Apply Metadata Transformation using replace rules
                if (continueProcessing)
                {
                    logger.Info("    Applying Metadata Transformation using replace rules ... ");
                    foreach (GlobalVars.ExportBatches batch in exportTransactions.Batches)
                    {
                        foreach (GlobalVars.ReplaceToken token in exportTransactions.ExportRule.FieldsReplaceRule)
                        {
                            string fieldName = token.Name.Trim();
                            // Remove Begin and End single quotes from string pattern  
                            string pattern = token.Pattern.Trim();
                            //pattern = pattern.Remove(token.Pattern.Length - 1, 1);
                            pattern = pattern.Remove(pattern.Length - 1, 1);
                            pattern = pattern.Substring(1);
                            string replaceBy = token.ReplaceBy.Trim();
                            replaceBy = replaceBy.Remove(replaceBy.Length - 1, 1);
                            replaceBy = replaceBy.Substring(1);
                            // Traverse Batch Documents
                            foreach (GlobalVars.ExportDocs document in batch.Documents)
                            {
                                // Traverse Document Metatara
                                foreach (GlobalVars.Metadata metadata in document.Fields)
                                {
                                    // Check for Field Name
                                    if (metadata.FieldName.Trim() == fieldName)
                                    {
                                        // Field Name found so apply the replace string rules                         
                                        if (pattern == "")
                                            metadata.FieldValue = token.ReplaceBy.Trim();
                                        else
                                            metadata.FieldValue = metadata.FieldValue.Replace(pattern, replaceBy);
                                        break;
                                    }
                                }
                            }
                        }
                    }                    
                }

                // 5- Build Target Directory based on Directory Format Rule
                if (continueProcessing)
                {
                    logger.Info("    Building Target Directory based on Directory Format Rule ... ");
                    foreach (GlobalVars.ExportBatches batch in exportTransactions.Batches)
                    {
                        // get directory path, use the following parameters export.rules + Document Metadata + transaction job (contains system variable information)  
                        // Traverse Batch Documents
                        logger.Info("       Batch: " + batch.BatchName);
                        foreach (GlobalVars.ExportDocs document in batch.Documents)
                        {
                            string directory = "";
                            string fieldTypeIdentifier = "";
                            string parameter = "";
                            string pattern = "";
                            string replaceBy = "";
                            foreach (string item in exportTransactions.ExportRule.DirectoryFormat)
                            {
                                fieldTypeIdentifier = item.Trim().Substring(0, 1);

                                switch (fieldTypeIdentifier)
                                {
                                    case "[":
                                        // This is a System Varibale Field
                                        parameter = item.Trim();
                                        parameter = parameter.Remove(parameter.Length - 1, 1);
                                        parameter = parameter.Substring(1);
                                        switch (parameter)
                                        {
                                            case "Customer Name":
                                                if (string.IsNullOrEmpty(exportTransactions.CustomerName))
                                                    directory = directory + "CUSTOMER-NAME";
                                                else
                                                    directory = directory + exportTransactions.CustomerName.Trim();
                                                break;
                                            case "Project Name":
                                                if (string.IsNullOrEmpty(exportTransactions.ProjectName))
                                                    directory = directory + "PROJECT-NAME";
                                                else
                                                    directory = directory + exportTransactions.ProjectName.Trim();
                                                break;
                                            case "Job Type":
                                                if (string.IsNullOrEmpty(exportTransactions.JobType))
                                                    directory = directory + "JOB-TYPE";
                                                else
                                                    directory = directory + exportTransactions.JobType.Trim();
                                                break;
                                            case "Work Order":
                                                if (string.IsNullOrEmpty(exportTransactions.WorkOrder))
                                                    directory = directory + "WORK-ORDER";
                                                else
                                                    directory = directory + exportTransactions.WorkOrder.Trim();
                                                break;
                                            case "Batch Name":
                                                if (string.IsNullOrEmpty(batch.BatchName))
                                                    directory = directory + "BATCH-NAME";
                                                else
                                                    directory = directory + batch.BatchName.Trim();
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    case "{":
                                        // This is a batch Variable Field
                                        parameter = item.Trim();
                                        parameter = parameter.Remove(parameter.Length - 1, 1);
                                        parameter = parameter.Substring(1);
                                        // look for field name in Doc Metadata

                                        // Traverse Document Metatara
                                        foreach (GlobalVars.Metadata metadata in document.Fields)
                                        {
                                            // Check for Field Name
                                            if (metadata.FieldName == parameter)
                                            {
                                                // Field Name found so proceed with building the Directory Path                  
                                                directory = directory + metadata.FieldValue;
                                                break;
                                            }
                                        }
                                        break;
                                    default:
                                        // This ia a String ( just append the string to the directory path)
                                        parameter = item.Trim();
                                        parameter = parameter.Remove(parameter.Length - 1, 1);
                                        parameter = parameter.Substring(1);
                                        if (parameter == "\\") parameter = "DIRSEPARATOR";
                                        directory = directory + parameter;
                                        break;
                                }
                            }
                            // A this point .... apply Directory Replace rules to the directory string
                            foreach (GlobalVars.ReplaceToken token in exportTransactions.ExportRule.DirectoryReplaceRule)
                            {
                                pattern = token.Pattern.Trim();
                                pattern = pattern.Remove(pattern.Length - 1, 1);
                                pattern = pattern.Substring(1);
                                replaceBy = token.ReplaceBy.Trim();
                                replaceBy = replaceBy.Remove(replaceBy.Length - 1, 1);
                                replaceBy = replaceBy.Substring(1);
                                directory = directory.Replace(pattern, replaceBy);
                            }
                            directory = directory.Replace("DIRSEPARATOR", "\\");
                            // Update Document Target Directory

                            document.TargetFileLocation = baseOutputDirectory + directory;
                            logger.Info("       Target Directory: " + document.TargetFileLocation);
                        }
                    }                    
                }

                // 6- Build File Name based on File Names Rules
                if (continueProcessing)
                {
                    logger.Info("     Building File Name based on File Names Rules ... ");
                    foreach (GlobalVars.ExportBatches batch in exportTransactions.Batches)
                    {
                        foreach (GlobalVars.ExportDocs document in batch.Documents)
                        {
                            string fileName = "";
                            string fieldTypeIdentifier = "";
                            string parameter = "";
                            string pattern = "";
                            string replaceBy = "";
                            foreach (string item in exportTransactions.ExportRule.FileNameFormat)
                            {
                                fieldTypeIdentifier = item.Trim().Substring(0, 1);

                                switch (fieldTypeIdentifier)
                                {
                                    case "[":
                                        // This is a System Varibale Field
                                        parameter = item.Trim();
                                        parameter = parameter.Remove(parameter.Length - 1, 1);
                                        parameter = parameter.Substring(1);
                                        switch (parameter)
                                        {
                                            case "Customer Name":
                                                if (string.IsNullOrEmpty(exportTransactions.CustomerName))
                                                    fileName = fileName + "CUSTOMER-NAME";
                                                else
                                                    fileName = fileName + exportTransactions.CustomerName.Trim();
                                                break;
                                            case "Project Name":
                                                if (string.IsNullOrEmpty(exportTransactions.ProjectName))
                                                    fileName = fileName + "PROJECT-NAME";
                                                else
                                                    fileName = fileName + exportTransactions.ProjectName.Trim();
                                                break;
                                            case "Job Type":
                                                if (string.IsNullOrEmpty(exportTransactions.JobType))
                                                    fileName = fileName + "JOB-TYPE";
                                                else
                                                    fileName = fileName + exportTransactions.JobType.Trim();
                                                break;
                                            case "Work Order":
                                                if (string.IsNullOrEmpty(exportTransactions.WorkOrder))
                                                    fileName = fileName + "WORK-ORDER";
                                                else
                                                    fileName = fileName + exportTransactions.WorkOrder.Trim();
                                                break;
                                            case "Batch Name":
                                                if (string.IsNullOrEmpty(batch.BatchName))
                                                    fileName = fileName + "BATCH-NAME";
                                                else
                                                    fileName = fileName + batch.BatchName.Trim();
                                                break;
                                            default:
                                                break;
                                        }
                                        break;
                                    case "{":
                                        // This is a batch Variable Field
                                        parameter = item.Trim();
                                        parameter = parameter.Remove(parameter.Length - 1, 1);
                                        parameter = parameter.Substring(1);
                                        // look for field name in Doc Metadata

                                        // Traverse Document Metatara
                                        foreach (GlobalVars.Metadata metadata in document.Fields)
                                        {
                                            // Check for Field Name
                                            if (metadata.FieldName == parameter)
                                            {
                                                // Field Name found so proceed with building the File Name                  
                                                fileName = fileName + metadata.FieldValue;
                                                break;
                                            }
                                        }
                                        break;
                                    default:
                                        // This ia a String ( just append the string to the File Name)
                                        parameter = item.Trim();
                                        parameter = parameter.Remove(parameter.Length - 1, 1);
                                        parameter = parameter.Substring(1);
                                        fileName = fileName + parameter;
                                        break;
                                }
                            }
                            // In the scenarion where fileName is empty, then use the CP original file name.
                            if (string.IsNullOrEmpty(fileName))
                            {
                                //fileName = Path.GetFileName(document.FileLocation + "\\" + document.FileName);
                                fileName = Path.GetFileNameWithoutExtension(document.FileLocation + "\\" + document.FileName);
                            }
                            // A this point .... apply File Name Replace rules to the File Name string
                            foreach (GlobalVars.ReplaceToken token in exportTransactions.ExportRule.FileNameReplaceRule)
                            {
                                pattern = token.Pattern.Trim();
                                pattern = pattern.Remove(pattern.Length - 1, 1);
                                pattern = pattern.Substring(1);
                                replaceBy = token.ReplaceBy.Trim();
                                replaceBy = replaceBy.Remove(replaceBy.Length - 1, 1);
                                replaceBy = replaceBy.Substring(1);
                                fileName = fileName.Replace(pattern, replaceBy);
                            }
                            string fileExtension = Path.GetExtension(document.FileLocation + "\\" + document.FileName);
                            if (!filesNamesCheckList.Contains(document.TargetFileLocation.ToUpper() + "\\" + fileName.ToUpper()))
                            {
                                lock (lockObj)
                                {
                                    // The name is unique, then Add name to the Check list
                                    filesNamesCheckList.Add(document.TargetFileLocation.ToUpper() + "\\" + fileName.ToUpper());
                                    document.TargetFilename = fileName + fileExtension;
                                    //Console.ForegroundColor = ConsoleColor.Green;
                                    //Console.WriteLine("File name not Found : " + fileName);
                                }
                            }
                            else
                            {                                
                                lock (lockObj)
                                {
                                    //Apply Duplicate File Name Action before the file name is assigned
                                    //Console.ForegroundColor = ConsoleColor.Red;
                                    //Console.WriteLine("File name Found : " + fileName);

                                    // The File Name exist, then goes by the rules and append a sequencial number to it
                                    int sequence = 0;
                                    for (; ; )
                                    {
                                        sequence++;
                                        if (!filesNamesCheckList.Contains(document.TargetFileLocation.ToUpper() + "\\" + fileName.ToUpper() + "_" + sequence.ToString()))
                                        {
                                            lock (lockObj)
                                            {
                                                filesNamesCheckList.Add(document.TargetFileLocation.ToUpper() + "\\" + fileName.ToUpper() + "_" + sequence.ToString());
                                                document.TargetFilename = fileName + "_" + sequence.ToString() + fileExtension;
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("New File name Found : " + fileName + "_" + sequence.ToString());
                                            }
                                            break;
                                        }

                                    }

                                }
                            }
                        }
                    }                    
                }

                // 7. Build Metadata Output File (use outputFilesNamesCheckList)
                Console.ForegroundColor = ConsoleColor.White;
                if (continueProcessing)
                {
                    logger.Info("     Building Metadata Output File ... ");
                    exportTransactions.OutputFiles = new List<GlobalVars.MetadataFiles>();
                    foreach (GlobalVars.ExportBatches batch in exportTransactions.Batches)
                    {
                        logger.Info("       Batch: " + batch.BatchName);
                        // Build Metadata Output Directory and File location
                        string directory = "";
                        string fieldTypeIdentifier = "";
                        string parameter = "";
                        string pattern = "";
                        string replaceBy = "";
                        foreach (string item in exportTransactions.ExportRule.MetadataDirectoryFormat)
                        {
                            fieldTypeIdentifier = item.Trim().Substring(0, 1);

                            switch (fieldTypeIdentifier)
                            {
                                case "[":
                                    // This is a System Varibale Field
                                    parameter = item.Trim();
                                    parameter = parameter.Remove(parameter.Length - 1, 1);
                                    parameter = parameter.Substring(1);
                                    switch (parameter)
                                    {
                                        case "Customer Name":
                                            if (string.IsNullOrEmpty(exportTransactions.CustomerName))
                                                directory = directory + "CUSTOMER-NAME";
                                            else
                                                directory = directory + exportTransactions.CustomerName.Trim();
                                            break;
                                        case "Project Name":
                                            if (string.IsNullOrEmpty(exportTransactions.ProjectName))
                                                directory = directory + "PROJECT-NAME";
                                            else
                                                directory = directory + exportTransactions.ProjectName.Trim();
                                            break;
                                        case "Job Type":
                                            if (string.IsNullOrEmpty(exportTransactions.JobType))
                                                directory = directory + "JOB-TYPE";
                                            else
                                                directory = directory + exportTransactions.JobType.Trim();
                                            break;
                                        case "Work Order":
                                            if (string.IsNullOrEmpty(exportTransactions.WorkOrder))
                                                directory = directory + "WORK-ORDER";
                                            else
                                                directory = directory + exportTransactions.WorkOrder.Trim();
                                            break;
                                        case "Batch Name":
                                            if (string.IsNullOrEmpty(batch.BatchName))
                                                directory = directory + "BATCH-NAME";
                                            else
                                                directory = directory + batch.BatchName.Trim();
                                            break;
                                        default:
                                            break;
                                    }
                                    break;

                                default:
                                    // This ia a String ( just append the string to the directory path)
                                    parameter = item.Trim();
                                    parameter = parameter.Remove(parameter.Length - 1, 1);
                                    parameter = parameter.Substring(1);
                                    if (parameter == "\\") parameter = "DIRSEPARATOR";
                                    directory = directory + parameter;
                                    break;
                            }
                        }
                        // A this point .... apply Directory Replace rules to the directory string
                        foreach (GlobalVars.ReplaceToken token in exportTransactions.ExportRule.DirectoryReplaceRule)
                        {
                            pattern = token.Pattern.Trim();
                            pattern = pattern.Remove(pattern.Length - 1, 1);
                            pattern = pattern.Substring(1);
                            replaceBy = token.ReplaceBy.Trim();
                            replaceBy = replaceBy.Remove(replaceBy.Length - 1, 1);
                            replaceBy = replaceBy.Substring(1);
                            directory = directory.Replace(pattern, replaceBy);
                        }
                        directory = directory.Replace("DIRSEPARATOR", "\\");
                        //build Metadata Output File Name and extension
                        string OutputFileName = "";

                        if (exportTransactions.ExportRule.MetadataFileName.Trim() == "Batch Name")
                            OutputFileName = batch.BatchName;
                        else
                            OutputFileName = exportTransactions.ExportRule.MetadataFileName.Trim();

                        switch (exportTransactions.ExportRule.OutputFileFormat.Trim())
                        {
                            case "CSV":
                                OutputFileName = OutputFileName + ".csv";
                                break;

                            case "XML":
                                OutputFileName = OutputFileName + ".xml";
                                break;
                        }

                        // Checking if Metadata File already exist
                        if (!outputFilesNamesCheckList.Contains(directory.ToUpper() + "\\" + OutputFileName.ToUpper()))
                        {
                            logger.Info("       Creating new Metadata Ouput file ...");
                            // This is a new Metadata File so a new content wil lbe created
                            lock (lockObj)
                            {
                                GlobalVars.MetadataFiles metadataFile = new GlobalVars.MetadataFiles();
                                metadataFile.OutputFileLocation = baseOutputDirectory + directory;
                                metadataFile.OutputFileName = OutputFileName;

                                // The Output Directory is unique, then Add the new name to the outputFilesNamesCheckList list
                                outputFilesNamesCheckList.Add(directory.ToUpper() + "\\" + OutputFileName.ToUpper());
                                
                                switch (exportTransactions.ExportRule.OutputFileFormat.Trim())
                                {
                                    case "XML":
                                        metadataFile.Content = CreateNewMetadataXML(batch, exportTransactions.ExportRule.OutputFields);
                                        break;

                                    case "CSV":
                                        char fieldDelimeter = new char();
                                        switch (exportTransactions.ExportRule.OutputFileDelimeter.Trim())
                                        {
                                            case "Comma":
                                                fieldDelimeter = (char)44;
                                                break;
                                            case "Space":
                                                fieldDelimeter = (char)32;
                                                break;
                                            case "Tab":
                                                fieldDelimeter = (char)9;
                                                break;
                                            default:
                                                fieldDelimeter = (char)44;
                                                break;
                                        }
                                        metadataFile.Content = CreateNewMetadatacSV(batch, exportTransactions.ExportRule.OutputFields, exportTransactions.ExportRule.IncludeHeaderFlag, true, fieldDelimeter);
                                        break;
                                }
                                exportTransactions.OutputFiles.Add(metadataFile);
                                logger.Info("       Output File Location: " + metadataFile.OutputFileLocation);
                                logger.Info("       Output File name: " + metadataFile.OutputFileName);
                                logger.Trace("      Output File Content: " + metadataFile.Content);
                            }
                        }
                        else
                        {
                            logger.Info("       Updating Metadata Ouput file (appending Documen information to it) ...");
                            // The Metadata File already exist so Document Metadata must be addeed to the content of the exiting Metadata Outut File
                            foreach (GlobalVars.MetadataFiles metadataFile in exportTransactions.OutputFiles)
                            {
                                // Look for item with the same Output Full file name
                                if (baseOutputDirectory.ToUpper() + directory.ToUpper() + "\\" + OutputFileName.ToUpper() == metadataFile.OutputFileLocation.ToUpper() + "\\" + metadataFile.OutputFileName.ToUpper())
                                {
                                    // Item found, so proceeed to update the Output Metadata File
                                    switch (exportTransactions.ExportRule.OutputFileFormat.Trim())
                                    {
                                        case "XML":
                                            metadataFile.Content = UpdateMetadataXML(batch, exportTransactions.ExportRule.OutputFields, metadataFile.Content);
                                            break;

                                        case "CSV":
                                            char fieldDelimeter = new char();
                                            switch (exportTransactions.ExportRule.OutputFileDelimeter.Trim())
                                            {
                                                case "Comma":
                                                    fieldDelimeter = (char)44;
                                                    break;
                                                case "Space":
                                                    fieldDelimeter = (char)32;
                                                    break;
                                                case "Tab":
                                                    fieldDelimeter = (char)9;
                                                    break;
                                                default:
                                                    fieldDelimeter = (char)44;
                                                    break;
                                            }
                                            metadataFile.Content = UpdateMetadatacSV(batch, exportTransactions.ExportRule.OutputFields, true, fieldDelimeter, metadataFile.Content);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                if (continueProcessing)
                    result.Message = "GetExportTransactionsJob transaction completed successfully.";
                else
                {
                    result.Message = "GetExportTransactionsJob transaction end with errors. See API logs for more detail";
                    // We may want o to set the exportTransactions object to null if the object was not generated completely.
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
            result.RecordsCount = totalNumDocuments;
            logger.Debug(result.Message);
            result.ReturnValue = exportTransactions;
            logger.Trace("Leaving GetExportTransactionsJob Method ...");
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="fieldNames"></param>
        /// <param name="metadataFileContent"></param>
        /// <returns></returns>
        static public string UpdateMetadataXML(GlobalVars.ExportBatches batch, List<string> fieldNames, string metadataFileContent)
        {
            string metadataValue = "";
            Boolean includeField = true;
            Boolean fieldNameFound = false;

            try
            {
                logger.Trace("Entering into UpdateMetadataXML Method for Batch Name: " + batch.BatchName);

                XDocument xmldoc = XDocument.Parse(metadataFileContent);

                XElement root = new XElement("root");
                foreach (GlobalVars.ExportDocs doc in batch.Documents)
                {
                    XElement document = new XElement("document");

                    if (fieldNames != null)
                    {
                        foreach (string fieldName in fieldNames)
                        {
                            fieldNameFound = false;
                            foreach (GlobalVars.Metadata metadata in doc.Fields)
                            {
                                if (fieldName == metadata.FieldName.Trim())
                                {
                                    metadataValue = metadata.FieldValue;
                                    XElement field = new XElement("field",
                                                    new XAttribute("name", metadata.FieldName.Trim()),
                                                    new XAttribute("value", metadataValue.Trim())
                                                );
                                    document.Add(field);
                                    fieldNameFound = true;
                                    break;
                                }
                            }
                            if (!fieldNameFound)
                            {
                                includeField = true;
                                switch (fieldName.Trim())
                                {
                                    case "FileName":
                                        metadataValue = doc.TargetFilename;
                                        break;
                                    case "FileDirectory":
                                        metadataValue = doc.TargetFileLocation;
                                        break;
                                    case "FullFileName":
                                        metadataValue = doc.TargetFileLocation + "\\" + doc.TargetFilename;
                                        break;
                                    case "BatchName":
                                        metadataValue = batch.BatchName;
                                        break;
                                    default:
                                        includeField = false;
                                        break;
                                }
                                if (includeField)
                                {
                                    // We only add field names that were seelected for Output. These field names are contained in the fieldNames list
                                    XElement field = new XElement("field",
                                                    new XAttribute("name", fieldName.Trim()),
                                                    new XAttribute("value", metadataValue.Trim())
                                                );
                                    document.Add(field);
                                }
                            }
                        }
                    }
                    xmldoc.Element("root").Add(document);
                    root.Add(document);                    
                }

                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmldoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    metadataFileContent = stringWriter.GetStringBuilder().ToString();
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
            }
            logger.Trace("Leaving into UpdateMetadataXML Method");
            return metadataFileContent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        static public string CreateNewMetadataXML(GlobalVars.ExportBatches batch, List<string> fieldNames)
        {
            string metadataFileContent = "";
            string metadataValue = "";
            Boolean includeField = true;
            Boolean fieldNameFound = false;

            try
            {
                logger.Trace("Entering into CreateNewMetadataXML Method for Batch Name: " + batch.BatchName);

                XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                XElement root = new XElement("root");
                foreach (GlobalVars.ExportDocs doc in batch.Documents)
                {
                    XElement document = new XElement("document");
                   
                    if (fieldNames != null)
                    {
                        foreach (string fieldName in fieldNames)
                        {
                            fieldNameFound = false;
                            foreach (GlobalVars.Metadata metadata in doc.Fields)
                            {
                                if (fieldName == metadata.FieldName.Trim())
                                {
                                    metadataValue = metadata.FieldValue;
                                    XElement field = new XElement("field",
                                                    new XAttribute("name", metadata.FieldName.Trim()),
                                                    new XAttribute("value", metadataValue.Trim())
                                                );
                                    document.Add(field);
                                    fieldNameFound = true;
                                    break;
                                }
                            }
                            if (!fieldNameFound)
                            {
                                includeField = true;
                                switch (fieldName.Trim())
                                {
                                    case "FileName":
                                        metadataValue = doc.TargetFilename;
                                        break;
                                    case "FileDirectory":
                                        metadataValue = doc.TargetFileLocation;
                                        break;
                                    case "FullFileName":
                                        metadataValue = doc.TargetFileLocation + "\\" + doc.TargetFilename;
                                        break;
                                    case "BatchName":
                                        metadataValue = batch.BatchName;
                                        break;
                                    default:
                                        includeField = false;
                                        break;
                                }
                                if (includeField)
                                {
                                    // We only add field names that were seelected for Output. These field names are contained in the fieldNames list
                                    XElement field = new XElement("field",
                                                    new XAttribute("name", fieldName.Trim()),
                                                    new XAttribute("value", metadataValue.Trim())
                                                );
                                    document.Add(field);
                                }
                            }
                        }
                    }                        
                    root.Add(document);
                }
                XDocument xDocument = new XDocument(new XDeclaration("1.0", "ISO-8859-1", "yes"), root);

                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xDocument.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    metadataFileContent = stringWriter.GetStringBuilder().ToString();
                }                
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);                
            }
            logger.Trace("Leaving into CreateNewMetadataXML Method");
            return metadataFileContent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="fieldNames"></param>
        /// <param name="includeHeader"></param>
        /// <param name="useQuotationAround"></param>
        /// <param name="outputFileDelimeter"></param>
        /// <param name="metadataFileContent"></param>
        /// <returns></returns>
        static public string UpdateMetadatacSV(GlobalVars.ExportBatches batch, List<string> fieldNames, Boolean useQuotationAround, char outputFileDelimeter, string metadataFileContent)
        {
            string metadataValue = "";
            Boolean includeField = true;
            Boolean fieldNameFound = false;
            try
            {
                logger.Trace("Entering into CreateNewMetadatacSV Method for Batch Name: " + batch.BatchName);

                //byte[] byteArray = Encoding.UTF8.GetBytes(metadataFileContent);
               // MemoryStream stream = new MemoryStream(byteArray);
                var stream = new MemoryStream();
                using (var writer = new CsvFileWriter(stream))
                {
                    writer.Delimiter = outputFileDelimeter;

                    // Include Double Quotes
                    if (useQuotationAround)
                        writer.Quote = (char)34;

                    foreach (GlobalVars.ExportDocs doc in batch.Documents)
                    {
                        List<string> rowFields = new List<string>();
                        if (fieldNames != null)
                        {
                            foreach (string fieldName in fieldNames)
                            {
                                fieldNameFound = false;
                                foreach (GlobalVars.Metadata metadata in doc.Fields)
                                {
                                    if (fieldName == metadata.FieldName.Trim())
                                    {
                                        rowFields.Add(metadata.FieldValue.Trim());
                                        fieldNameFound = true;
                                        break;
                                    }
                                }
                                if (!fieldNameFound)
                                {
                                    includeField = true;
                                    switch (fieldName.Trim())
                                    {
                                        case "FileName":
                                            metadataValue = doc.TargetFilename;
                                            break;
                                        case "FileDirectory":
                                            metadataValue = doc.TargetFileLocation;
                                            break;
                                        case "FullFileName":
                                            metadataValue = doc.TargetFileLocation + "\\" + doc.TargetFilename;
                                            break;
                                        case "BatchName":
                                            metadataValue = batch.BatchName;
                                            break;
                                        default:
                                            includeField = false;
                                            break;
                                    }
                                    if (includeField)
                                    {
                                        rowFields.Add(metadataValue.Trim());
                                    }
                                }
                            }
                        }
                        writer.WriteRow(rowFields);
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(stream);
                    metadataFileContent = metadataFileContent + Environment.NewLine + reader.ReadToEnd();
                    metadataFileContent = metadataFileContent.TrimEnd(new char[] { '\r', '\n' });
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
            }
            logger.Trace("Leaving into CreateNewMetadatacSV Method");
            return metadataFileContent;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="fieldNames"></param>
        /// <param name="includeHeader"></param>
        /// <param name="useQuotationAround"></param>
        /// <param name="outputFileDelimeter"></param>
        /// <returns></returns>
        static public string CreateNewMetadatacSV(GlobalVars.ExportBatches batch, List<string> fieldNames, Boolean includeHeader, Boolean useQuotationAround, char outputFileDelimeter)
        {
            string metadataFileContent = "";
            string metadataValue = "";
            Boolean includeField = true;
            Boolean fieldNameFound = false;
            try
            {
                logger.Trace("Entering into CreateNewMetadatacSV Method for Batch Name: " + batch.BatchName);

                var stream = new MemoryStream();               
                using (var writer = new CsvFileWriter(stream))
                {
                    writer.Delimiter = outputFileDelimeter;

                    // Include Double Quotes
                    if (useQuotationAround)
                        writer.Quote = (char)34;

                    // Include the Header
                    if (includeHeader)
                        writer.WriteRow(fieldNames);

                    foreach (GlobalVars.ExportDocs doc in batch.Documents)
                    {
                        List<string> rowFields = new List<string>();
                        if (fieldNames != null)
                        {
                            foreach (string fieldName in fieldNames)
                            {
                                fieldNameFound = false;
                                foreach (GlobalVars.Metadata metadata in doc.Fields)
                                {
                                    if (fieldName == metadata.FieldName.Trim())
                                    {
                                        rowFields.Add(metadata.FieldValue.Trim());
                                        fieldNameFound = true;
                                        break;
                                    }
                                }
                                if (!fieldNameFound)
                                {
                                    includeField = true;
                                    switch (fieldName.Trim())
                                    {
                                        case "FileName":
                                            metadataValue = doc.TargetFilename;
                                            break;
                                        case "FileDirectory":
                                            metadataValue = doc.TargetFileLocation;
                                            break;
                                        case "FullFileName":
                                            metadataValue = doc.TargetFileLocation + "\\" + doc.TargetFilename;
                                            break;
                                        case "BatchName":
                                            metadataValue = batch.BatchName;
                                            break;
                                        default:
                                            includeField = false;
                                            break;
                                    }
                                    if (includeField)
                                    {
                                        rowFields.Add(metadataValue.Trim());
                                    }
                                }
                            }
                        }
                        writer.WriteRow(rowFields);
                    }
                    
                    stream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(stream);
                    metadataFileContent = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
            }
            logger.Trace("Leaving into CreateNewMetadatacSV Method");
            return metadataFileContent;
        }
    }

}
