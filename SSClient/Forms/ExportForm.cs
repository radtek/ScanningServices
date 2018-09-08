using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static ScanningServicesDataObjects.GlobalVars;

namespace ScanningServicesAdmin.Forms
{
    public partial class ExportForm : Form
    {

        public int documentsCounter = 0;
        public int workOrdersCounter = 0;
        public string batchBeenDelivered = "";
        public int batchesCounter = 0;
        public int metadataFilesDelivered = 0;
        public int totalDocumentsToDeliver = 0;
        public int totalWorOrdersToDeliver = 0;
        public int totalBatchesToDeliver = 0;
        public int totalMetadtaFilesToDeliver = 0;
        public string generalTransactionMessage = "";
        public Boolean updateDataGrid = false;
        public string currentWorkOrder = "";
        public string workOrdertoUpdateInGrid = "";

        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public ExportForm()
        {
            InitializeComponent();
        }

        private string SetFilter()
        {
            //BatchNumber.Contains("FTW BOX-01")
            string queryFilter = "";

            if (WorkOrderNumber.Text.Length > 0)
            {
                switch (WorkOrderCondition.Text)
                {
                    case "":
                        // DO nothung
                        break;

                    case ">":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber > " + WorkOrderNumber.Text;
                        else
                            queryFilter = queryFilter + " LotNumber > " + WorkOrderNumber.Text;
                        break;

                    case ">=":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber >= " + WorkOrderNumber.Text;
                        else
                            queryFilter = queryFilter + " LotNumber >= " + WorkOrderNumber.Text;
                        break;

                    case "=":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber = " + WorkOrderNumber.Text;
                        else
                            queryFilter = queryFilter + " LotNumber = " + WorkOrderNumber.Text;
                        break;

                    case "<":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber < " + WorkOrderNumber.Text;
                        else
                            queryFilter = queryFilter + " LotNumber < " + WorkOrderNumber.Text;
                        break;

                    case "<=":
                        if (queryFilter.Length == 0)
                            queryFilter = " LotNumber <= " + WorkOrderNumber.Text;
                        else
                            queryFilter = queryFilter + " LotNumber <= " + WorkOrderNumber.Text;
                        break;
                }
            }

            //'Exported By
            if (ExportedByComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = " ExportedBy = \"" + ExportedByComboBox.Text + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + " ExportedBy = \"" + ExportedByComboBox.Text + "\"";
                }
            }

            // Check if Exported Day Filter has been selected
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

            if (ShowReadyForExportCheckBox.Checked)
            {
                if (ShowExportedCheckBox.Checked)
                {
                    if (queryFilter.Length == 0)
                    {
                        queryFilter = " ((LotNumber > 0 AND " + "StatusFlag = \"Approved\") OR (LotNumber > 0 AND " + "StatusFlag = \"Exported\"))";
                    }
                    else
                    {
                        queryFilter = queryFilter + " AND " + " ((LotNumber > 0 AND " + "StatusFlag = \"Approved\") OR (LotNumber > 0 AND " + "StatusFlag = \"Exported\"))";
                    }
                }
                else
                {
                    if (queryFilter.Length == 0)
                    {
                        queryFilter = " LotNumber > 0 AND " + "StatusFlag = \"Approved\"";
                    }
                    else
                    {
                        queryFilter = queryFilter + " AND " + " LotNumber > 0 AND " + "StatusFlag = \"Approved\"";
                    }
                }
            }
            else
            {
                if (ShowExportedCheckBox.Checked)
                {
                    if (queryFilter.Length == 0)
                    {
                        queryFilter = " LotNumber > 0 AND " + "StatusFlag = \"Exported\"";
                    }
                    else
                    {
                        queryFilter = queryFilter + " AND " + " LotNumber > 0 AND " + "StatusFlag = \"Exported\"";
                    }
                }
            }

            //if (ShowReadyForExportCheckBox.Checked)
            //{
            //    // Query for Work Orders that are pending for export
            //    queryFilter = " LotNumber > 0 AND " + "StatusFlag = \"Approved\"";
            //}

            //if (ShowExportedCheckBox.Checked)
            //{

            //    // Query for Work Orders that were exported
            //    queryFilter = " LotNumber > 0 AND " + "StatusFlag = \"Exported\"";
            //}

            // Filter by Job Type

