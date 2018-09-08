using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;

namespace ScanningServicesAdmin.Forms
{
    public partial class QualityControlForm : Form
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public QualityControlForm()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BatchNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CheckBatchNumber();
                CheckApplyOption();
            }
        }

        private void CheckBatchNumber()
        {
            string batchName = "";
            ResultBatches resultBatches = new ResultBatches();
            ResultJobsExtended resultJobsExtended = new ResultJobsExtended();
            Batch batch = new Batch();
            //string kodakBatchPath = "";
            string outputBatchDirectory = "";
            string captureBatchDirectory = "";
            Boolean batchIsLocked = false;
            Boolean batchDirectoryFound = false;
            DialogResult result;
            Boolean continueProcess = true;
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
                        DisableAll();
                        ResetValues();

                        BatchNameTextBox.Text = batchName;
                        CurrentStatusTextBox.Text = batch.StatusFlag;
                        originalBatchStatus = batch.StatusFlag;
                        BatchAliasNameTextBox.Text = batch.BatchAlias;
                        PreparationTimeTextBox.Enabled = true;
                        //PrepTimeMaskedHoursTextBox.Enabled = true;
                        //PrepTimaMaskMinutesTextBox.Enabled = true;
                        //CurrentStatusTextBox.Refresh();
                        //BatchNameTextBox.Refresh();
                        //BatchAliasNameTextBox.Refresh();
                        CommentsTextBox.Text = batch.Comments;
                        RecallCommentsTextBox.Text = batch.RecallReason;
                        RejectCommentsTextBox.Text = batch.RejectionReason;

                        PreparationTimeTextBox.Text = batch.PrepTime.ToString("0#.##"); // batch.PrepTime.ToString().PadLeft(2,'0');
                        //string str = batch.PrepTime.ToString();
                        //string[] strArr = new string[1];
                        //strArr = str.Split('.');
                        //PrepTimeMaskedHoursTextBox.Text = strArr[0].ToString().PadLeft(2, '0');
                        //PrepTimaMaskMinutesTextBox.Text = strArr[1].ToString().PadRight(2, '0');

                        // Get Batch Directory                       
                        foreach (JobExtended job in resultJobsExtended.ReturnValue)
                        {
                            if (job.JobName.ToUpper() == batch.JobType.ToUpper())
                            {                                
                                captureBatchDirectory = job.ScanningFolder + "\\" + job.JobName + "\\" + batch.BatchNumber;
                                if (batch.BatchAlias.Length == 0)
                                    outputBatchDirectory = job.QCOuputFolder + "\\" + job.JobName + "\\" + batch.BatchNumber;
                                else
                                    outputBatchDirectory = job.QCOuputFolder + "\\" + job.JobName + "\\" + batch.BatchAlias;
                                break;
                            }
                        }

                        // Check if Batch Capture Directroy Exist
                        if (!Directory.Exists(captureBatchDirectory))
                        {
                            nlogger.Trace("         Unable to access Batch Capture Directory or Directory not found: " + captureBatchDirectory);
                            result = MessageBox.Show(this, "Unable to access Batch Capture Directory or Directory not found: " + captureBatchDirectory, "Info ....", MessageBoxButtons.OK,
                                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            ClearButton.Enabled = true;
                            return;
                        }

                        // Check if info file exist
                        if (System.IO.File.Exists(captureBatchDirectory + "\\info.lock") || System.IO.File.Exists(captureBatchDirectory + "\\info"))
                        {
                            if (System.IO.File.Exists(captureBatchDirectory + "\\info.lock"))
                            {
                                nlogger.Trace("         The requested Batch Name " + BatchNumber.Text + " is locked");
                                batchIsLocked = true;
                            }
                            else
                            {
                                if (System.IO.File.Exists(captureBatchDirectory + "\\info"))
                                {
                                    nlogger.Trace("         The requested Batch Name " + BatchNumber.Text + " is unlocked");
                                    batchIsLocked = false;
                                }
                            }
                        }
                        else
                        {
                            nlogger.Trace("         Unable to find Info or info.lock file in Batch Capture Directory: " + captureBatchDirectory);
                            result = MessageBox.Show(this, "Unable to find Info or info.lock file in Batch Capture Directory: " + captureBatchDirectory, "Info ....", MessageBoxButtons.OK,
                                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            ClearButton.Enabled = true;
                            return;
                        }

                        QCFailedRadioButton.Checked = false;
                        QCinHoldRadioButton.Checked = false;
                        QCCompletedRadioButton.Checked = false;
                        WaitingForQCRadioButton.Checked = false;
                        nlogger.Trace("         Current Status: " + CurrentStatusTextBox.Text);

                        //Do not let the batch go forther than here 
                        //Only Batches in the following stages are allowed to be change:
                        //Waiting for Validation, Waiting for QC, QC on Hold, QC Failed, Reejcted, and Waiting for PDF Conversion
                        if (!(CurrentStatusTextBox.Text == "Waiting for Validation" || CurrentStatusTextBox.Text == "Waiting for QC" ||
                                CurrentStatusTextBox.Text == "QC on Hold" || CurrentStatusTextBox.Text == "QC Failed" ||
                                CurrentStatusTextBox.Text == "Rejected" || CurrentStatusTextBox.Text == "Waiting for PDF Conversion" || CurrentStatusTextBox.Text == "Waiting to be Cleaned"))
                        {
                            switch (CurrentStatusTextBox.Text)
                            {
                                case "Waiting for Output":
                                    result = MessageBox.Show(this, "This Batch has been QC Completed. You need to Output this Batch in Capture Pro." + Environment.NewLine +
                                        "The current status for this Batch is: " + CurrentStatusTextBox.Text, "Message...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    nlogger.Trace("         This Batch has been QC Completed.You need to Output this Batch in Capture Pro." + Environment.NewLine +
                                        "The current status for this Batch is: " + CurrentStatusTextBox.Text);
                                    break;

                                case "Validation Approved":
                                    result = MessageBox.Show(this, "This Batch has been submitted for Validation and it was approved. You DO NOT NEED to Output this Batch." + Environment.NewLine +
                                        "The current status for this Batch is: " + CurrentStatusTextBox.Text, "Message...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    nlogger.Trace("         This Batch has been submitted for Validation and it was approved. You DO NOT NEED to Output this Batch." + Environment.NewLine +
                                        "The current status for this Batch is: " + CurrentStatusTextBox.Text);
                                    break;

                                case "Validation Completed":
                                    result = MessageBox.Show(this, "This Batch has been Validated for Re-Output. You need to Output this Batch in Capture Pro." + Environment.NewLine +
                                        "The current status for this Batch is: " + CurrentStatusTextBox.Text, "Message...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    nlogger.Trace("         This Batch has been Validated for Re - Output.You need to Output this Batch in Capture Pro.");
                                    break;

                                case "Recall":
                                    result = MessageBox.Show(this, "You cannot change the status of this Batch." + Environment.NewLine +
                                                                 "The current status for this Batch is: " + CurrentStatusTextBox.Text + Environment.NewLine +
                                                                 "Batches that have been tagged for Recall is just a remider that you need to Request the Batch for rescan. " + Environment.NewLine +
                                                                 "Once the Batch is ready for scan, delete the Batch from SSS before you scan it."
                                                                , "Message...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    nlogger.Trace("         You cannot change the status of this Batch.");
                                    break;

                                default:
                                    result = MessageBox.Show(this, "You cannot change the status of this Batch." + Environment.NewLine +
                                        "The current status for this Batch is: " + CurrentStatusTextBox.Text, "Message...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    nlogger.Trace("         You cannot change the status of this Batch.");
                                    break;
                            }
                        }
                        else
                        {
                            //Procesing Batches that are allowed to be changed...
                            //If Batch is unlock, we will create a dommie folder in Capture Pro Output Directory
                            if (batchIsLocked)
                            {
                                //Means ... The Batch is locked
                                result = MessageBox.Show(this, "Batch Number " + batchName + " is locked. Do you want to unlock this batch ?", "Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    nlogger.Trace("         Unlok Info File for Request for Batch :" + batchName);
                                    if (System.IO.File.Exists(captureBatchDirectory + "\\info.lock"))
                                    {
                                        // If by any chance , there is an Info file already, remove this file
                                        if (System.IO.File.Exists(captureBatchDirectory + "\\info"))
                                        {
                                            System.IO.File.Delete(captureBatchDirectory + "\\info");
                                        }

                                        //Unlock the Info File
                                        System.IO.File.Move(captureBatchDirectory + "\\info.lock", captureBatchDirectory + "\\info");
                                        nlogger.Trace("         Info file successfully Unlocked");
                                    }

                                    switch (CurrentStatusTextBox.Text)
                                    {
                                        case "Waiting for QC":
                                            DBTransactions.BatchTrackingEvent(batchName, CurrentStatusTextBox.Text, "Waiting for QC", "QC Started", OperatorTextBox.Text);
                                            nlogger.Trace("         The Status of the Batch is Waiting for QC (QC Started Event).");
                                            break;

                                        case "Waiting for Validation":
                                            DBTransactions.BatchTrackingEvent(batchName, CurrentStatusTextBox.Text, "Waiting for Validation", "QC Validation Started", OperatorTextBox.Text);
                                            nlogger.Trace("         The Status of the Batch is Waiting for Validation (QC Validation Started Event).");
                                            break;

                                        case "QC Failed":                                            
                                            batch.StatusFlag = "Waiting for QC";
                                            DBTransactions.BatchUpdate(batch);
                                            CurrentStatusTextBox.Text = "Waiting for QC";
                                            DBTransactions.BatchTrackingEvent(batchName, CurrentStatusTextBox.Text, "Waiting for QC", "QC Started", OperatorTextBox.Text);
                                            nlogger.Trace("         The Status of this Batch is QC Failed. Once the Batch is unlocked, the status is se to Waiting for QC (QC Started Event).");
                                            break;

                                        case "QC on Hold":                                           
                                            batch.StatusFlag = "Waiting for QC";
                                            DBTransactions.BatchUpdate(batch);
                                            CurrentStatusTextBox.Text = "Waiting for QC";
                                            DBTransactions.BatchTrackingEvent(batchName, CurrentStatusTextBox.Text, "Waiting for QC", "QC Started", OperatorTextBox.Text);
                                            nlogger.Trace("         The Status of the Batch is QC on Hold. Once the Batch is unlocked, the status is se to Waiting for QC (QC Started Event).");
                                            break;

                                        case "Rejected":
                                            result = MessageBox.Show(this, "This Batch was tagged for Rejection by the client. Check the Rejection comments for help.", "Informacion...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            // If Batch is unlock with the status of "Rejected" is must be set to "Waiting for QC"
                                            // The Rejection message will show up as well as Rejection times counter                                            
                                            batch.StatusFlag = "Waiting for QC";
                                            DBTransactions.BatchUpdate(batch);
                                            CurrentStatusTextBox.Text = "Waiting for QC";
                                            DBTransactions.BatchTrackingEvent(batchName, CurrentStatusTextBox.Text, "Waiting for QC", "QC Started", OperatorTextBox.Text);
                                            nlogger.Trace("         The Status of the Batch is Rejected. Once is unlocked, the status is se to Waiting for QC (QC Started Event).");
                                            break;

                                        case "Waiting for PDF Conversion":
                                            // Future Considerations: if a Btach is Waiting for PDF Conversion, we should wait for the Batch to be Converted to PDF First before 
                                            // we allow to change the Status. This will prevent overlaping the Batch information while the Batch is been processed by Autobahn
                                            // Once we have implemneted a new Batch delivery process that adjust the Batch Status to "Waiting for Indexing", we will keep the logic below                                            
                                            batch.StatusFlag = "Waiting for QC";
                                            DBTransactions.BatchUpdate(batch);
                                            CurrentStatusTextBox.Text = "Waiting for QC";
                                            DBTransactions.BatchTrackingEvent(batchName, CurrentStatusTextBox.Text, "Waiting for C", "QC Started", OperatorTextBox.Text);
                                            nlogger.Trace("         The Status of the Batch is Waiting for PDF Conversion. Once the Batch is unlocked, the status is se to Waiting for QC (QC Started Event).");
                                            break;

                                        case "Waiting to be Cleaned":
                                            DBTransactions.BatchTrackingEvent(batchName, CurrentStatusTextBox.Text, "Waiting to be Cleanned", "Cleannig Started", OperatorTextBox.Text);
                                            nlogger.Trace("         The Status of the Batch is Waiting to be Cleaned. Once the Batch is unlocked, the status remain as Waiting to be Cleaned.");
                                            break;
                                    }

                                    //Create a dommmie Output Folder and DOCUMENTS.XML File
                                    nlogger.Trace("         Creating a dommie Output Folder and Documents.xml file");
                                    if (!(System.IO.Directory.Exists(outputBatchDirectory)))
                                    {
                                        nlogger.Trace("         Creating new directory: " + outputBatchDirectory);
                                        System.IO.Directory.CreateDirectory(outputBatchDirectory);
                                        nlogger.Trace("         Creating a Dommie File: " + outputBatchDirectory + "\\DOCUMENTS.XML");
                                        System.IO.File.WriteAllText(outputBatchDirectory + "\\DOCUMENTS.XML", string.Empty);
                                        System.IO.File.WriteAllText(outputBatchDirectory + "\\DOCUMENTS.XML", "This is a dommie file and is used to prevent QC operators to Output Batches that are no completed.");
                                        nlogger.Trace("         Creating a Dommie File: " + outputBatchDirectory + "\\Process.lock");
                                    }
                                    else
                                    {
                                        nlogger.Trace("         Directory already exist." + outputBatchDirectory);
                                    }

                                    // Update QCStartTime = current time
                                    // Call UpdateQCStartTime(BatchNumber.Text, Date.Now)
                                    batch.QCStartTime = DateTime.Now;
                                    DBTransactions.BatchUpdate(batch);

                                    LockedLabel.Visible = false;

                                    if (originalBatchStatus == "Waiting for Validation")
                                    {
                                        nlogger.Trace("         This Batch has been Flagged for Validation. You will either Approve it without outputing the Batch, or Select the Option Validation Completed in order to output the Batch.");
                                        result = MessageBox.Show(this, "This Batch has been Flagged for Validation. You will either Approve it without outputing the Batch, or Select the Option Validation Completed in order to output the Batch. Do you want to continue?", "Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (result != DialogResult.Yes)
                                        {
                                            nlogger.Trace("         Operator decided not to continue.");
                                            continueProcess = false;
                                            DisableButtonsOnly();
                                        }
                                        else
                                        {
                                            nlogger.Trace("         Continue option was selected.");
                                            continueProcess = true;
                                        }
                                    }
                                }
                                else
                                {
                                    LockedLabel.Visible = true;
                                    DisableButtonsOnly();
                                }
                            }
                            else
                            {
                                //Means ... The Batch is unlocked
                                LockedLabel.Visible = false;
                                if (originalBatchStatus == "Waiting for Output")
                                {
                                    result = MessageBox.Show(this, "This Batch has been Flagged for Output. You MUST OUTPUT the Batch in Capture Pro.", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    nlogger.Trace("         This Batch is in Waiting for Output. You MUST OUTPUT the Batch in Capture Pro. ");
                                }
                                else
                                {
                                    if (originalBatchStatus == "Waiting for Validation")
                                    {
                                        nlogger.Trace("         This Batch has been Flagged for Validation. You will either Approve it without outputing the Batch, or Select the Option Validation Completed in order to output the Batch.");
                                        result = MessageBox.Show(this, "This Batch has been Flagged for Validation. You will either Approve it without outputing the Batch, or Select the Option Validation Completed in order to output the Batch. Do you want to continue?", "Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (result != DialogResult.Yes)
                                        {
                                            nlogger.Trace("         Operator decided not to continue.");
                                            continueProcess = false;
                                            DisableButtonsOnly();
                                        }
                                        else
                                        {
                                            nlogger.Trace("         Continue option was selected.");
                                            continueProcess = true;
                                        }
                                    }
                                    else
                                    {
                                        if (originalBatchStatus == "Waiting for QC")
                                        {
                                            //Create a dommmie Output Folder and DOCUMENTS.XML File
                                            nlogger.Trace("         Creating a dommie Output Folder and Documents.xml file");
                                            if (!(System.IO.Directory.Exists(outputBatchDirectory)))
                                            {
                                                nlogger.Trace("         Creating new directory: " + outputBatchDirectory);
                                                System.IO.Directory.CreateDirectory(outputBatchDirectory);
                                                nlogger.Trace("         Creating a Dommie File: " + outputBatchDirectory + "\\DOCUMENTS.XML");
                                                System.IO.File.WriteAllText(outputBatchDirectory + "\\DOCUMENTS.XML", string.Empty);
                                                System.IO.File.WriteAllText(outputBatchDirectory + "\\DOCUMENTS.XML", "This is a dommie file and is used to prevent QC operators to Output Batches that are no completed.");
                                                nlogger.Trace("         Creating a Dommie File: " + outputBatchDirectory + "\\Process.lock");
                                            }
                                            else
                                            {
                                                nlogger.Trace("         Directory already exist." + outputBatchDirectory);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //This section enable / disable options based on Batch Current Status
                        ClearButton.Enabled = true;
                        ApplyButton.Enabled = false;
                        CommentsTextBox.Enabled = false;
                        RecallCommentsTextBox.Enabled = false;
                        RejectCommentsTextBox.Enabled = false;
                        WaitingForQCRadioButton.Enabled = false;
                        WaitingForQCRadioButton.Checked = false;
                        QCinHoldRadioButton.Checked = false;
                        QCFailedRadioButton.Checked = false;
                        QCCompletedRadioButton.Checked = false;
                        ValidationApprovedRadioButton.Checked = false;
                        ReleaseBatchforOutputRadioButton.Checked = false;
                        CommentsTextBox.Text = batch.Comments;
                        RecallCommentsTextBox.Text = batch.RecallReason;
                        RejectCommentsTextBox.Text = batch.RejectionReason;

                        if (LockedLabel.Visible || continueProcess)
                        {

                            switch (CurrentStatusTextBox.Text)
                            {
                                case "Waiting to be Cleaned":
                                    QCinHoldRadioButton.Enabled = false;
                                    QCFailedRadioButton.Enabled = false;
                                    QCCompletedRadioButton.Enabled = false;
                                    ValidationApprovedRadioButton.Enabled = false;
                                    ReleaseBatchforOutputRadioButton.Enabled = false;
                                    WaitingForQCRadioButton.Enabled = false;
                                    CommentsTextBox.Enabled = false;
                                    nlogger.Trace("         Since Current Status is Waiting to be Cleanned, enable Waiting for QC Option.");
                                    break;

                                case "QC on Hold":
                                    QCinHoldRadioButton.Enabled = false;
                                    QCFailedRadioButton.Enabled = true;
                                    QCCompletedRadioButton.Enabled = true;
                                    ValidationApprovedRadioButton.Enabled = false;
                                    ReleaseBatchforOutputRadioButton.Enabled = false;
                                    WaitingForQCRadioButton.Enabled = false;
                                    CommentsTextBox.Enabled = true;
                                    nlogger.Trace("         Since Current Status is QC on Hold, enable QC Failed, and QC Completed Options.");
                                    break;

                                case "QC Failed":
                                    QCinHoldRadioButton.Enabled = true;
                                    QCFailedRadioButton.Enabled = false;
                                    QCCompletedRadioButton.Enabled = true;
                                    ValidationApprovedRadioButton.Enabled = false;
                                    ReleaseBatchforOutputRadioButton.Enabled = false;
                                    WaitingForQCRadioButton.Enabled = false;
                                    CommentsTextBox.Enabled = true;
                                    nlogger.Trace("         Since Current Status is QC Failed, enable QC on Hold, and QC Completed Options.");
                                    break;

                                case "Rejected":
                                case "Waiting for PDF Conversion":
                                case "Waiting for QC":
                                    QCinHoldRadioButton.Enabled = true;
                                    QCFailedRadioButton.Enabled = true;
                                    QCCompletedRadioButton.Enabled = true;
                                    ValidationApprovedRadioButton.Enabled = false;
                                    ReleaseBatchforOutputRadioButton.Enabled = false;
                                    WaitingForQCRadioButton.Enabled = false;
                                    CommentsTextBox.Enabled = true;
                                    nlogger.Trace("         Since Current Status is " + CurrentStatusTextBox.Text + ", enable QC on Hold, QC Failed, and QC Completed Options.");
                                    break;

                                case "Waiting for Validation":
                                    if (continueProcess)
                                    {
                                        QCinHoldRadioButton.Enabled = true;
                                        QCFailedRadioButton.Enabled = true;
                                        QCCompletedRadioButton.Enabled = false;
                                        ValidationApprovedRadioButton.Enabled = true;
                                        ReleaseBatchforOutputRadioButton.Enabled = true;
                                        WaitingForQCRadioButton.Enabled = false;
                                        CommentsTextBox.Enabled = true;
                                        nlogger.Trace("         Since Current Status is Waiting for Validation, enable Validation Completed, and Validation Approved Options.");
                                    }
                                    else
                                    {
                                        nlogger.Trace("         Operator decided not to continue.");
                                        DisableAll();
                                        ResetValues();
                                    }
                                    break;

                                default:
                                    nlogger.Trace("         Unable to determine what options need to be enabled.");
                                    DisableAll();
                                    ResetValues();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                General.ErrorMessage(ex);
            }            
        }

        private void CheckApplyOption()
        {
            if (PreparationTimeTextBox.Text != ""  && OperatorTextBox.Text.Length > 0 && StationNameTextBox.Text.Length > 0 &&
               (QCinHoldRadioButton.Checked || QCFailedRadioButton.Checked || QCCompletedRadioButton.Checked ||
               ValidationApprovedRadioButton.Checked || ReleaseBatchforOutputRadioButton.Checked || WaitingForQCRadioButton.Checked))
                ApplyButton.Enabled = true;
            else
                ApplyButton.Enabled = false;
        }

        private void DisableButtonsOnly()
        {
            QCinHoldRadioButton.Enabled = false;
            QCFailedRadioButton.Enabled = false;
            QCCompletedRadioButton.Enabled = false;
            QCCompletedRadioButton.Enabled = false;
            ValidationApprovedRadioButton.Enabled = false;
            ReleaseBatchforOutputRadioButton.Enabled = false;
            WaitingForQCRadioButton.Enabled = false;

            //UsersComboBox.Enabled = False
            //'StationComboBox.Enabled = False
            ApplyButton.Enabled = false;
            ClearButton.Enabled = false;
            CommentsTextBox.Enabled = false;
        }

        private void EnableButtons()
        {
            QCinHoldRadioButton.Enabled = true;
            QCFailedRadioButton.Enabled = true;
            QCCompletedRadioButton.Enabled = true;
            ValidationApprovedRadioButton.Enabled = true;
            ApplyButton.Enabled = true;
            ClearButton.Enabled = true;
            CommentsTextBox.Enabled = true;
        }

        private void DisableAll()
        {
            //PrepTimeMaskedHoursTextBox.Enabled = false;
            //PrepTimaMaskMinutesTextBox.Enabled = false;
            PreparationTimeTextBox.Enabled = false;
            QCinHoldRadioButton.Enabled = false;
            QCFailedRadioButton.Enabled = false;
            QCCompletedRadioButton.Enabled = false;
            QCFailedRadioButton.Checked = false;
            QCinHoldRadioButton.Checked = false;
            QCCompletedRadioButton.Enabled = false;
            WaitingForQCRadioButton.Enabled = false;
            WaitingForQCRadioButton.Checked = false;
            ValidationApprovedRadioButton.Enabled = false;
            ReleaseBatchforOutputRadioButton.Enabled = false;
            ApplyButton.Enabled = false;
            ClearButton.Enabled = false;
            CommentsTextBox.Enabled = false;
            RejectCommentsTextBox.Enabled = false;
            RecallCommentsTextBox.Enabled = false;
        }

        private void ResetValues()
        {
            QCFailedRadioButton.Checked = false;
            QCinHoldRadioButton.Checked = false;
            QCCompletedRadioButton.Checked = false;
            ValidationApprovedRadioButton.Checked = false;
            ReleaseBatchforOutputRadioButton.Checked = false;
            WaitingForQCRadioButton.Checked = false;
            BatchNameTextBox.Text = "";
            CurrentStatusTextBox.Text = "";
            BatchNumber.Text = "";
            CommentsTextBox.Text = "";
            BatchAliasNameTextBox.Text = "";
            BatchAliasNameTextBox.Text = "";
            RecallCommentsTextBox.Text = "";
            RejectCommentsTextBox.Text = "";
            //PrepTimeMaskedHoursTextBox.Text = "";
            //PrepTimaMaskMinutesTextBox.Text = "";
            PreparationTimeTextBox.Text = "";

            DisableAll();
            LockedLabel.Visible = false;
        }

        private void QualityControlForm_Load(object sender, EventArgs e)
        {
            DisableAll();
            LockedLabel.Visible = false;

            StationNameTextBox.Text = System.Net.Dns.GetHostName();
            OperatorTextBox.Text = Environment.UserName;

            nlogger.Trace("QC Operation Report - " + DateTime.Now);
            nlogger.Trace("     Quality Control - Adjust Status Operation");
            nlogger.Trace("         Operator ID: " + OperatorTextBox.Text);
            nlogger.Trace("         Station Name: " + StationNameTextBox.Text);

            if (!string.IsNullOrEmpty(Data.GlovalVariables.currentBatchName))
                BatchNumber.Text = Data.GlovalVariables.currentBatchName;

        }

        private void QCinHoldRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }

        private void QCFailedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }

        private void QCCompletedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }

        private void ValidationApprovedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }

        private void ReleaseBatchforOutputRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }

        private void PrepTimeMaskedHoursTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }

        private void PrepTimaMaskMinutesTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ResetValues();
        }
        private static string CleanString(string inpuText)
        {
        //    string clean = "";
        //    for (int i = 1; i <= inpuText.Length; i++)
        //    {
        //        if ((Convert.ToInt32(inpuText.Substring((i - 1), 1)) > 31)  && (Convert.ToInt32(inpuText.Substring((i - 1), 1)) < 127))
        //        {
        //            clean = clean + inpuText.Substring((i - 1), 1);
        //        }
        //    }
            return inpuText;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            string batchName = "";
            string newStatusFlag = "";
            string originalBatchStatus = "";
            string action = "";
            string eventDescription = "";
            string outputBatchDirectory = "";
            string captureBatchDirectory = "";
            DateTime currentDate;
            Boolean updateBatchStatus = false;
            DialogResult result;
            ResultBatches resultBatches = new ResultBatches();
            ResultJobsExtended resultJobsExtended = new ResultJobsExtended();
            Batch batch = new Batch();
            ResultGeneric resultGeneric = new ResultGeneric();

            try
            {
                nlogger.Trace("     QC Transaction (Apply Action) for Batch: " + BatchNameTextBox.Text);
                resultBatches = DBTransactions.GetBatchesInformation("BatchNumber = \"" + BatchNameTextBox.Text + "\"", ""); // OR BatchAlias = \"" + BatchNumber.Text + "\"", "");

                if (resultBatches.RecordsCount == 0)
                {
                    nlogger.Trace("         Batch Name " + BatchNumber.Text + " entered could not be found");
                    result = MessageBox.Show(this, "The Batch Name entered could not be found.", "Info ....", MessageBoxButtons.OK,
                                                  MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    batch = resultBatches.ReturnValue[0];
                    originalBatchStatus = batch.StatusFlag;

                    resultJobsExtended = DBTransactions.GetJobByName(batch.JobType);

                    // Get Batch Directory                       
                    foreach (JobExtended job in resultJobsExtended.ReturnValue)
                    {
                        if (job.JobName.ToUpper() == batch.JobType.ToUpper())
                        {
                            captureBatchDirectory = job.ScanningFolder + "\\" + job.JobName + "\\" + batch.BatchNumber;
                            if (batch.BatchAlias.Length == 0)
                                outputBatchDirectory = job.QCOuputFolder + "\\" + job.JobName + "\\" + batch.BatchNumber;
                            else
                                outputBatchDirectory = job.QCOuputFolder + "\\" + job.JobName + "\\" + batch.BatchAlias;
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

                    if (QCCompletedRadioButton.Checked)
                    {
                        action = "QCCompleted";
                    }
                   if (ValidationApprovedRadioButton.Checked)
                    {
                        action = "ValidationApproved";
                    }
                    if (ReleaseBatchforOutputRadioButton.Checked)
                    {
                        action = "ReleaseBatchforOutput";
                    }
                    if (QCFailedRadioButton.Checked)
                    {
                        action = "QCFailed";
                    }
                    if (QCinHoldRadioButton.Checked)
                    {
                        action = "QCinHold";
                    }
                    if (WaitingForQCRadioButton.Checked)
                    {
                        action = "WaitingForQC";
                    }

                    // Update prep time
                    //batch.PrepTime = Convert.ToDouble(PrepTimeMaskedHoursTextBox.Text.ToString().PadLeft(2, '0') + "." + PrepTimaMaskMinutesTextBox.Text.ToString().PadRight(2, '0'));
                    batch.PrepTime = Convert.ToDouble(PreparationTimeTextBox.Text.Replace("_", "0"));  //Convert.ToDouble(PreparationTimeTextBox.Text);
                    
                    switch (action)
                    {
                        case "QCCompleted":
                            // It means that the QC Stage finished
                            newStatusFlag = "Waiting for Output";
                            CurrentStatusTextBox.Text = "Waiting for Output";
                            eventDescription = "QC Completed";
                            updateBatchStatus = true;
                            nlogger.Trace("         Action selected = QC Completed. The New Status will be: " + newStatusFlag);                           

                            // Update QCEndTime = current time
                            currentDate = DateTime.Now;
                            batch.QCEndTime = currentDate;

                            // Update QCStageTime = current time - ScanningEndTime
                            nlogger.Trace("         Scanning End Time : " + batch.ScanningEndTime.ToString());
                            nlogger.Trace("         QC End Time (Current Time): " + currentDate.ToString());

                            if (batch.ScanningEndTime.ToString().Length == 0)
                            {
                                nlogger.Trace("         QC Stage Time (QC End Time - Scanning End Time): 0");
                                batch.QCStageTime = 0;
                            }
                            else
                            {
                                nlogger.Trace("         QC Stage Time(QC End Time - Scanning End Time): " + currentDate.Subtract(batch.ScanningEndTime).TotalMinutes);
                                batch.QCStageTime = Convert.ToInt32(currentDate.Subtract(batch.ScanningEndTime).TotalMinutes);
                            }

                            // Get the Modified date from the Kodak info file
                            //oRead = IO.File.OpenText(KodakBatchPath & "\info")
                            //While oRead.Peek <> -1
                            //    LineIn = oRead.ReadLine()
                            //    If LineIn.Contains("batch.ModifiedDatetime") Then
                            //        sModdifiedDate = LineIn.Split("=")
                            //    End If
                            //    If LineIn.Contains("batch.ModifiedWorkstationID") Then
                            //        sModdifiedStation = LineIn.Split("=")
                            //    End If
                            //End While
                            //oRead.Close()

                            // Formula: Update QCTime = Modified date - QCStartTime
                            //Call GenerateFile(sReportLogFile, Now &Space(12) & "Last Time Moddified (from info file) : " & Trim(sModdifiedDate(1)).ToString, bDAFDebug)
                            //Call GenerateFile(sReportLogFile, Now &Space(12) & "QC Start Time : " & Batches(0).QCStartTime.ToString)

                            // We cannot relay in the LastModifiedDate information in KCP info File so we are going to use the time in which the
                            // operator perform the QC Complete.
                            //If Batches(0).QCStartTime.Length = 0 Then
                            //    Call GenerateFile(sReportLogFile, Now & Space(12) & "QC Time (Last Time Moddified - QC Start Time): " & DateDiff("n", CurrentDate, CurrentDate), bDAFDebug)
                            //    Call UpdateQCTime(BatchNameTextBox.Text, DateDiff("n", CurrentDate, CurrentDate))
                            //Else
                            //    Call GenerateFile(sReportLogFile, Now & Space(12) & "QC Time (Last Time Moddified - QC Start Time): " & DateDiff("n", Batches(0).QCStartTime, Trim(sModdifiedDate(1))).ToString, bDAFDebug)
                            //    Call GenerateFile(sReportLogFile, Now &Space(12) & "QC Time (Last Time Moddified - QC Start Time): " & DateDiff("n", Batches(0).QCStartTime, CurrentDate), bDAFDebug)
                            //    Call UpdateQCTime(BatchNameTextBox.Text, DateDiff("n", Batches(0).QCStartTime, CurrentDate))
                            //End If

                            // Update Work Station ID based in the information conatined in the Info File
                            //Call GenerateFile(sReportLogFile, Now &Space(12) & "Modified Station: " & Trim(sModdifiedStation(1)), bDAFDebug)
                            //Call UpdateQCWorkStation(BatchNameTextBox.Text, Trim(sModdifiedStation(1)))

                            //Create a second backup copy of the info file
                            nlogger.Trace("         Creating an Info Backup File: " + captureBatchDirectory + "\\info" + ".bck2");
                            
                            // Remove Back2 copy if the file exist
                            if (System.IO.File.Exists(captureBatchDirectory + "\\info" + ".bck2"))
                                System.IO.File.Delete(captureBatchDirectory + "\\info" + ".bck2");

                            System.IO.File.Copy(captureBatchDirectory + "\\info", captureBatchDirectory + "\\info" + ".bck2");

                            // Rename the Batch folder so the QC Operator can Output the Batch
                            nlogger.Trace("         Renaming existing Kodak Output Folder so QC Operator can output the Bacth via Capture Pro.");

                            if (System.IO.Directory.Exists(outputBatchDirectory + ".REMOVE"))
                            {
                                // Delete Folder First ...
                                System.IO.Directory.Delete(outputBatchDirectory + ".REMOVE", true);
                            }
                            if (System.IO.Directory.Exists(outputBatchDirectory))
                            {
                                // Rename the Batch Folder for future deletion
                                nlogger.Trace("         Renaming directory " + outputBatchDirectory +  " to " + outputBatchDirectory + ".REMOVE");
                                System.IO.Directory.Move(outputBatchDirectory, outputBatchDirectory + ".REMOVE");
                            }

                            updateBatchStatus = true;
                            break;

                        case "ValidationApproved":
                            // Change the status to "Validation Approved"
                            newStatusFlag = "Validation Approved";
                            eventDescription = "QC Validation Forced to Complete";
                            updateBatchStatus = true;
                            nlogger.Trace("         Action selected = Validation Approved. The New Status will be: " + newStatusFlag);
                             break;

                        case "ReleaseBatchforOutput":
                            // Rename output folder so capture pro to prevent Capture Pro failng in the Output Process
                            nlogger.Trace("         Batch Ouptut Release requested.");
                            newStatusFlag = "Validation Completed";
                            eventDescription = "QC Validation Completed";
                            updateBatchStatus = true;
                            nlogger.Trace("         Action selected = Validation Completed. The New Status will be: " + newStatusFlag);
                            nlogger.Trace("         Renaming existing Kodak Output Folder so QC Operator can output the Bacth via Capture Pro.");

                            nlogger.Trace("         Looking for folder: " + outputBatchDirectory);
                            if (System.IO.Directory.Exists(outputBatchDirectory))
                            {
                                nlogger.Trace("         Renaming directory " + outputBatchDirectory + " to " + outputBatchDirectory + ".REMOVE");
                                System.IO.Directory.Move(outputBatchDirectory, outputBatchDirectory + ".REMOVE");
                            }
                            else
                            {
                                nlogger.Trace("         Folder not Found");
                            }             
                            break;

                        case "QCFailed":
                            // Change the status to "QC Failed"
                            newStatusFlag = "QC Failed";
                            eventDescription = "QC Failed";
                            updateBatchStatus = true;
                            nlogger.Trace("         Action selected = QC Failed. The New Status will be: " + newStatusFlag);
                            // Lock Info File
                            General.LockBatchInfoFile(captureBatchDirectory);
                            break;

                        case "QCinHold":
                            newStatusFlag = "QC on Hold";
                            eventDescription = "QC on Hold";
                            updateBatchStatus = true;
                            nlogger.Trace("         Action selected = On Hold. The New Status will be: " + newStatusFlag);
                            // Lock Info File
                            General.LockBatchInfoFile(captureBatchDirectory);
                            break;

                        case "WaitingForQC":
                            newStatusFlag = "Waiting for QC";
                            eventDescription = "Waiting for QC";
                            updateBatchStatus = true;
                            nlogger.Trace("         Action selected = Waiitng for QC. The New Status will be: " + newStatusFlag);
                            // Lock Info File
                            General.LockBatchInfoFile(captureBatchDirectory);
                            break;
                    }

                    // Remove no printable characters from text box
                    CommentsTextBox.Text = CleanString(CommentsTextBox.Text);

                    if (updateBatchStatus)
                    {
                        // Update Batch Information
                        batch.StatusFlag = newStatusFlag;
                        batch.QCBy = OperatorTextBox.Text;
                        batch.QCStation = StationNameTextBox.Text;
                        batch.Comments = CommentsTextBox.Text;

                        nlogger.Trace("         Updating Batch Information in Database...");
                        nlogger.Trace("             New Status: " + newStatusFlag);
                        nlogger.Trace("             Batch Number: " + batch.BatchNumber);
                        nlogger.Trace("             Operator: " + OperatorTextBox.Text);
                        nlogger.Trace("             Station: " + StationNameTextBox.Text);
                        nlogger.Trace("             Comments: " + CommentsTextBox.Text);

                        // Update Batch Information
                        resultGeneric = DBTransactions.BatchUpdate(batch);
                        if (resultGeneric.ReturnCode == 0)
                        {
                            // Batch Event Tracking
                            DBTransactions.BatchTrackingEvent(batch.BatchNumber, originalBatchStatus, newStatusFlag, eventDescription, OperatorTextBox.Text);
                        }

                        if (action == "QCCompleted" || action == "QCinHold" || action == "QCFailed" ||
                            action == "ValidationApproved" || action == "ReleaseBatchforOutput")
                        {
                            ResetValues();
                        }
                        //this.Close();
                    }       
                }               
            }
            catch (Exception ex)
            {
                //General.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void PreparationTimeTextBox_Enter(object sender, EventArgs e)
        {
           
        }

        private void PreparationTimeTextBox_Leave(object sender, EventArgs e)
        {
            //Batch batch = new Batch();
            //batch.PrepTime = Convert.ToDouble(PreparationTimeTextBox.Text.Replace("_", "0"));
        }

        private void PreparationTimeTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckApplyOption();
        }
    }
}
