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
using System.Net.Http.Headers;
using System.IO;

namespace ScanningServicesAdmin.Forms
{
    public partial class JobTypeForm : Form
    {
        public  JobExtended originalJob = new JobExtended();

        public JobTypeForm()
        {
            InitializeComponent();

            ResultWorkingFolders resultWorkingFolders = new ResultWorkingFolders();
            resultWorkingFolders = GetWorkingFolders();

            ComboboxItem firstItem = new ComboboxItem();
            firstItem.Text = "";
            firstItem.ID = 0;
            AutoImportWatchFolderComboBox.Items.Add(firstItem);
            //ScanningFolderComboBox.Items.Add(firstItem);
            PostValidationComboBox.Items.Add(firstItem);
            LoadBalancerWatchFolderComboBox.Items.Add(firstItem);
            BackupFolderComboBox.Items.Add(firstItem);
            //FileConversionWatchFolderComboBox.Items.Add(firstItem);
            BatchDeliveryWatchFolderComboBox.Items.Add(firstItem);
            //BatchFinalLocationComboBox.Items.Add(firstItem);
            //VFRRenamerWatchFolderComboBox.Items.Add(firstItem);
            //VFRDuplicateRemoverWatchFolderComboBox.Items.Add(firstItem);
            //VFRBatchMonitorComboBox.Items.Add(firstItem);
            //VFRBatchUploaderComboBox.Items.Add(firstItem);
            QCOutputWatchFolderComboBox.Items.Add(firstItem);

            if (resultWorkingFolders.ReturnCode == 0)
            {
                foreach (WorkingFolder folder in resultWorkingFolders.ReturnValue)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = folder.Path;
                    item.ID = folder.FolderID;
                    AutoImportWatchFolderComboBox.Items.Add(item);
                    ScanningFolderComboBox.Items.Add(item);
                    PostValidationComboBox.Items.Add(item);
                    LoadBalancerWatchFolderComboBox.Items.Add(item);
                    BackupFolderComboBox.Items.Add(item);
                    //FileConversionWatchFolderComboBox.Items.Add(item);
                    BatchDeliveryWatchFolderComboBox.Items.Add(item);
                    BatchFinalLocationComboBox.Items.Add(item);
                    //VFRRenamerWatchFolderComboBox.Items.Add(item);
                    //VFRDuplicateRemoverWatchFolderComboBox.Items.Add(item);
                    //VFRBatchMonitorComboBox.Items.Add(item);
                    //VFRBatchUploaderComboBox.Items.Add(item);
                    QCOutputWatchFolderComboBox.Items.Add(item);
                }
            }

            AutoImportWatchFolderComboBox.Visible = false;
            //ScanningFolderComboBox.Visible = false;
            PostValidationComboBox.Visible = false;
            LoadBalancerWatchFolderComboBox.Visible = false;
            BackupFolderComboBox.Visible = false;
            //FileConversionWatchFolderComboBox.Visible = false;
            BatchDeliveryWatchFolderComboBox.Visible = true;
            //BatchFinalLocationComboBox.Visible = false;
            //VFRRenamerWatchFolderComboBox.Visible = false;
            //VFRDuplicateRemoverWatchFolderComboBox.Visible = false;
            //VFRBatchMonitorComboBox.Visible = false;
            //VFRBatchUploaderComboBox.Visible = false;
            QCOutputWatchFolderComboBox.Visible = true;

            // These Item are mandatory in the Scanning Process
            IncludeQCandOutputCheckBox.Checked = true;
            IncludeScanningCheckBox.Checked = true;
            IncludeBatchDeliveryCheckBox.Checked = true;
            IncludeFinalLocationCheckBox.Checked = true;

            newJobsList.Clear();
            CustomerNameTextBox.Text = Data.GlovalVariables.currentCustomerName;
            ProjectNameTextBox.Text = Data.GlovalVariables.currentProjectName;

            ResultJobsExtended resultJobs = new ResultJobsExtended();

