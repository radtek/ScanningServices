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
using NLog;

namespace ScanningServicesAdmin.Forms
{
    public partial class WorkingFoldersForm : Form
    {

        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public WorkingFoldersForm()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WorkingFoldersForm_Load(object sender, EventArgs e)
        {
            LoadWorkinFolderListView();
        }

        private void LoadWorkinFolderListView()
        {
            WorkingFoldersListView.Items.Clear();
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
            // Add Services stations to Stations List
            if (resultWorkingFolders.RecordsCount != 0)
            {
                foreach (var item in resultWorkingFolders.ReturnValue)
                {
                    ListViewItem newItem = new ListViewItem();
                    newItem.Text = item.Path;
                    newItem.Tag = item.FolderID;
                    WorkingFoldersListView.Items.Add(newItem);
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
            try
            {
                string URL = "";
                string bodyString = "";
                string workingFolderJS = "";
                string returnMessage = "";
                string urlParameters = "";

                WorkingFolder workingFolder = new WorkingFolder();
                ResultWorkingFolders resultWorkingFolders = new ResultWorkingFolders();

                foreach (ListViewItem item in WorkingFoldersListView.Items)
                {
                    workingFolder.Path = item.Text;
                    if (!string.IsNullOrEmpty(item.Tag.ToString()))
                    {
                        workingFolder.FolderID = Convert.ToInt32(item.Tag);
                    }
                    else
                    {
                        workingFolder.FolderID = 0;
                    }
                    workingFolderJS = JsonConvert.SerializeObject(workingFolder, Newtonsoft.Json.Formatting.Indented);
                    workingFolderJS = workingFolderJS.Replace(@"\", "\\\\");

                    // if Item Back color DarkGreen --> Teh item correspond to a New Station 
                    if (item.ForeColor == Color.DarkGreen)
                    {
                        HttpClient client = new HttpClient();
                        client.Timeout = TimeSpan.FromMinutes(15);
                        URL = BaseURL + "GeneralSettings/NewWorkingFolder";
                        bodyString = "'" + workingFolderJS + "'";

                        HttpContent body_for_new = new StringContent(bodyString);
                        body_for_new.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response_for_new = client.PostAsync(URL, body_for_new).Result;

                        using (HttpContent content = response_for_new.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            resultWorkingFolders = JsonConvert.DeserializeObject<ResultWorkingFolders>(returnMessage);
                        }
                        if (response_for_new.IsSuccessStatusCode)
                        {
                            // Set the value of the new customer to a gloval variable
                            if (resultWorkingFolders.ReturnCode == -1)
                            {
                                MessageBox.Show("Warning:" + "\r\n" + resultWorkingFolders.Message.Replace(". ", "\r\n"), "New Working Folder Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                // Do nothing
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error:" + "\r\n" + resultWorkingFolders.Message.Replace(". ", "\r\n") + resultWorkingFolders.Message, "New Working Folder Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    // If Back color is DarkRed --> The item correspond to a Station that needs to be deleted
                    if (item.ForeColor == Color.DarkRed)
                    {
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = new HttpResponseMessage();
                        client.Timeout = TimeSpan.FromMinutes(15);
                        URL = BaseURL + "GeneralSettings/DeleteWorkingFolder";
                        urlParameters = "?folderID=" + item.Tag.ToString();
                        client.BaseAddress = new Uri(URL);
                        response = client.DeleteAsync(urlParameters).Result;

                        using (HttpContent content = response.Content)
                        {
                            Task<string> resultTemp = content.ReadAsStringAsync();
                            returnMessage = resultTemp.Result;
                            resultWorkingFolders = JsonConvert.DeserializeObject<ResultWorkingFolders>(returnMessage);
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            // Set the value of the new customer to a global variable
                            if (resultWorkingFolders.ReturnCode == -1)
                            {
                                MessageBox.Show("Warning:" + "\r\n" + resultWorkingFolders.Message.Replace(". ", "\r\n"), "Delete Working Folder Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                // Do nothing
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error:" + "\r\n" + resultWorkingFolders.Message.Replace(". ", "\r\n") + resultWorkingFolders.Message, "Delete Working Folder Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                if (action == "SaveAndExit") this.Close();
                else
                {
                    LoadWorkinFolderListView();
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                //private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem item = WorkingFoldersListView.FindItemWithText(WorkingFolderTextBox.Text);

                if (item == null)
                {
                    ListViewItem newItem = new ListViewItem();
                    newItem.Font = new System.Drawing.Font(newItem.Font, System.Drawing.FontStyle.Bold);
                    newItem.Text = WorkingFolderTextBox.Text;
                    newItem.Tag = "";
                    // New Item are shown in Green Color
                    newItem.ForeColor = Color.DarkGreen;
                    //newItem.BackColor = Color.LightSeaGreen;
                    WorkingFoldersListView.Items.Add(newItem);
                    WorkingFolderTextBox.Text = "";
                }
            }
            catch (FormatException)
            {

            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (WorkingFoldersListView.SelectedItems.Count != 0)
            {
                ListViewItem item = WorkingFoldersListView.SelectedItems[0];
                item.Font = new System.Drawing.Font(item.Font, System.Drawing.FontStyle.Bold);

                if (item.BackColor == Color.DarkRed)
                {
                    // This is an item that was deleted so, ignore the transaction
                    toolTip1.ToolTipTitle = "Delete request";
                    toolTip1.Show("The selected value is already tagged for deletion.", WorkingFoldersListView, 5000);
                }
                else
                {
                    if (item.BackColor == Color.DarkGreen)
                    {
                        // This itme was recently added so it will remove the item from List
                        WorkingFoldersListView.SelectedItems[0].Remove();
                    }
                    else
                    {
                        // Tag the Item for deletion
                        item.ForeColor = Color.DarkRed;

                    }
                }
            }
        }

        private void WorkigDirectoryButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                WorkingFolderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
