namespace ScanningServicesAdmin.Forms
{
    partial class SchedulingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchedulingForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DailyRadioButton = new System.Windows.Forms.RadioButton();
            this.WeeklyRadioButton = new System.Windows.Forms.RadioButton();
            this.DailyGroupBox = new System.Windows.Forms.GroupBox();
            this.RecurDaysTextBoxUpDown = new System.Windows.Forms.DomainUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.RangeCheckBox = new System.Windows.Forms.CheckBox();
            this.StartRadioButton = new System.Windows.Forms.RadioButton();
            this.RepeatTaskEveryRadioButton = new System.Windows.Forms.RadioButton();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label20 = new System.Windows.Forms.Label();
            this.BeginDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label21 = new System.Windows.Forms.Label();
            this.RepeatTaskEveryUpDown = new System.Windows.Forms.DomainUpDown();
            this.RepeatEveryGroupBox = new System.Windows.Forms.GroupBox();
            this.RepeatMinutesRadioButton = new System.Windows.Forms.RadioButton();
            this.RepeatHoursRadioButton = new System.Windows.Forms.RadioButton();
            this.WeeklyGroupBox = new System.Windows.Forms.GroupBox();
            this.SaturdayCheckBox = new System.Windows.Forms.CheckBox();
            this.FridayCheckBox = new System.Windows.Forms.CheckBox();
            this.ThursdayCheckBox = new System.Windows.Forms.CheckBox();
            this.WednesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.TuesdayCheckBox = new System.Windows.Forms.CheckBox();
            this.MondayCheckBox = new System.Windows.Forms.CheckBox();
            this.SundayCheckBox = new System.Windows.Forms.CheckBox();
            this.EnablecheckBox = new System.Windows.Forms.CheckBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.HelpButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.ApplyHutton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ProcessNameLabel = new System.Windows.Forms.Label();
            this.JobNameLabel = new System.Windows.Forms.Label();
            this.ServiceStationsComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.DailyGroupBox.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.RepeatEveryGroupBox.SuspendLayout();
            this.WeeklyGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DailyRadioButton);
            this.groupBox1.Controls.Add(this.WeeklyRadioButton);
            this.groupBox1.Controls.Add(this.DailyGroupBox);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.WeeklyGroupBox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(9, 199);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 329);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            // 
            // DailyRadioButton
            // 
            this.DailyRadioButton.AutoSize = true;
            this.DailyRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DailyRadioButton.Location = new System.Drawing.Point(23, 41);
            this.DailyRadioButton.Name = "DailyRadioButton";
            this.DailyRadioButton.Size = new System.Drawing.Size(57, 20);
            this.DailyRadioButton.TabIndex = 47;
            this.DailyRadioButton.TabStop = true;
            this.DailyRadioButton.Text = "Daily";
            this.DailyRadioButton.UseVisualStyleBackColor = true;
            this.DailyRadioButton.CheckedChanged += new System.EventHandler(this.DailyRadioButton_CheckedChanged);
            // 
            // WeeklyRadioButton
            // 
            this.WeeklyRadioButton.AutoSize = true;
            this.WeeklyRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WeeklyRadioButton.Location = new System.Drawing.Point(23, 111);
            this.WeeklyRadioButton.Name = "WeeklyRadioButton";
            this.WeeklyRadioButton.Size = new System.Drawing.Size(125, 20);
            this.WeeklyRadioButton.TabIndex = 48;
            this.WeeklyRadioButton.TabStop = true;
            this.WeeklyRadioButton.Text = "Day of the Week";
            this.WeeklyRadioButton.UseVisualStyleBackColor = true;
            this.WeeklyRadioButton.CheckedChanged += new System.EventHandler(this.WeeklyRadioButton_CheckedChanged);
            // 
            // DailyGroupBox
            // 
            this.DailyGroupBox.Controls.Add(this.RecurDaysTextBoxUpDown);
            this.DailyGroupBox.Controls.Add(this.label22);
            this.DailyGroupBox.Controls.Add(this.label23);
            this.DailyGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DailyGroupBox.Location = new System.Drawing.Point(163, 19);
            this.DailyGroupBox.Name = "DailyGroupBox";
            this.DailyGroupBox.Size = new System.Drawing.Size(361, 55);
            this.DailyGroupBox.TabIndex = 49;
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
            this.RecurDaysTextBoxUpDown.Leave += new System.EventHandler(this.RecurDaysTextBoxUpDown_Leave);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 21);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(84, 16);
            this.label22.TabIndex = 39;
            this.label22.Text = "Recur every:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(141, 22);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(38, 16);
            this.label23.TabIndex = 41;
            this.label23.Text = "days";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.RangeCheckBox);
            this.groupBox6.Controls.Add(this.StartRadioButton);
            this.groupBox6.Controls.Add(this.RepeatTaskEveryRadioButton);
            this.groupBox6.Controls.Add(this.EndDateTimePicker);
            this.groupBox6.Controls.Add(this.StartDateTimePicker);
            this.groupBox6.Controls.Add(this.label20);
            this.groupBox6.Controls.Add(this.BeginDateTimePicker);
            this.groupBox6.Controls.Add(this.label21);
            this.groupBox6.Controls.Add(this.RepeatTaskEveryUpDown);
            this.groupBox6.Controls.Add(this.RepeatEveryGroupBox);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(13, 192);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(511, 131);
            this.groupBox6.TabIndex = 51;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = " Advanced ";
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
            this.RangeCheckBox.CheckedChanged += new System.EventHandler(this.RangeCheckBox_CheckedChanged);
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
            this.StartRadioButton.CheckedChanged += new System.EventHandler(this.StartRadioButton_CheckedChanged);
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
            this.RepeatTaskEveryRadioButton.CheckedChanged += new System.EventHandler(this.RepeatTaskEveryRadioButton_CheckedChanged);
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
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(313, 63);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(20, 16);
            this.label20.TabIndex = 41;
            this.label20.Text = "---";
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
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(162, 64);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(63, 16);
            this.label21.TabIndex = 39;
            this.label21.Text = "Between:";
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
            this.RepeatTaskEveryUpDown.Leave += new System.EventHandler(this.RepeatTaskEveryUpDown_Leave);
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
            // WeeklyGroupBox
            // 
            this.WeeklyGroupBox.Controls.Add(this.SaturdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.FridayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.ThursdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.WednesdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.TuesdayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.MondayCheckBox);
            this.WeeklyGroupBox.Controls.Add(this.SundayCheckBox);
            this.WeeklyGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WeeklyGroupBox.Location = new System.Drawing.Point(163, 83);
            this.WeeklyGroupBox.Name = "WeeklyGroupBox";
            this.WeeklyGroupBox.Size = new System.Drawing.Size(361, 97);
            this.WeeklyGroupBox.TabIndex = 50;
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
            // EnablecheckBox
            // 
            this.EnablecheckBox.AutoSize = true;
            this.EnablecheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnablecheckBox.Location = new System.Drawing.Point(12, 180);
            this.EnablecheckBox.Name = "EnablecheckBox";
            this.EnablecheckBox.Size = new System.Drawing.Size(70, 20);
            this.EnablecheckBox.TabIndex = 62;
            this.EnablecheckBox.Text = "Enable";
            this.EnablecheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Image = global::ScanningServicesAdmin.Properties.Resources.Save_32x32;
            this.SaveButton.Location = new System.Drawing.Point(62, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(56, 61);
            this.SaveButton.TabIndex = 63;
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
            this.ExitButton.Location = new System.Drawing.Point(294, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(56, 61);
            this.ExitButton.TabIndex = 64;
            this.ExitButton.Text = "Exit";
            this.ExitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton.Image = global::ScanningServicesAdmin.Properties.Resources.Help_32x32;
            this.HelpButton.Location = new System.Drawing.Point(4, 3);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(56, 61);
            this.HelpButton.TabIndex = 65;
            this.HelpButton.Text = "Help";
            this.HelpButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.HelpButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.HelpButton.UseVisualStyleBackColor = true;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.Image = global::ScanningServicesAdmin.Properties.Resources.Delete_32x32;
            this.DeleteButton.Location = new System.Drawing.Point(236, 3);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(56, 61);
            this.DeleteButton.TabIndex = 68;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.DeleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // ApplyHutton
            // 
            this.ApplyHutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyHutton.Image = global::ScanningServicesAdmin.Properties.Resources.Apply_32x32;
            this.ApplyHutton.Location = new System.Drawing.Point(178, 3);
            this.ApplyHutton.Name = "ApplyHutton";
            this.ApplyHutton.Size = new System.Drawing.Size(56, 61);
            this.ApplyHutton.TabIndex = 67;
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
            this.ResetButton.Location = new System.Drawing.Point(120, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(56, 61);
            this.ResetButton.TabIndex = 66;
            this.ResetButton.Text = "Reset";
            this.ResetButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ResetButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 69;
            this.label1.Text = "Process Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 70;
            this.label2.Text = "Job Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 16);
            this.label3.TabIndex = 71;
            this.label3.Text = "Service Station Name:";
            // 
            // ProcessNameLabel
            // 
            this.ProcessNameLabel.AutoSize = true;
            this.ProcessNameLabel.Location = new System.Drawing.Point(160, 87);
            this.ProcessNameLabel.Name = "ProcessNameLabel";
            this.ProcessNameLabel.Size = new System.Drawing.Size(95, 16);
            this.ProcessNameLabel.TabIndex = 72;
            this.ProcessNameLabel.Text = "ProcessName";
            // 
            // JobNameLabel
            // 
            this.JobNameLabel.AutoSize = true;
            this.JobNameLabel.Location = new System.Drawing.Point(160, 116);
            this.JobNameLabel.Name = "JobNameLabel";
            this.JobNameLabel.Size = new System.Drawing.Size(68, 16);
            this.JobNameLabel.TabIndex = 73;
            this.JobNameLabel.Text = "JobName";
            // 
            // ServiceStationsComboBox
            // 
            this.ServiceStationsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServiceStationsComboBox.FormattingEnabled = true;
            this.ServiceStationsComboBox.Location = new System.Drawing.Point(162, 142);
            this.ServiceStationsComboBox.Name = "ServiceStationsComboBox";
            this.ServiceStationsComboBox.Size = new System.Drawing.Size(228, 24);
            this.ServiceStationsComboBox.TabIndex = 75;
            this.ServiceStationsComboBox.SelectedIndexChanged += new System.EventHandler(this.ServiceStationsComboBox_SelectedIndexChanged);
            // 
            // SchedulingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(564, 532);
            this.Controls.Add(this.ServiceStationsComboBox);
            this.Controls.Add(this.JobNameLabel);
            this.Controls.Add(this.ProcessNameLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.ApplyHutton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.EnablecheckBox);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SchedulingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SCHEDULING SETTINGS";
            this.Load += new System.EventHandler(this.SchedulingForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.DailyGroupBox.ResumeLayout(false);
            this.DailyGroupBox.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.RepeatEveryGroupBox.ResumeLayout(false);
            this.RepeatEveryGroupBox.PerformLayout();
            this.WeeklyGroupBox.ResumeLayout(false);
            this.WeeklyGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton DailyRadioButton;
        private System.Windows.Forms.RadioButton WeeklyRadioButton;
        private System.Windows.Forms.GroupBox DailyGroupBox;
        private System.Windows.Forms.DomainUpDown RecurDaysTextBoxUpDown;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox RangeCheckBox;
        private System.Windows.Forms.RadioButton StartRadioButton;
        private System.Windows.Forms.RadioButton RepeatTaskEveryRadioButton;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.DateTimePicker BeginDateTimePicker;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DomainUpDown RepeatTaskEveryUpDown;
        private System.Windows.Forms.GroupBox RepeatEveryGroupBox;
        private System.Windows.Forms.RadioButton RepeatMinutesRadioButton;
        private System.Windows.Forms.RadioButton RepeatHoursRadioButton;
        private System.Windows.Forms.GroupBox WeeklyGroupBox;
        private System.Windows.Forms.CheckBox SaturdayCheckBox;
        private System.Windows.Forms.CheckBox FridayCheckBox;
        private System.Windows.Forms.CheckBox ThursdayCheckBox;
        private System.Windows.Forms.CheckBox WednesdayCheckBox;
        private System.Windows.Forms.CheckBox TuesdayCheckBox;
        private System.Windows.Forms.CheckBox MondayCheckBox;
        private System.Windows.Forms.CheckBox SundayCheckBox;
        private System.Windows.Forms.CheckBox EnablecheckBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button ApplyHutton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ProcessNameLabel;
        private System.Windows.Forms.Label JobNameLabel;
        private System.Windows.Forms.ComboBox ServiceStationsComboBox;
    }
}