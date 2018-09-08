namespace ScanningServicesAdmin
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ExitButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.SSSTreeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSizeCategoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ExitButton);
            this.panel1.Controls.Add(this.HelpButton);
            this.panel1.Controls.Add(this.SSSTreeView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(618, 635);
            this.panel1.TabIndex = 0;
            // 
            // ExitButton
            // 
            this.ExitButton.Image = global::ScanningServicesAdmin.Properties.Resources.Exit_32x32;
            this.ExitButton.Location = new System.Drawing.Point(61, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(56, 61);
            this.ExitButton.TabIndex = 35;
            this.ExitButton.Text = "Exit";
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click_1);
            // 
            // HelpButton
            // 
            this.HelpButton.Image = global::ScanningServicesAdmin.Properties.Resources.Help_32x32;
            this.HelpButton.Location = new System.Drawing.Point(3, 3);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(56, 61);
            this.HelpButton.TabIndex = 34;
            this.HelpButton.Text = "Help";
            this.HelpButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.HelpButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // SSSTreeView
            // 
            this.SSSTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SSSTreeView.BackColor = System.Drawing.SystemColors.Window;
            this.SSSTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SSSTreeView.ImageIndex = 0;
            this.SSSTreeView.ImageList = this.imageList;
            this.SSSTreeView.Location = new System.Drawing.Point(4, 69);
            this.SSSTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.SSSTreeView.Name = "SSSTreeView";
            this.SSSTreeView.SelectedImageIndex = 47;
            this.SSSTreeView.Size = new System.Drawing.Size(608, 560);
            this.SSSTreeView.TabIndex = 0;
            this.SSSTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.SSSTreeView_BeforeExpand);
            this.SSSTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.SSSTreeView_AfterExpand);
            this.SSSTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SSSTreeView_MouseDown);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "ScanningServicesOperation-V1.png");
            this.imageList.Images.SetKeyName(1, "CustomerList-V2.png");
            this.imageList.Images.SetKeyName(2, "Reports-V1.png");
            this.imageList.Images.SetKeyName(3, "Customer-V1.png");
            this.imageList.Images.SetKeyName(4, "Process.png");
            this.imageList.Images.SetKeyName(5, "Jobs.png");
            this.imageList.Images.SetKeyName(6, "Job-V1.png");
            this.imageList.Images.SetKeyName(7, "Workflow.png");
            this.imageList.Images.SetKeyName(8, "VFRProcesses.png");
            this.imageList.Images.SetKeyName(9, "Project-V2.png");
            this.imageList.Images.SetKeyName(10, "Projects.png");
            this.imageList.Images.SetKeyName(11, "Fields.png");
            this.imageList.Images.SetKeyName(12, "Field.png");
            this.imageList.Images.SetKeyName(13, "BlueFilledCircle.png");
            this.imageList.Images.SetKeyName(14, "DarkGreenCircle.png");
            this.imageList.Images.SetKeyName(15, "FlorecentGreenCircle.png");
            this.imageList.Images.SetKeyName(16, "GreeFilledCircle-V1.png");
            this.imageList.Images.SetKeyName(17, "GreyFilledCircle.png");
            this.imageList.Images.SetKeyName(18, "CPField-V1.png");
            this.imageList.Images.SetKeyName(19, "CPVFRField-V1.png");
            this.imageList.Images.SetKeyName(20, "Report.png");
            this.imageList.Images.SetKeyName(21, "Report-Disable-V1.png");
            this.imageList.Images.SetKeyName(22, "Report-Incomplete-V1.png");
            this.imageList.Images.SetKeyName(23, "Report-Enable-V1.png");
            this.imageList.Images.SetKeyName(24, "Services.png");
            this.imageList.Images.SetKeyName(25, "GeneralSettings.png");
            this.imageList.Images.SetKeyName(26, "PDFConversionSites.png");
            this.imageList.Images.SetKeyName(27, "SMTPSettings.png");
            this.imageList.Images.SetKeyName(28, "Locations.png");
            this.imageList.Images.SetKeyName(29, "OutLineRedCircle-V1.png");
            this.imageList.Images.SetKeyName(30, "ServiceStations.png");
            this.imageList.Images.SetKeyName(31, "ServiceStation-V1.png");
            this.imageList.Images.SetKeyName(32, "Service-Gray.png");
            this.imageList.Images.SetKeyName(33, "Service-Red.png");
            this.imageList.Images.SetKeyName(34, "Service-Green.png");
            this.imageList.Images.SetKeyName(35, "GreeFilledCircleAll.png");
            this.imageList.Images.SetKeyName(36, "GreenFilledCircleAll-V1.png");
            this.imageList.Images.SetKeyName(37, "Service-Clock-Gray.png");
            this.imageList.Images.SetKeyName(38, "Service-Clock-Green.png");
            this.imageList.Images.SetKeyName(39, "Service-Clock-Red.png");
            this.imageList.Images.SetKeyName(40, "Service-Clock-Green-V2.png");
            this.imageList.Images.SetKeyName(41, "Service-Clock-Gray-V2.png");
            this.imageList.Images.SetKeyName(42, "Service-Clock-Red-V2.png");
            this.imageList.Images.SetKeyName(43, "Users.png");
            this.imageList.Images.SetKeyName(44, "ReportTemplates.png");
            this.imageList.Images.SetKeyName(45, "CustomerList.png");
            this.imageList.Images.SetKeyName(46, "Customer.png");
            this.imageList.Images.SetKeyName(47, "ScanningServicesOperation.png");
            this.imageList.Images.SetKeyName(48, "Job.png");
            this.imageList.Images.SetKeyName(49, "Project.png");
            this.imageList.Images.SetKeyName(50, "Reports.png");
            this.imageList.Images.SetKeyName(51, "Report-Disable.png");
            this.imageList.Images.SetKeyName(52, "Report-Incomplete.png");
            this.imageList.Images.SetKeyName(53, "Report-Enable.png");
            this.imageList.Images.SetKeyName(54, "CPField.png");
            this.imageList.Images.SetKeyName(55, "CPVFRField-V2.png");
            this.imageList.Images.SetKeyName(56, "CPVFRField.png");
            this.imageList.Images.SetKeyName(57, "GreeFilledCircle.png");
            this.imageList.Images.SetKeyName(58, "GreenFilledCircleAll.png");
            this.imageList.Images.SetKeyName(59, "GreyPause-16x16.png");
            this.imageList.Images.SetKeyName(60, "OutLineRedCircle-V2.png");
            this.imageList.Images.SetKeyName(61, "OutLineRedCircle.png");
            this.imageList.Images.SetKeyName(62, "PDF-Station.png");
            this.imageList.Images.SetKeyName(63, "PDFStation.png");
            this.imageList.Images.SetKeyName(64, "ServiceStation.png");
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.newToolStripMenuItem,
            this.pageSizeCategoriesToolStripMenuItem,
            this.importFieldsToolStripMenuItem,
            this.toolStripSeparator,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(183, 142);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Image = global::ScanningServicesAdmin.Properties.Resources.Properties;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::ScanningServicesAdmin.Properties.Resources.New;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // pageSizeCategoriesToolStripMenuItem
            // 
            this.pageSizeCategoriesToolStripMenuItem.Image = global::ScanningServicesAdmin.Properties.Resources.Category_16x16;
            this.pageSizeCategoriesToolStripMenuItem.Name = "pageSizeCategoriesToolStripMenuItem";
            this.pageSizeCategoriesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.pageSizeCategoriesToolStripMenuItem.Text = "Page Size Categories";
            this.pageSizeCategoriesToolStripMenuItem.Click += new System.EventHandler(this.pageSizeCategoriesToolStripMenuItem_Click);
            // 
            // importFieldsToolStripMenuItem
            // 
            this.importFieldsToolStripMenuItem.Name = "importFieldsToolStripMenuItem";
            this.importFieldsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.importFieldsToolStripMenuItem.Text = "Import Fields";
            this.importFieldsToolStripMenuItem.Click += new System.EventHandler(this.importFieldsToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(179, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::ScanningServicesAdmin.Properties.Resources.Delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 635);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCANNING  SERVICES ADMINISTRATION";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView SSSTreeView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.ToolStripMenuItem importFieldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageSizeCategoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
    }
}

