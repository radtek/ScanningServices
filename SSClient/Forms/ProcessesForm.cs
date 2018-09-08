using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;
using static ScanningServicesAdmin.Data.GlovalVariables;
using Newtonsoft.Json;
using NLog;


// Process Database Fields
// Process ID       (Key) { use "0" when we refers to All Jobs }
// Station ID       (key) { anything but "0" }
// PDF Station ID   (key) { use "0" when NA }
// Job ID           (key) { use "0" when NA }
// Enable                 { true , false }
// Schedule               { string an a Json Format }

namespace ScanningServicesAdmin.Forms
{
    public partial class ProcessesForm : Form
    {
        public BindingSource bs = new BindingSource();

        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public ProcessesForm()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProcessForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = (currentProcessName + " Service Configuration").ToUpper();

                ResultJobsExtended resultJobs = new ResultJobsExtended();
                ResultProcesses resultProcess = new ResultProcesses();
                List<JobExtended> jobs = new List<JobExtended>();
                List<ServiceStation> serviceStations = new List<ServiceStation>();
                List<ServiceStation> pdfServiceStations = new List<ServiceStation>();

                // Load Process List View
                string returnMessage = "";
                Cursor.Current = Cursors.WaitCursor;

                string urlParameters = "";
                string URL = "";
                HttpResponseMessage response = new HttpResponseMessage();

                // Get Jobs    
                HttpClient client1 = new HttpClient();
                client1.Timeout = TimeSpan.FromMinutes(15);
                URL = BaseURL + "Jobs/GetJobs";
                urlParameters = "";
                client1.BaseAddress = new Uri(URL);
                response = client1.GetAsync(urlParameters).Result;
                using (HttpContent content = response.Content)
                {
                    Task<string> resultTemp = content.ReadAsStringAsync();
                    returnMessage = resultTemp.Result;
                    resultJobs = JsonConvert.DeserializeObject<ResultJobsExtended>(returnMessage);
                }
                if (response.IsSuccessStatusCode)
                {
                    jobs = resultJobs.ReturnValue;
                    // Add especial Job tothe Jobs List
                    JobExtended job = new JobExtended();
                    job.JobName = "ALL";
                    job.JobID = 0;
                    jobs.Add(job);
                    JobsComboBox.Items.Add("");
                    foreach (var item in jobs)
                    {
                        JobsComboBox.Items.Add(item.JobName.Trim());
                    }
                }
                else
                {
                    MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(Data.GlovalVariables.currentJobName))
                {
                    // This is the case where this template is called from a Job configuration
                    JobsComboBox.Enabled = false;
                    JobsComboBox.Text = Data.GlovalVariables.currentJobName.Trim();
                }

