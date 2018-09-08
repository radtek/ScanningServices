namespace ScanningServicesAdmin.Forms
{
    partial class StatusChangeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusChangeForm));
            this.BatchAliasNameTextBox = new System.Windows.Forms.TextBox();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.RecallCommentsTextBox = new System.Windows.Forms.TextBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.RejectCommentsTextBox = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.CurrentStatusTextBox = new System.Windows.Forms.TextBox();
            this.BatchNameTextBox = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.OperatorTextBox = new System.Windows.Forms.TextBox();
            this.StationNameTextBox = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.LockedLabel = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.BatchNumber = new System.Windows.Forms.TextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.DeleteRadioButton = new System.Windows.Forms.RadioButton();
            this.Label9 = new System.Windows.Forms.Label();
            this.RejectedRadioButton = new System.Windows.Forms.RadioButton();
            this.RecallRadioButton = new System.Windows.Forms.RadioButton();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.GroupBox4.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // BatchAliasNameTextBox
            // 
            this.BatchAliasNameTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.BatchAliasNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BatchAliasNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchAliasNameTextBox.ForeColor = System.Drawing.Color.Navy;
            this.BatchAliasNameTextBox.Location = new System.Drawing.Point(108, 222);
            this.BatchAliasNameTextBox.Name = "BatchAliasNameTextBox";
            this.BatchAliasNameTextBox.Size = new System.Drawing.Size(227, 15);
            this.BatchAliasNameTextBox.TabIndex = 219;
            // 
            // GroupBox4
            // 
            this.GroupBox4.BackColor = System.Drawing.SystemColors.Control;
            this.GroupBox4.Controls.Add(this.RecallCommentsTextBox);
            this.GroupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox4.Location = new System.Drawing.Point(351, 278);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(332, 139);
            this.GroupBox4.TabIndex = 218;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Recall Comments";
            // 
            // RecallCommentsTextBox
            // 
            this.RecallCommentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RecallCommentsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecallCommentsTextBox.Location = new System.Drawing.Point(6, 21);
            this.RecallCommentsTextBox.MaxLength = 200;
            this.RecallCommentsTextBox.Multiline = true;
            this.RecallCommentsTextBox.Name = "RecallCommentsTextBox";
            this.RecallCommentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RecallCommentsTextBox.Size = new System.Drawing.Size(319, 112);
            this.RecallCommentsTextBox.TabIndex = 49;
            // 
            // GroupBox2
            // 
            this.GroupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.GroupBox2.Controls.Add(this.RejectCommentsTextBox);
            this.GroupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(351, 124);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(332, 134);
            this.GroupBox2.TabIndex = 217;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Rejection Comments";
            // 
            // RejectCommentsTextBox
            // 
            this.RejectCommentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RejectCommentsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RejectCommentsTextBox.Location = new System.Drawing.Point(6, 21);
            this.RejectCommentsTextBox.MaxLength = 200;
            this.RejectCommentsTextBox.Multiline = true;
            this.RejectCommentsTextBox.Name = "RejectCommentsTextBox";
            this.RejectCommentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RejectCommentsTextBox.Size = new System.Drawing.Size(319, 107);
            this.RejectCommentsTextBox.TabIndex = 49;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.Location = new System.Drawing.Point(12, 221);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(81, 16);
            this.Label10.TabIndex = 216;
            this.Label10.Text = "Alias Name:";
            // 
            // CurrentStatusTextBox
            // 
            this.CurrentStatusTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.CurrentStatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurrentStatusTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentStatusTextBox.ForeColor = System.Drawing.Color.Navy;
            this.CurrentStatusTextBox.Location = new System.Drawing.Point(108, 118);
            this.CurrentStatusTextBox.Name = "CurrentStatusTextBox";
            this.CurrentStatusTextBox.Size = new System.Drawing.Size(227, 15);
            this.CurrentStatusTextBox.TabIndex = 215;
            // 
            // BatchNameTextBox
            // 
            this.BatchNameTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.BatchNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BatchNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchNameTextBox.ForeColor = System.Drawing.Color.Navy;
            this.BatchNameTextBox.Location = new System.Drawing.Point(108, 196);
            this.BatchNameTextBox.Name = "BatchNameTextBox";
            this.BatchNameTextBox.Size = new System.Drawing.Size(227, 15);
            this.BatchNameTextBox.TabIndex = 214;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(12, 195);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(85, 16);
            this.Label3.TabIndex = 213;
            this.Label3.Text = "Batch Name:";
            // 
            // OperatorTextBox
            // 
            this.OperatorTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.OperatorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OperatorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperatorTextBox.ForeColor = System.Drawing.Color.Navy;
            this.OperatorTextBox.Location = new System.Drawing.Point(108, 144);
            this.OperatorTextBox.Name = "OperatorTextBox";
            this.OperatorTextBox.Size = new System.Drawing.Size(227, 15);
            this.OperatorTextBox.TabIndex = 212;
            // 
            // StationNameTextBox
            // 
            this.StationNameTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.StationNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.StationNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StationNameTextBox.ForeColor = System.Drawing.Color.Navy;
            this.StationNameTextBox.Location = new System.Drawing.Point(108, 170);
            this.StationNameTextBox.Name = "StationNameTextBox";
            this.StationNameTextBox.Size = new System.Drawing.Size(227, 15);
            this.StationNameTextBox.TabIndex = 211;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(12, 169);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(74, 16);
            this.Label4.TabIndex = 210;
            this.Label4.Text = "QC Station:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(12, 143);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(86, 16);
            this.Label6.TabIndex = 209;
            this.Label6.Text = "QC Operator:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(12, 117);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(93, 16);
            this.Label1.TabIndex = 208;
            this.Label1.Text = "Current Status:";
            // 
            // LockedLabel
            // 
            this.LockedLabel.AutoSize = true;
            this.LockedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LockedLabel.Location = new System.Drawing.Point(297, 86);
            this.LockedLabel.Name = "LockedLabel";
            this.LockedLabel.Size = new System.Drawing.Size(61, 16);
            this.LockedLabel.TabIndex = 206;
            this.LockedLabel.Text = "LOCKED";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(11, 86);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(85, 16);
            this.Label2.TabIndex = 205;
            this.Label2.Text = "Box Number:";
            // 
            // BatchNumber
            // 
            this.BatchNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BatchNumber.Location = new System.Drawing.Point(104, 83);
            this.BatchNumber.Name = "BatchNumber";
            this.BatchNumber.Size = new System.Drawing.Size(176, 22);
            this.BatchNumber.TabIndex = 204;
            this.BatchNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BatchNumber_KeyDown);
            // 
            // GroupBox3
            // 
            this.GroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox3.Controls.Add(this.Label7);
            this.GroupBox3.Controls.Add(this.Label5);
            this.GroupBox3.Controls.Add(this.Label8);
            this.GroupBox3.Controls.Add(this.DeleteRadioButton);
            this.GroupBox3.Controls.Add(this.Label9);
            this.GroupBox3.Controls.Add(this.RejectedRadioButton);
            this.GroupBox3.Controls.Add(this.RecallRadioButton);
            this.GroupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox3.Location = new System.Drawing.Point(5, 249);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(340, 169);
            this.GroupBox3.TabIndex = 207;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Options";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.ForeColor = System.Drawing.Color.Green;
            this.Label7.Location = new System.Drawing.Point(3, 107);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(136, 13);
            this.Label7.TabIndex = 52;
            this.Label7.Text = " requested to the customer)";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.ForeColor = System.Drawing.Color.Green;
            this.Label5.Location = new System.Drawing.Point(3, 47);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(318, 13);
            this.Label5.TabIndex = 51;
            this.Label5.Text = "(This option only applies for Batches that are Waiting for Approval)";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.Green;
            this.Label8.Location = new System.Drawing.Point(2, 148);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(320, 13);
            this.Label8.TabIndex = 50;
            this.Label8.Text = "(Select this option to remove this Batch completely from the Sytem)";
            // 
            // DeleteRadioButton
            // 
            this.DeleteRadioButton.AutoSize = true;
            this.DeleteRadioButton.Enabled = false;
            this.DeleteRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.DeleteRadioButton.Location = new System.Drawing.Point(6, 125);
            this.DeleteRadioButton.Name = "DeleteRadioButton";
            this.DeleteRadioButton.Size = new System.Drawing.Size(66, 20);
            this.DeleteRadioButton.TabIndex = 49;
            this.DeleteRadioButton.TabStop = true;
            this.DeleteRadioButton.Text = "Delete";
            this.DeleteRadioButton.UseVisualStyleBackColor = true;
            this.DeleteRadioButton.CheckedChanged += new System.EventHandler(this.DeleteRadioButton_CheckedChanged);
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.Color.Green;
            this.Label9.Location = new System.Drawing.Point(1, 91);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(307, 13);
            this.Label9.TabIndex = 45;
            this.Label9.Text = "(Use this Option only as a reminder for Batches that needs to be";
            // 
            // RejectedRadioButton
            // 
            this.RejectedRadioButton.AutoSize = true;
            this.RejectedRadioButton.Enabled = false;
            this.RejectedRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.RejectedRadioButton.Location = new System.Drawing.Point(6, 24);
            this.RejectedRadioButton.Name = "RejectedRadioButton";
            this.RejectedRadioButton.Size = new System.Drawing.Size(65, 20);
            this.RejectedRadioButton.TabIndex = 43;
            this.RejectedRadioButton.TabStop = true;
            this.RejectedRadioButton.Text = "Reject";
            this.RejectedRadioButton.UseVisualStyleBackColor = true;
            this.RejectedRadioButton.CheckedChanged += new System.EventHandler(this.RejectedRadioButton_CheckedChanged);
            // 
            // RecallRadioButton
            // 
            this.RecallRadioButton.AutoSize = true;
            this.RecallRadioButton.Enabled = false;
            this.RecallRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.RecallRadioButton.Location = new System.Drawing.Point(5, 68);
            this.RecallRadioButton.Name = "RecallRadioButton";
            this.RecallRadioButton.Size = new System.Drawing.Size(65, 20);
            this.RecallRadioButton.TabIndex = 44;
            this.RecallRadioButton.TabStop = true;
            this.RecallRadioButton.Text = "Recall";
            this.RecallRadioButton.UseVisualStyleBackColor = true;
            this.RecallRadioButton.CheckedChanged += new System.EventHandler(this.RecallRadioButton_CheckedChanged);
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Image = global::ScanningServicesAdmin.Properties.Resources.Exit_32x322;
            this.ExitButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ExitButton.Location = new System.Drawing.Point(148, 5);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(68, 63);
            this.ExitButton.TabIndex = 223;
            this.ExitButton.Text = "Exit";
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ApplyButton.Enabled = false;
            this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ApplyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyButton.Image = global::ScanningServicesAdmin.Properties.Resources.Apply_32x321;
            this.ApplyButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ApplyButton.Location = new System.Drawing.Point(75, 5);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(68, 63);
            this.ApplyButton.TabIndex = 222;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ClearButton.Enabled = false;
            this.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearButton.Image = global::ScanningServicesAdmin.Properties.Resources.Clear_32x32;
            this.ClearButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ClearButton.Location = new System.Drawing.Point(3, 5);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(68, 63);
            this.ClearButton.TabIndex = 221;
            this.ClearButton.Text = "Clear";
            this.ClearButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // StatusChangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 426);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.BatchAliasNameTextBox);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.CurrentStatusTextBox);
            this.Controls.Add(this.BatchNameTextBox);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.OperatorTextBox);
            this.Controls.Add(this.StationNameTextBox);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.LockedLabel);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.BatchNumber);
            this.Controls.Add(this.GroupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StatusChangeForm";
            this.Text = "STATUS ADJUSTMENT";
            this.Load += new System.EventHandler(this.StatusChangeForm_Load);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox BatchAliasNameTextBox;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.TextBox RecallCommentsTextBox;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.TextBox RejectCommentsTextBox;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.TextBox CurrentStatusTextBox;
        internal System.Windows.Forms.TextBox BatchNameTextBox;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox OperatorTextBox;
        internal System.Windows.Forms.TextBox StationNameTextBox;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label LockedLabel;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox BatchNumber;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.RadioButton DeleteRadioButton;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.RadioButton RejectedRadioButton;
        internal System.Windows.Forms.RadioButton RecallRadioButton;
        internal System.Windows.Forms.Button ExitButton;
        internal System.Windows.Forms.Button ApplyButton;
        internal System.Windows.Forms.Button ClearButton;
    }
}