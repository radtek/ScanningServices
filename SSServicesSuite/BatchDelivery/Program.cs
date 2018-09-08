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

namespace BatchDelivery
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

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void DeliveryProcess(string logJobName, JobExtended job, string jobBatchDeliveryDirectory, ResultBatches resulBatches)
        {
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();
                    string batchNumber = "";
                    string batchName;
                    int batchDocumentCount = 0;
                    Boolean batchExist;
                    string outputFileExtension = "";
                    string batchLocation = "";
                    string text = "";
                    Batch batch = null;
                    string currentBatchStatus = "";
                    string filePath;
                    string fileName;
                    ResultBatches resultBatchesAux = new ResultBatches();
                    int docCounter = 0;

                    if (!Directory.Exists(jobBatchDeliveryDirectory))
                    {                       
                        nlogger.Info("      Directory not found: " + jobBatchDeliveryDirectory);
                        Console.Out.WriteLineAsync("      Directory not found: " + jobBatchDeliveryDirectory);
                    }
                    else
                    {
                        string[] folders = System.IO.Directory.GetDirectories(jobBatchDeliveryDirectory, "*", System.IO.SearchOption.TopDirectoryOnly);
                        foreach (string folder in folders)
                        {

                            // 1. Get the folder Name which is the Batch Number or the Batch Alias Name, in other words, the Batch Name
                            batchDocumentCount = 0;
                            batchName = Path.GetFileName(Path.GetDirectoryName(folder + "\\"));
                            nlogger.Info("      Checking directory: " + folder);
                            nlogger.Info("          Batch Name: " + batchName);
                            Console.Out.WriteLineAsync("      Checking directory: " + folder);
                            Console.Out.WriteLineAsync("          Batch Name: " + batchName);

                            Console.Out.WriteLineAsync("          Checking if Directory is empty:" + IsDirectoryEmpty(folder));
                            // Check of the folder is empty
                            if (IsDirectoryEmpty(folder))
                            {
                                // Remove Folder
                                nlogger.Info("          Batch Folder is empty. The directory will be removed.");
                                Console.Out.WriteLineAsync("          Batch Folder is empty. The directory will be removed.");
                                Directory.Delete(folder, true);
                            }
                            else
                            {
                                // 2. Check if Folder contains a "DOCUMENTS.XML File", if it does not exist, report the error skip the folder 
                                if (File.Exists(folder + "\\Documents.xml"))
                                {
                                    nlogger.Info("            Documents.xml Metadata File found.");
                                    Console.Out.WriteLineAsync("            Documents.xml Metadata File found.");

                                    // 4. Check if Batch exist in resultBatches List. Ignore Batch Processing if not
                                    batchExist = false;
                                    foreach (Batch batchAux in resulBatches.ReturnValue)
                                    {
                                        if (string.IsNullOrEmpty(batchAux.BatchAlias))
                                        {
                                            if (batchAux.BatchNumber.Trim() == batchName)
                                            {
                                                batchNumber = batchAux.BatchNumber.Trim();
                                                batchExist = true;
                                                //currentStatus = batchAux.StatusFlag;
                                                resultBatchesAux = DBTransactions.GetBatchesbyName(logJobName, batchAux.BatchNumber.Trim());
                                                //batch = batchAux;
                                                batch = resultBatchesAux.ReturnValue[0];
                                                
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (batchAux.BatchAlias.Trim() == batchName || batchAux.BatchNumber.Trim() == batchName)
                                            {
                                                batchNumber = batchAux.BatchNumber.Trim();
                                                batchExist = true;
                                                resultBatchesAux = DBTransactions.GetBatchesbyName(logJobName, batchAux.BatchNumber.Trim());
                                                //currentStatus = batchAux.StatusFlag;
                                                //batch = batchAux;
                                                batch = resultBatchesAux.ReturnValue[0];
                                                //nlogger.Info("            Documents.xml Metadata File found.");
                                                break;
                                            }
                                        }
                                    }
                                    if (!batchExist)
                                    {
                                        if (!batchName.Contains(".REMOVE"))
                                        {
                                            nlogger.Info("            Batch " + batchName + " does not exist in the Database. This batch will be deleted.");
                                            Console.Out.WriteLineAsync("            Batch " + batchName + " does not exist in the Database. This batch will be deleted.");
                                            nlogger.Info("            Removing " + batchName + " folder.");
                                            Console.Out.WriteLineAsync("            Removing " + batchName + " folder.");
                                            Directory.Delete(folder, true);
                                        }
                                        else
                                        {
                                            nlogger.Info("            Removing " + batchName + " folder.");
                                            Console.Out.WriteLineAsync("            Removing " + batchName + " folder.");
                                            Directory.Delete(folder, true);                                               
                                        }
                                        
                                    }
                                    else
                                    {
                                        nlogger.Trace("           Batch Found in Database: " + batch.BatchNumber);
                                        nlogger.Trace("           Batch Status: " + batch.StatusFlag);
                                        nlogger.Trace("           Batch Page Size Count Information: " + batch.PageSizesCount);
                                        // Only batches in one of the following stages can be sent for indexing:
                                        // - Waiting for Approval
                                        // - Waiting for PDF Conversion
                                        // - Waiting for Output
                                        // - Validation Approved (not sure at the moment)
                                        // - Validation Completed (not sure at the moment)

                                        if (batch.StatusFlag == "Approved" || batch.StatusFlag == "Waiting for Approval" || 
                                            batch.StatusFlag == "Waiting for PDF Conversion" || batch.StatusFlag == "Waiting for Output")
                                        {
                                            XmlDocument xml = new XmlDocument();
                                            //xml.Load(folder + "\\Documents.MOVING");
                                            xml.Load(folder + "\\Documents.xml");
                                            XmlNodeList documents = xml.SelectNodes("//document");
                                            batchDocumentCount = documents.Count;
                                            Console.Out.WriteLineAsync("            Number of Documents recorded in the XML file: " + documents.Count.ToString());
                                            nlogger.Trace("            Number of Documents  recorded in the XML file: " + documents.Count.ToString());

                                            if (job.OutputFileType.Trim() == "PDF" || job.OutputFileType.Trim() == "Searchable PDF")
                                            {
                                                outputFileExtension = "pdf";
                                            }
                                            else
                                            {
                                                outputFileExtension = "tif";
                                            }

                                            // 5. Get number of files and check if match expected number of documents is correct
                                            string[] files = System.IO.Directory.GetFiles(folder, "*." + outputFileExtension, System.IO.SearchOption.AllDirectories);
                                            Console.Out.WriteLineAsync("           Number of Documents found in Batch Folder: " + files.Count());
                                            nlogger.Trace("           Number of Documents found in Batch Folder: " + files.Count());
                                            if (files.Count() != batchDocumentCount)
                                            {
                                                nlogger.Info("           Number of documents expected with " + outputFileExtension +  " extension, does not match with the existing number of documents in the Batch directory.");
                                                Console.Out.WriteLineAsync("           Number of documents expected with " + outputFileExtension + " extension ,does not match with the existing number of documents in the Batch directory.");
                                                //nlogger.Info("            Renaming  " + batchName + " folder to " + batchName + ".MISSMATCH");
                                                //Console.Out.WriteLineAsync("            Renaming  " + batchName + " folder to " + batchName + ".MISSMATCH");
                                                //Directory.Move(folder,folder + ".MISSMATCH");
                                            }
                                            else
                                            {
                                                nlogger.Info("            Number of documents expected with " + outputFileExtension + " extension ,matches with the existing number of documents in the Batch directory.");
                                                Console.Out.WriteLineAsync("            Number of documents expected with " + outputFileExtension + " extension ,matches with the existing number of documents in the Batch directory.");

                                                // 3. Rename Documents.xml file to prevent any other process to open it 
                                                nlogger.Info("            Renaming Documents.xml to Documents.MOVING in Batch folder location.");
                                                Console.Out.WriteLineAsync("            Renaming Documents.xml to Documents.MOVING in Batch folder location.");
                                                File.Move(folder + "\\Documents.XML", folder + "\\Documents.MOVING");

                                                // Verify that the file was renamed
                                                if (!File.Exists(folder + "\\Documents.MOVING"))
                                                {
                                                    // Not sure if this section will be reach in case of failure
                                                    nlogger.Info("            Failed to rename Documents.XML to Documents.MOVING in Batch folder location. Batch will be ignored.");
                                                    Console.Out.WriteLineAsync("            Failed to rename Documents.XML to Documents.MOVING in Batch folder location. Batch will be ignored.");
                                                    return;
                                                }

                                                // 6. Create base directory in Batch Resting Location
                                                string restingLocation = "";
                                                restingLocation = job.RestingLocation + "\\" + job.JobName.Trim() + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString() + "\\" + batchName;
                                                if (!Directory.Exists(restingLocation))
                                                {
                                                    Directory.CreateDirectory(restingLocation);
                                                    nlogger.Info("            Copying Batch to resting location: " + restingLocation);
                                                    Console.Out.WriteLineAsync("            Copying Batch to resting location: " + restingLocation);
                                                }
                                                else
                                                {
                                                    // Consider the envent in which the Batch Folder exist in the resting location and all we are doing is replacing Files
                                                    if (File.Exists(restingLocation + "\\Documents.xml"))
                                                    {
                                                        File.Delete(restingLocation + "\\Documents.xml");
                                                    }
                                                    if (File.Exists(restingLocation + "\\Documents.MOVING"))
                                                    {
                                                        File.Delete(restingLocation + "\\Documents.MOVING");
                                                    }
                                                    nlogger.Info("            The Batch already exist in the resting location.");
                                                    Console.Out.WriteLineAsync("            The Batch already exist in the resting location.");
                                                    nlogger.Info("            Replacing Batch in resting location: " + restingLocation);
                                                    Console.Out.WriteLineAsync("            Replacing Batch in resting location: " + restingLocation);
                                                }

                                                // Move Batch folder to Resting Location
                                                //Directory.Move(@folder, @restingLocation + "\\" + batchName);
                                                //Alternative way of performing a copy Operation recursevly ... CopyFolder(@folder , @restingLocation );  

                                                CopyFolder(@folder, @restingLocation, "*." + outputFileExtension, job.JobName.Trim());
                                                //CopyFolder(@folder, @restingLocation, "*.*", job.JobName.Trim());
                                                File.Copy(folder + "\\Documents.MOVING", restingLocation + "\\Documents.MOVING",true);
                                                nlogger.Info("            Batch Folder was successfully transfered to the resting location.");
                                                Console.Out.WriteLineAsync("            Batch Folder was successfully transfered to the resting location.");

                                                // Verify that the Documents.MOVING was successfully copied.
                                                if (!File.Exists(restingLocation + "\\Documents.MOVING"))
                                                {
                                                    // Not sure if this section will be reach in case of failure
                                                    nlogger.Info("            Failed to copy Documents.MOVING to resting location. Batch will be ignored.");
                                                    Console.Out.WriteLineAsync("            Failed to copy Documents.MOVING to resting location. Batch will be ignored.");


                                                    if (!File.Exists(folder + "\\Documents.MOVING")) File.Move(folder + "\\Documents.MOVING", folder + "\\Documents.FAILED");
                                                    return;
                                                }

                                                // Verify that the count of files in the resting location is the same as the source location
                                                string[] filesInRestingLocation = System.IO.Directory.GetFiles(restingLocation, "*." + outputFileExtension, System.IO.SearchOption.AllDirectories);
                                                //string[] filesInRestingLocation = System.IO.Directory.GetFiles(restingLocation, "*.*", System.IO.SearchOption.AllDirectories);
                                                if (filesInRestingLocation.Count() != batchDocumentCount)
                                                //if (filesInRestingLocation.Count() != batchDocumentCount +1) // +1 since we are counting for  the Documents.xml
                                                {
                                                    nlogger.Info("            Number of documents expected with " + outputFileExtension + " extension in the resting location ,does not match with the existing number of documents in the Batch directory. Batch will be ignored.");
                                                    Console.Out.WriteLineAsync("            Number of documents expected with " + outputFileExtension + " extension in the resting location ,does not match with the existing number of documents in the Batch directory. Batch will be ignored.");

                                                    if (!File.Exists(folder + "\\Documents.MOVING")) File.Move(folder + "\\Documents.MOVING", folder + "\\Documents.FAILED");
                                                    return;
                                                }

                                                nlogger.Info("            Deleting source Directory: " + @folder);
                                                Console.Out.WriteLineAsync("            Deleting source Directory: " + folder);
                                                Directory.Delete(@folder, true);
                                                // Verify that the Source directory was deleted
                                                if (Directory.Exists(@folder))
                                                {
                                                    // Not sure if this section will be reach in case of failure
                                                    nlogger.Info("            Could not removed the Batch Folder location: " + @folder);
                                                    Console.Out.WriteLineAsync("            Could not removed the Batch Folder location: " + @folder);

                                                    if (!File.Exists(folder + "\\Documents.MOVING")) File.Move(folder + "\\Documents.MOVING", folder + "\\Documents.FAILED");
                                                    return;
                                                }

                                                // Find the Batch Location Attribute value in Document.xml and replace ths value for the new Batch Location in the
                                                // entire Document.xml file
                                                nlogger.Info("            Updating Documents.xml with new Batch Location and File Extension ...");
                                                Console.Out.WriteLineAsync("            Updating Documents.xml with new Batch Location and File Extension ...");
                                                XmlDocument finalXml = new XmlDocument();
                                                finalXml.Load(restingLocation + "\\Documents.MOVING");
                                                XmlNodeList finalDocuments = finalXml.SelectNodes("//document");

                                                // Batch location is the original Batch location contains in the XML File
                                                batchLocation = "";
                                                foreach (XmlElement doc in finalDocuments)
                                                {                                                    
                                                    foreach (XmlNode node in doc)
                                                    {
                                                        switch (node.Attributes["name"].Value)
                                                        {
                                                            case "Batch Location":
                                                                batchLocation = node.Attributes["value"].Value;
                                                                break;                                                            
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(batchLocation))
                                                        break;
                                                }
                                                nlogger.Info("            Old Batch Location contained in Documents.xml: " + batchLocation);
                                                Console.Out.WriteLineAsync("             Old Batch Location contained in Documents.xml: " + batchLocation);
                                                nlogger.Info("            New Batch Location : " + restingLocation);
                                                Console.Out.WriteLineAsync("            New Batch Location: " + restingLocation);
                                                foreach (XmlElement doc in finalDocuments)
                                                {
                                                    docCounter = docCounter + 1;
                                                    nlogger.Info("              Updating Document: " + docCounter.ToString() + " / " + batchDocumentCount.ToString());
                                                    foreach (XmlNode node in doc)
                                                    {
                                                        switch (node.Attributes["name"].Value)
                                                        {
                                                            case "Batch Location":
                                                                nlogger.Info("              Batch Location Tag Old Value " + node.Attributes["value"].Value);
                                                                node.Attributes["value"].Value = restingLocation + "\\";
                                                                nlogger.Info("              Batch Location Tag New Value " + node.Attributes["value"].Value);
                                                                break;

                                                            case "Document Filename with Full Path":
                                                                nlogger.Info("              Document File Name Full Path Tag Old Value " + node.Attributes["value"].Value);
                                                                filePath = Path.GetDirectoryName(node.Attributes["value"].Value) + "\\";
                                                                nlogger.Info("                    File Path: " + filePath);
                                                                fileName = Path.GetFileNameWithoutExtension(node.Attributes["value"].Value) + "." + outputFileExtension;
                                                                nlogger.Info("                    File Name: " + fileName);
                                                                nlogger.Info("                    Replacing :" + batchLocation);
                                                                nlogger.Info("                    By :" + restingLocation + "\\");
                                                                node.Attributes["value"].Value = filePath.Replace(batchLocation, restingLocation + "\\") + "\\" + fileName;
                                                                nlogger.Info("              Document File Name Full Path Tag New Value " + node.Attributes["value"].Value);
                                                                break;

                                                            case "Document Filename":
                                                                nlogger.Info("              Document Filename Tag Old Value " + node.Attributes["value"].Value);
                                                                fileName = Path.GetFileNameWithoutExtension(node.Attributes["value"].Value) + "." + outputFileExtension;
                                                                nlogger.Info("                    File Name: " + fileName);
                                                                node.Attributes["value"].Value = fileName;
                                                                nlogger.Info("              Document Filename Tag New Value " + node.Attributes["value"].Value);
                                                                break;

                                                            case "Document location":
                                                                nlogger.Info("              Document Location Tag Old Value " + node.Attributes["value"].Value);
                                                                filePath = Path.GetDirectoryName(node.Attributes["value"].Value) + "\\";
                                                                nlogger.Info("                    File Path: " + filePath);
                                                                nlogger.Info("                    Replacing :" + batchLocation);
                                                                nlogger.Info("                    By :" + restingLocation + "\\");
                                                                string documentLocation = node.Attributes["value"].Value;
                                                                node.Attributes["value"].Value = filePath.Replace(batchLocation, restingLocation + "\\");
                                                                nlogger.Info("              Document Location Tag New Value " + node.Attributes["value"].Value);
                                                                break;
                                                        }
                                                    }
                                                }
                                                finalXml.Save(restingLocation + "\\Documents.MOVING");

                                                nlogger.Info("            Renaming Documents.MOVING back to Documents.xml.");
                                                Console.Out.WriteLineAsync("            Renaming Documents.MOVING back to Documents.xml.");
                                                File.Move(restingLocation + "\\Documents.MOVING", restingLocation + "\\Documents.xml");
                                                // Verify that the Documents.MOVING was successfully renamed.
                                                if (!File.Exists(restingLocation + "\\Documents.xml"))
                                                {
                                                    nlogger.Info("            Failed to rename Documents.MOVING to Documents.xml in the resting location. Batch will be ignored.");
                                                    Console.Out.WriteLineAsync("            Failed to rename Documents.MOVING to Documents.xml in the resting location. Batch will be ignored.");
                                                    return;
                                                }

                                                // Check if Batch Backup exist

                                                if (job.FileConversionEnableFlag)
                                                {
                                                    nlogger.Info("            Checking if Batch Backup Exist.");
                                                    Console.Out.WriteLineAsync("            Checking if Batch Backup Exist.");
                                                    if (Directory.Exists(job.BackupFolder + "\\" + job.JobName.Trim()))
                                                    {
                                                        nlogger.Info("            Backup Directory Found. Directroy will be removed.");
                                                        Console.Out.WriteLineAsync("            Backup Directory Found. Directroy will be removed.");
                                                        Directory.Delete(job.BackupFolder + "\\" + job.JobName.Trim(), true);
                                                    }
                                                    else
                                                    {
                                                        nlogger.Info("            Backup Directory not Found.");
                                                        Console.Out.WriteLineAsync("            Backup Directory not Found.");
                                                    }
                                                }
                                                else
                                                {
                                                    nlogger.Info("            There is not Backup directory configured for this Batch.");
                                                    Console.Out.WriteLineAsync("            There is not Backup directory configured for this Batch.");
                                                }

                                                // Update Batch Status and Batch Resting Location in Batch Control Table
                                                nlogger.Info("            Updating Batch Information in the Database...");
                                                Console.Out.WriteLineAsync("            Updating Batch Information in the Database...");
                                                batch.DocumentPath = restingLocation;
                                                currentBatchStatus = batch.StatusFlag;
                                                batch.StatusFlag = "Waiting for Indexing";
                                                result = DBTransactions.BatchUpdate(logJobName, batch);
                                                if (result.ReturnCode != 0)
                                                {
                                                    nlogger.Info("          " + result.Message);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Out.WriteLineAsync("          " + result.Message);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    return;
                                                }

                                                // register Transaction in Database Transaction Table
                                                nlogger.Info("            Adding Batch Transaction Event to the Database. Batch Number: " + batch.BatchNumber);
                                                Console.Out.WriteLineAsync("            Adding Batch Transaction Event to the Database. Batch Number: " + batch.BatchNumber);
                                                result = DBTransactions.BatchTrackingEvent(logJobName, batch.BatchNumber, currentBatchStatus);
                                                if (result.ReturnCode != 0)
                                                {
                                                    nlogger.Info("          " + result.Message);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Out.WriteLineAsync("          " + result.Message);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                    return;
                                                }
                                                //}
                                            }
                                        }
                                        else
                                        {
                                            nlogger.Info("            Batch with the status " + batch.StatusFlag + " is not allowed to be set for Indexing.");
                                            nlogger.Info("            Only Batches in Waiting for PDF conversion, Waiting for Output, Waiting for Approval or Approved are accepted.");
                                            Console.Out.WriteLineAsync("            Batch with the status " + batch.StatusFlag + " is not allowed to be set for Indexing.");
                                            Console.Out.WriteLineAsync("            Only Batches in Waiting for PDF conversion, Waiting for Output, Waiting for Approval or Approved are accepted.");
                                        }                                        
                                    }
                                }
                                else
                                {
                                    // Report the issue found with this Batch
                                    if (File.Exists(folder + "\\Documents.MOVING"))
                                    {
                                        nlogger.Info("            Documents.MOVING file was found, so the Batch Delivery Process failed or was interrupted in a previous execution. Batch Name " + batchName + " will be ignored.");
                                        Console.Out.WriteLineAsync("            Documents.MOVING file was found, so the Batch Delivery Process failed or was interrupted in a previous execution. Batch Name " + batchName + " will be ignored.");
                                    }
                                    else
                                    {
                                        nlogger.Info("            Documents.xml file was not found. Batch Name " + batchName + " will be ignored.");
                                        Console.Out.WriteLineAsync("            Documents.xml file was not found. Batch Name " + batchName + " will be ignored.");
                                    }
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
        }

        public static void AnalizeJob(string logJobName, JobExtended job)
        {
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    ResultGeneric result = new ResultGeneric();
                    string jobBatchDeliveryDirectory = "";

                    nlogger.Info("  Processing Job:  " + job.JobName.Trim());
                    Console.Out.WriteLineAsync("    Processing Job:  " + job.JobName.Trim());

                    // Build the full Job Type directory ...
                    jobBatchDeliveryDirectory = job.BatchDeliveryWatchFolder.Trim() + "\\" + job.JobName.Trim();
                    nlogger.Info("      Traversing directory: " + jobBatchDeliveryDirectory);
                    Console.Out.WriteLineAsync("      Traversing directory: " + jobBatchDeliveryDirectory);

                    // Get Existing Batches for this Job Type in SSS
                    ResultBatches resulBatches = new ResultBatches();
                    resulBatches = DBTransactions.GetBatchesbyJobType(logJobName, job.JobName.Trim());

                    if (resulBatches.ReturnCode != 0) return;

                    

                    // When File conversion is configured, the service must browse all PDF Target directories
                    // When thre is not file conversion, then we need to browse the Batch Delivery Watch Folder  
                    if (!job.FileConversionEnableFlag)
                    {
                        // Get the top level directories
                        if (!Directory.Exists(jobBatchDeliveryDirectory))
                        {
                            nlogger.Info("      Directory does not exist: " + jobBatchDeliveryDirectory);
                            nlogger.Info("      Skipping Job Type: " + job.JobName.Trim());
                            Console.Out.WriteLineAsync("      Directory does not exist: " + jobBatchDeliveryDirectory);
                            Console.Out.WriteLineAsync("      Skipping Job Type: " + job.JobName.Trim());
                            return;
                        }

                        // Process Jobs Batch Delivery Directory
                        DeliveryProcess(logJobName, job, jobBatchDeliveryDirectory, resulBatches);
                    }
                    else
                    {
                        // Perform Batch Deliverty operation process for each PDF Station Target Folder 
                        ResultServiceStationsExtended resulServiceStations = new ResultServiceStationsExtended();
                        resulServiceStations = DBTransactions.GetServiceStations();

                        if (resulServiceStations.ReturnCode != 0) return;

                        foreach (ServiceStationExtended serviceStation in resulServiceStations.ReturnValue)
                        {
                            // Verify if this is a PDF Station
                            if (serviceStation.PDFStationFlag)
                            {
                                if (!string.IsNullOrEmpty(serviceStation.TargetFolder.Trim()))
                                {
                                    // We do this only for PDF Stationss that are enabled
                                    if (!string.IsNullOrEmpty(serviceStation.TargetFolder.Trim()))
                                        nlogger.Info("      Checking PDF Service Station: " + serviceStation.StationName);
                                    Console.Out.WriteLineAsync("      Checking PDF Service Station: " + serviceStation.StationName);
                                    DeliveryProcess(logJobName, job, serviceStation.TargetFolder.Trim() + "\\" + job.JobName.Trim(), resulBatches);
                                }
                            }
                        }
                    }
                    nlogger.Info("  End Processing Job:  " + job.JobName.Trim());
                    Console.Out.WriteLineAsync("    End Processing Job:  " + job.JobName.Trim());

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

        public static void BatchDeliveryProcess(string logJobName, int jobID)
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
                        localCronJobsList = BatchDeliveryService.cronJobsList;
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
            BatchDeliveryService service1 = new BatchDeliveryService(args);
            service1.Startup(args);
            do
            {
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);            

#else
            ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new  BatchDeliveryService(args)
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
                resultProcess = DBTransactions.GetProcessByName("Batch Delivery");
                if (resultProcess.ReturnCode == 0)
                {
                    // Check list of processes
                    foreach (ScanningServicesDataObjects.GlobalVars.Process process in resultProcess.ReturnValue)
                    {
                        // When Job ID is = 0 means that rule applies for all Jobs
                        if (process.JobID == 0)
                            process.JobName = "ALL";

                        // Process only recordsa that match Host Name
                        if (BatchDeliveryService.hostName == process.StationName.Trim())
                        {
                            // 1. Check if process exist in the cronJob List anb if the cron exist, verify is the schedule has changed
                            //    This loop check if processes in the Database are already registerd in the Process List
                            cronJobFound = false;
                            cronJobChanged = false;
                            foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in BatchDeliveryService.cronJobsList)
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
                                        BatchDeliveryService.cronJobsList.Add(jobProcess);

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
                                        foreach (ScanningServicesDataObjects.GlobalVars.Process cronJob in BatchDeliveryService.cronJobsList)
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
                                                    BatchDeliveryService.cronJobsList.Remove(cronJob);
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

                    BatchDelivery.Program.BatchDeliveryProcess(logJobName, Convert.ToInt32(context.JobDetail.Key.Name.Replace("JOB-", "")));

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
