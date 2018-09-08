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

namespace ScanningServicesAdmin.Forms
{
    public partial class ReportForm : Form
    {
        //public string BaseURL = "http://localhost:47063" + "/api/";
        public string emailStingTest = "lalo@cdlac.com,pedro@cdlac.com,juan@cdlac.com";
        //-------------------------------------------------------------------------------------------------------------
        public string Title1ColorCode = "#993300"; //Red
        public string Title1Content = "COMPU - DATA International, LLC";
        public string Title1FontBoldTagBegin = "<b>";
        public string Title1FontBoldTagEnd = "</b>";
        public string Title1FontSize = "6";
        //-------------------------------------------------------------------------------------------------------------
        public string Title2ColorCode = "#000000"; //Black
        public string Title2Content = "Scanning Services System - REPORT NAME";
        public string Title2FontBoldTagBegin = "";
        public string Title2FontBoldTagEnd = "";
        public string Title2FontSize = "5";
        //-------------------------------------------------------------------------------------------------------------
        public string Title3ColorCode = "#993300"; //Red
        public string Title3Content = "DESCRIBE WHAT YOU EXPECT TO SEE IN THE TABLE";
        public string Title3FontBoldTagBegin = "";
        public string Title3FontBoldTagEnd = "";
        public string Title3FontSize = "4";
        //-------------------------------------------------------------------------------------------------------------
        public string TableHeaderFontColorCode = "#2980b9"; //Black
        public string TableHeaderFontBoldTagBegin = "<b>";
        public string TableHeaderFontBoldTagEnd = "</b>";
        public string TableHeaderBackColorCode = "#fdf5e6"; //Black
        public string TableHeaderFontSize = "4";
        //-------------------------------------------------------------------------------------------------------------
        public string TableColumnNmaesFontColorCode = "#000000"; //Black
        public string TableColumnNamesFontBoldTagBegin = "<b>";
        public string TableColumnNamesFontBoldTagEnd = "</b>";
        public string TableColumnNamesBackColorCode = "#fdf5e6"; //Black
        public string TableColumnNamesFontSize = "3";
        //-------------------------------------------------------------------------------------------------------------
        public Boolean formInitialization = true;
        public Boolean validateReportParameters = true;
        public ScheduleTime scheduleOriginal = new ScheduleTime();

        public ReportForm()
        {
            InitializeComponent();
            ReportNameButton.Text = Data.GlovalVariables.currentReportName.ToUpper() + " - FORMAT PREVIEW";
            ReportNameTextBox.Text = Data.GlovalVariables.currentReportName;
        }

        private void PropertiesTabPage_Click(object sender, EventArgs e)
        {

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Show the dialog.
            DialogResult result = fontDialog1.ShowDialog();
            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                // Get Font.
                Font font = fontDialog1.Font;
                // Set TextBox properties.
               // button2.Text = string.Format("Font: {0}", font.Name);
              //  button2.Font = font;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                UpdateButton.BackColor = colorDialog1.Color;
                var xyz = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");


            }
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            ReportsLoad();
            formInitialization = false;
            reportViewUpdate();
            webBrowser1.Refresh();
        }

