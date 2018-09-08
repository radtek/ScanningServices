using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using static ScanningServicesDataObjects.GlobalVars;
//using ScanningServicesAdmin.Data.GlovalVariables
using static ScanningServicesAdmin.Data.GlovalVariables;
using System.Windows.Forms;

namespace ScanningServicesAdmin
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

                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BaseURL + "Jobs/GetJobs";

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
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...", 
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultJobs.ReturnCode = -2;
                    resultJobs.Message = ex.Message;
                }
            }
            return resultJobs;
        }


        /// <summary>
        /// Get Reports
        /// </summary>
        /// <returns></returns>
        public static ResultReports GetReports()
        {
            ResultReports resulReports = new ResultReports();
            resulReports.ReturnCode = 0;
            resulReports.Message = "";
            resulReports.RecordsCount = 0;

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
                    URL = BaseURL + "Reports/GetReports";

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resulReports = JsonConvert.DeserializeObject<ResultReports>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resulReports.ReturnCode = -2;
                    resulReports.Message = ex.Message;
                }
            }
            return resulReports;
        }

        /// <summary>
        /// Get Job Informtion by Job Type Name
        /// </summary>
        /// <returns></returns>
        public static ResultJobsExtended GetJobByName(string jobName)
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
                    URL = BaseURL + "Jobs/GetJobByName?jobName=" + jobName;

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
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {

                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultJobs.ReturnCode = -2;
                    resultJobs.Message = ex.Message;
                }
            }
            return resultJobs;
        }


        /// <summary>
        /// Get Batch Tracking Events for a given Batch
        /// </summary>
        /// <returns></returns>
        public static ResultBatchTrackingExtended GetBatchTrackingEvents(string filter)
        {
            ResultBatchTrackingExtended batchTrackingResult = new ResultBatchTrackingExtended();
            batchTrackingResult.ReturnCode = 0;
            batchTrackingResult.Message = "";
            batchTrackingResult.RecordsCount = 0;

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
                    URL = BaseURL + "Batches/GetBatchTrackingInformation?filter=" + filter ;

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        batchTrackingResult = JsonConvert.DeserializeObject<ResultBatchTrackingExtended>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {

                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    batchTrackingResult.ReturnCode = -2;
                    batchTrackingResult.Message = ex.Message;
                }
            }
            return batchTrackingResult;
        }


        /// <summary>
        /// Get Batch Documents for a given Batch
        /// </summary>
        /// <returns></returns>
        public static ResultBatchDocs GetBatchDocuments(string filter)
        {
            ResultBatchDocs resultBatchDocumnets = new ResultBatchDocs();
            resultBatchDocumnets.ReturnCode = 0;
            resultBatchDocumnets.Message = "";
            resultBatchDocumnets.RecordsCount = 0;

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
                    URL = BaseURL + "Batches/GetBatchDocsInformation?filter=" + filter;

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultBatchDocumnets = JsonConvert.DeserializeObject<ResultBatchDocs>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {

                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultBatchDocumnets.ReturnCode = -2;
                    resultBatchDocumnets.Message = ex.Message;
                }
            }
            return resultBatchDocumnets;
        }

        /// <summary>
        /// Get UI Functionalities 
        /// </summary>
        /// <returns></returns>
        public static ResultUIFunctionalities GetUIFunctionalities()
        {
            ResultUIFunctionalities resulUIFunctionalities = new ResultUIFunctionalities();
            resulUIFunctionalities.ReturnCode = 0;
            resulUIFunctionalities.Message = "";
            resulUIFunctionalities.RecordsCount = 0;

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
                    URL = BaseURL + "Users/GetUIFunctionalities";

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resulUIFunctionalities = JsonConvert.DeserializeObject<ResultUIFunctionalities>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {

                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resulUIFunctionalities.ReturnCode = -2;
                    resulUIFunctionalities.Message = ex.Message;
                }
            }
            return resulUIFunctionalities;
        }

        /// <summary>
        /// Get exiting Customers Informtion 
        /// </summary>
        /// <returns></returns>
        public static ResultCustomers GetCustomers()
        {
            ResultCustomers resulCustomers = new ResultCustomers();
            resulCustomers.ReturnCode = 0;
            resulCustomers.Message = "";
            resulCustomers.RecordsCount = 0;

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
                    URL = BaseURL + "Customers/GetCustomers";

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resulCustomers = JsonConvert.DeserializeObject<ResultCustomers>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {

                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resulCustomers.ReturnCode = -2;
                    resulCustomers.Message = ex.Message;
                }
            }
            return resulCustomers;
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
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BaseURL + "GeneralSettings/GetServiceStations";

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
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BaseURL + "Jobs/GetJobByID?jobID=" + jobID.ToString();

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
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string urlParameters = "";
                    string URL = "";
                    string returnMessage = "";

                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    URL = BaseURL + "Process/GetProcessesByName?processName=" + processName;

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
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultProcess.ReturnCode = -2;
                    resultProcess.Message = ex.Message;
                }
            }
            return resultProcess;
        }

        public static ResultGeneric BatchRegistration(string batchNumber, string batchAlias, JobExtended job)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
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

                    URL = BaseURL + "Batches/BatchRegistration";

                    bodyString = "'" + batchJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

        /// <summary>
        /// Record Batch Status Change in Tracking Datbase Table
        /// </summary>
        /// <param name="batchNumber"></param>
        /// <param name="currentStatus"></param>
        /// <param name="newStatus"></param>
        /// <param name="eventDescription"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ResultGeneric BatchTrackingEvent(string batchNumber, string currentStatus, string newStatus, string eventDescription, string user)
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
                    batchTracking.FinalStatus = newStatus;
                    batchTracking.Event = eventDescription;
                    batchTracking.OperatorName = user;
                    batchTracking.StationName = Dns.GetHostName();
                    batchTrackingJS = JsonConvert.SerializeObject(batchTracking, Newtonsoft.Json.Formatting.Indented);
                    batchTrackingJS = batchTrackingJS.Replace(@"\", "\\\\");

                    URL = BaseURL + "Batches/NewBatchEvent";

                    bodyString = "'" + batchTrackingJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }

            return result;
        }


        /// <summary>
        /// Update Batch Information
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static ResultGeneric BatchUpdate(Batch batch)
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

                    URL = BaseURL + "Batches/UpdateBatchInformation";

                    bodyString = "'" + batchJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

        public static ResultBatches GetBatchesbyJobType(string jobType)
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

                    URL = BaseURL + "Batches/GetBatchesInformation?filter=" + "JobType = \"" + jobType + "\"";

                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultBatches.Message = General.ErrorMessage(ex);
                    resultBatches.ReturnCode = -2;
                }
            }
            return resultBatches;
        }

        public static ResultExportTransactionsJob GetExportTransactionsJob(int jobID, int workOrder, string baseOutputDirectoy)
        {
            ResultExportTransactionsJob result = new ResultExportTransactionsJob();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

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

                    URL = BaseURL + "Export/GetExportTransactionsJob?jobID=" + jobID.ToString() + "&workOrder=" + workOrder.ToString() + " &baseOutputDirectory=" + baseOutputDirectoy;

                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result.Message = "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage;
                        result.ReturnCode = -1;
                    }
                    else
                    {
                        using (HttpContent content = response.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            result = JsonConvert.DeserializeObject<ResultExportTransactionsJob>(returnMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }


        /// <summary>
        /// This methid send a request o Generate a Work Order(s) Report
        /// </summary>
        /// <returns></returns>
        public static ResultGeneric GenerateWorkOrdersReports(string workOrders, Boolean sendEmail, Boolean attachPDF)
        {
            ResultGeneric result = new ResultGeneric();
            result.ReturnCode = 0;
            result.Message = "";
            result.RecordsCount = 0;

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
                    URL = BaseURL + "Reports/GenerateWorkOrderReport?workOrders=" + workOrders + "&sendEmail=" + sendEmail.ToString() + "&attachPDF=" + attachPDF.ToString();

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        result = JsonConvert.DeserializeObject<ResultGeneric>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {

                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.ReturnCode = -2;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        /// <summary>
        /// This Method get Batches from the Databse based on a given filter and sort criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortBy"></param>
        /// <returns>Batch Information Data Structure</returns>
        public static ResultBatches GetBatchesInformation(string filter, string sortBy)
        {
            ResultBatches resultBatches = new ResultBatches();
            resultBatches.ReturnCode = 0;
            resultBatches.Message = "";
            resultBatches.RecordsCount = 0;

            try
            {
                lock (lockObj)
                {
                    // Scape character from filter string
                    filter = Uri.EscapeDataString(filter);

                    HttpClient client = new HttpClient();
                    client.Timeout = TimeSpan.FromMinutes(15);
                    string urlParameters = "";
                    string URL = "";
                    HttpResponseMessage response = new HttpResponseMessage();
                    string returnMessage;

                    URL = BaseURL + "Batches/GetBatchesInformation?filter=" + filter + "&orderby=" + sortBy;

                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultBatches.Message = General.ErrorMessage(ex);
                    resultBatches.ReturnCode = -2;
                }
            }
            return resultBatches;
        }


        /// <summary>
        /// Get Page Sizes definied for a given Job ID
        /// </summary>
        /// <returns></returns>
        public static ResultJobPageSizes GetPageSizesByJobID(int jobID)
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
                    URL = BaseURL + "Jobs/GetPageSizesByJobID?jobID=" + jobID.ToString() ;

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
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultJobPageSizes.ReturnCode = -2;
                    resultJobPageSizes.Message = ex.Message;
                }
            }
            return resultJobPageSizes;
        }

        /// <summary>
        /// Get Users from Database
        /// </summary>
        /// <returns></returns>
        public static ResultUsers GetUsers()
        {
            ResultUsers resultUsers = new ResultUsers();
            resultUsers.ReturnCode = 0;
            resultUsers.Message = "";
            resultUsers.RecordsCount = 0;

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
                    URL = BaseURL + "Users/GetUsers";

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultUsers = JsonConvert.DeserializeObject<ResultUsers>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultUsers.ReturnCode = -2;
                    resultUsers.Message = ex.Message;
                }
            }
            return resultUsers;
        }


        /// <summary>
        /// Get User by Name
        /// </summary>
        /// <returns></returns>
        public static ResultUsers GetUserByName(string userName)
        {
            ResultUsers resultUsers = new ResultUsers();
            resultUsers.ReturnCode = 0;
            resultUsers.Message = "";
            resultUsers.RecordsCount = 0;

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
                    URL = BaseURL + "Users/GetUserByName?userName=" + userName;

                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultUsers = JsonConvert.DeserializeObject<ResultUsers>(returnMessage);
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (lockObj)
                {
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resultUsers.ReturnCode = -2;
                    resultUsers.Message = ex.Message;
                }
            }
            return resultUsers;
        }

        /// <summary>
        /// Update Job Page Size Information
        /// </summary>
        /// <param name="jobPageSize"></param>
        /// <returns></returns>
        public static ResultGeneric UpdateJobPageSize(JobPageSize jobPageSize)
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
                    string jobPageSizeJS = "";

                    jobPageSizeJS = JsonConvert.SerializeObject(jobPageSize, Newtonsoft.Json.Formatting.Indented);
                    jobPageSizeJS = jobPageSizeJS.Replace(@"\", "\\\\");

                    URL = BaseURL + "Jobs/UpdateJobPageSize";

                    bodyString = "'" + jobPageSizeJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }


        /// <summary>
        /// Update User Information
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ResultGeneric UpdateUser(User user)
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
                    string userJS = "";

                    userJS = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);
                    userJS = userJS.Replace(@"\", "\\\\");

                    URL = BaseURL + "Users/UpdateUser";

                    bodyString = "'" + userJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

        /// <summary>
        /// New Job Page Size Information
        /// </summary>
        /// <param name="jobPageSize"></param>
        /// <returns></returns>
        public static ResultGeneric NewJobPageSize(JobPageSize jobPageSize)
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
                    string jobPageSizeJS = "";

                    jobPageSizeJS = JsonConvert.SerializeObject(jobPageSize, Newtonsoft.Json.Formatting.Indented);
                    jobPageSizeJS = jobPageSizeJS.Replace(@"\", "\\\\");

                    URL = BaseURL + "Jobs/NewJobPageSize";

                    bodyString = "'" + jobPageSizeJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }


        /// <summary>
        /// Add a new User to the Database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ResultGeneric NewUser(User user)
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
                    string userJS = "";

                    userJS = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);
                    userJS = userJS.Replace(@"\", "\\\\");

                    URL = BaseURL + "Users/NewUser";

                    bodyString = "'" + userJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    if (!response_for_new.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response_for_new.ReasonPhrase + "\r\n" + response_for_new.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

        /// <summary>
        /// Delete Job Page Size from Database
        /// </summary>
        /// <param name="pageSizeID"></param>
        /// <returns></returns>
        public static ResultGeneric DeleteJobPageSize(int pageSizeID)
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
                    string returnMessage = "";

                    // Get New Customer Information based of currentCustomerName Gloal variable value
                    URL = BaseURL + "Jobs/DeleteJobPageSize";
                    urlParameters = "?pageSizeID=" + pageSizeID.ToString();
                    client.BaseAddress = new Uri(URL);
                    response = client.DeleteAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result.Message = "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage;
                        result.ReturnCode = -1;
                    }
                    else
                    {
                        using (HttpContent content = response.Content)
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

        /// <summary>
        /// Delete User from Database
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static ResultGeneric DeleteUser(int userID)
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
                    string returnMessage = "";

                    URL = BaseURL + "Users/DeleteUser";
                    urlParameters = "?userID=" + userID.ToString();
                    client.BaseAddress = new Uri(URL);
                    response = client.DeleteAsync(urlParameters).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage, "Error ...",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result.Message = "Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage;
                        result.ReturnCode = -1;
                    }
                    else
                    {
                        using (HttpContent content = response.Content)
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
                    nlogger.Fatal(General.ErrorMessage(ex));
                    MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Message = General.ErrorMessage(ex);
                    result.ReturnCode = -2;
                }
            }
            return result;
        }

    }
}