            if (JobTypesComboBox.Text.Length > 0)
            {
                if (queryFilter.Length == 0)
                {
                    queryFilter = "JobType = \"" + JobTypesComboBox.Text.Trim() + "\"";
                }
                else
                {
                    queryFilter = queryFilter + " AND " + "JobType = \"" + JobTypesComboBox.Text.Trim() + "\"";
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
                    queryFilter = queryFilter + " AND " + "Customer = \"" + CustomersComboBox.Text.Trim() + "\"";
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

            return queryFilter;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string queryFilter = SetFilter();
            int currentWorkOrder = 0;
            int totalWODocs = 0;
            int totalWOBoxes = 0;
            int totalWOImages = 0;
            double totalWOSize = 0;
            string currentJobType = "";
            string currentCustomer = "";
            string currentProject = "";
            string currentExportBy = "";
            string currentExportedDate = "";
            Double totalZSizeMB = 0;

            WorkOrdersList.Rows.Clear();

            ResultBatches resultBatches = new ResultBatches();
            ResultBatches resultBatchesHelper = new ResultBatches();
            resultBatches = DBTransactions.GetBatchesInformation(queryFilter, "LotNUmber");

            WorkOrderCountlabel.Text = "";
            BatchCountLabel.Text = "";
            DocCountLabel.Text = "";
            BatchNameLlabel.Text = "";
            DatabaseQqueryLabel.Text = "";
            PercentageLabel.Text = "";
            ProgressBar.Visible = false;

            if (resultBatches.RecordsCount == 0)
            {
                SelectAllButton.Enabled = false;
                UnselectAllButton.Enabled = false;
                ExportButton.Enabled = false;
                ReportButton.Enabled = false;
                WorkOrderInfobutton.Enabled = false;
            }
            else
            {
                SelectAllButton.Enabled = true;
                UnselectAllButton.Enabled = true;
                ExportButton.Enabled = true;
                ReportButton.Enabled = true;
                WorkOrderInfobutton.Enabled = true;

                // Group result list by Work Orders
                foreach (Batch batch in resultBatches.ReturnValue)
                {                    

                    if (currentWorkOrder != batch.LotNumber)
                    {
                        if (currentWorkOrder != 0)
                        {
                            totalZSizeMB = totalWOSize / 1024;
                            resultBatchesHelper =  DBTransactions.GetBatchesInformation("LotNUmber = \"" + currentWorkOrder + "\"", "LotNUmber");
                            string[] row = new string[] { "False", currentWorkOrder.ToString(),currentCustomer, currentJobType,
                                                            resultBatchesHelper.RecordsCount.ToString(), totalWOBoxes.ToString(),
                                                            totalWODocs.ToString(),totalWOImages.ToString(), totalZSizeMB.ToString("0.##"),
                                                            currentExportBy,currentExportedDate };                            
                            WorkOrdersList.Rows.Add(row);
                            
                        }

                        currentJobType = batch.JobType;
                        currentCustomer = batch.Customer;
                        currentProject = batch.ProjectName;
                        currentExportBy = batch.ExportedBy;
                        currentExportedDate = batch.ExportedDate.ToString();

                        totalWODocs = batch.NumberOfDocuments;
                        totalWOBoxes = 1;
                        totalWOImages = batch.NumberOfScannedPages;
                        totalWOSize = batch.BatchSize;
                        
                        //Set current Lot Number
                        currentWorkOrder = batch.LotNumber;
                    }
                    else
                    {
                        totalWODocs = totalWODocs + batch.NumberOfDocuments;
                        totalWOBoxes = totalWOBoxes + 1;
                        totalWOImages = totalWOImages + batch.NumberOfScannedPages;
                        totalWOSize = totalWOSize + batch.BatchSize;                        
                    }
                    // Account for the last element in the Foreach Clause
                    if (resultBatches.ReturnValue.IndexOf(batch) == resultBatches.ReturnValue.Count - 1)
                    {
                        resultBatchesHelper = DBTransactions.GetBatchesInformation("LotNUmber = \"" + currentWorkOrder + "\"", "LotNUmber");
                        string[] row = new string[] { "False", currentWorkOrder.ToString(),currentCustomer, currentJobType,
                                                            resultBatchesHelper.RecordsCount.ToString(), totalWOBoxes.ToString(),
                                                            totalWODocs.ToString(),totalWOImages.ToString(), totalWOSize.ToString(),
                                                            currentExportBy,currentExportedDate };
                        WorkOrdersList.Rows.Add(row);
                    }
                }

                foreach (DataGridViewRow row in WorkOrdersList.Rows)
                {
                    if (string.IsNullOrEmpty(row.Cells["ExportedBy"].Value.ToString()))
                    {
                        if (row.Cells["BoxesVerification"].Value.ToString() != row.Cells["NumberBoxes"].Value.ToString())
                        {
                            row.Cells["ExportCheckBox"].ReadOnly = true;
                            row.DefaultCellStyle.BackColor = Color.MistyRose;
                        }
                        else
                        {                            
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            if (ExportSelectionRadioButton.Checked)
                            {
                                row.Cells["ExportCheckBox"].ReadOnly = false;
                                row.Cells["ExportCheckBox"].Value = false;
                            }
                            else
                            {
                                row.Cells["ExportCheckBox"].ReadOnly = true;
                            }
                        }
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        if (ReExportSelectionRadioButton.Checked)
                        {
                            row.Cells["ExportCheckBox"].ReadOnly = false;
                            row.Cells["ExportCheckBox"].Value = false;
                        }
                        else
                        {
                            row.Cells["ExportCheckBox"].ReadOnly = true;
                        }
                    }                    
                }
                WorkOrdersList.ClearSelection();
                WorkOrdersList.Refresh();
                // Account for the last element in the Foreach Clause
                //if (currentWorkOrder != 0)
                //{
                //    string[] row = new string[] { "False", currentWorkOrder.ToString(),currentCustomer, currentProject, currentJobType, totalWOBoxes.ToString(),
                //                                            totalWODocs.ToString(),totalWOImages.ToString(), totalWOSize.ToString(), currentExportBy,currentExportedDate };
                //    WorkOrdersList.Rows.Add(row);
                //}

                //foreach (Batch batch in resultBatches.ReturnValue)
                //{

                //    string[] row = new string[] { "False", batch.LotNumber.ToString(), batch.Customer, batch.ProjectName, batch.JobType, "1",batch.NumberOfDocuments.ToString(),
                //                                   batch.NumberOfPages.ToString(), batch.BatchSize.ToString(), batch.ExportedBy, batch.ExportedDate.ToString() };

                //    WorkOrdersList.Rows.Add(row);

                //    //WorkOrder, Customer, Project, JobType, NumberBoxes, TotalDocuments, TotalPages, BatchSize, ExportedBy, ExportedDate
                //}
            }

        }

        private void ExportForm_Load(object sender, EventArgs e)
        {

            WorkOrderCountlabel.Text = "";
            BatchCountLabel.Text = "";
            DocCountLabel.Text = "";
            BatchNameLlabel.Text = "";
            DatabaseQqueryLabel.Text = "";
            PercentageLabel.Text = "";


            // Set up the delays for the ToolTip.
            toolTip.AutoPopDelay = 8000;
            toolTip.InitialDelay = 8000;
            toolTip.ReshowDelay = 5000;

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
            JobTypesComboBox.Items.Add("");
            if (resultJobs.RecordsCount > 0)
            {
                foreach (JobExtended job in resultJobs.ReturnValue)
                {
                    JobTypesComboBox.Items.Add(job.JobName);
                }
            }

            // Load User List View
            ResultUsers resultUsers = new ResultUsers();
            List<User> users = new List<User>();
            resultUsers = DBTransactions.GetUsers();
            ExportedByComboBox.Items.Add("");
            if (resultUsers.RecordsCount > 0)
            {
                foreach (User user in resultUsers.ReturnValue)
                {
                    ExportedByComboBox.Items.Add(user.UserName);
                }
            }
        }

        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (BgrdWorker.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                BgrdWorker.CancelAsync();
            }
            this.Close();
        }

