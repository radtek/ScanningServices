namespace VFRUploader
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
            this.VFRUploaderServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.VFRUploaderServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // VFRUploaderServiceProcessInstaller
            // 
            this.VFRUploaderServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.VFRUploaderServiceProcessInstaller.Password = null;
            this.VFRUploaderServiceProcessInstaller.Username = null;
            this.VFRUploaderServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VFRUploaderServiceProcessInstaller_AfterInstall);
            // 
            // VFRUploaderServiceInstaller
            // 
            this.VFRUploaderServiceInstaller.Description = "VFRUploader Serices";
            this.VFRUploaderServiceInstaller.DisplayName = "VFRUploader Serices";
            this.VFRUploaderServiceInstaller.ServiceName = "Service1";
            this.VFRUploaderServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.VFRUploaderServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VFRUploaderServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.VFRUploaderServiceProcessInstaller,
            this.VFRUploaderServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller VFRUploaderServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller VFRUploaderServiceInstaller;
    }
}