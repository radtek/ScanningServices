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
using BitMiracle.LibTiff.Classic;


namespace Synchronizer
{
    static class Program
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        static object lockObj = new object();

        //private static bool IsSymbolic(string path)
        //{
        //    FileInfo pathInfo = new FileInfo(path);
        //    return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        //}

        //[DllImport("kernel32.dll")]
        //static extern bool CreateSymbolicLink(
        //string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        //enum SymbolicLink
        //{
        //    File = 0,
        //    Directory = 1
        //}


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


        public static string PageSizeAnalizer(string logJobName, int jobId , string batchScanDirectory)
        {
            string result = "";
            List<PageSizeInfo> pagesSizeInfo = new List<PageSizeInfo>();

            try
            {
                lock (lockObj)
                {
                    string assignedCategory = "";
                    LogManager.Configuration.Variables["JobName"] = logJobName;                   

                    List<JobPageSize> jobPageSizes = new List<JobPageSize>();
                    ResultJobPageSizes resultJobPageSizes = new ResultJobPageSizes();
                    resultJobPageSizes = DBTransactions.GetPageSizesByJobID(logJobName , jobId);

                    if (resultJobPageSizes.RecordsCount == 0)
                    {
                        nlogger.Info("           Not Page Size definition found for this Job Type.");
                        Console.Out.WriteLineAsync("          Not Page Size definition found for this Job Type");
                    }
                    else
                    {
                        // Set the Job Page Sizes List in ascending order by Area
                        jobPageSizes = resultJobPageSizes.ReturnValue;
                        jobPageSizes = jobPageSizes.OrderBy(s => s.Area).ToList();

                        // Create the Batch Page Size Categories Data Object
                        // Unknown is used to classified images that do notfallow in any category
                        PageSizeInfo pageSize = new PageSizeInfo();
                        pageSize.Category = "Unknown";
                        pageSize.ImageCount = 0;
                        pagesSizeInfo.Add(pageSize);
                        foreach (JobPageSize jobPageSize in jobPageSizes)
                        {
                            pageSize = new PageSizeInfo();
                            pageSize.Category = jobPageSize.CategoryName;
                            pageSize.ImageCount = 0;
                            pagesSizeInfo.Add(pageSize);
                        }

                        // Get tif files from Batch Scan Directory
                        string[] filesInRestingLocation = System.IO.Directory.GetFiles(batchScanDirectory, "*.tif", System.IO.SearchOption.AllDirectories);

                        // Analize Image File Properties ...
                        foreach (var file in filesInRestingLocation)
                        {
                            using (Tiff image = Tiff.Open(file, "r"))
                            {
                                FieldValue[] value = image.GetField(TiffTag.IMAGEWIDTH);
                                int width = value[0].ToInt();

                                value = image.GetField(TiffTag.IMAGELENGTH);
                                int height = value[0].ToInt();

                                value = image.GetField(TiffTag.XRESOLUTION);
                                float dpiX = value[0].ToFloat();

                                value = image.GetField(TiffTag.YRESOLUTION);
                                float dpiY = value[0].ToFloat();

                                nlogger.Info("           Analizing File : " + file);
                                nlogger.Info("              High: " + (height / dpiY).ToString());
                                nlogger.Info("              Width: " + (width / dpiX).ToString());
                                nlogger.Info("              Area: " + ((height / dpiY) * (width / dpiX)).ToString());
                                //Console.Out.WriteLineAsync("          Analizing File : " + file);
                                //Console.Out.WriteLineAsync("                High: " + (height / dpiY).ToString());
                                //Console.Out.WriteLineAsync("                Width: " + (width / dpiX).ToString());
                                //Console.Out.WriteLineAsync("                Area: " + ((height / dpiY) * (width / dpiX)).ToString());

                                // Check in which Category it follows through
                                assignedCategory = "Unknown";
                                foreach (JobPageSize jobPageSize in jobPageSizes)
                                {
                                    if (!(jobPageSize.Area < ((height / dpiY) * (width / dpiX))))
                                    {
                                        assignedCategory = jobPageSize.CategoryName;
                                        break;
                                    }
                                }
                                nlogger.Info("              Size Category: " + assignedCategory);
                                // Account for the Category Found
                                foreach (PageSizeInfo pageSizeInfo in pagesSizeInfo)
                                {
                                    if (pageSizeInfo.Category == assignedCategory)
                                    {
                                        pageSizeInfo.ImageCount++;
                                        break;
                                    }
                                }                                
                            }
                        }
                        result = JsonConvert.SerializeObject(pagesSizeInfo, Newtonsoft.Json.Formatting.Indented);
                        nlogger.Info("              Page Size Categories summary: " + result);
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
            return result;
        }

        /// <summary>
        /// This methid adjust the status of a batch with "Ready to Scan" Status to "Waiting for QC"
        /// </summary>
        /// <param name="logJobName"></param>
        /// <param name="job"></param>
        /// <param name="batch"></param>
        public static void BatchSynchronizerProcess(string logJobName, JobExtended job,  Batch batch)
        {
            string batchScanDirectory = "";
            string currentBatchStatus = "";
            string pageSizesCount = "";
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();
                    ResultFields resultFields = new ResultFields();
                    BatchDocs document = new BatchDocs();

                    string line;

                    if (string.IsNullOrEmpty(batch.BatchAlias)) batch.BatchAlias = "";

                    // We need to check if resting location uses Batch Number of Alias Name
                    nlogger.Info("      Processing Batch : " + batch.BatchNumber.Trim() + " ; Alias: " + batch.BatchAlias.Trim());
                    Console.Out.WriteLineAsync("      Processing Batch : " + batch.BatchNumber.Trim() + " ; Alias: " + batch.BatchAlias.Trim());

                    // Setting Batch Scanning Directory
                    batchScanDirectory = job.ScanningFolder.Trim() + "\\" + job.JobName.Trim() + "\\" + batch.BatchNumber.Trim();

                    //Check if Batch Scan Directory Exist
                    if (!Directory.Exists(batchScanDirectory))
                    {
                        nlogger.Info("          Could not find Scan Directory for this Batch: " + batchScanDirectory);
                        Console.Out.WriteLineAsync("          Could not find Scan Directory for this Batch: " + batchScanDirectory);
                        return;
                    }
                    
                    nlogger.Info("          Directory: " + batchScanDirectory);                   
                    Console.Out.WriteLineAsync("          Directory: " + batchScanDirectory);

                    //Check if Directory is locked by Capture Pro
                    if (File.Exists(batchScanDirectory + "\\lock"))
                    {
                        nlogger.Trace("          Directory is being locked by Kodak Capture Pro.");
                        Console.Out.WriteLineAsync("          Directory is being locked by Kodak Capture Pro.");
                        return;
                    }

                    // Check if Batch is Lock a Backup file , if backup file does not exist
                    if (File.Exists(batchScanDirectory + "\\info.lock"))
                    {
                        nlogger.Trace("          Batch is locked so it will be ignored.");
                        Console.Out.WriteLineAsync("          Batch is locked so it will be ignored.");
                        return;
                    }

                    // Create a temp copy of the Info File. We are going to open this copy to extract information from the Kodak Info File
                    if (File.Exists(batchScanDirectory + "\\info"))
                    {
                        if (!File.Exists(batchScanDirectory + "\\info.tmp"))
                        {
                            nlogger.Trace("          Creating a TMP copy of the Kodak Info File.");
                            Console.Out.WriteLineAsync("          Creating a TMP copy of the Kodak Info File.");
                            File.Copy(batchScanDirectory + "\\info", batchScanDirectory + "\\info.tmp");
                        }                        

                        // Create a Backup of Kodak Info File
                        if (!File.Exists(batchScanDirectory + "\\info.ori"))
                        {
                            nlogger.Trace("          Creating a backup copy of the Kodak Info File (info.ori).");
                            Console.Out.WriteLineAsync("          Creating a backup copy of the Kodak Info File (info.ori).");
                            File.Copy(batchScanDirectory + "\\info", batchScanDirectory + "\\info.ori");
                        }
                    }
                    else
                    {
                        nlogger.Trace("          Kodak Info find not found. This Batch will be ignored.");
                        Console.Out.WriteLineAsync("          Kodak Info find not found. This Batch will be ignored.");
                        return;
                    }

                    // Creating a Backup file , if backup file does not exist
                    if (!File.Exists(batchScanDirectory + "\\info.bck"))
                    {
                        nlogger.Trace("          Creating a Backup Copy (info.bck) from the original info file.");
                        Console.Out.WriteLineAsync("          Creating a Backup Copy (info.bck) from the original info file.");
                        File.Copy(batchScanDirectory + "\\info", batchScanDirectory + "\\info.bck");
                    }

                    // Locking Kodak Info File so no one can use it until a QC Operator unlock this Batch
                    nlogger.Trace("          Renaming original info file to info.lock.");
                    Console.Out.WriteLineAsync("          Renaming original info file to info.lock.");
                    File.Move(batchScanDirectory + "\\info", batchScanDirectory + "\\info.lock");

                    // Read the file and display it line by line.                      
                    using (StreamReader file = new System.IO.StreamReader(batchScanDirectory + "\\info.tmp"))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            Console.Out.WriteLineAsync("          Reading line: " + line);
                            switch (line.Split('=').First().Trim())
                            {
                                case "Key":
                                    // Do nothing
                                    break;
                                case "batch.OutputStartDatetime":
                                    if (string.IsNullOrEmpty(line.Split('=').Last()))
                                        batch.OutputDate = Convert.ToDateTime(line.Split('=').Last());
                                    break;
                                case "batch.errorstate":
                                    batch.KodakErrorState = line.Split('=').Last();
                                    break;
                                case "batch.locked":
                                    // Do nothing
                                    break;
                                case "batch.OutputWorkstationID":
                                    batch.OutputStation = line.Split('=').Last();
                                    break;
                                case "batch.OutputUserID":
                                    batch.OutputBy = line.Split('=').Last();
                                    break;
                                case "batch.CreatedUserID":
                                    batch.ScanOperator = line.Split('=').Last();
                                    break;
                                case "batch.CreatedDatetime":
                                    if (string.IsNullOrEmpty(line.Split('=').Last()))
                                        batch.ScannedDate = Convert.ToDateTime(line.Split('=').Last());
                                    break;
                                case "batch.state":
                                    batch.KodakStatus = line.Split('=').Last();
                                    break;
                                case "batch.CreatedWorkstationID":
                                    batch.ScanStation = line.Split('=').Last();
                                    break;
                                case "batch.ModifiedUserID":
                                    batch.ModifiedBy = line.Split('=').Last();
                                    break;
                                case "batch.ModifiedDatetime":
                                    if (string.IsNullOrEmpty(line.Split('=').Last()))
                                        batch.ModifiedDate = Convert.ToDateTime(line.Split('=').Last());
                                    break;
                                case "batch.ModifiedWorkstationID":
                                    batch.ModifiedStation = line.Split('=').Last();
                                    break;
                                case "batch.DocumentCount":
                                    batch.InitialNumberOfDocuments = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.ImageCount":
                                    batch.InitialNumberOfScannedPages = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.PageCount":
                                    batch.NumberOfPages = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.ImageCount_Grayscale":
                                    batch.ImageCountGrayscale = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.ImageCount_BlackWhite":
                                    batch.ImageCountBlackWhite = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.ImageCount_Grayscale_Back":
                                    batch.ImageCountGrayscaleBack = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.ImageCount_Grayscale_Front":
                                    batch.ImageCountGrayscaleFront = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.ImageCount_BlackWhite_Back":
                                    batch.ImageCountBlackWhiteBack = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.ImageCount_BlackWhite_Front":
                                    batch.ImageCountBlackWhiteFront = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.FrontsCapture":
                                    batch.FrontsCaptured = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.FrontsRemoved":
                                    batch.FrontsRemoved = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.FrontsDeleted":
                                    batch.FrontsDeleted = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.FrontsRescanned":
                                    batch.FrontsRescanned = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.BacksCaptured":
                                    batch.BacksCaptured = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.BacksRemoved":
                                    batch.BacksRemoved = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.BacksDeleted":
                                    batch.BacksDeleted = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                case "batch.BacksRescanned":
                                    batch.BacksRescanned = Convert.ToInt32(line.Split('=').Last());
                                    break;
                                default:
                                    // Do nothing
                                    break;
                            }
                        }
                        file.Close();
                    }

                    // The Page Size Analizer was moved to the Indexer Services
                    //nlogger.Info("          Getting Page Sizes Count ...");
                    //Console.Out.WriteLineAsync("          Getting Page Sizes Count ...");

                    //pageSizesCount = PageSizeAnalizer(logJobName, job.JobID, batchScanDirectory);

                    //nlogger.Info("          Page Sizes Count Completed.");
                    //Console.Out.WriteLineAsync("          Page Sizes Count Completed.");

                    // Update Batch Information
                    nlogger.Info("          Updating Batch Information ...");
                    Console.Out.WriteLineAsync("          Updating Batch Information ...");
                    currentBatchStatus = batch.StatusFlag;
                    // batch.PageSizesCount = pageSizesCount;
                    batch.StatusFlag = "Waiting for QC";
                    result = DBTransactions.BatchUpdate(logJobName, batch);
                    if (result.ReturnCode != 0)
                    {
                        nlogger.Info("          " + result.Message);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLineAsync("          " + result.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }

                    // Register Transaction in Database Transaction Table
                    nlogger.Info("          Adding Batch Transaction Event to the Database.");
                    Console.Out.WriteLineAsync("          Adding Batch Transaction Event to the Database.");
                    result = DBTransactions.BatchTrackingEvent(logJobName, batch.BatchNumber, currentBatchStatus);
                    if (result.ReturnCode != 0)
                    {
                        nlogger.Info("          " + result.Message);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLineAsync("          " + result.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }

                    // Wrapping up Batch Procssing
                    nlogger.Info("      Batch " + batch.BatchNumber + " was processed successfully.");
                    Console.Out.WriteLineAsync("      Batch " + batch.BatchNumber + " was processed successfully.");
                }
            }
            catch (Exception ex)
            //catch (SchedulerException ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    // Cleaning flag files used in the Synchroniation process
                    if (!string.IsNullOrEmpty(batchScanDirectory))
                    {                       
                        if (File.Exists(batchScanDirectory + "\\info.tmp"))
                        {
                            nlogger.Trace("          Removing TMP copy of the Kodak Info File.");
                            Console.Out.WriteLineAsync("          Removing TMP copy of the Kodak Info File.");
                            File.Delete(batchScanDirectory + "\\info.tmp");
                        }
                        if (File.Exists(batchScanDirectory + "\\info.lock"))
                        {
                            nlogger.Trace("          Unlocking Kodak Info File.");
                            Console.Out.WriteLineAsync("          Unlocking Kodak Info File.");
                            File.Move(batchScanDirectory + "\\info.lock", batchScanDirectory + "\\info");
                        }
                    }
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
                    string jobBatchScanDirectory = "";

                    jobBatchScanDirectory = job.ScanningFolder;
                    nlogger.Info("  Processing Job:  " + job.JobName.Trim());
                    Console.Out.WriteLineAsync("    Processing Job:  " + job.JobName.Trim());

                    // Get Batches in "Waiting for Synchronizer"
                    nlogger.Info("      Getting Batches with the status Ready to Scan ...");
                    Console.Out.WriteLineAsync("      Batches with the status Ready to Scan ...");
                    ResultBatches resulBatches = new ResultBatches();
                    resulBatches = DBTransactions.GetBatches(logJobName, job.JobName.Trim(),"Ready to Scan");

                    if (resulBatches.RecordsCount == 0)
                    {
                        nlogger.Info("      Not Batches were found.");
                        Console.Out.WriteLineAsync("      Not Batches were found.");
                        return;
                    }
                    else
                    {
                        // Check if Capture Pro Scan Location Exist
                        if (!Directory.Exists(jobBatchScanDirectory))
                        {
                            nlogger.Info("      Directory does not exist: " + jobBatchScanDirectory);
                            nlogger.Info("      Skipping Job Type: " + job.JobName.Trim());
                            Console.Out.WriteLineAsync("        Directory does not exist: " + jobBatchScanDirectory);
                            Console.Out.WriteLineAsync("        Skipping Job Type: " + job.JobName.Trim());
                            return;
                        }
                        else
                        {
                            foreach (Batch batch in resulBatches.ReturnValue)
                            {
                                BatchSynchronizerProcess(logJobName, job,  batch);
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

        public static void SynchronizerProcess(string logJobName, int jobID)
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
                        localCronJobsList = SynchronizerService.cronJobsList;
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
            SynchronizerService service1 = new SynchronizerService(args);
            service1.Startup(args);
            do
            {
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);            

#else
            ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new  SynchronizerService(args)
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
                resultProcess = DBTransactions.GetProcessByName("Synchronizer");
                if (resultProcess.ReturnCode == 0)
                {
                    // Check list of processes
                    foreach (ScanningServicesDataObjects.GlobalVars.Process process in resultProcess.ReturnValue)
                    {
                        // When Job ID is = 0 means that rule applies for all Jobs
                        if (process.JobID == 0)
                            process.JobName = "ALL";

                        // Process only recordsa that match Host Name
                        if (SynchronizerService.hostName == process.StationName.Trim())
                        {
                            // 1. Check if process exist in the cronJob List anb if the cron exist, verify is the schedule has changed
                            //    This loop check if processes in the Database are already registerd in the Process List
                            cronJobFound = false;
                            cronJobChanged = false;
                            foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in SynchronizerService.cronJobsList)
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
                                        SynchronizerService.cronJobsList.Add(jobProcess);

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
                                        foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in SynchronizerService.cronJobsList)
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
                                                    SynchronizerService.cronJobsList.Remove(cronJob);
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

                    Synchronizer.Program.SynchronizerProcess(logJobName, Convert.ToInt32(context.JobDetail.Key.Name.Replace("JOB-", "")));

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
