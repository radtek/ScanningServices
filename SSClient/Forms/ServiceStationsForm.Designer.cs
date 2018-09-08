namespace ScanningServicesAdmin.Forms
{
    partial class ServiceStationsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceStationsForm));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.ApplyHutton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.StationNameTextBox = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.EnablePDFCheckBox = new System.Windows.Forms.CheckBox();
            this.PDFGroupBox = new System.Windows.Forms.GroupBox();
            this.TargetFolderComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.WatchFolderComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.MaxNumBatchesUpDown = new System.Windows.Forms.DomainUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.WorkdayEndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.WorkdayStartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.WeekendEndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.WeekendStartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.EnableWorkdayCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableWeekendCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PDFGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 16);
            this.label6.TabIndex = 82;
            this.label6.Text = "Service Station:";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.Image = global::ScanningServicesAdmin.Properties.Resources.Delete_32x32;
            this.DeleteButton.Location = new System.Drawing.Point(233, 2);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(56, 61);
            this.DeleteButton.TabIndex = 80;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.DeleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // ApplyHutton
            // 
            this.ApplyHutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyHutton.Image = global::ScanningServicesAdmin.Properties.Resources.Apply_32x32;
            this.ApplyHutton.Location = new System.Drawing.Point(175, 2);
            this.ApplyHutton.Name = "ApplyHutton";
            this.ApplyHutton.Size = new System.Drawing.Size(56, 61);
            this.ApplyHutton.TabIndex = 79;
            this.ApplyHutton.Text = "Apply";
            this.ApplyHutton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ApplyHutton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ApplyHutton.UseVisualStyleBackColor = true;
            this.ApplyHutton.Click += new System.EventHandler(this.ApplyHutton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.Image = global::ScanningServicesAdmin.Properties.Resources.Reset_32x32;
            this.ResetButton.Location = new System.Drawing.Point(117, 2);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(56, 61);
            this.ResetButton.TabIndex = 78;
            this.ResetButton.Text = "Reset";
            this.ResetButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ResetButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton.Image = global::ScanningServicesAdmin.Properties.Resources.Help_32x32;
            this.HelpButton.Location = new System.Drawing.Point(1, 2);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(56, 61);
            this.HelpButton.TabIndex = 76;
            this.HelpButton.Text = "Help";
            this.HelpButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.HelpButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.HelpButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Image = global::ScanningServicesAdmin.Properties.Resources.Save_32x32;
            this.SaveButton.Location = new System.Drawing.Point(59, 2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(56, 61);
            this.SaveButton.TabIndex = 77;
            this.SaveButton.Text = "Save";
            this.SaveButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Image = global::ScanningServicesAdmin.Properties.Resources.Exit_32x32;
            this.ExitButton.Location = new System.Drawing.Point(291, 2);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(56, 61);
            this.ExitButton.TabIndex = 81;
            this.ExitButton.Text = "Exit";
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // StationNameTextBox
            // 
            this.StationNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StationNameTextBox.Location = new System.Drawing.Point(115, 78);
            this.StationNameTextBox.Name = "StationNameTextBox";
            this.StationNameTextBox.Size = new System.Drawing.Size(252, 22);
            this.StationNameTextBox.TabIndex = 92;
            this.StationNameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.StationNameTextBox_KeyUp);
            // 
            // toolTip1
            // 
            this.toolTip1.ForeColor = System.Drawing.Color.DarkRed;
            // 
            // EnablePDFCheckBox
            // 
            this.EnablePDFCheckBox.AutoSize = true;
            this.EnablePDFCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnablePDFCheckBox.Location = new System.Drawing.Point(11, 109);
            this.EnablePDFCheckBox.Name = "EnablePDFCheckBox";
            this.EnablePDFCheckBox.Size = new System.Drawing.Size(198, 20);
            this.EnablePDFCheckBox.TabIndex = 93;
            this.EnablePDFCheckBox.Text = "Enable it for PDF Conversion";
            this.EnablePDFCheckBox.UseVisualStyleBackColor = true;
            this.EnablePDFCheckBox.CheckedChanged += new System.EventHandler(this.EnablePDFCheckBox_CheckedChanged);
            // 
            // PDFGroupBox
            // 
            this.PDFGroupBox.Controls.Add(this.TargetFolderComboBox);
            this.PDFGroupBox.Controls.Add(this.label7);
            this.PDFGroupBox.Controls.Add(this.WatchFolderComboBox);
            this.PDFGroupBox.Controls.Add(this.label8);
            this.PDFGroupBox.Controls.Add(this.MaxNumBatchesUpDown);
            this.PDFGroupBox.Controls.Add(this.label4);
            this.PDFGroupBox.Controls.Add(this.WorkdayEndDateTimePicker);
            this.PDFGroupBox.Controls.Add(this.WorkdayStartDateTimePicker);
            this.PDFGroupBox.Controls.Add(this.label5);
            this.PDFGroupBox.Controls.Add(this.label3);
            this.PDFGroupBox.Controls.Add(this.WeekendEndDateTimePicker);
            this.PDFGroupBox.Controls.Add(this.WeekendStartDateTimePicker);
            this.PDFGroupBox.Controls.Add(this.label2);
            this.PDFGroupBox.Controls.Add(this.EnableWorkdayCheckBox);
            this.PDFGroupBox.Controls.Add(this.EnableWeekendCheckBox);
            this.PDFGroupBox.Controls.Add(this.label1);
            this.PDFGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PDFGroupBox.Location = new System.Drawing.Point(11, 136);
            this.PDFGroupBox.Name = "PDFGroupBox";
            this.PDFGroupBox.Size = new System.Drawing.Size(570, 257);
            this.PDFGroupBox.TabIndex = 94;
            this.PDFGroupBox.TabStop = false;
            this.PDFGroupBox.Text = "PDF Settings ";
            // 
            // TargetFolderComboBox
            // 
            this.TargetFolderComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetFolderComboBox.FormattingEnabled = true;
            this.TargetFolderComboBox.Location = new System.Drawing.Point(126, 65);
            this.TargetFolderComboBox.Name = "TargetFolderComboBox";
            this.TargetFolderComboBox.Size = new System.Drawing.Size(433, 24);
            this.TargetFolderComboBox.TabIndex = 108;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 16);
            this.label7.TabIndex = 107;
            this.label7.Text = "Target Location:";
            // 
            // WatchFolderComboBox
            // 
            this.WatchFolderComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WatchFolderComboBox.FormattingEnabled = true;
            this.WatchFolderComboBox.Location = new System.Drawing.Point(126, 30);
            this.WatchFolderComboBox.Name = "WatchFolderComboBox";
            this.WatchFolderComboBox.Size = new System.Drawing.Size(433, 24);
            this.WatchFolderComboBox.TabIndex = 106;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 16);
            this.label8.TabIndex = 105;
            this.label8.Text = "Watch Folder:";
            // 
            // MaxNumBatchesUpDown
            // 
            this.MaxNumBatchesUpDown.Items.Add("1");
            this.MaxNumBatchesUpDown.Items.Add("2");
            this.MaxNumBatchesUpDown.Items.Add("3");
            this.MaxNumBatchesUpDown.Items.Add("4");
            this.MaxNumBatchesUpDown.Items.Add("5");
            this.MaxNumBatchesUpDown.Items.Add("6");
            this.MaxNumBatchesUpDown.Items.Add("7");
            this.MaxNumBatchesUpDown.Items.Add("8");
            this.MaxNumBatchesUpDown.Items.Add("9");
            this.MaxNumBatchesUpDown.Items.Add("10");
            this.MaxNumBatchesUpDown.Items.Add("11");
            this.MaxNumBatchesUpDown.Items.Add("12");
            this.MaxNumBatchesUpDown.Items.Add("13");
            this.MaxNumBatchesUpDown.Items.Add("14");
            this.MaxNumBatchesUpDown.Items.Add("15");
            this.MaxNumBatchesUpDown.Items.Add("16");
            this.MaxNumBatchesUpDown.Items.Add("17");
            this.MaxNumBatchesUpDown.Items.Add("18");
            this.MaxNumBatchesUpDown.Items.Add("19");
            this.MaxNumBatchesUpDown.Items.Add("20");
            this.MaxNumBatchesUpDown.Location = new System.Drawing.Point(139, 102);
            this.MaxNumBatchesUpDown.Name = "MaxNumBatchesUpDown";
            this.MaxNumBatchesUpDown.Size = new System.Drawing.Size(40, 22);
            this.MaxNumBatchesUpDown.TabIndex = 104;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 16);
            this.label4.TabIndex = 103;
            this.label4.Text = "---";
            // 
            // WorkdayEndDateTimePicker
            // 
            this.WorkdayEndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.WorkdayEndDateTimePicker.Location = new System.Drawing.Point(224, 220);
            this.WorkdayEndDateTimePicker.Name = "WorkdayEndDateTimePicker";
            this.WorkdayEndDateTimePicker.ShowUpDown = true;
            this.WorkdayEndDateTimePicker.Size = new System.Drawing.Size(54, 22);
            this.WorkdayEndDateTimePicker.TabIndex = 102;
            this.WorkdayEndDateTimePicker.Value = new System.DateTime(2018, 2, 2, 0, 0, 0, 0);
            // 
            // WorkdayStartDateTimePicker
            // 
            this.WorkdayStartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.WorkdayStartDateTimePicker.Location = new System.Drawing.Point(150, 220);
            this.WorkdayStartDateTimePicker.Name = "WorkdayStartDateTimePicker";
            this.WorkdayStartDateTimePicker.ShowUpDown = true;
            this.WorkdayStartDateTimePicker.Size = new System.Drawing.Size(54, 22);
            this.WorkdayStartDateTimePicker.TabIndex = 101;
            this.WorkdayStartDateTimePicker.Value = new System.DateTime(2018, 2, 2, 0, 0, 0, 0);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 16);
            this.label5.TabIndex = 100;
            this.label5.Text = "Between:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 16);
            this.label3.TabIndex = 99;
            this.label3.Text = "---";
            // 
            // WeekendEndDateTimePicker
            // 
            this.WeekendEndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.WeekendEndDateTimePicker.Location = new System.Drawing.Point(226, 159);
            this.WeekendEndDateTimePicker.Name = "WeekendEndDateTimePicker";
            this.WeekendEndDateTimePicker.ShowUpDown = true;
            this.WeekendEndDateTimePicker.Size = new System.Drawing.Size(54, 22);
            this.WeekendEndDateTimePicker.TabIndex = 98;
            this.WeekendEndDateTimePicker.Value = new System.DateTime(2018, 2, 2, 0, 0, 0, 0);
            // 
            // WeekendStartDateTimePicker
            // 
            this.WeekendStartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.WeekendStartDateTimePicker.Location = new System.Drawing.Point(150, 159);
            this.WeekendStartDateTimePicker.Name = "WeekendStartDateTimePicker";
            this.WeekendStartDateTimePicker.ShowUpDown = true;
            this.WeekendStartDateTimePicker.Size = new System.Drawing.Size(54, 22);
            this.WeekendStartDateTimePicker.TabIndex = 97;
            this.WeekendStartDateTimePicker.Value = new System.DateTime(2018, 2, 2, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 96;
            this.label2.Text = "Between:";
            // 
            // EnableWorkdayCheckBox
            // 
            this.EnableWorkdayCheckBox.AutoSize = true;
            this.EnableWorkdayCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnableWorkdayCheckBox.Location = new System.Drawing.Point(17, 194);
            this.EnableWorkdayCheckBox.Name = "EnableWorkdayCheckBox";
            this.EnableWorkdayCheckBox.Size = new System.Drawing.Size(192, 20);
            this.EnableWorkdayCheckBox.TabIndex = 95;
            this.EnableWorkdayCheckBox.Text = "Enable WorkDay Operation";
            this.EnableWorkdayCheckBox.UseVisualStyleBackColor = true;
            this.EnableWorkdayCheckBox.CheckedChanged += new System.EventHandler(this.EnableWorkdayCheckBox_CheckedChanged);
            // 
            // EnableWeekendCheckBox
            // 
            this.EnableWeekendCheckBox.AutoSize = true;
            this.EnableWeekendCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnableWeekendCheckBox.Location = new System.Drawing.Point(17, 133);
            this.EnableWeekendCheckBox.Name = "EnableWeekendCheckBox";
            this.EnableWeekendCheckBox.Size = new System.Drawing.Size(194, 20);
            this.EnableWeekendCheckBox.TabIndex = 94;
            this.EnableWeekendCheckBox.Text = "Enable Weekend Operation";
            this.EnableWeekendCheckBox.UseVisualStyleBackColor = true;
            this.EnableWeekendCheckBox.CheckedChanged += new System.EventHandler(this.EnableWeekendCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 83;
            this.label1.Text = "Max Num Batches:";
            // 
            // ServiceStationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 396);
            this.Controls.Add(this.PDFGroupBox);
            this.Controls.Add(this.EnablePDFCheckBox);
            this.Controls.Add(this.StationNameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.ApplyHutton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ExitButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ServiceStationsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SERVICE  STATIONS";
            this.Load += new System.EventHandler(this.ServiceStationsForm_Load);
            this.PDFGroupBox.ResumeLayout(false);
            this.PDFGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button ApplyHutton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.TextBox StationNameTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox EnablePDFCheckBox;
        private System.Windows.Forms.GroupBox PDFGroupBox;
        private System.Windows.Forms.DomainUpDown MaxNumBatchesUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker WorkdayEndDateTimePicker;
        private System.Windows.Forms.DateTimePicker WorkdayStartDateTimePicker;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker WeekendEndDateTimePicker;
        private System.Windows.Forms.DateTimePicker WeekendStartDateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox EnableWorkdayCheckBox;
        private System.Windows.Forms.CheckBox EnableWeekendCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TargetFolderComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox WatchFolderComboBox;
        private System.Windows.Forms.Label label8;
    }
}