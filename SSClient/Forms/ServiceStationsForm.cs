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
using System.Text.RegularExpressions;
using NLog;

namespace ScanningServicesAdmin.Forms
{
    public partial class ServiceStationsForm : Form
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public ServiceStation originalServiceStation = new ServiceStation();

        public ServiceStationsForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
             
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class ComboBoxItem
        {
            public string Tag;
            public string Text;

            public ComboBoxItem(string tag, string text)
            {
                Tag = tag;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void ClearForm()
        {
            StationNameTextBox.Text = "";
            EnablePDFCheckBox.Checked = false;
            PDFGroupBox.Enabled = false;
            MaxNumBatchesUpDown.Text = "";
            EnableWeekendCheckBox.Checked = false;
            WeekendStartDateTimePicker.ResetText();
            WeekendStartDateTimePicker.Enabled = false;
            WeekendEndDateTimePicker.ResetText();
            WeekendEndDateTimePicker.Enabled = false;
            EnableWorkdayCheckBox.Checked = false;
            WorkdayStartDateTimePicker.ResetText();
            WorkdayStartDateTimePicker.Enabled = false;
            WorkdayEndDateTimePicker.ResetText();
            WorkdayEndDateTimePicker.Enabled = false;

            WatchFolderComboBox.Text = "";
            TargetFolderComboBox.Text = "";

            WeekendStartDateTimePicker.Format = DateTimePickerFormat.Custom;
            WeekendStartDateTimePicker.CustomFormat = "HH:mm";
            WeekendEndDateTimePicker.Format = DateTimePickerFormat.Custom;
            WeekendEndDateTimePicker.CustomFormat = "HH:mm";

            WorkdayStartDateTimePicker.Format = DateTimePickerFormat.Custom;
            WorkdayStartDateTimePicker.CustomFormat = "HH:mm";
            WorkdayEndDateTimePicker.Format = DateTimePickerFormat.Custom;
            WorkdayEndDateTimePicker.CustomFormat = "HH:mm";            
        }

        private void LoadData()
        {
            
            //StationsListView.Items.Clear();
            string returnMessage = "";
            Cursor.Current = Cursors.WaitCursor;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();

                       
            // Get Working Folders
            ResultWorkingFolders resultWorkingFolders = new ResultWorkingFolders();
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
            // Add Watch and target folders in Combo Boxes
            if (resultWorkingFolders.RecordsCount != 0)
            {
                foreach (var item in resultWorkingFolders.ReturnValue)
                {
                    WatchFolderComboBox.Items.Add(new ComboBoxItem(Convert.ToString(item.FolderID), item.Path));
                    TargetFolderComboBox.Items.Add(new ComboBoxItem(Convert.ToString(item.FolderID),item.Path));
                }
            }

            if (Data.GlovalVariables.transactionType == "New")
            {
                StationNameTextBox.Enabled = true;
            }
            else
            {
                StationNameTextBox.Enabled = false;
                // Get Current Service Information
                client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                URL = Data.GlovalVariables.BaseURL + "GeneralSettings/GetServiceStationByID";
                urlParameters = "?stationID=" + Data.GlovalVariables.currentServiceStationID;
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
                            originalServiceStation = station;
                            StationNameTextBox.Text = station.StationName;
                            if (station.PDFStationFlag)
                            {
                                PDFGroupBox.Enabled = true;
                                EnablePDFCheckBox.Checked = true;
                            }
                            else
                            {
                                PDFGroupBox.Enabled = false;
                                EnablePDFCheckBox.Checked = false;
                            }
                            MaxNumBatchesUpDown.Text = Convert.ToString(station.MaxNumberBatches);
                            if (station.WeekendFlag)
                            {
                                EnableWeekendCheckBox.Checked = true;
                                WeekendStartDateTimePicker.Value = Convert.ToDateTime(station.WeekendStartTime);
                                WeekendEndDateTimePicker.Value = Convert.ToDateTime(station.WeenkendEndTime);
                            }
                            else
                            {
                                EnableWeekendCheckBox.Checked = false;
                                WeekendStartDateTimePicker.ResetText();
                                WeekendStartDateTimePicker.Enabled = false;
                                WeekendEndDateTimePicker.ResetText();
                                WeekendEndDateTimePicker.Enabled = false;
                            }
                            if (station.WorkdayFlag)
                            {
                                EnableWorkdayCheckBox.Checked = true;
                                WorkdayStartDateTimePicker.Value = Convert.ToDateTime(station.WorkdayStartTime);
                                WorkdayEndDateTimePicker.Value = Convert.ToDateTime(station.WorkdayEndTime);
                            }
                            else
                            {
                                EnableWorkdayCheckBox.Checked = false;
                                WorkdayStartDateTimePicker.ResetText();
                                WorkdayStartDateTimePicker.Enabled = false;
                                WorkdayEndDateTimePicker.ResetText();
                                WorkdayEndDateTimePicker.Enabled = false;
                            }

                            foreach (ComboBoxItem item in WatchFolderComboBox.Items)
                            {
                                if (item.Tag == Convert.ToString(station.WatchFolderID))
                                {
                                    WatchFolderComboBox.Text = item.Text;
                                    break;
                                }                                    
                            }

                            foreach (ComboBoxItem item in TargetFolderComboBox.Items)
                            {
                                if (item.Tag == Convert.ToString(station.TargetFolderID))
                                {
                                    TargetFolderComboBox.Text = item.Text;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
                

        }

        private void ServiceStationsForm_Load(object sender, EventArgs e)
        {
            ClearForm();
            LoadData();
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
            try
            {
                ServiceStation serviceStation = new ServiceStation();
                ResultServiceStations resultServiceStations = new ResultServiceStations();
                string bodyString = "";
                string serviceStationJS = "";

                string returnMessage = "";
                Cursor.Current = Cursors.WaitCursor;
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                string urlParameters = "";
                string URL = "";
                HttpResponseMessage response = new HttpResponseMessage();

                // Validations Rules ....
                if (EnablePDFCheckBox.Checked)
                {
                    if (string.IsNullOrEmpty(WatchFolderComboBox.Text) || string.IsNullOrEmpty(TargetFolderComboBox.Text))
                    {
                        //continueOperation = false;
                        toolTip1.ToolTipTitle = "Warning ...";
                        if (string.IsNullOrEmpty(WatchFolderComboBox.Text))
                            toolTip1.Show("You need to set a value for the Watch Folder.", WatchFolderComboBox, 5000);
                        else
                            toolTip1.Show("You need to set a value for the Watch Folder.", TargetFolderComboBox, 5000);
                        return;
                    }
                    if (WatchFolderComboBox.Text == TargetFolderComboBox.Text)
                    {
                        // Do nothing
                        // For PDF Conversion, Watch Folder and Target Folder could be the same
                    }
                    if (EnableWeekendCheckBox.Checked)
                    {
                        if (WeekendStartDateTimePicker.Value >= WeekendEndDateTimePicker.Value)
                        {
                            toolTip1.ToolTipTitle = "Invalid Range.";
                            WeekendStartDateTimePicker.Focus();
                            toolTip1.Show("Invalid Range Values. Begin Time must be less than End Time value.", WeekendStartDateTimePicker, 5000);
                            WeekendStartDateTimePicker.Focus();
                            WeekendStartDateTimePicker.Select();
                            return;
                        }
                    }
                    if (EnableWorkdayCheckBox.Checked)
                    {
                        if (WorkdayStartDateTimePicker.Value >= WorkdayEndDateTimePicker.Value)
                        {
                            toolTip1.ToolTipTitle = "Invalid Range.";
                            WorkdayStartDateTimePicker.Focus();
                            toolTip1.Show("Invalid Range Values. Begin Time must be less than End Time value.", WorkdayStartDateTimePicker, 5000);
                            WorkdayStartDateTimePicker.Focus();
                            WorkdayStartDateTimePicker.Select();
                            return;
                        }
                    }
                    if (string.IsNullOrEmpty(MaxNumBatchesUpDown.Text))
                    {
                        toolTip1.ToolTipTitle = "Warning ...";
                        toolTip1.Show("You need to set a value for the Max Number of Batches to be processed for this PDF Station.", MaxNumBatchesUpDown, 5000);
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(MaxNumBatchesUpDown.Text) == 0)
                        {
                            toolTip1.ToolTipTitle = "Warning ...";
                            toolTip1.Show("Max Number of Batches cannot be 0.", MaxNumBatchesUpDown, 5000);
                            return;
                        }
                    }
                   
                    // Set PDF fields for Update/New transaction
                    serviceStation.PDFStationFlag = true;
                    serviceStation.MaxNumberBatches = Convert.ToInt32(MaxNumBatchesUpDown.Text);
                    serviceStation.WeekendFlag = EnableWeekendCheckBox.Checked;
                    serviceStation.WeekendStartTime = Convert.ToString(WeekendStartDateTimePicker.Value);
                    serviceStation.WeenkendEndTime = Convert.ToString(WeekendEndDateTimePicker.Value);
                    serviceStation.WorkdayFlag = EnableWorkdayCheckBox.Checked;
                    serviceStation.WorkdayStartTime = Convert.ToString(WorkdayStartDateTimePicker.Value);
                    serviceStation.WorkdayEndTime = Convert.ToString(WorkdayEndDateTimePicker.Value);
                    ComboBoxItem comboItem = (ComboBoxItem)WatchFolderComboBox.SelectedItem;
                    serviceStation.WatchFolderID = Convert.ToInt32(comboItem.Tag);
                    comboItem = (ComboBoxItem)TargetFolderComboBox.SelectedItem;
                    serviceStation.TargetFolderID = Convert.ToInt32(comboItem.Tag);
                }
                else
                {
                    // reset PDF fields for Update/New transaction
                    serviceStation.PDFStationFlag = false;
                    serviceStation.MaxNumberBatches = 0;
                    serviceStation.WeekendFlag = false;
                    serviceStation.WeekendStartTime = "";
                    serviceStation.WeenkendEndTime = "";
                    serviceStation.WorkdayFlag = false;
                    serviceStation.WorkdayStartTime = "";
                    serviceStation.WorkdayEndTime = "";
                    serviceStation.WatchFolderID = 0;
                    serviceStation.TargetFolderID = 0;
                    serviceStation.BackupFolderID = 0;
                }

                serviceStation.StationName = StationNameTextBox.Text;
                // Check if Service Station Name if and only if the Transactioon type is "New Service Sttaion"
                if (Data.GlovalVariables.transactionType == "New")
                {
                    // Get Service Stations
                    URL = BaseURL + "GeneralSettings/GetServiceStations";
                    urlParameters = "";
                    client.BaseAddress = new Uri(URL);
                    response = client.GetAsync(urlParameters).Result;
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultServiceStations = JsonConvert.DeserializeObject<ResultServiceStations>(returnMessage);
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        if (resultServiceStations.RecordsCount != 0)
                        {
                            foreach (var item in resultServiceStations.ReturnValue)
                            {
                                if (StationNameTextBox.Text == item.StationName.Trim())
                                {
                                    // Station Name Found
                                    toolTip1.ToolTipTitle = "Warning ...";
                                    toolTip1.Show("The Service Station Name entered already exist in the Database.", StationNameTextBox, 5000);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // The Station Name enter does not exist in the Database
                            // so continue
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultServiceStations.Message.Replace(". ", "\r\n") + resultServiceStations.Message, "New Service Station Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Add service station to the Database
                    serviceStationJS = JsonConvert.SerializeObject(serviceStation, Newtonsoft.Json.Formatting.Indented);
                    URL = BaseURL + "GeneralSettings/NewServiceStation";
                    bodyString = "'" + serviceStationJS + "'";

                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    using (HttpContent content = response_for_new.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultServiceStations = JsonConvert.DeserializeObject<ResultServiceStations>(returnMessage);
                    }

                    if (response_for_new.IsSuccessStatusCode)
                    {
                        // Set the value of the new Field to a gloval variable
                        if (resultServiceStations.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultServiceStations.Message.Replace(". ", "\r\n"),
                                            "New Service Station Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.newServiceStaionList.Add(serviceStation.StationName);
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                                ClearForm();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultServiceStations.Message.Replace(". ", "\r\n") + resultServiceStations.Exception,
                                        "New Service Station Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Transaction Type = Update
                    // Update Service station information
                    serviceStation.StationID = Data.GlovalVariables.currentServiceStationID;
                    serviceStationJS = JsonConvert.SerializeObject(serviceStation, Newtonsoft.Json.Formatting.Indented);
                    URL = BaseURL + "GeneralSettings/UpdateServiceStation";
                    bodyString = "'" + serviceStationJS + "'";

                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_new).Result;

                    using (HttpContent content = response_for_update.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultServiceStations = JsonConvert.DeserializeObject<ResultServiceStations>(returnMessage);
                    }

                    if (response_for_update.IsSuccessStatusCode)
                    {
                        if (action == "SaveAndExit") this.Close();
                        else
                        {
                            // Do nothing
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultServiceStations.Message.Replace(". ", "\r\n"), "Update Service Station Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }                
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StationNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validHostName(sender, e);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (Data.GlovalVariables.transactionType == "New")
            {
                ClearForm();
            }
            else
            {                
                StationNameTextBox.Text = originalServiceStation.StationName;
                if (originalServiceStation.PDFStationFlag)
                {
                    PDFGroupBox.Enabled = true;
                    EnablePDFCheckBox.Checked = true;
                }
                else
                {
                    PDFGroupBox.Enabled = false;
                    EnablePDFCheckBox.Checked = false;
                }
                MaxNumBatchesUpDown.Text = Convert.ToString(originalServiceStation.MaxNumberBatches);
                if (originalServiceStation.WeekendFlag)
                {
                    EnableWeekendCheckBox.Checked = true;
                    WeekendStartDateTimePicker.Value = Convert.ToDateTime(originalServiceStation.WeekendStartTime);
                    WeekendEndDateTimePicker.Value = Convert.ToDateTime(originalServiceStation.WeenkendEndTime);
                }
                else
                {
                    EnableWeekendCheckBox.Checked = false;
                    WeekendStartDateTimePicker.ResetText();
                    WeekendStartDateTimePicker.Enabled = false;
                    WeekendEndDateTimePicker.ResetText();
                    WeekendEndDateTimePicker.Enabled = false;
                }
                if (originalServiceStation.WorkdayFlag)
                {
                    EnableWorkdayCheckBox.Checked = true;
                    WorkdayStartDateTimePicker.Value = Convert.ToDateTime(originalServiceStation.WorkdayStartTime);
                    WorkdayEndDateTimePicker.Value = Convert.ToDateTime(originalServiceStation.WorkdayEndTime);
                }
                else
                {
                    EnableWorkdayCheckBox.Checked = false;
                    WorkdayStartDateTimePicker.ResetText();
                    WorkdayStartDateTimePicker.Enabled = false;
                    WorkdayEndDateTimePicker.ResetText();
                    WorkdayEndDateTimePicker.Enabled = false;
                }

                foreach (ComboBoxItem item in WatchFolderComboBox.Items)
                {
                    if (item.Tag == Convert.ToString(originalServiceStation.WatchFolderID))
                    {
                        WatchFolderComboBox.Text = item.Text;
                        break;
                    }
                }

                foreach (ComboBoxItem item in TargetFolderComboBox.Items)
                {
                    if (item.Tag == Convert.ToString(originalServiceStation.TargetFolderID))
                    {
                        TargetFolderComboBox.Text = item.Text;
                        break;
                    }
                }
            }
        }

        private void EnablePDFCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EnablePDFCheckBox.Checked)
            {
                PDFGroupBox.Enabled = true;
            }
            else
            {
                PDFGroupBox.Enabled = false;
            }
        }

        private void EnableWeekendCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableWeekendCheckBox.Checked)
            {
                WeekendStartDateTimePicker.Enabled = true;
                WeekendEndDateTimePicker.Enabled = true;
            }
            else
            {
                WeekendStartDateTimePicker.Enabled = false;
                WeekendEndDateTimePicker.Enabled = false;
            }
        }

        private void EnableWorkdayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableWorkdayCheckBox.Checked)
            {
                WorkdayStartDateTimePicker.Enabled = true;
                WorkdayEndDateTimePicker.Enabled = true;
            }
            else
            {
                WorkdayStartDateTimePicker.Enabled = false;
                WorkdayEndDateTimePicker.Enabled = false;
            }
        }
    }
}