        private void WorkOrdersList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int workOrdersCount = 0;
            double batchSize = 0;
            int numOfImages = 0;
            int numDocs = 0;
            int numBoxes = 0;

            WorkOrdersList.EndEdit();

            foreach (DataGridViewRow row in WorkOrdersList.Rows)
            {
                if (row.Cells["ExportCheckBox"].Value.ToString() == "True")
                {
                    workOrdersCount = workOrdersCount + 1;
                    batchSize = batchSize + Convert.ToDouble(row.Cells["BatchSize"].Value);
                    numOfImages = numOfImages + Convert.ToInt32(row.Cells["TotalPages"].Value);
                    numDocs = numDocs + Convert.ToInt32(row.Cells["TotalDocuments"].Value);
                    numBoxes = numBoxes + Convert.ToInt32(row.Cells["NumberBoxes"].Value);
                }

            }
                     

            NumWorkOrdersTextBox.Text = workOrdersCount.ToString();
            BatchSizeTextBox.Text = (batchSize / 1024).ToString("0.##");
            NumImgTextBox.Text = numOfImages.ToString();
            NumDocsBox.Text = numDocs.ToString();
            NumBoxesBox.Text = numBoxes.ToString();

            WorkOrdersList.Refresh();
        }

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            int workOrdersCount = 0;
            double batchSize = 0;
            int numOfImages = 0;
            int numDocs = 0;
            int numBoxes = 0;

