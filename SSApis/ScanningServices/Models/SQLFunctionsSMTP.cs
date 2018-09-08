using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using System;
using System.Net.Mail;
using System.IO;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLFunctionsSMTP
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get SMTP Server Information
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultSMTP GetSMTPInfo()
        {
            GlobalVars.SMTP smtp = new GlobalVars.SMTP();
            GlobalVars.ResultSMTP resultSMTP = new GlobalVars.ResultSMTP()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = smtp,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetSMTPInfo Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Smtp.FirstOrDefault();
                    if (results != null)
                    {
                        resultSMTP.RecordsCount = 1;
                        smtp.HostName = (results.HostName ?? "").Trim();
                        smtp.PortNumber = results.PortNumber;
                        smtp.SenderEmailAddress = (results.SenderEmailAddress ?? "").Trim();
                        smtp.EnableSSLFlag = Convert.ToBoolean(results.EnableSslflag);
                        smtp.SenderName = (results.SenderName ?? "").Trim();
                        smtp.UserName = (results.UserName ?? "").Trim();
                        smtp.Password = (results.Password ?? "").Trim();
                    }
                    else
                    {
                        //There is no record in the database
                    }
                }
                resultSMTP.ReturnValue = smtp;
                resultSMTP.Message = "GetSMTPInfo transaction completed successfully. Number of records found: " + resultSMTP.RecordsCount;
                logger.Debug(resultSMTP.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultSMTP.ReturnCode = -2;
                resultSMTP.Message = e.Message;
                var baseException = e.GetBaseException();
                resultSMTP.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetSMTPInfo Method ...");
            return resultSMTP;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateSMTP(GlobalVars.SMTP smtp)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into UpdateSMTP Method ...");

                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Customer Names must be unique in the Database. The Name could be change but it must be unique
                    Smtp Matching_Result = DB.Smtp.FirstOrDefault();
                    Smtp record = new Smtp();
                    record.HostName = smtp.HostName;
                    record.PortNumber = smtp.PortNumber;
                    record.SenderEmailAddress = smtp.SenderEmailAddress;
                    record.SenderName = smtp.SenderName;
                    record.UserName = smtp.UserName;
                    record.Password = smtp.Password;
                    record.EnableSslflag = smtp.EnableSSLFlag.ToString();


                    if (Matching_Result == null)
                    {
                        // DB.Smtp.Add(record);
                        DB.Smtp.Add(record);
                        DB.SaveChanges();
                        result.Message = "There was not information associated to an SMTP Server, so new records was created successfully.";
                    }
                    else
                    {
                        // Means --> table has a record and it will be updated
                        Matching_Result.HostName = smtp.HostName;
                        Matching_Result.PortNumber = smtp.PortNumber;
                        Matching_Result.SenderEmailAddress = smtp.SenderEmailAddress;
                        Matching_Result.SenderName = smtp.SenderName;
                        Matching_Result.UserName = smtp.UserName;
                        Matching_Result.Password = smtp.Password;
                        Matching_Result.SenderName = record.SenderName;
                        DB.SaveChanges();
                        result.Message = "SMTP Inforation was updated successfully.";
                    }
                }
                logger.Debug(result.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving UpdateSMTP Method ...");
            return result;
        }


        /// <summary>
        /// Send Email 
        /// </summary>
        /// <returns> Return 0 if the email was sent successfully</returns>
        static public GlobalVars.ResultGeneric SendEmail(GlobalVars.EMAIL email)
        {
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into SendEmail Method ...");
               
                // Get SMTP Server Information
                GlobalVars.ResultSMTP resultSMTP = new GlobalVars.ResultSMTP();
                resultSMTP = SQLFunctionsSMTP.GetSMTPInfo();
                GlobalVars.SMTP smpt = new GlobalVars.SMTP();
                smpt = resultSMTP.ReturnValue;

                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();

                mail.From = new MailAddress(smpt.SenderEmailAddress, smpt.SenderName);
                //mail.To.Add(new MailAddress(email.RecipientsEmailAddress));

                foreach (var address in email.RecipientsEmailAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(address);
                }

                mail.Subject = email.Subject;
                mail.Body = email.Body;
                mail.IsBodyHtml = true;

                if (email.HasAttachment)
                {
                    mail.Attachments.Add(new Attachment(new MemoryStream(email.attachment), "Report.pdf"));
                }
                
                client.Port = smpt.PortNumber;
                client.Host = smpt.HostName;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                if (resultSMTP.ReturnValue.EnableSSLFlag)
                {
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential(smpt.UserName, smpt.Password);
                }
                else
                {
                    client.EnableSsl = false;
                    client.UseDefaultCredentials = true;
                }               

                logger.Trace("  Sender: " + smpt.SenderName);
                logger.Trace("  Recipients: " + email.RecipientsEmailAddress);
                logger.Trace("  Subject: " + email.Subject);
                
                client.Send(mail);
                logger.Trace("Eamil was sent sucessfully.");

            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving SendEmail Method ...");
            return result;
        }

    }
}
