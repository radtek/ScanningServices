using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;

namespace ScanningServicesAdmin.Forms
{
    public partial class StatusChangeForm : Form
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public StatusChangeForm()
        {
            InitializeComponent();
        }

        private void StatusChangeForm_Load(object sender, EventArgs e)
        {

            LockedLabel.Visible = false;
            StationNameTextBox.Text = System.Net.Dns.GetHostName();
            OperatorTextBox.Text = Environment.UserName;

            nlogger.Trace("QC Operation Report - " + DateTime.Now);
            nlogger.Trace("     Status Change Operation");
            nlogger.Trace("         Operator ID: " + OperatorTextBox.Text);
            nlogger.Trace("         Station Name: " + StationNameTextBox.Text);

            DisableButtons();
            DisableOptions();
            
            if (!string.IsNullOrEmpty(Data.GlovalVariables.currentBatchName))
                BatchNumber.Text = Data.GlovalVariables.currentBatchName;
            
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ResultBatches resultBatches = new ResultBatches();
            ResultGeneric resultGeneric = new ResultGeneric();
            string originalBatchStatus = "";
            Batch batch = new Batch();
            DialogResult result = DialogResult.No;
            string action = "";
            DateTime currentDate = DateTime.Now;
            ResultUsers resultUsers = new ResultUsers();
            List<User> users = new List<User>();
            Boolean allowDeletion = false;
            Boolean allowRecall = false;
            Boolean allowRejection = false;

            try
            {
                // Get User Information to control Accesst to Functionality               
                resultUsers = DBTransactions.GetUserByName(Environment.UserName);
                users = resultUsers.ReturnValue;
                foreach (UIFunctionality functionality in users[0].UIFunctionality)
                {
                    switch (functionality.Description)
                    {
                        case "Batch  Rejection":
                            allowRejection = true;
                            break;

                        case "Batch  Recall":
                            allowRecall = true;
                            break;

                        case "Batch  Removal":
                            allowDeletion = true;
                            break;
                    }
                }

                nlogger.Trace("     QC Transaction (Apply Action) for Batch: " + BatchNameTextBox.Text);
                resultBatches = DBTransactions.GetBatchesInformation("BatchNumber = \"" + BatchNameTextBox.Text + "\"", ""); // OR BatchAlias = \"" + BatchNumber.Text + "\"", "");

                batch = resultBatches.ReturnValue[0];
                originalBatchStatus = batch.StatusFlag;

                if (RejectedRadioButton.Checked)
                    action = "Rejected";                    

                if (RecallRadioButton.Checked)
                    action = "Recall";

                if (DeleteRadioButton.Checked)
                    action = "Delete";

                switch (action)
                {
                    case "Rejected":
                        if (allowRejection)
                        {
                            result = MessageBox.Show(this, "Do you want to Reject this Batch : " + BatchNameTextBox.Text + " ?", "Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                batch.RejectedBy = OperatorTextBox.Text;
                                batch.RejectionReason = RejectCommentsTextBox.Text;
                                batch.LastTimeRejected = currentDate;
                                batch.RejectedTimes = batch.RejectedTimes + 1;
                                batch.StatusFlag = "Rejected";

                                // Update Batch Information
                                nlogger.Trace("     Updating Batch information in Database ...");
                                resultGeneric = DBTransactions.BatchUpdate(batch);
                                if (resultGeneric.ReturnCode == 0)
                                {
                                    // Batch Event Tracking
                                    nlogger.Trace("     Adding transaction in Tracking Database Table ...");
                                    DBTransactions.BatchTrackingEvent(batch.BatchNumber, originalBatchStatus, "Rejected", "Batch Rejected", OperatorTextBox.Text);
                                }
                            }
                        }
                        else
                        {
                            result = MessageBox.Show(this, "You are not auhorized to perform this operation.", "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        
                        break;

                    case "Recall":
                        if (allowRecall)
                        {
                            result = MessageBox.Show(this, "Do you want to request a Recall this Batch : " + BatchNameTextBox.Text + " ?", "Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                batch.RecallBy = OperatorTextBox.Text;
                                batch.RecallReason = RecallCommentsTextBox.Text;
                                batch.RecallDate = currentDate;
                                batch.RecallTimes = batch.RecallTimes + 1;
                                batch.StatusFlag = "Recall";

                                // Update Batch Information
                                nlogger.Trace("     Updating Batch information in Database ...");
                                resultGeneric = DBTransactions.BatchUpdate(batch);
                                if (resultGeneric.ReturnCode == 0)
                                {
                                    // Batch Event Tracking
                                    nlogger.Trace("     Adding transaction in Tracking Database Table ...");
                                    DBTransactions.BatchTrackingEvent(batch.BatchNumber, originalBatchStatus, "Recall", "Batch tagged for Recall", OperatorTextBox.Text);
                                }
                            }
                        }
                        else
                        {
                            result = MessageBox.Show(this, "You are not auhorized to perform this operation.", "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }                        
                        break;

                    case "Delete":
                        if (allowDeletion)
                        {
                            result = MessageBox.Show(this, "Do you want to Delete this Batch : " + BatchNameTextBox.Text + " ?", "Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                // Tag Batch for deletion
                                batch.StatusFlag = "Waiting for Deletion";
                                // Update Batch Information
                                nlogger.Trace("     Updating Batch information in Database ...");
                                resultGeneric = DBTransactions.BatchUpdate(batch);
                                if (resultGeneric.ReturnCode == 0)
                                {
                                    // Batch Event Tracking
                                    nlogger.Trace("     Adding transaction in Tracking Database Table ...");
                                    DBTransactions.BatchTrackingEvent(batch.BatchNumber, originalBatchStatus, "Waiting for Deletion", "Batch tagged for Deletion", OperatorTextBox.Text);
                                }
                                MessageBox.Show(this, "This operation is under Construction", "Alert...", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                            }
                        }
                        else
                        {
                            result = MessageBox.Show(this, "You are not auhorized to perform this operation.", "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }                        
                        break;
                }
                ResetValues();
                DisableButtons();
                DisableOptions();
                ExitButton.Enabled = true;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void RemoveBatch(string batchName)
        {
            // Remove Batch from Resting Location if necessary

            // Remove Batch from Otput Folder if necessary

            // Remove Batch from Bakup directory if necessary

            // Remove Batch from Capture directory

            // Remove Batch Documents from Doc Control Database Table

            // Remove Batch from Batch Control Database Table
        }

        private void CheckBatchNumber()
        {
            string batchName = "";
            ResultBatches resultBatches = new ResultBatches();
            ResultJobsExtended resultJobsExtended = new ResultJobsExtended();
            Batch batch = new Batch();
            DialogResult result;
            string originalBatchStatus = "";

            try
            {
                nlogger.Trace("         Operator entered the following Batch Name: " + BatchNumber.Text);

                if (BatchNumber.Text.Length > 0)
                {
                    resultBatches = DBTransactions.GetBatchesInformation("BatchNUmber = \"" + BatchNumber.Text + "\" OR BatchAlias = \"" + BatchNumber.Text + "\"", "");
                    if (resultBatches.RecordsCount == 0)
                    {
                        // Batch not found
                        nlogger.Trace("         Batch Name " + BatchNumber.Text + " entered could not be found");
                        result = MessageBox.Show(this, "The Batch Name entered could not be found.", "Info ....", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        // Batch Found
                        batch = resultBatches.ReturnValue[0];

                        resultJobsExtended = DBTransactions.GetJobByName(batch.JobType);

                        batchName = batch.BatchNumber;
                        if (BatchNumber.Text == resultBatches.ReturnValue[0].BatchAlias)
                        {
                            nlogger.Trace("         Batch Name " + BatchNumber.Text + " entered is an Alias. The corresponding Batch Name is: " + batchName);
                            result = MessageBox.Show(this, "The number entered is a Batch Alias Name. The corresponing Box Number/Name is: " + batchName, "Info ....", MessageBoxButtons.OK,
                                                      MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                        ResetValues();

                        BatchNameTextBox.Text = batchName;
                        CurrentStatusTextBox.Text = batch.StatusFlag;
                        originalBatchStatus = batch.StatusFlag;
                        BatchAliasNameTextBox.Text = batch.BatchAlias;
                        RecallCommentsTextBox.Text = batch.RecallReason;
                        RejectCommentsTextBox.Text = batch.RejectionReason;

                        //Only Batches in the following stages are allowed to be change:
                        //Waiting for Validation, Waiting for QC, QC on Hold, QC Failed, Rejected, and Waiting for PDF Conversion
                        if (CurrentStatusTextBox.Text == "Waiting for Validation" || CurrentStatusTextBox.Text == "Waiting for QC" ||
                            CurrentStatusTextBox.Text == "QC on Hold" || CurrentStatusTextBox.Text == "QC Failed" ||
                            CurrentStatusTextBox.Text == "Waiting for Approval" || CurrentStatusTextBox.Text == "Waiting for PDF Conversion")
                        {
                            EnableButtons();
                            nlogger.Trace("         Enable / Disable Options based on current Status.");

                            switch (CurrentStatusTextBox.Text)
                            {
                                case "Waiting for Approval":
                                    RejectedRadioButton.Enabled = true;
                                    RecallRadioButton.Enabled = true;
                                    DeleteRadioButton.Enabled = true;
                                    break;

                                case "Waiting for Validation":
                                case "Waiting for PDF Conversion":
                                case "QC on Hold":
                                case "QC Failed":
                                case "Waiting for QC":
                                    RejectedRadioButton.Enabled = false;
                                    RecallRadioButton.Enabled = true;
                                    DeleteRadioButton.Enabled = true;
                                    break;
                            }
                        }
                        else
                        {
                            RejectedRadioButton.Enabled = false;
                            RecallRadioButton.Enabled = false;
                            nlogger.Trace("         Batch Number " + batchName + " can only be set for Deletion.");
                            result = MessageBox.Show(this, "Batch Number " + batchName + " can only be set for Deletion.", "Message...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }                        
                    }
                }
                ApplyButton.Enabled = false;
                ClearButton.Enabled = true;
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }
        }

        private void DisableButtons()
        {
            ApplyButton.Enabled = false;
            ClearButton.Enabled = false;
        }

        private void EnableButtons()
        {
            ApplyButton.Enabled = true;
            ClearButton.Enabled = true;
        }

        private void DisableOptions()
        {
            RejectedRadioButton.Enabled = false;
            RecallRadioButton.Enabled = false;
            RejectCommentsTextBox.Enabled = false;
            RecallCommentsTextBox.Enabled = false;
            LockedLabel.Visible = false;
            DeleteRadioButton.Enabled = false;
        }

        private void ResetValues()
        {
            BatchNameTextBox.Text = "";
            CurrentStatusTextBox.Text = "";
            BatchNumber.Text = "";

            BatchAliasNameTextBox.Text = "";
            RecallCommentsTextBox.Text = "";
            RejectCommentsTextBox.Text = "";

            DisableButtons();
            LockedLabel.Visible = false;
        
            RejectedRadioButton.Checked = false;
            RecallRadioButton.Checked = false;
            DeleteRadioButton.Checked = false;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ResetValues();
            DisableOptions();
        }

        private void DeleteRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (DeleteRadioButton.Checked)
            {
                RejectCommentsTextBox.Enabled = false;
                RecallCommentsTextBox.Enabled = false;
                ApplyButton.Enabled = true;
                ClearButton.Enabled = true;
            } 
        }

        private void RecallRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (RecallRadioButton.Checked)
            {
                RejectCommentsTextBox.Enabled = false;
                RecallCommentsTextBox.Enabled = true;
                ApplyButton.Enabled = true;
                ClearButton.Enabled = true;
            }
        }

        private void RejectedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (RejectedRadioButton.Checked)
            {
                RejectCommentsTextBox.Enabled = true;
                RecallCommentsTextBox.Enabled = false;
                ApplyButton.Enabled = true;
                ClearButton.Enabled = true;
            }
        }

        private void BatchNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CheckBatchNumber();
                //CheckApplyOption();
            }
        }
    }
}


// STATUS CHANGE RULES
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// From/To	       | Ready to Scan | Scan on Hold | W. for QC | QC on Hold | QC Failed | W. for Output | W. for PDF | W. for Approval | Approved | Exported |Rejected  | Recall |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// Ready to Scan   |	    NA     |       YES	  | AUTO-SYNC |     NO	   |     NO	   |      NO	   |     NO	    |       NO	      |    NO	 |    NO	|    NO	   |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// Scan on Hold    |	    YES	   |       NA     |	   NO	  |     NO	   |     NO	   |      NO	   |     NO	    |       NO 	      |    NO	 |    NO	|    NO	   |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// Waiting for QC  |	    NO     |	   NO	  |    NA	  |     YES	   |     YES   |      YES	   |     NO	    |       NO	      |    NO	 |    NO	|    NO	   |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// QC on Hold	   |        NO	   |       NO     |	   YES    |     NA	   |     YES   |      YES	   |     NO	    |       NO	      |    NO	 |    NO	|    NO	   |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// QC Failed	   |        NO	   |       NO	  |    YES    |	    YES	   |     NA	   |      YES	   |     NO	    |       NO	      |    NO	 |    NO	|    NO	   |   NO   |
// '---------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// W. for Output   |	    NO	   |       NO	  |    YES    |	    YES	   |     YES   |      NA	   |     NO	    |       NO	      |    NO	 |    NO	|    NO	   |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// W. for PDF	NO |	    NO	   |       NO	  |    NO	  |     NO	   |     NO	   |      NA	   |     NO	    |       NO	      |    NO	 |    NO	|    NO    |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// W. for Approval |	    NO	   |       NO	  |    YES	  |     YES	   |     YES   |      YES	   |     NO	    |       NA	      |    YES   |	  NO    |    NO	   |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// Approved	       |        NO	   |       NO	  |    YES	  |     YES	   |     YES   |      YES	   |     NO	    |       YES	      |    NA	 |    NO    |    NO	   |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// Exported	       |        NO	   |       NO	  |    NO     |	    NO	   |     NO	   |      NO	   |     NO	    |       NO	      |    NO	 |    NA	|    YES   |   YES  |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// Rejected	       |        NO	   |       NO	  |    NO     |  AUTO-SYNC |     NO	   |      NO	   |     NO	    |       NO	      |    NO	 |    NO	|    NA    |   NO   |
// ----------------┼---------------┼--------------┼-----------┼------------┼-----------┼---------------┼------------┼-----------------┼----------┼----------┼----------┼--------┼
// Recall	       |        NO	   |       NO	  |    NO     |	    YES	   |     NO	   |      NO	   |     NO     |	    NO	      |    NO	 |    NO	|    NO	   |   NA   |
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
