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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace BatchRemover
{
    class DBTransactions
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        static object lockObj = new object();

#region // Other DB Transactions       
        /// <summary>
        /// Check for availability of the Services API
        /// This Methis helo to prevent he service from stoping if connection to the web services APis is lost
        /// Use this validation routine before the API call is made
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean IsUrlAvailable(string URL)
        {
            try
            {
                Uri uri = new Uri(URL);
                WebRequest request = WebRequest.Create(uri);
                request.Timeout = 300000;
                WebResponse responseService;
                responseService = request.GetResponse();
                responseService.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), "General");
                    General.Logger(General.LogLevel.Error, "URL: " + URL, "General");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.WriteLine("URL: " + URL);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return false;
            }
        }
#endregion // Other DB Transactions  

 #region // Gets DB Transactions

        /// <summary>
        /// Get Jobs
        /// </summary>
        /// <returns></returns>
        public static ResultJobsExtended GetJobs(string logJobName)
        {
            ResultJobsExtended resultJobs = new ResultJobsExtended();
            resultJobs.ReturnCode = 0;
            resultJobs.Message = "";
            resultJobs.RecordsCount = 0;

            try
            {
                lock (lockObj)                {
                    
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";                    

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "Jobs/GetJobs";

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultJobs = JsonConvert.DeserializeObject<ResultJobsExtended>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {                       
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultJobs.ReturnCode = -2;
                    resultJobs.Message = ex.Message;
                }
            }
            return resultJobs;
        }
        
        /// <summary>
        /// Get Page Sizes definied for a given Job ID
        /// </summary>
        /// <returns></returns>
        public static ResultJobPageSizes GetPageSizesByJobID(string logJobName, int jobID)
        {
            ResultJobPageSizes resultJobPageSizes = new ResultJobPageSizes();
            resultJobPageSizes.ReturnCode = 0;
            resultJobPageSizes.Message = "";
            resultJobPageSizes.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "Jobs/GetPageSizesByJobID?jobID=" + jobID.ToString();

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultJobPageSizes = JsonConvert.DeserializeObject<ResultJobPageSizes>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultJobPageSizes.ReturnCode = -2;
                    resultJobPageSizes.Message = ex.Message;
                }
            }
            return resultJobPageSizes;
        }

        /// <summary>
        /// Get Batches
        /// </summary>
        /// <returns></returns>
        public static ResultBatches GetBatches(string logJobName, string jobType, string statusFlag)
        {
            ResultBatches resultBatches = new ResultBatches();
            resultBatches.ReturnCode = 0;
            resultBatches.Message = "";
            resultBatches.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "Batches/GetBatchesInformation?filter=" + "StatusFlag = \"" + statusFlag + "\" AND JobType = \"" + jobType + "\""; 

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultBatches = JsonConvert.DeserializeObject<ResultBatches>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultBatches.ReturnCode = -2;
                    resultBatches.Message = ex.Message;
                }
            }
            return resultBatches;
        }


        /// <summary>
        /// Get Batch Document Information
        /// </summary>
        /// <returns></returns>
        public static ResultBatchDocs GetDocumentInformation(string logJobName, string batchName)
        {
            ResultBatchDocs resultDocs = new ResultBatchDocs();
            resultDocs.ReturnCode = 0;
            resultDocs.Message = "";
            resultDocs.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "Batches/GetBatchDocsInformation?filter=" + "BatchName = \"" + batchName + "\"";

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultDocs = JsonConvert.DeserializeObject<ResultBatchDocs>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultDocs.ReturnCode = -2;
                    resultDocs.Message = ex.Message;
                }
            }
            return resultDocs;
        }
        /// <summary>
        /// Get PDF Stations
        /// </summary>
        /// <returns></returns>
        public static ResultServiceStationsExtended GetServiceStations(string logJobName)
        {
            ResultServiceStationsExtended resultServriceStations = new ResultServiceStationsExtended();
            resultServriceStations.ReturnCode = 0;
            resultServriceStations.Message = "";
            resultServriceStations.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "GeneralSettings/GetServiceStations";

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultServriceStations = JsonConvert.DeserializeObject<ResultServiceStationsExtended>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultServriceStations.ReturnCode = -2;
                    resultServriceStations.Message = ex.Message;
                }
            }
            return resultServriceStations;
        }

        /// <summary>
        /// Get Job by a given ID
        /// </summary>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public static ResultJobsExtended GetJobByID(string logJobName,int jobID)
        {
            ResultJobsExtended resultJobs = new ResultJobsExtended();
            resultJobs.ReturnCode = 0;
            resultJobs.Message = "";
            resultJobs.RecordsCount = 0;
            try
            {
                lock (lockObj)
                {
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "Jobs/GetJobByID?jobID=" + jobID.ToString();

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultJobs = JsonConvert.DeserializeObject<ResultJobsExtended>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultJobs.ReturnCode = -2;
                    resultJobs.Message = ex.Message;
                }
            }
            return resultJobs;
        }

        /// <summary>
        /// Get Process by a given Name
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static ResultProcesses GetProcessByName(string logJobName, string processName)
        {
            ResultProcesses resultProcess = new ResultProcesses();
            resultProcess.ReturnCode = 0;
            resultProcess.Message = "";
            resultProcess.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "Process/GetProcessesByName?processName=" + processName;

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultProcess = JsonConvert.DeserializeObject<ResultProcesses>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultProcess.ReturnCode = -2;
                    resultProcess.Message = ex.Message;
                }
            }
            return resultProcess;
        }

        /// <summary>
        /// Get associates fields to a given Ob
        /// </summary>
        /// <param name="logJobName"></param>
        /// <param name="jobID"></param>
        /// <returns></returns>
        public static ResultFields GetFieldsByJobID(string logJobName, int jobID)
        {
            ResultFields resultFields = new ResultFields();
            resultFields.ReturnCode = 0;
            resultFields.Message = "";
            resultFields.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BatchRemoverService.BaseURL + "Fields/GetFieldsByJobID?jobID=" + jobID;

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultFields = JsonConvert.DeserializeObject<ResultFields>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultFields.ReturnCode = -2;
                    resultFields.Message = ex.Message;
                }
            }
            return resultFields;
        }

        public static ResultBatches GetBatchesbyJobType(string logJobName, string jobType)
        {
            ResultBatches resultBatches = new ResultBatches();
            resultBatches.ReturnCode = 0;
            resultBatches.Message = "";
            resultBatches.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string urlParameters = "";
                    string URL = "";
                    HttpResponseMessage response = new HttpResponseMessage();
                    string returnMessage;

                    URL = BatchRemoverService.BaseURL + "Batches/GetBatchesInformation?filter=" + "JobType = \"" + jobType + "\"";

                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        resultBatches.Message = "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage;
                        resultBatches.ReturnCode = -1;
                    }
                    else
                    {
                        using (HttpContent content = response.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            resultBatches = JsonConvert.DeserializeObject<ResultBatches>(returnMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultBatches.Message = General.ErrorMessage(ex);
                    resultBatches.ReturnCode = -2;
                }
            }
            return resultBatches;
        }



        public static ResultGeneric BatchTrackingEvent(string logJobName, string batchNumber, string currentStatus)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;
            try
            {
                lock (lockObj)
                {
                    BatchTracking batchTracking = new BatchTracking();

                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string bodyString = "";
                    string returnMessage = "";
                    string batchTrackingJS = "";

                    batchTracking.BatchNumber = batchNumber;
                    batchTracking.InitialStatus = currentStatus;
                    batchTracking.FinalStatus = "Deleted";
                    batchTracking.Event = "Batch Removed";
                    batchTracking.OperatorName = "BatchRemover Service";
                    batchTracking.StationName = Dns.GetHostName();
                    batchTrackingJS = JsonConvert.SerializeObject(batchTracking, Newtonsoft.Json.Formatting.Indented);
                    batchTrackingJS = batchTrackingJS.Replace(@"\", "\\\\");

                    URL = BatchRemoverService.BaseURL + "Batches/NewBatchEvent";

                    bodyString = "'" + batchTrackingJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        result.Message = "Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage;
                        result.ReturnCode = -1;
                    }
                    else
                    {
                        using (HttpContent content = response_for_new.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            result = JsonConvert.DeserializeObject<ResultGeneric>(returnMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }

            return result;
        }

        #endregion // Gets

#region // New DB Transactions
        /// <summary>
        /// Add a new Document for a given Batch
        /// </summary>
        /// <param name="logJobName"></param>
        /// <param name="batchNumber"></param>
        /// <param name="batchDoc"></param>
        /// <returns></returns>
        public static ResultGeneric NewBatchDocument(string logJobName, string batchNumber, BatchDocs batchDoc)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;
            try
            {
                lock (lockObj)
                {
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string bodyString = "";
                    string returnMessage = "";
                    string batcDocJS = "";

                    // StringEscapeHandling help to scape Single Quote in Jason string Format
                    batcDocJS = JsonConvert.SerializeObject(batchDoc, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
                                                            { StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml }); 
                    batcDocJS = batcDocJS.Replace(@"\", "\\\\");

                    URL = BatchRemoverService.BaseURL + "Batches/NewBatchDocument";
                    bodyString = "'" + batcDocJS + "'";
                    
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        result.Message = "Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage;
                        result.ReturnCode = -1;
                    }
                    else
                    {
                        using (HttpContent content = response_for_new.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            result = JsonConvert.DeserializeObject<ResultGeneric>(returnMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }

            return result;
        }

        #endregion // New DB Transactions

 #region // Update DB Transactions
        public static ResultGeneric BatchUpdate(string logJobName,Batch batch)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string bodyString = "";
                    string returnMessage = "";
                    string batchJS = "";
                    
                    batchJS = JsonConvert.SerializeObject(batch, Newtonsoft.Json.Formatting.Indented);
                    batchJS = batchJS.Replace(@"\", "\\\\");

                    URL = BatchRemoverService.BaseURL + "Batches/UpdateBatchInformation";

                    bodyString = "'" + batchJS + "'";
                    HttpContent body_for_update = new StringContent(bodyString);
                    body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                    if (!response_for_update.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response_for_update.ReasonPhrase + "\r\n" + response_for_update.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response_for_update.ReasonPhrase + "\r\n" + response_for_update.RequestMessage);
                        result.Message = "Error:" + "\r\n" + response_for_update.ReasonPhrase + "\r\n" + response_for_update.RequestMessage;
                        result.ReturnCode = -1;
                    }
                    else
                    {
                        using (HttpContent content = response_for_update.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            result = JsonConvert.DeserializeObject<ResultGeneric>(returnMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }
#endregion // Update DB Transactions

#region // Delete DB Transactions
        public static ResultGeneric DeleteBatchDocuments(string logJobName, string batchName)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string urlParameters = "";

                    URL = BatchRemoverService.BaseURL + "Batches/DeleteBatchDocuments";
                    urlParameters = "?batchName=" + batchName;
                    client.BaseAddress = new Uri(URL);
                    response = client.DeleteAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        result.Message = "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage;
                        result.ReturnCode = -1;
                    }
                 }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

        /// <summary>
        /// Remove a given Batch from the Database
        /// </summary>
        /// <param name="logJobName"></param>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public static ResultGeneric DeleteBatch(string logJobName, string batchName)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string urlParameters = "";

                    URL = BatchRemoverService.BaseURL + "Batches/DeleteBatch";
                    urlParameters = "?batchName=" + batchName;
                    client.BaseAddress = new Uri(URL);
                    response = client.DeleteAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        General.Logger(General.LogLevel.Error, "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, logJobName);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        result.Message = "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage;
                        result.ReturnCode = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Error, General.ErrorMessage(ex), logJobName);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }
        #endregion // Delete DB Transactions

    }
}
