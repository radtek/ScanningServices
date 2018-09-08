
// Follow this links:
// https://docs.microsoft.com/en-us/dotnet/framework/windows-services/walkthrough-creating-a-windows-service-application-in-the-component-designer
// https://stackify.com/creating-net-core-windows-services/

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Quartz.Logging;
using System.Collections.Specialized;
using Quartz.Impl;
using Quartz;
using NLog;
using System.Threading;
using System.IO;
using static ScanningServicesDataObjects.GlobalVars;
using System.Net.Http.Headers;
using CronExpressionDescriptor;
using System.Net;
using System.Diagnostics;
using System.ServiceProcess;

namespace Reporting
{

    static class Program
    {
        
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        static object lockObj = new object();
        
        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            LogManager.Configuration.Variables["JobName"] = "General";
            if (e != null && e.ExceptionObject != null)
            {
                // log exception:
                Exception ex = (Exception)e.ExceptionObject;
                //Console.WriteLine("Service Error on: " + ex.Message); // + ex.StackTrace);
                //nlogger.Fatal("Service Error on: " + ex.Message);
                lock (lockObj)
                {
                    nlogger.Fatal(ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ConsoleKeyInfo cki;
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


#if DEBUG
            ReportingService service1 = new ReportingService(args);
            service1.Startup(args);
            do
            {
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);

#else
            ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new  ReportingService(args)
                        };
                        ServiceBase.Run(ServicesToRun);
#endif

        }

        /// <summary>
        /// This is used to format the Exception Message 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ErrorMessage(Exception e)
        {
            // Getting the line number
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = e.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = e.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }

            //Getting the Method name
            var s = new StackTrace(e);
            var thisasm = System.Reflection.Assembly.GetExecutingAssembly();
            var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

            string message = "";
            if (e.InnerException != null)
            {
                message += "Exception type: " + e.InnerException.GetType() + Environment.NewLine +
                           "Exception message: " + e.InnerException.Message + Environment.NewLine +
                           "Stack trace: " + e.InnerException.StackTrace + Environment.NewLine +
                           "Method: " + methodname + Environment.NewLine +
                           "Line: " + lineNumber + Environment.NewLine;
            }
            else
            {
                message += "Exception Message: " + e.Message + Environment.NewLine +
                          "Stack trace: " + e.StackTrace + Environment.NewLine +
                          "Method: " + methodname + Environment.NewLine +
                          "Line: " + lineNumber + Environment.NewLine;
            }
            return message;
            //MessageBox.Show(message, "Error Message ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

        }

