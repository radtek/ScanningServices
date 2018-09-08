using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Indexing
{
    class General
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();
        static object lockObj = new object();

        [DllImport("kernel32.dll")]
        public static extern bool CreateSymbolicLink(
        string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

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

        /// <summary>
        /// This is the color selection Palete for the Console Messages
        /// </summary>
        public enum LogLevel
        {
            Error,
            Warn,
            Info,
            Debug,
            Trace
        };

        /// <summary>
        /// This Method was created to resolve the issue of overlaping messages in the wrong files names
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        /// <param name="logJobName"></param>
        public static void Logger(LogLevel logLevel, string message, string logJobName)
        {
            lock (lockObj)
            {
                LogManager.Configuration.Variables["JobName"] = logJobName;
                switch (logLevel.ToString().ToUpper())
                {
                    case "ERROR":
                        nlogger.Fatal(message);
                        break;
                    case "WARN":
                        nlogger.Warn(message);
                        break;
                    case "INFO":
                        nlogger.Info(message);
                        break;
                    case "DEBUG":
                        nlogger.Debug(message);
                        break;
                    case "TRACE":
                        nlogger.Trace(message);
                        break;
                }
            }
        }

        /// <summary>
        /// This is the color selection Palete for the Console Messages
        /// </summary>
        public enum Color
        {
            White,
            Yellow,
            Green,
            Magenta,
            Cyan,
            Red
        };

        // Used to control the message color in the console
        public static void ConsoleLogger(Color color, string message)
        {
            lock (lockObj)
            {
                //switch (color.ToUpper())
                switch (color.ToString().ToUpper())
                {
                    case "WHITE":
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "YELLOW":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case "GREEN":
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "MAGENTA":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case "RED":
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case "CYAN":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                }
                Console.WriteLine(message);
            }
        }
    }
}
