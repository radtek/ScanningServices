namespace VFRDuplicateRemover
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
            this.VFRDuplicateRemoverServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.VFRDuplicateRemoverServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // VFRDuplicateRemoverServiceProcessInstaller
            // 
            this.VFRDuplicateRemoverServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.VFRDuplicateRemoverServiceProcessInstaller.Password = null;
            this.VFRDuplicateRemoverServiceProcessInstaller.Username = null;
            this.VFRDuplicateRemoverServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VFRDuplicateRemoverServiceProcessInstaller_AfterInstall);
            // 
            // VFRDuplicateRemoverServiceInstaller
            // 
            this.VFRDuplicateRemoverServiceInstaller.Description = "VFRDuplicateRemover Serices";
            this.VFRDuplicateRemoverServiceInstaller.DisplayName = "VFRDuplicateRemover Serices";
            this.VFRDuplicateRemoverServiceInstaller.ServiceName = "Service1";
            this.VFRDuplicateRemoverServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.VFRDuplicateRemoverServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.VFRDuplicateRemoverServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.VFRDuplicateRemoverServiceProcessInstaller,
            this.VFRDuplicateRemoverServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller VFRDuplicateRemoverServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller VFRDuplicateRemoverServiceInstaller;
    }
}