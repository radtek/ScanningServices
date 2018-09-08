// I was having an issue with .Net standard custom dll
// I edeit the .csproject file
// look for  <itemGroup> tah that contains references 
// add the following line:
// <Reference Include="netstandard" />
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;
using static ScanningServicesAdmin.Data.GlovalVariables;
using NLog;

namespace ScanningServicesAdmin
{

    public partial class MainForm : Form
    {

        // public string BaseURL = ScanningServicesAdmin.Properties.Settings.Default["APIEndPointURL"].ToString(); //"http://localhost:47063" + "/api/";
        //string xyz = ScanningServicesAdmin.Properties.Settings.Default["APIEndPointURL"].ToString();

        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        //static public void ErrorMessage(Exception e)
        //{
        //    // Getting the line number
        //    var lineNumber = 0;
        //    const string lineSearch = ":line ";
        //    var index = e.StackTrace.LastIndexOf(lineSearch);
        //    if (index != -1)
        //    {
        //        var lineNumberText = e.StackTrace.Substring(index + lineSearch.Length);
        //        if (int.TryParse(lineNumberText, out lineNumber))
        //        {
        //        }
        //    }

        //    //Getting the Method name
        //    var s = new StackTrace(e);
        //    var thisasm = System.Reflection.Assembly.GetExecutingAssembly();
        //    var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

        //    string message = "";
        //    //string message =
        //    //        "Exception type: " + e.GetType() + Environment.NewLine +
        //    //        "-------------------------------------------------------------------------" + Environment.NewLine +
        //    //        "Exception message: " + e.Message + Environment.NewLine +
        //    //        "-------------------------------------------------------------------------" + Environment.NewLine +
        //    //         "Stack trace: " + e.StackTrace + Environment.NewLine;
        //    if (e.InnerException != null)
        //    {
        //        message += "Exception type: " + e.InnerException.GetType() + Environment.NewLine +
        //                   "Exception message: " + e.InnerException.Message + Environment.NewLine +
        //                   "Stack trace: " + e.InnerException.StackTrace + Environment.NewLine +
        //                   "Method: " + methodname + Environment.NewLine +
        //                   "Line: " + lineNumber + Environment.NewLine;
        //    }
        //    else
        //    {
        //        message += "Exception Message: " + e.Message + Environment.NewLine +
        //                  "Stack trace: " + e.StackTrace + Environment.NewLine +
        //                  "Method: " + methodname + Environment.NewLine +
        //                  "Line: " + lineNumber + Environment.NewLine;
        //    }
        //    MessageBox.Show(message, "Error Message ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

        //}

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Hide Windows Controls
            this.ControlBox = false;

            TreeNode mainNode = new TreeNode();
            mainNode.Text = "Scanning Services Operation";
            mainNode.Tag = "Scanning Services Operation";
            mainNode.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            mainNode.ImageIndex = imageList.Images.IndexOfKey("ScanningServicesOperation.png");
            mainNode.SelectedImageKey = "ScanningServicesOperation";
            mainNode.StateImageKey = "ScanningServicesOperation";
            mainNode.ForeColor = Color.DarkBlue;
            //mainNode.BackColor = Color.WhiteSmoke;
            SSSTreeView.Nodes.Add(mainNode);

