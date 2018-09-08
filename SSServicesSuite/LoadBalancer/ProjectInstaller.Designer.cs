namespace LoadBalancer
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
            this.LoadBalancerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.LoadBalancerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // LoadBalancerServiceProcessInstaller
            // 
            this.LoadBalancerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.LoadBalancerServiceProcessInstaller.Password = null;
            this.LoadBalancerServiceProcessInstaller.Username = null;
            this.LoadBalancerServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.LoadBalancerServiceProcessInstaller_AfterInstall);
            // 
            // LoadBalancerServiceInstaller
            // 
            this.LoadBalancerServiceInstaller.Description = "Load Balancer Serices";
            this.LoadBalancerServiceInstaller.DisplayName = "Load Balancer Serices";
            this.LoadBalancerServiceInstaller.ServiceName = "Service1";
            this.LoadBalancerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.LoadBalancerServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.LoadBalancerServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.LoadBalancerServiceProcessInstaller,
            this.LoadBalancerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller LoadBalancerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller LoadBalancerServiceInstaller;
    }
}