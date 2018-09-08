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
using NLog;
using ScanningServicesDataObjects;

namespace ScanningServicesAdmin.Forms
{
    public partial class UsersForm : Form
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public UsersForm()
        {
            InitializeComponent();
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public int ID { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {

            FormLoad();

        }


        private void FormLoad()
        {
            UserNameTextBox.Text = "";
            UserEmailTextBox.Text = "";
            ActiveCheckBox.Checked = false;
            UserTitleTextBox.Text = "";
            UsersListView.Items.Clear();
            AvaliableFunctionalityListView.Items.Clear();

            ResultUIFunctionalities uiFunctionalities = new ResultUIFunctionalities();
            uiFunctionalities = DBTransactions.GetUIFunctionalities();
            foreach (UIFunctionality uiFunctionality in uiFunctionalities.ReturnValue)
            {
                ListViewItem item = new ListViewItem(new string[] { "", Convert.ToString(uiFunctionality.FunctionalityID), uiFunctionality.Description });
                item.Tag = uiFunctionality.FunctionalityID;
                AvaliableFunctionalityListView.Items.Add(item);

            }
            AvaliableFunctionalityListView.Columns[1].Width = 0;

            // Load User List View
            ResultUsers resultUsers = new ResultUsers();
            List<User> users = new List<User>();
            resultUsers = DBTransactions.GetUsers();
            users = resultUsers.ReturnValue;
            string functionalities = "";
            foreach (User user in users)
            {
                if (user.UIFunctionality.Count > 0)
                {
                    List<int> functionalityList = new List<int>();
                    foreach (UIFunctionality functionality in user.UIFunctionality)
                    {
                        functionalityList.Add(functionality.FunctionalityID);
                    }
                    functionalities = String.Join(", ", functionalityList.ToArray());
                }
                else
                {
                    functionalities = "";
                }
                ListViewItem item = new ListViewItem(new string[] { Convert.ToString(user.UserID), Convert.ToString(user.ActiveFlag), user.UserName, user.Title, user.Email, functionalities });
                UsersListView.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AvaliableFunctionalityListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = AvaliableFunctionalityListView.Columns[e.ColumnIndex].Width;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Boolean userFound = false;
            List<string> functionalities = new List<string>();

            // Check for minimun information required to add a user
            // Username and Email Address
            if (string.IsNullOrEmpty(UserNameTextBox.Text))
            {
                MessageBox.Show("User Name is a required field.", "Add User Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (string.IsNullOrEmpty(UserEmailTextBox.Text))
                {
                    MessageBox.Show("User Email Address is a required field.", "Add User Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    for (int count1 = 0; count1 < UsersListView.Items.Count; count1++)
                    {
                        if (UsersListView.Items[count1].SubItems[2].Text == UserNameTextBox.Text)
                        {
                            userFound = true;
                            break;
                        }
                    }
                    if (!userFound)
                    {
                        // Add record to the User List
                        for (int count2 = 0; count2 < AvaliableFunctionalityListView.Items.Count; count2++)
                        {
                            if (AvaliableFunctionalityListView.Items[count2].Checked)
                            {
                                functionalities.Add(AvaliableFunctionalityListView.Items[count2].SubItems[1].Text);
                            }
                        }                            

                        ListViewItem item = new ListViewItem(new string[] { "", Convert.ToString(ActiveCheckBox.Checked), UserNameTextBox.Text, UserTitleTextBox.Text, UserEmailTextBox.Text , String.Join(", ", functionalities.ToArray()) });
                        item.ForeColor = Color.DarkGreen;
                        item.Font = new System.Drawing.Font(item.Font, System.Drawing.FontStyle.Bold);
                        UsersListView.Items.Add(item);
                        //item.SubItems[5].Text
                    }
                    else
                    {
                        MessageBox.Show("User Name already exist.", "Add User Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            UsersListView.SelectedItems.Clear();
        }

        private void UsersListView_Click(object sender, EventArgs e)
        {

            ActiveCheckBox.Checked = false;
            if (UsersListView.SelectedItems[0].SubItems[1].Text == "True")
            {
                ActiveCheckBox.Checked = true;
            }
            UserNameTextBox.Text = UsersListView.SelectedItems[0].SubItems[2].Text;
            UserEmailTextBox.Text = UsersListView.SelectedItems[0].SubItems[4].Text;
            UserTitleTextBox.Text = UsersListView.SelectedItems[0].SubItems[3].Text;

            // Uncheck Items in the Avaliable Functionlaity List
            for (int count1 = 0; count1 < AvaliableFunctionalityListView.Items.Count; count1++)
            {
                AvaliableFunctionalityListView.Items[count1].Checked = false;
            }

            if (!string.IsNullOrEmpty(UsersListView.SelectedItems[0].SubItems[5].Text))
            {
                List<int> functionalities = UsersListView.SelectedItems[0].SubItems[5].Text.Split(',').Select(int.Parse).ToList();

                foreach (int id in functionalities)
                {
                    for (int count1 = 0; count1 < AvaliableFunctionalityListView.Items.Count; count1++)
                    {

                        if (AvaliableFunctionalityListView.Items[count1].SubItems[1].Text == id.ToString())
                        {
                            AvaliableFunctionalityListView.Items[count1].Checked = true;
                            break;
                        }
                    }
                }
            }
            
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (UsersListView.SelectedItems.Count != 0)
            {
                UserNameTextBox.Text = "";
                UserEmailTextBox.Text = "";
                UserTitleTextBox.Text = "";
                ActiveCheckBox.Checked = false;

                if (UsersListView.SelectedItems[0].ForeColor == Color.DarkGreen)
                {
                    // This is a Users that was added in thi ssesson, so remove the item from the list
                    UsersListView.SelectedItems[0].Remove();
                }
                else
                {
                    if (UsersListView.SelectedItems[0].ForeColor == Color.DarkRed)
                    {
                        // THis records has been tagged for deletion so ignore Delete Request
                    }
                    else
                    {
                        UsersListView.SelectedItems[0].ForeColor = Color.DarkRed;
                        UsersListView.SelectedItems[0].Font = new System.Drawing.Font(UsersListView.SelectedItems[0].Font, System.Drawing.FontStyle.Bold);
                    }
                }
            }
            UsersListView.SelectedItems.Clear();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            Boolean userFound = false;
            List<string> functionalities = new List<string>();

            if (UsersListView.SelectedItems.Count != 0)
            {
                // Check if User Name is in the list
                for (int count1 = 0; count1 < UsersListView.Items.Count; count1++)
                {
                    if (UsersListView.Items[count1].SubItems[2].Text == UserNameTextBox.Text)
                    {
                        userFound = true;
                        // Update User List
                        if (ActiveCheckBox.Checked)
                        {
                            UsersListView.SelectedItems[0].SubItems[1].Text = "True";
                        }
                        else
                        {
                            UsersListView.SelectedItems[0].SubItems[1].Text = "False";
                        }
                        UsersListView.SelectedItems[0].SubItems[2].Text = UserNameTextBox.Text;
                        UsersListView.SelectedItems[0].SubItems[4].Text = UserEmailTextBox.Text;
                        UsersListView.SelectedItems[0].SubItems[3].Text = UserTitleTextBox.Text;


                        for (int count2 = 0; count2 < AvaliableFunctionalityListView.Items.Count; count2++)
                        {
                            if (AvaliableFunctionalityListView.Items[count2].Checked)
                            {
                                functionalities.Add(AvaliableFunctionalityListView.Items[count2].SubItems[1].Text);
                            }
                        }
                        UsersListView.SelectedItems[0].SubItems[5].Text = String.Join(", ", functionalities.ToArray());

                        if (UsersListView.SelectedItems[0].ForeColor == Color.DarkGreen || UsersListView.SelectedItems[0].ForeColor == Color.DarkRed)
                        {
                            // Change color only for records that already exist
                            // Update only applies on recordds that already exist
                        }
                        else
                        {
                            UsersListView.SelectedItems[0].ForeColor = Color.DarkViolet;
                            UsersListView.SelectedItems[0].Font = new System.Drawing.Font(UsersListView.SelectedItems[0].Font, System.Drawing.FontStyle.Bold);
                        }

                        break;
                    }
                }
                if (!userFound)
                {
                    MessageBox.Show("Update Request for a User Name that is not in the List.", "Add User Transaction ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            UsersListView.SelectedItems.Clear();
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
                foreach (ListViewItem item in UsersListView.Items)
                {
                    // Check for Deletion Request
                    if (item.ForeColor == Color.DarkRed)
                    {
                        // Delete User from the Database
                        DBTransactions.DeleteUser(Convert.ToInt32(item.SubItems[0].Text));
                        item.Remove();
                    }

                    // Check for New / Update
                    if (item.ForeColor == Color.DarkGreen || item.ForeColor == Color.DarkViolet)
                    {
                        // Add-update user in Databse
                        User user = new User();
                        user.UserName = item.SubItems[2].Text;
                        user.Email = item.SubItems[4].Text;
                        user.Title = item.SubItems[3].Text;
                        if (!string.IsNullOrEmpty(item.SubItems[0].Text))
                        {
                            user.UserID = Convert.ToInt32(item.SubItems[0].Text);
                        }
                        else
                        {
                            user.UserID = 0;
                        }
                        
                        if (item.SubItems[1].Text == "True")
                        {
                            user.ActiveFlag = true;
                        }
                        else
                        {
                            user.ActiveFlag = false;
                        }
                        // Add functionality to the User Data Object
                        List<int> functionalities = new List<int>();
                        if (!string.IsNullOrEmpty(item.SubItems[5].Text))
                        {
                            functionalities = item.SubItems[5].Text.Split(',').Select(int.Parse).ToList();
                            user.UIFunctionality = new List<UIFunctionality>();
                            foreach (int functionality in functionalities)
                            {
                                UIFunctionality userFunctionality = new UIFunctionality();
                                userFunctionality.FunctionalityID = functionality;
                                user.UIFunctionality.Add(userFunctionality);
                            }
                        }

                        if (item.ForeColor == Color.DarkGreen)
                        {
                            // Add User
                            DBTransactions.NewUser(user);
                        }
                        if (item.ForeColor == Color.DarkViolet)
                        {
                            // Update User
                            nlogger.Trace("Updating User Information ..");
                            nlogger.Trace("     Record: " + JsonConvert.SerializeObject(user, Formatting.Indented));                            
                            DBTransactions.UpdateUser(user);
                        }
                        item.ForeColor = Color.Black;
                        item.Font = new System.Drawing.Font(item.Font, System.Drawing.FontStyle.Regular);
                    }

                }

                if (action == "Save")
                {
                    // Rebuild PageSizesListView
                    //buildPageSizeList();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            FormLoad();
        }
    }
}
