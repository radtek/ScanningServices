using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static ScanningServicesDataObjects.GlobalVars;


//for (int i = 33; i < 256; i++)
//{
//    Console.WriteLine("Code: " + i.ToString() + ": " + (char) i);
//}
// Token separator 
// Code: 248: ø
// Code: 164: ¤
// for intance : Replace ~ by ABC , and replace & by XYZ
//  ~¤ABC ø &¤XYZ
//string sample = "~¤ABC ø &¤XYZ";
//var tokens = sample.Split((char)248);

namespace ScanningServicesAdmin.Forms
{
    public partial class ExportConfigurationForm : Form
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        // Variables to be used when the Rest Option is selected
        public List<string> directoryNameTokensOriginal = new List<string>();
        public List<string> fileNameTokensOriginal = new List<string>();
        public List<string> metadatadirectoryNameTokensOriginal = new List<string>();
        public List<ReplaceToken> fileNameReplaceTokensOriginal = new List<ReplaceToken>();
        public List<ReplaceToken> directoryReplaceTokensOriginal = new List<ReplaceToken>();
        public List<ReplaceToken> fieldsReplaceTokensOriginal = new List<ReplaceToken>();
        public List<string> selectedOutputFields = new List<string>();
        public string metadataFileFormatOriginal = "";
        public Boolean includeHeaderOriginal = false;
        public string delimiterOriginal = "";
        public string metadataFileNameOrignal = "Batch Name";
        public string duplicateNameActionOriginal = "Rename";

        // Global Variables to be use of the operation of the Form
        public List<string> directoryNameTokens = new List<string>();
        public List<string> fileNameTokens = new List<string>();
        public List<string> metadatadirectoryNameTokens = new List<string>();     
        public List<ReplaceToken> fileNameReplaceTokens = new List<ReplaceToken>();
        public List<ReplaceToken> directoryReplaceTokens = new List<ReplaceToken>();
        public List<ReplaceToken> fieldsReplaceTokens = new List<ReplaceToken>();
        //public string currentJobType = "";
        public ExportRules exportRule = new ExportRules();

        public ExportConfigurationForm()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void ExportConfigurationForm_Load(object sender, EventArgs e)
        {
            string urlParameters = "";
            string URL = "";
            string returnMessage = "";
            HttpResponseMessage response = new HttpResponseMessage();
            DialogResult result = new DialogResult();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);