        /// <summary>
        /// Check for availability of the Services API
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean IsUrlAvailable(string URL)
        {
            LogManager.Configuration.Variables["JobName"] = "General";
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
                    nlogger.Fatal(ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.White;
                }                   
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task RunMultipleJobs()
        {
            LogManager.Configuration.Variables["JobName"] = "General";
            try
            {
                HttpClient client = new HttpClient();
                HttpClient client1 = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                client1.Timeout = TimeSpan.FromMinutes(15);
                string urlParameters = "";
                string URL = "";

                HttpResponseMessage response = new HttpResponseMessage();
                HttpResponseMessage response1 = new HttpResponseMessage();
                string returnMessage;
                Boolean cronJobFound;
                Boolean cronJobChanged;
                //Boolean cronJobEnable;

                URL = ReportingService.BaseURL + "Reports/GetReports";

                // Check if URL is available before continue
                if (IsUrlAvailable(URL))
                {
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;

                    // Get Report's Process
                    ResultReports resultReports = new ResultReports();
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                        nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                    }
                    else
                    {
                        using (HttpContent content = response.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            resultReports = JsonConvert.DeserializeObject<ResultReports>(returnMessage);

                            if (resultReports.ReturnCode == 0)
                            {
                                // Check list of Reports
                                foreach (Report report in resultReports.ReturnValue)
                                {
                                    // Keep Job Process in a List SynchronizerService.jobProcesses
                                    // This loop check if processes in the Database are already registerd in the Process List
                                    cronJobFound = false;
                                    cronJobChanged = false;
                                    foreach (ReportingService.ReportProcess reportProcesses in ReportingService.reportProcesses)
                                    {
                                        if (reportProcesses.ReportID == report.ReportID && reportProcesses.CustomerID == report.CustomerID)
                                        {
                                            cronJobFound = true;
                                            if (reportProcesses.ScheduleCron != report.ScheduleCronFormat)
                                            {
                                                cronJobChanged = true;
                                            }
                                            break;
                                        }
                                    }

                                    // Cron Jobs that were not found in the list and has the Enable Flag set to 'true', must be created
                                    if (!cronJobFound && report.EnableFlag)
                                    {
                                        // Create new Cron Job
                                        if (report.EnableFlag)
                                        {
                                            lock (lockObj)
                                            {
                                                LogManager.Configuration.Variables["JobName"] = "General";
                                                nlogger.Info("Creating a new Cron Job...");
                                                nlogger.Info("     Report Name: " + report.ReportName.Trim());
                                                nlogger.Info("     Report ID: " + report.ReportID.ToString());
                                                nlogger.Info("     Customer ID: " + report.CustomerID.ToString());
                                                nlogger.Info("     Customer Name: " + report.CustomerName);
                                                nlogger.Info("     Template ID: " + report.TemplateID.ToString());
                                                nlogger.Info("     Schedule: " + report.ScheduleCronFormat);

                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("Creating a new Cron Job...");
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("     Report Name: " + report.ReportName.Trim());
                                                Console.WriteLine("     Report ID: " + report.ReportID.ToString());
                                                Console.WriteLine("     Customer ID: " + report.CustomerID.ToString());
                                                Console.WriteLine("     Customer Name: " + report.CustomerName);
                                                Console.WriteLine("     Template ID: " + report.TemplateID.ToString());
                                                Console.WriteLine("     Schedule: " + report.ScheduleCronFormat);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }

                                            // Only Register Valid Cron Jobs
                                            if (Quartz.CronExpression.IsValidExpression(report.ScheduleCronFormat))
                                            {
                                                var result = ExpressionDescriptor.GetDescription(report.ScheduleCronFormat, new Options());
                                                //{
                                                //    ThrowExceptionOnParseError = false
                                                //});

                                                ReportingService.ReportProcess reportProcess = new ReportingService.ReportProcess();
                                                reportProcess.ReportID = report.ReportID;
                                                reportProcess.CustomerID = report.CustomerID;
                                                reportProcess.ScheduleCron = report.ScheduleCronFormat;
                                                reportProcess.EnableFlag = report.EnableFlag;
                                                ReportingService.reportProcesses.Add(reportProcess);
                                                RunJob("JOB-" + report.ReportID.ToString() + "-" + report.TemplateID.ToString(), "PROCESS-" + report.ReportID.ToString() + "-" + report.CustomerID.ToString(), report.ScheduleCronFormat).GetAwaiter().GetResult();
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
                                                var result = ExpressionDescriptor.GetDescription(report.ScheduleCronFormat, new Options()
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
                                            // The Cron was found in the List but the Schedule has changed or the Cron has been disabled
                                            // If the Cron has been disbaled, we must remove the Cron Job
                                            // If the Cron was changed, we must remove and recreate the Cron Job (the cron willbe recreated in the next cycle of the services)
                                            // In both cases, we must remove the Cron Job

                                            if (!report.EnableFlag)
                                            {
                                                // Cron Job was Disable
                                                lock (lockObj)
                                                {
                                                    LogManager.Configuration.Variables["JobName"] = "General";
                                                    nlogger.Info("Cron Job was disabled.");
                                                    nlogger.Info("     Report Name: " + report.ReportName.Trim());
                                                    nlogger.Info("     Report ID: " + report.ReportID.ToString());
                                                    nlogger.Info("     Customer ID: " + report.CustomerID.ToString());
                                                    nlogger.Info("     Customer Name: " + report.CustomerName);
                                                    nlogger.Info("     Template ID: " + report.TemplateID.ToString());

                                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                                    Console.WriteLine("Cron Job was disabled.");
                                                    Console.WriteLine("     Report Name: " + report.ReportName.Trim());
                                                    Console.WriteLine("     Report ID: " + report.ReportID.ToString());
                                                    Console.WriteLine("     Customer ID: " + report.CustomerID.ToString());
                                                    Console.WriteLine("     Customer Name: " + report.CustomerName);
                                                    Console.WriteLine("     Template ID: " + report.TemplateID.ToString());
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                            }
                                            if (cronJobChanged && report.EnableFlag)
                                            {
                                                // Cron Job changed and still enable
                                                lock (lockObj)
                                                {
                                                    LogManager.Configuration.Variables["JobName"] = "General";
                                                    nlogger.Info("Cron Job was changed.");
                                                    nlogger.Info("     Report Name: " + report.ReportName.Trim());
                                                    nlogger.Info("     Report ID: " + report.ReportID.ToString());
                                                    nlogger.Info("     Customer ID: " + report.CustomerID.ToString());
                                                    nlogger.Info("     Customer Name: " + report.CustomerName);
                                                    nlogger.Info("     Template ID: " + report.TemplateID.ToString());
                                                    nlogger.Info("     New Schedule String: " + report.ScheduleCronFormat);

                                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                                    Console.WriteLine("Cron Job changed.");
                                                    Console.WriteLine("     Report Name: " + report.ReportName.Trim());
                                                    Console.WriteLine("     Report ID: " + report.ReportID.ToString());
                                                    Console.WriteLine("     Customer ID: " + report.CustomerID.ToString());
                                                    Console.WriteLine("     Customer Name: " + report.CustomerName);
                                                    Console.WriteLine("     Template ID: " + report.TemplateID.ToString());
                                                    Console.WriteLine("     New Schedule String: " + report.ScheduleCronFormat);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                            }


                                            if ((!report.EnableFlag) || (cronJobChanged && report.EnableFlag))
                                            {
                                                // Now, the job needs to be removed it from the Cron Jobs List
                                                // When a Cron Job is removed, the system will let the last running istance to finhish it works
                                                NameValueCollection props = new NameValueCollection
                                            {
                                                { "quartz.serializer.type", "binary" }
                                            };
                                                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                                                IScheduler scheduler = await factory.GetScheduler();
                                                await scheduler.UnscheduleJob(new TriggerKey("PROCESS-" + report.ReportID.ToString() + "-" + report.CustomerID.ToString(), "group1"));
                                                await scheduler.DeleteJob(new JobKey("JOB-" + report.ReportID.ToString() + "-" + report.TemplateID.ToString(), "group1"));
                                                // Keeping the Cron Job List uptodate 
                                                // Remove Cron Job from the List
                                                foreach (ReportingService.ReportProcess reportProcesss in ReportingService.reportProcesses)
                                                {
                                                    if (reportProcesss.ReportID == report.ReportID && reportProcesss.CustomerID == report.CustomerID)
                                                    {
                                                        lock (lockObj)
                                                        {
                                                            LogManager.Configuration.Variables["JobName"] = "General";
                                                            nlogger.Info("Cron Job was deleted form the List");
                                                            nlogger.Info("     Report Name: " + report.ReportName.Trim());
                                                            nlogger.Info("     Report ID: " + report.ReportID.ToString());
                                                            nlogger.Info("     Customer ID: " + report.CustomerID.ToString());
                                                            nlogger.Info("     Customer Name: " + report.CustomerName);
                                                            nlogger.Info("     Template ID: " + report.TemplateID.ToString());

                                                            Console.ForegroundColor = ConsoleColor.Magenta;
                                                            ReportingService.reportProcesses.Remove(reportProcesss);
                                                            Console.WriteLine("Cron Job was deleted form the List.");
                                                            Console.WriteLine("     Report Name: " + report.ReportName.Trim());
                                                            Console.WriteLine("     Report ID: " + report.ReportID.ToString());
                                                            Console.WriteLine("     Customer ID: " + report.CustomerID.ToString());
                                                            Console.WriteLine("     Customer Name: " + report.CustomerName);
                                                            Console.WriteLine("     Template ID: " + report.TemplateID.ToString());
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
                                //await Task.Delay(TimeSpan.FromSeconds(300));
                            }
                            else
                            {
                                lock (lockObj)
                                {
                                    LogManager.Configuration.Variables["JobName"] = "General";
                                    nlogger.Error("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                }                                   
                            }
                        }
                    }
                }
                
                // Some Code that may be helpfull when shuting down the Service ....
                //string schedTest = "0/5 * * 1/1 * ? *";
                //RunJob("job1","trigger1", schedTest).GetAwaiter().GetResult();
                //schedTest = "0/2 * * 1/1 * ? *";
                //RunJob("job2", "trigger2", schedTest).GetAwaiter().GetResult();
                //await Task.Delay(TimeSpan.FromSeconds(300));
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

        private static async Task RunJob(string jobName, string triggerName, string schedule)
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

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<ReportJob>()
                    .WithIdentity(jobName, "group1")
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
                //nlogger.Fatal(se);
                //Console.WriteLine(se);
                lock (lockObj)
                {
                    nlogger.Fatal(ErrorMessage(ex));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ErrorMessage(ex));
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
                        //nlogger.Trace("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                        nlogger.Trace("     [" + level + "] " + func(), parameters);
                        //Console.WriteLine("     [" + level + "] " + func(), parameters);
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
    public class ReportJob : IJob
    {
        static object lockObj = new object();
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            string logJobName = context.JobDetail.Description;
            LogManager.Configuration.Variables["JobName"] = logJobName; // context.JobDetail.Key.Name;

            DateTime time = DateTime.Now;
            string format = "MMM ddd d HH:mm yyyy";   // Use this format.                        

            try
            {
                lock (lockObj)
                {
                    nlogger.Info("Starting Cron Job : " + context.JobDetail.Key.Name + " @ " + time);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(time.ToString(format));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Out.WriteLineAsync("Starting Cron Job : " + context.JobDetail.Key.Name + " @ " + time);
                    // await Console.Out.WriteLineAsync("Starting Cron Job : " + context.JobDetail.Key.Name + " @ " + time);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                EMAIL email = new EMAIL();
                HttpClient client1 = new HttpClient();
                HttpClient client2 = new HttpClient();
                client1.Timeout = TimeSpan.FromMinutes(15);
                client2.Timeout = TimeSpan.FromMinutes(15);
                string reportName = "";
                string customerName = "";
                string recipients = "";
                string subject = "";

                string urlParameters = "";
                string URL = "";
                HttpResponseMessage response1 = new HttpResponseMessage();
                HttpResponseMessage response2 = new HttpResponseMessage();
                string returnMessage;

                // 1. Extarct the ReportID and TemplateID from context.JobDetail.Key.Name
                string jobKeyName = context.JobDetail.Key.Name.Replace("JOB-", "");
                int reportID = Convert.ToInt32(jobKeyName.Split('-').First());
                int tempalteID = Convert.ToInt32(jobKeyName.Split('-').Last());

                // 2. Generate Reports Body
                nlogger.Info("Requesting report information ...");
                URL = ReportingService.BaseURL + "Reports/GenerateReport?reportID=" + reportID.ToString() + "&templateID=" + tempalteID.ToString() + "&sendEmail=true";
                client1.BaseAddress = new Uri(URL);
                response1 = client1.GetAsync(urlParameters).Result;
                ResultGeneric result = new ResultGeneric();
                if (response1.IsSuccessStatusCode)
                {
                    using (HttpContent content1 = response1.Content)
                    {
                        Task<string> resultTemp1 = content1.ReadAsStringAsync();
                        returnMessage = resultTemp1.Result;
                        result = JsonConvert.DeserializeObject<ResultGeneric>(returnMessage);

                        if (result.ReturnCode == 0)
                        {
                            // Here is Body of the report ... we may want to log this along with some other information
                            email.Body = result.StringReturnValue;

                            // 3. Get reportID information by report ID  - We may want to get more information about this report.
                            // like:
                            // mail was sent to
                            // Report Name
                            ResultReports reports = new ResultReports();
                            URL = ReportingService.BaseURL + "Reports/GetReportByID?reportID=" + reportID.ToString();
                            client2.BaseAddress = new Uri(URL);
                            response2 = client2.GetAsync(urlParameters).Result;
                            if (response2.IsSuccessStatusCode)
                            {
                                using (HttpContent content2 = response2.Content)
                                {
                                    Task<string> resultTemp2 = content2.ReadAsStringAsync();
                                    returnMessage = resultTemp2.Result;
                                    reports = JsonConvert.DeserializeObject<ResultReports>(returnMessage);
                                }
                                reportName = reports.ReturnValue[0].ReportName.Trim();
                                customerName = reports.ReturnValue[0].CustomerName;
                                recipients = reports.ReturnValue[0].EmailRecipients.Trim();
                                subject = reports.ReturnValue[0].EmailSubject.Trim();
                            }
                            lock (lockObj)
                            {
                                if (result.Message.Contains("This Report has not been implemented yet."))
                                {
                                    nlogger.Info("The Report \"" + reportName.Trim() + "\" has not been implemented yet.");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("The Report \"" + reportName.Trim() + "\" has not been implemented yet.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    nlogger.Info("Email Report was sent successfully.");
                                    nlogger.Info("     Report ID :" + reportID.ToString());
                                    nlogger.Info("     Customer Name :" + customerName);
                                    nlogger.Info("     Report Name :" + reportName);
                                    nlogger.Info("     Recipients :" + recipients);
                                    nlogger.Info("     Subject :" + subject);

                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.WriteLine("Email Report was sent successfully.");
                                    Console.WriteLine("     Report ID :" + reportID.ToString());
                                    Console.WriteLine("     Customer Name :" + customerName);
                                    Console.WriteLine("     Report Name :" + reportName);
                                    Console.WriteLine("     Recipients :" + recipients);
                                    Console.WriteLine("     Subject :" + subject);
                                    //Console.WriteLine("Report Body:" + result.StringReturnValue);
                                    Console.ForegroundColor = ConsoleColor.White;
                                }                                
                            }                                                            
                        }
                        else
                        {
                            // No Report found ...
                            lock (lockObj)
                            {
                                nlogger.Error("Error:" + "\r\n" + response1.ReasonPhrase + "\r\n" + response1.RequestMessage);

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error:" + "\r\n" + response1.ReasonPhrase + "\r\n" + response1.RequestMessage);
                                Console.ForegroundColor = ConsoleColor.White;
                            }                                
                        }
                    }
                }
                else
                {
                    // Error generating Report
                    lock (lockObj)
                    {
                        nlogger.Error("Error:" + "\r\n" + response1.ReasonPhrase + "\r\n" + response1.RequestMessage);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error:" + "\r\n" + response1.ReasonPhrase + "\r\n" + response1.RequestMessage);
                        Console.ForegroundColor = ConsoleColor.White;
                    }                       
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
        }
    }

}
