namespace ScanningServicesAdmin.Forms
{
    partial class BatchDocumentViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchDocumentViewerForm));
            this.BatchMetadataListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DocsMetadataListView = new System.Windows.Forms.ListView();
            this.Scope = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FieldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FieldValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.JobTypeLabel = new System.Windows.Forms.Label();
            this.BatchNameLabel = new System.Windows.Forms.Label();
            this.DocNumberTextBox = new System.Windows.Forms.TextBox();
            this.PDFPanel = new System.Windows.Forms.Panel();
            this.PDFFile = new AxAcroPDFLib.AxAcroPDF();
            this.TIFFPanel = new System.Windows.Forms.Panel();
            this.ImagePageTextBox = new System.Windows.Forms.TextBox();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.LastButton = new System.Windows.Forms.Button();
            this.FirstButton = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.FirstImagePageButton = new System.Windows.Forms.Button();
            this.LastImagePageButton = new System.Windows.Forms.Button();
            this.NextPageButton = new System.Windows.Forms.Button();
            this.PrviousPageButton = new System.Windows.Forms.Button();
            this.ReportButton = new System.Windows.Forms.Button();
            this.ResetSearchButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.MetadataPpanel = new System.Windows.Forms.Panel();
            this.ViewerPpanel = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.DocViewerSubPanel = new System.Windows.Forms.Panel();
            this.ImageControlGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.PDFPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PDFFile)).BeginInit();
            this.TIFFPanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.MetadataPpanel.SuspendLayout();
            this.ViewerPpanel.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.DocViewerSubPanel.SuspendLayout();
            this.ImageControlGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // BatchMetadataListView
            // 
            this.BatchMetadataListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BatchMetadataListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.BatchMetadataListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchMetadataListView.FullRowSelect = true;
            this.BatchMetadataListView.HideSelection = false;
            this.BatchMetadataListView.LabelEdit = true;
            this.BatchMetadataListView.Location = new System.Drawing.Point(5, 110);
            this.BatchMetadataListView.MultiSelect = false;
            this.BatchMetadataListView.Name = "BatchMetadataListView";
            this.BatchMetadataListView.Size = new System.Drawing.Size(424, 171);
            this.BatchMetadataListView.TabIndex = 78;
            this.BatchMetadataListView.UseCompatibleStateImageBehavior = false;
            this.BatchMetadataListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Scope";
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Field Name";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            this.columnHeader3.Width = 270;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Location = new System.Drawing.Point(5, 278);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(423, 37);
            this.groupBox3.TabIndex = 75;
            this.groupBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(114, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "DOCUMENT  METADATA";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(6, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 37);
            this.groupBox1.TabIndex = 74;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "BATCH METADATA";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DocsMetadataListView
            // 
            this.DocsMetadataListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DocsMetadataListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Scope,
            this.FieldName,
            this.FieldValue});
            this.DocsMetadataListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocsMetadataListView.FullRowSelect = true;
            this.DocsMetadataListView.HideSelection = false;
            this.DocsMetadataListView.LabelEdit = true;
            this.DocsMetadataListView.Location = new System.Drawing.Point(4, 319);
            this.DocsMetadataListView.MultiSelect = false;
            this.DocsMetadataListView.Name = "DocsMetadataListView";
            this.DocsMetadataListView.Size = new System.Drawing.Size(427, 303);
            this.DocsMetadataListView.TabIndex = 71;
            this.DocsMetadataListView.UseCompatibleStateImageBehavior = false;
            this.DocsMetadataListView.View = System.Windows.Forms.View.Details;
            // 
            // Scope
            // 
            this.Scope.Text = "Scope";
            this.Scope.Width = 0;
            // 
            // FieldName
            // 
            this.FieldName.Text = "Field Name";
            this.FieldName.Width = 150;
            // 
            // FieldValue
            // 
            this.FieldValue.Text = "Value";
            this.FieldValue.Width = 270;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.JobTypeLabel);
            this.groupBox2.Controls.Add(this.BatchNameLabel);
            this.groupBox2.Location = new System.Drawing.Point(7, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(420, 70);
            this.groupBox2.TabIndex = 77;
            this.groupBox2.TabStop = false;
            // 
            // JobTypeLabel
            // 
            this.JobTypeLabel.AutoSize = true;
            this.JobTypeLabel.ForeColor = System.Drawing.Color.Navy;
            this.JobTypeLabel.Location = new System.Drawing.Point(109, 41);
            this.JobTypeLabel.Name = "JobTypeLabel";
            this.JobTypeLabel.Size = new System.Drawing.Size(45, 16);
            this.JobTypeLabel.TabIndex = 76;
            this.JobTypeLabel.Text = "label5";
            // 
            // BatchNameLabel
            // 
            this.BatchNameLabel.AutoSize = true;
            this.BatchNameLabel.ForeColor = System.Drawing.Color.Navy;
            this.BatchNameLabel.Location = new System.Drawing.Point(109, 17);
            this.BatchNameLabel.Name = "BatchNameLabel";
            this.BatchNameLabel.Size = new System.Drawing.Size(45, 16);
            this.BatchNameLabel.TabIndex = 75;
            this.BatchNameLabel.Text = "label4";
            // 
            // DocNumberTextBox
            // 
            this.DocNumberTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DocNumberTextBox.Location = new System.Drawing.Point(170, 18);
            this.DocNumberTextBox.Name = "DocNumberTextBox";
            this.DocNumberTextBox.ReadOnly = true;
            this.DocNumberTextBox.Size = new System.Drawing.Size(81, 22);
            this.DocNumberTextBox.TabIndex = 196;
            this.DocNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PDFPanel
            // 
            this.PDFPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PDFPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PDFPanel.Controls.Add(this.PDFFile);
            this.PDFPanel.Location = new System.Drawing.Point(8, 29);
            this.PDFPanel.Name = "PDFPanel";
            this.PDFPanel.Size = new System.Drawing.Size(524, 296);
            this.PDFPanel.TabIndex = 245;
            // 
            // PDFFile
            // 
            this.PDFFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PDFFile.Enabled = true;
            this.PDFFile.Location = new System.Drawing.Point(0, 0);
            this.PDFFile.Name = "PDFFile";
            this.PDFFile.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PDFFile.OcxState")));
            this.PDFFile.Size = new System.Drawing.Size(522, 294);
            this.PDFFile.TabIndex = 0;
            // 
            // TIFFPanel
            // 
            this.TIFFPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TIFFPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TIFFPanel.Controls.Add(this.PictureBox);
            this.TIFFPanel.Location = new System.Drawing.Point(10, 342);
            this.TIFFPanel.Name = "TIFFPanel";
            this.TIFFPanel.Size = new System.Drawing.Size(522, 263);
            this.TIFFPanel.TabIndex = 246;
            // 
            // ImagePageTextBox
            // 
            this.ImagePageTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ImagePageTextBox.Location = new System.Drawing.Point(220, 18);
            this.ImagePageTextBox.Name = "ImagePageTextBox";
            this.ImagePageTextBox.ReadOnly = true;
            this.ImagePageTextBox.Size = new System.Drawing.Size(81, 22);
            this.ImagePageTextBox.TabIndex = 249;
            this.ImagePageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.Controls.Add(this.splitContainer);
            this.MainPanel.Location = new System.Drawing.Point(12, 87);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(981, 682);
            this.MainPanel.TabIndex = 252;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 78;
            this.label3.Text = "Job Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 16);
            this.label2.TabIndex = 77;
            this.label2.Text = "Batch Name:";
            // 
            // PreviousButton
            // 
            this.PreviousButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PreviousButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PreviousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousButton.Image = ((System.Drawing.Image)(resources.GetObject("PreviousButton.Image")));
            this.PreviousButton.Location = new System.Drawing.Point(132, 14);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(30, 30);
            this.PreviousButton.TabIndex = 194;
            this.PreviousButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.NextButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextButton.Image = ((System.Drawing.Image)(resources.GetObject("NextButton.Image")));
            this.NextButton.Location = new System.Drawing.Point(258, 15);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(30, 30);
            this.NextButton.TabIndex = 193;
            this.NextButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // LastButton
            // 
            this.LastButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LastButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LastButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastButton.Image = ((System.Drawing.Image)(resources.GetObject("LastButton.Image")));
            this.LastButton.Location = new System.Drawing.Point(294, 15);
            this.LastButton.Name = "LastButton";
            this.LastButton.Size = new System.Drawing.Size(30, 30);
            this.LastButton.TabIndex = 192;
            this.LastButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.LastButton.UseVisualStyleBackColor = true;
            this.LastButton.Click += new System.EventHandler(this.LastButton_Click);
            // 
            // FirstButton
            // 
            this.FirstButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FirstButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.FirstButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstButton.Image = ((System.Drawing.Image)(resources.GetObject("FirstButton.Image")));
            this.FirstButton.Location = new System.Drawing.Point(96, 15);
            this.FirstButton.Name = "FirstButton";
            this.FirstButton.Size = new System.Drawing.Size(30, 30);
            this.FirstButton.TabIndex = 195;
            this.FirstButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.FirstButton.UseVisualStyleBackColor = true;
            this.FirstButton.Click += new System.EventHandler(this.FirstButton_Click);
            // 
            // PictureBox
            // 
            this.PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox.Location = new System.Drawing.Point(0, 0);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(520, 261);
            this.PictureBox.TabIndex = 3;
            this.PictureBox.TabStop = false;
            // 
            // FirstImagePageButton
            // 
            this.FirstImagePageButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FirstImagePageButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.FirstImagePageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstImagePageButton.Image = ((System.Drawing.Image)(resources.GetObject("FirstImagePageButton.Image")));
            this.FirstImagePageButton.Location = new System.Drawing.Point(146, 15);
            this.FirstImagePageButton.Name = "FirstImagePageButton";
            this.FirstImagePageButton.Size = new System.Drawing.Size(30, 30);
            this.FirstImagePageButton.TabIndex = 252;
            this.FirstImagePageButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.FirstImagePageButton.UseVisualStyleBackColor = true;
            this.FirstImagePageButton.Click += new System.EventHandler(this.FirstImagePageButton_Click);
            // 
            // LastImagePageButton
            // 
            this.LastImagePageButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LastImagePageButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LastImagePageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastImagePageButton.Image = ((System.Drawing.Image)(resources.GetObject("LastImagePageButton.Image")));
            this.LastImagePageButton.Location = new System.Drawing.Point(341, 15);
            this.LastImagePageButton.Name = "LastImagePageButton";
            this.LastImagePageButton.Size = new System.Drawing.Size(30, 30);
            this.LastImagePageButton.TabIndex = 251;
            this.LastImagePageButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.LastImagePageButton.UseVisualStyleBackColor = true;
            this.LastImagePageButton.Click += new System.EventHandler(this.LastImagePageButton_Click);
            // 
            // NextPageButton
            // 
            this.NextPageButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.NextPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NextPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextPageButton.Image = ((System.Drawing.Image)(resources.GetObject("NextPageButton.Image")));
            this.NextPageButton.Location = new System.Drawing.Point(307, 14);
            this.NextPageButton.Name = "NextPageButton";
            this.NextPageButton.Size = new System.Drawing.Size(30, 30);
            this.NextPageButton.TabIndex = 247;
            this.NextPageButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.NextPageButton.UseVisualStyleBackColor = true;
            this.NextPageButton.Click += new System.EventHandler(this.NextPageButton_Click);
            // 
            // PrviousPageButton
            // 
            this.PrviousPageButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.PrviousPageButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PrviousPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrviousPageButton.Image = ((System.Drawing.Image)(resources.GetObject("PrviousPageButton.Image")));
            this.PrviousPageButton.Location = new System.Drawing.Point(184, 14);
            this.PrviousPageButton.Name = "PrviousPageButton";
            this.PrviousPageButton.Size = new System.Drawing.Size(30, 30);
            this.PrviousPageButton.TabIndex = 248;
            this.PrviousPageButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.PrviousPageButton.UseVisualStyleBackColor = true;
            this.PrviousPageButton.Click += new System.EventHandler(this.PrviousPageButton_Click);
            // 
            // ReportButton
            // 
            this.ReportButton.Enabled = false;
            this.ReportButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ReportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportButton.Image = ((System.Drawing.Image)(resources.GetObject("ReportButton.Image")));
            this.ReportButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ReportButton.Location = new System.Drawing.Point(76, 4);
            this.ReportButton.Name = "ReportButton";
            this.ReportButton.Size = new System.Drawing.Size(68, 75);
            this.ReportButton.TabIndex = 244;
            this.ReportButton.Text = "\r\n\r\nApply";
            this.ReportButton.UseVisualStyleBackColor = true;
            // 
            // ResetSearchButton
            // 
            this.ResetSearchButton.Enabled = false;
            this.ResetSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ResetSearchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetSearchButton.Image = ((System.Drawing.Image)(resources.GetObject("ResetSearchButton.Image")));
            this.ResetSearchButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ResetSearchButton.Location = new System.Drawing.Point(5, 4);
            this.ResetSearchButton.Name = "ResetSearchButton";
            this.ResetSearchButton.Size = new System.Drawing.Size(68, 75);
            this.ResetSearchButton.TabIndex = 243;
            this.ResetSearchButton.Text = "\r\n\r\nSave";
            this.ResetSearchButton.UseVisualStyleBackColor = true;
            // 
            // ExitButton
            // 
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Image = global::ScanningServicesAdmin.Properties.Resources.Exit_32x322;
            this.ExitButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ExitButton.Location = new System.Drawing.Point(147, 4);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(68, 75);
            this.ExitButton.TabIndex = 242;
            this.ExitButton.Text = "\r\nExit\r\n\r\n";
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.MetadataPpanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.ViewerPpanel);
            this.splitContainer.Size = new System.Drawing.Size(981, 682);
            this.splitContainer.SplitterDistance = 433;
            this.splitContainer.TabIndex = 0;
            // 
            // MetadataPpanel
            // 
            this.MetadataPpanel.Controls.Add(this.groupBox4);
            this.MetadataPpanel.Controls.Add(this.groupBox2);
            this.MetadataPpanel.Controls.Add(this.groupBox1);
            this.MetadataPpanel.Controls.Add(this.BatchMetadataListView);
            this.MetadataPpanel.Controls.Add(this.groupBox3);
            this.MetadataPpanel.Controls.Add(this.DocsMetadataListView);
            this.MetadataPpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetadataPpanel.Location = new System.Drawing.Point(0, 0);
            this.MetadataPpanel.Name = "MetadataPpanel";
            this.MetadataPpanel.Size = new System.Drawing.Size(433, 682);
            this.MetadataPpanel.TabIndex = 0;
            // 
            // ViewerPpanel
            // 
            this.ViewerPpanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewerPpanel.Controls.Add(this.ImageControlGroupBox);
            this.ViewerPpanel.Controls.Add(this.DocViewerSubPanel);
            this.ViewerPpanel.Location = new System.Drawing.Point(0, 0);
            this.ViewerPpanel.Name = "ViewerPpanel";
            this.ViewerPpanel.Size = new System.Drawing.Size(544, 680);
            this.ViewerPpanel.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.PreviousButton);
            this.groupBox4.Controls.Add(this.DocNumberTextBox);
            this.groupBox4.Controls.Add(this.NextButton);
            this.groupBox4.Controls.Add(this.FirstButton);
            this.groupBox4.Controls.Add(this.LastButton);
            this.groupBox4.Location = new System.Drawing.Point(7, 623);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(424, 51);
            this.groupBox4.TabIndex = 79;
            this.groupBox4.TabStop = false;
            // 
            // DocViewerSubPanel
            // 
            this.DocViewerSubPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DocViewerSubPanel.Controls.Add(this.TIFFPanel);
            this.DocViewerSubPanel.Controls.Add(this.PDFPanel);
            this.DocViewerSubPanel.Location = new System.Drawing.Point(3, -1);
            this.DocViewerSubPanel.Name = "DocViewerSubPanel";
            this.DocViewerSubPanel.Size = new System.Drawing.Size(538, 621);
            this.DocViewerSubPanel.TabIndex = 247;
            // 
            // ImageControlGroupBox
            // 
            this.ImageControlGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageControlGroupBox.Controls.Add(this.FirstImagePageButton);
            this.ImageControlGroupBox.Controls.Add(this.ImagePageTextBox);
            this.ImageControlGroupBox.Controls.Add(this.LastImagePageButton);
            this.ImageControlGroupBox.Controls.Add(this.PrviousPageButton);
            this.ImageControlGroupBox.Controls.Add(this.NextPageButton);
            this.ImageControlGroupBox.Location = new System.Drawing.Point(4, 622);
            this.ImageControlGroupBox.Name = "ImageControlGroupBox";
            this.ImageControlGroupBox.Size = new System.Drawing.Size(535, 51);
            this.ImageControlGroupBox.TabIndex = 248;
            this.ImageControlGroupBox.TabStop = false;
            // 
            // BatchDocumentViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 781);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.ReportButton);
            this.Controls.Add(this.ResetSearchButton);
            this.Controls.Add(this.ExitButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BatchDocumentViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Document Viewer";
            this.Load += new System.EventHandler(this.BatchDocumentViewerForm_Load);
            this.Resize += new System.EventHandler(this.BatchDocumentViewerForm_Resize);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.PDFPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PDFFile)).EndInit();
            this.TIFFPanel.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.MetadataPpanel.ResumeLayout(false);
            this.ViewerPpanel.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.DocViewerSubPanel.ResumeLayout(false);
            this.ImageControlGroupBox.ResumeLayout(false);
            this.ImageControlGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView DocsMetadataListView;
        private System.Windows.Forms.ColumnHeader FieldName;
        private System.Windows.Forms.ColumnHeader FieldValue;
        internal System.Windows.Forms.Button LastButton;
        internal System.Windows.Forms.Button NextButton;
        internal System.Windows.Forms.Button PreviousButton;
        internal System.Windows.Forms.Button FirstButton;
        private System.Windows.Forms.TextBox DocNumberTextBox;
        internal System.Windows.Forms.Button ReportButton;
        internal System.Windows.Forms.Button ResetSearchButton;
        internal System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.ColumnHeader Scope;
        private System.Windows.Forms.Panel PDFPanel;
        private AxAcroPDFLib.AxAcroPDF PDFFile;
        private System.Windows.Forms.Label JobTypeLabel;
        private System.Windows.Forms.Label BatchNameLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel TIFFPanel;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.TextBox ImagePageTextBox;
        internal System.Windows.Forms.Button PrviousPageButton;
        internal System.Windows.Forms.Button NextPageButton;
        internal System.Windows.Forms.Button FirstImagePageButton;
        internal System.Windows.Forms.Button LastImagePageButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView BatchMetadataListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel MetadataPpanel;
        private System.Windows.Forms.Panel ViewerPpanel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox ImageControlGroupBox;
        private System.Windows.Forms.Panel DocViewerSubPanel;
    }
}