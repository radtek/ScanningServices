using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;
using static ScanningServicesAdmin.Data.GlovalVariables;
using System.Net.Mail;
using NLog;

namespace ScanningServicesAdmin.Forms
{
    public partial class GeneralSettingsForm : Form
    {
        public GeneralSettings originalGeneralSettings = new GeneralSettings();
        public ResultWorkingFolders resultWorkingFolders = new ResultWorkingFolders();
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public GeneralSettingsForm()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            // Set Combo Boxes
            ResultWorkingFolders resultWorkingFolders = new ResultWorkingFolders();
            resultWorkingFolders = GetWorkingFolders();
            if (resultWorkingFolders.ReturnCode == 0)
            {
                ComboboxItem firstItem = new ComboboxItem();
                firstItem.Text = "";
                firstItem.ID = 0;
                AutoImportWatchFolderComboBox.Items.Add(firstItem);
                ScanningFolderComboBox.Items.Add(firstItem);
                PostValidationComboBox.Items.Add(firstItem);
                LoadBalancerWatchFolderComboBox.Items.Add(firstItem);
                BackupFolderComboBox.Items.Add(firstItem);
                FileConversionWatchFolderComboBox.Items.Add(firstItem);
                BatchDeliveryWatchFolderComboBox.Items.Add(firstItem);
                BatchFinalLocationComboBox.Items.Add(firstItem);
                VFRRenamerWatchFolderComboBox.Items.Add(firstItem);
                VFRDuplicateRemoverWatchFolderComboBox.Items.Add(firstItem);
                VFRBatchMonitorComboBox.Items.Add(firstItem);
                VFRBatchUploaderComboBox.Items.Add(firstItem);
                QCOutputFolderComboBox.Items.Add(firstItem);

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
                    FileConversionWatchFolderComboBox.Items.Add(item);
                    BatchDeliveryWatchFolderComboBox.Items.Add(item);
                    BatchFinalLocationComboBox.Items.Add(item);
                    VFRRenamerWatchFolderComboBox.Items.Add(item);
                    VFRDuplicateRemoverWatchFolderComboBox.Items.Add(item);
                    VFRBatchMonitorComboBox.Items.Add(item);
                    VFRBatchUploaderComboBox.Items.Add(item);
                    QCOutputFolderComboBox.Items.Add(item);
                }
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private ResultWorkingFolders GetWorkingFolders()
        {
            string urlParameters = "";
            string URL = "";
            string returnMessage = "";
                        
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


        static public string GetFolderPath(int folderID, ResultWorkingFolders workingFolder)
        {
            string path = "";
            try
            {
                foreach (WorkingFolder folder in workingFolder.ReturnValue)
                {
                    if (folder.FolderID == folderID)
                    {
                        path = folder.Path.Trim();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                //MainForm.ErrorMessage(e);
                nlogger.Fatal(General.ErrorMessage(e));
                MessageBox.Show(General.ErrorMessage(e), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return path;
        }

        private void GeneralSettingsForm_Load(object sender, EventArgs e)
        {
            // Get General Settings
            ResultGeneralSettings resultGeneralSettings = new ResultGeneralSettings();
            string returnMessage = "";
            Cursor.Current = Cursors.WaitCursor;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            URL = BaseURL + "GeneralSettings/GetGeneralSettingsInfo";
            client.BaseAddress = new Uri(URL);

            response = client.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultGeneralSettings = JsonConvert.DeserializeObject<ResultGeneralSettings>(returnMessage);
            }

            if (response.IsSuccessStatusCode)
            {
                if (resultGeneralSettings.RecordsCount != 0)
                {
                    if (resultGeneralSettings.ReturnValue.DebugFlag)
                    {
                        EnableDebugCheckBox.Checked = true;
                    }
                    else
                    {
                        EnableDebugCheckBox.Checked = true;
                    }
                    CPAppPathTextBox.Text = resultGeneralSettings.ReturnValue.CPApplicationFilePath.Trim();
                    ImageViewerAppPathTextBox.Text = resultGeneralSettings.ReturnValue.ImageViewerFilePath.Trim();
                    DBServerTextBox.Text = resultGeneralSettings.ReturnValue.DBServer.Trim();
                    DBUserNameTextBox.Text = resultGeneralSettings.ReturnValue.DBUserName.Trim();
                    DBPasswordTextBox.Text =  resultGeneralSettings.ReturnValue.DBPassword.Trim();
                    DBProviderTextBox.Text = resultGeneralSettings.ReturnValue.DBProvider.Trim();
                    DBNameTextBox.Text = resultGeneralSettings.ReturnValue.DBName.Trim();
                    DBRDBMSTextBox.Text =  resultGeneralSettings.ReturnValue.DBRDBMS.Trim();
                    CADIWSURLTextBox.Text = resultGeneralSettings.ReturnValue.CdiWebUrl.Trim();
                    AutoImportWatchFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.AutoImportWatchFolderID, resultWorkingFolders);
                    ScanningFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.ScanningFolderID, resultWorkingFolders);
                    PostValidationComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.PostValidationWatchFolderID, resultWorkingFolders);
                    LoadBalancerWatchFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.LoadBalancerWatchFolderID, resultWorkingFolders);
                    BackupFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.BackupFolderID, resultWorkingFolders);
                    FileConversionWatchFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.FileConversionWatchFolderID, resultWorkingFolders);
                    BatchDeliveryWatchFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.BatchDeliveryWatchFolderID, resultWorkingFolders);
                    BatchFinalLocationComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.RestingLocationID, resultWorkingFolders);
                    VFRBatchUploaderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.VFRBatchUploaderWatchFolderID, resultWorkingFolders);
                    VFRRenamerWatchFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.VFRRenamerWatchFolderID, resultWorkingFolders);
                    VFRDuplicateRemoverWatchFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.VFRDuplicateRemoverWatchFolderID, resultWorkingFolders);
                    VFRBatchMonitorComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.VFRBatchMonitorFolderID, resultWorkingFolders);
                    QCOutputFolderComboBox.Text = GetFolderPath(resultGeneralSettings.ReturnValue.QCOutputFolderID, resultWorkingFolders);

                    //Keep the original values in originalGeneralSettings
                    originalGeneralSettings.DebugFlag = resultGeneralSettings.ReturnValue.DebugFlag;
                    originalGeneralSettings.DBServer = resultGeneralSettings.ReturnValue.DBServer.Trim();
                    originalGeneralSettings.DBUserName = resultGeneralSettings.ReturnValue.DBUserName.Trim();
                    originalGeneralSettings.DBPassword = resultGeneralSettings.ReturnValue.DBPassword.Trim();
                    originalGeneralSettings.DBProvider = resultGeneralSettings.ReturnValue.DBProvider.Trim();
                    originalGeneralSettings.DBName = resultGeneralSettings.ReturnValue.DBName.Trim();
                    originalGeneralSettings.DBRDBMS = resultGeneralSettings.ReturnValue.DBRDBMS.Trim();
                    originalGeneralSettings.CdiWebUrl = resultGeneralSettings.ReturnValue.CdiWebUrl.Trim();
                    originalGeneralSettings.CPApplicationFilePath = resultGeneralSettings.ReturnValue.CPApplicationFilePath.Trim();
                    originalGeneralSettings.ImageViewerFilePath = resultGeneralSettings.ReturnValue.ImageViewerFilePath.Trim();
                    originalGeneralSettings.AutoImportWatchFolderID = resultGeneralSettings.ReturnValue.AutoImportWatchFolderID;
                    originalGeneralSettings.ScanningFolderID = resultGeneralSettings.ReturnValue.ScanningFolderID;
                    originalGeneralSettings.PostValidationWatchFolderID = resultGeneralSettings.ReturnValue.PostValidationWatchFolderID;
                    originalGeneralSettings.LoadBalancerWatchFolderID = resultGeneralSettings.ReturnValue.LoadBalancerWatchFolderID;
                    originalGeneralSettings.BackupFolderID = resultGeneralSettings.ReturnValue.BackupFolderID;
                    originalGeneralSettings.FileConversionWatchFolderID = resultGeneralSettings.ReturnValue.FileConversionWatchFolderID;
                    originalGeneralSettings.BatchDeliveryWatchFolderID = resultGeneralSettings.ReturnValue.BatchDeliveryWatchFolderID;
                    originalGeneralSettings.RestingLocationID = resultGeneralSettings.ReturnValue.RestingLocationID;
                    originalGeneralSettings.VFRRenamerWatchFolderID = resultGeneralSettings.ReturnValue.VFRRenamerWatchFolderID;
                    originalGeneralSettings.VFRDuplicateRemoverWatchFolderID = resultGeneralSettings.ReturnValue.VFRDuplicateRemoverWatchFolderID;
                    originalGeneralSettings.VFRBatchUploaderWatchFolderID = resultGeneralSettings.ReturnValue.VFRBatchUploaderWatchFolderID;
                    originalGeneralSettings.VFRBatchMonitorFolderID = resultGeneralSettings.ReturnValue.VFRBatchMonitorFolderID;
                    originalGeneralSettings.QCOutputFolderID = resultGeneralSettings.ReturnValue.QCOutputFolderID;
                }
                else
                {
                    CPAppPathTextBox.Text = "";
                    ImageViewerAppPathTextBox.Text = "";
                    DBServerTextBox.Text = "";
                    DBUserNameTextBox.Text = "";
                    DBPasswordTextBox.Text = "";
                    DBProviderTextBox.Text = "";
                    DBNameTextBox.Text = "";
                    DBRDBMSTextBox.Text = "";
                    CADIWSURLTextBox.Text = "";
                    EnableDebugCheckBox.Checked = false;

                    //Set original values in originalGeneralSettings
                    originalGeneralSettings.DebugFlag = false;
                    originalGeneralSettings.DBServer = "";
                    originalGeneralSettings.DBUserName = "";
                    originalGeneralSettings.DBPassword = "";
                    originalGeneralSettings.DBProvider = "";
                    originalGeneralSettings.DBName = "";
                    originalGeneralSettings.DBRDBMS = "";
                    originalGeneralSettings.CdiWebUrl = "";
                    originalGeneralSettings.CPApplicationFilePath = "";
                    originalGeneralSettings.ImageViewerFilePath = "";
                    originalGeneralSettings.AutoImportWatchFolderID = 0;
                    originalGeneralSettings.ScanningFolderID = 0;
                    originalGeneralSettings.PostValidationWatchFolderID = 0;
                    originalGeneralSettings.LoadBalancerWatchFolderID = 0;
                    originalGeneralSettings.BackupFolderID = 0;
                    originalGeneralSettings.FileConversionWatchFolderID = 0;
                    originalGeneralSettings.BatchDeliveryWatchFolderID = 0;
                    originalGeneralSettings.RestingLocationID = 0 ;
                    originalGeneralSettings.VFRRenamerWatchFolderID = 0;
                    originalGeneralSettings.VFRDuplicateRemoverWatchFolderID = 0;
                    originalGeneralSettings.VFRBatchUploaderWatchFolderID = 0;
                    originalGeneralSettings.VFRBatchMonitorFolderID = 0;
                    originalGeneralSettings.QCOutputFolderID = 0;

                }
            }          
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
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string URL = "";
            string bodyString = "";
            string generalSettingsJS = "";
            string returnMessage = "";
            GeneralSettings generalSettings = new GeneralSettings();
            ResultGeneralSettings resultGeneralSettings = new ResultGeneralSettings();

            switch (Data.GlovalVariables.transactionType)
            {
                case "Update":

                    // Build the General Settings Object
                    generalSettings.CPApplicationFilePath = CPAppPathTextBox.Text.Trim();
                    generalSettings.ImageViewerFilePath = ImageViewerAppPathTextBox.Text.Trim();
                    generalSettings.DBServer = DBServerTextBox.Text.Trim();
                    generalSettings.DBUserName = DBUserNameTextBox.Text.Trim();
                    generalSettings.DBPassword = DBPasswordTextBox.Text.Trim();
                    generalSettings.DBProvider = DBProviderTextBox.Text.Trim();
                    generalSettings.DBName = DBNameTextBox.Text.Trim();
                    generalSettings.DBRDBMS = DBRDBMSTextBox.Text.Trim();
                    generalSettings.CdiWebUrl = CADIWSURLTextBox.Text.Trim();

                    if (EnableDebugCheckBox.Checked)
                    {
                        generalSettings.DebugFlag = true;
                    }
                    else
                    {
                        generalSettings.DebugFlag = false;
                    }


                    if (string.IsNullOrEmpty(ScanningFolderComboBox.Text))
                        generalSettings.ScanningFolderID = 0;
                    else
                        generalSettings.ScanningFolderID = ((ComboboxItem)ScanningFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(AutoImportWatchFolderComboBox.Text))
                        generalSettings.AutoImportWatchFolderID = 0;
                    else
                        generalSettings.AutoImportWatchFolderID = ((ComboboxItem)AutoImportWatchFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(PostValidationComboBox.Text))
                        generalSettings.PostValidationWatchFolderID = 0;
                    else
                        generalSettings.PostValidationWatchFolderID = ((ComboboxItem)PostValidationComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(LoadBalancerWatchFolderComboBox.Text))
                        generalSettings.LoadBalancerWatchFolderID = 0;
                    else
                        generalSettings.LoadBalancerWatchFolderID = ((ComboboxItem)LoadBalancerWatchFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(BackupFolderComboBox.Text))
                        generalSettings.BackupFolderID = 0;
                    else
                        generalSettings.BackupFolderID = ((ComboboxItem)BackupFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(FileConversionWatchFolderComboBox.Text))
                        generalSettings.FileConversionWatchFolderID = 0;
                    else
                        generalSettings.FileConversionWatchFolderID = ((ComboboxItem)FileConversionWatchFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(BatchDeliveryWatchFolderComboBox.Text))
                        generalSettings.BatchDeliveryWatchFolderID = 0;
                    else
                        generalSettings.BatchDeliveryWatchFolderID = ((ComboboxItem)BatchDeliveryWatchFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(BatchFinalLocationComboBox.Text))
                        generalSettings.RestingLocationID = 0;
                    else
                        generalSettings.RestingLocationID = ((ComboboxItem)BatchFinalLocationComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(VFRRenamerWatchFolderComboBox.Text))
                        generalSettings.VFRRenamerWatchFolderID = 0;
                    else
                        generalSettings.VFRRenamerWatchFolderID = ((ComboboxItem)VFRRenamerWatchFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(VFRDuplicateRemoverWatchFolderComboBox.Text))
                        generalSettings.VFRDuplicateRemoverWatchFolderID = 0;
                    else
                        generalSettings.VFRDuplicateRemoverWatchFolderID = ((ComboboxItem)VFRDuplicateRemoverWatchFolderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(VFRBatchUploaderComboBox.Text))
                        generalSettings.VFRBatchUploaderWatchFolderID = 0;
                    else
                        generalSettings.VFRBatchUploaderWatchFolderID = ((ComboboxItem)VFRBatchUploaderComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(VFRBatchMonitorComboBox.Text))
                        generalSettings.VFRBatchMonitorFolderID = 0;
                    else
                        generalSettings.VFRBatchMonitorFolderID = ((ComboboxItem)VFRBatchMonitorComboBox.SelectedItem).ID;

                    if (string.IsNullOrEmpty(QCOutputFolderComboBox.Text))
                        generalSettings.QCOutputFolderID = 0;
                    else
                        generalSettings.QCOutputFolderID = ((ComboboxItem)QCOutputFolderComboBox.SelectedItem).ID;


                    // Build General Setting Object in Json Format
                    generalSettingsJS = JsonConvert.SerializeObject(generalSettings, Newtonsoft.Json.Formatting.Indented);
                    generalSettingsJS = generalSettingsJS.Replace(@"\", "\\\\");

                    URL = BaseURL + "GeneralSettings/UpdateGeneralSettings";
                    bodyString = "'" + generalSettingsJS + "'";

                    HttpContent body_for_update = new StringContent(bodyString);
                    body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                    using (HttpContent content = response_for_update.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultGeneralSettings = JsonConvert.DeserializeObject<ResultGeneralSettings>(returnMessage);
                    }

                    if (response_for_update.IsSuccessStatusCode)
                    {
                        if (resultGeneralSettings.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultGeneralSettings.Message.Replace(". ", "\r\n"), "Update General Settings Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultGeneralSettings.Message.Replace(". ", "\r\n") + "\r\n" + resultGeneralSettings.Exception, "Update General Settings Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                  
                    break;
            }

        }

        private void CPSAppBrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                CPAppPathTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void ImageViewerBrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                ImageViewerAppPathTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void CPAppPathTextBox_Leave(object sender, EventArgs e)
        {
            if (CPAppPathTextBox.Text.Length > 0)
            {
                if (!Validation.IsValidPath(CPAppPathTextBox.Text))
                {
                    toolTip1.ToolTipTitle = "Invalid Path";
                    toolTip1.Show("The value you entered is not a valid Path. Please change the value.", CPAppPathTextBox, 5000);
                    CPAppPathTextBox.Focus();
                    CPAppPathTextBox.SelectAll();
                }
            }
        }

        private void ImageViewerAppPathTextBox_Leave(object sender, EventArgs e)
        {
            if (ImageViewerAppPathTextBox.Text.Length > 0)
            {
                if (!Validation.IsValidPath(ImageViewerAppPathTextBox.Text))
                {
                    toolTip1.ToolTipTitle = "Invalid Path";
                    toolTip1.Show("The value you entered is not a valid Path. Please change the value.", ImageViewerAppPathTextBox, 5000);
                    ImageViewerAppPathTextBox.Focus();
                    ImageViewerAppPathTextBox.SelectAll();
                }
            }
        }

        private void CADIWSURLTextBox_Leave(object sender, EventArgs e)
        {
            if (CADIWSURLTextBox.Text.Length > 0)
            {
                if (!Validation.IsValidURL(CADIWSURLTextBox.Text))
                {
                    toolTip1.ToolTipTitle = "Invalid Path";
                    toolTip1.Show("The value you entered is not a valid URL. Please change the value.", CADIWSURLTextBox, 5000);
                    CADIWSURLTextBox.Focus();
                    CADIWSURLTextBox.SelectAll();
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            CPAppPathTextBox.Text = originalGeneralSettings.CPApplicationFilePath;
            ImageViewerAppPathTextBox.Text = originalGeneralSettings.ImageViewerFilePath;
            DBServerTextBox.Text = originalGeneralSettings.DBServer;
            DBUserNameTextBox.Text = originalGeneralSettings.DBUserName;
            DBPasswordTextBox.Text = originalGeneralSettings.DBPassword;
            DBProviderTextBox.Text = originalGeneralSettings.DBProvider;
            DBNameTextBox.Text = originalGeneralSettings.DBName;
            DBRDBMSTextBox.Text = originalGeneralSettings.DBRDBMS;
            CADIWSURLTextBox.Text = originalGeneralSettings.CdiWebUrl;
            if (originalGeneralSettings.DebugFlag)
            {
                EnableDebugCheckBox.Checked = true;
            }
            else
            {
                EnableDebugCheckBox.Checked = true;
            }

            AutoImportWatchFolderComboBox.Text = GetFolderPath(originalGeneralSettings.AutoImportWatchFolderID, resultWorkingFolders);
            ScanningFolderComboBox.Text = GetFolderPath(originalGeneralSettings.ScanningFolderID, resultWorkingFolders);
            PostValidationComboBox.Text = GetFolderPath(originalGeneralSettings.PostValidationWatchFolderID, resultWorkingFolders);
            LoadBalancerWatchFolderComboBox.Text = GetFolderPath(originalGeneralSettings.LoadBalancerWatchFolderID, resultWorkingFolders);
            BackupFolderComboBox.Text = GetFolderPath(originalGeneralSettings.BackupFolderID, resultWorkingFolders);
            FileConversionWatchFolderComboBox.Text = GetFolderPath(originalGeneralSettings.FileConversionWatchFolderID, resultWorkingFolders);
            BatchDeliveryWatchFolderComboBox.Text = GetFolderPath(originalGeneralSettings.BatchDeliveryWatchFolderID, resultWorkingFolders);
            BatchFinalLocationComboBox.Text = GetFolderPath(originalGeneralSettings.BatchDeliveryWatchFolderID, resultWorkingFolders);
            VFRBatchUploaderComboBox.Text = GetFolderPath(originalGeneralSettings.VFRBatchUploaderWatchFolderID, resultWorkingFolders);
            VFRRenamerWatchFolderComboBox.Text = GetFolderPath(originalGeneralSettings.VFRRenamerWatchFolderID, resultWorkingFolders);
            VFRDuplicateRemoverWatchFolderComboBox.Text = GetFolderPath(originalGeneralSettings.VFRDuplicateRemoverWatchFolderID, resultWorkingFolders);
            VFRBatchMonitorComboBox.Text = GetFolderPath(originalGeneralSettings.VFRBatchMonitorFolderID, resultWorkingFolders);
            QCOutputFolderComboBox.Text = GetFolderPath(originalGeneralSettings.QCOutputFolderID, resultWorkingFolders);

        }

        private void ScanningFolderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Force the Auto Import Watch Folder to be always the Capure Pro Scan Folder
            AutoImportWatchFolderComboBox.Text = ScanningFolderComboBox.Text;
        }
    }
}
