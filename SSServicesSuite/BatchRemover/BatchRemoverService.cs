using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using static ScanningServicesDataObjects.GlobalVars;
using System.Net.Http.Headers;
using Quartz.Logging;
using System.Collections.Specialized;
using Quartz.Impl;
using Quartz;
using System.Net;
using System.ServiceModel;


 // use the folloeing command to install the application as service
 // sc \\HostName create BatchRemover binpath= ExcutableFullFilePath DisplayName= "SSS Auto Import"
namespace BatchRemover
{
    public partial class BatchRemoverService : ServiceBase
    {
       
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        //public static string BaseURL = "http://localhost:47063" + "/api/";
        public static string BaseURL = BatchRemover.Properties.Settings.Default["APIEndPointURL"].ToString(); 
        public static int SleepTime = Convert.ToInt32(BatchRemover.Properties.Settings.Default["SleepTime"]);

        static object lockObj = new object();

        // Processes List wil lbe used to keep track of runing cron Jobs
        public static List<ScanningServicesDataObjects.GlobalVars.Process> cronJobsList = new List<ScanningServicesDataObjects.GlobalVars.Process>();

        public static string hostName = Dns.GetHostName();

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        public BatchRemoverService(string[] args)
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Trace, "Entering into Service OnStart", "General");

                   // Console.WriteLine("Entering into Service OnStart");
                    General.ConsoleLogger(General.Color.White, "Entering into Service OnStart");

                    // Update the service state to Start Pending.  
                    ServiceStatus serviceStatus = new ServiceStatus();
                    serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
                    serviceStatus.dwWaitHint = SleepTime; // 100000;
                    SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                    // Set up a timer to trigger every minute.  
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 60000; // 60 seconds  
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
                    timer.Start();

                    // Update the service state to Running.  
                    serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
                    SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                    // Start Cron Jobs

                    BatchRemover.Program.RunMultipleJobs().GetAwaiter().GetResult();
                }                
            }
            catch (AggregateException se)
            //catch (SchedulerException se)
            {
                lock (lockObj)
                {
                    foreach (Exception ex in se.InnerExceptions)
                    {
                        General.Logger(General.LogLevel.Error, ex.Message + ". " + ex.InnerException.Message, "General");
                        General.Logger(General.LogLevel.Error, ex.InnerException.InnerException.Message, "General");

                        General.ConsoleLogger(General.Color.Red, ex.Message + ". " + ex.InnerException.Message);
                        General.ConsoleLogger(General.Color.Red, ex.InnerException.InnerException.Message);
                    }
                }                
            }
        }

        protected override void OnStop()
        {
            try
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Trace, "Entering into Service OnStop", "General");
                    General.ConsoleLogger(General.Color.White, "Entering into Service OnStop");

                    // Update the service state to Stop Pending.  
                    ServiceStatus serviceStatus = new ServiceStatus();
                    serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
                    serviceStatus.dwWaitHint = SleepTime; // 100000;
                    SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                    // Update the service state to Stop.  
                    serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
                    SetServiceStatus(this.ServiceHandle, ref serviceStatus);
                }                    
            }
            catch (AggregateException se)
            //catch (SchedulerException se)
            {
                lock (lockObj)
                {
                    foreach (Exception ex in se.InnerExceptions)
                    {
                        General.Logger(General.LogLevel.Error, ex.Message + ". " + ex.InnerException.Message, "General");
                        General.Logger(General.LogLevel.Error, ex.InnerException.InnerException.Message, "General");

                        General.ConsoleLogger(General.Color.Red, ex.Message + ". " + ex.InnerException.Message);
                        General.ConsoleLogger(General.Color.Red, ex.InnerException.InnerException.Message);
                    }
                }                    
            }
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                lock (lockObj)
                {
                    // TODO: Insert monitoring activities here.  
                    DateTime time = DateTime.Now;
                    string format = "MMM ddd d HH:mm yyyy";   // Use this format.
                    General.ConsoleLogger(General.Color.Cyan, time.ToString(format));

                    General.Logger(General.LogLevel.Trace, "Monitoring the System " + EventLogEntryType.Information, "General");
                    General.ConsoleLogger(General.Color.Cyan, "Monitoring the System " + EventLogEntryType.Information);

                    // Start Cron Jobs
                    BatchRemover.Program.RunMultipleJobs().GetAwaiter().GetResult();
                }                   
            }
            catch (AggregateException se)
            //catch (SchedulerException se)
            {
                lock (lockObj)
                {
                    foreach (Exception ex in se.InnerExceptions)
                    {
                        General.Logger(General.LogLevel.Error, ex.Message + ". " + ex.InnerException.Message, "General");
                        General.Logger(General.LogLevel.Error, ex.InnerException.InnerException.Message, "General");

                        General.ConsoleLogger(General.Color.Red, ex.Message + ". " + ex.InnerException.Message);
                        General.ConsoleLogger(General.Color.Red, ex.InnerException.InnerException.Message);
                    }
                }                    
            }
        }

        protected override void OnContinue()
        {
            try
            {
                lock (lockObj)
                {
                    General.Logger(General.LogLevel.Trace, "Entering In onContinue", "General");
                    Console.WriteLine("Entering In onContinue");
                }                    
            }
            catch (AggregateException se)
            //catch (SchedulerException se)

            {
                lock (lockObj)
                {
                    foreach (Exception ex in se.InnerExceptions)
                    {
                        General.Logger(General.LogLevel.Error, ex.Message + ". " + ex.InnerException.Message, "General");
                        General.Logger(General.LogLevel.Error, ex.InnerException.InnerException.Message, "General");

                        General.ConsoleLogger(General.Color.Red, ex.Message + ". " + ex.InnerException.Message);
                        General.ConsoleLogger(General.Color.Red, ex.InnerException.InnerException.Message);
                    }
                }                    
            }
        }

        internal void Startup(string[] args)
        {
            lock (lockObj)
            {
                General.Logger(General.LogLevel.Trace, "SART ....", "General");
                Console.WriteLine("SART ....");
                this.OnStart(args);
                //Console.ReadLine();
                //this.OnStop();
            }
        }
    }
}
