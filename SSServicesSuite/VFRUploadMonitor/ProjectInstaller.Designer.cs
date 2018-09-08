namespace VFRUploadMonitor
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VFRUploadMonitorServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.VFRUploadMonitorServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // VFRUploadMonitorServiceProcessInstaller
            // 
            this.VFRUploadMonitorServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.VFRUploadMonitorServiceProcessInstaller.Password = null;
            this.VFRUploadMonitorServiceProcessInstaller.Username = null;
            this.VFRUploadMonitorServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VFRUploadMonitorServiceProcessInstaller_AfterInstall);
            // 
            // VFRUploadMonitorServiceInstaller
            // 
            this.VFRUploadMonitorServiceInstaller.Description = "VFRUploadMonitor Serices";
            this.VFRUploadMonitorServiceInstaller.DisplayName = "VFRUploadMonitor Serices";
            this.VFRUploadMonitorServiceInstaller.ServiceName = "Service1";
            this.VFRUploadMonitorServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.VFRUploadMonitorServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VFRUploadMonitorServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.VFRUploadMonitorServiceProcessInstaller,
            this.VFRUploadMonitorServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller VFRUploadMonitorServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller VFRUploadMonitorServiceInstaller;
    }
}