            TreeNode dommyNode = new TreeNode();
            dommyNode.Text = "DummyNode";
            dommyNode.ImageKey = "";
            mainNode.Nodes.Add(dommyNode);
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SSSTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            Boolean recordFound = false;
            Cursor.Current = Cursors.WaitCursor;
            HttpClient client = new HttpClient();
            HttpClient client1 = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            client1.Timeout = TimeSpan.FromMinutes(15);
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            HttpResponseMessage response1 = new HttpResponseMessage();
            string returnMessage;
            SSSTreeView.ImageList = imageList;
            try
            {

                if (e.Node.FirstNode.Text == "DummyNode")
                {
                    e.Node.FirstNode.Remove();
                }
                // Clear child nodes and refresh it
                e.Node.Nodes.Clear();

                TreeNode Node;
                //if (e.Node.FirstNode.Text == "DummyNode")
                //{
                    //e.Node.FirstNode.Remove();
                    switch (e.Node.StateImageKey)
                    {
                        case "ScanningServicesOperation":
                            TreeNode customersList = new TreeNode();
                            customersList.Text = "Customers List";
                            customersList.ForeColor = Color.DarkBlue;
                            customersList.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                            //customersList.BackColor = Color.WhiteSmoke;
                            customersList.ImageIndex = imageList.Images.IndexOfKey("CustomerList.png");
                            customersList.SelectedImageKey = "CustomerList.png";
                            customersList.StateImageKey = "CustomersList";
                            customersList.Nodes.Add("DummyNode");
                            e.Node.Nodes.Add(customersList);

                            TreeNode generalSettings = new TreeNode();
                            generalSettings.Text = "General Configuration";
                            generalSettings.ForeColor = Color.DarkBlue;
                            generalSettings.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                            //generalSettings.BackColor = Color.WhiteSmoke;
                            generalSettings.ImageIndex = imageList.Images.IndexOfKey("GeneralSettings.png");
                            generalSettings.SelectedImageKey = "GeneralSettings.png";
                            generalSettings.StateImageKey = "GeneralSettings";
                            generalSettings.Nodes.Add("DummyNode");
                            e.Node.Nodes.Add(generalSettings);

                            break;

                    case "GeneralSettings":
                        TreeNode batchLocations = new TreeNode();
                        batchLocations.Text = "Working Folders";
                        batchLocations.ForeColor = Color.DarkBlue;
                        batchLocations.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        batchLocations.Name = "Working Folder";
                        batchLocations.ImageIndex = imageList.Images.IndexOfKey("Locations.png");
                        batchLocations.SelectedImageKey = "Locations.png";
                        batchLocations.StateImageKey = "Locations";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(batchLocations);

                        TreeNode servicesList = new TreeNode();
                        servicesList.Text = "Services";
                        servicesList.ForeColor = Color.DarkBlue;
                        servicesList.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //servicesList.BackColor = Color.WhiteSmoke;
                        servicesList.Name = "Services";
                        servicesList.ImageIndex = imageList.Images.IndexOfKey("Services.png");
                        servicesList.SelectedImageKey = "Services.png";
                        servicesList.StateImageKey = "Services";
                        servicesList.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(servicesList);

                        TreeNode pdfStations = new TreeNode();
                        pdfStations.Text = "Service Stations";
                        pdfStations.ForeColor = Color.DarkBlue;
                        pdfStations.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //pdfStations.BackColor = Color.WhiteSmoke;
                        pdfStations.Name = "Service Stations";
                        pdfStations.ImageIndex = imageList.Images.IndexOfKey("ServiceStations.png");
                        pdfStations.SelectedImageKey = "ServiceStations.png";
                        pdfStations.StateImageKey = "ServiceStations";
                        pdfStations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(pdfStations);

                        TreeNode smtpSettings = new TreeNode();
                        smtpSettings.Text = "SMTP Settings";
                        smtpSettings.ForeColor = Color.DarkBlue;
                        smtpSettings.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //smtpSettings.BackColor = Color.WhiteSmoke;
                        smtpSettings.ImageIndex = imageList.Images.IndexOfKey("SMTPSettings.png");
                        smtpSettings.SelectedImageKey = "SMTPSettings.png";
                        smtpSettings.StateImageKey = "SMTPSettings";
                        //smtpSettings.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(smtpSettings);

                        TreeNode systemReportsList = new TreeNode();
                        systemReportsList.Text = "System Reports List";
                        systemReportsList.ForeColor = Color.DarkBlue;
                        systemReportsList.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        systemReportsList.Name = "System Reports List";
                        systemReportsList.ImageIndex = imageList.Images.IndexOfKey("Reports.png");
                        systemReportsList.SelectedImageKey = "Reports.png";
                        systemReportsList.StateImageKey = "SystemReports";
                        systemReportsList.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(systemReportsList);

                        TreeNode reportTemplates = new TreeNode();
                        reportTemplates.Text = "Report Templates";
                        reportTemplates.ForeColor = Color.DarkBlue;
                        reportTemplates.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //smtpSettings.BackColor = Color.WhiteSmoke;
                        reportTemplates.ImageIndex = imageList.Images.IndexOfKey("ReportTemplates.png");
                        reportTemplates.SelectedImageKey = "ReportTemplates.png";
                        reportTemplates.StateImageKey = "ReportTemplates";
                        //reportTemplates.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(reportTemplates);

                        TreeNode users = new TreeNode();
                        users.Text = "Users";
                        users.ForeColor = Color.DarkBlue;
                        users.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //smtpSettings.BackColor = Color.WhiteSmoke;
                        users.ImageIndex = imageList.Images.IndexOfKey("Users.png");
                        users.SelectedImageKey = "Users.png";
                        users.StateImageKey = "Users";
                        //reportTemplates.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(users);

                        break;

                    case "Services":
                        TreeNode autoImport = new TreeNode();
                        autoImport.Text = "Auto Import";
                        autoImport.ForeColor = Color.DarkBlue;
                        autoImport.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        autoImport.Name = "Auto Import";
                        autoImport.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Green-V2.png");
                        autoImport.SelectedImageKey = "Service-Clock-Green-V2.png";
                        autoImport.StateImageKey = "AutoImportService";
                        e.Node.Nodes.Add(autoImport);

                        TreeNode synchronizer = new TreeNode();
                        synchronizer.Text = "Synchronizer";
                        synchronizer.ForeColor = Color.DarkBlue;
                        synchronizer.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        synchronizer.Name = "Synchronizer";
                        synchronizer.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Green-V2.png");
                        synchronizer.SelectedImageKey = "Service-Clock-Green-V2.png";
                        synchronizer.StateImageKey = "SynchronizerService";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(synchronizer);

                        TreeNode indexer = new TreeNode();
                        indexer.Text = "Indexer";
                        indexer.ForeColor = Color.DarkBlue;
                        indexer.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        indexer.Name = "Indexer";
                        indexer.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Green-V2.png");
                        indexer.SelectedImageKey = "Service-Clock-Green-V2.png";
                        indexer.StateImageKey = "IndexerService";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(indexer);

                        TreeNode batchDelivery = new TreeNode();
                        batchDelivery.Text = "Batch Delivery";
                        batchDelivery.ForeColor = Color.DarkBlue;
                        batchDelivery.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        batchDelivery.Name = "Batch Delivery";
                        batchDelivery.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Green-V2.png");
                        batchDelivery.SelectedImageKey = "Service-Clock-Green-V2.png";
                        batchDelivery.StateImageKey = "BatchDeliveryService";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(batchDelivery);

                        TreeNode batchLoadBalancer = new TreeNode();
                        batchLoadBalancer.Text = "Batch Load Balancer";
                        batchLoadBalancer.ForeColor = Color.DarkBlue;
                        batchLoadBalancer.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        batchLoadBalancer.Name = "Batch Load Balancer";
                        batchLoadBalancer.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Green-V2.png");
                        batchLoadBalancer.SelectedImageKey = "Service-Clock-Green-V2.png";
                        batchLoadBalancer.StateImageKey = "BatchLoadBalancerService";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(batchLoadBalancer);

                        TreeNode postValidation = new TreeNode();
                        postValidation.Text = "Post Validation";
                        postValidation.ForeColor = Color.DarkBlue;
                        postValidation.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        postValidation.Name = "Post Validation";
                        postValidation.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Gray-V2.png");
                        postValidation.SelectedImageKey = "Service-Clock-Gray-V2.png";
                        postValidation.StateImageKey = "PostValidation";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(postValidation);

                        TreeNode batchRemoval = new TreeNode();
                        batchRemoval.Text = "Batch Remover";
                        batchRemoval.ForeColor = Color.DarkBlue;
                        batchRemoval.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        ////systemReportsList.BackColor = Color.WhiteSmoke;
                        batchRemoval.Name = "Batch Remover";
                        batchRemoval.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Green-V2.png");
                        batchRemoval.SelectedImageKey = "Service-Clock-Green-V2.png";
                        batchRemoval.StateImageKey = "BatchRemoverService";
                        ////batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(batchRemoval);

                        TreeNode vfrRenamer = new TreeNode();
                        vfrRenamer.Text = "VFR Upload Monitor";
                        vfrRenamer.ForeColor = Color.DarkBlue;
                        vfrRenamer.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        vfrRenamer.Name = "VFR Upload Monitor";
                        vfrRenamer.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Red-V2.png");
                        vfrRenamer.SelectedImageKey = "Service-Clock-Red-V2.png";
                        vfrRenamer.StateImageKey = "VFRUploadMonitor";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(vfrRenamer);

                        TreeNode vfrDuplicateRemover = new TreeNode();
                        vfrDuplicateRemover.Text = "VFR Duplicate Remover";
                        vfrDuplicateRemover.ForeColor = Color.DarkBlue;
                        vfrDuplicateRemover.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        vfrDuplicateRemover.Name = "VFR Duplicate Remover";
                        vfrDuplicateRemover.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Red-V2.png");
                        vfrDuplicateRemover.SelectedImageKey = "Service-Clock-Red-V2.png";
                        vfrDuplicateRemover.StateImageKey = "VFRDuplicateRemover";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(vfrDuplicateRemover);

                        TreeNode vfrUploader = new TreeNode();
                        vfrUploader.Text = "VFR Uploader";
                        vfrUploader.ForeColor = Color.DarkBlue;
                        vfrUploader.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //systemReportsList.BackColor = Color.WhiteSmoke;
                        vfrUploader.Name = "VFR Uploader";
                        vfrUploader.ImageIndex = imageList.Images.IndexOfKey("Service-Clock-Red-V2.png");
                        vfrUploader.SelectedImageKey = "Service-Clock-Red-V2.png";
                        vfrUploader.StateImageKey = "VFRUploader";
                        //batchLocations.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(vfrUploader);                        

                        //TreeNode reporting = new TreeNode();
                        //reporting.Text = "Reporting";
                        //reporting.ForeColor = Color.DarkBlue;
                        //reporting.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        ////reporting.BackColor = Color.WhiteSmoke;
                        //reporting.Name = "Reporting";
                        //reporting.ImageIndex = imageList.Images.IndexOfKey("Reporting.png");
                        //reporting.SelectedImageKey = "Reporting.png";
                        //reporting.StateImageKey = "Reporting";
                        ////reporting.Nodes.Add("DummyNode");
                        //e.Node.Nodes.Add(reporting);

                        break;

                    case "CustomersList":
                            //Display Customers
                            URL = Data.GlovalVariables.BaseURL + "Customers/GetCustomers";
                            client.BaseAddress = new Uri(URL);
                            //HttpResponseMessage 
                            response = client.GetAsync(urlParameters).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;
                                   
                                    ResultCustomers resultCustomers = JsonConvert.DeserializeObject<ResultCustomers>(returnMessage);
                                    if (resultCustomers.ReturnCode == 0)
                                    {
                                        foreach (Customer customer in resultCustomers.ReturnValue)
                                        {
                                            Node = new TreeNode();
                                            Node.Text = customer.CustomerName;
                                            Node.Tag = customer.CustomerID;
                                            Node.Name = customer.CustomerName;
                                            Node.ImageIndex = imageList.Images.IndexOfKey("Customer.png");
                                            Node.SelectedImageKey = "Customer.png";
                                            Node.StateImageKey = "Customer";
                                            Node.Nodes.Add("DummyNode");
                                            e.Node.Nodes.Add(Node);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                }
                            }
                            break;

                        case "Customer":
                            TreeNode projectsList = new TreeNode();
                            projectsList.Text = "Projects List";
                            projectsList.ForeColor = Color.DarkBlue;
                            projectsList.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                            //projectsList.BackColor = Color.WhiteSmoke;
                            projectsList.ImageIndex = imageList.Images.IndexOfKey("Projects.png");
                            projectsList.SelectedImageKey = "Projects.png";
                            projectsList.StateImageKey = "Projects";
                            projectsList.Nodes.Add("DummyNode");
                            e.Node.Nodes.Add(projectsList);

                            TreeNode projectReports = new TreeNode();
                            projectReports.Text = "Reports";
                            projectReports.ForeColor = Color.DarkBlue;
                            projectReports.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                            //projectReports.BackColor = Color.WhiteSmoke;
                            projectReports.Name = "Reports";
                            projectReports.ImageIndex = imageList.Images.IndexOfKey("Reports.png");
                            projectReports.SelectedImageKey = "Reports.png";
                            projectReports.StateImageKey = "ProjectReports";
                            projectReports.Nodes.Add("DummyNode");
                            e.Node.Nodes.Add(projectReports);

                            break;

                        case "Projects":
                            //Display Projects
                            // Get cutomer ID from Node's Parent TAG property
                            int customerID = Int32.Parse(e.Node.Parent.Tag.ToString());
                            URL = BaseURL + "PRojects/GetProjectsByCustomerID?customerID=" + customerID.ToString();
                            client.BaseAddress = new Uri(URL);
                            //HttpResponseMessage 
                            response = client.GetAsync(urlParameters).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultProjects resultProjects = JsonConvert.DeserializeObject<ResultProjects>(returnMessage);
                                    if (resultProjects.ReturnCode == 0)
                                    {
                                        foreach (Project project in resultProjects.ReturnValue)
                                        {
                                            Node = new TreeNode();
                                            Node.Text = project.ProjectName;
                                            Node.Tag = project.ProjectID;
                                            Node.Name = project.ProjectName;
                                            Node.ImageIndex = imageList.Images.IndexOfKey("Project.png");
                                            Node.SelectedImageKey = "Project.png";
                                            Node.StateImageKey = "Project";
                                            Node.Nodes.Add("DummyNode");
                                            e.Node.Nodes.Add(Node);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            break;

                        case "Project":
                            TreeNode jobsList = new TreeNode();
                            jobsList.Text = "Jobs List";
                            jobsList.ForeColor = Color.DarkBlue;
                            jobsList.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                            //jobsList.BackColor = Color.WhiteSmoke;
                            jobsList.ImageIndex = imageList.Images.IndexOfKey("Jobs.png");
                            jobsList.SelectedImageKey = "Jobs.png";
                            jobsList.StateImageKey = "Jobs";
                            jobsList.Nodes.Add("DummyNode");
                            e.Node.Nodes.Add(jobsList);

                            
                            break;


                        case "Jobs":
                            //Display Projects
                            // Get Project ID from Node's Parent TAG property
                            int projectID = Int32.Parse(e.Node.Parent.Tag.ToString());
                            URL = Data.GlovalVariables.BaseURL + "Jobs/GetJobsByProjectID?projectID=" + projectID.ToString();
                            client.BaseAddress = new Uri(URL);
                            //HttpResponseMessage 
                            response = client.GetAsync(urlParameters).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                  "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultJobsExtended resultJobs = JsonConvert.DeserializeObject<ResultJobsExtended>(returnMessage);
                                    if (resultJobs.ReturnCode == 0)
                                    {
                                        foreach (JobExtended job in resultJobs.ReturnValue)
                                        {
                                            Node = new TreeNode();
                                            Node.Text = job.JobName;
                                            Node.Tag = job.JobID;
                                            Node.Name = job.JobName;
                                            Node.ImageIndex = imageList.Images.IndexOfKey("Job.png");
                                            Node.SelectedImageKey = "Job.png";
                                            Node.StateImageKey = "Job";
                                            Node.Nodes.Add("DummyNode");
                                            e.Node.Nodes.Add(Node);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            break;

                        case "Job":
                            TreeNode jobFields = new TreeNode();
                            jobFields.Text = "Fields List";
                            jobFields.ForeColor = Color.DarkBlue;
                            jobFields.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                            //jobFields.BackColor = Color.WhiteSmoke;
                            jobFields.ImageIndex = imageList.Images.IndexOfKey("Fields.png");
                            jobFields.SelectedImageKey = "Fields.png";
                            jobFields.StateImageKey = "Fields";
                            jobFields.Nodes.Add("DummyNode");
                            e.Node.Nodes.Add(jobFields);

                            TreeNode workFlowConfiguration = new TreeNode();
                            workFlowConfiguration.Text = "Background Processes";
                            workFlowConfiguration.ForeColor = Color.DarkBlue;
                            workFlowConfiguration.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                            //workFlowConfiguration.BackColor = Color.WhiteSmoke;
                            workFlowConfiguration.ImageIndex = imageList.Images.IndexOfKey("Workflow.png");
                            workFlowConfiguration.SelectedImageKey = "Workflow.png";
                            workFlowConfiguration.StateImageKey = "Workflow";
                            workFlowConfiguration.Nodes.Add("DummyNode");
                            e.Node.Nodes.Add(workFlowConfiguration);
                            break;

                        case "Fields":
                            //Display Job's Fields
                            // Get Job ID from Node's Parent TAG property
                            int jobID = Int32.Parse(e.Node.Parent.Tag.ToString());
                            URL = Data.GlovalVariables. BaseURL + "Fields/GetFieldsByJobID?jobID=" + jobID.ToString();
                            client.BaseAddress = new Uri(URL);
                            //HttpResponseMessage 
                            response = client.GetAsync(urlParameters).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                  "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultFields resultFields = JsonConvert.DeserializeObject<ResultFields>(returnMessage);
                                    if (resultFields.ReturnCode == 0)
                                    {
                                        foreach (Field field in resultFields.ReturnValue)
                                        {
                                            Node = new TreeNode();
                                            Node.Text = field.CPFieldName;
                                            Node.Tag = field.FieldID;
                                            Node.Name = field.CPFieldName;
                                            if (string.IsNullOrEmpty(field.VFRFieldName.Trim()))
                                            {
                                                Node.ImageIndex = imageList.Images.IndexOfKey("CPField.png");
                                                Node.SelectedImageKey = "CPField.png";
                                            }
                                            else
                                            {
                                                Node.ImageIndex = imageList.Images.IndexOfKey("CPVFRField.png");
                                                Node.SelectedImageKey = "CPVFRField.png";
                                            }
                                            Node.StateImageKey = "Field";
                                            e.Node.Nodes.Add(Node);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            break;

                    case "ServiceStations":
                        //Display Service Stations
                        URL = Data.GlovalVariables.BaseURL + "GeneralSettings/GetServiceStations";
                        client.BaseAddress = new Uri(URL);
                        //HttpResponseMessage 
                        response = client.GetAsync(urlParameters).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                              "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            using (HttpContent content = response.Content)
                            {
                                Task<string> resultTemp = content.ReadAsStringAsync();
                                returnMessage = resultTemp.Result;

                                ResultServiceStations resultServiceStations = JsonConvert.DeserializeObject<ResultServiceStations>(returnMessage);
                                if (resultServiceStations.ReturnCode == 0)
                                {
                                    foreach (ServiceStation serviceStation in resultServiceStations.ReturnValue)
                                    {
                                        Node = new TreeNode();
                                        Node.Text = serviceStation.StationName;
                                        Node.Tag = serviceStation.StationID;
                                        Node.Name = serviceStation.StationName;
                                        if (serviceStation.PDFStationFlag)
                                        {
                                            Node.ImageIndex = imageList.Images.IndexOfKey("PDFStation.png");
                                            Node.SelectedImageKey = "PDFStation.png"; 
                                        }
                                        else
                                        {
                                            Node.ImageIndex = imageList.Images.IndexOfKey("ServiceStation.png");
                                            Node.SelectedImageKey = "ServiceStation.png";
                                        }
                                        
                                        Node.StateImageKey = "ServiceStation";
                                        e.Node.Nodes.Add(Node);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        break;

                    case "Workflow":
                        string iconName;
                        currentProcessEnable = false;
                        int jobID1 = Int32.Parse(e.Node.Parent.Tag.ToString());


                        ScanningServicesDataObjects.GlobalVars.ResultProcesses resultProcesses = new ResultProcesses();
                        resultProcesses = GetProcess();

                        TreeNode autoImportProcess = new TreeNode();
                        autoImportProcess.Text = "Auto Import";
                        autoImportProcess.StateImageKey = "AutoImportProcess";                       
                        iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, jobID1, "Auto Import");
                        autoImportProcess.ImageIndex = imageList.Images.IndexOfKey(iconName);
                        autoImportProcess.SelectedImageKey = iconName;
                        e.Node.Nodes.Add(autoImportProcess);

                        TreeNode batchSynchronizationProcess = new TreeNode();
                        batchSynchronizationProcess.Text = "Batch Synchronization";                           
                        batchSynchronizationProcess.StateImageKey = "BatchSynchronizationProcess";
                        iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, jobID1, "Synchronizer");
                        batchSynchronizationProcess.ImageIndex = imageList.Images.IndexOfKey(iconName);
                        batchSynchronizationProcess.SelectedImageKey = iconName;
                        e.Node.Nodes.Add(batchSynchronizationProcess);

                        //TreeNode qcProcess = new TreeNode();
                        //qcProcess.Text = "04 - QC";
                        //qcProcess.ImageIndex = imageList.Images.IndexOfKey("GreyFilledCircle.png");
                        //qcProcess.SelectedImageKey = "GreyFilledCircle.png";
                        //qcProcess.StateImageKey = "QCProcess";
                        //e.Node.Nodes.Add(qcProcess);

                        //TreeNode outputProcess = new TreeNode();
                        //outputProcess.Text = "05 - Output";
                        //outputProcess.ImageIndex = imageList.Images.IndexOfKey("GreyFilledCircle.png");
                        //outputProcess.SelectedImageKey = "GreyFilledCircle.png";
                        //outputProcess.StateImageKey = "OutputProcess";
                        //e.Node.Nodes.Add(outputProcess);

                        TreeNode postValidationProcess = new TreeNode();
                        postValidationProcess.Text = "Post Validation";
                        postValidationProcess.ImageIndex = imageList.Images.IndexOfKey("OutLineRedCircle.png");
                        postValidationProcess.SelectedImageKey = "OutLineRedCircle.png";
                        postValidationProcess.StateImageKey = "PostValidationProcess";
                        e.Node.Nodes.Add(postValidationProcess);

                        TreeNode pdfLoadBalancerProcess = new TreeNode();
                        pdfLoadBalancerProcess.Text = "Batch Load Balancer";
                        iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, jobID1, "Load Balancer");
                        pdfLoadBalancerProcess.ImageIndex = imageList.Images.IndexOfKey(iconName);
                        pdfLoadBalancerProcess.SelectedImageKey = iconName;
                        pdfLoadBalancerProcess.StateImageKey = "PDFLoadBalancerProcess";
                        e.Node.Nodes.Add(pdfLoadBalancerProcess);

                        TreeNode pdfConversionProcess = new TreeNode();
                        pdfConversionProcess.Text = "File Conversion";
                        pdfConversionProcess.ImageIndex = imageList.Images.IndexOfKey("GreyFilledCircle.png");
                        pdfConversionProcess.SelectedImageKey = "GreyFilledCircle.png";
                        pdfConversionProcess.StateImageKey = "PDFConversionProcess";
                        e.Node.Nodes.Add(pdfConversionProcess);

                        TreeNode batchDeliveryProcess = new TreeNode();
                        batchDeliveryProcess.Text = "Batch Delivery";
                        iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, jobID1, "Batch Delivery");
                        batchDeliveryProcess.ImageIndex = imageList.Images.IndexOfKey(iconName);
                        batchDeliveryProcess.SelectedImageKey = iconName;
                        batchDeliveryProcess.StateImageKey = "BatchDeliveryProcess";
                        e.Node.Nodes.Add(batchDeliveryProcess);

                        TreeNode indexingProcess = new TreeNode();
                        indexingProcess.Text = "Indexing";
                        iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, jobID1, "Indexer");
                        indexingProcess.ImageIndex = imageList.Images.IndexOfKey(iconName);
                        indexingProcess.SelectedImageKey = iconName;
                        indexingProcess.StateImageKey = "IndexingProcess";
                        e.Node.Nodes.Add(indexingProcess);

                        TreeNode vfrProcesse = new TreeNode();
                        vfrProcesse.Text = "VFR Processes";
                        vfrProcesse.ForeColor = Color.DarkBlue;
                        vfrProcesse.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                        //vfrProcesse.BackColor = Color.WhiteSmoke;
                        vfrProcesse.ImageIndex = imageList.Images.IndexOfKey("VFRProcesses.png");
                        vfrProcesse.SelectedImageKey = "VFRProcesses.png";
                        vfrProcesse.StateImageKey = "VFRProcesses";
                        vfrProcesse.Nodes.Add("DummyNode");
                        e.Node.Nodes.Add(vfrProcesse);

                        //TreeNode approvalProcess = new TreeNode();
                        //approvalProcess.Text = "12 - Approval";
                        //approvalProcess.ImageIndex = imageList.Images.IndexOfKey("GreyFilledCircle.png");
                        //approvalProcess.SelectedImageKey = "GreyFilledCircle.png";
                        //approvalProcess.StateImageKey = "ApprovalProcess";
                        //e.Node.Nodes.Add(approvalProcess);


                        URL = Data.GlovalVariables.BaseURL + "Export/GetExportRulesByJobID?jobID=" + jobID1.ToString();
                        client = new HttpClient();
                        client.BaseAddress = new Uri(URL);
                        response = client.GetAsync(urlParameters).Result;
                        ResultExportRules resultExportRules = new ResultExportRules();
                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            using (HttpContent content = response.Content)
                            {
                                Task<string> resultTemp = content.ReadAsStringAsync();
                                returnMessage = resultTemp.Result;
                                resultExportRules = JsonConvert.DeserializeObject<ResultExportRules>(returnMessage);

                                TreeNode exportProcess = new TreeNode();
                                exportProcess.Text = "Export";
                                if (resultExportRules.RecordsCount > 0)
                                {
                                    exportProcess.ImageIndex = imageList.Images.IndexOfKey("GreeFilledCircle.png");
                                    exportProcess.SelectedImageKey = "GreeFilledCircle.png";
                                }
                                else
                                {
                                    exportProcess.ImageIndex = imageList.Images.IndexOfKey("OutLineRedCircle.png");
                                    exportProcess.SelectedImageKey = "OutLineRedCircle.png";
                                }
                                exportProcess.StateImageKey = "ExportProcess";
                                e.Node.Nodes.Add(exportProcess);
                            }
                        }


                        TreeNode batchRemoverProcess = new TreeNode();
                        batchRemoverProcess.Text = "Batch Remover";
                        batchRemoverProcess.StateImageKey = "BatchRemoverProcess";
                        iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, jobID1, "Batch Remover");
                        batchRemoverProcess.ImageIndex = imageList.Images.IndexOfKey(iconName);
                        batchRemoverProcess.SelectedImageKey = iconName;
                        e.Node.Nodes.Add(batchRemoverProcess);


                        break;

                        case "VFRProcesses":
                            TreeNode renamerProcess = new TreeNode();
                            renamerProcess.Text = "Upload Monitor";
                            renamerProcess.ImageIndex = imageList.Images.IndexOfKey("OutLineRedCircle.png");
                            renamerProcess.SelectedImageKey = "OutLineRedCircle.png";
                            renamerProcess.StateImageKey = "UploadMonitorProcess";
                            e.Node.Nodes.Add(renamerProcess);

                            TreeNode duplicateRemoverProcess = new TreeNode();
                            duplicateRemoverProcess.Text = "Duplicate Remover";
                            duplicateRemoverProcess.ImageIndex = imageList.Images.IndexOfKey("OutLineRedCircle.png");
                            duplicateRemoverProcess.SelectedImageKey = "OutLineRedCircle.png";
                            duplicateRemoverProcess.StateImageKey = "DuplicateRemoverProcess";
                            e.Node.Nodes.Add(duplicateRemoverProcess);

                            TreeNode batchUploaderProcess = new TreeNode();
                            batchUploaderProcess.Text = "Uploader";
                            batchUploaderProcess.ImageIndex = imageList.Images.IndexOfKey("OutLineRedCircle.png");
                            batchUploaderProcess.SelectedImageKey = "OutLineRedCircle.png";
                            batchUploaderProcess.StateImageKey = "UploaderProcess";
                            e.Node.Nodes.Add(batchUploaderProcess);

                            break;

                        case "SystemReports":
                            //Display System Reports Template
                            URL = Data.GlovalVariables.BaseURL + "Reports/GetSystemReportsTemplate";
                            client.BaseAddress = new Uri(URL);
                            //HttpResponseMessage 
                            response = client.GetAsync(urlParameters).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                  "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultReportsTemplate resultReportTemplate = JsonConvert.DeserializeObject<ResultReportsTemplate>(returnMessage);
                                    if (resultReportTemplate.ReturnCode == 0)
                                    {
                                    //foreach (ReportTemplate reportTemplate in resultReportTemplate.ReturnValue)
                                    //{
                                    //    Node = new TreeNode();
                                    //    Node.Text = reportTemplate.Description;
                                    //    Node.Tag = reportTemplate.TemplateID;
                                    //    Node.Name = reportTemplate.Description;

                                    //    Node.ImageIndex = imageList.Images.IndexOfKey("Report-Incomplete.png");
                                    //    Node.SelectedImageKey = "Report-Incomplete.png";
                                    //    Node.StateImageKey = "Report";
                                    //    e.Node.Nodes.Add(Node);
                                    //}
                                    // Geeting the list of Customer Reports so we can decide what color to use 
                                    // when the object is displayed in the Treeview
                                    URL = Data.GlovalVariables.BaseURL + "Reports/GetReports";
                                    client1.BaseAddress = new Uri(URL);
                                    ResultReports resultReport = new ResultReports();
                                    urlParameters = "";
                                    response1 = client1.GetAsync(urlParameters).Result;
                                    if (!response1.IsSuccessStatusCode)
                                    {
                                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                          "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        using (HttpContent content1 = response1.Content)
                                        {
                                            Task<string> resultTemp1 = content1.ReadAsStringAsync();
                                            returnMessage = resultTemp1.Result;
                                            resultReport = JsonConvert.DeserializeObject<ResultReports>(returnMessage);
                                        }
                                    }

                                    foreach (ReportTemplate reportTemplate in resultReportTemplate.ReturnValue)
                                    {
                                        recordFound = false;
                                        Node = new TreeNode();
                                        Node.Text = reportTemplate.Description;
                                        Node.Tag = reportTemplate.TemplateID;
                                        Node.Name = reportTemplate.Description;
                                        // Check if the tenplate id is in the Customer's Reports List
                                        foreach (Report report in resultReport.ReturnValue)
                                        {
                                            if (report.TemplateID == reportTemplate.TemplateID && reportTemplate.Type == "System")
                                            {
                                                //Tamplate Id Found
                                                if (report.EnableFlag.ToString().ToLower() == "true")
                                                {
                                                    Node.ImageIndex = imageList.Images.IndexOfKey("Report-Enable.png");
                                                    Node.SelectedImageKey = "Report-Enable.png";
                                                }
                                                else
                                                {
                                                    Node.ImageIndex = imageList.Images.IndexOfKey("Report-Disable.png");
                                                    Node.SelectedImageKey = "Report-Disable.png";
                                                }
                                                recordFound = true;
                                                break;
                                            }
                                        }
                                        if (!recordFound)
                                        {
                                            Node.ImageIndex = imageList.Images.IndexOfKey("Report-Incomplete.png");
                                            Node.SelectedImageKey = "Report-Incomplete.png";
                                        }
                                        Node.StateImageKey = "Report";
                                        e.Node.Nodes.Add(Node);
                                    }
                                }
                                    else
                                    {
                                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            break;

                        case "ProjectReports":
                            //Display Customer Reports Template
                            URL = Data.GlovalVariables.BaseURL + "Reports/GetCustomerReportsTemplate";
                            client.BaseAddress = new Uri(URL);
                            //HttpResponseMessage 
                            response = client.GetAsync(urlParameters).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                  "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultReportsTemplate resultReportTemplate = JsonConvert.DeserializeObject<ResultReportsTemplate>(returnMessage);
                                    if (resultReportTemplate.ReturnCode == 0)
                                    {
                                        // Geeting the list of Customer Reports so we can decide what color to use 
                                        // when the object is displayed in the Treeview
                                        URL = Data.GlovalVariables.BaseURL + "Reports/GetReportsByCustomerName";
                                        client1.BaseAddress = new Uri(URL);
                                        ResultReports resultReport = new ResultReports();
                                        urlParameters = "?customerName=" + Data.GlovalVariables.currentCustomerName;
                                        response1 = client1.GetAsync(urlParameters).Result;
                                        if (!response1.IsSuccessStatusCode)
                                        {
                                            MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                              "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            using (HttpContent content1 = response1.Content)
                                            {
                                                Task<string> resultTemp1 = content1.ReadAsStringAsync();
                                                returnMessage = resultTemp1.Result;
                                                resultReport = JsonConvert.DeserializeObject<ResultReports>(returnMessage);
                                            }
                                        }

                                        foreach (ReportTemplate reportTemplate in resultReportTemplate.ReturnValue)
                                        {
                                            recordFound = false;
                                            Node = new TreeNode();
                                            Node.Text = reportTemplate.Description;
                                            Node.Tag = reportTemplate.TemplateID;
                                            Node.Name = reportTemplate.Description;
                                            // Check if the tenplate id is in the Customer's Reports List
                                            foreach (Report report in resultReport.ReturnValue)
                                            {
                                                if (report.TemplateID == reportTemplate.TemplateID)
                                                {
                                                    //Tamplate Id Found
                                                    if (report.EnableFlag.ToString().ToLower() == "true")
                                                    {
                                                        Node.ImageIndex = imageList.Images.IndexOfKey("Report-Enable.png");
                                                        Node.SelectedImageKey = "Report-Enable.png";
                                                    }
                                                    else
                                                    {
                                                        Node.ImageIndex = imageList.Images.IndexOfKey("Report-Disable.png");
                                                        Node.SelectedImageKey = "Report-Disable.png";
                                                    }
                                                    recordFound = true;
                                                    break;
                                                }
                                            }
                                            if (!recordFound)
                                            {
                                               Node.ImageIndex = imageList.Images.IndexOfKey("Report-Incomplete.png");
                                               Node.SelectedImageKey = "Report-Incomplete.png";
                                            }
                                            Node.StateImageKey = "Report";
                                            e.Node.Nodes.Add(Node);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            break;

                    }
                //}
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //catch (Exception ex)
            //{
            //    Cursor.Current = Cursors.Default;
            //    Console.WriteLine("{0} Second exception caught.", ex);
            //}
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

        }

        private void SSSTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            propertiesToolStripMenuItem.Enabled = true;
            newToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            SSSTreeView.ContextMenuStrip = null;          


            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            if (e.Button == MouseButtons.Left)
            {
                if (SSSTreeView.GetNodeAt(e.X, e.Y) != null)
                {
                    SSSTreeView.SelectedNode = SSSTreeView.GetNodeAt(e.X, e.Y);

                    switch (SSSTreeView.SelectedNode.StateImageKey)
                    {
                        case "Customer":
                            Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Text;
                            Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                            break;
                        case "Project":
                            Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                            Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                            Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Text;
                            Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                            break;

                        case "Job":
                            Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Text;
                            Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Tag);
                            Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                            Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                            Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Text;
                            Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                            break;

                        case "Field":
                            Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Text;
                            Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Tag);
                            Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Text;
                            Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Tag);
                            Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                            Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                            Data.GlovalVariables.currentFieldName = SSSTreeView.SelectedNode.Text;
                            Data.GlovalVariables.currentFieldID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                            break;

                        case "VFRProcesses":
                            Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Text;
                            Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Tag);
                            Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Text;
                            Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Tag);
                            Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                            Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                            Forms.VFRSettingsForm _VFRSettingsForm = new Forms.VFRSettingsForm();
                            break;
                    }
                }
            }

