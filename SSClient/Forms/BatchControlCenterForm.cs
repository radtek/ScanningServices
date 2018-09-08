using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Newtonsoft.Json;

namespace ScanningServicesAdmin.Forms
{
    public partial class BatchControlCenterForm : Form
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public BatchControlCenterForm()
        {
            InitializeComponent();
            
        }

        private string SetFilter()
        {
            //BatchNumber.Contains("FTW BOX-01")
            string queryFilter = "";
            
            //Filter by Lot Number = Work Order Number
            if (LotNumberComboBox.Text.Length > 0)
            {
                switch (WOCondComboBox.Text)
                {
                    case "":
                        // DO nothung
                        break;

                    case ">":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber > " + LotNumberComboBox.Text;
                        else
                            queryFilter = queryFilter + " LotNumber > " + LotNumberComboBox.Text;
                        break;

                    case ">=":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber >= " + LotNumberComboBox.Text;
                        else
                            queryFilter = queryFilter + " LotNumber >= " + LotNumberComboBox.Text;
                        break;

                    case "=":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber = " + LotNumberComboBox.Text;
                        else
                            queryFilter = queryFilter + " LotNumber = " + LotNumberComboBox.Text;
                        break;

                    case "<":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber < " + LotNumberComboBox.Text;
                        else
                            queryFilter = queryFilter + " LotNumber < " + LotNumberComboBox.Text;
                        break;

                    case "<=":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber <= " + LotNumberComboBox.Text;
                        else
                            queryFilter = queryFilter + " LotNumber <= " + LotNumberComboBox.Text;
                        break;
                }
            }
           

            // Filter by Batch Number
            if (BatchNumberTextBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "(BatchNumber.Contains(\"" + BatchNumberTextBox.Text.Trim() + "\") OR BatchAlias.Contains(\"" + BatchNumberTextBox.Text.Trim() + "\"))";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "(BatchNumber.Contains(\"" + BatchNumberTextBox.Text.Trim() + "\") OR BatchAlias.Contains(\"" + BatchNumberTextBox.Text.Trim() + "\"))";
                }
            }

