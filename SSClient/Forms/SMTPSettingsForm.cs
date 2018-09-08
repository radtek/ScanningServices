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
    public partial class SMTPSettingsForm : Form
    {
        //public string BaseURL = "http://localhost:47063" + "/api/";
        public SMTP originalSMTP = new SMTP();

        public SMTPSettingsForm()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;        
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SMTPSettingsForm_Load(object sender, EventArgs e)
        {
            // Get SMTP Settings
            ResultSMTP resultSMTP = new ResultSMTP();
            string returnMessage = "";
            Cursor.Current = Cursors.WaitCursor;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            URL = BaseURL + "SMTP/GetSMTPInfo";
            client.BaseAddress = new Uri(URL);

            response = client.GetAsync(urlParameters).Result;
            using (HttpContent content = response.Content)
            {
                Task<string> resultTemp = content.ReadAsStringAsync();
                returnMessage = resultTemp.Result;
                // Reformating the result string
                //returnMessage = returnMessage.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace("\\", "");
                //returnMessage = returnMessage.Remove(returnMessage.Length - 1, 1).Substring(1);
                resultSMTP = JsonConvert.DeserializeObject<ResultSMTP>(returnMessage);
            }

            if (response.IsSuccessStatusCode)
            {
                if (resultSMTP.RecordsCount != 0)
                {
                    HostNameTextBox.Text = resultSMTP.ReturnValue.HostName.Trim();
                    PortNumberTextBox.Text = resultSMTP.ReturnValue.PortNumber.ToString().Trim();
                    EnableSSLCheckBox.Checked = resultSMTP.ReturnValue.EnableSSLFlag;
                    SenderEmailTextBox.Text = resultSMTP.ReturnValue.SenderEmailAddress.Trim();
                    SenderNameTextBox.Text = resultSMTP.ReturnValue.SenderName.Trim();
                    UserNameTextBox.Text = resultSMTP.ReturnValue.UserName.Trim();
                    UserPasswordTextBox.Text = resultSMTP.ReturnValue.Password.Trim();
                    originalSMTP = resultSMTP.ReturnValue;
                }
                else
                {
                    HostNameTextBox.Text = "";
                    PortNumberTextBox.Text = "";
                    EnableSSLCheckBox.Checked = false;
                    SenderEmailTextBox.Text = "";
                    SenderNameTextBox.Text = "";
                    UserNameTextBox.Text = "";
                    UserPasswordTextBox.Text = "";
                }                
            }
            if (!EnableSSLCheckBox.Checked)
            {
                UserNameTextBox.Enabled = false;
                UserPasswordTextBox.Enabled = false;
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
            Boolean continueTransaction = true;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string URL = "";
            string bodyString = "";
            string smtpJS = "";
            string returnMessage = "";
            SMTP smtp = new SMTP();
            ResultSMTP resultSMTP = new ResultSMTP();

            switch (Data.GlovalVariables.transactionType)
            {
                case "Update":

                    // Validation rules
                    if (HostNameTextBox.Text.Length == 0 || PortNumberTextBox.Text.Length == 0 || SenderEmailTextBox.Text.Length == 0)
                    {
                        MessageBox.Show("Warning:" + "\r\n" + "You must provide values for Host Name, Port NUmber, and Sender Email Address.", "Update SMTP Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continueTransaction = false;
                    }
                    else
                    {
                        if (!EnableSSLCheckBox.Checked)
                        {
                            // we are Ok with the Update
                        }
                        else
                        {
                            if (UserNameTextBox.Text.Length == 0 || UserPasswordTextBox.Text.Length == 0)
                            {
                                MessageBox.Show("Warning:" + "\r\n" + "You must provide values for User Name and Password when SSL option is checked.", "Update SMTP Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continueTransaction = false;
                            }
                            else
                            {
                                // We are ok with the update
                            }
                        }
                    }
                    // End of Validation Rules

                    if (continueTransaction)
                    {
                        // Build the smpt Object
                        smtp.HostName = HostNameTextBox.Text.Trim();
                        smtp.PortNumber = Convert.ToInt32(PortNumberTextBox.Text.Trim());
                        smtp.EnableSSLFlag = EnableSSLCheckBox.Checked;
                        smtp.SenderEmailAddress = SenderEmailTextBox.Text.Trim();
                        smtp.SenderName= SenderNameTextBox.Text.Trim();
                        smtp.UserName = UserNameTextBox.Text.Trim();
                        smtp.Password = UserPasswordTextBox.Text.Trim();

                        // Build smpt Object in Json Format
                        smtpJS = JsonConvert.SerializeObject(smtp, Newtonsoft.Json.Formatting.Indented);
                        URL = BaseURL + "SMTP/UpdateSMTP";
                        bodyString = "'" + smtpJS + "'";

                        HttpContent body_for_update = new StringContent(bodyString);
                        body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                        using (HttpContent content = response_for_update.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            // Reformating the result string
                            //returnMessage = returnMessage.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace("\\", "");
                            //returnMessage = returnMessage.Remove(returnMessage.Length - 1, 1).Substring(1);
                            resultSMTP = JsonConvert.DeserializeObject<ResultSMTP>(returnMessage);
                        }

                        if (response_for_update.IsSuccessStatusCode)
                        {
                            if (resultSMTP.ReturnCode == -1)
                            {
                                MessageBox.Show("Warning:" + "\r\n" + resultSMTP.Message.Replace(". ", "\r\n"), "Update SMTP Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            MessageBox.Show("Error:" + "\r\n" + resultSMTP.Message.Replace(". ", "\r\n") + "\r\n" + resultSMTP.Exception, "Update SMTP Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }                    
                    break;

            }

        }

        private void EnableSSLCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableSSLCheckBox.Checked)
            {
                UserNameTextBox.Enabled = true;
                UserPasswordTextBox.Enabled = true;
            }
            else
            {
                UserNameTextBox.Enabled = false;
                UserPasswordTextBox.Enabled = false;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            HostNameTextBox.Text = originalSMTP.HostName;
            if (originalSMTP.PortNumber == 0)
                PortNumberTextBox.Text = "";
            else
                PortNumberTextBox.Text = originalSMTP.PortNumber.ToString();
            EnableSSLCheckBox.Checked = originalSMTP.EnableSSLFlag;
            SenderEmailTextBox.Text = originalSMTP.SenderEmailAddress;
            SenderNameTextBox.Text = originalSMTP.SenderName;
            UserNameTextBox.Text = originalSMTP.UserName;
            UserPasswordTextBox.Text = originalSMTP.Password;
        }

        private void PortNumberTextBox_Leave(object sender, EventArgs e)
        {
 
        }

        private void PortNumberTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateTextIntegerInTextBox(sender, e);
        }

        private void SenderEmailTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                MailAddress m = new MailAddress(SenderEmailTextBox.Text);
            }
            catch (FormatException)
            {
                toolTip1.ToolTipTitle = "Invalid Email Address";
                toolTip1.Show("The value you entered is not a valid Email. Please change the value.", SenderEmailTextBox, 5000);
            }
        }
    }
}