            switch (Data.GlovalVariables.transactionType)
            {
                case "Update":
                    
                    JobNameTextBox.Text = Data.GlovalVariables.currentJobName;
                    //originalJob.JobName = Data.GlovalVariables.currentJobName;
                    resultJobs = GetJobInformation(JobNameTextBox.Text);

                    if (resultJobs.RecordsCount > 0)
                    {
                        MaxNumberBatchesPerWOUpDown.Text = resultJobs.ReturnValue[0].MaxBatchesPerWorkOrder.ToString();
                        CustomerNameTextBox.Text = resultJobs.ReturnValue[0].CustomerName;
                        JobNameTextBox.Text = resultJobs.ReturnValue[0].JobName;
                        DepartmentTextBox.Text = resultJobs.ReturnValue[0].DepartmentName;
                        ProjectNameTextBox.Text = resultJobs.ReturnValue[0].ProjectName;
                        ExportClassNameTextBox.Text = resultJobs.ReturnValue[0].ExportClassName;
                        if (resultJobs.ReturnValue[0].OutputFileType.Trim() == "PDF")
                        {
                            PDFRadioButton.Checked = true;
                        }                            
                        else
                        {
                            if (resultJobs.ReturnValue[0].OutputFileType.Trim() == "Searchable PDF")
                            {
                                SearchablePDFRadioButton.Checked = true;
                            }
                            else
                            {
                                TIFRadioButton.Checked = true;
                            }                                
                        }                            
                        if (resultJobs.ReturnValue[0].MultiPageFlag)
                            MultiPageRadioButton.Checked = true;
                        else
                            SinglePageRadioButton.Checked = true;

                        ScanningFolderComboBox.Text = resultJobs.ReturnValue[0].ScanningFolder;
                        IncludeAutoImportCheckBox.Checked = resultJobs.ReturnValue[0].AutoImportEnableFlag;
                        AutoImportWatchFolderComboBox.Text = resultJobs.ReturnValue[0].AutoImportWatchFolder;
                        IncludePostValidationCheckBox.Checked = resultJobs.ReturnValue[0].PostValidationEnableFlag;
                        PostValidationComboBox.Text = resultJobs.ReturnValue[0].PostValidationWatchFolder;
                        //IncludeLoadBalancerCheckBox.Checked = resultJobs.ReturnValue[0].LoadBalancerEnableFlag;
                        IncludePDFConversionCheckBox.Checked = resultJobs.ReturnValue[0].FileConversionEnableFlag;
                        LoadBalancerWatchFolderComboBox.Text = resultJobs.ReturnValue[0].LoadBalancerWatchFolder;
                        BackupFolderComboBox.Text = resultJobs.ReturnValue[0].BackupFolder;
                        
                        //FileConversionWatchFolderComboBox.Text = resultJobs.ReturnValue[0].FileConversionWatchFolder;
                        //IncludeBatchDeliveryCheckBox.Checked = resultJobs.ReturnValue[0].BatchDeliveryEnableFlag;
                        BatchDeliveryWatchFolderComboBox.Text = resultJobs.ReturnValue[0].BatchDeliveryWatchFolder;
                        BatchFinalLocationComboBox.Text = resultJobs.ReturnValue[0].RestingLocation;
                        IncludeVFRCheckBox.Checked = resultJobs.ReturnValue[0].VFREnableFlag;
                        //VFRRenamerWatchFolderComboBox.Text = resultJobs.ReturnValue[0].VFRRenamerWatchFolder;
                        //VFRDuplicateRemoverWatchFolderComboBox.Text = resultJobs.ReturnValue[0].VFRDuplicateRemoverWatchFolder;
                        //VFRBatchUploaderComboBox.Text = resultJobs.ReturnValue[0].VFRBatchUploaderWatchFolder;
                        //VFRBatchMonitorComboBox.Text = resultJobs.ReturnValue[0].VFRBatchMonitorFolder;
                        QCOutputWatchFolderComboBox.Text = resultJobs.ReturnValue[0].QCOuputFolder;
                    }
                    originalJob.MaxBatchesPerWorkOrder = resultJobs.ReturnValue[0].MaxBatchesPerWorkOrder;
                    originalJob.JobID = resultJobs.ReturnValue[0].JobID;
                    originalJob.ProjectID = resultJobs.ReturnValue[0].ProjectID;
                    originalJob.CustomerName = CustomerNameTextBox.Text;
                    originalJob.JobName = JobNameTextBox.Text;
                    originalJob.DepartmentName = DepartmentTextBox.Text;
                    originalJob.ProjectName = ProjectNameTextBox.Text;
                    originalJob.ExportClassName = ExportClassNameTextBox.Text;
                    originalJob.OutputFileType = resultJobs.ReturnValue[0].OutputFileType;
                    originalJob.MultiPageFlag = resultJobs.ReturnValue[0].MultiPageFlag;
                    originalJob.ScanningFolder = ScanningFolderComboBox.Text;
                    originalJob.ScanningFolderID = resultJobs.ReturnValue[0].ScanningFolderID;
                    originalJob.PostValidationWatchFolder = ScanningFolderComboBox.Text;
                    originalJob.PostValidationWatchFolderID = resultJobs.ReturnValue[0].ScanningFolderID;
                    originalJob.PostValidationEnableFlag = IncludePostValidationCheckBox.Checked;
                    originalJob.AutoImportEnableFlag = IncludeAutoImportCheckBox.Checked;
                    originalJob.AutoImportWatchFolder = resultJobs.ReturnValue[0].AutoImportWatchFolder;
                    originalJob.AutoImportWatchFolderID = resultJobs.ReturnValue[0].AutoImportWatchFolderID;
                    originalJob.QCOuputFolder = resultJobs.ReturnValue[0].QCOuputFolder;
                    originalJob.QCOuputFolderID = resultJobs.ReturnValue[0].QCOuputFolderID;
                    //originalJob.LoadBalancerEnableFlag = IncludeLoadBalancerCheckBox.Checked;
                    originalJob.LoadBalancerWatchFolder = resultJobs.ReturnValue[0].LoadBalancerWatchFolder;
                    originalJob.LoadBalancerWatchFolderID = resultJobs.ReturnValue[0].LoadBalancerWatchFolderID;
                    originalJob.BackupFolder = resultJobs.ReturnValue[0].BackupFolder;
                    originalJob.BackupFolderID = resultJobs.ReturnValue[0].BackupFolderID;
                    originalJob.FileConversionEnableFlag = IncludePDFConversionCheckBox.Checked;
                    originalJob.FileConversionWatchFolder = resultJobs.ReturnValue[0].FileConversionWatchFolder;
                    originalJob.FileConversionWatchFolderID = resultJobs.ReturnValue[0].FileConversionWatchFolderID;
                    //originalJob.BatchDeliveryEnableFlag = IncludeBatchDeliveryCheckBox.Checked;
                    originalJob.BatchDeliveryWatchFolder = resultJobs.ReturnValue[0].BatchDeliveryWatchFolder;
                    originalJob.BatchDeliveryWatchFolderID = resultJobs.ReturnValue[0].BatchDeliveryWatchFolderID;
                    originalJob.RestingLocation =  BatchFinalLocationComboBox.Text;
                    originalJob.RestingLocationID = resultJobs.ReturnValue[0].RestingLocationID;
                    originalJob.VFREnableFlag = IncludeVFRCheckBox.Checked;
                    originalJob.VFRRenamerWatchFolder = resultJobs.ReturnValue[0].VFRRenamerWatchFolder;
                    originalJob.VFRRenamerWatchFolderID = resultJobs.ReturnValue[0].VFRRenamerWatchFolderID;
                    originalJob.VFRDuplicateRemoverWatchFolder = resultJobs.ReturnValue[0].VFRDuplicateRemoverWatchFolder;
                    originalJob.VFRDuplicateRemoverWatchFolderID = resultJobs.ReturnValue[0].VFRDuplicateRemoverWatchFolderID;
                    originalJob.VFRBatchUploaderWatchFolder = resultJobs.ReturnValue[0].VFRBatchUploaderWatchFolder;
                    originalJob.VFRBatchUploaderWatchFolderID = resultJobs.ReturnValue[0].VFRBatchUploaderWatchFolderID;
                    originalJob.VFRBatchMonitorFolder = resultJobs.ReturnValue[0].VFRBatchMonitorFolder;
                    originalJob.VFRBatchMonitorFolderID = resultJobs.ReturnValue[0].VFRBatchMonitorFolderID;

                    break;

                case "New":
                    originalJob.MaxBatchesPerWorkOrder = 15;
                    MaxNumberBatchesPerWOUpDown.Text = "15";
                    originalJob.ProjectID = Data.GlovalVariables.currentProjectID;
                    originalJob.CustomerName = Data.GlovalVariables.currentCustomerName;
                    originalJob.JobName = "";
                    originalJob.DepartmentName = "";
                    originalJob.ProjectName = Data.GlovalVariables.currentProjectName;
                    originalJob.ExportClassName = "";
                    originalJob.OutputFileType = "Searchable PDF";
                    MultiPageRadioButton.Checked = true;
                    SearchablePDFRadioButton.Checked = true;
                    originalJob.MultiPageFlag = true;
                    originalJob.AutoImportEnableFlag = false;
                    originalJob.PostValidationEnableFlag = false;
                    //originalJob.LoadBalancerEnableFlag = false;
                    originalJob.FileConversionEnableFlag = false;
                    //originalJob.BatchDeliveryEnableFlag = false;
                    originalJob.VFREnableFlag = false;

                    // Get defaault path from General Settings
                    ResultGeneralSettingsExtended resultSettings = new ResultGeneralSettingsExtended();
                    resultSettings = GetGeneralSettings();
                    if (resultSettings.RecordsCount > 0)
                    {
                        ScanningFolderComboBox.Text = resultSettings.ReturnValue.ScanningFolder;
                        AutoImportWatchFolderComboBox.Text = resultSettings.ReturnValue.AutoImportWatchFolder;
                        PostValidationComboBox.Text = resultSettings.ReturnValue.PostValidationWatchFolder;
                        LoadBalancerWatchFolderComboBox.Text = resultSettings.ReturnValue.LoadBalancerWatchFolder;
                        BackupFolderComboBox.Text = resultSettings.ReturnValue.BackupFolder;
                        //FileConversionWatchFolderComboBox.Text = resultSettings.ReturnValue.FileConversionWatchFolder;
                        BatchDeliveryWatchFolderComboBox.Text = resultSettings.ReturnValue.BatchDeliveryWatchFolder;
                        BatchFinalLocationComboBox.Text = resultSettings.ReturnValue.RestingLocation;
                        //VFRRenamerWatchFolderComboBox.Text = resultSettings.ReturnValue.VFRRenamerWatchFolder;
                        //VFRDuplicateRemoverWatchFolderComboBox.Text = resultSettings.ReturnValue.VFRDuplicateRemoverWatchFolder;
                        //VFRBatchUploaderComboBox.Text = resultSettings.ReturnValue.VFRBatchUploaderWatchFolder;
                        //VFRBatchMonitorComboBox.Text = resultSettings.ReturnValue.VFRBatchMonitorFolder;
                        QCOutputWatchFolderComboBox.Text = resultSettings.ReturnValue.QCOutputFolder;

                        originalJob.ScanningFolder = resultSettings.ReturnValue.ScanningFolder;
                        originalJob.ScanningFolderID = resultSettings.ReturnValue.ScanningFolderID;
                        originalJob.PostValidationWatchFolder = resultSettings.ReturnValue.PostValidationWatchFolder;
                        originalJob.PostValidationWatchFolderID = resultSettings.ReturnValue.PostValidationWatchFolderID;
                        originalJob.AutoImportWatchFolder = resultSettings.ReturnValue.AutoImportWatchFolder;
                        originalJob.AutoImportWatchFolderID = resultSettings.ReturnValue.AutoImportWatchFolderID;
                        originalJob.LoadBalancerWatchFolder = resultSettings.ReturnValue.LoadBalancerWatchFolder;
                        originalJob.LoadBalancerWatchFolderID = resultSettings.ReturnValue.LoadBalancerWatchFolderID;
                        originalJob.BackupFolder = resultSettings.ReturnValue.BackupFolder; 
                        originalJob.BackupFolderID = resultSettings.ReturnValue.BackupFolderID;
                        originalJob.FileConversionWatchFolder = resultSettings.ReturnValue.FileConversionWatchFolder;
                        originalJob.FileConversionWatchFolderID = resultSettings.ReturnValue.FileConversionWatchFolderID;
                        originalJob.BatchDeliveryWatchFolder = resultSettings.ReturnValue.BatchDeliveryWatchFolder;
                        originalJob.BatchDeliveryWatchFolderID = resultSettings.ReturnValue.BatchDeliveryWatchFolderID;
                        originalJob.RestingLocation = resultSettings.ReturnValue.RestingLocation;
                        originalJob.RestingLocationID = resultSettings.ReturnValue.RestingLocationID;
                        originalJob.VFRRenamerWatchFolder = resultSettings.ReturnValue.VFRRenamerWatchFolder;
                        originalJob.VFRRenamerWatchFolderID = resultSettings.ReturnValue.VFRRenamerWatchFolderID;
                        originalJob.VFRDuplicateRemoverWatchFolder = resultSettings.ReturnValue.VFRDuplicateRemoverWatchFolder;
                        originalJob.VFRDuplicateRemoverWatchFolderID = resultSettings.ReturnValue.VFRDuplicateRemoverWatchFolderID;
                        originalJob.VFRBatchUploaderWatchFolder = resultSettings.ReturnValue.VFRBatchUploaderWatchFolder;
                        originalJob.VFRBatchUploaderWatchFolderID = resultSettings.ReturnValue.VFRBatchUploaderWatchFolderID;
                        originalJob.VFRBatchMonitorFolder = resultSettings.ReturnValue.VFRBatchMonitorFolder;
                        originalJob.VFRBatchMonitorFolderID = resultSettings.ReturnValue.VFRBatchMonitorFolderID;
                        originalJob.QCOuputFolderID = resultSettings.ReturnValue.QCOutputFolderID;
                    }
                    else
                    {
                        originalJob.ScanningFolder = "";
                        originalJob.ScanningFolderID = 0;
                        originalJob.AutoImportWatchFolder = "";
                        originalJob.PostValidationWatchFolder = "";
                        originalJob.PostValidationWatchFolderID = 0;
                        originalJob.AutoImportWatchFolderID = 0;
                        originalJob.LoadBalancerWatchFolder = "";
                        originalJob.LoadBalancerWatchFolderID = 0;
                        originalJob.BackupFolder = "";
                        originalJob.BackupFolderID = 0;
                        originalJob.FileConversionWatchFolder = "";
                        originalJob.FileConversionWatchFolderID = 0;
                        originalJob.BatchDeliveryWatchFolder = "";
                        originalJob.BatchDeliveryWatchFolderID = 0;
                        originalJob.RestingLocation = "";
                        originalJob.RestingLocationID = 0;
                        originalJob.VFRRenamerWatchFolder = "";
                        originalJob.VFRRenamerWatchFolderID = 0;
                        originalJob.VFRDuplicateRemoverWatchFolder = "";
                        originalJob.VFRDuplicateRemoverWatchFolderID = 0;
                        originalJob.VFRBatchUploaderWatchFolder = "";
                        originalJob.VFRBatchUploaderWatchFolderID = 0;
                        originalJob.VFRBatchMonitorFolder = "";
                        originalJob.VFRBatchMonitorFolderID = 0;
                        originalJob.QCOuputFolderID = 0;
                    }
                    break;
            }
            if (IncludePDFConversionCheckBox.Checked)
            {
                BatchDeliveryMessage2.Show();
                BatchDeliveryMessage1.Show();
                BatchDeliveryWatchFolderComboBox.Hide();
                BatchDeliveryWatchFolderComboBox.Text = "";
            }
            else
            {
                BatchDeliveryMessage2.Hide();
                BatchDeliveryMessage1.Hide();
                BatchDeliveryWatchFolderComboBox.Show();
            }
            if (!IncludeAutoImportCheckBox.Checked)
            {
                AutoImportWatchFolderComboBox.Text = "";
            }
            if (IncludePostValidationCheckBox.Checked)
            {
                PostValidationComboBox.Text = "";
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public int ID { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private ResultGeneralSettingsExtended GetGeneralSettings()
        {
            string urlParameters = "";
            string URL = "";
            string returnMessage = "";

            ResultGeneralSettingsExtended resultSettings = new ResultGeneralSettingsExtended();
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            URL = BaseURL + "GeneralSettings/GetGeneralSettingsInfo";
            urlParameters = "";
            client.BaseAddress = new Uri(URL);
            response = client.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultSettings = JsonConvert.DeserializeObject<ResultGeneralSettingsExtended>(returnMessage);
            }
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultSettings;
        }

        private ResultJobsExtended GetJobInformation(string jobName)
        {
            string urlParameters = "";
            string URL = "";
            string returnMessage = "";

            ResultJobsExtended resultJobs = new ResultJobsExtended();
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            URL = BaseURL + "Jobs/GetJobByName"; //?customerName=" + customerName;
            urlParameters = "?jobName=" + jobName;
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
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultJobs;
        }

        private ResultWorkingFolders GetWorkingFolders()
        {
            string urlParameters = "";
            string URL = "";
            string returnMessage = "";

            ResultWorkingFolders resultWorkingFolders = new ResultWorkingFolders();
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            URL = BaseURL + "GeneralSettings/GetWorkingFolders";
            urlParameters = "";
            client.BaseAddress = new Uri(URL);
            response = client.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultWorkingFolders = JsonConvert.DeserializeObject<ResultWorkingFolders>(returnMessage);
            }
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                        "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return resultWorkingFolders;
        }

