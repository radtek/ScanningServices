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

namespace ScanningServicesAdmin.Forms
{
    public partial class ProjectForm : Form
    {
        //public string BaseURL = "http://localhost:47063" + "/api/";

        public ProjectForm()
        {
            InitializeComponent();
            newProjectsList.Clear();
            CustomerNameTextBox.Text = Data.GlovalVariables.currentCustomerName;
            if (Data.GlovalVariables.transactionType == "Update")
            {
                ProjectNameTextBox.Text = Data.GlovalVariables.currentProjectName;
            }
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save("SaveAndExit");
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (Data.GlovalVariables.transactionType == "New")
            {
                ProjectNameTextBox.Text = "";
            }
            else
            {
                ProjectNameTextBox.Text = Data.GlovalVariables.currentProjectName;
            }
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
            string projectJS = "";
            string returnMessage = "";
            Project project = new Project();
            ResultProjects resultProjects = new ResultProjects();
            project.CustomerID = Data.GlovalVariables.currentCustomerID;
            project.ProjectName = ProjectNameTextBox.Text;

            switch (Data.GlovalVariables.transactionType)
            {
                case "New":

                    projectJS = JsonConvert.SerializeObject(project, Newtonsoft.Json.Formatting.Indented);
                    URL = BaseURL + "Projects/NewProject";
                    bodyString = "'" + projectJS + "'";

                    HttpContent body_for_new = new StringContent(bodyString);
                    body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                    using (HttpContent content = response_for_new.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        // Reformating the result string
                        //returnMessage = returnMessage.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace("\\", "");
                        //returnMessage = returnMessage.Remove(returnMessage.Length - 1, 1).Substring(1);
                        resultProjects = JsonConvert.DeserializeObject<ResultProjects>(returnMessage);
                    }

                    if (response_for_new.IsSuccessStatusCode)
                    {
                        // Set the value of the new project to a gloval variable
                        if (resultProjects.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultProjects.Message.Replace(". ", "\r\n"), "New Project Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.newProjectsList.Add(ProjectNameTextBox.Text);
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                                ProjectNameTextBox.Text = "";
                                ProjectNameTextBox.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultProjects.Message.Replace(". ", "\r\n") + resultProjects.Exception, "New Project Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case "Update":
                    project.ProjectID = Data.GlovalVariables.currentProjectID;
                    projectJS = JsonConvert.SerializeObject(project, Newtonsoft.Json.Formatting.Indented);
                    URL = BaseURL + "Projects/UpdateProject";
                    bodyString = "'" + projectJS + "'";

                    HttpContent body_for_update = new StringContent(bodyString);
                    body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;

                    using (HttpContent content = response_for_update.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;
                        // Reformating the result string
                        returnMessage = returnMessage.Replace(@"\n", "\n").Replace(@"\r", "\r").Replace("\\", "");
                        returnMessage = returnMessage.Remove(returnMessage.Length - 1, 1).Substring(1);
                        resultProjects = JsonConvert.DeserializeObject<ResultProjects>(returnMessage);
                    }

                    if (response_for_update.IsSuccessStatusCode)
                    {
                        // Set the value of the new project to a gloval variable
                        if (resultProjects.ReturnCode == -1)
                        {
                            MessageBox.Show("Warning:" + "\r\n" + resultProjects.Message.Replace(". ", "\r\n"), "Update Projects Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            Data.GlovalVariables.currentProjectName = ProjectNameTextBox.Text;
                            if (action == "SaveAndExit") this.Close();
                            else
                            {
                                CustomerNameTextBox.Focus();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error:" + "\r\n" + resultProjects.Message.Replace(". ", "\r\n"), "Update Project Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

            }
        }
    }
}
