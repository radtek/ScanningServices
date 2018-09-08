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

namespace AutoImport
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
        public static void AnalizeJob(string logJobName, JobExtended job)
        {
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();
                    string line;
                    Boolean boxNumberFound;
                    string batchNumber;
                    string aliasName;
                    string jobScanDirectory = "";
                    string jobAutoImportDirectory = "";
                    string folderName;
                    Boolean batchExist;

                    nlogger.Info("  Processing Job:  " + job.JobName.Trim());
                    Console.Out.WriteLineAsync("    Processing Job:  " + job.JobName.Trim());
                    // Check if AutoImport State has been set for this Job Type
                    nlogger.Info("      Enable Flag: \"" + job.AutoImportEnableFlag.ToString() + "\"");
                    Console.Out.WriteLineAsync("      Enable Flag: \"" + job.AutoImportEnableFlag.ToString() + "\"");
                    if (job.AutoImportEnableFlag)
                    {
                        // Build the full Job Type directory ...
                        //jobScanDirectory = job.AutoImportWatchFolder.Trim() + "\\" + job.JobName.Trim();
                        jobScanDirectory = job.ScanningFolder.Trim() + "\\" + job.JobName.Trim();
                        jobAutoImportDirectory = job.AutoImportWatchFolder.Trim() + "\\" + job.JobName.Trim();
                        nlogger.Info("      Traversing directory: " + jobScanDirectory);
                        Console.Out.WriteLineAsync("        Traversing directory: " + jobScanDirectory);

                        // Get Existing Batches for this Job Type in SSS
                        ResultBatches resulBatches = new ResultBatches();
                        resulBatches = DBTransactions.GetBatchesbyJobType(logJobName, job.JobName.Trim());
                       
                        if (resulBatches.ReturnCode != 0) return;

                        // Get the top level directories
                        if (!Directory.Exists(jobScanDirectory))
                        {
                            nlogger.Info("      Directory does not exist: " + jobScanDirectory);
                            Console.Out.WriteLineAsync("        Directory does not exist: " + jobScanDirectory);
                            nlogger.Info("      Creating Job Type Directory: " + job.JobName.Trim());
                            Console.Out.WriteLineAsync("        Creating Job Type Directory: " + job.JobName.Trim());
                            Directory.CreateDirectory(jobScanDirectory);

                            //nlogger.Info("      Skipping Job Type: " + job.JobName.Trim());                            
                            //Console.Out.WriteLineAsync("        Skipping Job Type: " + job.JobName.Trim());
                            //return;
                        }

                        string[] folders = System.IO.Directory.GetDirectories(jobScanDirectory, "*", System.IO.SearchOption.TopDirectoryOnly);

                        foreach (string folder in folders)
                        {
                            // Work only with non symbolic folders
                            if (!General.IsSymbolic(folder))
                            {
                                // Get the folder Name which will be used a Batch Alias Name to query SSS Datbase
                                aliasName = Path.GetFileName(Path.GetDirectoryName(folder + "\\"));
                                nlogger.Info("        Checking directory: " + folder);
                                nlogger.Info("          Alias Name: " + aliasName);
                                Console.Out.WriteLineAsync("        Checking directory: " + folder);
                                Console.Out.WriteLineAsync("          Alias Name: " + aliasName);

                                // Check if Folder contains a "lock File", if it does, skip the folder
                                if (File.Exists(folder + "\\lock"))
                                {
                                    nlogger.Info("          Lock file found. Directory " + aliasName + " will be ignored.");
                                    Console.Out.WriteLineAsync("            Lock file found. Directory " + aliasName + " will be ignored.");
                                }
                                else
                                {
                                    // Check if Batch exist in resultBatches List
                                    batchExist = false;
                                    foreach (Batch batch in resulBatches.ReturnValue)
                                    {
                                        if (batch.BatchAlias.Trim().ToUpper() == aliasName.ToUpper())
                                        {
                                            batchExist = true;
                                            break;
                                        }
                                    }
                                    if (!batchExist)
                                    {
                                        // This is a new Batch
                                        nlogger.Info("          Batch " + aliasName + " will be processed as new Batch.");
                                        Console.Out.WriteLineAsync("            Batch " + aliasName + " will be processed as new Batch.");
                                        // Get the batch Number fon index file (usse the first non empty value found)
                                        boxNumberFound = false;
                                        batchNumber = "";
                                        string[] files = System.IO.Directory.GetFiles(folder, "index", System.IO.SearchOption.AllDirectories);
                                        foreach (string item in files)
                                        {
                                            //Console.Out.WriteLineAsync("    File: " + item);
                                            // Read the Index and display it line by line.  
                                            System.IO.StreamReader file = new System.IO.StreamReader(item);
                                            while ((line = file.ReadLine()) != null && !boxNumberFound)
                                            {
                                                switch (line.Split('=').First())
                                                {
                                                    case "BoxNumber":
                                                        batchNumber = line.Split('=').Last();
                                                        batchNumber = batchNumber.Replace("\"", "");
                                                        if (!string.IsNullOrEmpty(batchNumber))
                                                        {
                                                            nlogger.Info("          Batch Number: " + line.Split('=').Last());
                                                            Console.Out.WriteLineAsync("            Batch Number: " + line.Split('=').Last());
                                                            boxNumberFound = true;
                                                        }
                                                        break;

                                                    default:
                                                        // Do nothing
                                                        break;
                                                }
                                            }
                                            file.Close();
                                            if (boxNumberFound)
                                                break;
                                        }

                                        if (boxNumberFound)
                                        {
                                            // Check if Batch Name already exist in the Database   
                                            batchExist = false;
                                            foreach (Batch batch in resulBatches.ReturnValue)
                                            {
                                                if (batch.BatchNumber.Trim().ToUpper() == batchNumber.ToUpper())
                                                {
                                                    nlogger.Info("          Batch Number " + batchNumber + " already exist in the Database. This batch will be ignored.");
                                                    nlogger.Info("          This Folder needs to be deleted: " + folder);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Out.WriteLineAsync("          Batch Number " + batchNumber + " already exist in the Database. This batch will be ignored.");
                                                    Console.Out.WriteLineAsync("          This Folder needs to be deleted: " + folder);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    batchExist = true;
                                                    break;
                                                }
                                            }

                                            if (!batchExist)
                                            {
                                               // Create a Soft like using the Batch Number
                                                // Check first if softlink exist
                                                bool softLinkResult;
                                                if (Directory.Exists(jobScanDirectory + "\\" + batchNumber))
                                                {
                                                    nlogger.Info("          Soft link already exist: " + jobScanDirectory + "\\" + batchNumber);
                                                    Console.Out.WriteLineAsync("            Soft link already exist: " + jobScanDirectory + "\\" + batchNumber);
                                                    softLinkResult = true;
                                                }
                                                else
                                                {
                                                    softLinkResult = General.CreateSymbolicLink(jobScanDirectory + "\\" + batchNumber, folder, General.SymbolicLink.Directory);
                                                    if (softLinkResult)
                                                    {
                                                        nlogger.Info("          Soft link was create successfuly:  " + jobScanDirectory + "\\" + batchNumber);
                                                        Console.Out.WriteLineAsync("            Soft link was create successfuly:  " + jobScanDirectory + "\\" + batchNumber);
                                                    }
                                                    else
                                                    {
                                                        nlogger.Info("          Failed creating Soft link:  " + jobScanDirectory + "\\" + batchNumber);
                                                        Console.Out.WriteLineAsync("            Failed creating Soft link:  " + jobScanDirectory + "\\" + batchNumber);
                                                    }
                                                }

                                                if (softLinkResult)
                                                {
                                                    // Register Batch in Database
                                                    nlogger.Info("          Adding Batch to the Database.");
                                                    Console.Out.WriteLineAsync("            Adding Batch to the Database.");
                                                    result = DBTransactions.BatchRegistration(logJobName, batchNumber, aliasName, job);
                                                    if (result.ReturnCode != 0)
                                                    {
                                                        nlogger.Info("          " + result.Message);
                                                        nlogger.Info("           This Alias needs to be deleted: " + jobScanDirectory + "\\" + batchNumber);
                                                         Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.Out.WriteLineAsync("          " + result.Message);
                                                        Console.Out.WriteLineAsync("           This Alias needs to be deleted: " + jobScanDirectory + "\\" + batchNumber);
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                    }
                                                    else
                                                    {
                                                        // register Transaction in Database Transaction Table
                                                        nlogger.Info("          Adding Batch Transaction Event to the Database.");
                                                        Console.Out.WriteLineAsync("            Adding Batch Transaction Event to the Database.");
                                                        result = DBTransactions.BatchTrackingEvent(logJobName, batchNumber);
                                                        if (result.ReturnCode != 0)
                                                        {
                                                            nlogger.Info("          " + result.Message);
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.Out.WriteLineAsync("          " + result.Message);
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                            //return;
                                                        }
                                                    }
                                                }
                                            }
                                            
                                        }
                                        else
                                        {
                                            nlogger.Info("          Box Number for Batch " + aliasName + " was not found. This batch will be ignored.");
                                            Console.Out.WriteLineAsync("            Box Number for Batch " + aliasName + " was not found. This batch will be ignored.");
                                        }
                                    }
                                    else
                                    {
                                        // This batch already exist in the Database
                                        // Skip this Batch
                                        nlogger.Info("          Batch " + aliasName + " already exist in the Database. This batch will be ignored.");
                                        Console.Out.WriteLineAsync("            Batch " + aliasName + " already exist in the Database. This batch will be ignored.");
                                    }
                                }
                            }
                        }

                        // Create Auto-Import working directories
                        if (!Directory.Exists(jobAutoImportDirectory + "\\Hot"))
                        {
                            nlogger.Info("          Hot folder does not exist, so it will be created: " + jobAutoImportDirectory + "\\Hot");
                            Console.Out.WriteLineAsync("          Hot folder does not exist, so it will be created: " + jobAutoImportDirectory + "\\Hot");
                            Directory.CreateDirectory(jobAutoImportDirectory + "\\Hot");
                        }
                        if (!Directory.Exists(jobAutoImportDirectory + "\\Scanned"))
                        {
                            nlogger.Info("          Scanned folder does not exist, so it will be created: " + jobAutoImportDirectory + "\\Scanned");
                            Console.Out.WriteLineAsync("          Scanned folder does not exist, so it will be created: " + jobAutoImportDirectory + "\\Scanned");
                            Directory.CreateDirectory(jobAutoImportDirectory + "\\Scanned");
                        }
                        if (!Directory.Exists(jobAutoImportDirectory + "\\Backup"))
                        {
                            nlogger.Info("          Hot Backup does not exist, so it will be created: " + jobAutoImportDirectory + "\\Backup");
                            Console.Out.WriteLineAsync("          Hot Backup does not exist, so it will be created: " + jobAutoImportDirectory + "\\Backup");
                            Directory.CreateDirectory(jobAutoImportDirectory + "\\Backup");
                        }

                        // Browse Auto Import Backup Folder and check if Batches in this location have been registered aleready in Scanning Services
                        nlogger.Info("          Analizing Backup Folder: " + jobAutoImportDirectory + "\\Backup");
                        Console.Out.WriteLineAsync("          Analizing Backup Folder: " + jobAutoImportDirectory + "\\Backup");
                        folders = System.IO.Directory.GetDirectories(jobAutoImportDirectory + "\\Backup", "*", System.IO.SearchOption.TopDirectoryOnly);
                        foreach (string folder in folders)
                        {
                            batchExist = false;
                            folderName = Path.GetFileName(Path.GetDirectoryName(folder + "\\"));
                            nlogger.Info("              Folder:" + folderName);
                            Console.Out.WriteLineAsync("            Folder:" + folderName);
                            // Check if Batch exist in resultBatches List
                            foreach (Batch batch in resulBatches.ReturnValue)
                            {
                                if (batch.BatchAlias.Trim().ToUpper() == folderName.ToUpper() ||
                                    batch.BatchNumber.Trim().ToUpper() == folderName.ToUpper())
                                {
                                    // The batch is already exist in Scanning Services so Backup is not needed anymore
                                    nlogger.Info("              Batch already in Scanning services. Folder will be deleted.");
                                    Console.Out.WriteLineAsync("            Batch already in Scanning services. Folder will be deleted.");
                                    Directory.Delete(folder, true);
                                    batchExist = true;
                                    break;
                                }
                            }
                            if (!batchExist)
                            {
                                nlogger.Info("              Batch is not yet in Scanning services. Folder will be ignored.");
                                Console.Out.WriteLineAsync("            Batch is not yet in Scanning services. Folder will be ignored.");
                            }
                        }
                        nlogger.Info("          Finished Analizing Backup Folder: " + jobAutoImportDirectory + "\\Backup");
                        Console.Out.WriteLineAsync("          Finished Analizing Backup Folder: " + jobAutoImportDirectory + "\\Backup");

                        // Count the number of Batches in Hot folder
                        // We will feed batches into the Hot Foder upto 10 Batches
                        int remainingCapacity = 0;
                        folders = System.IO.Directory.GetDirectories(jobAutoImportDirectory + "\\Hot", "*", System.IO.SearchOption.TopDirectoryOnly);
                        if (folders.Count() < 10)
                        {
                            remainingCapacity = 10 - folders.Count();
                            // get the folders in the Scanned directory
                            folders = System.IO.Directory.GetDirectories(jobAutoImportDirectory + "\\Scanned", "*", System.IO.SearchOption.TopDirectoryOnly);
                            foreach (string folder in folders)
                            {                                
                                if (remainingCapacity == 0) break; // exit when we reach the limit of the Hot Folder

                                string batchName = new DirectoryInfo(folder).Name;

                                // Backup Batch and feed Hot Folder
                                nlogger.Info("          Backing up Batch ...");
                                nlogger.Info("            Folder: " + folder);
                                nlogger.Info("            Into: " + jobAutoImportDirectory + "\\Backup\\" + batchName);
                                Console.Out.WriteLineAsync("          Backing up Batch ...");
                                Console.Out.WriteLineAsync("            Folder: " + folder);                              
                                Console.Out.WriteLineAsync("            Into: " + jobAutoImportDirectory + "\\Backup\\" + batchName);                                

                                CopyFolder(folder, jobAutoImportDirectory + "\\Backup\\" + batchName, "*", job.JobName.Trim());

                                // Copy Batch to Hot Folder
                                nlogger.Info("          Copy Batch to Hot Folder ...");
                                nlogger.Info("            Folder: " + folder);
                                nlogger.Info("            Into: " + jobAutoImportDirectory + "\\Hot\\" + batchName);
                                Console.Out.WriteLineAsync("          Copying Batch to Hot Folder ...");
                                Console.Out.WriteLineAsync("            Folder: " + folder);
                                Console.Out.WriteLineAsync("            Into: " + jobAutoImportDirectory + "\\Hot\\" + batchName);
                                
                                CopyFolder(folder, jobAutoImportDirectory + "\\Hot\\" + batchName, "*", job.JobName.Trim());

                                // Remove Batch from Scanned Folder
                                nlogger.Info("          Removing Batch from Scanned Folder ...");
                                Console.Out.WriteLineAsync("          Removing Batch from Scanned Folder ...");
                                Directory.Delete(folder, true);

                                remainingCapacity = remainingCapacity - 1;
                            }
                            nlogger.Info("      Finished Traversing directory: " + jobScanDirectory);
                            Console.Out.WriteLineAsync("        Finished Traversing directory: " + jobScanDirectory);

                        }
                        else
                        {
                            nlogger.Info("              Hot folder is at the limit of its capcacity (10 Batches). Wait for next iteration.");
                            Console.Out.WriteLineAsync("              Hot folder is at the limit of its capcacity (10 Batches). Wait for next iteration.");
                        }
                    }
                    else
                    {
                        nlogger.Info("          Job Type " + job.JobName.Trim() + " not set for Auto Import. Skiping Job Type.") ;
                        Console.Out.WriteLineAsync("        Job Type " + job.JobName.Trim() + " not set for Auto Import. Skiping Job Type.");
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

        public static void AutoImportProcess(string logJobName, int jobID)
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
                        localCronJobsList = AutoImportService.cronJobsList;
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
            AutoImportService service1 = new AutoImportService(args);
            service1.Startup(args);
            do
            {
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);            

#else
                        ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new  AutoImportService(args)
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
                resultProcess = DBTransactions.GetProcessByName("Auto Import");
                if (resultProcess.ReturnCode == 0)
                {
                    // Check list of processes
                    foreach (ScanningServicesDataObjects.GlobalVars.Process process in resultProcess.ReturnValue)
                    {
                        // When Job ID is = 0 means that rule applies for all Jobs
                        if (process.JobID == 0)
                            process.JobName = "ALL";

                        // Process only recordsa that match Host Name
                        if (AutoImportService.hostName == process.StationName.Trim())
                        {
                            // 1. Check if process exist in the cronJob List anb if the cron exist, verify is the schedule has changed
                            //    This loop check if processes in the Database are already registerd in the Process List
                            cronJobFound = false;
                            cronJobChanged = false;
                            foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in AutoImportService.cronJobsList)
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
                                        AutoImportService.cronJobsList.Add(jobProcess);

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
                                        foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in AutoImportService.cronJobsList)
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
                                                    AutoImportService.cronJobsList.Remove(cronJob);
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

                    AutoImport.Program.AutoImportProcess(logJobName, Convert.ToInt32(context.JobDetail.Key.Name.Replace("JOB-", "")));

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