            try
            {

                // Get Jobs Fields and populate List Views that display these fields names
                this.Text = "EXPORT CONFIGURATION - " + Data.GlovalVariables.currentJobName;
                URL = Data.GlovalVariables.BaseURL + "Fields/GetFieldsByJobID?jobID=" + Data.GlovalVariables.currentJobID.ToString();
                client.BaseAddress = new Uri(URL);
                response = client.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;

                        ResultFields resultFields = JsonConvert.DeserializeObject<ResultFields>(returnMessage);
                        if (resultFields.RecordsCount > 0)
                        {
                            foreach (Field field in resultFields.ReturnValue)
                            {
                                string[] row = { };
                                var listViewItem = new ListViewItem(row);
                                BatchVariableList.Items.Add(field.CPFieldName.Trim());
                                BatchVariableList2.Items.Add(field.CPFieldName.Trim());
                                FieldNamesComboBox.Items.Add(field.CPFieldName.Trim());
                                AvaliableFieldsLlistView.Items.Add(field.CPFieldName.Trim());
                            }
                            AvaliableFieldsLlistView.Items.Add("FileName");
                            AvaliableFieldsLlistView.Items.Add("FileDirectory");
                            AvaliableFieldsLlistView.Items.Add("FullFileName");
                            AvaliableFieldsLlistView.Items.Add("BatchName");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                    "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Look for tis Job in ExportRules Table so we can determine if this is a new Rule or an existing one
                client = new HttpClient();
                URL = Data.GlovalVariables.BaseURL + "Export/GetExportRulesByJobID?jobID=" + Data.GlovalVariables.currentJobID.ToString();
                client.BaseAddress = new Uri(URL);
                response = client.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        Task<string> resultTemp = content.ReadAsStringAsync();
                        returnMessage = resultTemp.Result;

                        ResultExportRules resultExportRules = JsonConvert.DeserializeObject<ResultExportRules>(returnMessage);
                        if (resultExportRules.RecordsCount > 0)
                        {
                            //ExportRules exportRule = new ExportRules();
                            exportRule = resultExportRules.ReturnValue[0];
                            //Leo currentJobType = resultExportRules.ReturnValue[0].JobName.Trim();
                           
                            // The Export rules Exist, so set windows form with the ExportRule Data
                            // Build Directory Format string
                            directoryNameTokens = exportRule.DirectoryFormat;                             
                            DirectoryFormatRichhTextBox.Text = "";
                            foreach (var token in exportRule.DirectoryFormat)
                            {
                                directoryNameTokensOriginal.Add(token.Trim());
                                DirectoryFormatRichhTextBox.Text = DirectoryFormatRichhTextBox.Text + token.Trim();                               
                            }

                            // Build File Name Format String
                            fileNameTokens = exportRule.FileNameFormat;                           
                            FileNameFormatRichTextBox.Text = "";
                            foreach (var token in exportRule.FileNameFormat)
                            {
                                fileNameTokensOriginal.Add(token.Trim());
                                FileNameFormatRichTextBox.Text = FileNameFormatRichTextBox.Text + token.Trim();
                            }

                            // Build Metadata Directory Format String
                            metadatadirectoryNameTokens = exportRule.MetadataDirectoryFormat;
                            MetadataDirectoryFormatRichhTextBox.Text = "";
                            foreach (var token in exportRule.MetadataDirectoryFormat)
                            {
                                metadatadirectoryNameTokensOriginal.Add(token.Trim());
                                MetadataDirectoryFormatRichhTextBox.Text = MetadataDirectoryFormatRichhTextBox.Text + token.Trim();
                            }

                            // Build Directory Replace List
                           // directoryReplaceTokensOriginal = exportRule.DirectoryReplaceRule;
                            foreach (ListViewItem item in DirReplacementListView.Items)
                                DirReplacementListView.Items.Remove(item);
                            foreach (var token in exportRule.DirectoryReplaceRule)
                            {
                                string[] row = { token.Pattern, token.ReplaceBy };
                                var listViewItem = new ListViewItem(row);
                                DirReplacementListView.Items.Add(listViewItem);
                            }
                            directoryReplaceTokensOriginal = exportRule.DirectoryReplaceRule;

                            // Build File Name Replace List
                            foreach (ListViewItem item in FileNameReplacementListView.Items)
                                FileNameReplacementListView.Items.Remove(item);
                            foreach (var token in exportRule.FileNameReplaceRule)
                            {
                                string[] row = { token.Pattern, token.ReplaceBy };
                                var listViewItem = new ListViewItem(row);
                                fileNameReplaceTokensOriginal.Add(token);
                                FileNameReplacementListView.Items.Add(listViewItem);
                            }

                            // Build Fields Replace List
                            foreach (ListViewItem item in FieldsReplacementListView.Items)
                                FieldsReplacementListView.Items.Remove(item);
                            foreach (var token in exportRule.FieldsReplaceRule)
                            {
                                string[] row = { token.Name, token.Pattern, token.ReplaceBy };
                                var listViewItem = new ListViewItem(row);
                                fieldsReplaceTokensOriginal.Add(token);
                                FieldsReplacementListView.Items.Add(listViewItem);
                            }
                            
                            //Build Selected Metadata Fields List]
                            if (exportRule.OutputFields != null)
                            {
                                foreach (string item in exportRule.OutputFields)
                                {
                                    string[] row = { item };
                                    var listViewItem = new ListViewItem(row);
                                    SelectedFieldsLlistView.Items.Add(listViewItem);
                                    // Remove field name from available Field Names List
                                    foreach (ListViewItem item2 in AvaliableFieldsLlistView.Items)
                                    {
                                        if (item2.Text == item)
                                        {
                                            AvaliableFieldsLlistView.Items.Remove(item2);
                                            break;
                                        }
                                    }
                                }
                            }
                            selectedOutputFields = exportRule.OutputFields;

                            // Set other values such as : export file name, Replace Action, Output File Format, Output File Delimeter
                            MetadataFileNameTextBox.Text = exportRule.MetadataFileName;
                            metadataFileNameOrignal = exportRule.MetadataFileName;
                            MetadataFileDelimeterComboBox.Text = exportRule.OutputFileDelimeter;
                            delimiterOriginal = exportRule.OutputFileDelimeter;
                            IncludeHeaderCheckBox.Checked = exportRule.IncludeHeaderFlag;
                            includeHeaderOriginal = exportRule.IncludeHeaderFlag;

                            if (exportRule.OutputFileFormat.Trim() == "CSV")
                                OutpuFormatCSVRadioButton.Checked = true;

                            if (exportRule.OutputFileFormat.Trim() == "XML")
                                OutpuFormatXMLRadioButton.Checked = true;
                            metadataFileFormatOriginal = exportRule.OutputFileFormat.Trim();
                           
                            
                            if (OutpuFormatCSVRadioButton.Checked)
                            {
                                IncludeHeaderCheckBox.Enabled = true;
                                MetadataFileDelimeterComboBox.Enabled = true;
                            }
                            else
                            {
                                IncludeHeaderCheckBox.Enabled = false;
                                MetadataFileDelimeterComboBox.Enabled = false;
                            }

                            if (exportRule.MetadataFileName.Trim() != "Batch Name")
                            {
                                UseOtherNameMetadataRadioButton.Checked = true;
                                MetadataFileNameTextBox.Enabled = true;
                            }
                            else
                            {
                                MetadataFileNameTextBox.Enabled = false;
                            }
                            MetadataFileNameTextBox.Text = exportRule.MetadataFileName;
                            metadataFileNameOrignal = exportRule.MetadataFileName.Trim();


                        }
                        else
                        {
                            directoryNameTokensOriginal = new List<string>();
                           // directoryNameTokensOriginal = directoryNameTokens;
                            fileNameTokensOriginal = fileNameTokens;

                            // This is a new Export Rule, then use the default information for this form
                            // UNDER DEVELOPMENT ...........................
                            if (UseOtherNameMetadataRadioButton.Checked)
                            {
                                MetadataFileNameTextBox.Enabled = true;
                                MetadataFileNameTextBox.Text = "";
                            }
                            else
                            {
                                MetadataFileNameTextBox.Enabled = false;
                                MetadataFileNameTextBox.Text = "Batch Name";
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
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SystemVariableListView_DoubleClick(object sender, EventArgs e)
        {
            DirectoryFormatRichhTextBox.Text = DirectoryFormatRichhTextBox.Text + "[" + SystemVariableListView.SelectedItems[0].Text + "]";
            directoryNameTokens.Add("[" + SystemVariableListView.SelectedItems[0].Text + "]");
        }

        private void BatchVariableList_DoubleClick(object sender, EventArgs e)
        {
            DirectoryFormatRichhTextBox.Text = DirectoryFormatRichhTextBox.Text + "{" + BatchVariableList.SelectedItems[0].Text + "}";
            directoryNameTokens.Add("{" + BatchVariableList.SelectedItems[0].Text + "}");
        }

        private void BatchVariableList2_DoubleClick(object sender, EventArgs e)
        {
            FileNameFormatRichTextBox.Text = FileNameFormatRichTextBox.Text + "{" + BatchVariableList2.SelectedItems[0].Text + "}";
            fileNameTokens.Add("{" + BatchVariableList2.SelectedItems[0].Text + "}");
        }

        private void SystemVariableListView2_DoubleClick(object sender, EventArgs e)
        {
            FileNameFormatRichTextBox.Text = FileNameFormatRichTextBox.Text + "[" + SystemVariableListView2.SelectedItems[0].Text + "]";
            fileNameTokens.Add("[" + SystemVariableListView2.SelectedItems[0].Text + "]");
        }

        private void AddDirSeparatorButton_Click(object sender, EventArgs e)
        {
            if (!DirectoryFormatRichhTextBox.Text.EndsWith("\\\""))
            {
                DirectoryFormatRichhTextBox.Text = DirectoryFormatRichhTextBox.Text + "\"\\\"";
                directoryNameTokens.Add("\"\\\"");
            }
        }

        private void AddMetadataDirSeparatorButton_Click(object sender, EventArgs e)
        {
            if (!MetadataDirectoryFormatRichhTextBox.Text.EndsWith("\\\""))
            {
                MetadataDirectoryFormatRichhTextBox.Text = MetadataDirectoryFormatRichhTextBox.Text + "\"\\\"";
                metadatadirectoryNameTokens.Add("\"\\\"");
            }
        }

        private void DirAddReplacementeButton_Click(object sender, EventArgs e)
        {
            Boolean itemFound = false;
            DialogResult result = new DialogResult();

            if (DirStringTextBox.Text.Length > 0)
            {
                foreach (ListViewItem item in DirReplacementListView.Items)
                {
                    if (item.Text == ("'" + DirStringTextBox.Text + "'"))
                    {
                        itemFound = true;
                        break;
                    }
                }
                if (!itemFound)
                {
                    // Also check if the Value does not exist --> display a message saying tha the Pattern String already exist
                    string[] row = { "'" + DirStringTextBox.Text + "'", "'" + DirReplaceStringTextBox.Text + "'" };
                    var listViewItem = new ListViewItem(row);
                    DirReplacementListView.Items.Add(listViewItem);
                    DirStringTextBox.Text = "";
                    DirReplaceStringTextBox.Text = "";
                }
                else
                {
                    result = MessageBox.Show("String/Char " + "'" + DirStringTextBox.Text + "'" + " already in the List", "Alert...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void FileNameAddReplacementeButton_Click(object sender, EventArgs e)
        {
            Boolean itemFound = false;
            DialogResult result = new DialogResult();

            if (FileNameStringTextBox.Text.Length > 0)
            {
                foreach (ListViewItem item in FileNameReplacementListView.Items)
                {
                    if (item.Text == ("'" + FileNameStringTextBox.Text + "'"))
                    {
                        itemFound = true;
                        break;
                    }
                }
                if (!itemFound)
                {
                    // Also check if the Value does not exist --> display a message saying tha the Pattern String already exist
                    string[] row = { "'" + FileNameStringTextBox.Text + "'", "'" + FileNameReplaceStringTextBox.Text + "'" };
                    var listViewItem = new ListViewItem(row);
                    FileNameReplacementListView.Items.Add(listViewItem);
                    FileNameStringTextBox.Text = "";
                    FileNameReplaceStringTextBox.Text = "";

                }
                else
                {
                    result = MessageBox.Show("String/Char " + "\"" + FileNameStringTextBox.Text + "\"" + " already in the List", "Alert...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void FieldAddReplacementeButton_Click(object sender, EventArgs e)
        {
            Boolean itemFound = false;
            DialogResult result = new DialogResult();

            //Check if string is not empty
            //if (!string.IsNullOrEmpty(FieldStringTextBox.Text))
            //{
            if (FieldNamesComboBox.Text.Length > 0) //&& FieldStringTextBox.Text.Length > 0)
            {
                foreach (ListViewItem item in FieldsReplacementListView.Items)
                {
                    if (item.SubItems[1].Text == ("'" + FieldStringTextBox.Text + "'"))
                    {
                        itemFound = true;
                        break;
                    }
                }
                if (!itemFound)
                {
                    // Also check if the Value does not exist --> display a message saying tha the Pattern String already exist
                    string[] row = { FieldNamesComboBox.SelectedItem.ToString(), "'" + FieldStringTextBox.Text + "'", "'" + FieldReplaceStringTextBox.Text + "'" };
                    var listViewItem = new ListViewItem(row);
                    FieldsReplacementListView.Items.Add(listViewItem);
                    FieldStringTextBox.Text = "";
                    FieldReplaceStringTextBox.Text = "";
                }
                else
                {
                    result = MessageBox.Show("String/Char " + "\"" + FieldStringTextBox.Text + "\"" + " already in the List", "Alert...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            //}
        }

        private void DirUpdateReplacementeButton_Click(object sender, EventArgs e)
        {
            if (DirReplacementListView.SelectedItems.Count > 0)
            {
                DirReplacementListView.SelectedItems[0].SubItems[1].Text = "'" + DirReplaceStringTextBox.Text + "'";
                DirReplaceStringTextBox.Text = "";
            }
        }

        private void DirReplacementeUpButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in DirReplacementListView.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int index = lvi.Index - 1;
                    DirReplacementListView.Items.RemoveAt(lvi.Index);
                    DirReplacementListView.Items.Insert(index, lvi);
                }
            }
        }

        private void DirReplacementDownButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in DirReplacementListView.SelectedItems)
            {
                if (lvi.Index >= 0)
                {
                    if (!(lvi.Index == (DirReplacementListView.Items.Count) - 1))
                    {
                        int index = lvi.Index + 1;
                        DirReplacementListView.Items.RemoveAt(lvi.Index);
                        DirReplacementListView.Items.Insert(index, lvi);
                    }

                }
            }
        }

        private void DirDeleteReplacementeButton_Click(object sender, EventArgs e)
        {
            DialogResult result = new DialogResult();
            if (DirReplacementListView.SelectedItems.Count > 0)
            {
                var values = new[] { "~", "#", "%", "&", "*", "{", "}", "/", ",", "[", "]", "?", "\\", "\"" };
                if (values.Any(DirReplacementListView.SelectedItems[0].Text.Contains))
                {
                    result = MessageBox.Show("String/Char " + "\"" + DirReplacementListView.SelectedItems[0].Text + "\"" + " cannot be removed from this list", "Alert...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    DirReplacementListView.SelectedItems[0].Remove();
            }
        }

        private void AddFiedToSelectedListButton_Click(object sender, EventArgs e)
        {
            if (AvaliableFieldsLlistView.SelectedItems.Count != 0)
            {
                string[] row = { AvaliableFieldsLlistView.SelectedItems[0].Text };
                var listViewItem = new ListViewItem(row);

                SelectedFieldsLlistView.Items.Add(listViewItem);
                AvaliableFieldsLlistView.SelectedItems[0].Remove();
            }
        }

        private void RemoveFiedFromSelectedListButton_Click(object sender, EventArgs e)
        {
            if (SelectedFieldsLlistView.SelectedItems.Count != 0)
            {
                string[] row = { SelectedFieldsLlistView.SelectedItems[0].Text };
                var listViewItem = new ListViewItem(row);

                AvaliableFieldsLlistView.Items.Add(listViewItem);
                SelectedFieldsLlistView.SelectedItems[0].Remove();
            }
        }

        private void SelectedFieldsUpButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in SelectedFieldsLlistView.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int index = lvi.Index - 1;
                    SelectedFieldsLlistView.Items.RemoveAt(lvi.Index);
                    SelectedFieldsLlistView.Items.Insert(index, lvi);
                }
            }
        }

        private void SelectedFieldsDownButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in SelectedFieldsLlistView.SelectedItems)
            {
                if (lvi.Index >= 0)
                {
                    if (!(lvi.Index == (SelectedFieldsLlistView.Items.Count) - 1))
                    {
                        int index = lvi.Index + 1;
                        SelectedFieldsLlistView.Items.RemoveAt(lvi.Index);
                        SelectedFieldsLlistView.Items.Insert(index, lvi);
                    }
                }
            }
        }

        private void DirAddFreeTextButton_Click(object sender, EventArgs e)
        {
            // Check that the string is not empty
            if (!string.IsNullOrEmpty(DirFreeTextBox.Text.Trim()))
            {
                DirectoryFormatRichhTextBox.Text = DirectoryFormatRichhTextBox.Text + "\"" + DirFreeTextBox.Text.Trim() + "\"";
                directoryNameTokens.Add("\"" + DirFreeTextBox.Text.Trim() + "\"");
            }
        }

        private void DirRemoveLastTokenButton_Click(object sender, EventArgs e)
        {
            if (directoryNameTokens.Count > 0 && DirectoryFormatRichhTextBox.TextLength != 0)
                directoryNameTokens.RemoveAt(directoryNameTokens.Count - 1);

            //Recreate the content of the Directory Tokens List
            DirectoryFormatRichhTextBox.Text = "";
            foreach (var item in directoryNameTokens)
            {
                DirectoryFormatRichhTextBox.Text = DirectoryFormatRichhTextBox.Text.Trim() + item.ToString().Trim();
            }
        }

        private void MetadataDirRemoveLastTokenButton_Click(object sender, EventArgs e)
        {
            if (metadatadirectoryNameTokens.Count > 0 && MetadataDirectoryFormatRichhTextBox.TextLength != 0)
                metadatadirectoryNameTokens.RemoveAt(metadatadirectoryNameTokens.Count - 1);

            //Recreate the content of the Directory Tokens List
            MetadataDirectoryFormatRichhTextBox.Text = "";
            foreach (var item in metadatadirectoryNameTokens)
            {
                MetadataDirectoryFormatRichhTextBox.Text = MetadataDirectoryFormatRichhTextBox.Text.Trim() + item.ToString().Trim();
            }
        }

        private void FileNameRemoveLastTokenButton_Click(object sender, EventArgs e)
        {
            if (fileNameTokens.Count >= 0 && FileNameFormatRichTextBox.TextLength != 0)
                fileNameTokens.RemoveAt(fileNameTokens.Count - 1);

            //Recreate the content of the File Names Tokens List
            FileNameFormatRichTextBox.Text = "";
            foreach (var item in fileNameTokens)
            {
                FileNameFormatRichTextBox.Text = FileNameFormatRichTextBox.Text.Trim() + item.ToString();
            }
        }

        private void FileNameAddFreeTextButton_Click(object sender, EventArgs e)
        {
            // Check that the string is not empty
            if (!string.IsNullOrEmpty(FileFreeTextBox.Text.Trim()))
            {
                FileNameFormatRichTextBox.Text = FileNameFormatRichTextBox.Text + "\"" + FileFreeTextBox.Text.Trim() + "\"";
                fileNameTokens.Add("\"" + FileFreeTextBox.Text.Trim() + "\"");
            }
        }



        private void FileNameReplacementeUpButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in FileNameReplacementListView.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int index = lvi.Index - 1;
                    FileNameReplacementListView.Items.RemoveAt(lvi.Index);
                    FileNameReplacementListView.Items.Insert(index, lvi);
                }
            }
        }

        private void FileNameReplacementDownButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in FileNameReplacementListView.SelectedItems)
            {
                if (lvi.Index >= 0)
                {
                    if (!(lvi.Index == (FileNameReplacementListView.Items.Count) - 1))
                    {
                        int index = lvi.Index + 1;
                        FileNameReplacementListView.Items.RemoveAt(lvi.Index);
                        FileNameReplacementListView.Items.Insert(index, lvi);
                    }
                }
            }
        }


        private void FileNameUpdateReplacementeButton_Click(object sender, EventArgs e)
        {
            if (FileNameReplacementListView.SelectedItems.Count > 0)
            {
                FileNameReplacementListView.SelectedItems[0].SubItems[1].Text = "'" + FileNameReplaceStringTextBox.Text + "'";
                DirReplaceStringTextBox.Text = "";
            }
        }

        private void FileNameDeleteReplacementeButton_Click(object sender, EventArgs e)
        {
            DialogResult result = new DialogResult();
            if (FileNameReplacementListView.SelectedItems.Count > 0)
            {
                var values = new[] { "~", "#", "%", "&", "*", "{", "}", "/", ",", "[", "]", "?", "\\", "\"" };
                if (values.Any(FileNameReplacementListView.SelectedItems[0].Text.Contains))
                {
                    result = MessageBox.Show("String/Char " + "'" + FileNameReplacementListView.SelectedItems[0].Text + "'" + " cannot be removed from this list", "Alert...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    FileNameReplacementListView.SelectedItems[0].Remove();
            }
        }

        private void FieldReplacementeUpButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in FieldsReplacementListView.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int index = lvi.Index - 1;
                    FieldsReplacementListView.Items.RemoveAt(lvi.Index);
                    FieldsReplacementListView.Items.Insert(index, lvi);
                }
            }
        }

        private void FieldReplacementDownButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in FieldsReplacementListView.SelectedItems)
            {
                if (lvi.Index >= 0)
                {
                    if (!(lvi.Index == (FieldsReplacementListView.Items.Count) - 1))
                    {
                        int index = lvi.Index + 1;
                        FieldsReplacementListView.Items.RemoveAt(lvi.Index);
                        FieldsReplacementListView.Items.Insert(index, lvi);
                    }
                }
            }
        }

        private void FieldDeleteReplacementeButton_Click(object sender, EventArgs e)
        {
            if (FieldsReplacementListView.SelectedItems.Count > 0)
            {
                FieldsReplacementListView.SelectedItems[0].Remove();
            }
        }

        private void FieldUpdateReplacementeButton_Click(object sender, EventArgs e)
        {
            if (FieldsReplacementListView.SelectedItems.Count > 0 && (!string.IsNullOrEmpty(FieldReplaceStringTextBox.Text.Trim())))
            {
                FieldsReplacementListView.SelectedItems[0].SubItems[2].Text = FieldReplaceStringTextBox.Text.Trim();
                FieldReplaceStringTextBox.Text = "";
            }
        }

        private void BrowseEsportDirButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                ExportOutputDirTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void DirFreeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateValidCharInFileDirNamesExtended(sender, e);
        }

        private void DirReplaceStringTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateValidCharInFileDirNames(sender, e);
        }

        private void FileFreeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateValidCharInFileDirNames(sender, e);
        }

        private void FileNameReplaceStringTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateValidCharInFileDirNames(sender, e);
        }

        private void MetadataFileNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateValidCharInFileDirNames(sender, e);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            StatusGroupBox.Visible = false;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(15);
            string urlParameters = "";
            string URL = "";
            HttpResponseMessage response = new HttpResponseMessage();
            string returnMessage;

            BatchStatusTextBox.Text = "";
            BatchNameTextBox.Text = "";
            AliasTextBox.Text = "";
            NumDocsTextBox.Text = "";
            CustomerNameTextBox.Text = "";
            JobTypeTextBox.Text = "";
            WorkOrderTextBox.Text = "";

            URL = Data.GlovalVariables.BaseURL + "Batches/GetBatchesInformation?filter=" + "BatchNumber = \"" + Uri.EscapeDataString(BatchNumberTextBox.Text.Trim()) + "\" OR BatchAlias = \"" + Uri.EscapeDataString(BatchNumberTextBox.Text.Trim()) + "\"";
            client.BaseAddress = new Uri(URL);
            response = client.GetAsync(urlParameters).Result;

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (HttpContent content = response.Content)
                {
                    Task<string> resultTemp = content.ReadAsStringAsync();
                    returnMessage = resultTemp.Result;

                    ResultBatches resultBatches = JsonConvert.DeserializeObject<ResultBatches>(returnMessage);
                    if (resultBatches.RecordsCount > 0)
                    {

                        Batch batch = new Batch();
                        batch = resultBatches.ReturnValue[0];

                        if (Data.GlovalVariables.currentJobName.ToUpper() == batch.JobType.Trim().ToUpper())
                        {
                            BatchStatusTextBox.Text = batch.StatusFlag;
                            BatchNameTextBox.Text = batch.BatchNumber;
                            AliasTextBox.Text = batch.BatchAlias;
                            NumDocsTextBox.Text = Convert.ToString(batch.NumberOfDocuments);
                            CustomerNameTextBox.Text = batch.Customer;
                            JobTypeTextBox.Text = batch.JobType;
                            WorkOrderTextBox.Text = Convert.ToString(batch.LotNumber);
                        }
                        else
                        {
                            BatchStatusTextBox.Text = "";
                            BatchNameTextBox.Text = "";
                            AliasTextBox.Text = "";
                            NumDocsTextBox.Text = "";
                            CustomerNameTextBox.Text = "";
                            JobTypeTextBox.Text = "";
                            WorkOrderTextBox.Text = "";
                            MessageBox.Show("The Batch Name you searched for does not Match the Job Type you are currently working on. You must search for a Batch with Job Type : \"" + Data.GlovalVariables.currentJobName + "\"",
                                        "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Batch Not Found !.",
                                        "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
            string exportRulesJS = "";
            string returnMessage = "";
            ExportRules exportRules = new ExportRules();
            ResultExportRules resulExportRules = new ResultExportRules();

            try
            {
                // Check the minimun information required to save the export rule record 
                if (UseOtherNameMetadataRadioButton.Checked)
                {
                    if (string.IsNullOrEmpty(MetadataFileNameTextBox.Text))
                    {
                        //OptionsTabControl.SelectedTab = Parameters;
                        ExportTabControl.SelectedTab = OutputDefinitionTabPage;
                        toolTip1.ToolTipTitle = "No value provided.";
                        MetadataFileNameTextBox.Focus();
                        toolTip1.Show("You need to provide a name for the Metadata Output File or use the Default Batch Name option.", MetadataFileNameTextBox, 5000);
                        MetadataFileNameTextBox.Focus();
                        MetadataFileNameTextBox.Select();
                        return;
                    }
                }

                exportRules.JobID = Data.GlovalVariables.currentJobID;
                exportRules.DirectoryFormat = directoryNameTokens;
                exportRules.FileNameFormat = fileNameTokens;
                exportRules.MetadataDirectoryFormat = metadatadirectoryNameTokens;

                // -------------------------------------------------------------------------------
                // Build Directory Replace Rules List
                directoryReplaceTokens.Clear();
                exportRules.DirectoryReplaceRule = new List<ReplaceToken>();
                foreach (ListViewItem item in DirReplacementListView.Items)
                {
                    ReplaceToken token = new ReplaceToken();
                    token.Pattern = item.SubItems[0].Text.Replace("'", "\"");
                    token.ReplaceBy = item.SubItems[1].Text.Replace("'", "\"");
                    directoryReplaceTokens.Add(token);
                }
                exportRules.DirectoryReplaceRule = directoryReplaceTokens;
                // ---------------------------------------------------------------------------------

                // Build File Name Replace Rules List
                fileNameReplaceTokens.Clear();
                exportRules.FileNameReplaceRule = new List<ReplaceToken>();
                foreach (ListViewItem item in FileNameReplacementListView.Items)
                {
                    ReplaceToken token = new ReplaceToken();
                    token.Pattern = item.SubItems[0].Text.Replace("'", "\"");
                    token.ReplaceBy = item.SubItems[1].Text.Replace("'", "\"");
                    fileNameReplaceTokens.Add(token);
                }
                exportRules.FileNameReplaceRule = fileNameReplaceTokens;
                // ---------------------------------------------------------------------------------

                // Build fields  Replace Rules List
                fieldsReplaceTokens.Clear();
                exportRules.FieldsReplaceRule = new List<ReplaceToken>();
                foreach (ListViewItem item in FieldsReplacementListView.Items)
                {
                    ReplaceToken token = new ReplaceToken();
                    token.Name = item.SubItems[0].Text;
                    token.Pattern = item.SubItems[1].Text.Replace("'", "\"");
                    token.ReplaceBy = item.SubItems[2].Text.Replace("'", "\"");
                    fieldsReplaceTokens.Add(token);
                }
                exportRules.FieldsReplaceRule = fieldsReplaceTokens;
                // ---------------------------------------------------------------------------------

                exportRules.OutputFields = new List<string>();
                // ---------------------------------------------------------------------------------   
                // Output File Format
                if (OutpuFormatXMLRadioButton.Checked)
                {
                    exportRules.OutputFileFormat = "XML";
                }
                else
                {
                    if (OutpuFormatCSVRadioButton.Checked)
                        exportRules.OutputFileFormat = "CSV";
                }
                // --------------------------------------------------------------------------------- 

                // ---------------------------------------------------------------------------------
                // Build Selected Meadata Fields
                exportRules.OutputFields = new List<string>();
                foreach (ListViewItem item in SelectedFieldsLlistView.Items)
                {
                    exportRules.OutputFields.Add(item.Text);
                }

                if (IncludeHeaderCheckBox.Checked)
                    exportRules.IncludeHeaderFlag = true;
                else
                    exportRules.IncludeHeaderFlag = false;
                // ---------------------------------------------------------------------------------

                exportRules.OutputFileDelimeter = MetadataFileDelimeterComboBox.Text;
                exportRules.FieldsReplaceRule = fieldsReplaceTokens;
                exportRules.MetadataFileName = MetadataFileNameTextBox.Text;

                // Build exportRules Object in Json Format and Add/Update Record in the Database
                exportRulesJS = JsonConvert.SerializeObject(exportRules, Newtonsoft.Json.Formatting.Indented);
                exportRulesJS = exportRulesJS.Replace(@"\", "\\\\");
                URL = Data.GlovalVariables.BaseURL + "Export/UpdateJobExportRules";
                bodyString = "'" + exportRulesJS + "'";
                HttpContent body_for_update = new StringContent(bodyString);
                body_for_update.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response_for_update = client.PostAsync(URL, body_for_update).Result;
                using (HttpContent content = response_for_update.Content)
                {
                    Task<string> resultTemp = content.ReadAsStringAsync();
                    returnMessage = resultTemp.Result;
                    resulExportRules = JsonConvert.DeserializeObject<ResultExportRules>(returnMessage);
                }
                if (response_for_update.IsSuccessStatusCode)
                {
                    if (resulExportRules.ReturnCode == -1)
                    {
                        MessageBox.Show("Warning:" + "\r\n" + resulExportRules.Message.Replace(". ", "\r\n"), "Update Job Export Rules Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Data.GlovalVariables.currentExportRulesConfigured = true;
                        Data.GlovalVariables.transactionType = "Update";
                        if (action == "SaveAndExit") this.Close();
                        else
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error:" + "\r\n" + resulExportRules.Message.Replace(". ", "\r\n") + "\r\n" + resulExportRules.Exception, "Update Job Export Rules Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }  
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);            }
        }

        private void DirStringTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void RunExportButton_Click(object sender, EventArgs e)
        {
            nlogger.Trace("Entering into RunExportButton Method ...");
            try
            {
                
                Cursor.Current = Cursors.WaitCursor;
                
                ExportTransactionsJob exportTransactions = new ExportTransactionsJob();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(15);
                string urlParameters = "";
                string URL = "";
                string returnMessage;
                string batchName = "";
                int numFilesFound = 0;
                int numFilesNotFound = 0;
                batchName = BatchNumberTextBox.Text.Trim();

                if (string.IsNullOrEmpty(BatchNameTextBox.Text) || string.IsNullOrEmpty(BatchStatusTextBox.Text))
                {
                    MessageBox.Show("You need to provide a Batch inforamtion before this Test.",
                                    "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nlogger.Trace("You need to provide a Batch inforamtion before this Test.");
                }
                else
                {
                    if (!(BatchStatusTextBox.Text == "Approved" || BatchStatusTextBox.Text == "Waiting for Approval" || BatchStatusTextBox.Text == "Exported"))
                    {
                        MessageBox.Show("Only Batches in Approved, Waiting for Approval, and Export Status, could be used for Test",
                                    "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        nlogger.Trace("Only Batches in Approved, Waiting for Approval, and Export Status, could be used for Test.");
                    }
                    else
                    {

                        if (string.IsNullOrEmpty(ExportOutputDirTextBox.Text))
                        {
                            MessageBox.Show("You need to select the Base Location for the Export to run the Test.",
                                   "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            nlogger.Trace("You need to select the Base Location for the Export to run the Test.");
                        }
                        else
                        {
                            // Get Export Transaction Jobs
                            StatusGroupBox.Visible = false;
                            ProgressBar.Visible = false;
                            GeetingExportTransactionLabel.Visible = true;
                            ExportProgressGgroupBox.Visible = true;
                           
                            client = new HttpClient();
                            if (ProcessWorkOrderCheckBox.Checked)
                            {
                                // Process the entire Work Order
                                nlogger.Trace("  Getting Transactions for the entire Work Order. Work Order: " + WorkOrderTextBox.Text.Trim());
                                URL = Data.GlovalVariables.BaseURL + "Export/GetExportTransactionsJob?jobID=" + Data.GlovalVariables.currentJobID.ToString() + "&workOrder=" + WorkOrderTextBox.Text.Trim() + " &baseOutputDirectory=" + ExportOutputDirTextBox.Text;
                            }
                            else
                            {
                                // Process Just a given Batch                                
                                nlogger.Trace("  Getting Transactions for an especific Batch. Batch Name: " + BatchNameTextBox.Text.Trim());
                                URL = Data.GlovalVariables.BaseURL + "Export/GetExportTransactionsJob?jobID=" + Data.GlovalVariables.currentJobID.ToString() + "&batchName=" + Uri.EscapeDataString(BatchNameTextBox.Text.Trim()) + "&baseOutputDirectory=" + ExportOutputDirTextBox.Text;
                            }
                            client.BaseAddress = new Uri(URL);
                            response = client.GetAsync(urlParameters).Result;
                           
                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage,
                                                "Transaction Error...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                nlogger.Trace("Error:" + "\r\n" + response.ReasonPhrase + "\r\n" + response.RequestMessage);
                            }
                            else
                            {
                                using (HttpContent content = response.Content)
                                {
                                   
                                    Task<string> resultTemp = content.ReadAsStringAsync();
                                    returnMessage = resultTemp.Result;
                                    nlogger.Trace("  ");
                                    ResultExportTransactionsJob resultExportTransactionsJob = JsonConvert.DeserializeObject<ResultExportTransactionsJob>(returnMessage);
                                    if (resultExportTransactionsJob.RecordsCount > 0 && resultExportTransactionsJob.ReturnCode == 0)
                                    {
                                        BatchLlabel.Text = "";
                                        DocProcessedCountLabel.Text = "";
                                        StatusGroupBox.Visible = true;
                                        StatusGroupBox.Refresh();
                                        GeetingExportTransactionLabel.Visible = false;
                                        ProgressBar.Visible = true;
                                        ProgressBar.Minimum = 0;
                                        ProgressBar.Value = 0;
                                        ProgressBar.Maximum = resultExportTransactionsJob.RecordsCount;

                                        ExportTransactionsJob exportTransactionsJob = new ExportTransactionsJob();
                                        exportTransactionsJob = resultExportTransactionsJob.ReturnValue;

                                        // 1. Create Directories and Copy Files 
                                        int percent = 0;
                                        nlogger.Trace("  Creaing Directories and copying Files to Target location ....");

                                        foreach (ExportBatches batch in exportTransactionsJob.Batches)
                                        {
                                            nlogger.Trace("      Batch Name:" + batch.BatchName);
                                            BatchLlabel.Text = batch.BatchName;
                                            BatchLlabel.Refresh();

                                            foreach (ExportDocs document in batch.Documents)
                                            {
                                                nlogger.Trace("          Source File Location:" + document.FileLocation);
                                                nlogger.Trace("          Source File Name:" + document.FileName);
                                                nlogger.Trace("          Target File Location:" + document.TargetFileLocation);
                                                nlogger.Trace("          Target File Name:" + document.TargetFilename);
                                                ProgressBar.Value ++;
                                                
                                                DocProcessedCountLabel.Text = ProgressBar.Value.ToString() + " / " + resultExportTransactionsJob.RecordsCount.ToString();
                                                percent = (int)(((double)(ProgressBar.Value) /(double)(ProgressBar.Maximum)) * 100);
                                                StatusGroupBox.Refresh();

                                                using (Graphics gr = ProgressBar.CreateGraphics())
                                                {
                                                    gr.DrawString(percent.ToString() + "%", SystemFonts.DefaultFont, Brushes.Black,
                                                        new PointF(ProgressBar.Width / 2 - (gr.MeasureString(percent.ToString() + "%",
                                                            SystemFonts.DefaultFont).Width / 2.0F),
                                                        ProgressBar.Height / 2 - (gr.MeasureString(percent.ToString() + "%",
                                                            SystemFonts.DefaultFont).Height / 2.0F)));
                                                }

                                                nlogger.Trace("              Check if Target Directory Exist ...");
                                                if (!Directory.Exists(document.TargetFileLocation))
                                                {
                                                    DirectoryInfo di = Directory.CreateDirectory(document.TargetFileLocation);
                                                    Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(document.TargetFileLocation));
                                                    nlogger.Trace("              Directory did not exist. Directory was created successfully. ");
                                                }
                                                // Check if the Source File Exist
                                                nlogger.Trace("              Check if Source File Exist ...");
                                                if (File.Exists(document.FileLocation + "\\" + document.FileName))
                                                {
                                                    // Copy the file 
                                                    numFilesFound++;
                                                   File.Copy(document.FileLocation + "\\" + document.FileName, document.TargetFileLocation + "\\" + document.TargetFilename,true);
                                                    nlogger.Trace("              File  was copied duccessfully to target directory. ");
                                                }
                                                else
                                                {
                                                    numFilesNotFound++;
                                                }
                                            }
                                        }

                                        // 3. Create Metadata Output Directory and Output file
                                        nlogger.Trace("  Creaing Directories and copying Metadata File to Target location ....");
                                        foreach (MetadataFiles metadataFile in exportTransactionsJob.OutputFiles)
                                        {
                                            nlogger.Trace("          Metadata File Location: " + metadataFile.OutputFileLocation);
                                            nlogger.Trace("          Metadata File Name: " + metadataFile.OutputFileName);
                                            // Check if the Directory for the Output Metadata Fiel Exist ...
                                            nlogger.Trace("              Check if Target Directory Exist: " + metadataFile.OutputFileLocation);
                                            if (!Directory.Exists(metadataFile.OutputFileLocation))
                                            {
                                                DirectoryInfo di = Directory.CreateDirectory(metadataFile.OutputFileLocation);
                                                Console.WriteLine("     The directory was created successfully at {0}.", Directory.GetCreationTime(metadataFile.OutputFileLocation));
                                                nlogger.Trace("              Directory did not exist so it was created successfully. ");
                                            }
                                            switch (exportTransactionsJob.ExportRule.OutputFileFormat.Trim())
                                            {
                                                case "XML":
                                                    // Create the XmlDocument.
                                                    XmlDocument doc = new XmlDocument();
                                                    doc.LoadXml(metadataFile.Content);
                                                    // Save the document to a file and auto-indent the output.
                                                    using (XmlTextWriter writer = new XmlTextWriter(metadataFile.OutputFileLocation + "\\" + metadataFile.OutputFileName, null))
                                                    {
                                                        writer.Formatting = System.Xml.Formatting.Indented;
                                                        doc.Save(writer);
                                                    }
                                                    nlogger.Trace("              Metadata File Create: " + metadataFile.OutputFileLocation + "\\" + metadataFile.OutputFileName);
                                                    break;

                                                case "CSV":
                                                    // Removes Last return line fromn the file
                                                    metadataFile.Content = metadataFile.Content.TrimEnd(new char[] { '\r', '\n' });
                                                    File.WriteAllText(metadataFile.OutputFileLocation + "\\" + metadataFile.OutputFileName, metadataFile.Content);
                                                    nlogger.Trace("              Metadata File Create: " + metadataFile.OutputFileLocation + "\\" + metadataFile.OutputFileName);
                                                    break;
                                            }                                            
                                        }
                                        if (numFilesNotFound > 0)
                                        {
                                            MessageBox.Show("Warning:" + "\r\n" + "The were files that could not be copied to the arget location." + "\r\n" + "Total Files found: " + numFilesFound.ToString() + "\r\n" + "Total Files Not Found: " + numFilesNotFound.ToString(),
                                               "Test result warining ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            nlogger.Trace("Warning:" + "\r\n" + "The were files that could not be copied to the arget location." + "\r\n" + "Total Files found: " + numFilesFound.ToString() + "\r\n" + "Total Files Not Found: " + numFilesNotFound.ToString());
                                        }
                                       
                                        // Open Otput Directory
                                        OpenFolder(ExportOutputDirTextBox.Text);

                                    }
                                    else
                                    {
                                        MessageBox.Show("Error getting Export Transactions Job !.", "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        nlogger.Trace("Error getting Export Transactions Job !.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //logger.Error("          Error:  " + ex.Message);
                //logger.Error("          Inner Exception:  " + ex.InnerException);
            }
            ProgressBar.Value = 0;
            ExportProgressGgroupBox.Visible = false;
            Cursor.Current = Cursors.Default;
            nlogger.Trace("Leaving RunExportButton Method ...");
        }

        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                System.Diagnostics.Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
            }
        }

        private void MetadataDirAddFreeTextButton_Click(object sender, EventArgs e)
        {
            // Check that the string is not empty
            if (!string.IsNullOrEmpty(MetadataDirFreeTextBox.Text.Trim()))
            {
                MetadataDirectoryFormatRichhTextBox.Text = MetadataDirectoryFormatRichhTextBox.Text + "\"" + MetadataDirFreeTextBox.Text.Trim() + "\"";
                metadatadirectoryNameTokens.Add("\"" + MetadataDirFreeTextBox.Text.Trim() + "\"");
            }
        }
        

        private void SystemVariableListView3_DoubleClick(object sender, EventArgs e)
        {
            MetadataDirectoryFormatRichhTextBox.Text = MetadataDirectoryFormatRichhTextBox.Text + "[" + SystemVariableListView3.SelectedItems[0].Text + "]";
            metadatadirectoryNameTokens.Add("[" + SystemVariableListView3.SelectedItems[0].Text + "]");
        }

        private void OutpuFormatCSVRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (OutpuFormatCSVRadioButton.Checked)
            {
                IncludeHeaderCheckBox.Enabled = true;
                MetadataFileDelimeterComboBox.Enabled = true;
            }
            else
            {
                IncludeHeaderCheckBox.Enabled = false;
                MetadataFileDelimeterComboBox.Enabled = false;
            }
        }

        private void UseOtherNameMetadataRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (UseOtherNameMetadataRadioButton.Checked)
            {
                MetadataFileNameTextBox.Enabled = true;
                MetadataFileNameTextBox.Text = "";
            }               
            else
            {
                MetadataFileNameTextBox.Enabled = false;
                MetadataFileNameTextBox.Text = "Batch Name";
            }
        }

        private void AdvancedRulesTabPage_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaturdayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Removes the last entry submitted to the File Name or Directory Format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLastTokenButton_Click(object sender, EventArgs e)
        {

        }

        private void MetadataDirFreeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateValidCharInFileDirNamesExtended(sender, e);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            BatchNumberTextBox.Text = "";
            BatchNameTextBox.Text = "";
            AliasTextBox.Text = "";
            NumDocsTextBox.Text = "";
            WorkOrderTextBox.Text = "";
            BatchStatusTextBox.Text = "";
            JobTypeTextBox.Text = "";
            CustomerNameTextBox.Text = "";
            ExportOutputDirTextBox.Text = "";
            BatchLlabel.Text = "";
            DocProcessedCountLabel.Text = "";
            StatusGroupBox.Visible = false;
            ProcessWorkOrderCheckBox.Checked = false;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            // Variables to be used when the Rest Option is selected
            //directoryNameTokensOriginal = new List<string>();
            //directoryFormatOriginal = "";
            //fileNameTokensOriginal = new List<string>();
            //fileNameFormatOriginal = "";
            //metadatadirectoryNameTokensOriginal = new List<string>();
            //metadataDirectoryFormatOriginal = "";
            //fileNameReplaceTokensOriginal = new List<ReplaceToken>();
            //directoryReplaceTokensOriginal = new List<ReplaceToken>();
            //fieldsReplaceTokensOriginal = new List<ReplaceToken>();
            //selectedOutputFields = new List<string>();
            //metadataFileFormatOriginal = "";
            //includeHeaderOriginal = false;
            // delimiterOriginal = "";
            //metadataFileNameOrignal = "Batch Name";
            //duplicateNameActionOriginal = "Rename";

            // Reset Export Directory Formula and Directory Tokens List
            DirectoryFormatRichhTextBox.Text = "";
            directoryNameTokens.Clear();
            foreach (var token in directoryNameTokensOriginal)
            {
               directoryNameTokens.Add(token.Trim());
               DirectoryFormatRichhTextBox.Text = DirectoryFormatRichhTextBox.Text + token.Trim();
            }

            // Reset Directory Replace List
            directoryReplaceTokens = directoryReplaceTokensOriginal;
            foreach (ListViewItem item in DirReplacementListView.Items)
                DirReplacementListView.Items.Remove(item);
            foreach (var token in directoryReplaceTokens)
            {
                string[] row = { token.Pattern, token.ReplaceBy };
                var listViewItem = new ListViewItem(row);
                DirReplacementListView.Items.Add(listViewItem);
            }

            // Reset File Name Formula and Directory Tokens List
            FileNameFormatRichTextBox.Text = "";
            fileNameTokens.Clear();
            foreach (var token in fileNameTokensOriginal)
            {
                fileNameTokens.Add(token.Trim());
                FileNameFormatRichTextBox.Text = FileNameFormatRichTextBox.Text + token.Trim();
            }

            // Reset File Name Replace List
            fileNameReplaceTokens = fileNameReplaceTokensOriginal;
            foreach (ListViewItem item in FileNameReplacementListView.Items)
                FileNameReplacementListView.Items.Remove(item);
            foreach (var token in fileNameReplaceTokens)
            {
                string[] row = { token.Pattern, token.ReplaceBy };
                var listViewItem = new ListViewItem(row);
                FileNameReplacementListView.Items.Add(listViewItem);
            }

            // Reset Output File Name Formula and metadata Tokens List
            MetadataDirectoryFormatRichhTextBox.Text = "";
            metadatadirectoryNameTokens.Clear();
            foreach (var token in metadatadirectoryNameTokensOriginal)
            {
                metadatadirectoryNameTokens.Add(token.Trim());
                MetadataDirectoryFormatRichhTextBox.Text = MetadataDirectoryFormatRichhTextBox.Text + token.Trim();
            }

            // Reset File Name Replace List
            foreach (ListViewItem item in FileNameReplacementListView.Items)
                FileNameReplacementListView.Items.Remove(item);
            foreach (var token in fileNameReplaceTokensOriginal)
            {
                string[] row = { token.Pattern, token.ReplaceBy };
                var listViewItem = new ListViewItem(row);
                FileNameReplacementListView.Items.Add(listViewItem);
            }

            // Reset Build Fields Replace List
            foreach (ListViewItem item in FieldsReplacementListView.Items)
                FieldsReplacementListView.Items.Remove(item);
            foreach (var token in fieldsReplaceTokensOriginal)
            {
                string[] row = { token.Name, token.Pattern, token.ReplaceBy };
                var listViewItem = new ListViewItem(row);
                FieldsReplacementListView.Items.Add(listViewItem);
            }

            // Rest Metadata Output File Format
            if (metadataFileFormatOriginal.Trim() == "CSV")
            {
                OutpuFormatCSVRadioButton.Checked = true;
                IncludeHeaderCheckBox.Enabled = true;
                MetadataFileDelimeterComboBox.Enabled = true;
                if (includeHeaderOriginal)
                {   
                    IncludeHeaderCheckBox.Checked = true;
                }
                else
                {
                    IncludeHeaderCheckBox.Checked = false;                    
                }
                MetadataFileDelimeterComboBox.Text = delimiterOriginal;
            }

            if (metadataFileFormatOriginal.Trim() == "XML")
            {
                OutpuFormatXMLRadioButton.Checked = true;
                IncludeHeaderCheckBox.Enabled = false;
                MetadataFileDelimeterComboBox.Enabled = false;
                MetadataFileDelimeterComboBox.Text = "";
            }
            
            if (metadataFileNameOrignal.Trim() == "Batch Name")
            {
                UseBatchNameMetadataRadioButton.Checked = true;
                MetadataFileNameTextBox.Enabled = true;
            }
            else
            {
                UseBatchNameMetadataRadioButton.Checked = false;
                MetadataFileNameTextBox.Enabled = false;
            }
            MetadataFileNameTextBox.Text = metadataFileNameOrignal.Trim();
            
        }

        private void IncludeHeaderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (IncludeHeaderCheckBox.Checked)
            {
                MetadataFileDelimeterComboBox.Text = "Comma";
            }
            else
            {
                MetadataFileDelimeterComboBox.Text = "";
            }
        }

        private void OutpuFormatXMLRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (OutpuFormatXMLRadioButton.Checked)
            {
                IncludeHeaderCheckBox.Checked = false;
                MetadataFileDelimeterComboBox.Text = "";

            }
        }
    }
    }

