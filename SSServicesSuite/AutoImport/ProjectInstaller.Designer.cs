namespace AutoImport
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
            this.AutoImportServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AutoImportServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AutoImportServiceProcessInstaller
            // 
            this.AutoImportServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.AutoImportServiceProcessInstaller.Password = null;
            this.AutoImportServiceProcessInstaller.Username = null;
            // 
            // AutoImportServiceInstaller
            // 
            this.AutoImportServiceInstaller.Description = "Auto Impor Serices";
            this.AutoImportServiceInstaller.DisplayName = "Auto Impor Serices";
            this.AutoImportServiceInstaller.ServiceName = "Service1";
            this.AutoImportServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.AutoImportServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.AutoImportServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AutoImportServiceProcessInstaller,
            this.AutoImportServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AutoImportServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AutoImportServiceInstaller;
    }
}