using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Quartz.Logging;
using System.Collections.Specialized;
using Quartz.Impl;
using Quartz;
using static ScanningServicesDataObjects.GlobalVars;
using NLog;
using System.Threading;
using System.IO;
using System.Diagnostics;
using CronExpressionDescriptor;
using System.Net;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceProcess;
using System.Reflection;
using System.Xml;

namespace VFRDuplicateRemover
{
    static class Program
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        static object lockObj = new object();

        public static void CopyFolder(string sourceFolder, string destFolder, string filter, string logJobName)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                string[] files = Directory.GetFiles(sourceFolder, filter);
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string dest = Path.Combine(destFolder, name);
                    File.Copy(file, dest, true);
                }
                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Path.Combine(destFolder, name);
                    CopyFolder(folder, dest, filter, logJobName);
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            
        }

        public static void BatchIndexingProcess(string logJobName, JobExtended job,  Batch batch)
        {
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();
                    ResultFields resultFields = new ResultFields();
                    BatchDocs document = new BatchDocs();
                    string batchRestingLocation = "";
                    string scanOperator = "";
                    int pageCountInBatch = 0;
                    int numDocsInBatch = 0;
                    int imageCountInBatch = 0;
                    int docKeysStrokes = 0;
                    int fieldsCount = 0;
                    int totalKeysStrokes = 0;
                    DateTime createdDateTime = new DateTime();                    
                    double batchSize = 0;
                    string batchLocation = "";

                    if (string.IsNullOrEmpty(batch.BatchAlias)) batch.BatchAlias = "";

                    // We need to check if resting location uses Batch Number of Alias Name
                    nlogger.Info("      Processing Batch : " + batch.BatchNumber.Trim() + " ; Alias: " + batch.BatchAlias.Trim());
                    Console.Out.WriteLineAsync("      Processing Batch : " + batch.BatchNumber.Trim() + " ; Alias: " + batch.BatchAlias.Trim());
                    if (string.IsNullOrEmpty(batch.DocumentPath.Trim()))
                    {
                        nlogger.Info("          Could not find Documents Path Location for this Batch.");
                        Console.Out.WriteLineAsync("          Could not find Documents Path Location for this Batch.");
                        return;
                    }

                    if (Directory.Exists(batch.DocumentPath.Trim()))
                    {
                        // Means that we are not using Alias in folder name
                        batchRestingLocation = batch.DocumentPath.Trim();
                    }
                    else
                    {
                        if (batch.BatchAlias != "" && Directory.Exists(batch.DocumentPath.Trim() ))
                        {
                            // Means that we are using Alias in folder name
                            batchRestingLocation = batch.DocumentPath.Trim() ;
                        }
                        else
                        {
                            nlogger.Info("          Directory not found: " + batchRestingLocation);
                            Console.Out.WriteLineAsync("          Directory not found: " + batchRestingLocation);
                            return;
                        }
                    }
                    nlogger.Info("          Directory: " + batchRestingLocation);                   
                    Console.Out.WriteLineAsync("          Directory: " + batchRestingLocation);

                    // Check if Document.xml file exit for current Batch
                    if (!File.Exists(batchRestingLocation + "\\Documents.xml"))
                    {
                        nlogger.Info("          Could not find Documents.xml metadata file.");
                        Console.Out.WriteLineAsync("          Could not find Documents.xml metadata file.");
                        return;
                    }
                    else
                    {
                        // We need to check if this Batch has been indexed before
                        nlogger.Info("          Checking if this batch was indexed before...");
                        Console.Out.WriteLineAsync("          Checking if this batch is already indexed ...");
                        ResultBatchDocs resultDocs = new ResultBatchDocs();
                        resultDocs = DBTransactions.GetDocumentInformation(logJobName,batch.BatchNumber);
                        if (resultDocs.ReturnCode == 0)
                        {
                            if (resultDocs.RecordsCount > 0)
                            {
                                nlogger.Info("          Batch is indexed already.");
                                Console.Out.WriteLineAsync("          Batch is indexed already.");
                                string path1 = Path.GetDirectoryName(resultDocs.ReturnValue[0].BatchLocation);
                                if (!path1.EndsWith("\\"))
                                {
                                    path1 += "\\";
                                }
                                string path2 = Path.GetFullPath(batchRestingLocation);
                                if (!path2.EndsWith("\\"))
                                {
                                    path2 += "\\";
                                }                                
                                if (path1 != path2)
                                {
                                    nlogger.Info("          Remove associated files in: " + batchRestingLocation);
                                    Console.Out.WriteLineAsync("          Remove associated files in: " + batchRestingLocation);
                                    // Remove files from resting location
                                    if (Directory.Exists(resultDocs.ReturnValue[0].BatchLocation.Trim()))
                                    {
                                        Directory.Delete(resultDocs.ReturnValue[0].BatchLocation.Trim(), true);
                                    }
                                }
                                nlogger.Info("          Remove associated records from the database.");
                                Console.Out.WriteLineAsync("          Remove associated records from the database.");
                                // Remove Batch Documents from the Database
                                DBTransactions.DeleteBatchDocuments(logJobName,batch.BatchNumber);
                            }
                            else
                            {
                                nlogger.Info("          Batch has never been indexed.");
                                Console.Out.WriteLineAsync("          Batch has never been indexed.");
                            }
                        }

                        // Get Job Fields
                        resultFields = DBTransactions.GetFieldsByJobID(logJobName,job.JobID);

                        // Get information from Metadata file
                        XmlDocument finalXml = new XmlDocument();
                        finalXml.Load(batchRestingLocation + "\\Documents.xml");
                        XmlNodeList finalDocuments = finalXml.SelectNodes("//document");
                        numDocsInBatch = 0;
                        pageCountInBatch = 0;
                        batchSize = 0;
                        document.Customer = batch.Customer;

                        foreach (XmlElement doc in finalDocuments)
                        {
                            List<Metadata> docMetadata = new List<Metadata>();
                            batchLocation = "";
                            fieldsCount = 0;
                            docKeysStrokes = 0;
                            numDocsInBatch = numDocsInBatch + 1;

                            foreach (XmlNode node in doc)
                            {
                                switch (node.Attributes["name"].Value)
                                {
                                    case "Batch Location":
                                        document.BatchLocation = node.Attributes["value"].Value;
                                        break;

                                    case "Document location":
                                        document.DocumentLocation = node.Attributes["value"].Value;
                                        break;

                                    case "Document ID":
                                        // document.DocumentID =  node.Attributes["value"].Value;
                                        // I am using Document Sequence number as Document ID 
                                        // For some unknown reason I was getting a Document ID out of the sequence
                                        break;

                                    case "Image count in document":
                                        document.ImageCountInDocument = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Page count in document":
                                        document.PageCountInDocument = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Batch name":
                                        // We are using the Batch name from batch.BatchNumber
                                        //document.BatchName = node.Attributes["value"].Value;
                                        break;

                                    case "Batch size (bytes)":
                                        batchSize = Convert.ToDouble(node.Attributes["value"].Value);
                                        document.BatchSize = Convert.ToDouble(node.Attributes["value"].Value);
                                        break;

                                    case "Created date & time":
                                        createdDateTime = Convert.ToDateTime(node.Attributes["value"].Value);
                                        document.CreateDateAndTime = Convert.ToDateTime(node.Attributes["value"].Value);
                                        break;

                                    case "Created station ID":
                                        document.CreateStationID = node.Attributes["value"].Value;
                                        break;

                                    case "Created station Name":
                                        document.CreateStationName = node.Attributes["value"].Value;
                                        break;

                                    case "Created user ID":
                                        scanOperator = node.Attributes["value"].Value;
                                        document.CreateUserID = node.Attributes["value"].Value;
                                        break;

                                    case "Output date & time":
                                        document.OutputDateAndTime = Convert.ToDateTime(node.Attributes["value"].Value);
                                        break;

                                    case "Output station ID":
                                        document.OutoutStationID = node.Attributes["value"].Value;
                                        break;

                                    case "Output station Name":
                                        document.OutputStationName = node.Attributes["value"].Value;
                                        break;

                                    case "Output user ID":
                                        document.OutputUserID = node.Attributes["value"].Value;
                                        break;

                                    case "Last modified date & time":
                                        document.LastModifiedDateAndTime = Convert.ToDateTime(node.Attributes["value"].Value);
                                        break;

                                    case "Last modified station ID":
                                        document.LastModifiedStationID = node.Attributes["value"].Value;
                                        break;

                                    case "Last modified station Name":
                                        document.LastModifiedStationName = node.Attributes["value"].Value;
                                        break;

                                    case "Last modified user ID":
                                        document.LastModifiedUserID = node.Attributes["value"].Value;

                                        break;
                                    case "Starting document ID":
                                        document.StartingDocumentID = node.Attributes["value"].Value;
                                        break;

                                    case "First document ID":
                                        document.FirstDocumentID = node.Attributes["value"].Value;
                                        break;

                                    case "Last document ID":
                                        document.LastDocumentID = node.Attributes["value"].Value;
                                        break;

                                    case "Document count in batch":
                                        //numDocsInBatch = Convert.ToInt32(node.Attributes["value"].Value);
                                        document.DocumentCountInBatch = Convert.ToInt32(node.Attributes["value"].Value);
                                        
                                        break;
                                    case "Page count in batch":
                                        document.PageCountInBatch = Convert.ToInt32(node.Attributes["value"].Value);
                                        pageCountInBatch = document.PageCountInBatch;
                                        break;

                                    case "Image count in batch":
                                        imageCountInBatch = Convert.ToInt32(node.Attributes["value"].Value);
                                        document.ImageCountInBatch = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Black and white image count":
                                        document.BlackAndWhiteImageCount = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Color image count":
                                        document.ColorImageCount = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Grayscale image count":
                                        document.GrayscaleImageCount = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Images captured - front":
                                        document.ImageCaptureFront = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Images rescanned - front":
                                        document.ImagesRescannedFront = Convert.ToInt32(node.Attributes["value"].Value);

                                        break;
                                    case "Images removed for blank - front":
                                        document.ImagesRemovedForBlankFront = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Image deleted - front":
                                        document.ImagesDeletedFront = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Images captured - back":
                                        document.ImagesCaptureBack = Convert.ToInt32(node.Attributes["value"].Value);

                                        break;
                                    case "Images rescanned - back":
                                        document.ImagesRescannedBack = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Images removed for blank - back":
                                        document.ImagesRemovedForBlankBack = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Image deleted - back":
                                        document.ImagesDeletedBack = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Document Sequence Number":
                                        document.DocumentSequenceNumber = node.Attributes["value"].Value;
                                        // I am taking the Document Sequence Number as the Document ID 
                                        document.DocumentID = Convert.ToInt32(node.Attributes["value"].Value);
                                        break;

                                    case "Document Filename with Full Path":
                                        document.DocumentFileNameWithFullPath = node.Attributes["value"].Value;
                                        break;

                                    case "Document Filename":
                                        document.DocumentFileName = node.Attributes["value"].Value;
                                        //Need to make a correction to the Batch Delivery Process
                                        //document.DocumentFilenameFullPath = document.DocumentLocation + "\" + document.DocumentFilename
                                        break;

                                    case "Document size (bytes)":
                                        document.DocumentSize = node.Attributes["value"].Value;
                                        break;

                                    default:
                                        // Ignore
                                        break;
                                }

                                switch (node.Attributes["level"].Value)
                                {
                                    case "document":
                                        fieldsCount = fieldsCount + 1;
                                        BuildCustomerField(fieldsCount, node.Attributes["name"].Value + "|D|" + node.Attributes["value"].Value, document);
                                        docMetadata.Add(BuildMetadata(node.Attributes["name"].Value + "|D|" + node.Attributes["value"].Value));
                                        // Check if KeysStroke for this field count
                                        foreach (Field field in resultFields.ReturnValue)
                                        {
                                            if (field.CPFieldName.Trim() == node.Attributes["name"].Value && !field.ExcludeFromKeystrokesCount)
                                            {
                                                docKeysStrokes = docKeysStrokes + node.Attributes["value"].Value.Length;
                                            }
                                        }
                                        break;

                                    case "batch":
                                        fieldsCount = fieldsCount + 1;
                                        BuildCustomerField(fieldsCount, node.Attributes["name"].Value + "|B|" + node.Attributes["value"].Value, document);
                                        docMetadata.Add(BuildMetadata(node.Attributes["name"].Value + "|B|" + node.Attributes["value"].Value));
                                        // Check if KeysStroke for this field count
                                        foreach (Field field in resultFields.ReturnValue)
                                        {
                                            if (field.CPFieldName.Trim() == node.Attributes["name"].Value && !field.ExcludeFromKeystrokesCount)
                                            {
                                                docKeysStrokes = docKeysStrokes + node.Attributes["value"].Value.Length;
                                            }
                                        }
                                        break;

                                    default:
                                        // Ignore
                                        break;
                                }
                            }
                            // Metadata to document data structure
                            document.DocMetadata = docMetadata;
                            // Update Doc KeysStrokes
                            document.keystrokes = docKeysStrokes;
                            totalKeysStrokes = totalKeysStrokes + docKeysStrokes;
                            document.BatchName = batch.BatchNumber;

                            // Add Document to Batch Doc Table
                            nlogger.Info("              Adding Document: " +  document.DocumentSequenceNumber);
                            Console.Out.WriteLineAsync("            Adding Document: " + document.DocumentSequenceNumber);
                            result = DBTransactions.NewBatchDocument(logJobName,batch.BatchNumber,document);
                            if (result.ReturnCode != 0)
                            {
                                nlogger.Info("          " + result.Message);
                                nlogger.Info("           Exception: " + result.Exception);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Out.WriteLineAsync("          " + result.Message);
                                Console.Out.WriteLineAsync("           Exception: " + result.Exception);
                                Console.ForegroundColor = ConsoleColor.White;
                                return;
                            }
                        }

                        // Update Batch Information
                        nlogger.Info("          Updating Batch Information ...");
                        Console.Out.WriteLineAsync("          Updating Batch Information ...");
                        // Status
                        batch.StatusFlag = "Waiting for Approval";
                        // Number of Documents
                        batch.NumberOfDocuments = numDocsInBatch;
                        // Update number of Pages
                        batch.NumberOfPages = pageCountInBatch;
                        // Update Number of Images
                        batch.NumberOfScannedPages = imageCountInBatch;
                        // Update Scanned Date
                        batch.ScannedDate = createdDateTime;
                        // Update Scanner Operator
                        batch.ScanOperator = scanOperator;
                        // Update Batch Size
                        batch.BatchSize = batchSize;
                        // Update KeysStorkes
                        batch.KeysStrokes = totalKeysStrokes;

                        result = DBTransactions.BatchUpdate(logJobName, batch);
                        if (result.ReturnCode != 0)
                        {
                            nlogger.Info("          " + result.Message);
                            nlogger.Info("          " + result.Exception);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Out.WriteLineAsync("          " + result.Message);
                            Console.Out.WriteLineAsync("          " + result.Exception);
                            Console.ForegroundColor = ConsoleColor.White;
                            return;
                        }

                        // register Transaction in Database Transaction Table
                        nlogger.Info("          Adding Batch Transaction Event to the Database.");
                        Console.Out.WriteLineAsync("          Adding Batch Transaction Event to the Database.");
                        result = DBTransactions.BatchTrackingEvent(logJobName, batch.BatchNumber, batch.StatusFlag);
                        if (result.ReturnCode != 0)
                        {
                            nlogger.Info("          " + result.Message);
                            nlogger.Info("          " + result.Exception);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Out.WriteLineAsync("          " + result.Message);
                            Console.Out.WriteLineAsync("          " + result.Exception);
                            Console.ForegroundColor = ConsoleColor.White;
                            return;
                        }
                    }
                    nlogger.Info("      Batch " + batch.BatchNumber + " was indexed successfully.");
                    Console.Out.WriteLineAsync("      Batch " + batch.BatchNumber + " was indexed successfully.");
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInformation"></param>
        /// <returns></returns>
        static public Metadata BuildMetadata(string fieldInformation)
        {
            var tokens = fieldInformation.Split('|');
            Metadata metadata = new Metadata();
            metadata.FieldName = tokens[0];
            metadata.FieldValue = tokens[2];
            if (tokens[1] == "D")
                metadata.Scope = MetadataScope.Document;
            else
                metadata.Scope = MetadataScope.Batch;
            return metadata;
        }

        /// <summary>
        /// Assign Field Information to Document Data Structure
        /// </summary>
        /// <param name="fieldCounter"></param>
        /// <param name="fieldInformation"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        static public BatchDocs BuildCustomerField(int fieldCounter, string fieldInformation, BatchDocs document)
        {

            switch (fieldCounter)
            {
                case 1:
                    document.CustonmerField1 = fieldInformation;
                    break;
                case 2:
                    document.CustonmerField2 = fieldInformation;
                    break;
                case 3:
                    document.CustonmerField3 = fieldInformation;
                    break;
                case 4:
                    document.CustonmerField4 = fieldInformation;
                    break;
                case 5:
                    document.CustonmerField5 = fieldInformation;
                    break;
                case 6:
                    document.CustonmerField6 = fieldInformation;
                    break;
                case 7:
                    document.CustonmerField7 = fieldInformation;
                    break;
                case 8:
                    document.CustonmerField8 = fieldInformation;
                    break;
                case 9:
                    document.CustonmerField9 = fieldInformation;
                    break;
                case 10:
                    document.CustonmerField10 = fieldInformation;
                    break;
                case 11:
                    document.CustonmerField11 = fieldInformation;
                    break;
                case 12:
                    document.CustonmerField12 = fieldInformation;
                    break;
                case 13:
                    document.CustonmerField13 = fieldInformation;
                    break;
                case 14:
                    document.CustonmerField14 = fieldInformation;
                    break;
                case 15:
                    document.CustonmerField15 = fieldInformation;
                    break;
            }
            return document;
        }

        public static void AnalizeJob(string logJobName, JobExtended job)
        {
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();
                    string jobBatchRestingDirectory = "";

                    jobBatchRestingDirectory = job.RestingLocation;
                    nlogger.Info("  Processing Job:  " + job.JobName.Trim());
                    Console.Out.WriteLineAsync("    Processing Job:  " + job.JobName.Trim());

                    // Get Batches in "Waiting for VFRDuplicateRemover"
                    nlogger.Info("      Getting Batches in Waiting for VFRDuplicateRemover ...");
                    Console.Out.WriteLineAsync("      Getting Batches in Waiting for VFRDuplicateRemover ...");
                    ResultBatches resulBatches = new ResultBatches();
                    resulBatches = DBTransactions.GetBatches(logJobName, job.JobName.Trim(),"Waiting for VFRDuplicateRemover");

                    if (resulBatches.RecordsCount == 0)
                    {
                        nlogger.Info("      Not Batches were found.");
                        Console.Out.WriteLineAsync("      Not Batches were found.");
                        return;
                    }
                    else
                    {
                        // Check if Resting Location Exist
                        if (!Directory.Exists(jobBatchRestingDirectory))
                        {
                            nlogger.Info("      Directory does not exist: " + jobBatchRestingDirectory);
                            nlogger.Info("      Skipping Job Type: " + job.JobName.Trim());
                            Console.Out.WriteLineAsync("        Directory does not exist: " + jobBatchRestingDirectory);
                            Console.Out.WriteLineAsync("        Skipping Job Type: " + job.JobName.Trim());
                            return;
                        }
                        else
                        {
                            foreach (Batch batch in resulBatches.ReturnValue)
                            {
                                BatchIndexingProcess(logJobName, job,  batch);
                            }
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public static void VFRDuplicateRemoverProcess(string logJobName, int jobID)
        {
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultJobsExtended resultjobs = new ResultJobsExtended();
                    List<ScanningServicesDataObjects.GlobalVars.Process> localCronJobsList = new List<ScanningServicesDataObjects.GlobalVars.Process>();
                    Boolean cronJobFound;
                    Boolean cronJobEnable;
                    // Check if Job ID correspond to "ALL". When Job ID = 0, we are refering to "ALL" Jobs
                    if (jobID == 0)
                    {
                        // Cron for All Job has been identified
                        // 1. Get Jobs information
                        resultjobs = DBTransactions. GetJobs();
                        // 2. For each Job, check if exist in cronJobsList (use a local copy of the cronJobsList)
                        localCronJobsList = VFRDuplicateRemoverService.cronJobsList;
                        foreach (JobExtended job in resultjobs.ReturnValue)
                        {
                            cronJobFound = false;
                            cronJobEnable = false;
                            foreach (ScanningServicesDataObjects.GlobalVars.Process cronProcess in localCronJobsList)
                            {
                                if (job.JobID == cronProcess.JobID)
                                {
                                    cronJobFound = true;
                                    cronJobEnable = cronProcess.EnableFlag;
                                    break;
                                }
                            }
                            if (cronJobFound && !cronJobEnable)
                            {
                                // The Cron Job was found but is disable so it will go by the ALL rules      
                                AnalizeJob(logJobName, job);
                            }
                            else
                            {
                                if (!cronJobFound)
                                {
                                    // The Cron Job was not found so it will go by the ALL rules 
                                    AnalizeJob(logJobName, job);
                                }
                            }
                        }
                    }
                    else
                    {
                        // 1. Get Job information
                        resultjobs = DBTransactions.GetJobByID(jobID);
                        JobExtended job = new JobExtended();
                        job = resultjobs.ReturnValue[0];
                        // Proces Job
                        AnalizeJob(logJobName, job);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            //return;    
        }

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject != null)
            {
                // log exception:
            }
        }
                
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


#if DEBUG
            VFRDuplicateRemoverService service1 = new VFRDuplicateRemoverService(args);
            service1.Startup(args);
            do
            {
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);

#else
            ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new  VFRDuplicateRemoverService(args)
                        };
                        ServiceBase.Run(ServicesToRun);
#endif


        }

        /// <summary>
        /// This method keeps the latest Crob Jobs in the cronJobsList 
        /// </summary>
        /// <returns></returns>
        public static async Task RunMultipleJobs()
        {            
            try
            {                
                Boolean cronJobFound;
                Boolean cronJobChanged;

                ResultProcesses resultProcess = new ResultProcesses();
                resultProcess = DBTransactions.GetProcessByName("Indexer");
                if (resultProcess.ReturnCode == 0)
                {
                    // Check list of processes
                    foreach (ScanningServicesDataObjects.GlobalVars.Process process in resultProcess.ReturnValue)
                    {
                        // When Job ID is = 0 means that rule applies for all Jobs
                        if (process.JobID == 0)
                            process.JobName = "ALL";

                        // Process only recordsa that match Host Name
                        if (VFRDuplicateRemoverService.hostName == process.StationName.Trim())
                        {
                            // 1. Check if process exist in the cronJob List anb if the cron exist, verify is the schedule has changed
                            //    This loop check if processes in the Database are already registerd in the Process List
                            cronJobFound = false;
                            cronJobChanged = false;
                            foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in VFRDuplicateRemoverService.cronJobsList)
                            {
                                if (cronJob.JobID == process.JobID && cronJob.ProcessID == process.ProcessID)
                                {
                                    cronJobFound = true;
                                    if (cronJob.ScheduleCronFormat.Trim() != process.ScheduleCronFormat.Trim())
                                    {
                                        cronJobChanged = true;
                                    }
                                    //cronJobEnable = process.EnableFlag;
                                    break;
                                }
                            }

                            // 2. Cron Jobs that were not found in the list and has the Enable Flag set to 'true', must be created
                            if (!cronJobFound && process.EnableFlag)
                            {
                                // Create new Cron Job
                                if (process.EnableFlag)
                                {
                                    lock (lockObj)
                                    {
                                        LogManager.Configuration.Variables["JobName"] = "General";
                                        nlogger.Info("Creating a new Cron Job...");
                                        nlogger.Info("     Job Name: " + process.JobName.Trim());
                                        nlogger.Info("     Job ID: " + process.JobID.ToString());
                                        nlogger.Info("     Process ID: " + process.ProcessID.ToString());
                                        nlogger.Info("     Schedule: " + process.ScheduleCronFormat);

                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("Creating a new Cron Job...");
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("     Job Name: " + process.JobName.Trim());
                                        Console.WriteLine("     Job ID: " + process.JobID.ToString());
                                        Console.WriteLine("     Process ID: " + process.ProcessID.ToString());
                                        Console.WriteLine("     Schedule: " + process.ScheduleCronFormat);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }

                                    // Only Register Valid Cron Jobs
                                    if (Quartz.CronExpression.IsValidExpression(process.ScheduleCronFormat))
                                    {
                                        var result = ExpressionDescriptor.GetDescription(process.ScheduleCronFormat, new Options());
                                        //{
                                        //    ThrowExceptionOnParseError = false
                                        //});

                                        ScanningServicesDataObjects.GlobalVars.Process jobProcess = new ScanningServicesDataObjects.GlobalVars.Process();
                                        jobProcess.JobID = process.JobID;
                                        jobProcess.ProcessID = process.ProcessID;
                                        jobProcess.ScheduleCronFormat = process.ScheduleCronFormat;
                                        jobProcess.EnableFlag = process.EnableFlag;
                                        VFRDuplicateRemoverService.cronJobsList.Add(jobProcess);

                                        RunJob("JOB-" + process.JobID.ToString(), "PROCESS-" + process.JobID.ToString() + "-" + process.ProcessID.ToString(), process.ScheduleCronFormat, process.JobName.Trim()).GetAwaiter().GetResult();

                                        lock (lockObj)
                                        {
                                            LogManager.Configuration.Variables["JobName"] = "General";
                                            nlogger.Info("     Rule: " + result);
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("     Rule: " + result);
                                            nlogger.Info("Cron Job was created successfully.");
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("Cron Job was created successfully.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                    else
                                    {
                                        var result = ExpressionDescriptor.GetDescription(process.ScheduleCronFormat, new Options()
                                        {
                                            ThrowExceptionOnParseError = false
                                        });
                                        lock (lockObj)
                                        {
                                            LogManager.Configuration.Variables["JobName"] = "General";
                                            nlogger.Info("Cron Job Format Error...");
                                            nlogger.Info("Message: " + result);

                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Cron Job Format Error...");
                                            Console.WriteLine("Message: " + result);
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cronJobFound)
                                {
                                    // 3. The Cron was found in the List but the Schedule has changed or the Cron has been disabled
                                    //    If the Cron has been disbaled, we must remove the Cron Job
                                    //    If the Cron was changed, we must remove and recreate the Cron Job (the cron willbe recreated in the next cycle of the services)
                                    //    In both cases, we must remove the Cron Job                                            
                                    if (!process.EnableFlag)
                                    {
                                        // Cron Job was Disable
                                        lock (lockObj)
                                        {
                                            LogManager.Configuration.Variables["JobName"] = "General";
                                            nlogger.Info("Cron Job was disabled.");
                                            nlogger.Info("     Job Name: " + process.JobName.Trim());
                                            nlogger.Info("     Job ID: " + process.JobID.ToString());
                                            nlogger.Info("     Process ID: " + process.ProcessID.ToString());

                                            Console.ForegroundColor = ConsoleColor.Magenta;
                                            Console.WriteLine("Cron Job was disabled.");
                                            Console.WriteLine("     Job Name: " + process.JobName.Trim());
                                            Console.WriteLine("     Job ID: " + process.JobID.ToString());
                                            Console.WriteLine("     Process ID: " + process.ProcessID.ToString());
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }

                                    if (cronJobChanged && process.EnableFlag)
                                    {
                                        // Cron Job changed and still enable
                                        lock (lockObj)
                                        {
                                            LogManager.Configuration.Variables["JobName"] = "General";
                                            nlogger.Info("Cron Job was changed.");
                                            nlogger.Info("     Job Name: " + process.JobName.Trim());
                                            nlogger.Info("     Job ID: " + process.JobID.ToString());
                                            nlogger.Info("     Process ID: " + process.ProcessID.ToString());
                                            nlogger.Info("     New Schedule String: " + process.ScheduleCronFormat);

                                            Console.ForegroundColor = ConsoleColor.Magenta;
                                            Console.WriteLine("Cron Job changed.");
                                            Console.WriteLine("     Job Name: " + process.JobName.Trim());
                                            Console.WriteLine("     Job ID: " + process.JobID.ToString());
                                            Console.WriteLine("     Process ID: " + process.ProcessID.ToString());
                                            Console.WriteLine("     New Schedule String: " + process.ScheduleCronFormat);
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }

                                    // 4. Check if the cronJob have to be removed from the cronJobsList
                                    if ((!process.EnableFlag) || (cronJobChanged && process.EnableFlag))
                                    {
                                        // Now, the job needs to be removed it from the Cron Jobs List
                                        // When a Cron Job is removed, the system will let the last running istance to finhish it works
                                        NameValueCollection props = new NameValueCollection
                                    {
                                        { "quartz.serializer.type", "binary" }
                                    };
                                        StdSchedulerFactory factory = new StdSchedulerFactory(props);
                                        IScheduler scheduler = await factory.GetScheduler();
                                        await scheduler.UnscheduleJob(new TriggerKey("PROCESS-" + process.JobID.ToString() + "-" + process.ProcessID.ToString(), "group1"));
                                        await scheduler.DeleteJob(new JobKey("JOB-" + process.JobID.ToString(), "group1"));
                                        // Keeping the Cron Job List uptodate 
                                        // Remove Cron Job from the List
                                        foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in VFRDuplicateRemoverService.cronJobsList)
                                        {
                                            if (cronJob.JobID == process.JobID && cronJob.ProcessID == process.ProcessID)
                                            {
                                                lock (lockObj)
                                                {
                                                    LogManager.Configuration.Variables["JobName"] = "General";
                                                    nlogger.Info("Cron Job was deleted form the List");
                                                    nlogger.Info("     Job Name: " + process.JobName.Trim());
                                                    nlogger.Info("     Job ID: " + process.JobID.ToString());
                                                    nlogger.Info("     Process ID: " + process.ProcessID.ToString());

                                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                                    VFRDuplicateRemoverService.cronJobsList.Remove(cronJob);
                                                    Console.WriteLine("Cron Job was deleted form the List.");
                                                    Console.WriteLine("     Job Name: " + process.JobName.Trim());
                                                    Console.WriteLine("     Job ID: " + process.JobID.ToString());
                                                    Console.WriteLine("     Process ID: " + process.ProcessID.ToString());
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Do nothing
                                    // This is the case where the Cron Job is in the Database but disabled
                                }
                            }
                        }
                    }
                    //await Task.Delay(TimeSpan.FromSeconds(300));
                }
                // Some Code that may be helpfull when shuting down the Service ....
                //await Task.Delay(TimeSpan.FromSeconds(300));
                //await scheduler.Shutdown();
            }
            //catch (ProtocolException ex)
            catch (SchedulerException ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        public static async Task StopScheduler()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(300));
                //await scheduler.Shutdown();
            }
            catch (SchedulerException ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }           
        }

        private static async Task RunJob(string jobName, string triggerName, string schedule, string jobDescription)
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                // and start it off
                await scheduler.Start();

                // define the job and tie it to our CronJob class
                IJobDetail job = JobBuilder.Create<CronJob>()
                    .WithIdentity(jobName, "group1")
                    .WithDescription(jobDescription)
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                if (CronExpression.IsValidExpression(schedule))
                {
                    ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerName, "group1")
                    .WithCronSchedule(schedule)
                    .StartAt(DateTime.UtcNow)
                    .WithPriority(1)
                    .Build();

                    // Tell quartz to schedule the job using our trigger
                    await scheduler.ScheduleJob(job, trigger);

                    // some sleep to show what's happening
                    //await Task.Delay(TimeSpan.FromSeconds(300));

                    // and last shut down the scheduler when you are ready to close your program
                    //await scheduler.Shutdown();
                }
            }
            catch (SchedulerException ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        // simple log provider to get something to the console
        private class ConsoleLogProvider : ILogProvider
        {
            public Quartz.Logging.Logger GetLogger(string name)
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                return (level, func, exception, parameters) =>
                {
                    if (level >= Quartz.Logging.LogLevel.Info && func != null)
                    {
                        nlogger.Trace("     [" + level + "] " + func(), parameters);
                        //Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }
        }
    }

    // The Attributes descrived below, prevent a Job with the same key to run simultaneously
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class CronJob : IJob
    {
        static object lockObj = new object();
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public async Task Execute(IJobExecutionContext context)
        {
            string logJobName = context.JobDetail.Description;
            DateTime time = DateTime.Now;
            string format = "MMM ddd d HH:mm yyyy";   // Use this format.
            try
            {
                lock (lockObj)
                {                    
                    LogManager.Configuration.Variables["JobName"] = logJobName; // context.JobDetail.Key.Name;

                    nlogger.Info("Starting Cron Job : " + context.JobDetail.Key.Name + " - " + context.JobDetail.Description + " @ " + time);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Out.WriteLineAsync("Starting Cron Job : " + context.JobDetail.Key.Name + " - " + context.JobDetail.Description + " @ " + time);        
                    Console.ForegroundColor = ConsoleColor.White;

                    VFRDuplicateRemover.Program.VFRDuplicateRemoverProcess(logJobName, Convert.ToInt32(context.JobDetail.Key.Name.Replace("JOB-", "")));

                    nlogger.Info("Completed Cron Job : " + context.JobDetail.Key.Name + " - " + context.JobDetail.Description + " @ " + time);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Out.WriteLineAsync("Completed Cron Job : " + context.JobDetail.Key.Name + " - " + context.JobDetail.Description + " @ " + time);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (IOException ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName; 
                    // context.JobDetail.Key.Name;
                    // Extract some information from this exception, and then   
                    // throw it to the parent method.  
                    if (ex.Source != null)
                    {
                        nlogger.Fatal("IOException source: {0}", ex.Source);
                        Console.WriteLine("IOException source: {0}", ex.Source);
                    }
                }                   
                throw;
            }
            
            //switch (context.JobDetail.Key.Name)
            //{
            //    case "job1":
            //        await Console.Out.WriteLineAsync("Greetings from HelloJob! --> " + time);
            //        break;
            //    case "job2":
            //        await Console.Out.WriteLineAsync("Greetings from COMPUDATA! --> " + time);
            //        break;
            //    default:
            //        //Console.WriteLine("Default case");
            //        break;
            //}
        }
    }
}