                // Get Service Stations
                HttpClient client2 = new HttpClient();
                client2.Timeout = TimeSpan.FromMinutes(15);
                ResultServiceStations resultResultStations = new ResultServiceStations();
                URL = BaseURL + "GeneralSettings/GetServiceStations";
                urlParameters = "";
                client2.BaseAddress = new Uri(URL);
                response = client2.GetAsync(urlParameters).Result;
                using (HttpContent content = response.Content)
                {
                    Task<string> resultTemp = content.ReadAsStringAsync();
                    returnMessage = resultTemp.Result;
                    resultResultStations = JsonConvert.DeserializeObject<ResultServiceStations>(returnMessage);
                }
                if (response.IsSuccessStatusCode)
                {
                    ServiceStationsComboBox.Items.Add("");
                    PDFServiceStationsComboBox.Items.Add("");
                    serviceStations = resultResultStations.ReturnValue;
                    foreach (ServiceStation serviceStation in serviceStations)
                    {
                        ServiceStationsComboBox.Items.Add(serviceStation.StationName.Trim());
                        // Identify if Station is use for PDF Conversion
                        if (serviceStation.PDFStationFlag)
                        {
                            pdfServiceStations.Add(serviceStation);
                            PDFServiceStationsComboBox.Items.Add(serviceStation.StationName.Trim());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                resultProcess = GetProcess();
                string enableFlag = "";
                string cronDescription = "";
                string cronJob = "";                

                // Table Definition - We will use this definition for Any Process in the System
                DataTable dt = new DataTable("Processes");
                dt.Columns.Add("ProcessID", typeof(string));
                dt.Columns.Add("JobID", typeof(string));
                dt.Columns.Add("StationID", typeof(string));
                dt.Columns.Add("PDFStationID", typeof(string));
               
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("CustomerName", typeof(string));
                dt.Columns.Add("JobName", typeof(string));
                dt.Columns.Add("ServiceStation", typeof(string));
                dt.Columns.Add("PDFServiceStation", typeof(string));
                dt.Columns.Add("Schedule", typeof(string));
                dt.Columns.Add("CronDescription", typeof(string));

                // Load Table Information
                // this condition is to support load balancing associated to FIle Conversion Stations
                if (Data.GlovalVariables.currentProcessName == "Load Balancer???")
                {
                    ServiceStationsComboBox.Enabled = true;
                    PDFServiceStationsComboBox.Enabled = true;
                    foreach (JobExtended job in jobs)
                    {                           
                        foreach (ServiceStation serviceStation in serviceStations)
                        {
                            foreach (ServiceStation pdfserviceStation in pdfServiceStations)
                            {
                                enableFlag = "No Configured";
                                cronDescription = "";
                                cronJob = "";
                                foreach (var process in resultProcess.ReturnValue)
                                {
                                    if (process.ProcessID.ToString() == Data.GlovalVariables.currentProcessID.ToString() && process.JobID.ToString() == job.JobID.ToString() &&
                                        process.StationID.ToString() == serviceStation.StationID.ToString() && process.PDFStationID.ToString() == pdfserviceStation.StationID.ToString())
                                    {
                                        enableFlag = Convert.ToString(process.EnableFlag);
                                        cronDescription = process.CronDescription;
                                        cronJob = process.ScheduleCronFormat;
                                        break;
                                    }
                                }

                                if (Data.GlovalVariables.currentJobName == job.JobName || string.IsNullOrEmpty(Data.GlovalVariables.currentJobName))
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["ProcessID"] = Data.GlovalVariables.currentProcessID.ToString();
                                    dr["JobID"] = job.JobID.ToString();
                                    dr["StationID"] = serviceStation.StationID.ToString();
                                    dr["PDFStationID"] = pdfserviceStation.StationID.ToString();
                                    dr["Status"] = enableFlag;
                                    if (job.JobName.Trim() == "ALL")
                                        dr["CustomerName"] = "";
                                    else
                                        dr["CustomerName"] = job.CustomerName.Trim();
                                    dr["JobName"] = job.JobName.Trim();
                                    dr["ServiceStation"] = serviceStation.StationName.Trim();
                                    dr["PDFServiceStation"] = pdfserviceStation.StationName.Trim();
                                    dr["Schedule"] = cronJob;
                                    dr["CronDescription"] = cronDescription;
                                    dt.Rows.Add(dr);
                                }                                
                            }
                        }
                    }
                }
                else
                {
                    ServiceStationsComboBox.Enabled = true;
                    PDFServiceStationsComboBox.Enabled = false;
                    string serviceStation = "";
                    string serviceStationID = "";
                    foreach (JobExtended job in jobs)
                    {
                        enableFlag = "No Configured";
                        serviceStation = "";
                        cronDescription = "";
                        cronJob = "";
                        foreach (var process in resultProcess.ReturnValue)
                        {
                            if (process.ProcessID.ToString() == Data.GlovalVariables.currentProcessID.ToString() && process.JobID.ToString() == job.JobID.ToString())
                            {
                                enableFlag = Convert.ToString(process.EnableFlag);
                                serviceStationID = process.StationID.ToString();
                                serviceStation = process.StationName;
                                cronDescription = process.CronDescription;
                                cronJob = process.ScheduleCronFormat;
                                break;
                            }
                        }
                        DataRow dr = dt.NewRow();
                        dr["ProcessID"] = Data.GlovalVariables.currentProcessID.ToString();
                        dr["JobID"] = job.JobID.ToString();
                        dr["StationID"] = serviceStationID;
                        //dr["PDFStationID"] = pdfserviceStation.StationID.ToString();                        
                        dr["Status"] = enableFlag;
                        if (job.JobName.Trim() == "ALL")
                            dr["CustomerName"] = "";
                        else
                            dr["CustomerName"] = job.CustomerName.Trim();
                        dr["JobName"] = job.JobName.Trim();
                        dr["ServiceStation"] = serviceStation.Trim();
                        //dr["PDFServiceStation"] = pdfserviceStation.StationName.Trim();
                        dr["Schedule"] = cronJob;
                        dr["CronDescription"] = cronDescription;
                        dt.Rows.Add(dr);
                    }
                }
                
                bs.DataSource = dt;

                // Define DataGrid
                // Filter Header based on Process Type { Synchronizer, Indexer, Load Balancer, Batch Delivery, etc }
                ProcessGridView.DataSource = dt;
                ProcessGridView.Columns["ProcessID"].Visible = false;
                ProcessGridView.Columns["JobID"].Visible = false;
                ProcessGridView.Columns["StationID"].Visible = false;
                ProcessGridView.Columns["PDFStationID"].Visible = false;
                ProcessGridView.Columns["CustomerName"].Visible = false;
                ProcessGridView.Columns["CustomerName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                ProcessGridView.Columns["CustomerName"].HeaderText = "Customer Name";
                ProcessGridView.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                ProcessGridView.Columns["Status"].ReadOnly = true;
                ProcessGridView.Columns["JobName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                ProcessGridView.Columns["JobName"].ReadOnly = true;
                ProcessGridView.Columns["JobName"].HeaderText = "Job Name";
                ProcessGridView.Columns["ServiceStation"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                ProcessGridView.Columns["ServiceStation"].ReadOnly = true;
                ProcessGridView.Columns["ServiceStation"].HeaderText = "Service Station";
                // The following condition is to be considered for future implementation
                if (Data.GlovalVariables.currentProcessName == "Load Balancer ???")
                {
                    ProcessGridView.Columns["PDFServiceStation"].Visible = true;
                }
                else
                {
                    ProcessGridView.Columns["PDFServiceStation"].Visible = false;
                }
                ProcessGridView.Columns["PDFServiceStation"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                ProcessGridView.Columns["PDFServiceStation"].ReadOnly = true;
                ProcessGridView.Columns["PDFServiceStation"].HeaderText = "PDF Service Station";
                ProcessGridView.Columns["Schedule"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                ProcessGridView.Columns["Schedule"].ReadOnly = true;
                ProcessGridView.Columns["CronDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                ProcessGridView.Columns["CronDescription"].ReadOnly = true;
                ProcessGridView.Columns["CronDescription"].HeaderText = "Cron Description";

                ProcessGridView.AllowUserToAddRows = false;
                ProcessGridView.RowHeadersVisible = true;
                ProcessGridView.AllowUserToAddRows = false;
                ProcessGridView.AllowUserToResizeRows = false;
                
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ResultProcesses GetProcessByIDs(int processID, int jobID, int stationID, int pdfStationID)
        {
            // GetProcessesByIDs?processID=4&jobID=1&stationID=2&pdfStationID=0
            string urlParameters = "";
            string URL = "";
            string returnMessage = "";
            
            ResultProcesses resultProcess = new ResultProcesses();
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            URL = BaseURL + "Process/GetProcessesByIDs";
            urlParameters = "?processID=" + processID.ToString() + "&jobID=" + jobID.ToString() + "&stationID=" + 
                             stationID.ToString() + "&pdfStationID=" + pdfStationID.ToString();
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
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultProcess;
        }

        private ResultProcesses GetProcess()
        {
            string urlParameters = "";
            string URL = "";
            string returnMessage = "";

            ResultProcesses resultProcess = new ResultProcesses();
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            URL = BaseURL + "Process/GetProcesses";
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
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultProcess;
        }

        private void SetScheduleButton_Click(object sender, EventArgs e)
        {
            
        }

       

        private void SaveButton_Click(object sender, EventArgs e)
        {

        }

        // WORK ON THIS METHOD TO SUPPORT OTHER PROCESSES ...
        private void ProcessGridView_DoubleClick(object sender, EventArgs e)
        {
            
            Data.GlovalVariables.currentJobID = Convert.ToInt32(ProcessGridView.CurrentRow.Cells[1].Value);
            Data.GlovalVariables.currentJobName = Convert.ToString(ProcessGridView.CurrentRow.Cells[6].Value);

            if (!string.IsNullOrEmpty(ProcessGridView.CurrentRow.Cells["StationID"].Value.ToString()))
                Data.GlovalVariables.currentServiceStationID = Convert.ToInt32(ProcessGridView.CurrentRow.Cells["StationID"].Value);
            else
                Data.GlovalVariables.currentServiceStationID = 0;
            Data.GlovalVariables.currentServiceStationName = Convert.ToString(ProcessGridView.CurrentRow.Cells["ServiceStation"].Value.ToString());

            if (!string.IsNullOrEmpty(ProcessGridView.CurrentRow.Cells["PDFStationID"].Value.ToString()))
                Data.GlovalVariables.currentPDFStationID = Convert.ToInt32(ProcessGridView.CurrentRow.Cells["PDFStationID"].Value);
            else
                Data.GlovalVariables.currentPDFStationID = 0;

            if (Convert.ToString(ProcessGridView.CurrentRow.Cells["Status"].Value).ToUpper() == "TRUE" ||
                Convert.ToString(ProcessGridView.CurrentRow.Cells["Status"].Value).ToUpper() == "FALSE")
                Data.GlovalVariables.transactionType = "Update";
            else
                Data.GlovalVariables.transactionType = "New";
            Forms.SchedulingForm _SchedulingForm = new Forms.SchedulingForm();
            _SchedulingForm.ShowDialog();

            //Refresh changes in Data Grid View
            if (Data.GlovalVariables.currentServiceStationID != 0)
            {
                ResultProcesses resultProcesses = new ResultProcesses();
                resultProcesses = GetProcessByIDs(Data.GlovalVariables.currentProcessID, Data.GlovalVariables.currentJobID,
                                                  Data.GlovalVariables.currentServiceStationID, Data.GlovalVariables.currentPDFStationID);
                if (resultProcesses.RecordsCount > 0)
                {
                    ProcessGridView.CurrentRow.Cells["Status"].Value = Convert.ToString(resultProcesses.ReturnValue[0].EnableFlag);
                    ProcessGridView.CurrentRow.Cells["Schedule"].Value = Convert.ToString(resultProcesses.ReturnValue[0].ScheduleCronFormat);
                    ProcessGridView.CurrentRow.Cells["CronDescription"].Value = Convert.ToString(resultProcesses.ReturnValue[0].CronDescription);
                    ProcessGridView.CurrentRow.Cells["ServiceStation"].Value = Convert.ToString(resultProcesses.ReturnValue[0].StationName);
                }
            }
            
        }


        private void FilterGridView()
        {
            string jobName = JobsComboBox.Text.Trim(); ;
            string stationName = ServiceStationsComboBox.Text.Trim();
            string pdfStationName = PDFServiceStationsComboBox.Text.Trim();
            //if (Data.GlovalVariables.currentProcessName == "Batch Delivery")
            //    bs.Filter = string.Format("JobName LIKE '%{0}%' AND ServiceStation LIKE '%{1}%' AND PDFServiceStation LIKE '%{2}%'", jobName, stationName, pdfStationName);
            //else
            //    bs.Filter = string.Format("JobName LIKE '%{0}%' AND ServiceStation LIKE '%{1}%'", jobName, stationName);
            bs.Filter = string.Format("JobName LIKE '%{0}%' AND ServiceStation LIKE '%{1}%'", jobName, stationName);
        }

        private void JobsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGridView();
        }

        private void ServiceStationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGridView();
        }

        private void PDFServiceStationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGridView();
        }

        private void DisplayCustomerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DisplayCustomerCheckBox.Checked)
                ProcessGridView.Columns["CustomerName"].Visible = true;
            else
                ProcessGridView.Columns["CustomerName"].Visible = false;
        }
    }
}
