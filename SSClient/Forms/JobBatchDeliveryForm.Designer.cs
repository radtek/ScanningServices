namespace ScanningServicesAdmin.Forms
{
    partial class JobBatchDeliveryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobBatchDeliveryForm));
            this.SupersedeGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RangeCheckBox = new System.Windows.Forms.CheckBox();
            this.StartRadioButton = new System.Windows.Forms.RadioButton();
            this.RepeatTaskEveryRadioButton = new System.Windows.Forms.RadioButton();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.BeginDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.RepeatTaskEveryUpDown = new System.Windows.Forms.DomainUpDown();
            this.RepeatEveryGroupBox = new System.Windows.Forms.GroupBox();
            this.RepeatMinutesRadioButton = new System.Windows.Forms.RadioButton();
            this.RepeatHoursRadioButton = new System.Windows.Forms.RadioButton();
            this.DailyRadioButton = new System.Windows.Forms.RadioButton();
            this.WeeklyRadioButton = new System.Windows.Forms.RadioButton();
            this.DailyGroupBox = new System.Windows.Forms.GroupBox();
            this.RecurDaysTextBoxUpDown = new System.Windows.Forms.DomainUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.WeeklyGroupBox = new System.Windows.Forms.GroupBox();
            this.SaturdayCheckBox = new System.Windows.Forms.CheckBox();
            this.FridayCheckBox = new System.Windows.Forms.CheckBox();
            this.ThursdayCheckBox = new System.Windows.Forms.CheckBox();
            this.WednesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.TuesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.MondayCheckBox = new System.Windows.Forms.CheckBox();
            this.SundayCheckBox = new System.Windows.Forms.CheckBox();
            this.SupersedeCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.DeleteButton = new System.Windows.Forms.Button();
            this.ApplyHutton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.EnablecheckBox = new System.Windows.Forms.CheckBox();
            this.SupersedeGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.RepeatEveryGroupBox.SuspendLayout();
            this.DailyGroupBox.SuspendLayout();
            this.WeeklyGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SupersedeGroupBox
            // 
            this.SupersedeGroupBox.Controls.Add(this.groupBox2);
            this.SupersedeGroupBox.Controls.Add(this.DailyRadioButton);
            this.SupersedeGroupBox.Controls.Add(this.WeeklyRadioButton);
            this.SupersedeGroupBox.Controls.Add(this.DailyGroupBox);
            this.SupersedeGroupBox.Controls.Add(this.WeeklyGroupBox);
            this.SupersedeGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SupersedeGroupBox.Location = new System.Drawing.Point(12, 134);
            this.SupersedeGroupBox.Name = "SupersedeGroupBox";
            this.SupersedeGroupBox.Size = new System.Drawing.Size(504, 313);
            this.SupersedeGroupBox.TabIndex = 56;
            this.SupersedeGroupBox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RangeCheckBox);
            this.groupBox2.Controls.Add(this.StartRadioButton);
            this.groupBox2.Controls.Add(this.RepeatTaskEveryRadioButton);
            this.groupBox2.Controls.Add(this.EndDateTimePicker);
            this.groupBox2.Controls.Add(this.StartDateTimePicker);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.BeginDateTimePicker);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.RepeatTaskEveryUpDown);
            this.groupBox2.Controls.Add(this.RepeatEveryGroupBox);
            this.groupBox2.Location = new System.Drawing.Point(16, 173);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 131);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Advanced ";
            // 
            // RangeCheckBox
            // 
            this.RangeCheckBox.AutoSize = true;
            this.RangeCheckBox.Location = new System.Drawing.Point(86, 62);
            this.RangeCheckBox.Name = "RangeCheckBox";
            this.RangeCheckBox.Size = new System.Drawing.Size(68, 20);
            this.RangeCheckBox.TabIndex = 45;
            this.RangeCheckBox.Text = "Range";
            this.RangeCheckBox.UseVisualStyleBackColor = true;
            // 
            // StartRadioButton
            // 
            this.StartRadioButton.AutoSize = true;
            this.StartRadioButton.Location = new System.Drawing.Point(18, 96);
            this.StartRadioButton.Name = "StartRadioButton";
            this.StartRadioButton.Size = new System.Drawing.Size(53, 20);
            this.StartRadioButton.TabIndex = 44;
            this.StartRadioButton.TabStop = true;
            this.StartRadioButton.Text = "Start";
            this.StartRadioButton.UseVisualStyleBackColor = true;
            // 
            // RepeatTaskEveryRadioButton
            // 
            this.RepeatTaskEveryRadioButton.AutoSize = true;
            this.RepeatTaskEveryRadioButton.Location = new System.Drawing.Point(18, 28);
            this.RepeatTaskEveryRadioButton.Name = "RepeatTaskEveryRadioButton";
            this.RepeatTaskEveryRadioButton.Size = new System.Drawing.Size(136, 20);
            this.RepeatTaskEveryRadioButton.TabIndex = 43;
            this.RepeatTaskEveryRadioButton.TabStop = true;
            this.RepeatTaskEveryRadioButton.Text = "Repeat task every";
            this.RepeatTaskEveryRadioButton.UseVisualStyleBackColor = true;
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.EndDateTimePicker.Location = new System.Drawing.Point(335, 60);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.ShowUpDown = true;
            this.EndDateTimePicker.Size = new System.Drawing.Size(84, 22);
            this.EndDateTimePicker.TabIndex = 42;
            this.EndDateTimePicker.Value = new System.DateTime(2018, 2, 2, 0, 0, 0, 0);
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartDateTimePicker.Location = new System.Drawing.Point(87, 94);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.ShowUpDown = true;
            this.StartDateTimePicker.Size = new System.Drawing.Size(83, 22);
            this.StartDateTimePicker.TabIndex = 35;
            this.StartDateTimePicker.Value = new System.DateTime(2018, 2, 2, 0, 0, 0, 0);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(313, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 16);
            this.label8.TabIndex = 41;
            this.label8.Text = "---";
            // 
            // BeginDateTimePicker
            // 
            this.BeginDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.BeginDateTimePicker.Location = new System.Drawing.Point(227, 60);
            this.BeginDateTimePicker.Name = "BeginDateTimePicker";
            this.BeginDateTimePicker.ShowUpDown = true;
            this.BeginDateTimePicker.Size = new System.Drawing.Size(84, 22);
            this.BeginDateTimePicker.TabIndex = 40;
            this.BeginDateTimePicker.Value = new System.DateTime(2018, 2, 2, 0, 0, 0, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(162, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 16);
            this.label7.TabIndex = 39;
            this.label7.Text = "Between:";
            // 
            // RepeatTaskEveryUpDown
            // 
            this.RepeatTaskEveryUpDown.Items.Add("1");
            this.RepeatTaskEveryUpDown.Items.Add("2");
            this.RepeatTaskEveryUpDown.Items.Add("3");
            this.RepeatTaskEveryUpDown.Items.Add("4");
            this.RepeatTaskEveryUpDown.Items.Add("5");
            this.RepeatTaskEveryUpDown.Items.Add("6");
            this.RepeatTaskEveryUpDown.Items.Add("7");
            this.RepeatTaskEveryUpDown.Items.Add("8");
            this.RepeatTaskEveryUpDown.Items.Add("9");
            this.RepeatTaskEveryUpDown.Items.Add("10");
            this.RepeatTaskEveryUpDown.Items.Add("11");
            this.RepeatTaskEveryUpDown.Items.Add("12");
            this.RepeatTaskEveryUpDown.Items.Add("13");
            this.RepeatTaskEveryUpDown.Items.Add("14");
            this.RepeatTaskEveryUpDown.Items.Add("15");
            this.RepeatTaskEveryUpDown.Items.Add("16");
            this.RepeatTaskEveryUpDown.Items.Add("17");
            this.RepeatTaskEveryUpDown.Items.Add("18");
            this.RepeatTaskEveryUpDown.Items.Add("19");
            this.RepeatTaskEveryUpDown.Items.Add("20");
            this.RepeatTaskEveryUpDown.Location = new System.Drawing.Point(165, 28);
            this.RepeatTaskEveryUpDown.Name = "RepeatTaskEveryUpDown";
            this.RepeatTaskEveryUpDown.Size = new System.Drawing.Size(40, 22);
            this.RepeatTaskEveryUpDown.TabIndex = 36;
            // 
            // RepeatEveryGroupBox
            // 
            this.RepeatEveryGroupBox.Controls.Add(this.RepeatMinutesRadioButton);
            this.RepeatEveryGroupBox.Controls.Add(this.RepeatHoursRadioButton);
            this.RepeatEveryGroupBox.Location = new System.Drawing.Point(229, 19);
            this.RepeatEveryGroupBox.Name = "RepeatEveryGroupBox";
            this.RepeatEveryGroupBox.Size = new System.Drawing.Size(190, 34);
            this.RepeatEveryGroupBox.TabIndex = 38;
            this.RepeatEveryGroupBox.TabStop = false;
            // 
            // RepeatMinutesRadioButton
            // 
            this.RepeatMinutesRadioButton.AutoSize = true;
            this.RepeatMinutesRadioButton.Location = new System.Drawing.Point(95, 9);
            this.RepeatMinutesRadioButton.Name = "RepeatMinutesRadioButton";
            this.RepeatMinutesRadioButton.Size = new System.Drawing.Size(72, 20);
            this.RepeatMinutesRadioButton.TabIndex = 2;
            this.RepeatMinutesRadioButton.TabStop = true;
            this.RepeatMinutesRadioButton.Text = "Minutes";
            this.RepeatMinutesRadioButton.UseVisualStyleBackColor = true;
            // 
            // RepeatHoursRadioButton
            // 
            this.RepeatHoursRadioButton.AutoSize = true;
            this.RepeatHoursRadioButton.Location = new System.Drawing.Point(9, 9);
            this.RepeatHoursRadioButton.Name = "RepeatHoursRadioButton";
            this.RepeatHoursRadioButton.Size = new System.Drawing.Size(62, 20);
            this.RepeatHoursRadioButton.TabIndex = 1;
            this.RepeatHoursRadioButton.TabStop = true;
            this.RepeatHoursRadioButton.Text = "Hours";
            this.RepeatHoursRadioButton.UseVisualStyleBackColor = true;
            // 
            // DailyRadioButton
            // 
            this.DailyRadioButton.AutoSize = true;
            this.DailyRadioButton.Location = new System.Drawing.Point(17, 30);
            this.DailyRadioButton.Name = "DailyRadioButton";
            this.DailyRadioButton.Size = new System.Drawing.Size(57, 20);
            this.DailyRadioButton.TabIndex = 0;
            this.DailyRadioButton.TabStop = true;
            this.DailyRadioButton.Text = "Daily";
            this.DailyRadioButton.UseVisualStyleBackColor = true;
            // 
            // WeeklyRadioButton
            // 
            this.WeeklyRadioButton.AutoSize = true;
            this.WeeklyRadioButton.Location = new System.Drawing.Point(16, 85);
            this.WeeklyRadioButton.Name = "WeeklyRadioButton";
            this.WeeklyRadioButton.Size = new System.Drawing.Size(125, 20);
            this.WeeklyRadioButton.TabIndex = 1;
            this.WeeklyRadioButton.TabStop = true;
            this.WeeklyRadioButton.Text = "Day of the Week";
            this.WeeklyRadioButton.UseVisualStyleBackColor = true;
            // 
            // DailyGroupBox
            // 
            this.DailyGroupBox.Controls.Add(this.RecurDaysTextBoxUpDown);
            this.DailyGroupBox.Controls.Add(this.label6);
            this.DailyGroupBox.Controls.Add(this.label5);
            this.DailyGroupBox.Location = new System.Drawing.Point(162, 13);
            this.DailyGroupBox.Name = "DailyGroupBox";
            this.DailyGroupBox.Size = new System.Drawing.Size(328, 55);
            this.DailyGroupBox.TabIndex = 42;
            this.DailyGroupBox.TabStop = false;
            // 
            // RecurDaysTextBoxUpDown
            // 
            this.RecurDaysTextBoxUpDown.Items.Add("1");
            this.RecurDaysTextBoxUpDown.Items.Add("2");
            this.RecurDaysTextBoxUpDown.Items.Add("3");
            this.RecurDaysTextBoxUpDown.Items.Add("4");
            this.RecurDaysTextBoxUpDown.Items.Add("5");
            this.RecurDaysTextBoxUpDown.Items.Add("6");
            this.RecurDaysTextBoxUpDown.Items.Add("7");
            this.RecurDaysTextBoxUpDown.Items.Add("8");
            this.RecurDaysTextBoxUpDown.Items.Add("9");
            this.RecurDaysTextBoxUpDown.Items.Add("10");
            this.RecurDaysTextBoxUpDown.Items.Add("11");
            this.RecurDaysTextBoxUpDown.Items.Add("12");
            this.RecurDaysTextBoxUpDown.Items.Add("13");
            this.RecurDaysTextBoxUpDown.Items.Add("14");
            this.RecurDaysTextBoxUpDown.Items.Add("15");
            this.RecurDaysTextBoxUpDown.Items.Add("16");
            this.RecurDaysTextBoxUpDown.Items.Add("17");
            this.RecurDaysTextBoxUpDown.Items.Add("18");
            this.RecurDaysTextBoxUpDown.Items.Add("19");
            this.RecurDaysTextBoxUpDown.Items.Add("20");
            this.RecurDaysTextBoxUpDown.Items.Add("21");
            this.RecurDaysTextBoxUpDown.Items.Add("22");
            this.RecurDaysTextBoxUpDown.Items.Add("23");
            this.RecurDaysTextBoxUpDown.Items.Add("24");
            this.RecurDaysTextBoxUpDown.Items.Add("25");
            this.RecurDaysTextBoxUpDown.Items.Add("26");
            this.RecurDaysTextBoxUpDown.Items.Add("27");
            this.RecurDaysTextBoxUpDown.Items.Add("28");
            this.RecurDaysTextBoxUpDown.Items.Add("29");
            this.RecurDaysTextBoxUpDown.Items.Add("30");
            this.RecurDaysTextBoxUpDown.Location = new System.Drawing.Point(98, 20);
            this.RecurDaysTextBoxUpDown.Name = "RecurDaysTextBoxUpDown";
            this.RecurDaysTextBoxUpDown.Size = new System.Drawing.Size(38, 22);
            this.RecurDaysTextBoxUpDown.TabIndex = 42;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 16);
            this.label6.TabIndex = 39;
            this.label6.Text = "Recur every:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(141, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 16);
            this.label5.TabIndex = 41;
            this.label5.Text = "days";
            // 
            // WeeklyGroupBox
            // 
            this.WeeklyGroupBox.Controls.Add(this.SaturdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.FridayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.ThursdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.WednesdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.TuesdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.MondayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.SundayCheckBox);
            this.WeeklyGroupBox.Location = new System.Drawing.Point(162, 70);
            this.WeeklyGroupBox.Name = "WeeklyGroupBox";
            this.WeeklyGroupBox.Size = new System.Drawing.Size(328, 97);
            this.WeeklyGroupBox.TabIndex = 6;
            this.WeeklyGroupBox.TabStop = false;
            // 
            // SaturdayCheckBox
            // 
            this.SaturdayCheckBox.AutoSize = true;
            this.SaturdayCheckBox.Location = new System.Drawing.Point(15, 73);
            this.SaturdayCheckBox.Name = "SaturdayCheckBox";
            this.SaturdayCheckBox.Size = new System.Drawing.Size(81, 20);
            this.SaturdayCheckBox.TabIndex = 12;
            this.SaturdayCheckBox.Text = "Saturday";
            this.SaturdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // FridayCheckBox
            // 
            this.FridayCheckBox.AutoSize = true;
            this.FridayCheckBox.Location = new System.Drawing.Point(226, 45);
            this.FridayCheckBox.Name = "FridayCheckBox";
            this.FridayCheckBox.Size = new System.Drawing.Size(65, 20);
            this.FridayCheckBox.TabIndex = 11;
            this.FridayCheckBox.Text = "Friday";
            this.FridayCheckBox.UseVisualStyleBackColor = true;
            // 
            // ThursdayCheckBox
            // 
            this.ThursdayCheckBox.AutoSize = true;
            this.ThursdayCheckBox.Location = new System.Drawing.Point(126, 45);
            this.ThursdayCheckBox.Name = "ThursdayCheckBox";
            this.ThursdayCheckBox.Size = new System.Drawing.Size(84, 20);
            this.ThursdayCheckBox.TabIndex = 10;
            this.ThursdayCheckBox.Text = "Thursday";
            this.ThursdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // WednesdayCheckBox
            // 
            this.WednesdayCheckBox.AutoSize = true;
            this.WednesdayCheckBox.Location = new System.Drawing.Point(15, 45);
            this.WednesdayCheckBox.Name = "WednesdayCheckBox";
            this.WednesdayCheckBox.Size = new System.Drawing.Size(101, 20);
            this.WednesdayCheckBox.TabIndex = 9;
            this.WednesdayCheckBox.Text = "Wednesday";
            this.WednesdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // TuesdayCheckBox
            // 
            this.TuesdayCheckBox.AutoSize = true;
            this.TuesdayCheckBox.Location = new System.Drawing.Point(226, 15);
            this.TuesdayCheckBox.Name = "TuesdayCheckBox";
            this.TuesdayCheckBox.Size = new System.Drawing.Size(81, 20);
            this.TuesdayCheckBox.TabIndex = 8;
            this.TuesdayCheckBox.Text = "Tuesday";
            this.TuesdayCheckBox.UseVisualStyleBackColor = true;
            // 
            // MondayCheckBox
            // 
            this.MondayCheckBox.AutoSize = true;
            this.MondayCheckBox.Location = new System.Drawing.Point(126, 15);
            this.MondayCheckBox.Name = "MondayCheckBox";
            this.MondayCheckBox.Size = new System.Drawing.Size(76, 20);
            this.MondayCheckBox.TabIndex = 7;
            this.MondayCheckBox.Text = "Monday";
            this.MondayCheckBox.UseVisualStyleBackColor = true;
            // 
            // SundayCheckBox
            // 
            this.SundayCheckBox.AutoSize = true;
            this.SundayCheckBox.Location = new System.Drawing.Point(15, 15);
            this.SundayCheckBox.Name = "SundayCheckBox";
            this.SundayCheckBox.Size = new System.Drawing.Size(73, 20);
            this.SundayCheckBox.TabIndex = 6;
            this.SundayCheckBox.Text = "Sunday";
            this.SundayCheckBox.UseVisualStyleBackColor = true;
            // 
            // SupersedeCheckBox
            // 
            this.SupersedeCheckBox.AutoSize = true;
            this.SupersedeCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SupersedeCheckBox.Location = new System.Drawing.Point(17, 108);
            this.SupersedeCheckBox.Name = "SupersedeCheckBox";
            this.SupersedeCheckBox.Size = new System.Drawing.Size(247, 20);
            this.SupersedeCheckBox.TabIndex = 55;
            this.SupersedeCheckBox.Text = "Supersede Service Master Schedule";
            this.SupersedeCheckBox.UseVisualStyleBackColor = true;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.Image = global::ScanningServicesAdmin.Properties.Resources.Delete_32x32;
            this.DeleteButton.Location = new System.Drawing.Point(235, 3);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(56, 61);
            this.DeleteButton.TabIndex = 53;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.DeleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // ApplyHutton
            // 
            this.ApplyHutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyHutton.Image = global::ScanningServicesAdmin.Properties.Resources.Apply_32x32;
            this.ApplyHutton.Location = new System.Drawing.Point(177, 3);
            this.ApplyHutton.Name = "ApplyHutton";
            this.ApplyHutton.Size = new System.Drawing.Size(56, 61);
            this.ApplyHutton.TabIndex = 52;
            this.ApplyHutton.Text = "Apply";
            this.ApplyHutton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ApplyHutton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ApplyHutton.UseVisualStyleBackColor = true;
            // 
            // ResetButton
            // 
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.Image = global::ScanningServicesAdmin.Properties.Resources.Reset_32x32;
            this.ResetButton.Location = new System.Drawing.Point(119, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(56, 61);
            this.ResetButton.TabIndex = 50;
            this.ResetButton.Text = "Reset";
            this.ResetButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ResetButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ResetButton.UseVisualStyleBackColor = true;
            // 
            // HelpButton
            // 
            this.HelpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton.Image = global::ScanningServicesAdmin.Properties.Resources.Help_32x32;
            this.HelpButton.Location = new System.Drawing.Point(3, 3);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(56, 61);
            this.HelpButton.TabIndex = 48;
            this.HelpButton.Text = "Help";
            this.HelpButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.HelpButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.HelpButton.UseVisualStyleBackColor = true;
            // 
            // ExitButton
            // 
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.Image = global::ScanningServicesAdmin.Properties.Resources.Exit_32x32;
            this.ExitButton.Location = new System.Drawing.Point(293, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(56, 61);
            this.ExitButton.TabIndex = 54;
            this.ExitButton.Text = "Exit";
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Image = global::ScanningServicesAdmin.Properties.Resources.Save_32x32;
            this.SaveButton.Location = new System.Drawing.Point(61, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(56, 61);
            this.SaveButton.TabIndex = 49;
            this.SaveButton.Text = "Save";
            this.SaveButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // EnablecheckBox
            // 
            this.EnablecheckBox.AutoSize = true;
            this.EnablecheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnablecheckBox.Location = new System.Drawing.Point(17, 83);
            this.EnablecheckBox.Name = "EnablecheckBox";
            this.EnablecheckBox.Size = new System.Drawing.Size(70, 20);
            this.EnablecheckBox.TabIndex = 51;
            this.EnablecheckBox.Text = "Enable";
            this.EnablecheckBox.UseVisualStyleBackColor = true;
            // 
            // JobBatchDeliveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 453);
            this.Controls.Add(this.SupersedeGroupBox);
            this.Controls.Add(this.SupersedeCheckBox);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.ApplyHutton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.EnablecheckBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "JobBatchDeliveryForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "JOB BATCH DELIVERY SETTINGS";
            this.SupersedeGroupBox.ResumeLayout(false);
            this.SupersedeGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.RepeatEveryGroupBox.ResumeLayout(false);
            this.RepeatEveryGroupBox.PerformLayout();
            this.DailyGroupBox.ResumeLayout(false);
            this.DailyGroupBox.PerformLayout();
            this.WeeklyGroupBox.ResumeLayout(false);
            this.WeeklyGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox SupersedeGroupBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox RangeCheckBox;
        private System.Windows.Forms.RadioButton StartRadioButton;
        private System.Windows.Forms.RadioButton RepeatTaskEveryRadioButton;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker BeginDateTimePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DomainUpDown RepeatTaskEveryUpDown;
        private System.Windows.Forms.GroupBox RepeatEveryGroupBox;
        private System.Windows.Forms.RadioButton RepeatMinutesRadioButton;
        private System.Windows.Forms.RadioButton RepeatHoursRadioButton;
        private System.Windows.Forms.RadioButton DailyRadioButton;
        private System.Windows.Forms.RadioButton WeeklyRadioButton;
        private System.Windows.Forms.GroupBox DailyGroupBox;
        private System.Windows.Forms.DomainUpDown RecurDaysTextBoxUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox WeeklyGroupBox;
        private System.Windows.Forms.CheckBox SaturdayCheckBox;
        private System.Windows.Forms.CheckBox FridayCheckBox;
        private System.Windows.Forms.CheckBox ThursdayCheckBox;
        private System.Windows.Forms.CheckBox WednesdayCheckBox;
        private System.Windows.Forms.CheckBox TuesdayCheckBox;
        private System.Windows.Forms.CheckBox MondayCheckBox;
        private System.Windows.Forms.CheckBox SundayCheckBox;
        private System.Windows.Forms.CheckBox SupersedeCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button ApplyHutton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.CheckBox EnablecheckBox;
    }
}