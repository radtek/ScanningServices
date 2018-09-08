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
using System.Text.RegularExpressions;

namespace ScanningServicesAdmin.Forms
{
    public partial class VFRSettingsForm : Form
    {

       // public string BaseURL = "http://localhost:47063" + "/api/";
        public VFR originalVFR = new VFR();

        public VFRSettingsForm()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save("SaveAndExit");
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            CADIWSURLTextBox.Text = originalVFR.CADIUrl;
            UserNameTextBox.Text = originalVFR.UserName;
            PasswordTextBox.Text = originalVFR.Password;
            InstanceNameTextBox.Text = originalVFR.InstanceName;
            CaptureTemplateTextBox.Text = originalVFR.CaptureTemplate;
            RepositoryNameTextBox.Text = originalVFR.RepositoryName;
            QueryFieldTextBox.Text = originalVFR.QueryField;
        }

        private void ApplyHutton_Click(object sender, EventArgs e)
        {
            Save("Save");
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VFRSettingsForm_Load(object sender, EventArgs e)
        {
            CustomerNameTextBox.Text = Data.GlovalVariables.currentCustomerName;
            ProjectNameTextBox.Text = Data.GlovalVariables.currentProjectName;
            JobNameTextBox.Text = Data.GlovalVariables.currentJobName;

            // Get SMTP Settings
            string returnMessage = "";
            Cursor.Current = Cursors.WaitCursor;
            string urlParameters = "";
            string URL = "";
            ResultsVFR resultVFR = new ResultsVFR();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            HttpResponseMessage response = new HttpResponseMessage();
            URL = BaseURL + "VFR/GetVFRInfoByJobID";
            urlParameters = "?jobID=" + Data.GlovalVariables.currentJobID;
            client.BaseAddress = new Uri(URL);

            response = client.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                resultVFR = JsonConvert.DeserializeObject<ResultsVFR>(returnMessage);
            }

            if (response.IsSuccessStatusCode)
            {
                if (resultVFR.RecordsCount != 0)
                {
                    CADIWSURLTextBox.Text = resultVFR.ReturnValue.CADIUrl;
                    UserNameTextBox.Text = resultVFR.ReturnValue.UserName;
                    PasswordTextBox.Text = resultVFR.ReturnValue.Password;
                    InstanceNameTextBox.Text = resultVFR.ReturnValue.InstanceName;
                    CaptureTemplateTextBox.Text = resultVFR.ReturnValue.CaptureTemplate;
                    RepositoryNameTextBox.Text = resultVFR.ReturnValue.RepositoryName;
                    QueryFieldTextBox.Text = resultVFR.ReturnValue.QueryField;
                    originalVFR = resultVFR.ReturnValue;
                }
                else
                {
                    CADIWSURLTextBox.Text = "";
                    UserNameTextBox.Text = "";
                    PasswordTextBox.Text = "";
                    InstanceNameTextBox.Text = "";
                    CaptureTemplateTextBox.Text = "";
                    RepositoryNameTextBox.Text = "";
                    QueryFieldTextBox.Text = "";
                }
            }           

        }

        private void Save(string action)
        {
            Boolean continueTransaction = true;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string URL = "";
            string bodyString = "";
            string vfrJS = "";
            string returnMessage = "";
            VFR vfr = new VFR();
            ResultsVFR resultVFR = new ResultsVFR();

            switch (Data.GlovalVariables.transactionType)
            {
                case "Update":

                    // Validation rules
                    if (CADIWSURLTextBox.Text.Length == 0 || UserNameTextBox.Text.Length == 0 || PasswordTextBox.Text.Length == 0 ||
                        InstanceNameTextBox.Text.Length == 0 || CaptureTemplateTextBox.Text.Length == 0 || 
                        RepositoryNameTextBox.Text.Length == 0 || QueryFieldTextBox.Text.Length == 0 )
                    {
                        MessageBox.Show("You must provide values for Host Cadi WS Url, User Name, Passsword, VFR Instance Name, Capture template, Repository Name, and Query Field.", "Update VFR Information Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        continueTransaction = false;
                    }                    
                    // End of Validation Rules

                    if (continueTransaction)
                    {
                        // Build the smpt Object
                        vfr.JobID = Data.GlovalVariables.currentJobID;
                        vfr.CADIUrl = CADIWSURLTextBox.Text.Trim();
                        vfr.UserName = UserNameTextBox.Text.Trim();
                        vfr.Password = PasswordTextBox.Text.Trim();
                        vfr.InstanceName = InstanceNameTextBox.Text.Trim();
                        vfr.CaptureTemplate = CaptureTemplateTextBox.Text.Trim();
                        vfr.RepositoryName = RepositoryNameTextBox.Text.Trim();
                        vfr.QueryField = QueryFieldTextBox.Text.Trim();

                        // Build vfr Object in Json Format
                        vfrJS = JsonConvert.SerializeObject(vfr, Newtonsoft.Json.Formatting.Indented);
                        URL = BaseURL + "VFR/UpdateVFRInfo";
                        bodyString = "'" + vfrJS + "'";

                        HttpContent body_for_update = new StringContent(bodyString);
                        body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                        using (HttpContent content = response_for_update.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            resultVFR = JsonConvert.DeserializeObject<ResultsVFR>(returnMessage);
                        }

                        if (response_for_update.IsSuccessStatusCode)
                        {
                            if (resultVFR.ReturnCode == -1)
                            {
                                MessageBox.Show("Warning:" + "\r\n" + resultVFR.Message.Replace(". ", "\r\n"), "Update VFR Information Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            MessageBox.Show("Error:" + "\r\n" + resultVFR.Message.Replace(". ", "\r\n") + "\r\n" + resultVFR.Exception, "Update VFR Information Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;

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
    }
}