            foreach (DataGridViewRow row in WorkOrdersList.Rows)
            {
                if (ExportSelectionRadioButton.Checked)
                {                   
                    if (row.DefaultCellStyle.BackColor == Color.LightGreen)
                    {
                        row.Cells["ExportCheckBox"].ReadOnly = false;
                        row.Cells["ExportCheckBox"].Value = true;
                        workOrdersCount = workOrdersCount + 1;
                        batchSize = batchSize + Convert.ToDouble(row.Cells["BatchSize"].Value);
                        numOfImages = numOfImages + Convert.ToInt32(row.Cells["TotalPages"].Value);
                        numDocs = numDocs + Convert.ToInt32(row.Cells["TotalDocuments"].Value);
                        numBoxes = numBoxes + Convert.ToInt32(row.Cells["NumberBoxes"].Value);
                    }
                }
                if (ReExportSelectionRadioButton.Checked)
                {                    
                    if (row.DefaultCellStyle.BackColor == Color.LightGray)
                    {
                        row.Cells["ExportCheckBox"].ReadOnly = false;
                        row.Cells["ExportCheckBox"].Value = true;
                        workOrdersCount = workOrdersCount + 1;
                        batchSize = batchSize + Convert.ToDouble(row.Cells["BatchSize"].Value);
                        numOfImages = numOfImages + Convert.ToInt32(row.Cells["TotalPages"].Value);
                        numDocs = numDocs + Convert.ToInt32(row.Cells["TotalDocuments"].Value);
                        numBoxes = numBoxes + Convert.ToInt32(row.Cells["NumberBoxes"].Value);
                    }
                }
            }
            NumWorkOrdersTextBox.Text = workOrdersCount.ToString();
            BatchSizeTextBox.Text = (batchSize / 1024).ToString("0.##");
            NumImgTextBox.Text = numOfImages.ToString();
            NumDocsBox.Text = numDocs.ToString();
            NumBoxesBox.Text = numBoxes.ToString();

