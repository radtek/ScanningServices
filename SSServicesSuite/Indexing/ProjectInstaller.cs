using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Indexing
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void IndexingServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void IndexingServiceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
