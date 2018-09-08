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
 // sc \\HostName create Synchronizer binpath= ExcutableFullFilePath DisplayName= "SSS Auto Import"
namespace Synchronizer
{
    public partial class SynchronizerService : ServiceBase
    {
       
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        //public static string BaseURL = "http://localhost:47063" + "/api/";
        public static string BaseURL = Synchronizer.Properties.Settings.Default["APIEndPointURL"].ToString(); 
        public static int SleepTime = Convert.ToInt32(Synchronizer.Properties.Settings.Default["SleepTime"]);

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

        public SynchronizerService(string[] args)
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                nlogger.Trace("Entering into Service OnStart");
                Console.WriteLine("Entering into Service OnStart");

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

                Synchronizer.Program.RunMultipleJobs().GetAwaiter().GetResult();
            }
            catch (AggregateException se)
            //catch (SchedulerException se)
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                foreach (Exception ex in se.InnerExceptions)
                {
                    nlogger.Fatal(ex.Message + ". " + ex.InnerException.Message);
                    nlogger.Fatal(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message + ". " + ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        protected override void OnStop()
        {
            try
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                nlogger.Trace("Entering into Service OnStop");
                Console.WriteLine("Entering into Service OnStop");

                // Update the service state to Stop Pending.  
                ServiceStatus serviceStatus = new ServiceStatus();
                serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
                serviceStatus.dwWaitHint = SleepTime; // 100000;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                // Update the service state to Stop.  
                serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            }
            catch (AggregateException se)
            //catch (SchedulerException se)
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                foreach (Exception ex in se.InnerExceptions)
                {
                    nlogger.Fatal(ex.Message + ". " + ex.InnerException.Message);
                    nlogger.Fatal(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message + ". " + ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            try
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                // TODO: Insert monitoring activities here.  
                DateTime time = DateTime.Now;
                string format = "MMM ddd d HH:mm yyyy";   // Use this format.
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(time.ToString(format));
                nlogger.Trace("Monitoring the System " + EventLogEntryType.Information);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Monitoring the System " + EventLogEntryType.Information);
                Console.ForegroundColor = ConsoleColor.White;

                // Start Cron Jobs
                Synchronizer.Program.RunMultipleJobs().GetAwaiter().GetResult();
            }
            catch (AggregateException se)
            //catch (SchedulerException se)
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                foreach (Exception ex in se.InnerExceptions)
                {
                    nlogger.Fatal(ex.Message + ". " + ex.InnerException.Message);
                    nlogger.Fatal(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message + ". " + ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        protected override void OnContinue()
        {
            try
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                nlogger.Trace("Entering In onContinue");
                Console.WriteLine("Entering In onContinue");
            }
            catch (AggregateException se)
            //catch (SchedulerException se)
            {
                LogManager.Configuration.Variables["JobName"] = "General";
                foreach (Exception ex in se.InnerExceptions)
                {
                    nlogger.Fatal(ex.Message + ". " + ex.InnerException.Message);
                    nlogger.Fatal(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message + ". " + ex.InnerException.Message);
                    Console.WriteLine(ex.InnerException.InnerException.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        internal void Startup(string[] args)
        {
            LogManager.Configuration.Variables["JobName"] = "General";
            nlogger.Trace("SART ....");
            Console.WriteLine("SART ....");
            this.OnStart(args);
            //Console.ReadLine();
            //this.OnStop();
        }
    }
}