            WorkOrdersList.EndEdit();
            WorkOrdersList.Refresh();
        }

        private void ExportSelectionRadioButton_Click(object sender, EventArgs e)
        {
            ExportSelection();
        }

        private void ExportSelection()
        {
            foreach (DataGridViewRow row in WorkOrdersList.Rows)
            {
                if (row.DefaultCellStyle.BackColor == Color.LightGreen)
                {
                    row.Cells["ExportCheckBox"].ReadOnly = false;
                }
                if (row.DefaultCellStyle.BackColor == Color.LightGray)
                {
                    row.Cells["ExportCheckBox"].ReadOnly = true;
                    row.Cells["ExportCheckBox"].Value = false;
                }
            }

            WorkOrdersList.EndEdit();
            WorkOrdersList.ClearSelection();
            WorkOrdersList.Refresh();
        }

        private void ReExportSelectionRadioButton_Click(object sender, EventArgs e)
        {
            ReExportSelection();
        }

        private void ReExportSelection()
        {
            foreach (DataGridViewRow row in WorkOrdersList.Rows)
            {
                if (row.DefaultCellStyle.BackColor == Color.LightGreen)
                {
                    row.Cells["ExportCheckBox"].ReadOnly = true;
                    row.Cells["ExportCheckBox"].Value = false;
                }
                if (row.DefaultCellStyle.BackColor == Color.LightGray)
                {
                    row.Cells["ExportCheckBox"].ReadOnly = false;
                }
            }

            WorkOrdersList.EndEdit();
            WorkOrdersList.ClearSelection();
            WorkOrdersList.Refresh();
        }

        private void UnselectAllButton_Click(object sender, EventArgs e)
        {
            int workOrdersCount = 0;
            double batchSize = 0;
            int numOfImages = 0;
            int numDocs = 0;
            int numBoxes = 0;

            foreach (DataGridViewRow row in WorkOrdersList.Rows)
            {              
                row.Cells["ExportCheckBox"].Value = false;
            }
            WorkOrdersList.EndEdit();

            NumWorkOrdersTextBox.Text = workOrdersCount.ToString();
            BatchSizeTextBox.Text = (batchSize / 1024).ToString("0.##");
            NumImgTextBox.Text = numOfImages.ToString();
            NumDocsBox.Text = numDocs.ToString();
            NumBoxesBox.Text = numBoxes.ToString();

            WorkOrdersList.Refresh();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                ExportDirectoryTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        //Inform user of pregress
        private void BgrdWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            ProgressBar.Value = e.ProgressPercentage;
            PercentageLabel.Text = ProgressBar.Value.ToString() + "%";
        }

        private void BgrdWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            long lngFreeSpace;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //CircularProgressBar.Visible = true;              

                if (!ReExportSelectionRadioButton.Checked)
                {
                    // Exorts must happens on Sundays, so check it out
                    if (!NoDeliveryFilesCheckBox.Checked)
                    {
                        if (DateTimePicker1.Value.DayOfWeek != DayOfWeek.Sunday)
                        {
                            DateTimePicker1.Focus();
                            DateTimePicker1.Select();
                            result = MessageBox.Show(this, "Export Date must fall on a Sunday.", "Warning  ....", MessageBoxButtons.OK,
                                                                 MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            nlogger.Trace("Leaving RunExpor Request transaction ...");
                            return;
                        }
                    }
                }

                // Check for Disk Space
                if (!NoDeliveryFilesCheckBox.Checked)
                {
                    if (string.IsNullOrEmpty(ExportDirectoryTextBox.Text))
                    {
                        DateTimePicker1.Focus();
                        DateTimePicker1.Select();
                        result = MessageBox.Show(this, "Export Directory cannot be empty.", "Warning  ....", MessageBoxButtons.OK,
                                                             MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        nlogger.Trace("Leaving RunExpor Request transaction ...");
                        return;
                    }
                    if (IsUncPath(ExportDirectoryTextBox.Text))
                    {
                        lngFreeSpace = GetAvailableDiskSpace(ExportDirectoryTextBox.Text);
                    }
                    else
                    {
                        FileInfo f = new FileInfo(ExportDirectoryTextBox.Text);
                        string driveLetter = Path.GetPathRoot(f.FullName);
                        DriveInfo drive = new DriveInfo(driveLetter);
                        lngFreeSpace = drive.AvailableFreeSpace;
                    }
                    if (lngFreeSpace < Convert.ToDouble(BatchSizeTextBox.Text))
                    {
                        ExportDirectoryTextBox.Focus();
                        ExportDirectoryTextBox.Select();
                        result = MessageBox.Show(this, "There is not enough space on the device.", "Warning  ....", MessageBoxButtons.OK,
                                                             MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        nlogger.Trace("Leaving RunExpor Request transaction ...");
                        return;
                    }
                }
                

                StatusGroupBox.Visible = true;
                //ProgressBar.Visible = true;
                //ProgressBar.Minimum = 0;
                //ProgressBar.Value = 0;

                ProgressBar.Visible = true;
                ProgressBar.Value = 0;
                ProgressBar.Minimum = 0;
                //ProgressBar.Maximum = Convert.ToInt32(NumDocsBox.Text);
                PercentageLabel.Text = "";
                               
                workOrdersCounter = 0;
                batchBeenDelivered = "";
                batchesCounter = 0;
                documentsCounter = 0;
                //metadataFilesDelivered = 0;

                totalWorOrdersToDeliver = Convert.ToInt32(NumWorkOrdersTextBox.Text);
                totalBatchesToDeliver = Convert.ToInt32(NumBoxesBox.Text);
                totalDocumentsToDeliver = Convert.ToInt32(NumDocsBox.Text);
                totalMetadtaFilesToDeliver = 0;
                
                WorkOrderCountlabel.Text = "0/" + NumWorkOrdersTextBox.Text;
                BatchCountLabel.Text =  "0/" + NumBoxesBox.Text;
                DocCountLabel.Text = "0/" + NumDocsBox.Text;
                BatchNameLlabel.Text = "";
                DatabaseQqueryLabel.Text = "";
                PercentageLabel.Text = "";

                //ProgressBar.Maximum = Convert.ToInt32(NumDocsBox.Text);


                BgrdWorker.RunWorkerAsync();

                //DatabaseQqueryLabel.Text = "Delivey completed !!!";
                //DatabaseQqueryLabel.Refresh();
                //nlogger.Trace("  Delivery Completed !!!!");
            }
            catch (Exception ex)
            {
                //MainForm.ErrorMessage(ex);
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //logger.Error("          Error:  " + ex.Message);
                //logger.Error("          Inner Exception:  " + ex.InnerException);
            }
            Cursor.Current = Cursors.Default;
            nlogger.Trace("Leaving RunExpor Request transaction ...");
        }


        private static long GetAvailableDiskSpace(string strUncPath)
        {
            long lBytesAvailable = -1;
            long lTotalBytes;
            long lTotalFreeBytes;
            IntPtr pszPath = Marshal.StringToHGlobalAuto(strUncPath);
            try
            {
                int iVal = Win32SDK.GetDiskFreeSpaceEx(pszPath, out lBytesAvailable, out lTotalBytes, out lTotalFreeBytes);
            }
            finally
            {
                Marshal.FreeHGlobal(pszPath);
            }
            return lBytesAvailable;
        }

        public static bool IsUncPath(string path)
        {
            return Win32SDK.PathIsUNC(path);
        }

        public class Win32SDK
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            [ResourceExposure(ResourceScope.None)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            internal static extern bool PathIsUNC([MarshalAsAttribute(UnmanagedType.LPWStr), In] string pszPath);

            [DllImport("kernel32.dll", PreserveSig = true, CharSet = CharSet.Auto)]
            public static extern IntPtr CreateFile(string strFileName, int dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, 
                                                    int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

            [DllImport("kernel32.dll", PreserveSig = true, CharSet = CharSet.Auto)]
            public static extern int GetDiskFreeSpaceEx(
                                                       IntPtr lpDirectoryName,                 // directory name
                                                       out long lpFreeBytesAvailable,    // bytes available to caller
                                                       out long lpTotalNumberOfBytes,    // bytes on disk
                                                       out long lpTotalNumberOfFreeBytes // free bytes on disk
                                                       );
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            DialogResult result;
            ResultGeneric resultReport = new ResultGeneric();
            ResultBatches resultBatches = new ResultBatches();
            List<Batch> batches = new List<Batch>();
            List<Batch> batchesSorted = new List<Batch>();
            string workOrders = "";

            try
            {
                Cursor.Current = Cursors.WaitCursor;
               
                nlogger.Trace("Leaving RunExportButton Method ...");
                
                // Navegate trough Work Orders List ....
                foreach (DataGridViewRow row in WorkOrdersList.Rows)
                {
                    // Process only the Work Orders that have been checked ...
                    if (row.Cells["ExportCheckBox"].Value.ToString() == "True")
                    {      
                        if (string.IsNullOrEmpty(workOrders))
                        {
                            workOrders = row.Cells["WorkOrder"].Value.ToString();
                        }
                        else
                        {
                            workOrders = workOrders + "," + row.Cells["WorkOrder"].Value.ToString();
                        }
                    }
                }  // Next Work Order in the List   
                if (string.IsNullOrEmpty(workOrders))
                {
                    MessageBox.Show("No work order has been selected.", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    resultReport = DBTransactions.GenerateWorkOrdersReports(workOrders, true, true);
                }
                
            }
            catch (Exception ex)
            {                
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);              
            }
            Cursor.Current = Cursors.Default;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void ExportedDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ExportedDateCheckBox.Checked)
            {
                ExportedDateTimePickerFrom.Enabled = true;
                ExportedDateTimePickerTo.Enabled = true;
                ShowExportedCheckBox.Checked = true;
                ShowReadyForExportCheckBox.Checked = false;
                //ShowNoReadyForExportCheckBox.Checked = false;
            }
            else
            {
                ExportedDateTimePickerFrom.Enabled = false;
                ExportedDateTimePickerTo.Enabled = false;
            }
        }

        private void ResetSearchButton_Click(object sender, EventArgs e)
        {
            WorkOrdersList.Rows.Clear();
            ExportDirectoryTextBox.Text = "";
            NumWorkOrdersTextBox.Text = "";
            BatchSizeTextBox.Text = "";
            NumBoxesBox.Text = "";
            NumDocsBox.Text = "";
            NumImgTextBox.Text = "";
            WorkOrderCountlabel.Text = "";
            BatchCountLabel.Text = "";
            DocCountLabel.Text = "";
            BatchNameLlabel.Text = "";
            PercentageLabel.Text = "";
            ProgressBar.Visible = false;
            NoDeliveryFilesCheckBox.Checked = false;
            WorkOrderNumber.Text = "";
            WorkOrderCondition.Text = "";
            ShowExportedCheckBox.Checked = false;
            ShowReadyForExportCheckBox.Checked = false;
            ExportedByComboBox.Text = "";
            ExportedDateCheckBox.Checked = false;
            ExportedDateTimePickerFrom.Value = DateTime.Now;
            ExportedDateTimePickerTo.Value = DateTime.Now;
            CustomersComboBox.Text = "";
            JobTypesComboBox.Text = "";
            SortByComboBox.Text = "";
            SortOrderComboBox.Text = "";
            DatabaseQqueryLabel.Text = "";
        }

        private void BgrdWorker_DoWork_1(object sender, DoWorkEventArgs e)
        {
            int numFilesFound = 0;
            int numFilesNotFound = 0;            
            DialogResult result;
            int percent = 0;
            string currentBatchStatus;
            DateTime currentDate = DateTime.Now;



            ExportTransactionsJob exportTransactionsJob = new ExportTransactionsJob();
            ResultExportTransactionsJob resultTransactionJob = new ResultExportTransactionsJob();
            ResultJobsExtended resultJobs = new ResultJobsExtended();
            JobExtended job = new JobExtended();
            BackgroundWorker worker = sender as BackgroundWorker;
            ResultBatches resultBatches = new ResultBatches();
            ResultGeneric resultGeneric = new ResultGeneric();

            try
            {
                // Loop trough the Work Orders List ....
                updateDataGrid = false;
                foreach (DataGridViewRow row in WorkOrdersList.Rows)
                {
                    // Process only the Work Orders that have been checked ...
                    if (row.Cells["ExportCheckBox"].Value.ToString() == "True")
                    {
                        // Update the Number of WorkOrders in progress
                        workOrdersCounter ++;

                       

                        resultJobs = DBTransactions.GetJobByName(row.Cells["JobType"].Value.ToString());
                        if (resultJobs.RecordsCount > 0)
                        {
                            currentWorkOrder = row.Cells["WorkOrder"].Value.ToString();

                            nlogger.Trace("  Getting Transactions for the entire Work Order. Work Order: " + currentWorkOrder);
                            job = resultJobs.ReturnValue[0];
                            generalTransactionMessage = "Getting Delivey Transactions information form Database. Please Wait ...";
                            worker.ReportProgress(percent);

                            if (NoDeliveryFilesCheckBox.Checked)
                            {
                                resultTransactionJob = DBTransactions.GetExportTransactionsJob(job.JobID, Convert.ToInt32(currentWorkOrder), "C:\\Dommie-path");
                            }
                            else
                            {
                                resultTransactionJob = DBTransactions.GetExportTransactionsJob(job.JobID, Convert.ToInt32(currentWorkOrder), ExportDirectoryTextBox.Text);
                            }

                            if (resultTransactionJob.RecordsCount > 0 && resultTransactionJob.ReturnCode == 0)
                            {
                                generalTransactionMessage = "Copying Work Order \"" + currentWorkOrder + "\" files to target location....";
                                worker.ReportProgress(percent);

                                exportTransactionsJob = resultTransactionJob.ReturnValue;

                                // Check if user request is just to export batches witouth delivery them
                                if (!NoDeliveryFilesCheckBox.Checked)
                                {
                                    // 1. Create Directories and Copy Files 
                                    percent = 0;
                                    nlogger.Trace("  Creaing Directories and copying Files to Target location ....");

                                    // Ready to deliver Work Order Documents ..
                                    foreach (ExportBatches batch in exportTransactionsJob.Batches)
                                    {
                                        batchesCounter++;
                                        nlogger.Trace("      Batch Name:" + batch.BatchName);
                                        batchBeenDelivered = batch.BatchName;

                                        foreach (ExportDocs document in batch.Documents)
                                        {
                                            //CircularProgressBar.Value = documentsCount;
                                            documentsCounter++;

                                            nlogger.Trace("          Source File Location:" + document.FileLocation);
                                            nlogger.Trace("          Source File Name:" + document.FileName);
                                            nlogger.Trace("          Target File Location:" + document.TargetFileLocation);
                                            nlogger.Trace("          Target File Name:" + document.TargetFilename);

                                            //ProgressBar.Value = documentsCount;
                                            percent = (int)(((double)(documentsCounter) / (double)(totalDocumentsToDeliver)) * 100);
                                            worker.ReportProgress(percent);

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
                                                File.Copy(document.FileLocation + "\\" + document.FileName, document.TargetFileLocation + "\\" + document.TargetFilename, true);
                                                nlogger.Trace("              File  was copied duccessfully to target directory. ");
                                            }
                                            else
                                            {
                                                numFilesNotFound++;
                                            }
                                        }
                                    }

                                    // Ready to deliver Work Order Associated Metadata Files ..
                                    // 2. Create Metadata Output Directory and Output file
                                    nlogger.Trace("          METADATA FILE CREATION ..............................................");
                                    nlogger.Trace("          Creaing Directories and copying Metadata File to Target location ....");
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
                                }                                

                                // Update Batch Information if no error found and if processing new Work Orders
                                if (ExportSelectionRadioButton.Checked && numFilesNotFound == 0)
                                {
                                    nlogger.Trace("         Work Order Documents deliverd successfuly !!!");
                                    nlogger.Trace("         Updating Batch Information and Tracking Events...");
                                    foreach (ExportBatches batch in exportTransactionsJob.Batches)
                                    {
                                        //Get Batch Information
                                        resultBatches = DBTransactions.GetBatchesInformation("BatchNumber=\"" + batch.BatchName + "\"", "");
                                        currentBatchStatus = resultBatches.ReturnValue[0].StatusFlag;
                                        resultBatches.ReturnValue[0].StatusFlag = "Exported";

                                        resultBatches.ReturnValue[0].ExportedBy = Environment.UserName;
                                        resultBatches.ReturnValue[0].ExportedDate = currentDate;
                                        resultBatches.ReturnValue[0].ExportedTimes = resultBatches.ReturnValue[0].ExportedTimes + 1;

                                        // Update Batch Information
                                        nlogger.Trace("             Updating Batch " + batch.BatchName + " information in Database ...");
                                        resultGeneric = DBTransactions.BatchUpdate(resultBatches.ReturnValue[0]);
                                        if (resultGeneric.ReturnCode == 0)
                                        {
                                            // Batch Event Tracking
                                            nlogger.Trace("             Adding transaction in Tracking Database Table ...");
                                            DBTransactions.BatchTrackingEvent(batch.BatchName, currentBatchStatus, "Exported", "Batch Exported - Work Order " + resultBatches.ReturnValue[0].LotNumber, Environment.UserName);
                                        }
                                    }
                                    // Update rwon in the grid
                                    updateDataGrid = true;
                                    workOrdertoUpdateInGrid = currentWorkOrder;
                                    worker.ReportProgress(percent);                                    
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error getting Export Transactions Job." + Environment.NewLine + resultTransactionJob.Message, "Information ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                nlogger.Trace("  Error getting Export Transactions Job." + resultTransactionJob.Message);
                            }
                        }
                        else
                        {
                            // No Job Type found. This will never happens
                        }
                    }
                } // Next Work Order in the List
                generalTransactionMessage = "Delivery Completed successfully !!!";
                worker.ReportProgress(percent);
            }
            catch (Exception ex)
            {
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BgrdWorker_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            PercentageLabel.Text = ProgressBar.Value.ToString() + "%";
            
            WorkOrderCountlabel.Text = workOrdersCounter .ToString() +  "/" + totalWorOrdersToDeliver.ToString();
            BatchCountLabel.Text = batchesCounter.ToString() + "/" + totalBatchesToDeliver.ToString();
            DocCountLabel.Text = documentsCounter.ToString() + "/" + totalDocumentsToDeliver.ToString();
            BatchNameLlabel.Text = batchBeenDelivered;

            DatabaseQqueryLabel.Text = generalTransactionMessage;

            // Update the Data Grid if Necessary
            if (updateDataGrid)
            {
                foreach(DataGridViewRow row in WorkOrdersList.Rows)
                {
                    if (row.Cells["WorkOrder"].Value.ToString() == workOrdertoUpdateInGrid)
                    {
                        row.Cells["ExportCheckBox"].Value = false;
                        row.DefaultCellStyle.BackColor = Color.LightGray;
                        row.Cells["ExportCheckBox"].ReadOnly = true;
                    }
                }
                updateDataGrid = false;
                workOrdertoUpdateInGrid = "";
            }
        }
    }
}
