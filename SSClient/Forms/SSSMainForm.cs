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
using static ScanningServicesAdmin.Data.GlovalVariables;

namespace ScanningServicesAdmin.Forms
{
    public partial class SSSMainForm : Form
    {
        public SSSMainForm()
        {
            InitializeComponent();
            //this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            AccesstoUIFunctinality();
        }

        public void AccesstoUIFunctinality()
        {
            ConfigurationButton.Enabled = false;
            QualityControlButton.Enabled = false;
            ExportButton.Enabled = false;
            StatusChangeButton.Enabled = false;
            VFRSearchButton.Enabled = false;
            BatchRegistrationButton.Enabled = false;


            // Get User Information to control Accesst to Functionality
            if (Environment.UserName.ToUpper() == "ADMINISTRATOR")
            {
                ConfigurationButton.Enabled = true;
                QualityControlButton.Enabled = true;
                ExportButton.Enabled = true;
                StatusChangeButton.Enabled = true;
                VFRSearchButton.Enabled = true;
                BatchRegistrationButton.Enabled = true;
            }
            else
            {
                ResultUsers resultUsers = new ResultUsers();
                List<User> users = new List<User>();
                resultUsers = DBTransactions.GetUserByName(Environment.UserName);
                users = resultUsers.ReturnValue;
                foreach (UIFunctionality functionality in users[0].UIFunctionality)
                {
                    switch (functionality.Description)
                    {
                        case "System Configuration":
                            ConfigurationButton.Enabled = true;
                            break;
                        case "Quality Control":
                            QualityControlButton.Enabled = true;
                            break;
                        case "Export":
                            ExportButton.Enabled = true;
                            break;
                        case "Batch Administration":
                            StatusChangeButton.Enabled = true;
                            break;
                        case "VFR Search":
                            VFRSearchButton.Enabled = true;
                            break;
                        case "Batch Registration":
                            BatchRegistrationButton.Enabled = true;
                            break;
                    }
                }
            }
            
        }

        private void ControlCenterButton_Click(object sender, EventArgs e)
        {
            BatchControlCenterForm _BatchControlCenterForm = new BatchControlCenterForm();
            _BatchControlCenterForm.StartPosition = FormStartPosition.CenterScreen;
            _BatchControlCenterForm.Show();
        }

        private void ConfigurationButton_Click(object sender, EventArgs e)
        {
            MainForm _MainForm = new MainForm();
            //_MainForm.Show();
            _MainForm.ShowDialog();
            AccesstoUIFunctinality();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            ExportForm _ExportForm = new ExportForm();
            _ExportForm.StartPosition = FormStartPosition.CenterScreen;
            _ExportForm.Show();
        }

        private void QualityControlButton_Click(object sender, EventArgs e)
        {
            QualityControlForm _QualityControlForm = new QualityControlForm();
            _QualityControlForm.StartPosition = FormStartPosition.CenterScreen;
            _QualityControlForm.Show();
        }

        private void StatusChangeButton_Click(object sender, EventArgs e)
        {
            StatusChangeForm _StatusChangeForm = new StatusChangeForm();
            _StatusChangeForm.StartPosition = FormStartPosition.CenterScreen;
            _StatusChangeForm.Show();
        }

        private void SSSMainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