            // Filter by Batch Status
            if (BatchStatusComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "StatusFlag = \"" + BatchStatusComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "StatusFlag = \"" + BatchStatusComboBox.Text.Trim() + "\"";
                }
            }

            // Filter by Job Type
            if (JobTypeComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "JobType = \"" + JobTypeComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "JobType = \"" + JobTypeComboBox.Text.Trim() + "\"";
                }
            }

            // Filter by Customer
            if (CustomersComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "Customer = \"" + CustomersComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "Customer = \"" + CustomersComboBox.Text.Trim() + "\"";
                }
            }


            // Date Selection Filters
            if (SubmittedDateCheckBox.Checked)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " SubmittedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND SubmittedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " SubmittedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND SubmittedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
            }

            if (ScannedDateCheckBox.Checked)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " ScannedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND ScannedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " ScannedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND ScannedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
            }

            if (QCDateCheckBox.Checked)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " QCDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND QCDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " QCDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND QCDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
            }

            if (OutputDateCheckBox.Checked)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " OutputDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND OutputDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " OutputDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND OutputDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
            }

            if (ApprovedDateCheckBox.Checked)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " ApprovedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND ApprovedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " ApprovedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND ApprovedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
            }

            if (ExportedDateCheckBox.Checked)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " ExportedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND ExportedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " ExportedDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND ExportedDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
            }

            if (VFRUpdateDateCheckBox.Checked)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " VFRUploadDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND VFRUploadDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " VFRUploadDate >= \"" + ExportedDateTimePickerFrom.Value.Date + "\" AND VFRUploadDate <= \"" + ExportedDateTimePickerTo.Value.Date + "\"";
                }
            }
            
            // Filter by Scann Operator
            if (ScannedByComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "ScanOPerator = \"" + ScannedByComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "ScanOPerator = \"" + ScannedByComboBox.Text.Trim() + "\"";
                }
            }

            // Filter by Rejected by
            if (RejectedByComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "RejectedBy = \"" + RejectedByComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "RejectedBy = \"" + RejectedByComboBox.Text.Trim() + "\"";
                }
            }

            // Filter by QC by
            if (QCByComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "QCBy = \"" + QCByComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "QCBy = \"" + QCByComboBox.Text.Trim() + "\"";
                }
            }

            // Filter by Approved by
            if (ApprovedByComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "ApprovedBy = \"" + ApprovedByComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "ApprovedBy = \"" + ApprovedByComboBox.Text.Trim() + "\"";
                }
            }

            // Filter by Exported by
            if (ExportedByComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "ExportedBy = \"" + ExportedByComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "ExportedBy = \"" + ExportedByComboBox.Text.Trim() + "\"";
                }
            }

            // Filter by Output By
            if (OutputByComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "OutputBy = \"" + OutputByComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + "AND " + "OutputBy = \"" + OutputByComboBox.Text.Trim() + "\"";
                }
            }



            return queryFilter;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {

            int count = 0;
            Cursor.Current = Cursors.WaitCursor;
            string queryFilter = SetFilter();
            BatchList.Rows.Clear();    

            ResultBatches resultBatches = new ResultBatches();
            resultBatches = DBTransactions.GetBatchesInformation(queryFilter, "");

            //CircularProgressBar.Visible = true;
            //CircularProgressBar.Minimum = 0;
            //CircularProgressBar.Maximum =10000;
            //for (int i = 1; i <= 10000; i++)
            //{
            //    Thread.Sleep(5);
            //    count++;
            //    CircularProgressBar.Value = count;
            //    CircularProgressBar.Update();
            //}
            if (resultBatches.RecordsCount == 0)
            {
                QualityControlButton.Enabled = false;
                SelectAllButton.Enabled = false;
                UnselectAllButton.Enabled = false;
                StatusChangeButton.Enabled = false;
                CPScanDirectoryButton.Enabled = false;
                CPInfoFileButton.Enabled = false;
                CPBatchLogButton.Enabled = false;
                BatchTraceButton.Enabled = false;
                ReportButton.Enabled = false;
            }
            else
            {
                
                //StatusChangeButton.Enabled = true;
                //SelectAllButton.Enabled = true;
                //UnselectAllButton.Enabled = true;
                //QualityControlButton.Enabled = true;
                //CPScanDirectoryButton.Enabled = true;
                //CPInfoFileButton.Enabled = true;
                //CPBatchLogButton.Enabled = true;
                //BatchTraceButton.Enabled = true;
                //ReportButton.Enabled = true;

                //if (ApprovalSelectionRadioButton.Checked)
                //{
                //    ApproveSelectedButton.Enabled = true;
                //}
                //else
                //{
                //    ApproveSelectedButton.Enabled = false;
                //}

                //CircularProgressBar.Visible = true;
                //CircularProgressBar.Minimum = 0;
                //CircularProgressBar.Maximum = resultBatches.RecordsCount;

                NumberOfBatchesFoundButton.Text = "NUMBER OF BATCHES FOUND :  " + resultBatches.RecordsCount.ToString();
                foreach (Batch batch in resultBatches.ReturnValue)
                {
                    count++;
                    //CircularProgressBar.Value = count;
                    //CircularProgressBar.Update();
                    string[] row = new string[] { "False", batch.BatchNumber, batch.LotNumber.ToString(), batch.StatusFlag, batch.KodakStatus, batch.JobType.Trim(), batch.NumberOfPages.ToString(),
                                                   batch.NumberOfDocuments.ToString(), batch.SubmittedBy, batch.SubmittedDate.ToString(), batch.ScanOperator, batch.ScanStation, batch.ScannedDate.ToString(),
                                                   batch.QCBy, batch.QCStation, batch.QCDate.ToString(), batch.OutputBy, batch.OutputStation, batch.OutputDate.ToString(),
                                                   batch.ApprovedBy, batch.ApprovedDate.ToString(),batch.RejectedBy, batch.LastTimeRejected.ToString(),batch.RejectedTimes.ToString(),
                                                   batch.ExportedBy,batch.ExportedDate.ToString(), batch.Comments, batch.BlockNumber.ToString(),batch.ProjectName, batch.DepName, batch.KodakErrorState, 
                                                   batch.PrepUserName, batch.PrepDate.ToString(), batch.KeysStrokes.ToString(), batch.QCStartTime.ToString(), batch.QCEndTime.ToString(),
                                                   batch.QCTime.ToString(), batch.QCStageTime.ToString(),batch.RecallBy, batch.RecallDate.ToString(),batch.RecallTimes.ToString(), batch.RecallReason,
                                                   batch.Customer, batch.BatchAlias, batch.VFRUploadeDate.ToString(), batch.InitialNumberOfDocuments.ToString(), batch.InitialNumberOfPages.ToString()};
                    //string[] row = new string[] { "False", batch.BatchNumber, batch.LotNumber.ToString(), batch.StatusFlag, batch.KodakStatus, batch.JobType.Trim(), batch.NumberOfPages.ToString(),
                    //                               batch.NumberOfDocuments.ToString(), batch.SubmittedBy, batch.SubmittedDate.ToString(), batch.ScanOperator, batch.ScanStation, batch.ScannedDate.ToString(),
                    //                               batch.QCBy, batch.QCStation, batch.QCDate.ToString(), batch.OutputBy, batch.OutputStation, batch.OutputDate.ToString(),
                    //                               batch.ApprovedBy, batch.ApprovedDate.ToString(),batch.RejectedBy, batch.LastTimeRejected.ToString(),batch.RejectedTimes.ToString(),
                    //                               batch.ExportedBy,batch.ExportedDate.ToString(), batch.Comments, batch.BlockNumber.ToString(),batch.ProjectName, batch.DepName, batch.KodakErrorState,
                    //                               batch.PrepUserName, batch.PrepDate.ToString(), batch.ScanningTime.ToString(), batch.ScanningStageTime.ToString(),batch.QCStartTime.ToString(), batch.QCEndTime.ToString(),
                    //                               batch.QCTime.ToString(), batch.QCStageTime.ToString(),batch.RecallBy, batch.RecallDate.ToString(),batch.RecallTimes.ToString(), batch.RecallReason,
                    //                               batch.Customer, batch.BatchAlias, batch.VFRUploadeDate.ToString(), batch.InitialNumberOfDocuments.ToString(), batch.InitialNumberOfPages.ToString()};

                    BatchList.Rows.Add(row);


                    
                }
                
                // Disable Check Boxes 
                foreach (DataGridViewRow row in BatchList.Rows)
                {
                    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                }

                // Set options based on Operation Moded: Delete, Approve, Reject, etc.
                foreach (DataGridViewRow row in BatchList.Rows)
                {
                    if (ApprovalSelectionRadioButton.Checked)
                    {
                        ApprovalSelection();
                    }

                    if (DeletionSelectionRadioButton.Checked)
                    {
                        DeletionSelection();
                    }

                    if (RejectionSelectionRadioButton.Checked)
                    {
                        row.Cells["ApprovalCheckBox"].ReadOnly = true;
                    }
                }
                BatchList.ClearSelection();
                //AccesstoUIFunctinality();

            }
            //CircularProgressBar.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        public static Boolean functionalityAllowed(string userName, string action)
        {
            Boolean allow = false;
            // Get User Information to control Accesst to Functionality
            ResultUsers resultUsers = new ResultUsers();
            List<User> users = new List<User>();
            resultUsers = DBTransactions.GetUserByName(Environment.UserName);
            users = resultUsers.ReturnValue;

            nlogger.Trace("Validating " + userName + "  Functionality ..");
            nlogger.Trace("     Record: " + JsonConvert.SerializeObject(users[0].UIFunctionality, Formatting.Indented));

            //MessageBox.Show("Checking user: " + userName + " for Action: " + action, "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            foreach (UIFunctionality functionality in users[0].UIFunctionality)
            {
                //MessageBox.Show("Comparng: " + action.Trim().ToUpper() + " Vs: " + functionality.Description.Trim().ToUpper(), "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (action.Trim().ToUpper() == functionality.Description.Trim().ToUpper())
                {
                    allow = true;
                    break;
                }               
            }
            return allow;
        }


        public void AccesstoUIFunctinality()
        {
            QualityControlButton.Enabled = false;
            StatusChangeButton.Enabled = false;
            BatchMetadtaButton.Enabled = false;
            ApproveSelectedButton.Enabled = false;
            RejectSelectedButton.Enabled = false;
            DeleteSelectedButton.Enabled = false;
            CPScanDirectoryButton.Enabled = false;
            CPInfoFileButton.Enabled = false;
            RefeedToVFRButton.Enabled = false;
            DeleteFromVFRButton.Enabled = false;

            // Get User Information to control Accesst to Functionality
            ResultUsers resultUsers = new ResultUsers();
            List<User> users = new List<User>();
            resultUsers = DBTransactions.GetUserByName(Environment.UserName);
            users = resultUsers.ReturnValue;
            SelectAllButton.Enabled = false;
            UnselectAllButton.Enabled = false;

            foreach (UIFunctionality functionality in users[0].UIFunctionality)
            {
                switch (functionality.Description)
                {
                    case "Quality Control":
                        QualityControlButton.Enabled = true;
                        break;

                    case "Batch Administration":
                        BatchMetadtaButton.Enabled = true;
                        //ApproveSelectedButton.Enabled = true;
                        //RejectSelectedButton.Enabled = true;
                        //DeleteSelectedButton.Enabled = true;
                        CPScanDirectoryButton.Enabled = true;
                        CPInfoFileButton.Enabled = true;
                        RefeedToVFRButton.Enabled = true;
                        DeleteFromVFRButton.Enabled = true;
                        break;

                    case "VFR Search":
                        //VFRSearchButton.Enabled = true;
                        break;

                    case "Batch Registration":
                        //BatchRegistrationButton.Enabled = true;
                        break;

                    case "Batch Approval":
                        if (ApprovalSelectionRadioButton.Checked)
                        {
                            SelectAllButton.Enabled = true;
                            UnselectAllButton.Enabled = true;
                            ApproveSelectedButton.Enabled = true;
                        }                        
                        break;

                    case "Batch Rejection":
                        if (RejectionSelectionRadioButton.Checked)
                        {
                            SelectAllButton.Enabled = true;
                            UnselectAllButton.Enabled = true;
                            RejectSelectedButton.Enabled = true;
                        }
                        
                        break;

                    case "Batch Removal":
                        if (DeletionSelectionRadioButton.Checked)
                        {
                            SelectAllButton.Enabled = true;
                            UnselectAllButton.Enabled = true;
                            DeleteSelectedButton.Enabled = true;
                        }
                        break;
                }
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BatchControlCenterForm_Load(object sender, EventArgs e)
        {
            // Load Customers combobox
            ResultCustomers resulCustomers = new ResultCustomers();
            resulCustomers = DBTransactions.GetCustomers();
            CustomersComboBox.Items.Add("");
            if (resulCustomers.RecordsCount > 0)
            {
                foreach (Customer customer in resulCustomers.ReturnValue)
                {
                    CustomersComboBox.Items.Add(customer.CustomerName);
                }
            }

            // Load Jobs combobox
            ResultJobsExtended resultJobs = new ResultJobsExtended();
            resultJobs = DBTransactions.GetJobs();
            JobTypeComboBox.Items.Add("");
            if (resultJobs.RecordsCount > 0)
            {
                foreach (JobExtended job in resultJobs.ReturnValue)
                {
                    JobTypeComboBox.Items.Add(job.JobName);
                }
            }

            // Load User List View
            ResultUsers resultUsers = new ResultUsers();
            List<User> users = new List<User>();
            resultUsers = DBTransactions.GetUsers();
            ScannedByComboBox.Items.Add("");
            RejectedByComboBox.Items.Add("");
            ApprovedByComboBox.Items.Add("");
            ExportedByComboBox.Items.Add("");
            OutputByComboBox.Items.Add("");
            QCByComboBox.Items.Add("");
            if (resultUsers.RecordsCount > 0)
            {
                foreach (User user in resultUsers.ReturnValue)
                {
                    ScannedByComboBox.Items.Add(user.UserName);
                    RejectedByComboBox.Items.Add(user.UserName);
                    ApprovedByComboBox.Items.Add(user.UserName);
                    ExportedByComboBox.Items.Add(user.UserName);
                    OutputByComboBox.Items.Add(user.UserName);
                    QCByComboBox.Items.Add(user.UserName);
                }
            }

        }

        private void JobTypeCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (JobTypeCheckBox.Checked)
                    BatchList.Columns["JobType"].Visible = true;
                
                else               
                    BatchList.Columns["JobType"].Visible = false;               
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DepartmentNameCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (DepartmentNameCheckBox.Checked)
                    BatchList.Columns["DepartmentName"].Visible = true;

                else
                    BatchList.Columns["DepartmentName"].Visible = false;
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void SubmittedDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (SubmittedDateCheckBox.Checked)
                {
                    SubmittedDateTimePickerFrom.Enabled = true;
                    SubmittedDateTimePickerTo.Enabled = true;
                }

                else
                {
                    SubmittedDateTimePickerFrom.Enabled = false;
                    SubmittedDateTimePickerTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }          
        }

        private void BatchAliasNameCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {                
                if (BatchAliasNameCheckBox.Checked)
                    BatchList.Columns["BatchAliasName"].Visible = true;

                else
                    BatchList.Columns["BatchAliasName"].Visible = false;
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CustomerCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (CustomerCheckBox.Checked)
                    BatchList.Columns["Customer"].Visible = true;

                else
                    BatchList.Columns["Customer"].Visible = false;
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void ProjectNameCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ProjectNameCheckBox.Checked)
                    BatchList.Columns["ProjectName"].Visible = true;

                else
                    BatchList.Columns["ProjectName"].Visible = false;
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BlockNumberCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (BlockNumberCheckBox.Checked)
                    BatchList.Columns["BlockNumber"].Visible = true;

                else
                    BatchList.Columns["BlockNumber"].Visible = false;
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WOCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (WOCheckBox.Checked)
                    BatchList.Columns["LotNumber"].Visible = true;

                else
                    BatchList.Columns["LotNumber"].Visible = false;
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckAllButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            BlockNumberCheckBox.Checked = true;
            ExportedDateCheckBox1.Checked = true;
            ExportedByCheckBox.Checked = true;
            RejectedTimesCheckBox.Checked = true;
            RejectedDateCheckBox.Checked = true;
            RejectedByCheckBox.Checked = true;
            ApprovedDateCheckBox1.Checked = true;
            ApprovedByCheckBox.Checked = true;
            OutputDateCheckBox1.Checked = true;
            OutputStationCheckBox.Checked = true;
            OutputByCheckBox.Checked = true;
            QCStationCheckBox.Checked = true;
            QCByCheckBox.Checked = true;
            ScannedDateCheckBox1.Checked = true;
            ScanStationCheckBox.Checked = true;
            ScannedByCheckBox.Checked = true;
            SumittedDateCheckBox.Checked = true;
            QCDateCheckBox1.Checked = true;
            SubmittedByCheckBox.Checked = true;
            NumberOfDocumentsCheckBox.Checked = true;
            JobTypeCheckBox.Checked = true; ;
            KodakStatusCheckBox.Checked = true;
            WOCheckBox.Checked = true;
            IndexingStatusCheckBox.Checked = true;
            NumberOfImagesCheckBox.Checked = true;
            CommentsCheckBox.Checked = true;
            DepartmentNameCheckBox.Checked = true;
            KodakErrorCheckBox.Checked = true;
            ProjectNameCheckBox.Checked = true;
            PrepDateCheckBox.Checked = true;
            QCTimeCheckBox.Checked = true;
            KeystrokesCheckBox.Checked = true;
            PrepByCheckBox.Checked = true;
            RecallTimesCheckBox.Checked = true;
            RecallByCheckBox.Checked = true;
            RecallDateCheckBox.Checked = true;
            RecallCommentsCheckBox.Checked = true;
            CustomerCheckBox.Checked = true;
            VFRUploadDateCheckBox.Checked = true; ;
            BatchAliasNameCheckBox.Checked = true;
            InitialNumberOfDocumentsCheckBox.Checked = true;
            InitialNumberOfPagesCheckBox.Checked = true;

            Cursor.Current = Cursors.Default;
        }

        private void UnCheckAllButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            BlockNumberCheckBox.Checked = false;
            ExportedDateCheckBox1.Checked = false;
            ExportedByCheckBox.Checked = false;
            RejectedTimesCheckBox.Checked = false;
            RejectedDateCheckBox.Checked = false;
            RejectedByCheckBox.Checked = false;
            ApprovedDateCheckBox1.Checked = false;
            ApprovedByCheckBox.Checked = false;
            OutputDateCheckBox1.Checked = false;
            OutputStationCheckBox.Checked = false;
            OutputByCheckBox.Checked = false;
            QCStationCheckBox.Checked = false;
            QCByCheckBox.Checked = false;
            ScannedDateCheckBox1.Checked = false;
            ScanStationCheckBox.Checked = false;
            ScannedByCheckBox.Checked = false; 
            SumittedDateCheckBox.Checked = false;
            QCDateCheckBox1.Checked = false;
            SubmittedByCheckBox.Checked = false;
            NumberOfDocumentsCheckBox.Checked = false;
            JobTypeCheckBox.Checked = false;
            KodakStatusCheckBox.Checked = false;
            WOCheckBox.Checked = false;
            IndexingStatusCheckBox.Checked = false;
            NumberOfImagesCheckBox.Checked = false;
            CommentsCheckBox.Checked = false;
            DepartmentNameCheckBox.Checked = false;
            KodakErrorCheckBox.Checked = false;
            ProjectNameCheckBox.Checked = false;
            PrepDateCheckBox.Checked = false;
            QCTimeCheckBox.Checked = false;
            KeystrokesCheckBox.Checked = false;
            PrepByCheckBox.Checked = false;
            RecallTimesCheckBox.Checked = false;
            RecallByCheckBox.Checked = false;
            RecallDateCheckBox.Checked = false;
            RecallCommentsCheckBox.Checked = false;
            CustomerCheckBox.Checked = false;
            VFRUploadDateCheckBox.Checked = false;
            BatchAliasNameCheckBox.Checked = false;
            InitialNumberOfDocumentsCheckBox.Checked = false;
            InitialNumberOfPagesCheckBox.Checked = false;

            Cursor.Current = Cursors.Default;
        }

        private void PrepByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (PrepByCheckBox.Checked)
                    BatchList.Columns["PrepBy"].Visible = true;

                else
                    BatchList.Columns["PrepBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void SubmittedByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (SubmittedByCheckBox.Checked)
                    BatchList.Columns["SubmittedBy"].Visible = true;

                else
                    BatchList.Columns["SubmittedBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void ScannedByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ScannedByCheckBox.Checked)
                    BatchList.Columns["ScanOperator"].Visible = true;

                else
                    BatchList.Columns["ScanOperator"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void QCByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (QCByCheckBox.Checked)
                    BatchList.Columns["QCBy"].Visible = true;

                else
                    BatchList.Columns["QCBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void OutputByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (OutputByCheckBox.Checked)
                    BatchList.Columns["OutputBy"].Visible = true;

                else
                    BatchList.Columns["OutputBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void PrepDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (PrepDateCheckBox.Checked)
                    BatchList.Columns["PrepDate"].Visible = true;

                else
                    BatchList.Columns["PrepDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void SumittedDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (SumittedDateCheckBox.Checked)
                    BatchList.Columns["SubmittedDate"].Visible = true;

                else
                    BatchList.Columns["SubmittedDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void ScannedDateCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ScannedDateCheckBox1.Checked)
                    BatchList.Columns["ScannedDate"].Visible = true;

                else
                    BatchList.Columns["ScannedDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        //private void ScanningTimeCheckBox_CheckStateChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ScanningTimeCheckBox.Checked)
        //        {
        //            BatchList.Columns["ScanningTime"].Visible = true;
        //            BatchList.Columns["ScanningStageTime"].Visible = true;
        //        }
        //        else
        //        {
        //            BatchList.Columns["ScanningTime"].Visible = false;
        //            BatchList.Columns["ScanningStageTime"].Visible = false;
        //        }                   
        //    }
        //    catch (Exception ex)
        //    {
        //        General.ErrorMessage(ex);

        //    }
        //}

        private void QCDateCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (QCDateCheckBox1.Checked)
                    BatchList.Columns["QCDate"].Visible = true;

                else
                    BatchList.Columns["QCDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void QCTimeCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (QCTimeCheckBox.Checked)
                {
                    BatchList.Columns["QCTime"].Visible = true;
                    BatchList.Columns["QCStartTime"].Visible = true;
                    BatchList.Columns["QCEndTime"].Visible = true;
                    BatchList.Columns["QCStageTime"].Visible = true;
                }
                else
                {
                    BatchList.Columns["QCTime"].Visible = false;
                    BatchList.Columns["QCStartTime"].Visible = false;
                    BatchList.Columns["QCEndTime"].Visible = false;
                    BatchList.Columns["QCStageTime"].Visible = false;
                }                   
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);

            }
        }

        private void OutputDateCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (OutputDateCheckBox1.Checked)
                    BatchList.Columns["OutputDate"].Visible = true;

                else
                    BatchList.Columns["OutputDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void ScanStationCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ScanStationCheckBox.Checked)
                    BatchList.Columns["ScanStation"].Visible = true;

                else
                    BatchList.Columns["ScanStation"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void QCStationCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (QCStationCheckBox.Checked)
                    BatchList.Columns["QCStation"].Visible = true;

                else
                    BatchList.Columns["QCStation"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void OutputStationCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (OutputStationCheckBox.Checked)
                    BatchList.Columns["OutputStation"].Visible = true;

                else
                    BatchList.Columns["OutputStation"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RecallByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (RecallByCheckBox.Checked)
                    BatchList.Columns["RecallBy"].Visible = true;

                else
                    BatchList.Columns["RecallBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RejectedByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (RejectedByCheckBox.Checked)
                    BatchList.Columns["RejectedBy"].Visible = true;

                else
                    BatchList.Columns["RejectedBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void ApprovedByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ApprovedByCheckBox.Checked)
                    BatchList.Columns["ApprovedBy"].Visible = true;

                else
                    BatchList.Columns["ApprovedBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void ExportedByCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ExportedByCheckBox.Checked)
                    BatchList.Columns["ExportedBy"].Visible = true;

                else
                    BatchList.Columns["ExportedBy"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RecallDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (RecallDateCheckBox.Checked)
                    BatchList.Columns["RecallDate"].Visible = true;

                else
                    BatchList.Columns["RecallDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RecallCommentsCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (RecallCommentsCheckBox.Checked)
                    BatchList.Columns["RecallComments"].Visible = true;

                else
                    BatchList.Columns["RecallComments"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RejectedDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (RejectedDateCheckBox.Checked)
                    BatchList.Columns["RejectedDate"].Visible = true;

                else
                    BatchList.Columns["RejectedDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RejectedTimesCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (RejectedTimesCheckBox.Checked)
                    BatchList.Columns["RejectedTimes"].Visible = true;

                else
                    BatchList.Columns["RejectedTimes"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void ApprovedDateCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ApprovedDateCheckBox1.Checked)
                    BatchList.Columns["ApprovedDate"].Visible = true;

                else
                    BatchList.Columns["ApprovedDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void ExportedDateCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ExportedDateCheckBox1.Checked)
                    BatchList.Columns["ExportedDate"].Visible = true;

                else
                    BatchList.Columns["ExportedDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RecallTimesCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (RecallTimesCheckBox.Checked)
                    BatchList.Columns["RecallTimes"].Visible = true;

                else
                    BatchList.Columns["RecallTimes"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void IndexingStatusCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (IndexingStatusCheckBox.Checked)
                    BatchList.Columns["Status"].Visible = true;

                else
                    BatchList.Columns["Status"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void CommentsCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (CommentsCheckBox.Checked)
                    BatchList.Columns["Comments"].Visible = true;

                else
                    BatchList.Columns["Comments"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void VFRUploadDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (VFRUploadDateCheckBox.Checked)
                    BatchList.Columns["VFRUploadDate"].Visible = true;

                else
                    BatchList.Columns["VFRUploadDate"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void KodakStatusCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (KodakStatusCheckBox.Checked)
                    BatchList.Columns["KodakStatus"].Visible = true;

                else
                    BatchList.Columns["KodakStatus"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void KodakErrorCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (KodakErrorCheckBox.Checked)
                    BatchList.Columns["KodakError"].Visible = true;

                else
                    BatchList.Columns["KodakError"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void NumberOfDocumentsCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (NumberOfDocumentsCheckBox.Checked)
                    BatchList.Columns["TotalDocuments"].Visible = true;

                else
                    BatchList.Columns["TotalDocuments"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void InitialNumberOfDocumentsCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (InitialNumberOfDocumentsCheckBox.Checked)
                    BatchList.Columns["TotalInitialDocuments"].Visible = true;

                else
                    BatchList.Columns["TotalInitialDocuments"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void NumberOfImagesCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (NumberOfImagesCheckBox.Checked)
                    BatchList.Columns["TotalPages"].Visible = true;

                else
                    BatchList.Columns["TotalPages"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void InitialNumberOfPagesCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (InitialNumberOfPagesCheckBox.Checked)
                    BatchList.Columns["TotalInitialPages"].Visible = true;

                else
                    BatchList.Columns["TotalInitialPages"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void KeystrokesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (KeystrokesCheckBox.Checked)
                    BatchList.Columns["Keystrokes"].Visible = true;

                else
                    BatchList.Columns["Keystrokes"].Visible = false;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void QualityControlButton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            QualityControlForm _QualityControlForm = new QualityControlForm();
            _QualityControlForm.StartPosition = FormStartPosition.CenterScreen;
            _QualityControlForm.Show();
            Data.GlovalVariables.currentBatchName = "";
        }

        private void StatusChangeButton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            StatusChangeForm _StatusChangeForm = new StatusChangeForm();
            _StatusChangeForm.StartPosition = FormStartPosition.CenterScreen;
            _StatusChangeForm.Show();
            Data.GlovalVariables.currentBatchName = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();

            Data.GlovalVariables.currentBatchName = "";
        }

        private void BatchInfobutton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            MessageBox.Show("This Option has not been implemented yet !!!!", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Data.GlovalVariables.currentBatchName = "";
        }

        private void CPScanDirectoryButton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            ResultJobsExtended resultJobsExtended = new ResultJobsExtended();
            ResultBatches resultBatches = new ResultBatches();
            Batch batch = new Batch();
            DialogResult result;
            string captureBatchDirectory = "";

            try
            {
                resultBatches = DBTransactions.GetBatchesInformation("BatchNumber = \"" + Data.GlovalVariables.currentBatchName + "\"", ""); // OR BatchAlias = \"" + BatchNumber.Text + "\"", "");
                if (resultBatches.RecordsCount == 0)
                {
                    nlogger.Trace("         Batch Name " + Data.GlovalVariables.currentBatchName + " entered could not be found");
                    result = MessageBox.Show(this, "The Batch Name entered could not be found.", "Info ....", MessageBoxButtons.OK,
                                                  MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    batch = resultBatches.ReturnValue[0];
                    // Get Batch Directory    
                    resultJobsExtended = DBTransactions.GetJobByName(batch.JobType);
                    foreach (JobExtended job in resultJobsExtended.ReturnValue)
                    {
                        if (job.JobName.ToUpper() == batch.JobType.ToUpper())
                        {
                            captureBatchDirectory = job.ScanningFolder + "\\" + job.JobName + "\\" + batch.BatchNumber;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(captureBatchDirectory))
                    {
                        nlogger.Trace("         Cannot find Batch Job Type " + batch.JobType + " in the Database");
                        result = MessageBox.Show(this, "Cannot find Batch Job Type " + batch.JobType + " in the Database", "Info ....", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else
                    {
                        if (System.IO.Directory.Exists(captureBatchDirectory))
                        {
                            // Open CP Directory
                            OpenFolder(captureBatchDirectory);
                        }
                        else
                        {
                            nlogger.Trace("         Cannot find Batch Directory :" + captureBatchDirectory);
                            result = MessageBox.Show(this, "Cannot find Batch Directory :" + captureBatchDirectory, "Info ....", MessageBoxButtons.OK,
                                                          MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //General.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Data.GlovalVariables.currentBatchName = "";
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

        private void CPBatchLogButton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            ResultJobsExtended resultJobsExtended = new ResultJobsExtended();
            ResultBatches resultBatches = new ResultBatches();
            Batch batch = new Batch();
            DialogResult result;
            string captureBatchDirectory = "";

            try
            {
                resultBatches = DBTransactions.GetBatchesInformation("BatchNumber = \"" + Data.GlovalVariables.currentBatchName + "\"", ""); // OR BatchAlias = \"" + BatchNumber.Text + "\"", "");
                if (resultBatches.RecordsCount == 0)
                {
                    nlogger.Trace("         Batch Name " + Data.GlovalVariables.currentBatchName + " entered could not be found");
                    result = MessageBox.Show(this, "The Batch Name entered could not be found.", "Info ....", MessageBoxButtons.OK,
                                                  MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    batch = resultBatches.ReturnValue[0];
                    // Get Batch Directory    
                    resultJobsExtended = DBTransactions.GetJobByName(batch.JobType);
                    foreach (JobExtended job in resultJobsExtended.ReturnValue)
                    {
                        if (job.JobName.ToUpper() == batch.JobType.ToUpper())
                        {
                            captureBatchDirectory = job.ScanningFolder + "\\" + job.JobName + "\\" + batch.BatchNumber;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(captureBatchDirectory))
                    {
                        nlogger.Trace("         Cannot find Batch Job Type " + batch.JobType + " in the Database");
                        result = MessageBox.Show(this, "Cannot find Batch Job Type " + batch.JobType + " in the Database", "Info ....", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else
                    {
                        if (System.IO.Directory.Exists(captureBatchDirectory))
                        {
                            if (System.IO.File.Exists(captureBatchDirectory + "\\Log"))
                            {
                                // Open CP Directory
                                System.Diagnostics.Process.Start(@captureBatchDirectory + "\\Log");
                            }
                            else
                            {
                                nlogger.Trace("         Cannot find Log File for Batch: " + Data.GlovalVariables.currentBatchName);
                                result = MessageBox.Show(this, "Cannot find Log File for Batch: " + Data.GlovalVariables.currentBatchName, "Info ....", MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else
                        {
                            nlogger.Trace("         Cannot find Batch Directory :" + captureBatchDirectory);
                            result = MessageBox.Show(this, "Cannot find Batch Directory :" + captureBatchDirectory, "Info ....", MessageBoxButtons.OK,
                                                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //General.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Data.GlovalVariables.currentBatchName = "";
        }

        private void CPInfoFileButton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            ResultJobsExtended resultJobsExtended = new ResultJobsExtended();
            ResultBatches resultBatches = new ResultBatches();
            Batch batch = new Batch();
            DialogResult result;
            string captureBatchDirectory = "";

            try
            {
                resultBatches = DBTransactions.GetBatchesInformation("BatchNumber = \"" + Data.GlovalVariables.currentBatchName + "\"", ""); // OR BatchAlias = \"" + BatchNumber.Text + "\"", "");
                if (resultBatches.RecordsCount == 0)
                {
                    nlogger.Trace("         Batch Name " + Data.GlovalVariables.currentBatchName + " entered could not be found");
                    result = MessageBox.Show(this, "The Batch Name entered could not be found.", "Info ....", MessageBoxButtons.OK,
                                                  MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    batch = resultBatches.ReturnValue[0];
                    // Get Batch Directory    
                    resultJobsExtended = DBTransactions.GetJobByName(batch.JobType);
                    foreach (JobExtended job in resultJobsExtended.ReturnValue)
                    {
                        if (job.JobName.ToUpper() == batch.JobType.ToUpper())
                        {
                            captureBatchDirectory = job.ScanningFolder + "\\" + job.JobName + "\\" + batch.BatchNumber;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(captureBatchDirectory))
                    {
                        nlogger.Trace("         Cannot find Batch Job Type " + batch.JobType + " in the Database");
                        result = MessageBox.Show(this, "Cannot find Batch Job Type " + batch.JobType + " in the Database", "Info ....", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else
                    {
                        if (System.IO.Directory.Exists(captureBatchDirectory))
                        {
                            if (System.IO.File.Exists(captureBatchDirectory + "\\info"))
                            {
                                // Open CP Directory
                                System.Diagnostics.Process.Start(@captureBatchDirectory + "\\info");
                            }
                            else
                            {
                                if (System.IO.File.Exists(captureBatchDirectory + "\\info.lock"))
                                {
                                    nlogger.Trace("         Batch Info File is currently Locked for Batch: " + Data.GlovalVariables.currentBatchName);
                                    result = MessageBox.Show(this, "Batch Info File is currently Locked for Batch: " + Data.GlovalVariables.currentBatchName, "Info ....", MessageBoxButtons.OK,
                                                     MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                }
                                else
                                {
                                    nlogger.Trace("         Cannot find CP Info File for Batch: " + Data.GlovalVariables.currentBatchName);
                                    result = MessageBox.Show(this, "Cannot find CP Info File for Batch: " + Data.GlovalVariables.currentBatchName, "Info ....", MessageBoxButtons.OK,
                                                     MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                }                                    
                            }                                
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //General.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Data.GlovalVariables.currentBatchName = "";
        }

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            // BatchList.CurrentRow.Index[0];
            foreach (DataGridViewRow row in BatchList.Rows)
            {
                if (ApprovalSelectionRadioButton.Checked)
                {
                    if (!row.Cells["ApprovalCheckBox"].ReadOnly)
                    {
                        row.Cells["ApprovalCheckBox"].Value = true;
                    }
                }

                if (DeletionSelectionRadioButton.Checked)
                {
                    if (!row.Cells["ApprovalCheckBox"].ReadOnly)
                    {
                        row.Cells["ApprovalCheckBox"].Value = true;
                    }
                }
            }
            BatchList.Refresh();
        }

        private void BatchList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void BatchList_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This event is necessary to solved the issue of check boxes no working properly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ////AccesstoUIFunctinality();
           
            //if (functionalityAllowed(Environment.UserName, "Quality Control"))
            //{
            //    QualityControlButton.Enabled = true;
            //}
            //else
            //{
            //    if (QualityControlButton.Enabled) QualityControlButton.Enabled = false;
            //}

            //if (functionalityAllowed(Environment.UserName, "Batch Administration"))
            //{
            //    StatusChangeButton.Enabled = true;
            //    BatchMetadtaButton.Enabled = true;
            //    CPScanDirectoryButton.Enabled = true;
            //    CPInfoFileButton.Enabled = true;
            //    RefeedToVFRButton.Enabled = true;
            //    DeleteFromVFRButton.Enabled = true;
            //}
            //else
            //{
            //    // to avoid flickering of the Button in the windows form
            //    if (StatusChangeButton.Enabled) StatusChangeButton.Enabled = false;
            //    if (BatchMetadtaButton.Enabled) BatchMetadtaButton.Enabled = false;
            //    if (CPScanDirectoryButton.Enabled) CPScanDirectoryButton.Enabled = false;
            //    if (CPInfoFileButton.Enabled) CPInfoFileButton.Enabled = false;
            //    if (RefeedToVFRButton.Enabled) RefeedToVFRButton.Enabled = false;
            //    if (DeleteFromVFRButton.Enabled) DeleteFromVFRButton.Enabled = false;
            //}

            //BatchList.EndEdit();
            //BatchList.Refresh();
        }
        
        private void UnselectAllButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in BatchList.Rows)
            {
                row.Cells["ApprovalCheckBox"].Value = false;
            }
            BatchList.Refresh();
        }

        /// <summary>
        /// This method need to differentiate between Batches with Work Order = 0 and other than 0
        /// Batches with work order grather than 0, means tha this batch was exported and later rejected.
        /// For Batches that has WO grather than 0, the approval action will set the Status Flag to "Exorted"
        /// For Batches that has WO equal to 0, this methos is responsable for looking at the next available WO
        /// to be assigned to a Batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApproveSelectedButton_Click(object sender, EventArgs e)
        {
            ResultBatches resultBatches = new ResultBatches();
            ResultGeneric result = new ResultGeneric();
            Batch batch = new Batch();
            foreach (DataGridViewRow row in BatchList.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ApprovalCheckBox"].Value))
                {
                    resultBatches = DBTransactions.GetBatchesInformation("BatchNumber = \"" + row.Cells["BatchNumber"].Value + "\"", "");
                    batch = resultBatches.ReturnValue[0];
                    batch.ApprovedBy = Environment.UserName;
                    batch.ApprovedDate = DateTime.Now;
                    if (row.Cells["LotNumber"].Value.ToString() == "0")
                    {
                        // Set new Status
                        batch.StatusFlag = "Approved";                         
                        result = DBTransactions.BatchUpdate(batch);
                        row.Cells["LotNumber"].Value = result.IntegerNumberReturnValue;
                        row.Cells["Status"].Value = "Approved";

                        // Record Batch Event in Tracking Database Table
                        DBTransactions.BatchTrackingEvent(row.Cells["BatchNumber"].Value.ToString(), "Waiting for Approval", "Approved", "Batch Approved", Environment.UserName);
                    }
                    else
                    {
                        if (batch.ExportedTimes > 0)
                        {
                            // Set new Status
                            batch.StatusFlag = "Exported";
                            result = DBTransactions.BatchUpdate(batch);
                            row.Cells["Status"].Value = "Exported";
                            // Record Batch Event in Tracking Database Table
                            DBTransactions.BatchTrackingEvent(row.Cells["BatchNumber"].Value.ToString(), "Waiting for Approval", "Exported", "Batch Re-Approved (Previously Exported)", Environment.UserName);
                        }
                        else
                        {
                            // Set new Status
                            batch.StatusFlag = "Approved";
                            result = DBTransactions.BatchUpdate(batch);
                            row.Cells["Status"].Value = "Approved";
                            // Record Batch Event in Tracking Database Table
                            DBTransactions.BatchTrackingEvent(row.Cells["BatchNumber"].Value.ToString(), "Waiting for Approval", "Aproved", "Batch Re-Approved", Environment.UserName);
                        }
                    }
                    row.Cells["Status"].Style.ForeColor = Color.ForestGreen;
                    row.Cells["ApprovalCheckBox"].Value = false;
                    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                }
            }
            BatchList.Refresh();
        }

        private void ApprovalSelection()
        {
            //AccesstoUIFunctinality();
            UnselectAllButton.Enabled = false;
            SelectAllButton.Enabled = false;
            ApproveSelectedButton.Enabled = false;
            if (functionalityAllowed(Environment.UserName, "Batch Approval"))
            {
                ApproveSelectedButton.Enabled = true;
                UnselectAllButton.Enabled = true;
                SelectAllButton.Enabled = true;
            }            
            RejectSelectedButton.Enabled = false;
            DeleteSelectedButton.Enabled = false;
            RefeedToVFRButton.Enabled = false; ;
            DeleteFromVFRButton.Enabled = false;

            foreach (DataGridViewRow row in BatchList.Rows)
            {
                row.Cells["ApprovalCheckBox"].Value = false;
                if (row.Cells["Status"].Value.ToString() == "Waiting for Approval")
                {
                    row.Cells["ApprovalCheckBox"].ReadOnly = false;
                    row.DefaultCellStyle.BackColor = Color.Bisque;
                }
                else
                {
                    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
            BatchList.ClearSelection();
            BatchList.Refresh();
        }

        private void ApprovalSelectionRadioButton_Click(object sender, EventArgs e)
        {

            ApprovalSelection();
        }

        private void RejectionSelectionRadioButton_Click(object sender, EventArgs e)
        {
            //AccesstoUIFunctinality();        
            UnselectAllButton.Enabled = false;
            SelectAllButton.Enabled = false;
            RejectSelectedButton.Enabled = false;
            if (functionalityAllowed(Environment.UserName, "Batch Rejection"))
            {
                RejectSelectedButton.Enabled = true;
                UnselectAllButton.Enabled = true;
                SelectAllButton.Enabled = true;
            }
            ApproveSelectedButton.Enabled = false;
            DeleteSelectedButton.Enabled = false;
            RefeedToVFRButton.Enabled = false; ;
            DeleteFromVFRButton.Enabled = false;

            foreach (DataGridViewRow row in BatchList.Rows)
            {
                row.Cells["ApprovalCheckBox"].Value = false;
                if (row.Cells["Status"].Value.ToString() == "Waiting for Approval" || row.Cells["Status"].Value.ToString() == "Waiting for Validation" ||
                    row.Cells["Status"].Value.ToString() == "Waiting for PDF Conversion" || row.Cells["Status"].Value.ToString() == "QC on Hold" ||
                    row.Cells["Status"].Value.ToString() == "QC Failed")
                {
                    row.Cells["ApprovalCheckBox"].ReadOnly = false;
                    row.DefaultCellStyle.BackColor = Color.Bisque;
                }
                else
                {
                    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                    row.DefaultCellStyle.BackColor = Color.White;
                }
                //if (RejectionSelectionRadioButton.Checked)
                //{
                //    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                //}
            }
            BatchList.ClearSelection();
            BatchList.Refresh();
        }

        private void DeletionSelection()
        {
            //AccesstoUIFunctinality();     
            UnselectAllButton.Enabled = false ;
            SelectAllButton.Enabled = false;
            DeleteSelectedButton.Enabled = false;
            if (functionalityAllowed(Environment.UserName, "Batch Removal"))
            {
                DeleteSelectedButton.Enabled = true;
                UnselectAllButton.Enabled = true;
                SelectAllButton.Enabled = true;
            }
            ApproveSelectedButton.Enabled = false;
            RejectSelectedButton.Enabled = false;
            RefeedToVFRButton.Enabled = false; ;
            DeleteFromVFRButton.Enabled = false;

            foreach (DataGridViewRow row in BatchList.Rows)
            {
                row.Cells["ApprovalCheckBox"].Value = false;
                row.Cells["ApprovalCheckBox"].ReadOnly = false;
                row.DefaultCellStyle.BackColor = Color.Bisque;
            }
            BatchList.ClearSelection();
            BatchList.Refresh();
        }

        private void DeletionSelectionRadioButton_Click(object sender, EventArgs e)
        {
            DeletionSelection();
        }

        private void BatchList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {

        }

        private void VFRDeletionRadioButton_Click(object sender, EventArgs e)
        {
            //AccesstoUIFunctinality();
            ApproveSelectedButton.Enabled = false;
            RejectSelectedButton.Enabled = false;
            DeleteSelectedButton.Enabled = false;
            RefeedToVFRButton.Enabled = false; ;
            DeleteFromVFRButton.Enabled = true;

            foreach (DataGridViewRow row in BatchList.Rows)
            {
                row.Cells["ApprovalCheckBox"].ReadOnly = true;
                row.DefaultCellStyle.BackColor = Color.White;
                //row.Cells["ApprovalCheckBox"].Value = false;
                //if (VFRDeletionRadioButton.Checked)
                //{
                //    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                //}
            }
            BatchList.Refresh();
            
        }

        private void VFRRefeedRadioButton_Click(object sender, EventArgs e)
        {
            //AccesstoUIFunctinality();
            ApproveSelectedButton.Enabled = false;
            RejectSelectedButton.Enabled = false;
            DeleteSelectedButton.Enabled = false;
            RefeedToVFRButton.Enabled = true; ;
            DeleteFromVFRButton.Enabled = false;

            foreach (DataGridViewRow row in BatchList.Rows)
            {

                row.Cells["ApprovalCheckBox"].ReadOnly = true;
                row.DefaultCellStyle.BackColor = Color.White;

                //row.Cells["ApprovalCheckBox"].Value = false;
                //if (VFRRefeedRadioButton.Checked)
                //{
                //    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                //}
            }
            BatchList.Refresh();               
        }

        private void ScannedDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ScannedDateCheckBox.Checked)
                {
                    ScannedDateTimePickerFrom.Enabled = true;
                    ScannedDateTimePickerTo.Enabled = true;
                }

                else
                {
                    ScannedDateTimePickerFrom.Enabled = false;
                    ScannedDateTimePickerTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QCDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (QCDateCheckBox.Checked)
                {
                    QCDateTimePickerFrom.Enabled = true;
                    QCDateTimePickerTo.Enabled = true;
                }

                else
                {
                    QCDateTimePickerFrom.Enabled = false;
                    QCDateTimePickerTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OutputDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (OutputDateCheckBox.Checked)
                {
                    OutputDateTimePickerFrom.Enabled = true;
                    OutputDateTimePickerTo.Enabled = true;
                }

                else
                {
                    OutputDateTimePickerFrom.Enabled = false;
                    OutputDateTimePickerTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApprovedDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ApprovedDateCheckBox.Checked)
                {
                    ApprovedDateTimePickerFrom.Enabled = true;
                    ApprovedDateTimePickerTo.Enabled = true;
                }

                else
                {
                    ApprovedDateTimePickerFrom.Enabled = false;
                    ApprovedDateTimePickerTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportedDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (ExportedDateCheckBox.Checked)
                {
                    ExportedDateTimePickerFrom.Enabled = true;
                    ExportedDateTimePickerTo.Enabled = true;
                }

                else
                {
                    ExportedDateTimePickerFrom.Enabled = false;
                    ExportedDateTimePickerTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VFRUpdateDateCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (VFRUpdateDateCheckBox.Checked)
                {
                    VFRUpdateDateTimePickerFrom.Enabled = true;
                    VFRUpdateDateTimePickerTo.Enabled = true;
                }

                else
                {
                    VFRUpdateDateTimePickerFrom.Enabled = false;
                    VFRUpdateDateTimePickerTo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BatchTraceButton_click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            BatchTrackingForm _BatchTrackingForm = new BatchTrackingForm();
            _BatchTrackingForm.StartPosition = FormStartPosition.CenterScreen;
            _BatchTrackingForm.Show();
            Data.GlovalVariables.currentBatchName = "";
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            //Creating iTextSharp Table from the DataTable data

            List<DataGridViewColumn> listVisible = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn col in BatchList.Columns)
            {
                if (col.Visible && !string.IsNullOrEmpty(col.HeaderText))
                    listVisible.Add(col);
            }
            PdfPTable pdfTable = new PdfPTable(listVisible.Count);
            pdfTable.DefaultCell.Padding = 6;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            for (int i = 0; i < listVisible.Count; i++)
            { 
                PdfPCell cell = new PdfPCell(new Phrase(listVisible[i].HeaderText));
                pdfTable.AddCell(cell);
            }

            //Adding DataRow
            for (int i = 0; i < BatchList.Rows.Count - 1; i++)
            {
                for (int j = 0; j < listVisible.Count; j++)
                {
                    try
                    {
                        pdfTable.AddCell(BatchList.Rows[i].Cells[listVisible[j].Name].Value.ToString());
                    }
                    catch
                    {
                    }
                }
            }

            //Exporting to PDF
            string folderPath = Application.StartupPath + "\\PDFs\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!ScanningServicesAdmin.General.IsFileLocked(folderPath + "BatchViewReport.pdf"))
            {
                using (FileStream stream = new FileStream(folderPath + "BatchViewReport.pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.LEDGER, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
                if (!ScanningServicesAdmin.General.IsFileLocked(folderPath + "BatchViewReport.pdf"))
                {
                    System.Diagnostics.Process.Start(folderPath + "BatchViewReport.pdf");
                }
            }
            else
            {
                MessageBox.Show("The BatchViewReport.pdf is open by another application." + Environment.NewLine +
                                "You need to close the PDF viewer in order to generate the PDF File.", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void RejectSelectedButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Option has not been implemented yet !!!!" , "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void DeleteSelectedButton_Click(object sender, EventArgs e)
        {
            string originalBatchStatus = "";
            //MessageBox.Show("This Option has not been implemented yet !!!!", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            ResultBatches resultBatches = new ResultBatches();
            ResultGeneric result = new ResultGeneric();
            Batch batch = new Batch();
            foreach (DataGridViewRow row in BatchList.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ApprovalCheckBox"].Value))
                {
                    resultBatches = DBTransactions.GetBatchesInformation("BatchNumber = \"" + row.Cells["BatchNumber"].Value + "\"", "");
                    batch = resultBatches.ReturnValue[0];

                    // Set new Status
                    originalBatchStatus = batch.StatusFlag;
                    batch.StatusFlag = "Waiting for Deletion";
                    result = DBTransactions.BatchUpdate(batch);
                    row.Cells["Status"].Value = "Waiting for Deletion";

                    // Record Batch Event in Tracking Database Table
                    DBTransactions.BatchTrackingEvent(row.Cells["BatchNumber"].Value.ToString(),originalBatchStatus, batch.StatusFlag, "Batch tagged for deletion", Environment.UserName);
                                   
                    row.Cells["ApprovalCheckBox"].Value = false;
                    row.Cells["ApprovalCheckBox"].ReadOnly = true;
                }
            }
            BatchList.Refresh();
        }

        private void BatchMetadtaButton_Click(object sender, EventArgs e)
        {
            Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();
            Data.GlovalVariables.currentJobName = BatchList.CurrentRow.Cells["JobType"].Value.ToString();
            BatchDocumentViewerForm _BatchDocumentViewerForm = new BatchDocumentViewerForm();
            _BatchDocumentViewerForm.StartPosition = FormStartPosition.CenterScreen;
            _BatchDocumentViewerForm.Show();
            ////MessageBox.Show("This Option has not been implemented yet !!!!", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void RefeedToVFRButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Option has not been implemented yet !!!!", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void DeleteFromVFRButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Option has not been implemented yet !!!!", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void ResetSearchButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Option has not been implemented yet !!!!", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private void BatchList_Click_1(object sender, EventArgs e)
        {
            //AccesstoUIFunctinality();
            //MessageBox.Show("User : " + Environment.UserName, "Info ...", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (functionalityAllowed(Environment.UserName, "Quality Control"))
            {
                //MessageBox.Show("Access to Quality Control.", "Info ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                QualityControlButton.Enabled = true;
                if (BatchList.CurrentRow.Cells["Status"].Value.ToString() == "Exported" ||
                     BatchList.CurrentRow.Cells["Status"].Value.ToString() == "Approved" ||
                     BatchList.CurrentRow.Cells["Status"].Value.ToString() == "Waiting for Approval")
                {
                    BatchMetadtaButton.Enabled = true;
                }
                
            }
            else
            {
                //MessageBox.Show("NO Access to Quality Control.", "Info ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (QualityControlButton.Enabled) QualityControlButton.Enabled = false;
            }

            if (functionalityAllowed(Environment.UserName, "Batch Administration"))
            {
                StatusChangeButton.Enabled = true;
                CPScanDirectoryButton.Enabled = true;
                CPInfoFileButton.Enabled = true;
                RefeedToVFRButton.Enabled = true;
                DeleteFromVFRButton.Enabled = true;
                BatchTraceButton.Enabled = true;
            }
            else
            {
                // to avoid flickering of the Button in the windows form
                if (StatusChangeButton.Enabled) StatusChangeButton.Enabled = false;
                if (BatchMetadtaButton.Enabled) BatchMetadtaButton.Enabled = false;
                if (CPScanDirectoryButton.Enabled) CPScanDirectoryButton.Enabled = false;
                if (CPInfoFileButton.Enabled) CPInfoFileButton.Enabled = false;
                if (RefeedToVFRButton.Enabled) RefeedToVFRButton.Enabled = false;
                if (DeleteFromVFRButton.Enabled) DeleteFromVFRButton.Enabled = false;
                if (BatchTraceButton.Enabled) BatchTraceButton.Enabled = false;
            }
            if (!BatchList.CurrentRow.Cells["ApprovalCheckBox"].ReadOnly)
            {
                if (Convert.ToBoolean(BatchList.CurrentRow.Cells["ApprovalCheckBox"].Value))
                {
                    BatchList.CurrentRow.Cells["ApprovalCheckBox"].Value = false;
                }
                else
                {
                    BatchList.CurrentRow.Cells["ApprovalCheckBox"].Value = true;
                }
            }

            BatchList.EndEdit();
            BatchList.Refresh();
        }
    }
}

//LotNumber
//BlockNumber












