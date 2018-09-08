using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScanningServicesDataObjects.GlobalVars;

namespace ScanningServicesAdmin.Data
{
    class GlovalVariables
    {
        // These Gloval Variables are mainly use to keep track of the item selected in the Tree Object
        // as well as new/update item in the Tree Object

        public static string BaseURL = ScanningServicesAdmin.Properties.Settings.Default["APIEndPointURL"].ToString(); //"http://localhost:47063" + "/api/";

        // For Customers
        public static int currentCustomerID { get; set; }
        public static string currentCustomerName { get; set; }
        public static List<string> newCustomersList = new List<string>();

        public static string transactionType { get; set; }

        // For Projects
        public static List<string> newProjectsList = new List<string>();
        public static int currentProjectID { get; set; }
        public static string currentProjectName { get; set; }

        //For Jobs
        public static List<string> newJobsList = new List<string>();
        public static int currentJobID { get; set; }
        public static string currentJobName { get; set; }

        //For Fields
        public static List<string> newFieldsList = new List<string>();
        public static int currentFieldID { get; set; }
        public static string currentFieldName { get; set; }
        public static string currentVFRFieldName { get; set; }

        //For Report Template
        public static int currentTemplateID { get; set; }

        //For Report
        public static int currentReportID { get; set; }
        public static string currentReportName { get; set; }
        public static Boolean currentReportEnable { get; set; }

        //For Processes
        public static int currentProcessID { get; set; }
        public static Boolean currentProcessEnable { get; set; }
        public static string currentProcessName { get; set; }
        public static List<Process> currentProcessesList = new List<Process>();
        public static Process currentMasterProcess { get; set; }

        //For Export Configuration
        public static Boolean currentExportRulesConfigured { get; set; }

        //For Service Station
        public static int currentServiceStationID { get; set; }
        public static string currentServiceStationName { get; set; }
        public static List<string> newServiceStaionList = new List<string>();
        public static int currentPDFStationID { get; set; }
        public static string currentPDFStationName { get; set; }

        public static string currentBatchName { get; set; }

    }
}
