using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanningServicesAdmin.Forms
{
    public partial class PDFConversionForm : Form
    {
        public PDFConversionForm()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e)
        {

        }
    }
}
