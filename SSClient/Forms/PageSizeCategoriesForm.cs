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
//using static ScanningServicesAdmin.Data.GlovalVariables;

namespace ScanningServicesAdmin.Forms
{
    public partial class PageSizeCategoriesForm : Form
    {

        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public List<JobPageSize> originalJobPageSizes = new List<JobPageSize>();
        public PageSizeCategoriesForm()
        {
            InitializeComponent();
        }

        private void PageSizeCategoriesForm_Load(object sender, EventArgs e)
        {
            PageSizesListView.Columns[0].Width = 0;
            JobNameLabel.Text = Data.GlovalVariables.currentJobName;
            buildPageSizeList();
        }

        private void buildPageSizeList()
        {
            ResultJobPageSizes resultPageSizes = new ResultJobPageSizes();
            resultPageSizes = DBTransactions.GetPageSizesByJobID(Data.GlovalVariables.currentJobID);
            PageSizesListView.Items.Clear();
            if (resultPageSizes.RecordsCount != 0)
            {
                foreach (JobPageSize jobPageSize in resultPageSizes.ReturnValue)
                {
                    string[] row = { jobPageSize.ID.ToString(), jobPageSize.CategoryName, jobPageSize.High.ToString(), jobPageSize.Width.ToString() };
                    var listViewItem = new ListViewItem(row);
                    PageSizesListView.Items.Add(listViewItem);

                    JobPageSize jobPageSizeItem = new JobPageSize();
                    jobPageSizeItem.ID = jobPageSize.ID;
                    jobPageSizeItem.JobID = jobPageSize.JobID;
                    jobPageSizeItem.CategoryName = jobPageSize.CategoryName;
                    jobPageSizeItem.Width = jobPageSize.Width;
                    jobPageSizeItem.High = jobPageSize.High;
                    originalJobPageSizes.Add(jobPageSizeItem);
                }
            }

            ResetButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WidthTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateTextDoubleInTextBox(sender, e);
        }

        private void HighValueTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Validation.validateTextDoubleInTextBox(sender, e);
        }

        private void CategoryNameTextBox_Leave(object sender, EventArgs e)
        {
            ResetButtons();
        }

        private void HighValueTextBox_Leave(object sender, EventArgs e)
        {
            ResetButtons();
        }

        private void WidthTextBox_Leave(object sender, EventArgs e)
        {
            ResetButtons();
        }