            propertiesToolStripMenuItem.Visible = false;
            newToolStripMenuItem.Visible = false;
            deleteToolStripMenuItem.Visible = false;
            importFieldsToolStripMenuItem.Visible = false;
            pageSizeCategoriesToolStripMenuItem.Visible = false;
            toolStripSeparator.Visible = false;

            if (e.Button == MouseButtons.Right)
            {
                SSSTreeView.SelectedNode = SSSTreeView.GetNodeAt(e.X, e.Y);
                switch (SSSTreeView.SelectedNode.StateImageKey)
                {
                    case "CustomersList":                       
                        newToolStripMenuItem.Visible = true;                        
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Customer":
                        propertiesToolStripMenuItem.Visible = true;
                        deleteToolStripMenuItem.Visible = true;
                        toolStripSeparator.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Projects":
                        newToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Project":
                        propertiesToolStripMenuItem.Visible = true;
                        newToolStripMenuItem.Visible = true;
                        deleteToolStripMenuItem.Visible = true;
                        toolStripSeparator.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Jobs":
                        newToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Job":
                        propertiesToolStripMenuItem.Visible = true;
                        deleteToolStripMenuItem.Visible = true;
                        toolStripSeparator.Visible = true;
                        pageSizeCategoriesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Fields":
                        newToolStripMenuItem.Visible = true;
                        importFieldsToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Field":
                        propertiesToolStripMenuItem.Visible = true;
                        deleteToolStripMenuItem.Visible = true;
                        toolStripSeparator.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "QCProcess":
                    case "ScanProcess":
                    case "PDFConversionProcess":
                    case "OutputProcess":
                    case "ApprovalProcess":
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "AutoImportProcess":
                    case "BatchSynchronizationProcess":
                    case "PostValidationProcess":
                    case "PDFLoadBalancerProcess":                    
                    case "BatchDeliveryProcess":
                    case "IndexingProcess":                    
                    case "ExportProcess":
                    case "RenamerProcess":
                    case "DuplicateRemoverProcess":
                    case "BatchUploaderProcess":
                    case "BatchMonitorProcess":
                    case "BatchRemoverProcess":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "VFRProcesses":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "SMTPSettings":
                       propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "GeneralSettings":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Report":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "ReportTemplates":
                        newToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;
                        
                    case "BatchDeliveryService":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "AutoImportService":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "SynchronizerService":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "IndexerService":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "BatchLoadBalancerService":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "BatchRemoverService":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "ServiceStations":
                        newToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "ServiceStation":
                        propertiesToolStripMenuItem.Visible = true;
                        deleteToolStripMenuItem.Visible = true;
                        toolStripSeparator.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Locations":
                        propertiesToolStripMenuItem.Visible = true;
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    case "Users":
                        propertiesToolStripMenuItem.Visible = true;
                       
                        SSSTreeView.ContextMenuStrip = contextMenuStrip;
                        break;

                    default:
                        //SSSTreeView.ContextMenuStrip = null;
                        break;
                }
            }
            deleteToolStripMenuItem.Enabled = false;
            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string iconName = "";
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            string returnMessage = "";
            Data.GlovalVariables.transactionType = "Update";
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            ResultProcessTypes resultResultProcessTypes = new ResultProcessTypes();