        private void ReportsLoad()
        {
            // NOTES:
            //  TemplateID + CustomerID make a unique record in Reports Database Table
            //  so, make sure the Node id in the Tree == Template ID
            //
            // When a System Report is selected
            //      Current Customer ID = 0
            //      Template ID = XYZ
            //      Search in Reports Table by Customer ID = 0 and Tempate ID = XYZ
            //      If result returns a valur --> means that the report already exist
            //      Otherwise, we only relays in the Report Template, so 
            //      search by Template ID will give us the Default for this report
            //
            // When a Customer Report is selected
            //      Current Customer ID = ABC
            //      Template ID = XYZ
            //      Search in Reports Table by Customer ID = ABC and Tempate ID = XYZ
            //      If result returns a valur --> means that the report already exist
            //      Otherwise, we only relays in the Report Template, so 
            //      search by Template ID will give us the Default for this report

            // Windows control initialization
            DailyGroupBox.Enabled = true;
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

            // Build Custom Colors based on previous reports definitions           
            List<int> customColors = new List<int>();
            ResultReports resultReports = new ResultReports();
            resultReports = DBTransactions.GetReports();
            if (resultReports.RecordsCount > 0)
            {
                foreach (Report report in resultReports.ReturnValue)
                {
                    if (!customColors.Contains(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableColumnNamesBackColor))))
                    {
                        customColors.Add(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableColumnNamesBackColor)));
                    }
                    if (!customColors.Contains(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableColumnNamesFontColor))))
                    {
                       customColors.Add(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableColumnNamesFontColor)));
                    }
                    if (!customColors.Contains(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableHeaderBackColor))))
                    {
                        customColors.Add(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableHeaderBackColor)));
                    }
                    if (!customColors.Contains(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableHeaderFontColor))))
                    {
                        customColors.Add(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TableHeaderFontColor)));
                    }
                    if (!customColors.Contains(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TitleFontColor1))))
                    {
                        customColors.Add(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TitleFontColor1)));
                    }
                    if (!customColors.Contains(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TitleFontColor2))))
                    {
                        customColors.Add(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TitleFontColor2)));
                    }
                    if (!customColors.Contains(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TitleFontColor3))))
                    {
                        customColors.Add(ColorTranslator.ToOle(ColorTranslator.FromHtml(report.TitleFontColor3)));
                    }
                }
            }           

            colorDialog1.CustomColors = customColors.ToArray();

            //reportViewUpdate();
            string parameterValue;
            string returnMessage = "";
            Cursor.Current = Cursors.WaitCursor;
            HttpClient client1 = new HttpClient();
            HttpClient client2 = new HttpClient();
            client1.Timeout = TimeSpan.FromMinutes(15);
            client2.Timeout = TimeSpan.FromMinutes(15);
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();                    

            // Setting Parameters List Header
            ParametersListView.Columns.Add("Name", 250, HorizontalAlignment.Left);
            ParametersListView.Columns.Add("Value", 100, HorizontalAlignment.Left);
            ParametersListView.Columns.Add("Type", 100, HorizontalAlignment.Left);
            ParametersListView.Columns.Add("Required", 100, HorizontalAlignment.Left);            

            // Get Report Information by Customer ID and Template ID
            ResultReports resultReport = new ResultReports();
            URL = BaseURL + "Reports/GetReportByCustomerAndTemplateID";
            urlParameters = "?customerID=" + Data.GlovalVariables.currentCustomerID + "&" + "templateID=" + Data.GlovalVariables.currentTemplateID;
            client1.BaseAddress = new Uri(URL);
            response = client1.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultReport = JsonConvert.DeserializeObject<ResultReports>(returnMessage);
            }

            ReportNameTextBox.Text = Data.GlovalVariables.currentReportName.ToUpper();
            if (resultReport.RecordsCount == 0)
            {
                DailyGroupBox.Enabled = false;
                WeeklyGroupBox.Enabled = false;
                RepeatTaskEveryUpDown.Enabled = false;
                RepeatEveryGroupBox.Enabled = false;
                RangeCheckBox.Enabled = false;

                BeginDateTimePicker.Enabled = false;
                EndDateTimePicker.Enabled = false;
                StartDateTimePicker.Enabled = false;

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

                // Use Report Default Values
                // Report Id = 0 means that there is no Record for this report in  the Database
                Data.GlovalVariables.currentReportID = 0;
                ReportEnableFlagCheckBox.Checked = false;
                Title1FontSizeNmericUpDown.Value = Convert.ToInt32(Title1FontSize);
                Title2FontSizeNmericUpDown.Value = Convert.ToInt32(Title2FontSize);
                Title3FontSizeNmericUpDown.Value = Convert.ToInt32(Title3FontSize);
                Title1FontColorButton.BackColor = ColorTranslator.FromHtml(Title1ColorCode);
                Title2FontColorButton.BackColor = ColorTranslator.FromHtml(Title2ColorCode);
                Title3FontColorButton.BackColor = ColorTranslator.FromHtml(Title3ColorCode);
                if (Title1FontBoldTagBegin == "<b>")
                    Tile1BoldCheckBox.Checked = true;
                else
                    Tile1BoldCheckBox.Checked = false;
                if (Title2FontBoldTagBegin == "<b>")
                    Tile2BoldCheckBox.Checked = true;
                else
                    Tile2BoldCheckBox.Checked = false;
                if (Title3FontBoldTagBegin == "<b>")
                    Tile3BoldCheckBox.Checked = true;
                else
                    Tile3BoldCheckBox.Checked = false;

                MainTitleContentTextBox.Text = Title1Content;
                SecundaryTitleContentTextBox.Text = Title2Content;
                ComplementaryTitleContentTextBox.Text = Title3Content;

                TableHeaderFontSizeNmericUpDown.Value = Convert.ToInt32(TableHeaderFontSize);
                if (TableHeaderFontBoldTagBegin == "<b>")
                    TableHeaderFontBoldCheckBox.Checked = true;
                else
                    TableHeaderFontBoldCheckBox.Checked = false;
                TableHeaderFontColorButton.BackColor = ColorTranslator.FromHtml(TableHeaderFontColorCode);
                TableHeaderBackColorButton.BackColor = ColorTranslator.FromHtml(TableHeaderBackColorCode);

                TableColumnNamesFontSizeNmericUpDown.Value = Convert.ToInt32(TableColumnNamesFontSize);
                if (TableColumnNamesFontBoldTagBegin == "<b>")
                    TableColumnsFontBoldCheckBox.Checked = true;
                else
                    TableColumnsFontBoldCheckBox.Checked = false;
                TableColumnsFontColorButton.BackColor = ColorTranslator.FromHtml(TableColumnNmaesFontColorCode);
                TableColumnsBackColorButton.BackColor = ColorTranslator.FromHtml(TableColumnNamesBackColorCode);
                               
            }
            else
            {
                // Job-Process already exist
                //Data.GlovalVariables.currentProcessID = resultProcess.ReturnValue[0].ProcessID;
                ScheduleTime schedule = new ScheduleTime();
                schedule = resultReport.ReturnValue[0].Schedule;
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

                // Set Report Values
                Data.GlovalVariables.currentReportID = resultReport.ReturnValue[0].ReportID;
                EmailSubjectTextBox.Text = resultReport.ReturnValue[0].EmailSubject;
                ReportEnableFlagCheckBox.Checked = resultReport.ReturnValue[0].EnableFlag;
                Title1FontSizeNmericUpDown.Value = Convert.ToInt32(resultReport.ReturnValue[0].TitleFontSize1);
                Title2FontSizeNmericUpDown.Value = Convert.ToInt32(resultReport.ReturnValue[0].TitleFontSize2);
                Title3FontSizeNmericUpDown.Value = Convert.ToInt32(resultReport.ReturnValue[0].TitleFontSize3);
                Title1FontColorButton.BackColor = ColorTranslator.FromHtml(resultReport.ReturnValue[0].TitleFontColor1);
                Title2FontColorButton.BackColor = ColorTranslator.FromHtml(resultReport.ReturnValue[0].TitleFontColor2);
                Title3FontColorButton.BackColor = ColorTranslator.FromHtml(resultReport.ReturnValue[0].TitleFontColor3);
                if (resultReport.ReturnValue[0].TitleFontBoldFlag1)
                {
                    Tile1BoldCheckBox.Checked = true;
                    Title1FontBoldTagBegin = "<b>";
                    Title1FontBoldTagEnd = "</b>";
                }
                else
                {
                    Tile1BoldCheckBox.Checked = false;
                    Title1FontBoldTagBegin = "";
                    Title1FontBoldTagEnd = "";
                }

                if (resultReport.ReturnValue[0].TitleFontBoldFlag2)
                {
                    Tile2BoldCheckBox.Checked = true;
                    Title2FontBoldTagBegin = "<b>";
                    Title2FontBoldTagEnd = "</b>";
                }
                else
                {
                    Tile2BoldCheckBox.Checked = false;
                    Title2FontBoldTagBegin = "";
                    Title2FontBoldTagEnd = "";
                }

                if (resultReport.ReturnValue[0].TitleFontBoldFlag3)
                {
                    Tile3BoldCheckBox.Checked = true;
                    Title3FontBoldTagBegin = "<b>";
                    Title3FontBoldTagEnd = "</b>";
                }
                else
                {
                    Tile3BoldCheckBox.Checked = false;
                    Title3FontBoldTagBegin = "";
                    Title3FontBoldTagEnd = "";
                }

                MainTitleContentTextBox.Text = resultReport.ReturnValue[0].TitleContent1;
                SecundaryTitleContentTextBox.Text = resultReport.ReturnValue[0].TitleContent2;
                ComplementaryTitleContentTextBox.Text = resultReport.ReturnValue[0].TitleContent3;
                // ------------------------------------------------------------
                TableHeaderFontSizeNmericUpDown.Value = Convert.ToInt32(resultReport.ReturnValue[0].TableHeaderFontSize);
                if (resultReport.ReturnValue[0].TableColumnNamesFontBoldFlag)
                {
                    TableHeaderFontBoldCheckBox.Checked = true;
                    TableHeaderFontBoldTagBegin = "<b>";
                    TableHeaderFontBoldTagEnd = "</b>";
                }                   
                else
                {
                    TableHeaderFontBoldCheckBox.Checked = false;
                    TableHeaderFontBoldTagBegin = "";
                    TableHeaderFontBoldTagEnd = "";
                }

                TableHeaderFontColorButton.BackColor = ColorTranslator.FromHtml(resultReport.ReturnValue[0].TableHeaderFontColor);
                TableHeaderBackColorButton.BackColor = ColorTranslator.FromHtml(resultReport.ReturnValue[0].TableHeaderBackColor);

                TableColumnNamesFontSizeNmericUpDown.Value = Convert.ToInt32(resultReport.ReturnValue[0].TableColumnNamesFontSize);
                if (resultReport.ReturnValue[0].TableColumnNamesFontBoldFlag)
                {
                    TableColumnsFontBoldCheckBox.Checked = true;
                    TableColumnNamesFontBoldTagBegin = "<b>";
                    TableColumnNamesFontBoldTagEnd = "</b>";
                }                   
                else
                {
                    TableColumnsFontBoldCheckBox.Checked = false;
                    TableColumnNamesFontBoldTagBegin = "";
                    TableColumnNamesFontBoldTagEnd = "";
                }                    

                TableColumnsFontColorButton.BackColor = ColorTranslator.FromHtml(resultReport.ReturnValue[0].TableColumnNamesFontColor);
                TableColumnsBackColorButton.BackColor = ColorTranslator.FromHtml(resultReport.ReturnValue[0].TableColumnNamesBackColor);

                // Extract Email Recipients
                Char delimiter = ';';
                if (!string.IsNullOrEmpty(resultReport.ReturnValue[0].EmailRecipients))
                {
                    String[] emails = resultReport.ReturnValue[0].EmailRecipients.Split(delimiter);
                    foreach (var email in emails)
                        if (!string.IsNullOrEmpty(email)) EmailAdressListView.Items.Add(email, email);
                }                
            }

            // Get Report Template by template ID
            ResultReportsTemplate resultReportsTemplate = new ResultReportsTemplate();
            URL = BaseURL + "Reports/GetReportTemplateByID";
            urlParameters = "?templateID=" + Data.GlovalVariables.currentTemplateID; 
            client2.BaseAddress = new Uri(URL);

            response = client2.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultReportsTemplate = JsonConvert.DeserializeObject<ResultReportsTemplate>(returnMessage);
            }

            if (response.IsSuccessStatusCode)
            {
                if (resultReportsTemplate.RecordsCount != 0)
                {
                    if (resultReportsTemplate.ReturnValue[0].Parameters == null)
                    {
                        ParameterValueTextBox.Enabled = false;
                        ParametersListView.Enabled = false;
                        if (tabControl.TabPages.ContainsKey("ParametersTabPage"))
                            tabControl.TabPages.Remove(tabControl.TabPages["ParametersTabPage"]);
                        validateReportParameters = false;
                    }
                    else
                    {
                        foreach (ReportTemplateParameter parameter in resultReportsTemplate.ReturnValue[0].Parameters)
                        {
                            //Get Parameter Value fron ReportsTemplate Database table                            
                            parameterValue = "";
                            // Only for existing Reports --> Report ID != 0
                            if (Data.GlovalVariables.currentReportID != 0)
                            {                                
                                if (resultReport.RecordsCount != 0)
                                {
                                    foreach (ReportParameter reportParameter in resultReport.ReturnValue[0].Parameters)
                                    {
                                        if (reportParameter.ParameterID == parameter.ParameterID)
                                        {
                                            parameterValue = reportParameter.Value;
                                            break;
                                        }
                                    }
                                }
                            }
                            string[] row = { parameter.Name.Trim(), parameterValue.Trim(), parameter.DataType.Trim(), Convert.ToString(parameter.RequiredFlag) };
                            ListViewItem listItem = new ListViewItem(row);
                            listItem.Tag = parameter.ParameterID;
                            ParametersListView.Items.Add(listItem);
                        }
                    }                    
                }                
                else
                {
                    MessageBox.Show("Warning:" + "\r\n" + resultReportsTemplate.Message.Replace(". ", "\r\n"), "Report Properties ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }            
        }

        private void ParametersListView_Click(object sender, EventArgs e)
        {
            ParameterNameTextBox.Text= ParametersListView.SelectedItems[0].SubItems[0].Text;
            ParameterValueTextBox.Text = ParametersListView.SelectedItems[0].SubItems[1].Text;
            ValueFormatTip.Text = "";
            switch (ParametersListView.SelectedItems[0].SubItems[2].Text)
            {
                case "Integer":
                    ValueFormatTip.Text = "Integer";                    
                     break;
                case "Boolean":
                    ValueFormatTip.Text = "true/false";
                    break;
                case "Time":
                    ValueFormatTip.Text = "HH:MM (2 Digit for Hour {00-24}, and 2 digit for the Minutes {00-59})";
                    break;
            }
            ValueLabel.ForeColor = System.Drawing.Color.Black;
            switch (ParametersListView.SelectedItems[0].SubItems[3].Text)
            {
                case "True":
                    ValueLabel.ForeColor = System.Drawing.Color.Red;
                    break;
                case "False":
                    ValueLabel.ForeColor = System.Drawing.Color.Black;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int counter = -1;
            Boolean validEntry;
            foreach (ListViewItem item in ParametersListView.Items)
            {
                counter++;
                validEntry = true;
                if (ParameterNameTextBox.Text == item.Text)
                {
                    if (!(string.IsNullOrWhiteSpace(ParameterValueTextBox.Text) && (ParametersListView.Items[counter].SubItems[3].Text == "False")))
                    {
                        validEntry = true;
                    
                        switch (ParametersListView.Items[counter].SubItems[2].Text)
                        {
                            case "Integer":
                                if (!Validation.IsValidInteger(ParameterValueTextBox.Text))
                                {
                                    toolTip1.ToolTipTitle = "Invalid Value";
                                    toolTip1.Show("The value you entered is not a valid Ineger.", ParameterValueTextBox, 5000);
                                    validEntry = false;
                                }
                                break;
                            case "Boolean":
                                if (!Validation.IsValidBoolean(ParameterValueTextBox.Text))
                                {
                                    toolTip1.ToolTipTitle = "Invalid Value";
                                    toolTip1.Show("The value you entered is not a valid Boolean.", ParameterValueTextBox, 5000);
                                    validEntry = false;
                                }
                                break;
                            case "Time":
                                if (!Validation.IsValidTime(ParameterValueTextBox.Text))
                                {
                                    toolTip1.ToolTipTitle = "Invalid Value";
                                    toolTip1.Show("The value you entered is not a valid Time.", ParameterValueTextBox, 5000);
                                    validEntry = false;
                                }
                                break;
                        }
                    }
                    if (validEntry)
                    {
                        ParametersListView.Items[counter].SubItems[1].Text = ParameterValueTextBox.Text;
                        ParameterValueTextBox.Text = "";
                        ValueLabel.ForeColor = System.Drawing.Color.Black;
                        ValueFormatTip.Text = "";
                        ParameterNameTextBox.Text = "";
                    }
                    else
                    {
                        ParameterValueTextBox.Focus();
                    }
                }
            }
        }

        private void ParametersListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Title1FontColrButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Title1FontColorButton.BackColor = colorDialog1.Color;
                Title1ColorCode = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Body.Style = "zoom:60%";
        }

        public void reportViewUpdate()
        {
            webBrowser1.DocumentText = "<!DOCTYPE html>" +
               "<body>" +
               "<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" width=\"1200\" >" +
               "<tbody align=\"center\" style=\"font-family:verdana; color:\"black ?; background - color:?silver ?= \"\">" +
               "<tr>" +
               "<td align=\"center\">" +
               "<font color=\"" + Title1ColorCode + "\" size=\"" + Title1FontSize + "\">" +
               Title1FontBoldTagBegin + Title1Content + Title1FontBoldTagEnd +
               "</font>" +
               "</td>" +
               "</tr>" +
               "<tr>" +
               "<td align = \"center\">" +
                "<font color=\"" + Title2ColorCode + "\" size=\"" + Title2FontSize + "\">" +
               Title2FontBoldTagBegin + Title2Content + Title2FontBoldTagEnd +
               "</font>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "<br>" +
               "<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" width=\"1200\">" +
               "<tbody align=\"center\" style=\"font-family:verdana; color:\" black ?; background - color:?silver ?= \"\">" +
               "<tr align=\"center\">" +
               "<td>" +
               "<font color=\"" + Title3ColorCode + "\" size=\"" + Title3FontSize + "\">" +
               Title3FontBoldTagBegin + Title3Content + Title3FontBoldTagEnd +
               "</font>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "<br>" +
               "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\" width=\"1200\">" +
               "<tbody align=\"center\" style=\"font-family:verdana; color:\" black ?; background - color:?silver ?= \"\">" +
               "<tr bgcolor=\"" + TableHeaderBackColorCode + "\">" +
               "<td align=\"center\">" +
               "<font color=\"" + TableHeaderFontColorCode + "\" size=\"" + TableHeaderFontSize + "\">" +
               TableHeaderFontBoldTagBegin + "Table Title" + TableHeaderFontBoldTagEnd +
               "</font>" +
               "</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "<table border=\"1\" cellpadding=\"1\" cellspacing=\"1\" width=\"1200\">" +
               "<tbody align=\"center\" style=\"font-family:verdana; color:\" black ?; background - color:?silver ?= \"\">" +
               "<tr bgcolor=\"" + TableColumnNamesBackColorCode + "\">" +
               "<b>" +
               "<td align=\"left\" width=\"200\">" + 
               "<font color=\"" + TableColumnNmaesFontColorCode + "\" size=\"" + TableColumnNamesFontSize + "\">" +
               TableColumnNamesFontBoldTagBegin + "Column Name" + TableColumnNamesFontBoldTagEnd +
               "</font>" +
               "</td>" +
               "<td align=\"left\" width=\"200\">" +
               "<font color=\"" + TableColumnNmaesFontColorCode + "\" size=\"" + TableColumnNamesFontSize + "\">" +
               TableColumnNamesFontBoldTagBegin + "Column Name" + TableColumnNamesFontBoldTagEnd +
               "</font>" +
               "</td>" +
               "<td align=\"left\" width=\"200\">" +
               "<font color=\"" + TableColumnNmaesFontColorCode + "\" size=\"" + TableColumnNamesFontSize + "\">" +
               TableColumnNamesFontBoldTagBegin + "Column Name" + TableColumnNamesFontBoldTagEnd +
               "</font>" +
               "</td>" +
               "<td align=\"left\" width=\"600\">" +
               "<font color=\"" + TableColumnNmaesFontColorCode + "\" size=\"" + TableColumnNamesFontSize + "\">" +
               TableColumnNamesFontBoldTagBegin + "Column Name" + TableColumnNamesFontBoldTagEnd +
               "</font>" +
               "</td>" +
               "</b>" +
               "</tr>" +
               "<tr>" +
               "<td align=\"left\" width=\"200\">11</td>" +
               "<td align=\"left\" width=\"200\">VCP0001</td>" +
               "<td align=\"left\" width=\"200\">11/17/2017 2:10:59 PM</td>" +
               "<td align=\"left\" width=\"600\">Value</td>" +
               "</tr>" +
               "</tbody>" +
               "</table>" +
               "<br>";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (Tile1BoldCheckBox.Checked)
            {
                Title1FontBoldTagBegin = "<b>";
                Title1FontBoldTagEnd = "</b>";
            }
            else
            {
                Title1FontBoldTagBegin = "";
                Title1FontBoldTagEnd = "";
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void Title1FontSizeNmericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Title1FontSize = Title1FontSizeNmericUpDown.Value.ToString();
            if (!formInitialization) reportViewUpdate();
        }

        private void Title2FontColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Title2FontColorButton.BackColor = colorDialog1.Color;
                Title2ColorCode = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void Title3FontColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Title3FontColorButton.BackColor = colorDialog1.Color;
                Title3ColorCode = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void Tile2BoldCcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Tile2BoldCheckBox.Checked)
            {
                Title2FontBoldTagBegin = "<b>";
                Title2FontBoldTagEnd = "</b>";
            }
            else
            {
                Title2FontBoldTagBegin = "";
                Title2FontBoldTagEnd = "";
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void Tile3BoldCcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Tile3BoldCheckBox.Checked)
            {
                Title3FontBoldTagBegin = "<b>";
                Title3FontBoldTagEnd = "</b>";
            }
            else
            {
                Title3FontBoldTagBegin = "";
                Title3FontBoldTagEnd = "";
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void Title2FontSizeNmericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Title2FontSize = Title2FontSizeNmericUpDown.Value.ToString();
            if (!formInitialization) reportViewUpdate();
        }

        private void Title3FontSizeNmericUpDown_ValueChanged(object sender, EventArgs e)
        {

            Title3FontSize = Title3FontSizeNmericUpDown.Value.ToString();
            if (!formInitialization) reportViewUpdate();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void TableHeaderFontSizeNmericUpDown_ValueChanged(object sender, EventArgs e)
        {
            TableHeaderFontSize = TableHeaderFontSizeNmericUpDown.Value.ToString();
            if (!formInitialization) reportViewUpdate();
        }

        private void TableColumnNamesFontSizeNmericUpDown_ValueChanged(object sender, EventArgs e)
        {
            TableColumnNamesFontSize = TableColumnNamesFontSizeNmericUpDown.Value.ToString();
            if (!formInitialization) reportViewUpdate();
        }

        private void TableHeaderFontColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                TableHeaderFontColorButton.BackColor = colorDialog1.Color;
                TableHeaderFontColorCode = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void TableColumnsFontColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                TableColumnsFontColorButton.BackColor = colorDialog1.Color;
                TableColumnNmaesFontColorCode = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void TableHeaderBackColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                TableHeaderBackColorButton.BackColor = colorDialog1.Color;
                TableHeaderBackColorCode = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void TableColumnsBackColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                TableColumnsBackColorButton.BackColor = colorDialog1.Color;
                TableColumnNamesBackColorCode = "#" + colorDialog1.Color.R.ToString("X2") + colorDialog1.Color.G.ToString("X2") + colorDialog1.Color.B.ToString("X2");
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void TableHeaderFontBoldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TableHeaderFontBoldCheckBox.Checked)
            {
                TableHeaderFontBoldTagBegin = "<b>";
                TableHeaderFontBoldTagEnd = "</b>";
            }
            else
            {
                TableHeaderFontBoldTagBegin = "";
                TableHeaderFontBoldTagEnd = "";
            }
            if (!formInitialization) reportViewUpdate();
        }

        private void TableColumnsFontBoldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TableColumnsFontBoldCheckBox.Checked)
            {
                TableColumnNamesFontBoldTagBegin = "<b>";
                TableColumnNamesFontBoldTagEnd = "</b>";
            }
            else
            {
                TableColumnNamesFontBoldTagBegin = "";
                TableColumnNamesFontBoldTagEnd = "";
            } 
            if (!formInitialization) reportViewUpdate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Title1Content = MainTitleContentTextBox.Text;
            if (!formInitialization) reportViewUpdate();            
        }

        private void SecundaryTitleContentTextBox_TextChanged(object sender, EventArgs e)
        {          
            Title2Content = SecundaryTitleContentTextBox.Text;
            if (!formInitialization) reportViewUpdate();
        }

        private void ComplementaryTitleContentTextBox_TextChanged(object sender, EventArgs e)
        {
            Title3Content = ComplementaryTitleContentTextBox.Text;
            if (!formInitialization) reportViewUpdate();          
        }

        private void Save(string action)
        {
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
            Boolean parameterCompletion = true;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string URL = "";
            string bodyString = "";
            string reportJS = "";
            string returnMessage = "";
            Report report = new Report();
            ResultReports resultReports = new ResultReports();

            // Build Report Data Object
            foreach (ListViewItem eachItem in EmailAdressListView.Items)
            {
                if (string.IsNullOrEmpty(report.EmailRecipients))
                {
                    report.EmailRecipients = eachItem.Text;
                }
                else
                {
                    report.EmailRecipients = report.EmailRecipients + ";" + eachItem.Text;
                }
            }
           
            // Some validation before the report information is saved
            if (ReportEnableFlagCheckBox.Checked)
            {
                // Make sure the recipients is not empty when the report has been enabled
                if (string.IsNullOrWhiteSpace(report.EmailRecipients))
                {
                    tabControl.SelectedIndex = tabControl.TabPages.IndexOfKey("RecipientsTabPage");
                    toolTip1.ToolTipTitle = "No recipients has been defined";
                    EmailAddressTextBox.Focus();
                    toolTip1.Show("Add a recipient email to enable this report.", EmailAddressTextBox, 5000);
                    EmailAddressTextBox.Focus();
                    EmailAddressTextBox.Select();
                    return;
                }
                // Make sure the subject is not empty when the report has been enabled
                if (string.IsNullOrWhiteSpace(EmailSubjectTextBox.Text))
                {
                    tabControl.SelectedIndex = tabControl.TabPages.IndexOfKey("PropertiesTabPage");
                    toolTip1.ToolTipTitle = "No subejct has been defined";
                    EmailSubjectTextBox.Focus();
                    toolTip1.Show("Add a Subject to enable this report.", EmailSubjectTextBox, 5000);
                    EmailSubjectTextBox.Focus();
                    EmailSubjectTextBox.Select();
                    return;
                }
            }

            report.CustomerID = Data.GlovalVariables.currentCustomerID;
            report.TemplateID = Data.GlovalVariables.currentTemplateID;
            report.EnableFlag = ReportEnableFlagCheckBox.Checked;
            report.EmailSubject = EmailSubjectTextBox.Text;
            report.TitleContent1 = MainTitleContentTextBox.Text;
            report.TitleContent2 = SecundaryTitleContentTextBox.Text;
            report.TitleContent3 = ComplementaryTitleContentTextBox.Text;
            report.TitleFontColor1 = Title1ColorCode;
            report.TitleFontColor2 = Title2ColorCode;
            report.TitleFontColor3 = Title3ColorCode;
            report.TitleFontSize1 = Convert.ToInt32(Title1FontSize);
            report.TitleFontSize2 = Convert.ToInt32(Title2FontSize);
            report.TitleFontSize3 = Convert.ToInt32(Title3FontSize);
            report.TitleFontBoldFlag1 = Tile1BoldCheckBox.Checked;
            report.TitleFontBoldFlag2 = Tile2BoldCheckBox.Checked;
            report.TitleFontBoldFlag3 = Tile3BoldCheckBox.Checked;
            report.TableHeaderBackColor = TableHeaderBackColorCode;
            report.TableHeaderFontBoldFlag = TableHeaderFontBoldCheckBox.Checked;
            report.TableHeaderFontColor = TableHeaderFontColorCode;
            report.TableHeaderFontSize = Convert.ToInt32(TableHeaderFontSize);
            report.TableColumnNamesBackColor = TableColumnNamesBackColorCode;
            report.TableColumnNamesFontColor = TableColumnNmaesFontColorCode;
            report.TableColumnNamesFontBoldFlag = TableColumnsFontBoldCheckBox.Checked;
            report.TableColumnNamesFontSize = Convert.ToInt32(TableColumnNamesFontSize);
            report.Schedule = schedule;

            // Build Report's Parameters List
            if (ParametersListView.Items.Count > 0)
            {
                List<ReportParameter> reportParameters = new List<ReportParameter>();
                foreach (ListViewItem eachItem in ParametersListView.Items)
                {
                    ReportParameter reportParamneter = new ReportParameter();
                    reportParamneter.ParameterID = Convert.ToInt32(eachItem.Tag);
                    reportParamneter.ReportID = Data.GlovalVariables.currentReportID;
                    reportParamneter.TemplateID = Data.GlovalVariables.currentTemplateID;
                    reportParamneter.Value = eachItem.SubItems[1].Text;
                    reportParameters.Add(reportParamneter);
                    if (eachItem.SubItems[3].Text == "True" && string.IsNullOrWhiteSpace(eachItem.SubItems[1].Text))
                    {
                        parameterCompletion = false;
                        break;
                    }
                }
                report.Parameters = reportParameters;
            }

            if (parameterCompletion)
            {
                reportJS = JsonConvert.SerializeObject(report, Newtonsoft.Json.Formatting.Indented);
                URL = BaseURL + "Reports/UpdateReport";
                bodyString = "'" + reportJS + "'";

                HttpContent body_for_update = new StringContent(bodyString);
                body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                using (HttpContent content = response_for_update.Content)
                {
                    Task<string> resultTemp = content.ReadAsStringAsync();
                    returnMessage = resultTemp.Result;
                    resultReports = JsonConvert.DeserializeObject<ResultReports>(returnMessage);
                }

                if (response_for_update.IsSuccessStatusCode)
                {
                    // Set the value of the new customer to a gloval variable
                    if (resultReports.ReturnCode == -1)
                    {
                        // MessageBox.Show("Warning:" + "\r\n" + resultCustomers.Message.Replace(". ", "\r\n"), "Update Customer Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Data.GlovalVariables.currentReportEnable = report.EnableFlag;
                        Data.GlovalVariables.transactionType = "Update";

                        if (action == "SaveAndExit") this.Close();
                        else
                        {
                            EmailSubjectTextBox.Focus();
                        }
                    }
                }
                else
                {
                    //    MessageBox.Show("Error:" + "\r\n" + resultCustomers.Message.Replace(". ", "\r\n"), "Update Customer Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Warning:" + "\r\n" + "You must supply a value for all the required parameters. Check the Parameters Tab.", "Update Report Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                MailAddress m = new MailAddress(EmailAddressTextBox.Text);
                ListViewItem item = EmailAdressListView.FindItemWithText(EmailAddressTextBox.Text);
                if (item == null)
                {
                    EmailAdressListView.Items.Add(EmailAddressTextBox.Text, EmailAddressTextBox.Text);
                }
                else
                {
                    toolTip1.ToolTipTitle = "Invalid Email Address";
                    toolTip1.Show("The value you entered already exist. Please change the value.", EmailAddressTextBox, 5000);
                }
                

            }
            catch (FormatException)
            {
                toolTip1.ToolTipTitle = "Invalid Email Address";
                toolTip1.Show("The value you entered is not a valid Email. Please change the value.", EmailAddressTextBox, 5000);
            }
        }

        private void DeleteEmailAddressButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in EmailAdressListView.SelectedItems)
            {
                EmailAdressListView.Items.Remove(eachItem);
            }
        }

        private void Title1FontColorButton_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Color c;
            c = Title1FontColorButton.BackColor;
            Title1ColorCode = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void Title2FontColorButton_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Color c;
            c = Title2FontColorButton.BackColor;
            Title2ColorCode = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void Title3FontColorButton_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Color c;
            c = Title3FontColorButton.BackColor;
            Title3ColorCode = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void TableHeaderFontColorButton_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Color c;
            c = TableHeaderFontColorButton.BackColor;
            TableHeaderFontColorCode = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void TableColumnsFontColorButton_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Color c;
            c = TableColumnsFontColorButton.BackColor;
            TableColumnNmaesFontColorCode = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void TableHeaderBackColorButton_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Color c;
            c = TableHeaderBackColorButton.BackColor;
            TableHeaderBackColorCode = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void TableColumnsBackColorButton_BackColorChanged(object sender, EventArgs e)
        {
            System.Drawing.Color c;
            c = TableColumnsBackColorButton.BackColor;
            TableColumnNamesBackColorCode = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
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

            ParametersListView.Items.Clear();
            EmailAdressListView.Items.Clear();
            formInitialization = true;
            ReportsLoad();
            formInitialization = false;
            reportViewUpdate();
        }

        private void ResetButton_Leave(object sender, EventArgs e)
        {           
            webBrowser1.Refresh();
        }

        private void DailyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (DailyRadioButton.Checked)
            {
                DailyGroupBox.Enabled = true;
                WeeklyGroupBox.Enabled = false;
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
    }
}
