namespace Synchronizer
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
            this.SynchronizerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SynchronizerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // SynchronizerServiceProcessInstaller
            // 
            this.SynchronizerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.SynchronizerServiceProcessInstaller.Password = null;
            this.SynchronizerServiceProcessInstaller.Username = null;
            this.SynchronizerServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.SynchronizerServiceProcessInstaller_AfterInstall);
            // 
            // SynchronizerServiceInstaller
            // 
            this.SynchronizerServiceInstaller.Description = "Synchronizer Serices";
            this.SynchronizerServiceInstaller.DisplayName = "Synchronizer Serices";
            this.SynchronizerServiceInstaller.ServiceName = "Service1";
            this.SynchronizerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.SynchronizerServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.SynchronizerServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SynchronizerServiceProcessInstaller,
            this.SynchronizerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SynchronizerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SynchronizerServiceInstaller;
    }
}