        private void IncludeAutoImportCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IncludeAutoImportCheckBox.Checked)
            {
                AutoImportWatchFolderComboBox.Visible = true;
                AutoImportWatchFolderComboBox.Enabled = true;
                //AutoImportWatchFolderComboBox.Text = ScanningFolderComboBox.Text;               
            }
            else
            {
                AutoImportWatchFolderComboBox.Visible = false;
                AutoImportWatchFolderComboBox.Enabled = false;
                AutoImportWatchFolderComboBox.Text = "";
            }
        }

        private void IncludePostValidationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IncludePostValidationCheckBox.Checked)
            {
                PostValidationComboBox.Visible = true;
                PostValidationComboBox.Text = QCOutputWatchFolderComboBox.Text;
            }
            else
            {
                PostValidationComboBox.Visible = false;
                PostValidationComboBox.Text = "";
            }
                
        }

        private void IncludePDFConversionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IncludePDFConversionCheckBox.Checked)
            {
                //FileConversionWatchFolderComboBox.Visible = true;
                LoadBalancerWatchFolderComboBox.Visible = true;
                BackupFolderComboBox.Visible = true;
                BatchDeliveryMessage2.Show();
                BatchDeliveryMessage1.Show();
                BatchDeliveryWatchFolderComboBox.Hide();
                BatchDeliveryWatchFolderComboBox.Text = "";
            }
            else
            {
                //FileConversionWatchFolderComboBox.Visible = false;
                LoadBalancerWatchFolderComboBox.Visible = false;
                BackupFolderComboBox.Visible = false;
                BatchDeliveryMessage2.Hide();
                BatchDeliveryMessage1.Hide();
                BatchDeliveryWatchFolderComboBox.Show();

            }   
            if (!IncludePostValidationCheckBox.Checked)
            {
                LoadBalancerWatchFolderComboBox.Text = QCOutputWatchFolderComboBox.Text;
            }

        }

        private void IncludeBatchDeliveryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IncludeBatchDeliveryCheckBox.Checked)
                BatchDeliveryWatchFolderComboBox.Visible = true;
            else
                BatchDeliveryWatchFolderComboBox.Visible = false;
        }

        private void IncludeVFRCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IncludeVFRCheckBox.Checked)
            {
                //VFRRenamerWatchFolderComboBox.Visible = true;
                //VFRBatchMonitorComboBox.Visible = true;
                //VFRBatchUploaderComboBox.Visible = true;
                //VFRDuplicateRemoverWatchFolderComboBox.Visible = true;
            }

            else
            {
                //VFRRenamerWatchFolderComboBox.Visible = false;
                //VFRBatchMonitorComboBox.Visible = false;
                //VFRBatchUploaderComboBox.Visible = false;
                //VFRDuplicateRemoverWatchFolderComboBox.Visible = false;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void JobTypeForm_Load(object sender, EventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save("SaveAndExit");
        }

        private void ApplyHutton_Click(object sender, EventArgs e)
        {           
                Save("Save");
        }

        private void Save(string action)
        {
            Job job = new Job();

            // Validation rules
            if (JobNameTextBox.Text.Length == 0)
            {
                toolTip1.ToolTipTitle = "Missing Value";
                toolTip1.Show("You must provide values for Job Name,", JobNameTextBox, 5000);
                ScanningFolderComboBox.Focus();
                ScanningFolderComboBox.Select();
                return;
            }


            // Check Auto-Import Watch Information
            if (IncludeAutoImportCheckBox.Checked)
            {
                if (AutoImportWatchFolderComboBox.Text.Length == 0)
                {
                    toolTip1.ToolTipTitle = "Missing Value";
                    toolTip1.Show("When this option is checked, you must provide values for the Auto Import Watch Folder.", ScanningFolderComboBox, 5000);
                    ScanningFolderComboBox.Focus();
                    ScanningFolderComboBox.Select();
                    return;
                }
                else
                {
                    job.AutoImportWatchFolderID = ((ComboboxItem)AutoImportWatchFolderComboBox.SelectedItem).ID;
                }
            }
            else
            {
                job.AutoImportWatchFolderID = 0;
            }

            // Check Scanning Information
            if (IncludeScanningCheckBox.Checked)
            {
                if (ScanningFolderComboBox.Text.Length == 0)
                {
                    toolTip1.ToolTipTitle = "Missing Value";
                    toolTip1.Show("When this option is checked, you must provide values for Scanning Folder.", ScanningFolderComboBox, 5000);
                    ScanningFolderComboBox.Focus();
                    ScanningFolderComboBox.Select();
                    return;
                }
                else
                {
                    job.AutoImportEnableFlag = true;
                    job.ScanningFolderID = ((ComboboxItem)ScanningFolderComboBox.SelectedItem).ID;
                }
            }
            else
            {
                job.AutoImportEnableFlag = false;
                job.ScanningFolderID = 0;
            }


            // Check QC and Ouput Folder Information
            if (IncludeQCandOutputCheckBox.Checked)
            {
                if (QCOutputWatchFolderComboBox.Text.Length == 0)
                {
                    toolTip1.ToolTipTitle = "Missing Value";
                    toolTip1.Show("When this option is checked, you must provide value a for QC and Output Folder.", QCOutputWatchFolderComboBox, 5000);
                    QCOutputWatchFolderComboBox.Focus();
                    QCOutputWatchFolderComboBox.Select();
                    return;
                }
                else
                {
                    job.QCOuputFolderID = ((ComboboxItem)QCOutputWatchFolderComboBox.SelectedItem).ID;
                }
            }
            else
            {
                job.QCOuputFolderID = 0;
            }

            // Check Post Validation Information
            if (IncludePostValidationCheckBox.Checked)
            {
                if (PostValidationComboBox.Text.Length == 0)
                {
                    toolTip1.ToolTipTitle = "Missing Value";
                    toolTip1.Show("When this option is checked, you must provide value a for Post Validation Watch Folder.", PostValidationComboBox, 5000);
                    PostValidationComboBox.Focus();
                    PostValidationComboBox.Select();
                    return;
                }
                else
                {
                    job.PostValidationEnableFlag = true;
                    job.PostValidationWatchFolderID = ((ComboboxItem)AutoImportWatchFolderComboBox.SelectedItem).ID;
                }
            }
            else
            {
                job.PostValidationEnableFlag = false;
                job.PostValidationWatchFolderID = 0;
            }

            // Check Load Balancer Information
            //if (IncludeLoadBalancerCheckBox.Checked)
            //{
            //    if (LoadBalancerWatchFolderComboBox.Text.Length == 0)
            //    {
            //        toolTip1.ToolTipTitle = "Missing Value";
            //        toolTip1.Show("When this option is checked, you must provide a value for Load Balancer Watch Folder.", LoadBalancerWatchFolderComboBox, 5000);
            //        LoadBalancerWatchFolderComboBox.Focus();
            //        LoadBalancerWatchFolderComboBox.Select();
            //        return;
            //    }
            //    else
            //    {
            //        job.LoadBalancerEnableFlag = true;
            //        job.LoadBalancerWatchFolderID = ((ComboboxItem)AutoImportWatchFolderComboBox.SelectedItem).ID;
            //        if (BackupFolderComboBox.Text.Length == 0)
            //        {
            //            job.BackupFolderID = 0;
            //        }
            //        else
            //        {
            //            job.BackupFolderID = ((ComboboxItem)BackupFolderComboBox.SelectedItem).ID;
            //        }
            //    }
            //}
            //else
            //{
            //    job.LoadBalancerEnableFlag = false;
            //    job.LoadBalancerWatchFolderID = 0;
            //}

            // Check File Conversion Information
            //if (IncludePDFConversionCheckBox.Checked)
            //{
            //    if (FileConversionWatchFolderComboBox.Text.Length == 0)
            //    {
            //        toolTip1.ToolTipTitle = "Missing Value";
            //        toolTip1.Show("When this option is checked, you must provide value a for File Conversion.", FileConversionWatchFolderComboBox, 5000);
            //        FileConversionWatchFolderComboBox.Focus();
            //        FileConversionWatchFolderComboBox.Select();
            //        return;
            //    }
            //    else
            //    {
            //        job.FileConversionEnableFlag = true;
            //        job.FileConversionWatchFolderID = ((ComboboxItem)FileConversionWatchFolderComboBox.SelectedItem).ID;
            //    }
            //}
            //else
            //{
            //    job.FileConversionEnableFlag = false;
            //    job.FileConversionWatchFolderID = 0;
            //}

            // Check Batch Delivery Information
            if (IncludeBatchDeliveryCheckBox.Checked)
            {
                if (BatchDeliveryWatchFolderComboBox.Text.Length == 0)
                {
                    if (!IncludePDFConversionCheckBox.Checked)
                    {
                        toolTip1.ToolTipTitle = "Missing Value";
                        toolTip1.Show("When this option is checked, you must provide value a for Batch Delivery.", BatchDeliveryWatchFolderComboBox, 5000);
                        BatchDeliveryWatchFolderComboBox.Focus();
                        BatchDeliveryWatchFolderComboBox.Select();
                        return;
                    }
                    else
                    {
                        job.BatchDeliveryWatchFolderID = 0;
                    }                        
                }
                else
                {
                    if (!IncludePostValidationCheckBox.Checked && !IncludePDFConversionCheckBox.Checked)
                    {
                        if (BatchDeliveryWatchFolderComboBox.Text != QCOutputWatchFolderComboBox.Text)
                        {
                            toolTip1.ToolTipTitle = "Wrong Selection";
                            toolTip1.Show("When Post Validation and File conversion are not included, " + Environment.NewLine + "the Batch Delivery Watch Folder must be the same as the QC Ouptut Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                            BatchDeliveryWatchFolderComboBox.Focus();
                            BatchDeliveryWatchFolderComboBox.Select();
                            return;
                        }
                        else
                        {
                            job.BatchDeliveryWatchFolderID = ((ComboboxItem)BatchDeliveryWatchFolderComboBox.SelectedItem).ID;
                        }
                    }
                    else
                    {
                        job.BatchDeliveryWatchFolderID = ((ComboboxItem)BatchDeliveryWatchFolderComboBox.SelectedItem).ID;
                    }                   
                }
            }
            else
            {
                //job.BatchDeliveryEnableFlag = false;
                job.BatchDeliveryWatchFolderID = 0;
            }

            if (!IncludePostValidationCheckBox.Checked && IncludePDFConversionCheckBox.Checked)
            {
                if (LoadBalancerWatchFolderComboBox.Text != QCOutputWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("When Post Validation is not included, " + Environment.NewLine + "the Load Balancer Watch Folder must be the same as the QC Ouptut Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchDeliveryWatchFolderComboBox.Focus();
                    BatchDeliveryWatchFolderComboBox.Select();
                    return;
                }
            }            
            
            // Check Final Location Information
            if (IncludeFinalLocationCheckBox.Checked)
            {
                if (BatchFinalLocationComboBox.Text.Length == 0)
                {
                    toolTip1.ToolTipTitle = "Missing Value";
                    toolTip1.Show("When this option is checked, you must provide values for Scanning Folder.", BatchFinalLocationComboBox, 5000);
                    BatchFinalLocationComboBox.Focus();
                    BatchFinalLocationComboBox.Select();
                    return;
                }
                else
                {
                    job.RestingLocationID = ((ComboboxItem)BatchFinalLocationComboBox.SelectedItem).ID;
                }
            }
            else
            {
                job.RestingLocationID = 0;
            }

            // ---------------------------------------------------------------------------------------------------------
            // Folder validation between Combo Boxes
            // ---------------------------------------------------------------------------------------------------------

            // AutoImportWatchFolderComboBox vs others
            if (IncludeAutoImportCheckBox.Checked)
            {
                if (AutoImportWatchFolderComboBox.Text == ScanningFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Capture Folder cannot be the same as the Auto Import Watch Folder.", ScanningFolderComboBox, 5000);
                    ScanningFolderComboBox.Focus();
                    ScanningFolderComboBox.Select();
                    return;
                }
                if (AutoImportWatchFolderComboBox.Text == QCOutputWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("QC Ouput Folder cannot be the same as the Auto Import Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    QCOutputWatchFolderComboBox.Focus();
                    QCOutputWatchFolderComboBox.Select();
                    return;
                }
                if (AutoImportWatchFolderComboBox.Text == PostValidationComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Post Validation Folder cannot be the same as the Auto Import Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    PostValidationComboBox.Focus();
                    PostValidationComboBox.Select();
                    return;
                }
                if (AutoImportWatchFolderComboBox.Text == LoadBalancerWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Load Balancer Watch Folder cannot be the same as the Auto Import Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    LoadBalancerWatchFolderComboBox.Focus();
                    LoadBalancerWatchFolderComboBox.Select();
                    return;
                }
                if (AutoImportWatchFolderComboBox.Text == BackupFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Backup Folder cannot be the same as the Auto Import Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BackupFolderComboBox.Focus();
                    BackupFolderComboBox.Select();
                    return;
                }
                if (AutoImportWatchFolderComboBox.Text == BatchDeliveryWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Delivery Watch Folder cannot be the same as the Auto Import Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchDeliveryWatchFolderComboBox.Focus();
                    BatchDeliveryWatchFolderComboBox.Select();
                    return;
                }
                if (AutoImportWatchFolderComboBox.Text == BatchFinalLocationComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Final Batch Location Folder cannot be the same as the Auto Import Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchFinalLocationComboBox.Focus();
                    BatchFinalLocationComboBox.Select();
                    return;
                }
            }
            
            // ScanningFolderComboBox vs others
            if (ScanningFolderComboBox.Text == QCOutputWatchFolderComboBox.Text)
            {
                toolTip1.ToolTipTitle = "Wrong Selection";
                toolTip1.Show("QC Ouput Folder cannot be the same as Capture Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                QCOutputWatchFolderComboBox.Focus();
                QCOutputWatchFolderComboBox.Select();
                return;
            }
            if (ScanningFolderComboBox.Text == PostValidationComboBox.Text)
            {
                toolTip1.ToolTipTitle = "Wrong Selection";
                toolTip1.Show("Post Validation Folder cannot be the same as Capture Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                PostValidationComboBox.Focus();
                PostValidationComboBox.Select();
                return;
            }
            if (ScanningFolderComboBox.Text == LoadBalancerWatchFolderComboBox.Text)
            {
                toolTip1.ToolTipTitle = "Wrong Selection";
                toolTip1.Show("Load Balancer Watch Folder cannot be the same as Capture Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                LoadBalancerWatchFolderComboBox.Focus();
                LoadBalancerWatchFolderComboBox.Select();
                return;
            }
            if (ScanningFolderComboBox.Text == BackupFolderComboBox.Text)
            {
                toolTip1.ToolTipTitle = "Wrong Selection";
                toolTip1.Show("Backup Folder cannot be the same as Capture Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                BackupFolderComboBox.Focus();
                BackupFolderComboBox.Select();
                return;
            }
            //if (ScanningFolderComboBox.Text == FileConversionWatchFolderComboBox.Text)
            //{
            //    toolTip1.ToolTipTitle = "Wrong Selection";
            //    toolTip1.Show("File Conversion Watch  Folder cannot be the same as Capture Folder.", BatchDeliveryWatchFolderComboBox, 5000);
            //    FileConversionWatchFolderComboBox.Focus();
            //    FileConversionWatchFolderComboBox.Select();
            //    return;
            //}
            if (ScanningFolderComboBox.Text == BatchDeliveryWatchFolderComboBox.Text)
            {
                toolTip1.ToolTipTitle = "Wrong Selection";
                toolTip1.Show("Batch Delivery Watch Folder cannot be the same as Capture Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                BatchDeliveryWatchFolderComboBox.Focus();
                BatchDeliveryWatchFolderComboBox.Select();
                return;
            }
            if (ScanningFolderComboBox.Text == BatchFinalLocationComboBox.Text)
            {
                toolTip1.ToolTipTitle = "Wrong Selection";
                toolTip1.Show("Final Batch Location Folder cannot be the same as Capture Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                BatchFinalLocationComboBox.Focus();
                BatchFinalLocationComboBox.Select();
                return;
            }

            // Folder validation between Combo Boxes
            // QCOutputWatchFolderComboBox vs others
            if (IncludePostValidationCheckBox.Checked)
            {
                if (QCOutputWatchFolderComboBox.Text != PostValidationComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Post Validation Folder must be the same as QC Output Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    PostValidationComboBox.Focus();
                    PostValidationComboBox.Select();
                    return;
                }

                if (QCOutputWatchFolderComboBox.Text != LoadBalancerWatchFolderComboBox.Text && !IncludePostValidationCheckBox.Checked && IncludePDFConversionCheckBox.Checked)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Load Balancer Watch Folder must be the same as QC Output Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    LoadBalancerWatchFolderComboBox.Focus();
                    LoadBalancerWatchFolderComboBox.Select();
                    return;
                }
                if (QCOutputWatchFolderComboBox.Text == LoadBalancerWatchFolderComboBox.Text && IncludePostValidationCheckBox.Checked)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Load Balancer Watch Folder cannot be the same as QC Output Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    LoadBalancerWatchFolderComboBox.Focus();
                    LoadBalancerWatchFolderComboBox.Select();
                    return;
                }
                if (QCOutputWatchFolderComboBox.Text == BackupFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Backup Folder cannot be the same as QC Output Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BackupFolderComboBox.Focus();
                    BackupFolderComboBox.Select();
                    return;
                }
                //if (QCOutputWatchFolderComboBox.Text == FileConversionWatchFolderComboBox.Text)
                //{
                //    toolTip1.ToolTipTitle = "Wrong Selection";
                //    toolTip1.Show("File Conversion Watch Folder cannot be the same as QC Output Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                //    FileConversionWatchFolderComboBox.Focus();
                //    FileConversionWatchFolderComboBox.Select();
                //    return;
                //}
                if (QCOutputWatchFolderComboBox.Text == BatchDeliveryWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Delivery Watch Folder cannot be the same as QC Output Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchDeliveryWatchFolderComboBox.Focus();
                    BatchDeliveryWatchFolderComboBox.Select();
                    return;
                }
                if (QCOutputWatchFolderComboBox.Text == BatchFinalLocationComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Final Location cannot be the same as QC Output Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchFinalLocationComboBox.Focus();
                    BatchFinalLocationComboBox.Select();
                    return;
                }
            }
            

            // Folder validation between Combo Boxes
            // PostValidationComboBox vs others
            if (IncludePostValidationCheckBox.Checked)
            {
                if (IncludePDFConversionCheckBox.Checked)
                {
                    if (PostValidationComboBox.Text != LoadBalancerWatchFolderComboBox.Text)
                    {
                        toolTip1.ToolTipTitle = "Wrong Selection";
                        toolTip1.Show("Load Balancer Watch Folder cannot be the same as Post Validation Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                        LoadBalancerWatchFolderComboBox.Focus();
                        LoadBalancerWatchFolderComboBox.Select();
                        return;
                    }
                    if (PostValidationComboBox.Text != BackupFolderComboBox.Text)
                    {
                        toolTip1.ToolTipTitle = "Wrong Selection";
                        toolTip1.Show("Backup Folder cannot be the same as Post Validation Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                        BackupFolderComboBox.Focus();
                        BackupFolderComboBox.Select();
                        return;
                    }
                    //if (PostValidationComboBox.Text != FileConversionWatchFolderComboBox.Text)
                    //{
                    //    toolTip1.ToolTipTitle = "Wrong Selection";
                    //    toolTip1.Show("File Conversion Watch Folder cannot be the same as Post Validation Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    //    FileConversionWatchFolderComboBox.Focus();
                    //    FileConversionWatchFolderComboBox.Select();
                    //    return;
                    //}
                }
                if (PostValidationComboBox.Text != BatchDeliveryWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Delivery Watch Folder cannot be the same as Post Validation Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchDeliveryWatchFolderComboBox.Focus();
                    BatchDeliveryWatchFolderComboBox.Select();
                    return;
                }
                if (PostValidationComboBox.Text != BatchFinalLocationComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Final Location cannot be the same as Post Validation Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchFinalLocationComboBox.Focus();
                    BatchFinalLocationComboBox.Select();
                    return;
                }
            }

            // Folder validation between Combo Boxes
            // LoadBalancerWatchFolderComboBox vs others
            if (IncludePDFConversionCheckBox.Checked)
            {
                if (LoadBalancerWatchFolderComboBox.Text == BackupFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Backup Folder cannot be the same as Load Balancer Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BackupFolderComboBox.Focus();
                    BackupFolderComboBox.Select();
                    return;
                }
                //if (LoadBalancerWatchFolderComboBox.Text == FileConversionWatchFolderComboBox.Text)
                //{
                //    toolTip1.ToolTipTitle = "Wrong Selection";
                //    toolTip1.Show("File Conversion Watch Folder cannot be the same as Load Balancer Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                //    FileConversionWatchFolderComboBox.Focus();
                //    FileConversionWatchFolderComboBox.Select();
                //    return;
                //}
                if (LoadBalancerWatchFolderComboBox.Text == BatchDeliveryWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Delivery Watch Folder cannot be the same as Load Balancer Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchDeliveryWatchFolderComboBox.Focus();
                    BatchDeliveryWatchFolderComboBox.Select();
                    return;
                }
                if (LoadBalancerWatchFolderComboBox.Text == BatchFinalLocationComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Final Location Folder cannot be the same as Load Balancer Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchFinalLocationComboBox.Focus();
                    BatchFinalLocationComboBox.Select();
                    return;
                }
                //----
                // BackupFolderComboBox vs others
                //if (BackupFolderComboBox.Text == FileConversionWatchFolderComboBox.Text)
                //{
                //    toolTip1.ToolTipTitle = "Wrong Selection";
                //    toolTip1.Show("File Conversion Watch Folder cannot be the same as Backup Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                //    FileConversionWatchFolderComboBox.Focus();
                //    FileConversionWatchFolderComboBox.Select();
                //    return;
                //}
                if (BackupFolderComboBox.Text == BatchDeliveryWatchFolderComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Delivery Watch Folder cannot be the same as Backup Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchDeliveryWatchFolderComboBox.Focus();
                    BatchDeliveryWatchFolderComboBox.Select();
                    return;
                }
                if (BackupFolderComboBox.Text == BatchFinalLocationComboBox.Text)
                {
                    toolTip1.ToolTipTitle = "Wrong Selection";
                    toolTip1.Show("Batch Final Location Folder cannot be the same as Backup Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                    BatchFinalLocationComboBox.Focus();
                    BatchFinalLocationComboBox.Select();
                    return;
                }
                job.LoadBalancerWatchFolderID = ((ComboboxItem)LoadBalancerWatchFolderComboBox.SelectedItem).ID;
                job.BackupFolderID = ((ComboboxItem)BackupFolderComboBox.SelectedItem).ID;

                //-----
                // FileConversionWatchFolderComboBox vs others
                //if (FileConversionWatchFolderComboBox.Text == BatchDeliveryWatchFolderComboBox.Text)
                //{
                //    toolTip1.ToolTipTitle = "Wrong Selection";
                //    toolTip1.Show("Batch Delivery Watch Folder cannot be the same as File Conversion Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                //    BatchDeliveryWatchFolderComboBox.Focus();
                //    BatchDeliveryWatchFolderComboBox.Select();
                //    return;
                //}
                //if (FileConversionWatchFolderComboBox.Text == BatchFinalLocationComboBox.Text)
                //{
                //    toolTip1.ToolTipTitle = "Wrong Selection";
                //    toolTip1.Show("Batch Final Location Folder cannot be the same as File Conversion Watch Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                //    BatchFinalLocationComboBox.Focus();
                //    BatchFinalLocationComboBox.Select();
                //    return;
                //}
            }

            // Folder validation between Combo Boxes
            // BatchDeliveryWatchFolderComboBox vs others
            if (BatchDeliveryWatchFolderComboBox.Text == BatchFinalLocationComboBox.Text)
            {
                toolTip1.ToolTipTitle = "Wrong Selection";
                toolTip1.Show("Batch Final Location  cannot be the same as Batch Delivery Folder.", BatchDeliveryWatchFolderComboBox, 5000);
                BatchFinalLocationComboBox.Focus();
                BatchFinalLocationComboBox.Select();
                return;
            }
            
            // Check VFR Option Information
            if (IncludeVFRCheckBox.Checked)
            {
                //if (VFRRenamerWatchFolderComboBox.Text.Length == 0)
                //{
                //    toolTip1.ToolTipTitle = "Missing Value";
                //    toolTip1.Show("When this option is checked, you must provide value a for Renamer Watch Folder.", VFRRenamerWatchFolderComboBox, 5000);
                //    VFRRenamerWatchFolderComboBox.Focus();
                //    VFRRenamerWatchFolderComboBox.Select();
                //    return;
                //}
                //else
                //{
                //    if (VFRDuplicateRemoverWatchFolderComboBox.Text.Length == 0)
                //    {
                //        toolTip1.ToolTipTitle = "Missing Value";
                //        toolTip1.Show("When this option is checked, you must provide value a for Duplicate Remover Watch Folder.", VFRDuplicateRemoverWatchFolderComboBox, 5000);
                //        VFRDuplicateRemoverWatchFolderComboBox.Focus();
                //        VFRDuplicateRemoverWatchFolderComboBox.Select();
                //        return;
                //    }
                //    else
                //    {
                //        if (VFRBatchUploaderComboBox.Text.Length == 0)
                //        {
                //            toolTip1.ToolTipTitle = "Missing Value";
                //            toolTip1.Show("When this option is checked, you must provide value a for Batch Uploader Watch Folder.", VFRBatchUploaderComboBox, 5000);
                //            VFRBatchUploaderComboBox.Focus();
                //            VFRBatchUploaderComboBox.Select();
                //            return;
                //        }
                //        else
                //        {
                //            job.VFREnableFlag = true;
                //            job.VFRRenamerWatchFolderID = ((ComboboxItem)VFRRenamerWatchFolderComboBox.SelectedItem).ID;
                //            job.VFRDuplicateRemoverWatchFolderID = ((ComboboxItem)VFRDuplicateRemoverWatchFolderComboBox.SelectedItem).ID;
                //            job.VFRBatchUploaderWatchFolderID = ((ComboboxItem)VFRBatchUploaderComboBox.SelectedItem).ID;
                //            if (VFRBatchUploaderComboBox.Text.Length == 0)
                //            {
                //                job.VFRBatchMonitorFolderID = 0;
                //            }
                //            else
                //            {
                //                job.VFRBatchMonitorFolderID = ((ComboboxItem)VFRRenamerWatchFolderComboBox.SelectedItem).ID;
                //            }                                
                //        }
                //    }
                //}
            }
            else
            {
                job.VFREnableFlag = false;
                job.VFRRenamerWatchFolderID = 0;
                job.VFRDuplicateRemoverWatchFolderID = 0;
                job.VFRBatchUploaderWatchFolderID = 0;                
                job.VFRBatchMonitorFolderID = 0;
            }

            job.ProjectID = Data.GlovalVariables.currentProjectID;
            job.JobID = Data.GlovalVariables.currentJobID;
            job.JobName = JobNameTextBox.Text;
            job.MultiPageFlag = MultiPageRadioButton.Checked;
            job.DepartmentName = DepartmentTextBox.Text;
            job.ExportClassName = ExportClassNameTextBox.Text;
            job.AutoImportEnableFlag = IncludeAutoImportCheckBox.Checked;
            job.FileConversionEnableFlag = IncludePDFConversionCheckBox.Checked;
            job.PostValidationEnableFlag = IncludePostValidationCheckBox.Checked;
            job.VFREnableFlag = IncludeVFRCheckBox.Checked;
            job.MaxBatchesPerWorkOrder = Convert.ToInt32(MaxNumberBatchesPerWOUpDown.Text);
            if (SearchablePDFRadioButton.Checked)
            {
                job.OutputFileType = "Searchable PDF";
            }
            else
            {
                if (PDFRadioButton.Checked)
                {
                    job.OutputFileType = "PDF";
                }
                else
                {
                    job.OutputFileType = "TIF";
                }
            }
   

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string URL = "";
            string bodyString = "";
            string jobJS = "";
            string returnMessage = "";
           
            ResultJobs resultJobs = new ResultJobs();
            
            switch (Data.GlovalVariables.transactionType)
            {
                case "New":
                    jobJS = JsonConvert.SerializeObject(job, Newtonsoft.Json.Formatting.Indented);
                    jobJS = jobJS.Replace(@"\", "\\\\");
                    URL = BaseURL + "Jobs/NewJob";
                    bodyString = "'" + jobJS + "'";
                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    using (HttpContent content = response_for_new.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        // Removing escape characters
                        //returnMessage = Regex.Unescape(returnMessage);
                        // Removing Double Quotes from gthe beginning and the end of the string
                        //returnMessage = Regex.Replace(returnMessage, "^\"|\"$", "");
                        resultJobs = JsonConvert.DeserializeObject<ResultJobs>(returnMessage);
                    }

                    if (response_for_new.IsSuccessStatusCode)
                    {
                        // Set the value of the new Job to a gloval variable
                        if (resultJobs.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultJobs.Message.Replace(". ", "\r\n"), "New Job Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.newJobsList.Add(JobNameTextBox.Text);
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                               // JobNameTextBox.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultJobs.Message.Replace(". ", "\r\n") + resultJobs.Exception, "New Job Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } 
                    break;

                case "Update":

                    jobJS = JsonConvert.SerializeObject(job, Newtonsoft.Json.Formatting.Indented);
                    jobJS = jobJS.Replace(@"\", "\\\\");
                    //jobJS = Regex.Escape(jobJS);
                    URL = BaseURL + "Jobs/UpdateJob";
                    bodyString = "'" + jobJS + "'";
                    HttpContent body_for_update = new StringContent(bodyString);
                    body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                    using (HttpContent content = response_for_update.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        // Removing escape characters
                        //returnMessage = Regex.Unescape(returnMessage);
                        // Removing Double Quotes from gthe beginning and the end of the string
                        //returnMessage = Regex.Replace(returnMessage, "^\"|\"$", "");
                        resultJobs = JsonConvert.DeserializeObject<ResultJobs>(returnMessage);
                    }

                    if (response_for_update.IsSuccessStatusCode)
                    {
                        // Set the value of the new Job to a gloval variable
                        if (resultJobs.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultJobs.Message.Replace(". ", "\r\n"), "Update Projects Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.currentJobName = JobNameTextBox.Text;
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                                JobNameTextBox.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultJobs.Message.Replace(". ", "\r\n"), "Update Project Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            //originalJob.JobID = resultJobs.ReturnValue[0].JobID;
            //originalJob.ProjectID = resultJobs.ReturnValue[0].ProjectID;
            //originalJob.CustomerName = CustomerNameTextBox.Text;
            JobNameTextBox.Text = originalJob.JobName;
            DepartmentTextBox.Text = originalJob.DepartmentName;
            ExportClassNameTextBox.Text  = originalJob.ExportClassName;
            if (originalJob.OutputFileType == "Searchable PDF")
            {
                SearchablePDFRadioButton.Checked = true;
            }
            else
            {
                if (ExportClassNameTextBox.Text == "PDF")
                {
                    PDFRadioButton.Checked = true;
                }
                else
                {
                    TIFRadioButton.Checked = true;
                }
            }
            if (originalJob.MultiPageFlag)
                MultiPageRadioButton.Checked = true;
            else
                SinglePageRadioButton.Checked = true;

            ScanningFolderComboBox.Text = originalJob.ScanningFolder;
           
            AutoImportWatchFolderComboBox.Text = originalJob.AutoImportWatchFolder;
            if (originalJob.AutoImportEnableFlag)
                IncludeAutoImportCheckBox.Checked = true;
            else
                IncludeAutoImportCheckBox.Checked = false;

            PostValidationComboBox.Text = originalJob.PostValidationWatchFolder;
            if (originalJob.PostValidationEnableFlag)
                IncludePostValidationCheckBox.Checked = true;
            else
                IncludePostValidationCheckBox.Checked = false;

            LoadBalancerWatchFolderComboBox.Text = originalJob.LoadBalancerWatchFolder;
            //if (originalJob.LoadBalancerEnableFlag)
            //    IncludeLoadBalancerCheckBox.Checked = true;
            //else
            //    IncludeLoadBalancerCheckBox.Checked = false;
            BackupFolderComboBox.Text = originalJob.BackupFolder;

            //FileConversionWatchFolderComboBox.Text = originalJob.FileConversionWatchFolder;
            if (originalJob.FileConversionEnableFlag)
               IncludePDFConversionCheckBox.Checked = true;
            else
                IncludePDFConversionCheckBox.Checked = false;

            BatchDeliveryWatchFolderComboBox.Text = originalJob.BatchDeliveryWatchFolder;
            //if (originalJob.BatchDeliveryEnableFlag)
            //    IncludeBatchDeliveryCheckBox.Checked = true;
            //else
            //    IncludeBatchDeliveryCheckBox.Checked = false;

            BatchFinalLocationComboBox.Text = originalJob.RestingLocation;

            if (originalJob.VFREnableFlag)
                IncludeVFRCheckBox.Checked = true;
            else
                IncludeVFRCheckBox.Checked = false;
            //VFRRenamerWatchFolderComboBox.Text = originalJob.VFRRenamerWatchFolder;
            //VFRDuplicateRemoverWatchFolderComboBox.Text = originalJob.VFRDuplicateRemoverWatchFolder;
            //VFRBatchUploaderComboBox.Text = originalJob.VFRBatchUploaderWatchFolder;
            //VFRBatchMonitorComboBox.Text = originalJob.VFRBatchMonitorFolder;
            MaxNumberBatchesPerWOUpDown.Text =  originalJob.MaxBatchesPerWorkOrder.ToString() ;
        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void ScanningFolderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Force the Auto Import Watch Folder to be always the Capure Pro Scan Folder
            //AutoImportWatchFolderComboBox.Text = ScanningFolderComboBox.Text;
        }

        private void QCOutputWatchFolderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PostValidationComboBox.Text = QCOutputWatchFolderComboBox.Text;
            
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\Workflow Templates.pdf" );
        }

        private void button1_Click(object sender, EventArgs e)
        {

            WorlflowTemplatesForm _WorlflowTemplatesForm = new WorlflowTemplatesForm();
            _WorlflowTemplatesForm.StartPosition = FormStartPosition.CenterScreen;
            _WorlflowTemplatesForm.Show();
        }
    }
}
