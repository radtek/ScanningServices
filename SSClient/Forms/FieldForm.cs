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
    public partial class FieldForm : Form
    {
        //public string BaseURL = "http://localhost:47063" + "/api/";
        public Field originalField = new Field();

        public FieldForm()
        {
            string URL = "";
            string urlParameters = "";
            string returnMessage = "";
            HttpResponseMessage response = new HttpResponseMessage();
            InitializeComponent();
            newFieldsList.Clear();
            CustomerNameTextBox.Text = Data.GlovalVariables.currentCustomerName;
            ProjectNameTextBox.Text = Data.GlovalVariables.currentProjectName;
            JobNameTextBox.Text = Data.GlovalVariables.currentJobName;
            if (Data.GlovalVariables.transactionType == "Update")
            {
                CPFieldNameTextBox.Text = Data.GlovalVariables.currentFieldName;
                originalField.CPFieldName = Data.GlovalVariables.currentFieldName;
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                // Get Field Information
                URL = BaseURL + "Fields/GetFieldByID"; 
                urlParameters = "?fieldID=" + Data.GlovalVariables.currentFieldID;
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
                            VFRFieldNameTextBox.Text = resultFields.ReturnValue[0].VFRFieldName;
                            originalField.VFRFieldName = resultFields.ReturnValue[0].VFRFieldName;

                            ExcludeKeystrokeCheckBox.Checked = resultFields.ReturnValue[0].ExcludeFromKeystrokesCount;
                            originalField.ExcludeFromKeystrokesCount = resultFields.ReturnValue[0].ExcludeFromKeystrokesCount;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                  "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentVFRFieldName = VFRFieldNameTextBox.Text;
            this.Close();
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
            string fieldJS = "";
            string returnMessage = "";
            Field field = new Field();
            ResultFields resultFields = new ResultFields();

            // Build the Field Object
            field.JobID = Data.GlovalVariables.currentJobID;
            field.FieldID = Data.GlovalVariables.currentFieldID;
            field.CPFieldName = CPFieldNameTextBox.Text;
            field.VFRFieldName = VFRFieldNameTextBox.Text;
            field.ExcludeFromKeystrokesCount = ExcludeKeystrokeCheckBox.Checked;
            Data.GlovalVariables.currentFieldName = "";
            Data.GlovalVariables.currentVFRFieldName = VFRFieldNameTextBox.Text;

            switch (Data.GlovalVariables.transactionType)
            {
                case "New":

                    // Validation rules
                    if (CPFieldNameTextBox.Text.Length == 0)
                    {
                        MessageBox.Show("Warning:" + "\r\n" + "You must provide values for the Capture Pro Field Name.", "Update Field Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continueTransaction = false;
                    }

                    if (continueTransaction)
                    {
                        fieldJS = JsonConvert.SerializeObject(field, Newtonsoft.Json.Formatting.Indented);
                        URL = BaseURL + "Fields/NewField";
                        bodyString = "'" + fieldJS + "'";

                        HttpContent body_for_new = new StringContent(bodyString);
                        body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                        using (HttpContent content = response_for_new.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            resultFields = JsonConvert.DeserializeObject<ResultFields>(returnMessage);
                        }

                        if (response_for_new.IsSuccessStatusCode)
                        {
                            // Set the value of the new Field to a gloval variable
                            if (resultFields.ReturnCode == -1)
                            {
                                MessageBox.Show("Warning:" + "\r\n" + resultFields.Message.Replace(". ", "\r\n"), "New Field Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                Data.GlovalVariables.newFieldsList.Add(CPFieldNameTextBox.Text);
                                if (action == "SaveAndExit") this.Close();
                                else
                                {
                                    CPFieldNameTextBox.Text = "";
                                    VFRFieldNameTextBox.Text = "";
                                    ExcludeKeystrokeCheckBox.Checked = false;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error:" + "\r\n" + resultFields.Message.Replace(". ", "\r\n") + resultFields.Exception, "New Field Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    break;

                case "Update":

                    
                    fieldJS = JsonConvert.SerializeObject(field, Newtonsoft.Json.Formatting.Indented);
                    //jobJS = Regex.Escape(jobJS);

                    URL = BaseURL + "Fields/UpdateField";
                    bodyString = "'" + fieldJS + "'";

                    HttpContent body_for_update = new StringContent(bodyString);
                    body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                    using (HttpContent content = response_for_update.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        resultFields = JsonConvert.DeserializeObject<ResultFields>(returnMessage);
                    }

                    if (response_for_update.IsSuccessStatusCode)
                    {
                        // Set the value of the new Field to a gloval variable
                        if (resultFields.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultFields.Message.Replace(". ", "\r\n"), "Update Field Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.currentFieldName = CPFieldNameTextBox.Text;
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                                CPFieldNameTextBox.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultFields.Message.Replace(". ", "\r\n"), "Update Field Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

        private void ResetButton_Click_1(object sender, EventArgs e)
        {
            CPFieldNameTextBox.Text = originalField.CPFieldName;
            VFRFieldNameTextBox.Text = originalField.VFRFieldName;
            ExcludeKeystrokeCheckBox.Checked = originalField.ExcludeFromKeystrokesCount;
        }

        private void FieldForm_Load(object sender, EventArgs e)
        {

        }
    }
}
