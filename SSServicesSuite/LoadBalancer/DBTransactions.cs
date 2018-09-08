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

namespace LoadBalancer
{
    class DBTransactions
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        static object lockObj = new object();
        
        
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
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
                    nlogger.Fatal("URL: " + URL);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.WriteLine("URL: " + URL);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return false;
            }
        }

        /// <summary>
        /// Get Jobs
        /// </summary>
        /// <returns></returns>
        public static ResultJobsExtended GetJobs()
        {
            ResultJobsExtended resultJobs = new ResultJobsExtended();
            resultJobs.ReturnCode = 0;
            resultJobs.Message = "";
            resultJobs.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";                    

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = LoadBalancerService.BaseURL + "Jobs/GetJobs";

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
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
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
        /// Get PDF Stations
        /// </summary>
        /// <returns></returns>
        public static ResultServiceStationsExtended GetServiceStations()
        {
            ResultServiceStationsExtended resultServriceStations = new ResultServiceStationsExtended();
            resultServriceStations.ReturnCode = 0;
            resultServriceStations.Message = "";
            resultServriceStations.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = LoadBalancerService.BaseURL + "GeneralSettings/GetServiceStations";

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
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
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
        public static ResultJobsExtended GetJobByID(int jobID)
        {
            ResultJobsExtended resultJobs = new ResultJobsExtended();
            resultJobs.ReturnCode = 0;
            resultJobs.Message = "";
            resultJobs.RecordsCount = 0;
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = LoadBalancerService.BaseURL + "Jobs/GetJobByID?jobID=" + jobID.ToString();

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
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
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
        public static ResultProcesses GetProcessByName(string processName)
        {
            ResultProcesses resultProcess = new ResultProcesses();
            resultProcess.ReturnCode = 0;
            resultProcess.Message = "";
            resultProcess.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = LoadBalancerService.BaseURL + "Process/GetProcessesByName?processName=" + processName;

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
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
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
        /// Get Batches
        /// </summary>
        /// <returns></returns>
        public static ResultBatches GetBatches(string logJobName, string batchNumber)
        {
            ResultBatches resultBatches = new ResultBatches();
            resultBatches.ReturnCode = 0;
            resultBatches.Message = "";
            resultBatches.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = LoadBalancerService.BaseURL + "Batches/GetBatchesInformation?filter=" + "BatchNumber=\"" + batchNumber + "\" OR BatchAlias=\"" + batchNumber + "\"";

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
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
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
                    resultBatches.ReturnCode = -2;
                    resultBatches.Message = ex.Message;
                }
            }
            return resultBatches;
        }


        /// <summary>
        /// Get Process by a given Name
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static ResultGeneric GetNextAvailableFileConversionStation(string logJobName)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = LoadBalancerService.BaseURL + "Process/GetNextAvailableFileConversionStation";
                    nlogger.Trace("     API Call: " + URL);

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        result = JsonConvert.DeserializeObject<ResultGeneric>(returnMessage);
                        nlogger.Trace("     API Call successfully executed. Number of reords found: " + result.RecordsCount.ToString());
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = "General";
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.ReturnCode = -2;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        public static ResultGeneric BatchRegistration(string logJobName, string batchNumber, string batchAlias, JobExtended job)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    Batch batch = new Batch();

                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string bodyString = "";
                    string returnMessage = "";
                    string batchJS = "";

                    batch.BatchNumber = batchNumber;
                    batch.StatusFlag = "Ready to Scan";
                    batch.SubmittedBy = "Auto Import Service";
                    batch.SubmittedDate = DateTime.Now;
                    batch.Customer = job.CustomerName;
                    batch.ProjectName = job.ProjectName;
                    batch.JobType = job.JobName;
                    batch.DepName = job.DepartmentName;
                    batch.BatchAlias = batchAlias;

                    batchJS = JsonConvert.SerializeObject(batch, Newtonsoft.Json.Formatting.Indented);
                    batchJS = batchJS.Replace(@"\", "\\\\");

                    URL = LoadBalancerService.BaseURL + "Batches/BatchRegistration";

                    bodyString = "'" + batchJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
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
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

        public static ResultGeneric BatchTrackingEvent(string logJobName, string batchNumber, string currentStatus, string newStatus, string eventDescription)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;
            try
            {
                lock (lockObj)
                {
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    BatchTracking batchTracking = new BatchTracking();

                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string bodyString = "";
                    string returnMessage = "";
                    string batchTrackingJS = "";

                    batchTracking.BatchNumber = batchNumber;
                    batchTracking.InitialStatus = currentStatus;
                    batchTracking.FinalStatus = newStatus;
                    batchTracking.Event = eventDescription;
                    batchTracking.OperatorName = "Load Balancer Service";
                    batchTracking.StationName = Dns.GetHostName();
                    batchTrackingJS = JsonConvert.SerializeObject(batchTracking, Newtonsoft.Json.Formatting.Indented);
                    batchTrackingJS = batchTrackingJS.Replace(@"\", "\\\\");

                    URL = LoadBalancerService.BaseURL + "Batches/NewBatchEvent";

                    bodyString = "'" + batchTrackingJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
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
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }

            return result;
        }


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
                    LogManager.Configuration.Variables["JobName"] = logJobName;

                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string URL = "";
                    string bodyString = "";
                    string returnMessage = "";
                    string batchJS = "";
                    
                    batchJS = JsonConvert.SerializeObject(batch, Newtonsoft.Json.Formatting.Indented);
                    batchJS = batchJS.Replace(@"\", "\\\\");

                    URL = LoadBalancerService.BaseURL + "Batches/UpdateBatchInformation";

                    bodyString = "'" + batchJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
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
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
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
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string urlParameters = "";
                    string URL = "";
                    HttpResponseMessage response = new HttpResponseMessage();
                    string returnMessage;

                    URL = LoadBalancerService.BaseURL + "Batches/GetBatchesInformation?filter=" + "JobType = \"" + jobType + "\"";

                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
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
                    LogManager.Configuration.Variables["JobName"] = logJobName;
                    nlogger.Fatal(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(General.ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                    resultBatches.Message = General.ErrorMessage(ex);
                    resultBatches.ReturnCode = -2;
                }
            }
            return resultBatches;
        }
    }
}
