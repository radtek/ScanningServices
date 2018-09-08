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

namespace ScanningServicesAdmin.Forms
{
    public partial class SchedulingForm : Form
    {

        public string scheduleString = "";
        public ScheduleTime scheduleOriginal = new ScheduleTime();
        public Boolean enableFlagOriginal;
        public int stationIDOriginal = 0;
        public string serviceStationNameOriginal = "";

        public SchedulingForm()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
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

        private void SchedulingForm_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            //this.Text = "BATCH SELIVERY SETTING FOR STATION " + currentServiceStationName;

            List<ServiceStation> serviceStations = new List<ServiceStation>();
            ProcessNameLabel.Text = Data.GlovalVariables.currentProcessName;
            JobNameLabel.Text = Data.GlovalVariables.currentJobName;
            

            Cursor.Current = Cursors.WaitCursor;
            string returnMessage = "";
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            
            // Get Service Stations
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            ResultServiceStations resultResultStations = new ResultServiceStations();
            URL = BaseURL + "GeneralSettings/GetServiceStations";
            urlParameters = "";
            client.BaseAddress = new Uri(URL);
            response = client.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultResultStations = JsonConvert.DeserializeObject<ResultServiceStations>(returnMessage);
            }
            if (response.IsSuccessStatusCode)
            {
                ServiceStationsComboBox.Items.Add("");
                serviceStations = resultResultStations.ReturnValue;
                foreach (ServiceStation serviceStation in serviceStations)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = serviceStation.StationName.Trim();
                    item.ID = serviceStation.StationID;
                    ServiceStationsComboBox.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            serviceStationNameOriginal = Data.GlovalVariables.currentServiceStationName.Trim();
            ServiceStationsComboBox.Text = Data.GlovalVariables.currentServiceStationName.Trim();

            //if (Data.GlovalVariables.currentProcessName == "Batch Delivery")
            //{
            //    stationIDOriginal = Data.GlovalVariables.currentServiceStationID;
            //    ServiceStationsComboBox.Enabled = false;
            //}
            //else
            //{
            stationIDOriginal = 0;
            ServiceStationsComboBox.Enabled = true;
            //}                

            // Windows control initialization
            WeeklyGroupBox.Enabled = false;
            DailyGroupBox.Enabled = false;

            WeeklyGroupBox.Enabled = true;
            RepeatTaskEveryUpDown.Enabled = true;
            RepeatEveryGroupBox.Enabled = false;
            BeginDateTimePicker.Enabled = false;
            EndDateTimePicker.Enabled = false;
            RangeCheckBox.Enabled = false;
            StartRadioButton.Enabled = true;
            StartDateTimePicker.Enabled = false;
            RepeatTaskEveryUpDown.Enabled = false;
            WeeklyGroupBox.Enabled = false;
            BeginDateTimePicker.Format = DateTimePickerFormat.Custom;
            BeginDateTimePicker.CustomFormat = "HH tt";
            EndDateTimePicker.Format = DateTimePickerFormat.Custom;
            EndDateTimePicker.CustomFormat = "HH tt";
            StartDateTimePicker.Format = DateTimePickerFormat.Custom;
            StartDateTimePicker.CustomFormat = "HH:mm tt";
            DailyRadioButton.Checked = false;
            WeeklyRadioButton.Checked = false;
            RepeatTaskEveryRadioButton.Checked = false;
            StartRadioButton.Checked = false;

            switch (Data.GlovalVariables.transactionType)
            {
                case "New":
                    // Set default entries for new Job-Process
                    DailyGroupBox.Enabled = false;
                    WeeklyGroupBox.Enabled = false;
                    RepeatTaskEveryUpDown.Enabled = false;
                    RepeatEveryGroupBox.Enabled = false;
                    RangeCheckBox.Enabled = false;

                    BeginDateTimePicker.Enabled = false;
                    EndDateTimePicker.Enabled = false;
                    StartDateTimePicker.Enabled = false;

                    enableFlagOriginal = false;
                    scheduleOriginal.dailyFlag = DailyRadioButton.Checked;
                    scheduleOriginal.recurEveryDays = RecurDaysTextBoxUpDown.Text;
                    scheduleOriginal.dayOfTheWeekFlag = WeeklyRadioButton.Checked;
                    scheduleOriginal.sunday = SundayCheckBox.Checked;
                    scheduleOriginal.monday = MondayCheckBox.Checked;
                    scheduleOriginal.tuesday = TuesdayCheckBox.Checked;
                    scheduleOriginal.wednesday = WednesdayCheckBox.Checked;
                    scheduleOriginal.thursday = ThursdayCheckBox.Checked;
                    scheduleOriginal.friday = FridayCheckBox.Checked;
                    scheduleOriginal.saturday = SaturdayCheckBox.Checked;
                    scheduleOriginal.repeatTaskFlag = RepeatTaskEveryRadioButton.Checked;
                    scheduleOriginal.repeatTaskTimes = RepeatTaskEveryUpDown.Text;
                    scheduleOriginal.repeatEveryHoursFlag = RepeatHoursRadioButton.Checked;
                    scheduleOriginal.repeatEveryMinutesFlag = RepeatMinutesRadioButton.Checked;
                    scheduleOriginal.repeatTaskRange = RangeCheckBox.Checked;
                    scheduleOriginal.taskBeginHour = "0";
                    scheduleOriginal.taskEndHour = "0";
                    scheduleOriginal.startTaskAtFlag = StartRadioButton.Checked;
                    scheduleOriginal.startTaskHour = "0";
                    scheduleOriginal.startTaskMinute = "0";  
                    break;

                case "Update":
                    ResultProcesses resultProcesses = new ResultProcesses();
                    resultProcesses = GetProcessByIDs(Data.GlovalVariables.currentProcessID, Data.GlovalVariables.currentJobID,
                                                      Data.GlovalVariables.currentServiceStationID, Data.GlovalVariables.currentPDFStationID);
                    if (resultProcesses.RecordsCount > 0)
                    {                       
                        EnablecheckBox.Checked = resultProcesses.ReturnValue[0].EnableFlag;
                        enableFlagOriginal = resultProcesses.ReturnValue[0].EnableFlag;
                        ScheduleTime schedule = new ScheduleTime();
                        schedule = resultProcesses.ReturnValue[0].Schedule;
                        
                        scheduleOriginal = schedule;                        
                        DailyRadioButton.Checked = schedule.dailyFlag;
                        RecurDaysTextBoxUpDown.Text = schedule.recurEveryDays;
                        WeeklyRadioButton.Checked = schedule.dayOfTheWeekFlag;
                        SundayCheckBox.Checked = schedule.sunday;
                        MondayCheckBox.Checked = schedule.monday;
                        TuesdayCheckBox.Checked = schedule.tuesday;
                        WednesdayCheckBox.Checked = schedule.wednesday;
                        ThursdayCheckBox.Checked = schedule.thursday;
                        FridayCheckBox.Checked = schedule.friday;
                        SaturdayCheckBox.Checked = schedule.saturday;
                        RepeatTaskEveryRadioButton.Checked = schedule.repeatTaskFlag;
                        RepeatTaskEveryUpDown.Text = schedule.repeatTaskTimes;
                        RepeatHoursRadioButton.Checked = schedule.repeatEveryHoursFlag;
                        RepeatMinutesRadioButton.Checked = schedule.repeatEveryMinutesFlag;
                        RangeCheckBox.Checked = schedule.repeatTaskRange;
                        BeginDateTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(schedule.taskBeginHour), 0, 0);
                        EndDateTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(schedule.taskEndHour), 0, 0);
                        StartRadioButton.Checked = schedule.startTaskAtFlag;
                        StartDateTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(schedule.startTaskHour), Convert.ToInt32(schedule.startTaskMinute), 0);
                    }
                    break;
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
            urlParameters = "?processID=" + processID.ToString() + "&jobID=" + jobID.ToString() + "&stationID=" + stationID.ToString() + "&pdfStationID=" + pdfStationID.ToString();
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

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DailyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (DailyRadioButton.Checked)
            {
                DailyGroupBox.Enabled = true;
                WeeklyGroupBox.Enabled = false;
            }
        }

        private void WeeklyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (WeeklyRadioButton.Checked)
            {
                DailyGroupBox.Enabled = false;
                WeeklyGroupBox.Enabled = true;
                WeeklyGroupBox.Enabled = true;
            }
        }

        private void RepeatTaskEveryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RangeCheckBox.Enabled = false;
            RepeatTaskEveryUpDown.Enabled = false;
            RepeatEveryGroupBox.Enabled = false;
            if (RepeatTaskEveryRadioButton.Checked)
            {
                RepeatTaskEveryUpDown.Enabled = true;
                RepeatEveryGroupBox.Enabled = true;
                if (!(RepeatHoursRadioButton.Checked || RepeatMinutesRadioButton.Checked))
                {
                    RepeatHoursRadioButton.Checked = true;
                }
                if (RangeCheckBox.Checked)
                {
                    BeginDateTimePicker.Enabled = true;
                    EndDateTimePicker.Enabled = true;
                }
                else
                {
                    BeginDateTimePicker.Enabled = false;
                    EndDateTimePicker.Enabled = false;
                }

                RangeCheckBox.Enabled = true;
                StartDateTimePicker.Enabled = false;
            }
        }

        private void StartRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            StartDateTimePicker.Enabled = false;
            if (StartRadioButton.Checked)
            {
                RepeatTaskEveryUpDown.Enabled = false;
                RepeatEveryGroupBox.Enabled = false;
                BeginDateTimePicker.Enabled = false;
                EndDateTimePicker.Enabled = false;
                RangeCheckBox.Enabled = false;
                StartDateTimePicker.Enabled = true;
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
            Process process = new Process();
            string processJS = "";
            string bodyString = "";
            string returnMessage = "";
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            ResultJobProcesses resultProcesses = new ResultJobProcesses();

            // Station must have a value
            if (string.IsNullOrEmpty(ServiceStationsComboBox.Text))
            {
                toolTip1.ToolTipTitle = "Invalid Station Name.";
                toolTip1.Show("Ths Station Name is a required field to perform this operation.", ServiceStationsComboBox, 5000);
                ServiceStationsComboBox.Focus();
                ServiceStationsComboBox.Select();
                return;
            }

            // Validate Time Range values
            if (RangeCheckBox.Checked)
            {
                if (BeginDateTimePicker.Value.Hour >= EndDateTimePicker.Value.Hour)
                {
                    toolTip1.ToolTipTitle = "Invalid Range.";
                    EndDateTimePicker.Focus();
                    toolTip1.Show("Invalid Range Values. Begin Time must be less than End Time value.", EndDateTimePicker, 5000);
                    EndDateTimePicker.Focus();
                    EndDateTimePicker.Select();
                    return;
                }
            }

            // Build Task Schedule in JS format           
            ScheduleTime schedule = new ScheduleTime();
            schedule.dailyFlag = DailyRadioButton.Checked;
            if (DailyRadioButton.Checked)
            {
                schedule.recurEveryDays = RecurDaysTextBoxUpDown.Text;
                SundayCheckBox.Checked = false;
                MondayCheckBox.Checked = false;
                TuesdayCheckBox.Checked = false;
                WednesdayCheckBox.Checked = false;
                ThursdayCheckBox.Checked = false;
                FridayCheckBox.Checked = false;
                SaturdayCheckBox.Checked = false;
            }
            else
            {
                schedule.recurEveryDays = "";
            }
            schedule.dayOfTheWeekFlag = WeeklyRadioButton.Checked;
            if (WeeklyRadioButton.Checked)
            {
                schedule.sunday = SundayCheckBox.Checked;
                schedule.monday = MondayCheckBox.Checked;
                schedule.tuesday = TuesdayCheckBox.Checked;
                schedule.wednesday = WednesdayCheckBox.Checked;
                schedule.thursday = ThursdayCheckBox.Checked;
                schedule.friday = FridayCheckBox.Checked;
                schedule.saturday = SaturdayCheckBox.Checked;
            }
            schedule.repeatTaskFlag = RepeatTaskEveryRadioButton.Checked;
            if (RepeatTaskEveryRadioButton.Checked)
            {
                schedule.repeatTaskTimes = RepeatTaskEveryUpDown.Text;
                schedule.repeatEveryHoursFlag = RepeatHoursRadioButton.Checked;
                schedule.repeatEveryMinutesFlag = RepeatMinutesRadioButton.Checked;
            }
            else
            {
                schedule.repeatTaskTimes = "";
                schedule.repeatEveryHoursFlag = false;
                schedule.repeatEveryMinutesFlag = false;
            }
            schedule.repeatTaskRange = RangeCheckBox.Checked;
            if (RangeCheckBox.Checked)
            {
                schedule.taskBeginHour = BeginDateTimePicker.Value.Hour.ToString();
                schedule.taskEndHour = EndDateTimePicker.Value.Hour.ToString();
            }
            else
            {
                schedule.taskBeginHour = "0";
                schedule.taskEndHour = "0";
            }
            schedule.startTaskAtFlag = StartRadioButton.Checked;
            if (StartRadioButton.Checked)
            {
                schedule.startTaskMinute = StartDateTimePicker.Value.Minute.ToString();
                schedule.startTaskHour = StartDateTimePicker.Value.Hour.ToString();
            }
            else
            {
                schedule.startTaskMinute = "0";
                schedule.startTaskHour = "0";
            }

            process.Schedule = schedule;
            process.ProcessID = Data.GlovalVariables.currentProcessID;
            process.StationID = Data.GlovalVariables.currentServiceStationID;
            process.JobID = Data.GlovalVariables.currentJobID;
            process.PDFStationID = Data.GlovalVariables.currentPDFStationID;
            process.EnableFlag = EnablecheckBox.Checked;

            processJS = JsonConvert.SerializeObject(process, Newtonsoft.Json.Formatting.Indented);
            processJS = processJS.Replace(@"\", "\\\\");
            URL = BaseURL + "Process/UpdateProcess";
            bodyString = "'" + processJS + "'";

            HttpContent body_for_update = new StringContent(bodyString);
            body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

            using (HttpContent content = response_for_update.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultProcesses = JsonConvert.DeserializeObject<ResultJobProcesses>(returnMessage);
            }

            if (response_for_update.IsSuccessStatusCode)
            {
                // Set the value of the new customer to a gloval variable
                if (resultProcesses.ReturnCode == -1)
                {
                    MessageBox.Show("Warning:" + "\r\n" + resultProcesses.Message.Replace(". ", "\r\n"), "Update Process Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (action == "SaveAndExit") this.Close();
                    else
                    {
                        // Do nothing
                    }
                }
            }
            else
            {
                MessageBox.Show("Error:" + "\r\n" + resultProcesses.Message.Replace(". ", "\r\n"), "Update Process Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void RangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RangeCheckBox.Checked)
            {
                EndDateTimePicker.Enabled = true;
                BeginDateTimePicker.Enabled = true;
            }
            else
            {
                EndDateTimePicker.Enabled = false;
                BeginDateTimePicker.Enabled = false;
            }
        }

        private void RecurDaysTextBoxUpDown_Leave(object sender, EventArgs e)
        {
            if (!Validation.IsValidInteger(RecurDaysTextBoxUpDown.Text))
            {
                toolTip1.ToolTipTitle = "Invalid Value";
                toolTip1.Show("The value you entered is not a valid. Please change the value.", RecurDaysTextBoxUpDown, 5000);
                RecurDaysTextBoxUpDown.Focus();
                RecurDaysTextBoxUpDown.Select();
            }
        }

        private void RepeatTaskEveryUpDown_Leave(object sender, EventArgs e)
        {
            if (!Validation.IsValidInteger(RepeatTaskEveryUpDown.Text))
            {
                toolTip1.ToolTipTitle = "Invalid Value";
                toolTip1.Show("The value you entered is not a valid. Please change the value.", RepeatTaskEveryUpDown, 5000);
                RepeatTaskEveryUpDown.Focus();
                RepeatTaskEveryUpDown.Select();
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ServiceStationsComboBox.Text = serviceStationNameOriginal;
            //if (Data.GlovalVariables.currentProcessName != "Batch Delivery")
            //{
            //    if (stationIDOriginal == 0)
            //        ServiceStationsComboBox.Text = "";
            //    else
            //    {
            //        foreach (ComboboxItem item in ServiceStationsComboBox.Items)
            //        {
            //            if (stationIDOriginal == item.ID)
            //            {
            //                ServiceStationsComboBox.Text = item.Text;
            //                break;
            //            }
            //        }
            //    }
            //}
            EnablecheckBox.Checked = enableFlagOriginal;
            DailyRadioButton.Checked = scheduleOriginal.dailyFlag;
            RecurDaysTextBoxUpDown.Text = scheduleOriginal.recurEveryDays;
            WeeklyRadioButton.Checked = scheduleOriginal.dayOfTheWeekFlag;
            SundayCheckBox.Checked = scheduleOriginal.sunday;
            MondayCheckBox.Checked = scheduleOriginal.monday;
            TuesdayCheckBox.Checked = scheduleOriginal.tuesday;
            WednesdayCheckBox.Checked = scheduleOriginal.wednesday;
            ThursdayCheckBox.Checked = scheduleOriginal.thursday;
            FridayCheckBox.Checked = scheduleOriginal.friday;
            SaturdayCheckBox.Checked = scheduleOriginal.saturday;
            RepeatTaskEveryRadioButton.Checked = scheduleOriginal.repeatTaskFlag;
            RepeatTaskEveryUpDown.Text = scheduleOriginal.repeatTaskTimes;
            RepeatHoursRadioButton.Checked = scheduleOriginal.repeatEveryHoursFlag;
            RepeatMinutesRadioButton.Checked = scheduleOriginal.repeatEveryMinutesFlag;
            RangeCheckBox.Checked = scheduleOriginal.repeatTaskRange;
            BeginDateTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(scheduleOriginal.taskBeginHour), 0, 0);
            EndDateTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(scheduleOriginal.taskEndHour), 0, 0);
            StartRadioButton.Checked = scheduleOriginal.startTaskAtFlag;
            StartDateTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(scheduleOriginal.startTaskHour), Convert.ToInt32(scheduleOriginal.startTaskMinute), 0);
        }

        private void ServiceStationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem item = new ComboboxItem();
            if (!string.IsNullOrEmpty(ServiceStationsComboBox.Text))
            {
                item = (ComboboxItem)ServiceStationsComboBox.SelectedItem;
                Data.GlovalVariables.currentServiceStationID = item.ID;
            }
           
        }
    }
}
