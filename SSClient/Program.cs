//Use this Web Site to download Free Icons:
// https://www.flaticon.com/packs/tech-support-8
// http://www.iconarchive.com/search?q=admin

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanningServicesAdmin
{

    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            Application.Run(new Forms.SSSMainForm());
        }
    }
}
