namespace BatchDelivery
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
            this.BatchDeliveryServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.BatchDeliveryServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // BatchDeliveryServiceProcessInstaller
            // 
            this.BatchDeliveryServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.BatchDeliveryServiceProcessInstaller.Password = null;
            this.BatchDeliveryServiceProcessInstaller.Username = null;
            // 
            // BatchDeliveryServiceInstaller
            // 
            this.BatchDeliveryServiceInstaller.Description = "Batch Delivery Serices";
            this.BatchDeliveryServiceInstaller.DisplayName = "Batch Delivery Serices";
            this.BatchDeliveryServiceInstaller.ServiceName = "Service1";
            this.BatchDeliveryServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.BatchDeliveryServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.BatchDeliveryServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.BatchDeliveryServiceProcessInstaller,
            this.BatchDeliveryServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller BatchDeliveryServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller BatchDeliveryServiceInstaller;
    }
}