            ScanningServicesDataObjects.GlobalVars.ResultProcesses resultProcesses = new ResultProcesses();
            resultProcesses = GetProcess();

            switch (SSSTreeView.SelectedNode.StateImageKey)
            {
                case "Customer":
                    Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Text;
                    Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                    Forms.CustomerForm _CustomerForm = new Forms.CustomerForm();
                    _CustomerForm.ShowDialog();

                    SSSTreeView.SelectedNode.Text = Data.GlovalVariables.currentCustomerName;
                    break;

                case "Project":
                    Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Text;
                    Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                    Forms.ProjectForm _PojectForm = new Forms.ProjectForm();
                    _PojectForm.ShowDialog();

                    SSSTreeView.SelectedNode.Text = Data.GlovalVariables.currentProjectName;
                    break;

                case "Job":
                    Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Text;
                    Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Tag);
                    Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Text;
                    Data.GlovalVariables.currentJobID= Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                    Forms.JobTypeForm _JobForm = new Forms.JobTypeForm();
                    Cursor.Current = Cursors.WaitCursor;
                    _JobForm.ShowDialog();
                    Cursor.Current = Cursors.Default;
                    SSSTreeView.SelectedNode.Text = Data.GlovalVariables.currentJobName;
                    break;

                case "Field":
                    Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Text;
                    Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Tag);
                    Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Text;
                    Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Tag);
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Data.GlovalVariables.currentFieldName = SSSTreeView.SelectedNode.Text;
                    Data.GlovalVariables.currentFieldID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                    Forms.FieldForm _FieldForm = new Forms.FieldForm();
                    _FieldForm.ShowDialog();

                    SSSTreeView.SelectedNode.Text = Data.GlovalVariables.currentFieldName;
                    if (string.IsNullOrEmpty(Data.GlovalVariables.currentVFRFieldName))
                    {                        
                        SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey("CPField.png");
                        SSSTreeView.SelectedNode.SelectedImageKey = "CPField.png";                                        
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Data.GlovalVariables.currentVFRFieldName.Trim()))
                        {
                            SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey("CPField.png");
                            SSSTreeView.SelectedNode.SelectedImageKey = "CPField.png";
                        }
                        else
                        {
                            SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey("CPVFRField.png");
                            SSSTreeView.SelectedNode.SelectedImageKey = "CPVFRField.png";
                        }
                    }
                    
                    break;

                case "SMTPSettings":                   
                    Forms.SMTPSettingsForm _SMTPSettingsForm = new Forms.SMTPSettingsForm();
                    _SMTPSettingsForm.ShowDialog();
                    break;


                case "GeneralSettings":
                    Forms.GeneralSettingsForm _GeneralSettingsForm = new Forms.GeneralSettingsForm();
                    _GeneralSettingsForm.ShowDialog();
                    break;

                case "VFRProcesses":
                    Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Text;
                    Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Parent.Tag);
                    Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Text;
                    Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Tag);
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Forms.VFRSettingsForm _VFRSettingsForm = new Forms.VFRSettingsForm();
                    _VFRSettingsForm.ShowDialog();
                    break;

                case "Report":
                    Data.GlovalVariables.transactionType = "";
                    Data.GlovalVariables.currentReportEnable = false;
                    Data.GlovalVariables.currentReportName = SSSTreeView.SelectedNode.Text;
                    Data.GlovalVariables.currentTemplateID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                    if (SSSTreeView.SelectedNode.Parent.Name == "System Reports List")
                        Data.GlovalVariables.currentCustomerID = 0;
                    else
                        Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                  
                    Forms.ReportForm _Reports = new Forms.ReportForm();
                    _Reports.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(Data.GlovalVariables.transactionType))
                    {

                        if (Data.GlovalVariables.currentReportEnable)
                        {   
                            SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey("Report-Enable.png");
                            SSSTreeView.SelectedNode.SelectedImageKey = "Report-Enable.png";
                        }
                        else
                        {
                            SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey("Report-Disable.png");
                            SSSTreeView.SelectedNode.SelectedImageKey = "Report-Disable.png";
                        }
                    }
                    

                    // Node.ImageIndex = imageList.Images.IndexOfKey("Report-Enable.png");
                    // Node.SelectedImageKey = "Report-Enable.png";
                    break;

                case "IndexingProcess":

                    Data.GlovalVariables.currentProcessID = getProcessID("Indexer");
                    Data.GlovalVariables.currentProcessName = "Indexer";
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Forms.ProcessesForm _ProcessesIndexerForm = new Forms.ProcessesForm();
                    _ProcessesIndexerForm.ShowDialog();
                    resultProcesses = GetProcess();
                    iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, Data.GlovalVariables.currentJobID, "Indexer");
                    SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey(iconName);
                    SSSTreeView.SelectedNode.SelectedImageKey = iconName;
                    break;

                case "PDFLoadBalancerProcess":
                    Data.GlovalVariables.currentProcessID = getProcessID("Load Balancer");
                    Data.GlovalVariables.currentProcessName = "Load Balancer";                    
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Forms.ProcessesForm _ProcessesLoadBalancerForm = new Forms.ProcessesForm();
                    _ProcessesLoadBalancerForm.ShowDialog();
                    resultProcesses = GetProcess();
                    iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, Data.GlovalVariables.currentJobID, "Load Balancer");
                    SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey(iconName);
                    SSSTreeView.SelectedNode.SelectedImageKey = iconName;
                    break;

                case "AutoImportProcess":
                    Data.GlovalVariables.currentProcessID = getProcessID("Auto Import");
                    Data.GlovalVariables.currentProcessName = "Auto Import";
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Forms.ProcessesForm _ProcessesAutoImportForm = new Forms.ProcessesForm();
                    _ProcessesAutoImportForm.ShowDialog();
                    resultProcesses = GetProcess();
                    iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, Data.GlovalVariables.currentJobID, "Auto Import");
                    SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey(iconName);
                    SSSTreeView.SelectedNode.SelectedImageKey = iconName;
                    break;

                case "BatchSynchronizationProcess":
                    Data.GlovalVariables.currentProcessID = getProcessID("Synchronizer");
                    Data.GlovalVariables.currentProcessName = "Synchronizer";                    
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Forms.ProcessesForm _ProcessesSynchronizerForm = new Forms.ProcessesForm();
                    _ProcessesSynchronizerForm.ShowDialog();
                    resultProcesses = GetProcess();
                    iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, Data.GlovalVariables.currentJobID, "Synchronizer");
                    SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey(iconName);
                    SSSTreeView.SelectedNode.SelectedImageKey = iconName;
                    break;

                case "BatchDeliveryProcess":
                    Data.GlovalVariables.currentProcessID = getProcessID("Batch Delivery");
                    Data.GlovalVariables.currentProcessName = "Batch Delivery";                                       
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Forms.ProcessesForm _BatchDeliveyProcessesServicesForm = new Forms.ProcessesForm();
                    _BatchDeliveyProcessesServicesForm.ShowDialog();
                    resultProcesses = GetProcess();
                    iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, Data.GlovalVariables.currentJobID, "Batch Delivery");
                    SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey(iconName);
                    SSSTreeView.SelectedNode.SelectedImageKey = iconName;
                    break;

                case "RenamerProcess":
                    Forms.RenamerForm _RenamerForm = new Forms.RenamerForm();
                    _RenamerForm.ShowDialog();
                    break;

                case "DuplicateRemoverProcess":
                    Forms.DuplicateRemoverForm _DuplicateRemoverForm = new Forms.DuplicateRemoverForm();
                    _DuplicateRemoverForm.ShowDialog();
                    break;

                case "BatchUploaderProcess":
                    Forms.UploaderForm _UploaderForm = new Forms.UploaderForm();
                    _UploaderForm.ShowDialog();
                    break;

                case "BatchMonitorProcess":
                    Forms.UploaderMonitorForm _UploaderMonitorForm = new Forms.UploaderMonitorForm();
                    _UploaderMonitorForm.ShowDialog();
                    break;

                case "BatchRemoverProcess":
                    Data.GlovalVariables.currentProcessID = getProcessID("Batch Remover");
                    Data.GlovalVariables.currentProcessName = "Batch Remover";
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text;
                    Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Tag);
                    Forms.ProcessesForm _BatchRemoverProcessForm = new Forms.ProcessesForm();
                    _BatchRemoverProcessForm.ShowDialog();
                    resultProcesses = GetProcess();
                    iconName = workFlowIconToDisplay(resultProcesses.ReturnValue, Data.GlovalVariables.currentJobID, "Batch Remover");
                    SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey(iconName);
                    SSSTreeView.SelectedNode.SelectedImageKey = iconName;
                    break;

                case "ExportProcess":
                    Data.GlovalVariables.transactionType = "";
                    Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Parent.Text.Trim();
                    switch (SSSTreeView.SelectedNode.SelectedImageKey)
                    { 
                        case "GreeFilledCircle.png":
                            Data.GlovalVariables.currentExportRulesConfigured = true;
                            break;
                        case "OutLineRedCircle.png":
                            Data.GlovalVariables.currentExportRulesConfigured = false;
                            break;
                    }
                   
                    Forms.ExportConfigurationForm _ExportConfigurationForm = new Forms.ExportConfigurationForm();
                    _ExportConfigurationForm.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(Data.GlovalVariables.transactionType))
                    {

                        if (Data.GlovalVariables.currentExportRulesConfigured)
                        {
                            SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey("GreeFilledCircle.png");
                            SSSTreeView.SelectedNode.SelectedImageKey = "GreeFilledCircle.png";
                        }
                        else
                        {
                            SSSTreeView.SelectedNode.ImageIndex = imageList.Images.IndexOfKey("OutLineRedCircle.png");
                            SSSTreeView.SelectedNode.SelectedImageKey = "OutLineRedCircle.png";
                        }
                    }
                    break;

                case "AutoImportService":
                case "IndexerService":
                case "SynchronizerService":
                case "BatchLoadBalancerService":
                case "BatchDeliveryService":
                case "BatchRemoverService":
                    switch (SSSTreeView.SelectedNode.StateImageKey)
                    {
                        case "AutoImportService":
                            currentProcessName = "Auto Import";
                            break;
                        case "IndexerService":
                            currentProcessName = "Indexer";
                            break;
                        case "SynchronizerService":
                            currentProcessName = "Synchronizer";
                            break;
                        case "BatchLoadBalancerService":
                            currentProcessName = "Load Balancer";
                            break;
                        case "BatchDeliveryService":
                            currentProcessName = "Batch Delivery";
                            break;
                        case "BatchRemoverService":
                            currentProcessName = "Batch Remover";
                            break;
                    }
                    Data.GlovalVariables.currentJobName = "";

                    // Get Process Type ID based on process name
                    URL = BaseURL + "Process/GetProcessTYpes";
                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultResultProcessTypes = JsonConvert.DeserializeObject<ResultProcessTypes>(returnMessage);
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        // Identify what is the Process ID 
                        foreach (ProcessType process in resultResultProcessTypes.ReturnValue)
                        {                           
                            if (process.Name.Trim() == currentProcessName)
                            {
                                currentProcessID = process.ProcessID;                                
                                break;
                            }  
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Forms.ProcessesForm _ProcessesServicesForm = new Forms.ProcessesForm();
                    _ProcessesServicesForm.ShowDialog();

                    break;

                case "ServiceStation":
                    Data.GlovalVariables.currentServiceStationID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
                    Data.GlovalVariables.transactionType = "Update";
                    Forms.ServiceStationsForm _ServiceStationsForm = new Forms.ServiceStationsForm();
                    _ServiceStationsForm.ShowDialog();
                    break;

                case "Locations":
                    Forms.WorkingFoldersForm _WorkingFoldersForm = new Forms.WorkingFoldersForm();
                    _WorkingFoldersForm.ShowDialog();
                    break;

                case "Users":
                    Forms.UsersForm _UsersForm = new Forms.UsersForm();
                    _UsersForm.ShowDialog();
                    break;
            }


        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            string returnMessage = "";
            Data.GlovalVariables.transactionType = "New";

            try
            {
                switch (SSSTreeView.SelectedNode.StateImageKey)
                {
                    case "CustomersList":
                        Forms.CustomerForm _CustomerForm = new Forms.CustomerForm();
                        Cursor.Current = Cursors.WaitCursor;
                        _CustomerForm.ShowDialog();

                        foreach (string customerName in Data.GlovalVariables.newCustomersList)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Customer Information based of currentCustomerName Gloal variable value
                            URL = Data.GlovalVariables.BaseURL + "Customers/GetCustomerByName"; //?customerName=" + customerName;
                            urlParameters = "?customerName=" + customerName;
                            client.BaseAddress = new Uri(URL);
                            response = client.GetAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;


                                    ResultCustomers resultCustomers = JsonConvert.DeserializeObject<ResultCustomers>(returnMessage);
                                    if (resultCustomers.ReturnCode == 0)
                                    {
                                        Customer customer = new Customer();
                                        customer = resultCustomers.ReturnValue.First();
                                        TreeNode Node = new TreeNode();
                                        Node.Text = customer.CustomerName;
                                        Node.Tag = customer.CustomerID;
                                        Node.Name = customer.CustomerName;
                                        Node.ImageIndex = imageList.Images.IndexOfKey("Customer.png");
                                        Node.SelectedImageKey = "Customer.png";
                                        Node.StateImageKey = "Customer";
                                        Node.Nodes.Add("DummyNode");
                                        SSSTreeView.SelectedNode.Nodes.Add(Node);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        break;

                    case "Projects":
                        Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Tag);
                        Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Text;
                        Forms.ProjectForm _ProjectForm = new Forms.ProjectForm();                        
                        Cursor.Current = Cursors.WaitCursor;
                        _ProjectForm.ShowDialog();

                        foreach (string projectName in Data.GlovalVariables.newProjectsList)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Project Information based of currentProjectName Gloal variable value
                            URL = Data.GlovalVariables.BaseURL + "Projects/GetProjectByName"; 
                            urlParameters = "?projectName=" + projectName;
                            client.BaseAddress = new Uri(URL);
                            response = client.GetAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultProjects resultProjects = JsonConvert.DeserializeObject<ResultProjects>(returnMessage);
                                    if (resultProjects.ReturnCode == 0)
                                    {
                                        Project project = new Project();
                                        project = resultProjects.ReturnValue.First();
                                        TreeNode Node = new TreeNode();
                                        Node.Text = project.ProjectName;
                                        Node.Tag = project.ProjectID;
                                        Node.Name = project.ProjectName;
                                        Node.ImageIndex = imageList.Images.IndexOfKey("Project.png");
                                        Node.SelectedImageKey = "Project.png";
                                        Node.StateImageKey = "Project";
                                        Node.Nodes.Add("DummyNode");
                                        SSSTreeView.SelectedNode.Nodes.Add(Node);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                    case "Jobs":
                        Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Text;
                        Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Tag);
                        Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Text;
                        Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Tag);
                        Data.GlovalVariables.currentJobName = "";
                        Data.GlovalVariables.currentJobID = 0;
                        Forms.JobTypeForm _JobForm = new Forms.JobTypeForm();
                        Cursor.Current = Cursors.WaitCursor;
                        _JobForm.ShowDialog();
                        Cursor.Current = Cursors.Default;

                        foreach (string jobName in Data.GlovalVariables.newJobsList)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Job Information based of newJobsList Global Variable
                            URL = Data.GlovalVariables.BaseURL + "Jobs/GetJobByName";
                            urlParameters = "?jobName=" + jobName;
                            client.BaseAddress = new Uri(URL);
                            response = client.GetAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultJobsExtended resultJobs = JsonConvert.DeserializeObject<ResultJobsExtended>(returnMessage);
                                    if (resultJobs.ReturnCode == 0)
                                    {
                                        JobExtended job = new JobExtended();
                                        job = resultJobs.ReturnValue.First();
                                        TreeNode Node = new TreeNode();
                                        Node.Text = job.JobName;
                                        Node.Tag = job.JobID;
                                        Node.Name = job.JobName;
                                        Node.ImageIndex = imageList.Images.IndexOfKey("Job.png");
                                        Node.SelectedImageKey = "Job.png";
                                        Node.StateImageKey = "Job";
                                        Node.Nodes.Add("DummyNode");
                                        SSSTreeView.SelectedNode.Nodes.Add(Node);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                    case "Fields":
                        Data.GlovalVariables.currentCustomerName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Text;
                        Data.GlovalVariables.currentCustomerID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Parent.Parent.Tag);
                        Data.GlovalVariables.currentProjectName = SSSTreeView.SelectedNode.Parent.Parent.Parent.Text;
                        Data.GlovalVariables.currentProjectID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Parent.Parent.Tag);
                        Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Parent.Text;
                        Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Parent.Tag);
                        Data.GlovalVariables.currentFieldName = "";
                        Data.GlovalVariables.currentFieldID = 0;
                        Forms.FieldForm _FieldForm = new Forms.FieldForm();
                        _FieldForm.ShowDialog();

                        foreach (string fieldName in Data.GlovalVariables.newFieldsList)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Field Information based of newFieldsList Global Variable
                            URL = Data.GlovalVariables.BaseURL + "Fields/GetFieldsByNameAndJobID";
                            urlParameters = "?fieldName=" + fieldName + "&JobID=" + Data.GlovalVariables.currentJobID;
                            client.BaseAddress = new Uri(URL);
                            response = client.GetAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultFields resultFields = JsonConvert.DeserializeObject<ResultFields>(returnMessage);
                                    if (resultFields.ReturnCode == 0)
                                    {
                                        Field field = new Field();
                                        field = resultFields.ReturnValue.First();
                                        TreeNode Node = new TreeNode();
                                        Node.Text = field.CPFieldName;
                                        Node.Tag = field.FieldID;
                                        Node.Name = field.CPFieldName;
                                        if (string.IsNullOrEmpty(field.VFRFieldName.Trim()))
                                        {
                                            Node.ImageIndex = imageList.Images.IndexOfKey("CPField.png");
                                            Node.SelectedImageKey = "CPField.png";
                                        }
                                        else
                                        {
                                            Node.ImageIndex = imageList.Images.IndexOfKey("CPVFRField.png");
                                            Node.SelectedImageKey = "CPVFRField.png";
                                        }
                                        Node.StateImageKey = "Field";
                                        SSSTreeView.SelectedNode.Nodes.Add(Node);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                    case "ServiceStations":
                        Forms.ServiceStationsForm _ServiceStationsForm = new Forms.ServiceStationsForm();
                        _ServiceStationsForm.ShowDialog();

                        foreach (string serviceStation in Data.GlovalVariables.newServiceStaionList)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get Service Station Information based of newServiceStaionList Global Variable
                            URL = Data.GlovalVariables.BaseURL + "GeneralSettings/GetServiceStationByName";
                            urlParameters = "?stationName=" + serviceStation;
                            client.BaseAddress = new Uri(URL);
                            response = client.GetAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultServiceStations resultServiceStations = JsonConvert.DeserializeObject<ResultServiceStations>(returnMessage);
                                    if (resultServiceStations.ReturnCode == 0)
                                    {
                                        ServiceStation station = new ServiceStation();
                                        station = resultServiceStations.ReturnValue.First();
                                        TreeNode Node = new TreeNode();
                                        Node.Text = station.StationName;
                                        Node.Tag = station.StationID;
                                        Node.Name = station.StationName;                                       
                                        Node.ImageIndex = imageList.Images.IndexOfKey("ServiceStation.png");
                                        Node.SelectedImageKey = "ServiceStation.png";
                                        Node.StateImageKey = "ServiceStation";
                                        SSSTreeView.SelectedNode.Nodes.Add(Node);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                    default:
                        int xyz = SSSTreeView.SelectedImageIndex;
                        break;
                }
            }
            catch (Exception ex)
            {
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateListView(TreeNode e)
        {

            switch (e.StateImageKey)
            {
                case "CustomersList":
                    // SSSTreeView.Items.Add("Lock", "Lock");
                    // SSSTreeView.Items.Add("Synchronizer", "Synchronizer");
                    break;

                case "Host":

                    break;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            string returnMessage = "";
            Data.GlovalVariables.transactionType = "Delete";
            DialogResult result = new DialogResult();

            try
            {
                switch (SSSTreeView.SelectedNode.StateImageKey)
                {
                    case "Customer":
                        // Delete Confirmation
                        result = MessageBox.Show("Do you would like to remove customer " + SSSTreeView.SelectedNode.Text + "." + "\r\n" + "All the dependencies for this customer will be removed from the Database." , "Confirmation ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Customer Information based of currentCustomerName Gloal variable value
                            URL = Data.GlovalVariables.BaseURL + "Customers/DeleteCustomer";
                            urlParameters = "?customerID=" + SSSTreeView.SelectedNode.Tag;
                            client.BaseAddress = new Uri(URL);
                            response = client.DeleteAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;

                                    ResultCustomers resultCustomers = JsonConvert.DeserializeObject<ResultCustomers>(returnMessage);

                                    switch (resultCustomers.ReturnCode)
                                    {
                                        case 0:
                                            // Delete Project from the Tree
                                            SSSTreeView.SelectedNode.Remove();
                                            break;

                                        case -1:
                                            // Customer does not exist
                                            MessageBox.Show("Warning:" + "\r\n" + resultCustomers.Message.Replace(". ", "\r\n") + resultCustomers.Message, "Delete Customer Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }                            
                        }                        
                        break;

                    case "Project":
                        // Delete Confirmation
                        result = MessageBox.Show("Do you would like to remove project " + SSSTreeView.SelectedNode.Text + "." + "\r\n" + "All the dependencies for this project will be removed from the Database.", "Confirmation ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Project Information based of currentProjectName Gloal variable value
                            URL = Data.GlovalVariables.BaseURL + "Projects/DeleteProject";
                            urlParameters = "?projectID=" + SSSTreeView.SelectedNode.Tag;
                            client.BaseAddress = new Uri(URL);
                            response = client.DeleteAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;
                                    ResultProjects resultProjects = JsonConvert.DeserializeObject<ResultProjects>(returnMessage);

                                    switch (resultProjects.ReturnCode)
                                    {
                                        case 0:
                                            // Delete Project from the Tree
                                            SSSTreeView.SelectedNode.Remove();
                                            break;

                                        case -1:
                                            // Project does not exist
                                            MessageBox.Show("Warning:" + "\r\n" + resultProjects.Message.Replace(". ", "\r\n") + resultProjects.Message, "Delete Project Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                // Ignore transaction                           
                            }
                        }
                        break;

                    case "Job":
                        // Delete Confirmation
                        result = MessageBox.Show("Do you would like to remove job " + SSSTreeView.SelectedNode.Text + "." + "\r\n" + "All the dependencies for this job will be removed from the Database.", "Confirmation ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Project Information based of currentProjectName Gloal variable value
                            URL = Data.GlovalVariables.BaseURL + "Jobs/DeleteJob";
                            urlParameters = "?jobID=" + SSSTreeView.SelectedNode.Tag;
                            client.BaseAddress = new Uri(URL);
                            response = client.DeleteAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;
                                    ResultJobsExtended resultJobs = JsonConvert.DeserializeObject<ResultJobsExtended>(returnMessage);

                                    switch (resultJobs.ReturnCode)
                                    {
                                        case 0:
                                            // Delete Job from the Tree
                                            SSSTreeView.SelectedNode.Remove();
                                            break;

                                        case -1:
                                            // Project does not exist
                                            MessageBox.Show("Warning:" + "\r\n" + resultJobs.Message.Replace(". ", "\r\n") + resultJobs.Message, "Delete Job Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                    case "Field":
                        // Delete Confirmation
                        result = MessageBox.Show("Do you would like to remove Field " + SSSTreeView.SelectedNode.Text + "." + "\r\n" + "All the dependencies for this job will be removed from the Database.", "Confirmation ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            HttpClient client = new HttpClient();
                            client.Timeout = TimeSpan.FromMinutes(15);
                            // Get New Project Information based of currentProjectName Gloal variable value
                            URL = Data.GlovalVariables.BaseURL + "Fields/DeleteField";
                            urlParameters = "?fieldID=" + SSSTreeView.SelectedNode.Tag;
                            client.BaseAddress = new Uri(URL);
                            response = client.DeleteAsync(urlParameters).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;
                                    ResultFields resultFields = JsonConvert.DeserializeObject<ResultFields>(returnMessage);

                                    switch (resultFields.ReturnCode)
                                    {
                                        case 0:
                                            // Delete Field from the Tree
                                            SSSTreeView.SelectedNode.Remove();
                                            break;

                                        case -1:
                                            // Project does not exist
                                            MessageBox.Show("Warning:" + "\r\n" + resultFields.Message.Replace(". ", "\r\n") + resultFields.Message, "Delete Field Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                //ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SSSTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //foreach (TreeNode treeNode in e.Node.Nodes)
            //{
            //    treeNode.Remove();
            //}
        }

        private void importFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.JobFieldsImportForm _JobFieldsImportForm = new Forms.JobFieldsImportForm();
            _JobFieldsImportForm.ShowDialog();
        }

        private static int getProcessID(string processName)
        {
            int processID = 0;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string returnMessage;
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            ResultProcessTypes resultResultProcessTypes = new ResultProcessTypes();

            // Get Process Type ID based on process name
            URL = BaseURL + "Process/GetProcessTYpes";
            urlParameters = "";
            client.BaseAddress = new Uri(URL);
            response = client.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultResultProcessTypes = JsonConvert.DeserializeObject<ResultProcessTypes>(returnMessage);
            }
            if (response.IsSuccessStatusCode)
            {
                // Identify what is the Process ID 
                foreach (ProcessType process in resultResultProcessTypes.ReturnValue)
                {
                    if (process.Name.Trim() == processName)
                    {
                        processID = process.ProcessID;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return processID;
        }

        private static ScanningServicesDataObjects.GlobalVars.ResultProcesses GetProcess()
        {
            ResultProcesses resultProcesses = new ResultProcesses();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string returnMessage;
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();

            URL = Data.GlovalVariables.BaseURL + "Process/GetProcesses";
            client.BaseAddress = new Uri(URL);
            response = client.GetAsync(urlParameters).Result;
           
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (HttpContent content = response.Content)
                {
                    Task<string> resultTemp = content.ReadAsStringAsync();
                    returnMessage = resultTemp.Result;
                    //resultProcess = JsonConvert.DeserializeObject<ResultJobProcesses>(returnMessage);
                    resultProcesses = JsonConvert.DeserializeObject<ResultProcesses>(returnMessage);                   
                }
            }
            return resultProcesses;
        }

        private static string workFlowIconToDisplay(List<ScanningServicesDataObjects.GlobalVars.Process> processes, int jobID, string processName) 
        {
            string iconName = "";
            
            foreach (ScanningServicesDataObjects.GlobalVars.Process process in processes)
            {
                if (process.ProcessName.Trim() == processName && process.JobID == jobID )
                {
                    if (process.EnableFlag)
                    {
                        // This is when Cron has been configured and enabled for this Job
                        iconName = "GreeFilledCircle.png";
                        return iconName;
                    }
                    else
                    {
                        //iconName = "OutLineRedCircle.png";
                        //return iconName;
                        foreach (ScanningServicesDataObjects.GlobalVars.Process process_aux in processes)
                        {
                            if (process_aux.ProcessName.Trim() == processName && process_aux.JobID == 0 && process_aux.EnableFlag)
                            {
                                // This is when Cron has been configured but disable for this Job, and ALL Jobs Cron has been configured and enabled
                                iconName = "GreenFilledCircleAll.png";
                                return iconName;
                            }
                        }
                    }
                    
                }
            }
            foreach (ScanningServicesDataObjects.GlobalVars.Process process in processes)
            {
                if (process.ProcessName.Trim() == processName && process.JobID == 0 && process.EnableFlag)
                {
                    // This is when services is accesed from General Properties
                    iconName = "GreenFilledCircleAll.png";
                    return iconName;
                }
            }
            iconName = "OutLineRedCircle.png";
            return iconName;
        }

        private static string workFlowIconToDisplay1(int jobID, string processName) //, List<ScanningServicesDataObjects.GlobalVars.Process> process)
        {
            string iconName = "OutLineRedCircle.png";
            Boolean processFound = false;
            List<ScanningServicesDataObjects.GlobalVars.Process> process = new List<ScanningServicesDataObjects.GlobalVars.Process>();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string returnMessage;
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();

            URL = Data.GlovalVariables.BaseURL + "Process/GetProcessesByJobID?jobID=" + jobID.ToString();
            client.BaseAddress = new Uri(URL);
            response = client.GetAsync(urlParameters).Result;
            //ResultJobProcesses resultProcess = new ResultJobProcesses();
            ResultProcesses resultProcess = new ResultProcesses();
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (HttpContent content = response.Content)
                {
                    Task<string> resultTemp = content.ReadAsStringAsync();
                    returnMessage = resultTemp.Result;
                    //resultProcess = JsonConvert.DeserializeObject<ResultJobProcesses>(returnMessage);
                    resultProcess = JsonConvert.DeserializeObject<ResultProcesses>(returnMessage);
                }
                process = resultProcess.ReturnValue;
                foreach (ScanningServicesDataObjects.GlobalVars.Process item in process)
                {
                    if (item.ProcessName.Contains(processName))
                    {
                        processFound = true;
                        if (item.EnableFlag)
                        {
                            iconName = "GreeFilledCircle.png";
                        }
                        else
                        {
                            iconName = "FlorecentGreenCircle.png";
                        }
                        // The following condition force the iteration to be completed in the when the process name is Batch Delivery
                        if (processName != "Batch Delivery")
                        {
                            break;
                        }                           
                    }
                }
                if (!processFound)
                    iconName = "OutLineRedCircle.png";
            }  
            return iconName;
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            Forms.SSSMainForm _TitleSampleForm = new Forms.SSSMainForm();
            _TitleSampleForm.ShowDialog();
        }

        private void pageSizeCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentJobID = Convert.ToInt32(SSSTreeView.SelectedNode.Tag);
            Data.GlovalVariables.currentJobName = SSSTreeView.SelectedNode.Text;
            Forms.PageSizeCategoriesForm _PageSizeCategoriesForm = new Forms.PageSizeCategoriesForm();
            _PageSizeCategoriesForm.ShowDialog();
        }
    }
}
