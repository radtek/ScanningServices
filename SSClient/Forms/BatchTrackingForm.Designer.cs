namespace ScanningServicesAdmin.Forms
{
    partial class BatchTrackingForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchTrackingForm));
            this.BatchEventsList = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BatchNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InitialStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinalStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OperatorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.ReportButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BatchEventsList)).BeginInit();
            this.SuspendLayout();
            // 
            // BatchEventsList
            // 
            this.BatchEventsList.AllowUserToAddRows = false;
            this.BatchEventsList.AllowUserToDeleteRows = false;
            this.BatchEventsList.AllowUserToOrderColumns = true;
            this.BatchEventsList.AllowUserToResizeRows = false;
            this.BatchEventsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BatchEventsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BatchEventsList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.BatchEventsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BatchEventsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.BatchNumber,
            this.EventDate,
            this.InitialStatus,
            this.FinalStatus,
            this.OperatorID,
            this.StationID,
            this.EventDescription});
            this.BatchEventsList.Location = new System.Drawing.Point(5, 72);
            this.BatchEventsList.MultiSelect = false;
            this.BatchEventsList.Name = "BatchEventsList";
            this.BatchEventsList.RowHeadersWidth = 50;
            this.BatchEventsList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BatchEventsList.Size = new System.Drawing.Size(1196, 487);
            this.BatchEventsList.TabIndex = 98;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Width = 46;
            // 
            // BatchNumber
            // 
            this.BatchNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchNumber.DefaultCellStyle = dataGridViewCellStyle2;
            this.BatchNumber.FillWeight = 32.32325F;
            this.BatchNumber.HeaderText = "Batch Number";
            this.BatchNumber.Name = "BatchNumber";
            this.BatchNumber.ReadOnly = true;
            this.BatchNumber.Width = 120;
            // 
            // EventDate
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.EventDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.EventDate.FillWeight = 184.8846F;
            this.EventDate.HeaderText = "Date";
            this.EventDate.Name = "EventDate";
            this.EventDate.ReadOnly = true;
            this.EventDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.EventDate.Width = 43;
            // 
            // InitialStatus
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InitialStatus.DefaultCellStyle = dataGridViewCellStyle4;
            this.InitialStatus.FillWeight = 169.3697F;
            this.InitialStatus.HeaderText = "Initial Status";
            this.InitialStatus.Name = "InitialStatus";
            this.InitialStatus.ReadOnly = true;
            this.InitialStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.InitialStatus.Width = 76;
            // 
            // FinalStatus
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FinalStatus.DefaultCellStyle = dataGridViewCellStyle5;
            this.FinalStatus.FillWeight = 251.1599F;
            this.FinalStatus.HeaderText = "Final Status";
            this.FinalStatus.Name = "FinalStatus";
            this.FinalStatus.Width = 94;
            // 
            // OperatorID
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.OperatorID.DefaultCellStyle = dataGridViewCellStyle6;
            this.OperatorID.HeaderText = "Operator ID";
            this.OperatorID.Name = "OperatorID";
            this.OperatorID.Width = 94;
            // 
            // StationID
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.StationID.DefaultCellStyle = dataGridViewCellStyle7;
            this.StationID.HeaderText = "Station ID";
            this.StationID.Name = "StationID";
            this.StationID.Width = 83;
            // 
            // EventDescription
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.EventDescription.DefaultCellStyle = dataGridViewCellStyle8;
            this.EventDescription.HeaderText = "Event Descritpion";
            this.EventDescription.Name = "EventDescription";
            this.EventDescription.Width = 126;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::ScanningServicesAdmin.Properties.Resources.Exit_32x322;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.Location = new System.Drawing.Point(77, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 63);
            this.button1.TabIndex = 233;
            this.button1.Text = "Exit";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ReportButton
            // 
            this.ReportButton.Enabled = false;
            this.ReportButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ReportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReportButton.Image = global::ScanningServicesAdmin.Properties.Resources.PDF_Report_32x32_V2;
            this.ReportButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ReportButton.Location = new System.Drawing.Point(4, 3);
            this.ReportButton.Name = "ReportButton";
            this.ReportButton.Size = new System.Drawing.Size(68, 62);
            this.ReportButton.TabIndex = 232;
            this.ReportButton.Text = "PDF View";
            this.ReportButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ReportButton.UseVisualStyleBackColor = true;
            this.ReportButton.Click += new System.EventHandler(this.ReportButton_Click);
            // 
            // BatchTrackingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 564);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ReportButton);
            this.Controls.Add(this.BatchEventsList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BatchTrackingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BATCH TRACKING ";
            this.Load += new System.EventHandler(this.BatchTrackingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BatchEventsList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.DataGridView BatchEventsList;
        internal System.Windows.Forms.Button ReportButton;
        internal System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn InitialStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn FinalStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperatorID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventDescription;
    }
}