using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reporting
{
    class General
    {

        /// <summary>
        /// This method will check a url to see that it does not return server or protocol errors
        /// </summary>
        /// <param name="url">The path to check</param>
        /// <returns></returns>
        public bool UrlIsValid(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) //Good requests
                {
                    return true;
                }
                else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                {
                    //log.Warn(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url));
                    return false;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                {
                    return false;
                }
                else
                {
                    //log.Warn(String.Format("Unhandled status [{0}] returned for url: {1}", ex.Status, url), ex);
                }
            }
            catch (Exception ex)
            {
                //log.Error(String.Format("Could not test url {0}.", url), ex);
            }
            return false;
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

        }
    }
}
