namespace BatchRemover
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
            this.BatchRemoverServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.BatchRemoverServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // BatchRemoverServiceProcessInstaller
            // 
            this.BatchRemoverServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.BatchRemoverServiceProcessInstaller.Password = null;
            this.BatchRemoverServiceProcessInstaller.Username = null;
            this.BatchRemoverServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.BatchRemoverServiceProcessInstaller_AfterInstall);
            // 
            // BatchRemoverServiceInstaller
            // 
            this.BatchRemoverServiceInstaller.Description = "BatchRemover Serices";
            this.BatchRemoverServiceInstaller.DisplayName = "BatchRemover Serices";
            this.BatchRemoverServiceInstaller.ServiceName = "Service1";
            this.BatchRemoverServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.BatchRemoverServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.BatchRemoverServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.BatchRemoverServiceProcessInstaller,
            this.BatchRemoverServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller BatchRemoverServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller BatchRemoverServiceInstaller;
    }
}