using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancer
{
    class General
    {

        [DllImport("kernel32.dll")]
        public static extern bool CreateSymbolicLink(
        string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);


        public class BatchFolderInformation
        {
            public int JobID { get; set; }
            public string JobName { get; set; }
            public DateTime TimeStamp { get; set; }           
            public string FolderFullPath { get; set; }
            public string FileExtension { get; set; }
            public string batchDeliveryFolder { get; set; }
            public string batchNumber { get; set; }
            public string currentBatchStatus { get; set; }
            public string backupFolder { get; set; }
        }

        

        public enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        public static bool IsSymbolic(string path)
        {
            FileInfo pathInfo = new FileInfo(path);
            return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }


        /// <summary>
        /// This is used to format the Exception Message 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ErrorMessage(Exception e)
        {
            // Getting the line number
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = e.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = e.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }

            //Getting the Method name
            var s = new StackTrace(e);
            var thisasm = System.Reflection.Assembly.GetExecutingAssembly();
            var methodname = s.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;

            string message = "";
            if (e.InnerException != null)
            {
                message += "Exception type: " + e.InnerException.GetType() + Environment.NewLine +
                           "Exception message: " + e.InnerException.Message + Environment.NewLine +
                           "Stack trace: " + e.InnerException.StackTrace + Environment.NewLine +
                           "Method: " + methodname + Environment.NewLine +
                           "Line: " + lineNumber + Environment.NewLine;
            }
            else
            {
                message += "Exception Message: " + e.Message + Environment.NewLine +
                          "Stack trace: " + e.StackTrace + Environment.NewLine +
                          "Method: " + methodname + Environment.NewLine +
                          "Line: " + lineNumber + Environment.NewLine;
            }
            return message;
            //MessageBox.Show(message, "Error Message ...", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

        }
    }
}
