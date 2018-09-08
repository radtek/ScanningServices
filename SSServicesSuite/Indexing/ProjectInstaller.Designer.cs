namespace Indexing
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
            this.IndexingServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.IndexingServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // IndexingServiceProcessInstaller
            // 
            this.IndexingServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.IndexingServiceProcessInstaller.Password = null;
            this.IndexingServiceProcessInstaller.Username = null;
            this.IndexingServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.IndexingServiceProcessInstaller_AfterInstall);
            // 
            // IndexingServiceInstaller
            // 
            this.IndexingServiceInstaller.Description = "Indexing Serices";
            this.IndexingServiceInstaller.DisplayName = "Indexing Serices";
            this.IndexingServiceInstaller.ServiceName = "Service1";
            this.IndexingServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.IndexingServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.IndexingServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.IndexingServiceProcessInstaller,
            this.IndexingServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller IndexingServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller IndexingServiceInstaller;
    }
}