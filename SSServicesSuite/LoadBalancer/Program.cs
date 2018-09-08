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

namespace LoadBalancer
{
    static class Program
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        static object lockObj = new object();

        //public static List<General.BatchFolderInformation> BatchFoldersList = new List<General.BatchFolderInformation>();

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


        /// <summary>
        /// This method is designed to move a Batch folder from the Load Balancer Location to a File Conversion 
        /// target directory.
        /// </summary>
        /// <param name="logJobName"></param>
        /// <param name="fileConversionFolder"></param>
        /// <param name="batchItem"></param>
        public static void BalancerProcess(string logJobName, string fileConversionFolder, General.BatchFolderInformation batchItem)
        {
            string targetLocation = "";
            string eventDescription = "";
            string newStatus = "";
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();
                    string currentBatchStatus = "";
                    Batch batch = null;
                    
                    // Get Batch Information in order to retrieve the Kodak Batch Number
                    ResultBatches resulBatches = new ResultBatches();
                    resulBatches = DBTransactions.GetBatches(logJobName, batchItem.batchNumber);

                    if (resulBatches.ReturnCode != 0)
                    {
                        nlogger.Info("          " + resulBatches.Message);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLineAsync("          " + resulBatches.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    if (resulBatches.RecordsCount == 0)
                    {
                        nlogger.Info("      The Batch does not exist in the Database.");
                        Console.Out.WriteLineAsync("      The Batch does not exist in the Database.");
                        return;
                    }
                    batch = resulBatches.ReturnValue[0];

                    if (!Directory.Exists(fileConversionFolder))
                    {
                        nlogger.Info("      File Conversion Directory not found: " + fileConversionFolder);
                        Console.Out.WriteLineAsync("      File Conversion Directory not found: " + fileConversionFolder);
                    }
                    else
                    {
                        if (!File.Exists(batchItem.FolderFullPath + "\\Documents.XML"))
                        {
                            nlogger.Info("      Documents.xml file was not found.");
                            Console.Out.WriteLineAsync("      Documents.xml file was not found.");
                            return;
                        }
                        else
                        { 
                            // Create Batch Directory in Target Location
                            targetLocation = fileConversionFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber;
                            if (!Directory.Exists(targetLocation))
                            {
                                Directory.CreateDirectory(targetLocation);
                            }
                            else
                            {
                                // Skip this batch
                                nlogger.Info("      A Folder with the Batch Name was found in File Coversion location: " + targetLocation);
                                nlogger.Info("      Skipping Batch: " + batchItem.batchNumber);
                                Console.Out.WriteLineAsync("      A Folder with the Batch Name was found in File Coversion location: " + targetLocation);
                                Console.Out.WriteLineAsync("      Skipping Batch: " + batchItem.batchNumber);
                                return;
                            }

                            // Rename Documents.xml file to prevent any other process to open it 
                            File.Move(batchItem.FolderFullPath + "\\Documents.XML", batchItem.FolderFullPath + "\\Documents.MOVING");

                            // Copy Batch to the File Conversion Target Location
                            nlogger.Info("      Copying Batch to File Coversion location: " + targetLocation);
                            Console.Out.WriteLineAsync("      Copying Batch to File Coversion location: " + targetLocation);

                            //File.Copy(batchItem.FolderFullPath + "\\Documents.MOVING", targetLocation + "\\Documents.MOVING");
                            //CopyFolder(batchItem.FolderFullPath, targetLocation, "*." + batchItem.FileExtension, logJobName);
                            CopyFolder(batchItem.FolderFullPath, targetLocation, "*", logJobName);

                            // Once the copy is done, retablish the name of the metadata file on the Target Location
                            nlogger.Info("      Renaming Documents.MOVING to Documents.xml in target location.");
                            Console.Out.WriteLineAsync("      Renaming Documents.MOVING to Documents.xml in target location.");
                            File.Move(targetLocation + "\\Documents.MOVING", targetLocation + "\\Documents.xml");

                            // Create a Backup Directory
                            if (!string.IsNullOrEmpty(batchItem.backupFolder))
                            {
                                if (!Directory.Exists(batchItem.backupFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber.Trim()))
                                {
                                    nlogger.Info("      Creating Batch Backup Folder: " + batchItem.backupFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber.Trim());
                                    Console.Out.WriteLineAsync("      Creating Batch Backup Folder: " + batchItem.backupFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber.Trim());
                                    Directory.CreateDirectory(batchItem.backupFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber.Trim());
                                }
                                // Backing up Batch Folder
                                File.Move(batchItem.FolderFullPath + "\\Documents.MOVING", batchItem.backupFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber.Trim() + "\\Documents.xml");
                                nlogger.Info("      Baking up Batch Folder ...");
                                Console.Out.WriteLineAsync("      Baking up Batch Folder ...");
                                //CopyFolder(batchItem.FolderFullPath, batchItem.backupFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber.Trim(), "*." + batchItem.FileExtension, logJobName);
                                CopyFolder(batchItem.FolderFullPath, batchItem.backupFolder + "\\" + batchItem.JobName.Trim() + "\\" + batchItem.batchNumber.Trim(), "*", logJobName);
                            }
                            else
                            {
                                nlogger.Info("      There is no an assigned Backup Directory for this Job.");
                                Console.Out.WriteLineAsync("      There is no an assigned Backup Directory for this Job.");
                            }


                            // Remove Batch folder from source location
                            if (Directory.Exists(batchItem.FolderFullPath))
                            {
                                nlogger.Info("      Deleting Batch Source location: " + batchItem.FolderFullPath);
                                Console.Out.WriteLineAsync("      Deleting Batch Source location: " + batchItem.FolderFullPath);
                                Directory.Delete(batchItem.FolderFullPath, true);
                            }
                            else
                            {
                                // This should never happen
                                nlogger.Info("      Could not find Batch Source Location: " + batchItem.FolderFullPath);
                                Console.Out.WriteLineAsync("      Could not find Batch Source Location: " + batchItem.FolderFullPath);
                            }

                            // Update Batch Status Flag  
                            nlogger.Info("      Updating Batch Status FLag to \"Waitng for PDF Conversion\".");
                            Console.Out.WriteLineAsync("      Updating Batch Status FLag to \"Waiting for PDF Conversion\"");
                            currentBatchStatus = batch.StatusFlag;
                            batch.StatusFlag = "Waiting for PDF Conversion";
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
                            nlogger.Info("      Adding Batch Transaction Event to the Database.");
                            Console.Out.WriteLineAsync("      Adding Batch Transaction Event to the Database.");
                            eventDescription = "Batch Sent for File Conversion";
                            newStatus = "Waiting for PDF Conversion";
                            result = DBTransactions.BatchTrackingEvent(logJobName, batch.BatchNumber, currentBatchStatus, newStatus, eventDescription);
                            if (result.ReturnCode != 0) return;
                            if (result.ReturnCode != 0)
                            {
                                nlogger.Info("          " + result.Message);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Out.WriteLineAsync("          " + result.Message);
                                Console.ForegroundColor = ConsoleColor.White;
                                return;
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

        
        // Build list of batch folders found in the Load Balancer Watch Folder
        public static List<General.BatchFolderInformation> BuildFoldersList(string logJobName, JobExtended job, List<General.BatchFolderInformation> BatchFoldersList)
        {
            try
            {
                lock (lockObj)
                {
                    if (!string.IsNullOrEmpty(job.LoadBalancerWatchFolder))
                    {
                        if (Directory.Exists(job.LoadBalancerWatchFolder + "\\" + job.JobName.Trim()))
                        {
                            string[] folders = System.IO.Directory.GetDirectories(job.LoadBalancerWatchFolder + "\\" + job.JobName.Trim(), "*", System.IO.SearchOption.TopDirectoryOnly);
                            foreach (string folder in folders)
                            {
                                if (File.Exists(folder + "\\Documents.xml"))
                                {
                                    General.BatchFolderInformation batchFolder = new General.BatchFolderInformation();
                                    batchFolder.FolderFullPath = folder;
                                    batchFolder.JobID = job.JobID;
                                    batchFolder.backupFolder = job.BackupFolder;
                                    batchFolder.JobName = job.JobName.Trim();
                                    batchFolder.batchDeliveryFolder = job.BatchDeliveryWatchFolder.Trim();
                                    batchFolder.FileExtension = job.OutputFileType;
                                    batchFolder.batchNumber = Path.GetFileName(Path.GetDirectoryName(folder + "\\"));
                                    batchFolder.TimeStamp = Directory.GetCreationTime(folder);
                                    BatchFoldersList.Add(batchFolder);
                                }                                
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
            return BatchFoldersList;
        }

        public static void AnalizeJob(string logJobName, List<General.BatchFolderInformation> BatchFoldersList)
        {
            string currentBatchStatus = "";
            string fileConversionFolder = "";
            ResultBatches resulBatches = new ResultBatches();
            Batch batch = new Batch();
            int batchDocumentCount = 0;

            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();

                    foreach (General.BatchFolderInformation batchItem in BatchFoldersList)
                    {
                        nlogger.Info("  Processing Batch:  " + batchItem.batchNumber);
                        nlogger.Info("      Job:  " + batchItem.JobName);
                        nlogger.Info("      Folder:  " + batchItem.FolderFullPath);
                        Console.Out.WriteLineAsync("  Processing Batch:  " + batchItem.batchNumber);
                        Console.Out.WriteLineAsync("      Job:  " + batchItem.JobName);
                        Console.Out.WriteLineAsync("      Folder:  " + batchItem.FolderFullPath);
                        
                        
                        // Check if folder only have the 1 KB Documents.xml
                        string[] pdfFiles = System.IO.Directory.GetFiles(batchItem.FolderFullPath, "*.pdf", System.IO.SearchOption.AllDirectories);
                        string[] tiffFiles = System.IO.Directory.GetFiles(batchItem.FolderFullPath, "*.tif", System.IO.SearchOption.AllDirectories);
                        if ((pdfFiles.Count() + tiffFiles.Count()) == 0 && !batchItem.batchNumber.Contains(".REMOVE"))
                        {
                            // Batch is not ready for the Load Balancer
                            nlogger.Info("      Batch is still in Quality Control, so it will be ignored.");
                            Console.Out.WriteLineAsync("      Batch is still in Quality Control, so it will be ignored.");
                        }
                        else
                        {
                            // Look for Batch Information in order to retrieve the Kodak Batch Number
                            resulBatches = new ResultBatches();
                            resulBatches = DBTransactions.GetBatches(logJobName, batchItem.batchNumber);
                            if (resulBatches.RecordsCount == 0)
                            {
                                // Batch Does not exist, so report this issue
                                nlogger.Info("      Batch does not exist in the Database.");
                                Console.Out.WriteLineAsync("      Batch does not exist in the Database.");

                                nlogger.Info("      Deleting Folder: " + batchItem.FolderFullPath);
                                Console.Out.WriteLineAsync("      Deleting Folder: " + batchItem.FolderFullPath);
                                Directory.Delete(batchItem.FolderFullPath, true);
                            }
                            else
                            {
                                // Check if Batch's Jobs Type, based on the wach directory, is in the correct Job Directory.
                                if (resulBatches.ReturnValue[0].JobType.Trim() != logJobName)
                                {
                                    nlogger.Info("      This Batch is in the wrong output folder. The Job type for this batch is: " + resulBatches.ReturnValue[0].JobType.Trim());
                                    Console.Out.WriteLineAsync("      This Batch is in the wrong output folder. The Job type for this batch is: " + resulBatches.ReturnValue[0].JobType.Trim());
                                }
                                else
                                {
                                    // Check if number of files in Documents XML correspond to the number of files in the file system
                                    XmlDocument xml = new XmlDocument();
                                    xml.Load(batchItem.FolderFullPath + "\\Documents.xml");
                                    XmlNodeList documents = xml.SelectNodes("//document");
                                    batchDocumentCount = documents.Count;
                                    //string[] pdfFiles = System.IO.Directory.GetFiles(batchItem.FolderFullPath, "*.pdf", System.IO.SearchOption.AllDirectories);
                                    //string[] tiffFiles = System.IO.Directory.GetFiles(batchItem.FolderFullPath, "*.tif", System.IO.SearchOption.AllDirectories);

                                    if ((pdfFiles.Count() + tiffFiles.Count()) != batchDocumentCount)
                                    {
                                        nlogger.Info("      Number of documents expected with tif/pdf extension, does not match with the existing number of documents in the Batch directory. Batch will be ignored.");
                                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                                        Console.Out.WriteLineAsync("      Number of documents expected with tif/pdf extension ,does not match with the existing number of documents in the Batch directory. Batch will be ignored.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        // Consider sending an email notification to report this issue.
                                    }
                                    else
                                    {
                                        batch = resulBatches.ReturnValue[0];
                                        // Get Next File Conversion Station
                                        result = DBTransactions.GetNextAvailableFileConversionStation(logJobName);
                                        if (result.ReturnCode != 0)
                                        {
                                            nlogger.Info("      " + result.Message);
                                            nlogger.Info("      Failed Processing Batch:  " + batchItem.batchNumber);
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Out.WriteLineAsync("      " + result.Message);
                                            Console.Out.WriteLineAsync("      Failed Processing Batch:  " + batchItem.batchNumber);
                                            Console.ForegroundColor = ConsoleColor.White;
                                            return;
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(result.StringReturnValue))
                                            {
                                                nlogger.Info("      No File Conversion Station available at this moment.");
                                                Console.Out.WriteLineAsync("      No File Conversion Station available at this moment.");

                                                if (batch.StatusFlag != "Waiting for File Conversion Station")
                                                {
                                                    // Update Batch Status Flag  
                                                    nlogger.Info("      Updating Batch Status FLag to \"Waiting for File Conversion Station\".");
                                                    Console.Out.WriteLineAsync("      Updating Batch Status FLag to \"Waiting for File Conversion Station\".");
                                                    currentBatchStatus = batch.StatusFlag;
                                                    batch.StatusFlag = "Waiting for File Conversion Station";
                                                    result = DBTransactions.BatchUpdate(logJobName, batch);
                                                    if (result.ReturnCode != 0)
                                                    {
                                                        nlogger.Info("          " + result.Message);
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.Out.WriteLineAsync("          " + result.Message);
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                    }
                                                    else
                                                    {
                                                        // Register Transaction in Database Transaction Table
                                                        nlogger.Info("      Adding Batch Transaction Event to the Database.");
                                                        Console.Out.WriteLineAsync("      Adding Batch Transaction Event to the Database.");
                                                        result = DBTransactions.BatchTrackingEvent(logJobName, batch.BatchNumber, currentBatchStatus, "Waiting for File Conversion Station", "File Conversion Station assignment on hold");
                                                        if (result.ReturnCode != 0)
                                                        {
                                                            nlogger.Info("          " + result.Message);
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.Out.WriteLineAsync("          " + result.Message);
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                fileConversionFolder = result.StringReturnValue;
                                                // Prepare Batch for File Conversion
                                                BalancerProcess(logJobName, fileConversionFolder, batchItem);
                                            }
                                        }
                                    }
                                }                                                            
                            }
                            nlogger.Info("  End Processing Batch:  " + batchItem.batchNumber);
                            Console.Out.WriteLineAsync("  End Processing Batch:  " + batchItem.batchNumber);
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

        /// <summary>
        /// This methid get a list of Batch folders that ready for file conversion and order them by creation date
        /// The startegy to follow will be FIFO, first created, first processed.
        /// </summary>
        /// <param name="logJobName"></param>
        /// <param name="jobID"></param>
        public static void LoadBalancerProcess(string logJobName, int jobID)
        {
            List<General.BatchFolderInformation> BatchFoldersList = new List<General.BatchFolderInformation>();
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
                        //Under this condition, we will ignore Jobs that has their own Cron
                        // Cron for All Job has been identified
                        // 1. Get Jobs information
                        resultjobs = DBTransactions. GetJobs();
                        // 2. For each Job, check if exist in cronJobsList (use a local copy of the cronJobsList)
                        localCronJobsList = LoadBalancerService.cronJobsList;

                        // 3. Build Batch Folder List. This is the list of Batches that are ready for the Load Balancer
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
                                //AnalizeJob(logJobName, job);
                                BatchFoldersList = BuildFoldersList(logJobName, job, BatchFoldersList);
                            }
                            else
                            {
                                if (!cronJobFound)
                                {
                                    // The Cron Job was not found so it will go by the ALL rules 
                                    //AnalizeJob(logJobName, job);
                                    BatchFoldersList = BuildFoldersList(logJobName, job, BatchFoldersList);
                                }
                            }
                        }

                        // 4. Order BatchList by Folder Stamp Time (oldest first)
                        //BatchFoldersList = BatchFoldersList.OrderBy(x => x.TimeStamp).ToList();
                        BatchFoldersList.Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp));

                        AnalizeJob(logJobName, BatchFoldersList);
                    }
                    else
                    {
                        // 1. Get Job information
                        resultjobs = DBTransactions.GetJobByID(jobID);
                        JobExtended job = new JobExtended();
                        job = resultjobs.ReturnValue[0];
                        // Proces Job
                        //AnalizeJob(logJobName, job);
                        BatchFoldersList = BuildFoldersList(logJobName, job, BatchFoldersList);

                        // Order BatchList by Folder Stamp Time (oldest first)
                        BatchFoldersList.Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp));

                        AnalizeJob(logJobName, BatchFoldersList);
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
            LoadBalancerService service1 = new LoadBalancerService(args);
            service1.Startup(args);
            do
            {
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);

#else
            ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new  LoadBalancerService(args)
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
                resultProcess = DBTransactions.GetProcessByName("Load Balancer");
                if (resultProcess.ReturnCode == 0)
                {
                    // Check list of processes
                    foreach (ScanningServicesDataObjects.GlobalVars.Process process in resultProcess.ReturnValue)
                    {
                        // When Job ID is = 0 means that rule applies for all Jobs
                        if (process.JobID == 0)
                            process.JobName = "ALL";

                        // Process only recordsa that match Host Name
                        if (LoadBalancerService.hostName == process.StationName.Trim())
                        {
                            // 1. Check if process exist in the cronJob List anb if the cron exist, verify is the schedule has changed
                            //    This loop check if processes in the Database are already registerd in the Process List
                            cronJobFound = false;
                            cronJobChanged = false;
                            foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in LoadBalancerService.cronJobsList)
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
                                        LoadBalancerService.cronJobsList.Add(jobProcess);

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
                                        foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in LoadBalancerService.cronJobsList)
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
                                                    LoadBalancerService.cronJobsList.Remove(cronJob);
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

                    LoadBalancer.Program.LoadBalancerProcess(logJobName, Convert.ToInt32(context.JobDetail.Key.Name.Replace("JOB-", "")));

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