        private void ResetButtons()
        {
            AddButton.Enabled = false;
            UpdateButton.Enabled = false;
            RemoveButton.Enabled = false;

            if (CategoryNameTextBox.Enabled)
            {
                // We are in Add New Mode
                if (!string.IsNullOrEmpty(WidthTextBox.Text) && !string.IsNullOrEmpty(High.Text))
                {
                    AddButton.Enabled = true;
                }
            }
            else
            {
                // We are in Deletion an/or Update Mode
                RemoveButton.Enabled = true;
                if (!string.IsNullOrEmpty(WidthTextBox.Text) && !string.IsNullOrEmpty(High.Text))
                {
                    UpdateButton.Enabled = true;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Boolean categoryExist = false;
            foreach (ListViewItem item in PageSizesListView.Items)
            {
                if (item.SubItems[1].Text == CategoryNameTextBox.Text)
                {
                    // Category already exist
                    MessageBox.Show("Category \"" + CategoryNameTextBox.Text + "\" already exist", "Adding Page Size Category ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    categoryExist = true;
                    break;
                }
                if (item.SubItems[2].Text == HighValueTextBox.Text && item.SubItems[3].Text == WidthTextBox.Text)
                {
                    MessageBox.Show("High and Widh values already exist.", "Adding Page Size Category ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    categoryExist = true;
                    break;
                }
            }
            if (!categoryExist)
            {
                string[] row = { "", CategoryNameTextBox.Text, HighValueTextBox.Text, WidthTextBox.Text };
                var listViewItem = new ListViewItem(row);
                listViewItem.ForeColor = Color.DarkGreen;
                listViewItem.Font = new Font(PageSizesListView.Font, FontStyle.Bold);
                PageSizesListView.Items.Add(listViewItem);
            }
        }

        private void PageSizesListView_Click(object sender, EventArgs e)
        {
            if (PageSizesListView.SelectedItems.Count != 0)
            {
                AddButton.Enabled = false;
                UpdateButton.Enabled = true;
                RemoveButton.Enabled = true;
                NewNameTextBox.Enabled = true;
                CategoryNameTextBox.Enabled = false;
                NewNameLabel.Visible = true;

                ListViewItem item = new ListViewItem();
                item = PageSizesListView.SelectedItems[0];

                CategoryNameTextBox.Text = item.SubItems[1].Text;
                HighValueTextBox.Text = item.SubItems[2].Text;
                WidthTextBox.Text = item.SubItems[3].Text;
            }
        }

        private void ClearForm()
        {
            AddButton.Enabled = false;
            UpdateButton.Enabled = false;
            RemoveButton.Enabled = false;
            NewNameTextBox.Enabled = false;
            CategoryNameTextBox.Enabled = true;

            NewNameLabel.Visible = false;
            WidthTextBox.Text = "";
            HighValueTextBox.Text = "";
            CategoryNameTextBox.Text = "";
            NewNameTextBox.Text = "";
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Boolean continueUpdate = true;
            Boolean updatingCategoryName = false;
            // Check if user is updating only the High and Widh
            if (string.IsNullOrEmpty(NewNameTextBox.Text))
            {
                foreach (ListViewItem item in PageSizesListView.Items)
                {
                    if (item.SubItems[1].Text == CategoryNameTextBox.Text)
                    {
                        // Category was foun in the list
                        // 
                        if (item.SubItems[2].Text == HighValueTextBox.Text && item.SubItems[3].Text == WidthTextBox.Text)
                        {
                            MessageBox.Show("No Changes in the High or/and Widh values for this category.", "Updating Page Size Category ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            continueUpdate = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                updatingCategoryName = true;
                // Make sure the new name does not exist in the list view
                foreach (ListViewItem item in PageSizesListView.Items)
                {
                    if (item.SubItems[1].Text == NewNameTextBox.Text)
                    {
                        MessageBox.Show("Category \"" + NewNameTextBox.Text + "\" already exist", "Adding Page Size Category ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        continueUpdate = true;
                        break;
                    }
                }
            }

            // Check that Widh and High do not exist in the list view
            if (continueUpdate && !updatingCategoryName)
            {
                foreach (ListViewItem item in PageSizesListView.Items)
                {
                    if (item.SubItems[2].Text == HighValueTextBox.Text && item.SubItems[3].Text == WidthTextBox.Text)
                    {
                        MessageBox.Show("High and Widh values already exist.", "Updating Page Size Category ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        continueUpdate = false;
                        break;
                    }
                }
            }

            //Update Category
            if (continueUpdate)
            {
                foreach (ListViewItem item in PageSizesListView.Items)
                {
                    if (item.SubItems[1].Text == CategoryNameTextBox.Text || item.SubItems[1].Text == NewNameTextBox.Text)
                    {
                        //Update Item in the list View
                        if (!string.IsNullOrEmpty(NewNameTextBox.Text))
                        {
                            item.SubItems[1].Text = NewNameTextBox.Text;
                        }
                        item.SubItems[2].Text = HighValueTextBox.Text;
                        item.SubItems[3].Text = WidthTextBox.Text;
                        if (item.ForeColor == Color.DarkGreen)
                        {
                            // Keep the Green Color
                        }
                        else
                        {
                            // Change the Color to blue (Blue will be interpreted as an Update)
                            item.ForeColor = Color.DarkViolet;
                            item.Font = new Font(PageSizesListView.Font, FontStyle.Bold);
                        }
                    }
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (PageSizesListView.SelectedItems.Count != 0)
            {
                ListViewItem item = new ListViewItem();
                item = PageSizesListView.SelectedItems[0];

                if (item.ForeColor == Color.DarkGreen)
                {
                    // This Item has not bee uploaded into the Database yet so remove the item from the list
                    PageSizesListView.SelectedItems[0].Remove();
                }
                else
                {
                    if (item.ForeColor == Color.DarkRed)
                    {
                        // Item has been already tagged for deletion ,so ignore request
                    }
                    else
                    {
                        // Mark Record for deletion
                        item.ForeColor = Color.DarkRed;
                        item.Font = new Font(PageSizesListView.Font, FontStyle.Bold);
                    }
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            PageSizesListView.Items.Clear();

            foreach (JobPageSize jobPageSize in originalJobPageSizes)
            {
                string[] row = { jobPageSize.ID.ToString(), jobPageSize.CategoryName, jobPageSize.High.ToString(), jobPageSize.Width.ToString() };
                var listViewItem = new ListViewItem(row);
                PageSizesListView.Items.Add(listViewItem);
            }
            ClearForm();
            ResetButtons();
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
                foreach (ListViewItem item in PageSizesListView.Items)
                {
                    // Check for Deletion
                    if (item.ForeColor == Color.DarkRed)
                    {
                        // Delete Record
                        DBTransactions.DeleteJobPageSize(Convert.ToInt32(item.SubItems[0].Text));
                    }

                    // Check for New
                    if (item.ForeColor == Color.DarkGreen)
                    {
                        // Add Record
                        JobPageSize jobPageZise = new JobPageSize();
                        jobPageZise.JobID = Data.GlovalVariables.currentJobID;
                        jobPageZise.CategoryName = item.SubItems[1].Text;
                        jobPageZise.High = Convert.ToDouble(item.SubItems[2].Text);
                        jobPageZise.Width = Convert.ToDouble(item.SubItems[3].Text);
                        DBTransactions.NewJobPageSize(jobPageZise);
                    }

                    //Check for Update
                    if (item.ForeColor == Color.DarkViolet)
                    {
                        // Update Record
                        JobPageSize jobPageZise = new JobPageSize();
                        jobPageZise.ID = Convert.ToInt32(item.SubItems[0].Text);
                        jobPageZise.JobID = Data.GlovalVariables.currentJobID;
                        jobPageZise.CategoryName = item.SubItems[1].Text;
                        jobPageZise.High = Convert.ToDouble(item.SubItems[2].Text);
                        jobPageZise.Width = Convert.ToDouble(item.SubItems[3].Text);
                        DBTransactions.UpdateJobPageSize(jobPageZise);
                    }
                }

                if (action == "Save")
                {
                    // Rebuild PageSizesListView
                    buildPageSizeList();
                }
                else
                {
                    this.Close();
                }               
            }
            catch (Exception ex)
            {
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
