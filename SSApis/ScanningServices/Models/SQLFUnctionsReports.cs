using System.Collections.Generic;
using System.Linq;
using NLog;
using ScanningServices.MSSQLEntities;
using ScanningServicesDataObjects;
using System;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
//using iTextSharp.tool.xml;

namespace ScanningServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class SQLFunctionsReports
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalDate"></param>
        /// <param name="workDays"></param>
        /// <returns></returns>
        public static DateTime AddBusinessDays(this DateTime originalDate, int workDays)
        {
            DateTime tmpDate = originalDate;
            while (workDays > 0)
            {
                tmpDate = tmpDate.AddDays(1);
                if (tmpDate.DayOfWeek < DayOfWeek.Saturday &&
                    tmpDate.DayOfWeek > DayOfWeek.Sunday)
                    workDays--;
            }
            return tmpDate;
        }

        /// <summary>
        /// This reports is used to display Batches that are ready for customer approval
        /// for a given Customer 
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric BatchesReadyForCustomerApprovalReport(int reportID)
        {
            string body = "";
            string dateFrom = "";
            string dateTo = "";
            string startTime = "00:00 AM";
            string endTime = "00:00 AM";
            Boolean businessDaysOnly = false;
            int appovalPeriodDays = 0;
            int vfrDocCounter = 0;
            int totalDocumentCounter = 0;

            DateTime today = DateTime.Now;            

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into BatchesReadyForCustomerApprovalReport Method ...");
                result.BooleanReturnValue = false;

                

                // Get Report Settings to be used
                GlobalVars.ResultReports resultReport = new GlobalVars.ResultReports();
                resultReport = SQLFunctionsReports.GetReportByID(reportID);

                if (resultReport.RecordsCount >= 1)
                {
                    // Report was found
                    GlobalVars.Report report = resultReport.ReturnValue[0];

                    //Setting the Day range for the Report
                    // This reports start from the day before to the day the report is called to the current day
                    // i.e: if the eport is executed on a Tuesday, the count will start from Monday to Tuesday in the 
                    // time fram especified in the start and end time
                    // if the report is call a Monday, the count will start 3 day before, in other words , on Friday
                    // For the expiration date, we have to account for the flag BusinessDays={tue/false}
                    if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dateFrom = today.AddDays(-3).ToString("mm/dd/yyyy");
                    }
                    //dateTo = today.ToString("mm/dd/yyyy");
                    // Get information from the Reports Parameters ...
                    foreach (GlobalVars.ReportParameter parameter in report.Parameters)
                    {
                        if (parameter.ParameterName.Trim() == "BusinessDaysOnly")
                        {
                            businessDaysOnly = Convert.ToBoolean(parameter.Value);
                        }
                        if (parameter.ParameterName.Trim() == "ApprovalPeriodDays")
                        {
                            appovalPeriodDays = Convert.ToInt32(parameter.Value);
                        }
                        switch (parameter.ParameterName.Trim())
                        {
                            case "BusinessDaysOnly":
                                businessDaysOnly = Convert.ToBoolean(parameter.Value);
                                break;
                            case "ApprovalPeriodDays":
                                appovalPeriodDays = Convert.ToInt32(parameter.Value);
                                break;
                            case "PreviousDayStartTime":
                                startTime = parameter.Value;
                                break;
                            case "CurrentDayEndTime":
                                endTime = parameter.Value;
                                break;
                        }
                    }
                    // Build the dateTo
                    if (businessDaysOnly)
                    {           
                        dateTo = AddBusinessDays(today, appovalPeriodDays).ToString("mm/dd/yyy");
                    }
                    else
                    {
                        dateTo = today.AddDays(appovalPeriodDays).ToString("mm/dd/yyy");
                    }
                    
                    // Header
                    body = "<!DOCTYPE HTML PUBLIC \"-//W3C/DTD HTML 4.0 Transitional//EN\">";
                    body += "<HTML>";
                    body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                    body += "<BODY>";
                    body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1200'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    if (report.TitleContent1.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag1)
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                    report.TitleContent1 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent2.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag2)
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" +
                                    "<b>" + report.TitleContent2 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" +
                                    report.TitleContent2 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent3.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag3)
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                    report.TitleContent3 + "</font></td>";
                        body += "</tr>";
                    }
                    body += "</tbody>";
                    body += "</TABLE>";
                    

                    //Parameters are as follow:
                    //'''     <Parameter Name="ApprovalPeriodDays" Value="7"/> : determine the number of days the customer has to approve the batches
                    //'''		<Parameter Name="BusinessDaysOnly" Value="true"/>: determine is business day only are use to do the calculation
                    //'''		<Parameter Name="PreviousDayStartTime" Value="06:00"/>: start time from the previus day
                    //'''		<Parameter Name="CurrentDayEndTime" Value="06:00"/>: end time from the current day
                    //'''		<Parameter Name="ShowReprocessedBatches" Value="true"/>: to show re-processed batches in a table
                    //'''		<Parameter Name="ResetVFRUploadDate" Value="true"/> : to reset the VFRUploadDate field in SSS Database when there is differecnce between SSS and VFR Documents count
                    //'''		<Parameter Name="ShowDocumentsCountDiscrepancy" Value="true"/>: to show Documents Countn Discrepancy in a table

                    //'Calculate Acceptance day based on BusinessDaysOnly flag in configuration file
                    //    If bBusinessDaysOnly Then
                    //        dDueDate = AddDays(dDateFrom, lApprovalPeriodDays)
                    //        body = body & "<tr align='center'><H3>Automatic Acceptance on: " & Replace(dDueDate, "-", "/") & "</H3></tr>"
                    //    Else
                    //        dDueDate = dDateFrom.AddDays(lApprovalPeriodDays)
                    //        body = body & "<tr align='center'><H3>Automatic Acceptance on: " & Replace(dDueDate, "-", "/") & "</H3></tr>"
                    //    End If

                    //body = body & "<tr align='center'><H3>Date Available: " & Replace(Date.Today, "-", "/") & "</H3></tr>"
                    //If Date.Today.DayOfWeek = DayOfWeek.Monday Then
                    //  body = body & "<tr align='center'><H3>Automatic Acceptance on: " & Replace(Date.Today.AddDays(+5), "-", "/") & "</H3></tr>"
                    //Else
                    //  body = body & "<tr align='center'><H3>Automatic Acceptance on: " & Replace(Date.Today.AddDays(+7), "-", "/") & "</H3></tr>"
                    //End If



                    GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                    // Get Projects for a given Customer
                    GlobalVars.ResultProjects resultProjects = new GlobalVars.ResultProjects();
                    resultProjects = SQLFunctionsProjects.GetProjectByCustomerID(report.CustomerID);
                    foreach (GlobalVars.Project project in resultProjects.ReturnValue)
                    {
                        // Get Jobs for a given Project
                        GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
                        resultJobs = SQLFunctionsJobs.GetJobByProjectID(project.ProjectID);
                        foreach (GlobalVars.JobExtended job in resultJobs.ReturnValue)
                        {
                            resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"Waiting for Approval\") " +
                                                                                      "AND (VFRUploadDate >= \"" + dateFrom + " " + startTime + "\") " +
                                                                                       "AND (VFRUploadDate <= \"" + dateTo + " " + endTime + "\") " +
                                                                                      "AND (Customer = \"" + report.CustomerName + "\") " +
                                                                                      "AND (ProjectName = \"" + project.ProjectName + "\") " +
                                                                                      "AND (JobName = \"" + job.JobName + "\")", "BatchNumber");
                            if (resultBatches.RecordsCount != 0)
                            {
                                body += "<TABLE border='1' cellpadding='2' cellspacing='1' width='1400'>";
                                body += "<tbody bgcolor='" + report.TableHeaderBackColor + "' align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                                body += "<tr>";
                                body += "<td width='300'>Batch Name</td>";
                                body += "<td width='300'>Number of Documents</td>";
                                body += "<td width='300'>VFR Docs</td>";
                                body += "<td width='300'>Batch Status</td>";
                                body += "</tr>";

                                foreach (GlobalVars.Batch batch in resultBatches.ReturnValue)
                                {
                                    vfrDocCounter = 0;
                                    //                        VFRConnection = ProjectVFRConnectionInfo(sCustomer, sProject)
                                    //                        lVFRDocCounter = VFRBatchDocumentCount(VFRConnection.UserName, VFRConnection.Password, VFRConnection.CADIWebServiceURL, _
                                    //                                                               VFRConnection.InstanceName, VFRConnection.QueryField, VFRUploadStatistics(lNum).BatchName, _
                                    //                                                               VFRConnection.RepositoryName)
                                    //  If(lVFRDocCounter.ToString = VFRUploadStatistics(lNum).NumberOfDocuments) Then
                                    body += "<tr>";
                                    body += "<td width='300'>" + batch.BatchNumber + "</td>";
                                    body += "<td width='300'>" + batch.NumberOfDocuments + "</td>";
                                    //                             body += "<td width='300'>" & lVFRDocCounter & "</td>"
                                    body += "<td width='300'>" + batch.StatusFlag + "</td>";
                                    body += "</tr>";
                                    //   totalDocumentCounter = TotalDocumentCounter + VFRUploadStatistics(lNum).NumberOfDocuments
                                    //  Else
                                    //     bIssueFlag = True
                                    //  End If
                                }

                                body += "</tbody>";
                                body += "</TABLE>";

                                if (totalDocumentCounter > 0)
                                {
                                    body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1400'>";
                                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                                    body += "<tr>";
                                    body += "<tr align='center'><H3>Total Number of Documents: " + totalDocumentCounter + "</H3></tr>";
                                    body += "<br/>";
                                    body += "<tr>Any document not accepted by customer will not be invoiced until corrected by COMPU-DATA and resubmitted for approval by the customer.</tr>";
                                    body += "<br/>";
                                    body += "</tbody>";
                                    body += "</TABLE>";
                                }


                                //'THIS SECTIONS REPORT THE RE-UPLOADED BATCHES IN VFR
                                //        If bShowReprocesdBatches Then
                                //            ReDim VFRUploadStatistics(0)
                                //            TotalDocumentsReprocessed = 0
                                //            Call GetVFRUploadStatisticsExtended(VFRUploadStatistics, dDateFrom, sPreviousDayStartTime, dDateTo, sCurrentDayEndTime, TotalDocumentsReprocessed, sCustomer, sProject)
                                //            If TotalDocumentsReprocessed<> 0 Then
                                //               bSendEmail = True
                                body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1400'>";
                                body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                                body += "<tr>";
                                body += "<tr align='center'><H2>Reprocessed Batches</H2></tr>";
                                body += "<br/>";
                                body += "</tbody>";
                                body += "</TABLE>";
                                body += "<TABLE border='1' cellpadding='2' cellspacing='1' width='500'>";
                                body += "<tbody bgcolor='" + report.TableHeaderBackColor + "' align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                                body += "<tr>";
                                body += "<td width='300'>Batch Name</td>";
                                body += "<td width='300'>Number of Documents</td>";
                                body += "<td width='300'>VFR Docs</td>";
                                body += "<td width='300'>Batch Status</td>";
                                body += "</tr>";

                                //                If Not VFRUploadStatistics Is Nothing Then
                                foreach (GlobalVars.Batch batch in resultBatches.ReturnValue)
                                {
                                    vfrDocCounter = 0;
                                    //                        VFRConnection = ProjectVFRConnectionInfo(sCustomer, sProject)
                                    //                        lVFRDocCounter = VFRBatchDocumentCount(VFRConnection.UserName, VFRConnection.Password, VFRConnection.CADIWebServiceURL, _
                                    //                                                                   VFRConnection.InstanceName, VFRConnection.QueryField, VFRUploadStatistics(lNum).BatchName, _
                                    //                                                                   VFRConnection.RepositoryName)
                                    //                        If(lVFRDocCounter.ToString = VFRUploadStatistics(lNum).NumberOfDocuments) Then
                                    body += "<tr>";
                                    body += "<td width='300'>" + batch.BatchNumber + "</td>";
                                    body += "<td width='300'>" + batch.NumberOfDocuments + "</td>";
                                    //                             body += "<td width='300'>" & lVFRDocCounter & "</td>"
                                    body += "<td width='300'>" + batch.StatusFlag + "</td>";
                                    body += "</tr>";
                                    //                            TotalDocumentsReprocessedCounter = TotalDocumentsReprocessedCounter + VFRUploadStatistics(lNum).NumberOfDocuments
                                    //                        Else
                                    body += "<td BGCOLOR='#ffff00' width='300'>" + batch.BatchNumber + "</td>";
                                    body += "<td BGCOLOR='#ffff00' width='300'>" + batch.NumberOfDocuments + "</td>";
                                    //                            'body &= "<td BGCOLOR='#ffff00' width='300'>" & lVFRDocCounter & "</td>"
                                    body += "<td BGCOLOR='#ffff00' width='300'>" + batch.StatusFlag + "</td>";
                                    //                        End If
                                    //                        Call GenerateFile(sLogFile, Now &Space(8) & "Batch Name: " & VFRUploadStatistics(lNum).BatchName, bDAFDebug)
                                    //                        Call GenerateFile(sLogFile, Now &Space(10) & "NUmber of Documents in SSS: " & VFRUploadStatistics(lNum).NumberOfDocuments, bDAFDebug)
                                    //                        Call GenerateFile(sLogFile, Now &Space(10) & "Total Documents Reporcessed: " & TotalDocumentsReprocessedCounter, bDAFDebug)

                                    //                    Next
                                }
                                //                End If
                                body += "</tbody>";
                                body += "</TABLE>";
                                body += "<br/>";

                                //                If TotalDocumentsReprocessedCounter > 0 Then
                                body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1400'>";
                                body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                                body += "<tr>";
                                //                    body = body & "<tr align='center'><H3>Total Number of Documents: " & TotalDocumentsReprocessedCounter & "</H3></tr>"
                                body += "<br/>";
                                body += "<br/>";
                                body += "<tr>Any document not accepted by customer will not be invoiced until corrected by COMPU-DATA and resubmitted for approval by the customer.</tr>";
                                body += "<br/>";
                                body += "</tbody>";
                                body += "</TABLE>";
                                //                End If

                                //            End If
                                //        End If

                                //        If TotalDocumentsReprocessedCounter = 0 And TotalDocumentCounter = 0 Then
                                body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1400'>";
                                body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                                body += "<tr>";
                                body += "<tr align='center'><H3>The total Number of Batches to report is : 0 </H3></tr>";
                                body += "<br/>";
                                body += "<br/>";
                                body += "<tr>Any document not accepted by customer will not be invoiced until corrected by COMPU-DATA and resubmitted for approval by the customer.</tr>";
                                body += "<br/>";
                                body += "</tbody>";
                                body += "</TABLE>";
                                //        End If


                                //        'THIS SECTIONS REPORT THE UPLOADED BATCHES IN VFR WITH DISCREPANCY IN THE NUMBER OF PAGES BETWEEN VFR AND SSS
                                //        If bShowDocumentsCountDiscrepancy Then
                                //            'body = body & "<br/>"
                                //            'body = body & "<br/>"
                                //            If bIssueFlag Then
                                //                Call GetVFRUploadStatistics(VFRUploadStatistics, dDateFrom, sPreviousDayStartTime, dDateTo, sCurrentDayEndTime, TotalDocuments, sCustomer, sProject)
                                //                TotalDocumentCounter = 0
                                //                If TotalDocuments<> 0 Then
                                //                   bSendEmail = True
                                //                    body &= "<TABLE border='0' cellpadding='1' cellspacing='1' width='1400'>"
                                //                    body &= "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>"
                                //                    body &= "<tr>"
                                //                    body = body & "<tr align='center'><H3>Batches that need attention</H3></tr>"
                                //                    body &= "</tbody>"
                                //                    body &= "</TABLE>"

                                body += "<TABLE border='1' cellpadding='2' cellspacing='1' width='1400'>";
                                //body += "<tbody bgcolor='" & ReportHeader.ReportTable.HeaderBackGroundColor & "' align='center' style='font-family:verdana; color:'black'; background-color:'silver'>"
                                body += "<tr>";
                                body += "<td width='300'>Batch Name</td>";
                                body += "<td width='300'>Number of Documents</td>";
                                body += "<td width='300'>VFR Docs</td>";
                                body += "<td width='300'>Batch Status</td>";
                                body += "</tr>";

                                //                    'If Not VFRUploadStatistics Is Nothing Then
                                //                    If Not VFRUploadStatistics Is Nothing Then
                                //                        For lNum = 0 To UBound(VFRUploadStatistics)
                                //                            lVFRDocCounter = 0
                                //                            If VFRUploadStatistics(lNum).Customer = sCustomer Then
                                //                                VFRConnection = ProjectVFRConnectionInfo(sCustomer, sProject)
                                //                                lVFRDocCounter = VFRBatchDocumentCount(VFRConnection.UserName, VFRConnection.Password, VFRConnection.CADIWebServiceURL, _
                                //                                                                       VFRConnection.InstanceName, VFRConnection.QueryField, VFRUploadStatistics(lNum).BatchName, _
                                //                                                                       VFRConnection.RepositoryName)
                                //                                If(lVFRDocCounter.ToString = VFRUploadStatistics(lNum).NumberOfDocuments) Then
                                //                                    'body &= "<td width='300'>" & VFRUploadStatistics(lNum).BatchName & "</td>"
                                //                                    'body &= "<td width='300'>" & VFRUploadStatistics(lNum).NumberOfDocuments & "</td>"
                                //                                    'body &= "<td width='300'>" & lVFRDocCounter & "</td>"
                                //                                    'body &= "<td width='300'>" & VFRUploadStatistics(lNum).StatusFlag & "</td>"
                                //                                Else
                                //                                    body &= "<tr>"
                                //                                    body &= "<td BGCOLOR='#ffff00' width='300'>" & VFRUploadStatistics(lNum).BatchName & "</td>"
                                //                                    body &= "<td BGCOLOR='#ffff00' width='300'>" & VFRUploadStatistics(lNum).NumberOfDocuments & "</td>"
                                //                                    body &= "<td BGCOLOR='#ffff00' width='300'>" & lVFRDocCounter & "</td>"
                                //                                    body &= "<td BGCOLOR='#ffff00' width='300'>" & VFRUploadStatistics(lNum).StatusFlag & "</td>"
                                //                                    body &= "</tr>"
                                //                                    TotalDocumentCounter = TotalDocumentCounter + VFRUploadStatistics(lNum).NumberOfDocuments
                                //                                    'reset VFRUpdateDaaae and VFRUploadModifiedDate
                                //                                    If bResetVFRUploadDate Then
                                //                                        Call ResetVFRUploadDate(VFRUploadStatistics(lNum).BatchName)
                                //                                    End If
                                //                                End If
                                //                            End If
                                //                        Next
                                //                    End If
                                //                    body &= "</tbody>"
                                //                    body &= "</TABLE>"
                                //                End If

                                //                If TotalDocumentCounter > 0 Then
                                body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1400'>";
                                body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                                body += "<tr>";
                                //body = body & "<tr align='center'><H3>Total Number of Documents: " & TotalDocumentCounter & "</H3></tr>"
                                body += "<br/>";
                                //                    'body = body & "<br/>"
                                //                    'body = body & "<tr>Any document not accepted by customer will not be invoiced until corrected by COMPU-DATA and resubmitted for approval by the customer.</tr>"
                                //                    'body = body & "<br/>"
                                body += "</tbody>";
                                body += "</TABLE>";
                                //                End If
                                //            End If                                
                            }

                        }
                    }                    
                    
                    

                }
                body += "<br/>";
                body += "</BODY></HTML>";

                result.StringReturnValue = body;
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving BatchesReadyForCustomerApprovalReport Method ...");
            return result;
        }

        /// <summary>
        /// This method quey DAF by a Batch Number and rertuns the number of documents for this Batch in DAF
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="cadiURL"></param>
        /// <param name="dafInstance"></param>
        /// <param name="queyField"></param>
        /// <param name="fieldValue"></param>
        /// <param name="repositoryName"></param>
        /// <returns></returns>
        public static int VFRBatchDocumentCount(string userName, string password, string cadiURL, string dafInstance, string queyField, string fieldValue, string repositoryName)
        {
            int documentCount = 0;
            return documentCount;
        }

    //    Public Function VFRBatchDocumentCount(ByVal CADIUsername As String, ByVal CADIPassword As String, ByVal CADIURL As String, ByVal CADIInstance As String, ByVal queryField As String, ByVal queryValue As String, ByVal CADIRepository As String) As Double
    //    Dim actualCount As Integer = -1
    //    Dim query As String
    //    Dim resultStr As String

    //    'INFORMATION TO BE CAPTURE FROM CONFIGURATIN FILE
    //    'CADIUsername = "l4FVOXOkrk/wyynz7+Epvg=="
    //    'CADIPassword = "/3Tz7TwDxYXdKSES5nrAnA=="
    //    'CADIURL = "http://SCANSTATION5:82/CADIWebService.svc"
    //    'CADIInstance = "HCSO"
    //    'queryField = "BoxNumber"
    //    'queryValue = "0241000-0242549.2"
    //    'CADIRepository = "HCSO"

    //    VFRBatchDocumentCount = 0
    //    Try
    //        Dim client As New CADIWS.CADIWebServiceClient("BasicHttpBinding_ICADIWebService", CADIURL)
    //        Dim crypto As New Cdi.Tools.CryptoTools
    //        Dim userIdKey As String
    //        userIdKey = client.DAF_LogIn(crypto.decrypt(CADIUsername), crypto.decrypt(CADIPassword), CADIInstance).useridkey

    //        Call GenerateFile(sLogFile, Now & Space(8) & "CADI Username: " & crypto.decrypt(CADIUsername), bDAFDebug)
    //        Call GenerateFile(sLogFile, Now & Space(8) & "CADI Password: " & crypto.decrypt(CADIPassword), bDAFDebug)
    //        Call GenerateFile(sLogFile, Now & Space(8) & "CADI URL: " & CADIURL, bDAFDebug)
    //        Call GenerateFile(sLogFile, Now & Space(8) & "CADI Instance: " & CADIInstance, bDAFDebug)
    //        Call GenerateFile(sLogFile, Now & Space(8) & "CADI Repository:" & CADIRepository, bDAFDebug)
    //        Call GenerateFile(sLogFile, Now & Space(8) & "CADI User Id Key: " & userIdKey, bDAFDebug)
    //        Dim ABC = New CADIWS.StructuresRepository

    //        If userIdKey.Length<> 0 Then
    //           query = "{'CADIQuery':{'scope': {'scope_daf': {'repository': " & _
    //                    "{'@name': '" & CADIRepository & "'}" & _
    //                    ", 'identity': {'username': '" & crypto.decrypt(CADIUsername) + "'}}}, 'metadata' : { " & _
    //                    "'booleanfilters' : { 'booleanfilter': {'@field': 'fpd_asset_type', '#text':'MEMBER', '@exclude':'true'}}," & _
    //                    "'booleanfilters' : { 'booleanfilter': {'@field': '" & queryField & "', '#text':'" & queryValue + "'}}" & _
    //                    "}, hits :{ maxhits: 100000}}"
    //            resultStr = Nothing
    //            Dim queryResults = client.RunQuery(query, "json")
    //            resultStr = queryResults
    //            Dim resultObj = JObject.Parse(queryResults)
    //            Dim results = resultObj("CADIQueryResults")

    //            If(results IsNot Nothing) Then
    //               If results("errors").Type = JTokenType.Null Then
    //                    VFRBatchDocumentCount = results("totalitems")
    //                End If
    //            End If

    //            Call GenerateFile(sLogFile, Now & Space(8) & "VRF-CADI API", bDAFDebug)
    //            Call GenerateFile(sLogFile, Now & Space(10) & "Field: " & queryField, bDAFDebug)
    //            Call GenerateFile(sLogFile, Now & Space(10) & "Value: " & queryValue, bDAFDebug)
    //            Call GenerateFile(sLogFile, Now & Space(10) & "Num of Docs: " & VFRBatchDocumentCount, bDAFDebug)
    //            Call GenerateFile(sLogFile, Now & Space(8) & resultStr, True)

    //        Else
    //            Call GenerateFile(sLogFile, Now & Space(8) & "VRF-CADI API: Empty UserIDKey", bDAFDebug)
    //        End If

    //        'MessageBox.Show("Number of documents retuned: " & actualCount, "My Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)


    //    Catch ex As Exception
    //        Call GenerateFile(sLogFile, Now & Space(8) & "VRF-CADI API ERROR: " & ex.Message, bDAFDebug)
    //    End Try

    //End Function

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric BatchByStatusReport(int reportID)
        {
            string body = "";

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into BatchByStatusReport Method ...");
                result.BooleanReturnValue = false;

                // Get Report Information to be used in the report
                GlobalVars.ResultReports resultReport = new GlobalVars.ResultReports();
                resultReport = SQLFunctionsReports.GetReportByID(reportID);

                if (resultReport.RecordsCount >= 1)
                {
                    // Report was found
                    GlobalVars.Report report = resultReport.ReturnValue[0];

                    // Header
                    body = "<!DOCTYPE HTML PUBLIC \"-//W3C/DTD HTML 4.0 Transitional//EN\">";
                    body += "<HTML>";
                    body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                    body += "<BODY>";
                    body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1200'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    if (report.TitleContent1.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag1)
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                    report.TitleContent1 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent2.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag2)
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" +
                                    "<b>" + report.TitleContent2 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" +
                                    report.TitleContent2 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent3.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag3)
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                    report.TitleContent3 + "</font></td>";
                        body += "</tr>";
                    }
                    body += "</tbody>";
                    body += "</TABLE>";

                    GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"Waiting for QC\") AND (Customer = \"" + report.CustomerName + "\")","");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "Waiting for QC - " + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }

                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"QC on Hold\") AND (Customer = \"" + report.CustomerName + "\")","");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "Waiting for Hold" + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }

                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"QC Failed\") AND (Customer = \"" + report.CustomerName + "\")","");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "QC Failed" + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }

                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"Waiting for Output\") AND (Customer = \"" + report.CustomerName + "\")","");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "Waiting for Output" + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }

                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"Waiting for Validation\") AND (Customer = \"" + report.CustomerName + "\")", "");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "Waiting for Validation" + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }

                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"Waiting for PDF Conversion\") AND (Customer = \"" + report.CustomerName + "\")","");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "Waiting for PDF Conversion" + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }

                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"Validation Completed\") AND (Customer = \"" + report.CustomerName + "\")","");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "Validation Completed" + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }

                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("(StatusFlag = \"Validation Approved\") AND (Customer = \"" + report.CustomerName + "\")","");
                    if (resultBatches.RecordsCount > 0)
                    {
                        result.BooleanReturnValue = true;
                        body += BatchByStatusReportHelper(report, resultBatches, "Validation Approved" + resultBatches.RecordsCount.ToString() + " Batch(es) found");
                    }                   
                }
                body += "<br/>";
                body += "</BODY></HTML>";

                result.StringReturnValue = body;
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving BatchByStatusReport Method ...");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="report"></param>
        /// <param name="resultBatches"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        static public string BatchByStatusReportHelper( GlobalVars.Report report, GlobalVars.ResultBatches resultBatches, string title)
        {
            string recordDate = "";
            string body = "";
            body = "<br/>";
            body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='1200'>";
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr align='center'>";
            body += "<td><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" + title + "<font></tb>";
            body += "</tr>";

            string jobType = "";
            foreach (GlobalVars.Batch batch in resultBatches.ReturnValue)
            {
                if (string.IsNullOrWhiteSpace(batch.JobType))
                {
                    jobType = "Missing Job Type";
                }
                if (jobType != batch.JobType)
                {
                    if (jobType != "")
                    {
                        body += "</tbody>";
                        body += "</TABLE>";
                    }
                    jobType = batch.JobType;
                    body += "<br/>";
                    body += "<TABLE border='1' cellpadding='0' cellspacing='0' width='1200'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    body += "<tr bgcolor='" + report.TableHeaderBackColor + "'>";
                    body += "<td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'>Job Type: " + jobType + "</font></td>";
                    body += "</tr>";
                    body += "</tbody>";
                    body += "</TABLE>";

                    body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='1200'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    body += "<tr bgcolor='" + report.TableHeaderBackColor + "'><b>";
                    body += "<td align='left' width='200'>Batch Name</td>";
                    body += "<td align='left' width='200'>Batch Alias</td>";
                    body += "<td align='left' width='200'>Last Change</td>";
                    body += "<td align='left' width='600'>Comments</td>";
                    body += "</b></tr>";
                }

                // Get the most recent transaction in the Tracking database table ...
                GlobalVars.ResultBatchTracking resultBatchTracking = new GlobalVars.ResultBatchTracking();
                resultBatchTracking = SQLFunctionsBatches.GetBatchTrackingInformation("(BatchNumber=\"" + batch.BatchNumber + "\" OR BatchNumber =\"" + batch.BatchAlias + "\")", "Date descending");
                if (resultBatchTracking.RecordsCount > 0)
                {
                    recordDate = Convert.ToString(resultBatchTracking.ReturnValue[0].Date);                    
                }

                body += "<tr>";
                body += "<td align='left' width='200'>" + batch.BatchNumber + "</td>";
                body += "<td align='left' width='200'>" + batch.BatchAlias + "</td>";
                body += "<td align='left' width='200'>" + recordDate + "</td>";
                body += "<td align='left' width='600'>" + batch.Comments + "</td>";
                body += "</tr>";

            }
            body += "</tbody>";
            body += "</TABLE>";
            return body;
        }


        /// <summary>
        /// This Methot returns a Work Order Table for an especific Job Type
        /// The workOrderSummary Object contains a summary for a  given Job Type withing the Work Order
        /// </summary>
        /// <param name="report"></param>
        /// <param name="woJobType"></param>
        /// <returns></returns>
        static public string WorkOrderReportHelper(GlobalVars.Report report, GlobalVars.WorkOrderJobType woJobType)
        {
            
            Boolean showNumberOfDocuments = false;
            Boolean showNumberOfScannedImages = false;
            Boolean showNumberOfKeystrokes = false;
            Boolean showNumberOfBlankImages = false;
            Boolean showPrepTime = false;
            Boolean showPageSizeCategories = false;
            string body = "";
            DateTime exportedDate;

            // Find out which one of the report columns needs to be presented for this Job Type in the Work Order Report
            foreach (GlobalVars.ReportParameter parameter in report.Parameters)
            {
                switch (parameter.ParameterName)
                {
                    case "ShowNumberOfDocuments":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfDocuments = false;
                        else
                            showNumberOfDocuments = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowNumberOfScannedImages":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfScannedImages = false;
                        else
                            showNumberOfScannedImages = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowNumberOfKeystrokes":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfKeystrokes = false;
                        else
                            showNumberOfKeystrokes = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowNumberBlankImages":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfBlankImages = false;
                        else
                            showNumberOfBlankImages = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowPrepTime":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showPrepTime = false;
                        else
                            showPrepTime = Convert.ToBoolean(parameter.Value);
                        break;

                    case "ShowPageSizeCategories":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showPageSizeCategories = false;
                        else
                            showPageSizeCategories = Convert.ToBoolean(parameter.Value);
                        break;
                }
            }
            exportedDate = woJobType.batches[0].ExportedDate;
            //exportedDate.ToString("D")

            // Get Job Page Sizes
            GlobalVars.ResultJobsExtended resultJobs = new GlobalVars.ResultJobsExtended();
            resultJobs =   SQLFunctionsJobs.GetJobByName(woJobType.JobName);
            GlobalVars.ResultJobPageSizes resultJobPageSizes = new GlobalVars.ResultJobPageSizes();
            resultJobPageSizes = SQLFunctionsJobs.GetPageSizesByJobID(resultJobs.ReturnValue[0].JobID);

            // Creating the header of the Job Type Sub-table
            body += "<TABLE border='1' cellpadding='0' cellspacing='0' width='100%'>"; //800
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableHeaderBackColor + "'>";
            body += "<td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'>" + woJobType.JobName +  "</font></td>";
            body += "</tr>";
            body += "</tbody>";
            body += "</TABLE>";

            body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='100%'>"; //800
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableColumnNamesBackColor + "'><b>";
            body += "<td align='left' width='200'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Box Number</td>";

            if (showNumberOfDocuments)
                body += "<td align='left' width='80'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Docs</td>";
            if (showNumberOfScannedImages)
                body += "<td align='left' width='80'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Scanned Images</td>";
            if (showNumberOfBlankImages)
                body += "<td align='left' width='80'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Blanck Images</td>";
            if (showNumberOfKeystrokes)
                body += "<td align='left' width='80'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Indexing</td>";
            if (showPrepTime)
                body += "<td align='left' width='80'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Prep Time</td>";

            if (resultJobPageSizes.RecordsCount != 0 && showPageSizeCategories)
            {
                body += "<td>";
                body += "   <TABLE border='0' cellpadding='0' cellspacing='0' width='100%'>";
                body += "       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                body += "           <tr>";
                body += "               <td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'>" + "Large Format Image Quantities" + "</font></td>";
                body += "           </tr>";
                body += "           <tr>";
                body += "               <td>";
                body += "                   <TABLE border='0' cellpadding='0' cellspacing='0' width='100%'>";
                body += "                       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                body += "                           <tr>";
                foreach (GlobalVars.JobPageSize jobPageSie in resultJobPageSizes.ReturnValue)
                {
                    if (jobPageSie.CategoryName == "Unknown")
                    {
                        body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + "ND" + "</td>";
                    }
                    else
                    {
                        body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + jobPageSie.CategoryName + "</td>";
                    }
                }
                body += "                           </tr>";
                body += "                       </tbody>";
                body += "                   </TABLE>";
                body += "               </td>";
                body += "           </tr>";
                body += "       </tbody>";
                body += "   </TABLE>";
                body += "</td>";
            }
            

            //body += "<td>";
            //body += "   <TABLE border='0' cellpadding='2' cellspacing='0' width='100%'>";
            //body += "       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            //body += "           <tr>";
            //body += "               <td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'>" + "Large Format Image Quantities" + "</font></td>";
            //body += "           </tr>";
            //body += "           <tr>";
            //body += "               <td>";
            //body += "                   <TABLE border='0' cellpadding='0' cellspacing='0' width='100%'>";
            //body += "                       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            //body += "                           <tr>";
            //body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Grp1</td>";
            //body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Grp2</td>";
            //body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Grp3</td>";
            //body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Grp4</td>";
            //body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Grp5</td>";
            //body += "                               <td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Grp6</td>";
            //body += "                           </tr>";
            //body += "                       </tbody>";
            //body += "                   </TABLE>";
            //body += "               </td>";
            //body += "           </tr>";
            //body += "       </tbody>";
            //body += "   </TABLE>";
            //body += "</td>";

            body += "</b></tr>";
            int unknownCount = 0;
            foreach (GlobalVars.Batch batch in woJobType.batches)
            {
                // Build a rown entry in the JobType Sub-Table
                body += "<tr>";
                body += "<td align='left' width='200'>" + batch.BatchNumber + "</td>";
                if (showNumberOfDocuments)
                {
                    body += "<td align='left' width='80'>" + batch.NumberOfDocuments + "</td>";
                }
                if (showNumberOfScannedImages)
                {
                    body += "<td align='left' width='80'>" + batch.NumberOfScannedPages + "</td>";
                }
                if (showNumberOfBlankImages)
                {
                    body += "<td align='left' width='80'>" + "</td>";
                }
                if (showNumberOfKeystrokes)
                {
                    body += "<td align='left' width='80'>" + batch.KeysStrokes + "</td>";
                }
                if (showPrepTime)
                {
                    body += "<td align='left' width='80'>" + batch.PrepTime.ToString("0.##") + "</td>";
                }


                if (resultJobPageSizes.RecordsCount != 0 && showPageSizeCategories)
                {
                    // Get Batch Page Size Categoires counts
                    GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("BatchNUmber = \"" + batch.BatchNumber + "\"", "");

                    GlobalVars.Batch batchAux = new GlobalVars.Batch();
                    batchAux = resultBatches.ReturnValue[0];

                    List<GlobalVars.PageSizeInfo> pageSizesInfo = new List<GlobalVars.PageSizeInfo>();
                    pageSizesInfo = JsonConvert.DeserializeObject<List<GlobalVars.PageSizeInfo>>(batchAux.PageSizesCount);

                    logger.Trace("  Page Size information for Batch Number:" + batch.BatchNumber);
                    logger.Trace("      " + batchAux.PageSizesCount);

                    unknownCount = 0;
                    if (!string.IsNullOrEmpty(batchAux.PageSizesCount))
                    {
                        body += "<td>";
                        body += "   <TABLE border='0' cellpadding='0' cellspacing='0' width='100%'>";
                        body += "       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                        body += "           <tr>";
                        //body += "               <td>";
                        //body += "                   <TABLE border='1' cellpadding='1' cellspacing='1' width='100%'>";
                        //body += "                       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                        //body += "                           <tr>";
                        foreach (GlobalVars.JobPageSize jobPageSize in resultJobPageSizes.ReturnValue)
                        {                            
                            foreach (GlobalVars.PageSizeInfo pageSizeInfo in pageSizesInfo)
                            {
                                if (pageSizeInfo.Category == jobPageSize.CategoryName)
                                {
                                    jobPageSize.TotalCounnt = jobPageSize.TotalCounnt + pageSizeInfo.ImageCount;
                                    body += "                               <td align='left' width='50'>" + pageSizeInfo.ImageCount.ToString() + "</td>";
                                }
                            }
                        }
                        //body += "                           </tr>";
                        //body += "                       </tbody>";
                        //body += "                   </TABLE>";
                        //body += "               </td>";
                        body += "           </tr>";
                        body += "       </tbody>";
                        body += "   </TABLE>";
                        body += "</td>";
                    }


                }

                //body += "<td>";
                //body += "   <TABLE border='0' cellpadding='0' cellspacing='0' width='100%'>";
                //body += "       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                //body += "           <tr>";
                //body += "               <td>";
                //body += "                   <TABLE border='0' cellpadding='0' cellspacing='0' width='100%'>";
                //body += "                       <tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                //body += "                           <tr>";
                //body += "                               <td align='left' width='50'>" + "1" + "</td>";
                //body += "                               <td align='left' width='50'>" + "1" + "</td>";
                //body += "                               <td align='left' width='50'>" + "10" + "</td>";
                //body += "                               <td align='left' width='50'>" + "89" + "</td>";
                //body += "                               <td align='left' width='50'>" + "98" + "</td>";
                //body += "                               <td align='left' width='50'>" + "56" + "</td>";
                //body += "                           </tr>";
                //body += "                       </tbody>";
                //body += "                   </TABLE>";
                //body += "               </td>";
                //body += "           </tr>";
                //body += "       </tbody>";
                //body += "   </TABLE>";
                //body += "</td>";




                body += "</tr>";                
            }
            
            // Closing the JobType Sub-Table
            body += "</tbody>";
            body += "</TABLE>";

            body += "<br/>";

            // Creating the JobType Summary section
            body += "<TABLE border='1' cellpadding='0' cellspacing='0' width='100%'>";
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableHeaderBackColor + "'>";
            body += "<td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'> TOTALS FOR " + woJobType.JobName.ToUpper() + "</font></td>";
            body += "</tr>";
            body += "</tbody>";
            body += "</TABLE>";

            body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='100%'>";
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableColumnNamesBackColor + "'><b>";

            body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Boxes</td>";
            if (showNumberOfDocuments)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Docs</td>";
            if (showNumberOfScannedImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Scanned Images</td>";
            if (showNumberOfBlankImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Blanc Images</td>";
            if (showNumberOfKeystrokes)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Indexing</td>";
            if (showPrepTime)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Prep Time</td>";

            if (resultJobPageSizes.RecordsCount != 0 && showPageSizeCategories)
            {
                foreach (GlobalVars.JobPageSize jobPageSize in resultJobPageSizes.ReturnValue)
                {
                    if (jobPageSize.CategoryName == "Unknown")
                    {
                        body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + "ND" + "</td>";
                    }
                    else
                    {
                        body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + jobPageSize.CategoryName + "</td>";
                    }
                }
            }
            body += "</b></tr>";

            body += "<tr>";
            body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberBoxes.ToString("0,0") + "</td>";
            if (showNumberOfDocuments)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberDocs.ToString("0,0") + "</td>";
            if (showNumberOfScannedImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberScannedImages.ToString("0,0") + "</td>";
            if (showNumberOfBlankImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberScannedImages.ToString("0,0") + "</td>";
            if (showNumberOfKeystrokes)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberKeystrokes.ToString("0,0") + "</td>";
            if (showPrepTime)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.PrepTime.ToString("0.##") + "</td>";
            if (resultJobPageSizes.RecordsCount != 0 && showPageSizeCategories)
            {
                foreach (GlobalVars.JobPageSize jobPageSize in resultJobPageSizes.ReturnValue)
                {
                        body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + jobPageSize.TotalCounnt.ToString() + "</td>";
                }
            }
            body += "</tr>";

            body += "</tbody>";
            body += "</TABLE>";

            body += "<br/>";
            body += "<br/>";

            if (resultJobPageSizes.RecordsCount != 0 && showPageSizeCategories)
            {
                 // Creating Legend Section JobType Summary section
                body += "<TABLE border='1' cellpadding='0' cellspacing='0' width='300'>";
                body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                body += "<tr bgcolor='" + report.TableHeaderBackColor + "'>";
                body += "<td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'> LEGEND " + "</font></td>";
                body += "</tr>";
                body += "</tbody>";
                body += "</TABLE>";

                body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='300'>";
                body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                
                foreach (GlobalVars.JobPageSize jobPageSize in resultJobPageSizes.ReturnValue)
                {
                    body += "<tr>";
                    if (jobPageSize.CategoryName == "Unknown")
                    {
                        body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + "ND" + "</td>";
                        body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + "Not Defined" + "</td>";
                    }
                    else
                    {
                        body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + jobPageSize.CategoryName + "</td>";
                        body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + jobPageSize.High.ToString() + " x " + jobPageSize.Width.ToString() + "</td>";
                    }                    
                    body += "</tr>";
                }

                body += "</tbody>";
                body += "</TABLE>";

                body += "<br/>";
            }

            return body;
        }

        /// <summary>
        /// This Methot returns a Work Order Table for an especific Job Type
        /// The workOrderSummary Object contains a summary for a  given Job Type withing the Work Order
        /// </summary>
        /// <param name="report"></param>
        /// <param name="woJobType"></param>
        /// <returns></returns>
        static public string WorkOrderReportHelperOriginal(GlobalVars.Report report, GlobalVars.WorkOrderJobType woJobType)
        {

            Boolean showNumberOfDocuments = false;
            Boolean showNumberOfScannedImages = false;
            Boolean showNumberOfKeystrokes = false;
            Boolean showNumberOfBlankImages = false;
            Boolean showPrepTime = false;
            string body = "";
            DateTime exportedDate;

            // Find out which one of the report columns needs to be presented for this Job Type in the Work Order Report
            foreach (GlobalVars.ReportParameter parameter in report.Parameters)
            {
                switch (parameter.ParameterName)
                {
                    case "ShowNumberOfDocuments":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfDocuments = false;
                        else
                            showNumberOfDocuments = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowNumberOfScannedImages":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfScannedImages = false;
                        else
                            showNumberOfScannedImages = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowNumberOfKeystrokes":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfKeystrokes = false;
                        else
                            showNumberOfKeystrokes = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowNumberBlankImages":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showNumberOfBlankImages = false;
                        else
                            showNumberOfBlankImages = Convert.ToBoolean(parameter.Value);
                        break;
                    case "ShowPrepTime":
                        if (string.IsNullOrEmpty(parameter.Value))
                            showPrepTime = false;
                        else
                            showPrepTime = Convert.ToBoolean(parameter.Value);
                        break;
                }
            }
            exportedDate = woJobType.batches[0].ExportedDate;
            //exportedDate.ToString("D")

            // Creating the header of the Job Type Sub-table
            body += "<TABLE border='1' cellpadding='0' cellspacing='0' width='800'>";
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableHeaderBackColor + "'>";
            body += "<td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'>" + woJobType.JobName + "</font></td>";
            body += "</tr>";
            body += "</tbody>";
            body += "</TABLE>";

            body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='800'>";
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableColumnNamesBackColor + "'><b>";
            body += "<td align='left' width='300'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Box Number</td>";

            if (showNumberOfDocuments)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Docs</td>";
            if (showNumberOfScannedImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Scanned Images</td>";
            if (showNumberOfBlankImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Blanck Images</td>";
            if (showNumberOfKeystrokes)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Indexing</td>";
            if (showPrepTime)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Prep Time</td>";
            body += "</b></tr>";

            foreach (GlobalVars.Batch batch in woJobType.batches)
            {
                // Build a rown entry in the JobType Sub-Table
                body += "<tr>";
                body += "<td align='left' width='300'>" + batch.BatchNumber + "</td>";
                if (showNumberOfDocuments)
                {
                    body += "<td align='left' width='100'>" + batch.NumberOfDocuments + "</td>";
                }
                if (showNumberOfScannedImages)
                {
                    body += "<td align='left' width='100'>" + batch.NumberOfScannedPages + "</td>";
                }
                if (showNumberOfBlankImages)
                {
                    body += "<td align='left' width='100'>" + "</td>";
                }
                if (showNumberOfKeystrokes)
                {
                    body += "<td align='left' width='100'>" + batch.KeysStrokes + "</td>";
                }
                if (showPrepTime)
                {
                    body += "<td align='left' width='100'>" + batch.PrepTime + "</td>";
                }
                body += "</tr>";
            }

            // Closing the JobType Sub-Table
            body += "</tbody>";
            body += "</TABLE>";

            //body += "<br/>";

            // Creating the JobType Summary section
            body += "<TABLE border='1' cellpadding='0' cellspacing='0' width='800'>";
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableHeaderBackColor + "'>";
            body += "<td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'> SUMMARY REPORT FOR " + woJobType.JobName.ToUpper() + "</font></td>";
            body += "</tr>";
            body += "</tbody>";
            body += "</TABLE>";

            body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='800'>";
            body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
            body += "<tr bgcolor='" + report.TableColumnNamesBackColor + "'><b>";

            body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Boxes</td>";
            if (showNumberOfDocuments)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Docs</td>";
            if (showNumberOfScannedImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Scanned Images</td>";
            if (showNumberOfBlankImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Blanc Images</td>";
            if (showNumberOfKeystrokes)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Indexing</td>";
            if (showPrepTime)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Prep Time</td>";
            body += "</b></tr>";

            body += "<tr>";
            body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberBoxes.ToString("0,0") + "</td>";
            if (showNumberOfDocuments)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberDocs.ToString("0,0") + "</td>";
            if (showNumberOfScannedImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberScannedImages.ToString("0,0") + "</td>";
            if (showNumberOfBlankImages)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberScannedImages.ToString("0,0") + "</td>";
            if (showNumberOfKeystrokes)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.NumberKeystrokes.ToString("0,0") + "</td>";
            if (showPrepTime)
                body += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woJobType.PrepTime.ToString("0,0") + "</td>";
            body += "</tr>";

            body += "</tbody>";
            body += "</TABLE>";

            body += "<br/>";
            body += "<br/>";

            return body;
        }

        //Public Function GetBatchLastChangeDate(ByVal sBatchName As String, ByVal sBatchAlias As String) As String
        //Dim sStatement As String
        //Dim sResult As sTable
        //Dim lNumRecords As Integer
        //Dim lChildren As Integer
        //sResult = Nothing
        //GetBatchLastChangeDate = ""
        //'Build SQL Statament
        //sStatement = "SELECT  MAX([Date]) " & _
        //             "FROM BatchTracking " & _
        //             "WHERE  BatchNumber ='" & sBatchName & "' OR BatchNumber ='" & sBatchAlias & "'"


        /// <summary>
        /// This is a System Report (in the Databse the entry for system report has by convetion, CustomerID = 0)
        /// Build the body of Overall Batch Status Report
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns>The body of the email in HTMl format</returns>
        static public GlobalVars.ResultGeneric OverallBatchStatusReport(int reportID)
        {
            string body = "";
            
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into UpdateReport Method ...");

                // Get Report Information to be used in the report
                GlobalVars.ResultReports resultReport = new GlobalVars.ResultReports();
                resultReport = SQLFunctionsReports.GetReportByID(reportID);
                if (resultReport.RecordsCount >= 1)
                {
                    // Report was found
                    GlobalVars.Report report = resultReport.ReturnValue[0];
                    //Header
                    body = "<!DOCTYPE HTML PUBLIC \"-//W3C/DTD HTML 4.0 Transitional//EN\">";
                    body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                    body += "</HEAD>";
                    body += "<BODY>";
                    body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='500'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    if (resultReport.ReturnValue[0].TitleContent1.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag1)
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" + 
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" + 
                                    report.TitleContent1 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent2.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag2)
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" + 
                                    "<b>" + report.TitleContent2 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" + 
                                    report.TitleContent2 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent3.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag3)
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" + 
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" + 
                                    report.TitleContent3 + "</font></td>";
                        body += "</tr>";
                    }
                    body += "</tbody>";
                    body += "</TABLE>";

                    //Body
                    body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='500'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    if (report.TableHeaderFontBoldFlag)
                    {
                        body += "<tr><td bgcolor='" + report.TableHeaderBackColor + "' align='center'><font color='" + report.TableHeaderFontColor + "' size='" +
                                report.TableHeaderFontSize + "'>" + "<b>" + "BATCH REPORT BY STATUS" + "</b>"  + "</td></tr>";
                    }
                    else
                    {
                        body += "<tr><td bgcolor='" + report.TableHeaderBackColor + "' align='center'><font color='" + report.TableHeaderFontColor + "' size='" + 
                                report.TableHeaderFontSize + "'>BATCH REPORT BY STATUS</td></tr>";
                    }
                    
                    body += "</tbody>";
                    body += "</TABLE>";

                    body += "<TABLE border='1' cellpadding='2' cellspacing='1' width='500'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    body += "<tr>";
                    if (report.TableColumnNamesFontBoldFlag)
                    {
                        body += "<td bgcolor='" + report.TableColumnNamesBackColor + "' width='300'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'><b>Batch Status</b></td>";
                        body += "<td bgcolor='" + report.TableColumnNamesBackColor + "' width='300'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'><b>Number of Batches<b></td>";
                    }
                    else
                    {
                        body += "<td bgcolor='" + report.TableColumnNamesBackColor + "' width='300'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Batch Status</td>";
                        body += "<td bgcolor='" + report.TableColumnNamesBackColor + "' width='300'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Number of Batches</td>";
                    }
                   
                    body += "</tr>";

                    // Table Content
                    GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
                    body += "<tr>";
                    body += "<td width='300'>Approved</td>";
                    // Alternative Option ... resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Approved\"");
                    //resultBatches = SQLFunctionsBatches.GetBatchesInformation("(ScannedDate >= \"01/01/2000 00:00 AM\") AND (ScannedDate <= \"01/01/2019 00:00 AM\")");
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Approved\"","");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Exported</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Exported\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>QC on Hold</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"QC on Hold\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>QC Failed</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"QC Failed\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Ready to Scan</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Ready to Scan\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Rejected</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Rejected\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Recall</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Recall\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Scan on Hold</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Scan on Hold\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting for Assistance</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting for Assistance\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting for Approval</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting for Approval\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting for QC</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting for QC\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting for Output</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting for Output\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting to be Cleaned</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting to be Cleaned\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting for Validation</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting for Validation\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Validation Approved</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Validation Approved\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Validation Completed</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Validation Completed\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting for PDF Conversion</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting for PDF Conversion\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "<tr>";
                    body += "<td width='300'>Waiting for File Conversion Station</td>";
                    resultBatches = SQLFunctionsBatches.GetBatchesInformation("StatusFlag = \"Waiting for File Conversion Station\"", "");
                    body += "<td width='300'>" + resultBatches.RecordsCount.ToString() + "</td>";
                    body += "</tr>";

                    body += "</tbody>";
                    body += "</TABLE>";

                    // '-----------------------------------------------------------------------
                    //' Additional Information we could generate
                    //'-----------------------------------------------------------------------
                    //'   1- Number of Boxes scanned that day
                    //'   2- Number of Boxes QCd that day
                    //'   3- Average Scanning per box
                    //'-----------------------------------------------------------------------
                    DateTime dTodayDate = DateTime.Now;
                    DateTime dYesterday = dTodayDate.AddDays(-1);
                    GlobalVars.ResultBatchSummary resultBatchSummary = new GlobalVars.ResultBatchSummary();
                    //resultBatchSummary = SQLFunctionsBatches.GetBatchSummary("(ScannedDate >= \"" +  "01/01/2000 00:00 AM\") AND (ScannedDate <= \"" +  dYesterday.ToString("d") + " 11:59 PM\")");
                    resultBatchSummary = SQLFunctionsBatches.GetBatchSummary("(ScannedDate >= \"" + dYesterday.ToString("d") + " 00:00 AM\") AND (ScannedDate <= \"" + dYesterday.ToString("d") + " 11:59 PM\")");
                    if (resultBatchSummary.RecordsCount > 0)
                    {                        
                        // Header
                        body += "<br/><br/>";
                        body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='500'>";
                        body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                        if (report.TableHeaderFontBoldFlag)
                        {
                            body += "<tr><td bgcolor='" + report.TableHeaderBackColor + "' align='center'><font color='" + report.TableHeaderFontColor + "' size='" +
                                    report.TableHeaderFontSize + "'>" + "<b>" + "OPERATION STATISTICS : " + dYesterday.ToString("d") + "  00:00 AM - " + dYesterday.ToString("d") + " 11:59 PM" + "</b>" + "</td></tr>";
                        }
                        else
                        {
                            body += "<tr><td bgcolor='" + report.TableHeaderBackColor + "' align='center'><font color='" + report.TableHeaderFontColor + "' size='" +
                                    report.TableHeaderFontSize + "'>OPERATION STATISTICS : " + dYesterday.ToString("d") + " 00:00 AM - " + dYesterday.ToString("d") + " 11:59 PM" + "</td></tr>";
                        }
                        body += "</tbody>";
                        body += "</TABLE>";
                    
                        body += "<TABLE border='1' cellpadding='2' cellspacing='1' width='500'>";
                        body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                        body += "<tr>";
                        body += "<td width='300'>Boxes Scanned</td>";
                        if (!string.IsNullOrEmpty(resultBatchSummary.ReturnValue[0].Count.ToString()))
                            body += "<td width='300'>" + String.Format("{0:0,0}",resultBatchSummary.ReturnValue[0].Count) + "</td>";
                        else
                            body += "<td width='300'>" + " " + "</td>";
                        body += "</tr>";
                        body += "<tr>";
                        body += "<td width='300'>Images Scanned</td>";
                        if (!string.IsNullOrEmpty(resultBatchSummary.ReturnValue[0].TotalNumOfScannedPages.ToString()))
                            body += "<td width='300'>" + String.Format("{0:0,0}", resultBatchSummary.ReturnValue[0].TotalNumOfScannedPages) + "</td>";
                        else
                            body += "<td width='300'>" + " " + "</td>";
                        body += "</tr>";
                        body += "<tr>";
                        body += "<td width='300'>Avg Images per Box</td>";
                        if (!string.IsNullOrEmpty(resultBatchSummary.ReturnValue[0].AvgNumOfScannedPages.ToString()))
                            body += "<td width='300'>" + String.Format("{0:0,0.00}", resultBatchSummary.ReturnValue[0].AvgNumOfScannedPages)  + "</td>";
                        else
                            body += "<td width='300'>" + " " + "</td>";
                        body += "</tr>";
                        body += "<tr>";
                        body += "<td width='300'>Avg Scanning Time per Box (min)</td>";
                        if (!string.IsNullOrEmpty(resultBatchSummary.ReturnValue[0].AvgScanningTime.ToString()))
                            body += "<td width='300'>" + String.Format("{0:0,0.00}", resultBatchSummary.ReturnValue[0].AvgScanningTime) + "</td>";
                        else
                            body += "<td width='300'>" + " " + "</td>";
                        body += "</tr>";

                        body += "</tbody>";
                        body += "</TABLE>";
                    }

                    body += "</FONT></DIV></BODY></HTML>";

                    result.StringReturnValue = body;
                }

            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving OverallBatchStatusReport Method ...");
            return result;
        }

        /// <summary>
        /// UNDER CONSTRUCTION .........................................
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric QCDailyOperationReport(int reportID)
        {
            string currentOperator = "";
            string body = "";
            int QCBatchesGrantTotal = 0;
            int QCDocumnetsGrantTotal = 0;
            double QCTimeGrantTotal = 0;
            int recordsCount = 0;

            int OperatorQCBatchesCount = 0;
            int OperatorQCDocumnetsCount = 0;
            double OperatorQCTimeTotal = 0;
            GlobalVars.ResultBatchTrackingExtended resultBatchTracking = new GlobalVars.ResultBatchTrackingExtended();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into QCDailyOperationReport Method ...");

                // Get Report Information to be used in the report
                GlobalVars.ResultReports resultReport = new GlobalVars.ResultReports();
                resultReport = SQLFunctionsReports.GetReportByID(reportID);
                if (resultReport.RecordsCount >= 1)
                {
                    // Report was found
                    GlobalVars.Report report = resultReport.ReturnValue[0];
                    //Header
                    body = "<!DOCTYPE HTML PUBLIC \"-//W3C/DTD HTML 4.0 Transitional//EN\">";
                    body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                    body += "</HEAD>";
                    body += "<BODY>";
                    body += "<TABLE border='0' cellpadding='1' cellspacing='1' width='500'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    if (resultReport.ReturnValue[0].TitleContent1.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag1)
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                    report.TitleContent1 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent2.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag2)
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" +
                                    "<b>" + report.TitleContent2 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" +
                                    report.TitleContent2 + "</font></td>";
                        body += "</tr>";
                    }
                    if (report.TitleContent3.Length > 0)
                    {
                        body += "<tr>";
                        if (report.TitleFontBoldFlag3)
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                    "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                        else
                            body += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                    report.TitleContent3 + "</font></td>";
                        body += "</tr>";
                    }
                    body += "</tbody>";
                    body += "</TABLE>";

                    // Boby
                    body += "<TABLE border='1' cellpadding='1' cellspacing='1' width='1200'>";
                    body += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                    body += "<tr bgcolor='" + report.TableHeaderBackColor + "'><b>";
                    body += "<td align='left' width='200'>Operator</td>";
                    body += "<td align='left' width='200'>Batch Name</td>";
                    body += "<td align='left' width='200'>QC Time (Minutes)</td>";
                    body += "<td align='left' width='200'>SSS Docs</td>";
                    body += "<td align='left' width='600'>Type</td>";
                    body += "</b></tr>";

                    resultBatchTracking = SQLFunctionsBatches.GetInformationForDailyOperationReport(DateTime.Today, DateTime.Today.AddDays(1));

                    foreach (GlobalVars.BatchTrackingExtended batchEvent in resultBatchTracking.ReturnValue)
                    {
                        if (batchEvent.OperatorName.Length > 0 && resultBatchTracking.RecordsCount > 0)
                        {
                            body += "<tr>";
                            if (currentOperator != batchEvent.OperatorName)
                            {
                                if (recordsCount != 0)
                                {
                                    // Scenario : very first time
                                    body += "<tr>";
                                    body += "<td align='center' width='300'>Totals</td>";
                                    body += "<td width='300'>" + OperatorQCBatchesCount + "</td>";
                                    body += "<td align='center' width='300'>" + OperatorQCTimeTotal + "</td>";
                                    body += "<td align='center' width='300'>" + OperatorQCDocumnetsCount + "</td>";
                                    body += "<td align='center'width='300'>" + "</td>";
                                    body += "<tr>";

                                    body += "<tr>";
                                    body += "<td width='300'>       </td>";
                                    body += "<td width='300'>       </td>";
                                    body += "<td width='300'>       </td>";
                                    body += "<td width='300'>       </td>";
                                    body += "<td width='300'>       </td>";
                                    body += "</tr>";
                                }

                                recordsCount = recordsCount + 1;
                                OperatorQCBatchesCount = 0;
                                OperatorQCDocumnetsCount = 0;
                                OperatorQCTimeTotal = 0;
                                currentOperator = batchEvent.OperatorName;
                                body += "<td width='300'>" + batchEvent.OperatorName + "</td>";
                                body += "<td width='300'>" + batchEvent.BatchNumber + "</td>";
                                body += "<td align='center' width='300'>" + batchEvent.totalQcMinutes + "</td>";
                                body += "<td align='center' width='300'>" + batchEvent.totalDocuments + "</td>";
                                body += "<td align='center'width='300'>" + batchEvent.JobType + "</td>";
                            }
                            else
                            {
                                OperatorQCDocumnetsCount = OperatorQCDocumnetsCount + batchEvent.totalDocuments;
                                OperatorQCTimeTotal = OperatorQCTimeTotal + batchEvent.totalQcMinutes;
                                OperatorQCBatchesCount = OperatorQCBatchesCount + 1;
                                body += "<td width='300'>" +  "</td>";
                                body += "<td width='300'>" + batchEvent.BatchNumber + "</td>";
                                body += "<td align='center' width='300'>" + batchEvent.totalQcMinutes + "</td>";
                                body += "<td align='center' width='300'>" + batchEvent.totalDocuments + "</td>";
                                body += "<td align='center'width='300'>" + batchEvent.JobType + "</td>";
                            }
                            body += "</tr>";      

                            QCBatchesGrantTotal = QCBatchesGrantTotal + 1;
                            QCDocumnetsGrantTotal = QCDocumnetsGrantTotal + batchEvent.totalDocuments;
                            QCTimeGrantTotal = QCTimeGrantTotal + batchEvent.totalQcMinutes;
                        }
                    }

                    body += "<tr>";
                    body += "<td align='right' width='300'>GRANT TOTALS</td>";
                    body += "<td align='center' width='300'>" + QCBatchesGrantTotal + "</td>";
                    body += "<td align='center' width='300'>" + QCTimeGrantTotal + "</td>";
                    body += "<td align='center' width='300'>" + QCDocumnetsGrantTotal + "</td>";
                    body += "<td align='center' width='300'>      </td>";
                    body += "</tr>";

                    body += "</tbody>";
                    body += "</TABLE>";

                    result.StringReturnValue = body;
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving QCDailyOperationReport Method ...");
            return result;
        }


        /// <summary>
        /// Get List of Sytem Reports Templates
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultReportsTemplate GetReportsTemplate(string reportType)
        {
            List<GlobalVars.ReportTemplate> reportsTemplate = new List<GlobalVars.ReportTemplate>();
            GlobalVars.ResultReportsTemplate resultReportsTemplates = new GlobalVars.ResultReportsTemplate()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = reportsTemplate,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetReportsTemplate Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ReportsTemplate.Where(x => x.Type == reportType);
                    resultReportsTemplates.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ReportTemplate reportTemplate = new GlobalVars.ReportTemplate()
                            {
                                TemplateID = x.TemplateId,
                                Name = (x.Name ?? "").Trim(),
                                Description = (x.Description ?? "").Trim(),
                                Notes = (x.Notes ?? "").Trim(),
                                Type = (x.Type ?? "").Trim()
                            };
                            
                            var results1 = DB.ReportsTemplateParameters.Where(y => y.TemplateId == reportTemplate.TemplateID);
                            if (results1.Count() >= 1)
                            {
                                reportTemplate.Parameters = new List<GlobalVars.ReportTemplateParameter>();
                                foreach (var y in results1)
                                {
                                    GlobalVars.ReportTemplateParameter reportTemplateParameter = new GlobalVars.ReportTemplateParameter()
                                    {
                                        Name = (y.Name ?? "").Trim(),
                                        Description = (y.Description ?? "").Trim(),
                                        RequiredFlag =  Convert.ToBoolean(y.RequiredFlag),
                                        TemplateID = y.TemplateId,
                                        ParameterName = GetParameterName(y.ParameterId),
                                        ParameterID = y.ParameterId,
                                        DataType = (y.DataType ?? "").Trim()
                                    };
                                    reportTemplate.Parameters.Add(reportTemplateParameter);
                                }                               
                            }
                            reportsTemplate.Add(reportTemplate);
                        }
                    }
                }
                resultReportsTemplates.ReturnValue = reportsTemplate;
                resultReportsTemplates.Message = "GetReportsTemplate transaction completed successfully. Number of records found: " + resultReportsTemplates.RecordsCount;
                logger.Debug(resultReportsTemplates.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultReportsTemplates.ReturnCode = -2;
                resultReportsTemplates.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReportsTemplates.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetReportsTemplate Method ...");
            return resultReportsTemplates;
        }

        /// <summary>
        /// Get List of Reports for a given Customer Name
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultReports GetReportsByCustomerName(string customerName)
        {
            List<GlobalVars.Report> customerReports = new List<GlobalVars.Report>();
            GlobalVars.ResultReports resultCustomerReports = new GlobalVars.ResultReports()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = customerReports,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetReportsByCustomerName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    var results = from r in DB.Reports
                                  join t in DB.ReportsTemplate on r.TemplateId equals t.TemplateId
                                  join c in DB.Customers on r.CustomerId equals c.CustomerId
                                  where c.CustomerName == customerName
                                  select new { r, t.Name };

                    resultCustomerReports.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var record in results)
                        {
                            GlobalVars.Report report = new GlobalVars.Report()
                            {
                                ReportName = (record.Name ?? "").Trim(),
                                TemplateID = record.r.TemplateId,
                                CustomerID = record.r.CustomerId,
                                EmailRecipients = (record.r.EmailRecipients ?? "").Trim(),
                                EmailSubject = (record.r.EmailSubject ?? "").Trim(),
                                EnableFlag = Convert.ToBoolean(record.r.EnableFlag),
                                ReportID = record.r.ReportId,
                                TitleContent1 = (record.r.TitleContent1 ?? "").Trim(),
                                TitleContent2 = (record.r.TitleContent2 ?? "").Trim(),
                                TitleContent3 = (record.r.TitleContent3 ?? "").Trim(),
                                TitleFontBoldFlag1 = Convert.ToBoolean(record.r.TitleFontBoldFlag1),
                                TitleFontBoldFlag2 = Convert.ToBoolean(record.r.TitleFontBoldFlag2),
                                TitleFontBoldFlag3 = Convert.ToBoolean(record.r.TitleFontBoldFlag3),
                                TitleFontColor1 = (record.r.TitleFontColor1 ?? "").Trim(),
                                TitleFontColor2 = (record.r.TitleFontColor2 ?? "").Trim(),
                                TitleFontColor3 = (record.r.TitleFontColor3 ?? "").Trim(),
                                TitleFontSize1 = record.r.TitleFontSize1.Value,
                                TitleFontSize2 = record.r.TitleFontSize2.Value,
                                TitleFontSize3 = record.r.TitleFontSize3.Value,
                                TableColumnNamesBackColor =(record.r.TableColumnNamesBackColor ?? "").Trim(),
                                TableColumnNamesFontBoldFlag = Convert.ToBoolean(record.r.TableColumnNamesFontBoldFlag),
                                TableColumnNamesFontColor = (record.r.TableColumnNamesFontColor ?? "").Trim(),
                                TableColumnNamesFontSize = record.r.TableColumnNamesFontSize.Value,
                                TableHeaderBackColor = (record.r.TableHeaderBackColor ?? "").Trim(),
                                TableHeaderFontBoldFlag = Convert.ToBoolean(record.r.TableHeaderFontBoldFlag),
                                TableHeaderFontColor = (record.r.TableHeaderFontColor ?? "").Trim(),
                                TableHeaderFontSize = record.r.TableHeaderFontSize.Value,                                
                            };
                            if (!string.IsNullOrWhiteSpace(record.r.ScheduleTime))
                            {
                                report.Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(record.r.ScheduleTime);
                                report.ScheduleCronFormat = GeneralTools.scheduleStringBuilder(record.r.ScheduleTime);
                            }
                            else
                            {
                                GlobalVars.ScheduleTime schedule = new GlobalVars.ScheduleTime();
                                report.Schedule = schedule;
                                report.ScheduleCronFormat = "";
                            }

                            var results1 = DB.ReportParameters.Where(y => y.TemplateId == report.TemplateID);
                            if (results1.Count() >= 1)
                            {
                                report.Parameters = new List<GlobalVars.ReportParameter>();
                                foreach (var y in results1)
                                {
                                    GlobalVars.ReportParameter reportParameter = new GlobalVars.ReportParameter()
                                    {
                                        ReportID = y.ReportId,
                                        TemplateID = y.TemplateId,
                                        ParameterID = y.ParameterId,
                                        ParameterName = GetParameterName(y.ParameterId),
                                        Value = (y.Value ?? "").Trim()
                                    };
                                    report.Parameters.Add(reportParameter);
                                }
                            }
                            customerReports.Add(report);
                        }
                    }
                }
                resultCustomerReports.ReturnValue = customerReports;
                resultCustomerReports.Message = "GetReportsByCustomerName transaction completed successfully. Number of records found: " + resultCustomerReports.RecordsCount;
                logger.Debug(resultCustomerReports.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultCustomerReports.ReturnCode = -2;
                resultCustomerReports.Message = e.Message;
                var baseException = e.GetBaseException();
                resultCustomerReports.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetReportsByCustomerName Method ...");
            return resultCustomerReports;
        }

        /// <summary>
        /// Get Report Template Information By ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultReportsTemplate GetReportTemplateByID(int templateID)
        {
            List<GlobalVars.ReportTemplate> reportsTemplate = new List<GlobalVars.ReportTemplate>();
            GlobalVars.ResultReportsTemplate resultReportsTemplates = new GlobalVars.ResultReportsTemplate()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = reportsTemplate,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetReportTemplateByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ReportsTemplate.Where(x => x.TemplateId == templateID);
                    resultReportsTemplates.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var x in results)
                        {
                            GlobalVars.ReportTemplate reportTemplate = new GlobalVars.ReportTemplate()
                            {
                                TemplateID = x.TemplateId,
                                Name = (x.Name ?? "").Trim(),
                                Description = (x.Description ?? "").Trim(),
                                Notes = (x.Notes ?? "").Trim(),
                                Type = (x.Type ?? "").Trim()
                            };

                            var results1 = DB.ReportsTemplateParameters.Where(y => y.TemplateId == reportTemplate.TemplateID);
                            if (results1.Count() >= 1)
                            {
                                reportTemplate.Parameters = new List<GlobalVars.ReportTemplateParameter>();
                                foreach (var y in results1)
                                {
                                    GlobalVars.ReportTemplateParameter reportTemplateParameter = new GlobalVars.ReportTemplateParameter()
                                    {
                                        Name = (y.Name ?? "").Trim(),
                                        Description = (y.Description ?? "").Trim(),
                                        RequiredFlag = Convert.ToBoolean(y.RequiredFlag),
                                        TemplateID = y.TemplateId,
                                        ParameterID = y.ParameterId,
                                        ParameterName = GetParameterName(y.ParameterId),
                                        DataType = (y.DataType ?? "").Trim()
                                    };
                                    reportTemplate.Parameters.Add(reportTemplateParameter);
                                }
                            }
                            reportsTemplate.Add(reportTemplate);
                        }
                    }
                }
                resultReportsTemplates.ReturnValue = reportsTemplate;
                resultReportsTemplates.Message = "GetReportTemplateByID transaction completed successfully. Number of records found: " + resultReportsTemplates.RecordsCount;
                logger.Debug(resultReportsTemplates.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultReportsTemplates.ReturnCode = -2;
                resultReportsTemplates.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReportsTemplates.Exception = baseException.ToString();
                //return resultHosts;
            }
            logger.Trace("Leaving GetReportTemplateByID Method ...");
            return resultReportsTemplates;
        }
        
        /// <summary>
        /// Get Reports Information
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultReports GetReports()
        {
            List<GlobalVars.Report> reports = new List<GlobalVars.Report>();
            GlobalVars.ResultReports resultReports = new GlobalVars.ResultReports()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = reports,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetReports Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    var results = from r in DB.Reports
                                  join t in DB.ReportsTemplate on r.TemplateId equals t.TemplateId
                                  select new { r, t.Name };

                    resultReports.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var record in results)
                        {
                            GlobalVars.Report report = new GlobalVars.Report()
                            {
                                ReportName = (record.Name ?? "").Trim(),
                                TemplateID = record.r.TemplateId,
                                CustomerID = record.r.CustomerId,
                                EmailRecipients = (record.r.EmailRecipients ?? "").Trim(),
                                EmailSubject = (record.r.EmailSubject ?? "").Trim(),
                                EnableFlag = Convert.ToBoolean(record.r.EnableFlag),
                                ReportID = record.r.ReportId,
                                TitleContent1 = (record.r.TitleContent1 ?? "").Trim(),
                                TitleContent2 = (record.r.TitleContent2 ?? "").Trim(),
                                TitleContent3 = (record.r.TitleContent3 ?? "").Trim(),
                                TitleFontBoldFlag1 = Convert.ToBoolean(record.r.TitleFontBoldFlag1),
                                TitleFontBoldFlag2 = Convert.ToBoolean(record.r.TitleFontBoldFlag2),
                                TitleFontBoldFlag3 = Convert.ToBoolean(record.r.TitleFontBoldFlag3),
                                TitleFontColor1 = (record.r.TitleFontColor1 ?? "").Trim(),
                                TitleFontColor2 = (record.r.TitleFontColor2 ?? "").Trim(),
                                TitleFontColor3 = (record.r.TitleFontColor3 ?? "").Trim(),
                                TitleFontSize1 = record.r.TitleFontSize1.Value,
                                TitleFontSize2 = record.r.TitleFontSize2.Value,
                                TitleFontSize3 = record.r.TitleFontSize3.Value,
                                TableColumnNamesBackColor = (record.r.TableColumnNamesBackColor ?? "").Trim(),
                                TableColumnNamesFontBoldFlag = Convert.ToBoolean(record.r.TableColumnNamesFontBoldFlag),
                                TableColumnNamesFontColor = (record.r.TableColumnNamesFontColor ?? "").Trim(),
                                TableColumnNamesFontSize = record.r.TableColumnNamesFontSize.Value,
                                TableHeaderBackColor = (record.r.TableHeaderBackColor ?? "").Trim(),
                                TableHeaderFontBoldFlag = Convert.ToBoolean(record.r.TableHeaderFontBoldFlag),
                                TableHeaderFontColor = (record.r.TableHeaderFontColor ?? "").Trim(),
                                TableHeaderFontSize = record.r.TableHeaderFontSize.Value
                            };

                            // Get Customer Name
                            if (record.r.CustomerId != 0)
                            {
                                Customers customer = DB.Customers.FirstOrDefault(x => x.CustomerId == record.r.CustomerId);
                                if (customer != null)
                                    report.CustomerName = customer.CustomerName;
                                else
                                    report.CustomerName = "N/A - This is a System Report.";
                            }

                            if (!string.IsNullOrWhiteSpace(record.r.ScheduleTime))
                            {
                                report.Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(record.r.ScheduleTime);
                                report.ScheduleCronFormat = GeneralTools.scheduleStringBuilder(record.r.ScheduleTime);
                            }
                            else
                            {
                                GlobalVars.ScheduleTime schedule = new GlobalVars.ScheduleTime();
                                report.Schedule = schedule;
                                report.ScheduleCronFormat = "";
                            }
                            var results1 = DB.ReportParameters.Where(y => y.TemplateId == report.TemplateID & y.ReportId == report.ReportID);
                            if (results1.Count() >= 1)
                            {
                                report.Parameters = new List<GlobalVars.ReportParameter>();
                                foreach (var y in results1)
                                {
                                    GlobalVars.ReportParameter reportParameter = new GlobalVars.ReportParameter()
                                    {
                                        TemplateID = y.TemplateId,
                                        ParameterID = y.ParameterId,
                                        ParameterName = GetParameterName(y.ParameterId),
                                        ReportID = y.ReportId,
                                        Value = (y.Value ?? "").Trim()
                                    };
                                    report.Parameters.Add(reportParameter);
                                }
                            }
                            reports.Add(report);
                        }
                    }
                }
                resultReports.ReturnValue = reports;
                resultReports.Message = "GetReports transaction completed successfully. Number of records found: " + resultReports.RecordsCount;
                logger.Debug(resultReports.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultReports.ReturnCode = -2;
                resultReports.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReports.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetReports Method ...");
            return resultReports;
        }

        /// <summary>
        ///  Get Template Parameter Name based for a given Parameter ID
        /// </summary>
        /// <param name="parameterID"></param>
        /// <returns></returns>
        static public string GetParameterName(int parameterID)
        {
            string parameterName = "";
            try
            {
                logger.Trace("Entering into GetParameterName Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    ReportsTemplateParameters templateParameter = DB.ReportsTemplateParameters.FirstOrDefault(x => x.ParameterId == parameterID);
                    if (templateParameter != null)
                        parameterName = (templateParameter.Name ?? "").Trim();                    
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
            }
            logger.Trace("Leaving GetParameterName Method ...");
            return parameterName;
        }

        /// <summary>
        /// Get Report Information By ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultReports GetReportByID(int reportID)
        {
            List<GlobalVars.Report> reports = new List<GlobalVars.Report>();
            GlobalVars.ResultReports resultReports = new GlobalVars.ResultReports()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = reports,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetReportByID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    var results = from r in DB.Reports
                                join t in DB.ReportsTemplate on r.TemplateId equals t.TemplateId
                                where r.ReportId == reportID
                                select new { r,t.Name};

                    resultReports.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var record in results)
                        {
                            GlobalVars.Report report = new GlobalVars.Report()
                            {
                                ReportName = (record.Name ?? "").Trim(),
                                TemplateID = record.r.TemplateId,
                                CustomerID = record.r.CustomerId,
                                EmailRecipients = (record.r.EmailRecipients ?? "").Trim(),
                                EmailSubject = (record.r.EmailSubject ?? "").Trim(),
                                EnableFlag = Convert.ToBoolean(record.r.EnableFlag),
                                ReportID = record.r.ReportId,
                                TitleContent1 = (record.r.TitleContent1 ?? "").Trim(),
                                TitleContent2 = (record.r.TitleContent2 ?? "").Trim(),
                                TitleContent3 = (record.r.TitleContent3 ?? "").Trim(),
                                TitleFontBoldFlag1 = Convert.ToBoolean(record.r.TitleFontBoldFlag1),
                                TitleFontBoldFlag2 = Convert.ToBoolean(record.r.TitleFontBoldFlag2),
                                TitleFontBoldFlag3 = Convert.ToBoolean(record.r.TitleFontBoldFlag3),
                                TitleFontColor1 = (record.r.TitleFontColor1 ?? "").Trim(),
                                TitleFontColor2 = (record.r.TitleFontColor2 ?? "").Trim(),
                                TitleFontColor3 = (record.r.TitleFontColor3 ?? "").Trim(),
                                TitleFontSize1 = record.r.TitleFontSize1.Value,
                                TitleFontSize2 = record.r.TitleFontSize2.Value,
                                TitleFontSize3 = record.r.TitleFontSize3.Value,
                                TableColumnNamesBackColor = (record.r.TableColumnNamesBackColor ?? "").Trim(),
                                TableColumnNamesFontBoldFlag = Convert.ToBoolean(record.r.TableColumnNamesFontBoldFlag),
                                TableColumnNamesFontColor = (record.r.TableColumnNamesFontColor ?? "").Trim(),
                                TableColumnNamesFontSize = record.r.TableColumnNamesFontSize.Value,
                                TableHeaderBackColor = (record.r.TableHeaderBackColor ?? "").Trim(),
                                TableHeaderFontBoldFlag = Convert.ToBoolean(record.r.TableHeaderFontBoldFlag),
                                TableHeaderFontColor = (record.r.TableHeaderFontColor ?? "").Trim(),
                                TableHeaderFontSize = record.r.TableHeaderFontSize.Value
                            };
                            // Get Customer Name
                            if (record.r.CustomerId != 0)
                            {
                                Customers customer = DB.Customers.FirstOrDefault(x => x.CustomerId == record.r.CustomerId);
                                if (customer != null)
                                    report.CustomerName = customer.CustomerName;
                                else
                                    report.CustomerName = "N/A - This is a System Report.";
                            }

                            if (!string.IsNullOrWhiteSpace(record.r.ScheduleTime))
                            {
                                report.Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(record.r.ScheduleTime);
                                report.ScheduleCronFormat = GeneralTools.scheduleStringBuilder(record.r.ScheduleTime);
                            }
                            else
                            {
                                GlobalVars.ScheduleTime schedule = new GlobalVars.ScheduleTime();
                                report.Schedule = schedule;
                                report.ScheduleCronFormat = "";
                            }
                            var results1 = DB.ReportParameters.Where(y => y.TemplateId == report.TemplateID & y.ReportId == report.ReportID);
                            if (results1.Count() >= 1)
                            {
                                report.Parameters = new List<GlobalVars.ReportParameter>();
                                foreach (var y in results1)
                                {
                                    GlobalVars.ReportParameter reportParameter = new GlobalVars.ReportParameter()
                                    {
                                        TemplateID = y.TemplateId,
                                        ParameterID = y.ParameterId,
                                        ParameterName = GetParameterName(y.ParameterId),
                                        ReportID = y.ReportId,
                                        Value = (y.Value ?? "").Trim()                                       
                                    };
                                    report.Parameters.Add(reportParameter);
                                }
                            }
                            reports.Add(report);
                        }
                    }
                }
                resultReports.ReturnValue = reports;
                resultReports.Message = "GetReportByID transaction completed successfully. Number of records found: " + resultReports.RecordsCount;
                logger.Debug(resultReports.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultReports.ReturnCode = -2;
                resultReports.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReports.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetReportTemplateByID Method ...");
            return resultReports;
        }

        /// <summary>
        /// Get Report Information By Customer and Template ID
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultReports GetReportByCustomerAndTemplateID(int customerID, int templateID)
        {
            List<GlobalVars.Report> reports = new List<GlobalVars.Report>();
            GlobalVars.ResultReports resultReports = new GlobalVars.ResultReports()
            {
                ReturnCode = 0,
                Message = "",
                ReturnValue = reports,
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GetReportByCustomerAndTemplateID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {

                    //var results3 = from r in DB.Reports
                    //              join t in DB.ReportsTemplate on r.TemplateId equals t.TemplateId
                    //              join c in DB.Customers on r.CustomerId equals c.CustomerId
                    //              where c.CustomerId == customerID && r.TemplateId == templateID
                    //              select new { r, t.Name };

                    var results = from r in DB.Reports
                                  join t in DB.ReportsTemplate on r.TemplateId equals t.TemplateId
                                  where r.CustomerId == customerID && r.TemplateId == templateID
                                  select new { r, t.Name };

                    //var results = DB.Reports.Where(x => x.TemplateId == templateID & x.CustomerId == customerID);
                    resultReports.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        foreach (var record in results)
                        {
                            GlobalVars.Report report = new GlobalVars.Report()
                            {
                                ReportName = (record.Name ?? "").Trim(),
                                TemplateID = record.r.TemplateId,
                                CustomerID = record.r.CustomerId,
                                EmailRecipients = (record.r.EmailRecipients ?? "").Trim(),
                                EmailSubject = (record.r.EmailSubject ?? "").Trim(),
                                EnableFlag = Convert.ToBoolean(record.r.EnableFlag),
                                ReportID = record.r.ReportId,
                                TitleContent1 = (record.r.TitleContent1 ?? "").Trim(),
                                TitleContent2 = (record.r.TitleContent2 ?? "").Trim(),
                                TitleContent3 = (record.r.TitleContent3 ?? "").Trim(),
                                TitleFontBoldFlag1 = Convert.ToBoolean(record.r.TitleFontBoldFlag1),
                                TitleFontBoldFlag2 = Convert.ToBoolean(record.r.TitleFontBoldFlag2),
                                TitleFontBoldFlag3 = Convert.ToBoolean(record.r.TitleFontBoldFlag3),
                                TitleFontColor1 = (record.r.TitleFontColor1 ?? "").Trim(),
                                TitleFontColor2 = (record.r.TitleFontColor2 ?? "").Trim(),
                                TitleFontColor3 = (record.r.TitleFontColor3 ?? "").Trim(),
                                TitleFontSize1 = record.r.TitleFontSize1.Value,
                                TitleFontSize2 = record.r.TitleFontSize2.Value,
                                TitleFontSize3 = record.r.TitleFontSize3.Value,
                                TableColumnNamesBackColor = (record.r.TableColumnNamesBackColor ?? "").Trim(),
                                TableColumnNamesFontBoldFlag = Convert.ToBoolean(record.r.TableColumnNamesFontBoldFlag),
                                TableColumnNamesFontColor = (record.r.TableColumnNamesFontColor ?? "").Trim(),
                                TableColumnNamesFontSize = record.r.TableColumnNamesFontSize.Value,
                                TableHeaderBackColor = (record.r.TableHeaderBackColor ?? "").Trim(),
                                TableHeaderFontBoldFlag = Convert.ToBoolean(record.r.TableHeaderFontBoldFlag),
                                TableHeaderFontColor = (record.r.TableHeaderFontColor ?? "").Trim(),
                                TableHeaderFontSize = record.r.TableHeaderFontSize.Value
                            };

                            // Get Customer Name
                            if (record.r.CustomerId != 0)
                            {
                                Customers customer = DB.Customers.FirstOrDefault(x => x.CustomerId == record.r.CustomerId);
                                if (customer != null)
                                    report.CustomerName = customer.CustomerName;
                                else
                                    report.CustomerName = "N/A - This is a System Report.";
                            }

                            if (!string.IsNullOrWhiteSpace(record.r.ScheduleTime))
                            {
                                report.Schedule = JsonConvert.DeserializeObject<GlobalVars.ScheduleTime>(record.r.ScheduleTime);
                                report.ScheduleCronFormat = GeneralTools.scheduleStringBuilder(record.r.ScheduleTime);
                            }
                            else
                            {
                                GlobalVars.ScheduleTime schedule = new GlobalVars.ScheduleTime();
                                report.Schedule = schedule;
                                report.ScheduleCronFormat = "";
                            }
                            var results1 = DB.ReportParameters.Where(y => y.TemplateId == report.TemplateID & y.ReportId == report.ReportID);
                            if (results1.Count() >= 1)
                            {
                                report.Parameters = new List<GlobalVars.ReportParameter>();
                                foreach (var y in results1)
                                {
                                    GlobalVars.ReportParameter reportParameter = new GlobalVars.ReportParameter()
                                    {
                                        TemplateID = y.TemplateId,
                                        ParameterID = y.ParameterId,
                                        ParameterName = GetParameterName(y.ParameterId),
                                        ReportID = y.ReportId,
                                        Value = (y.Value ?? "").Trim()
                                    };
                                    report.Parameters.Add(reportParameter);
                                }
                            }
                            reports.Add(report);
                        }
                    }
                }
                resultReports.ReturnValue = reports;
                resultReports.Message = "GetReportByCustomerAndTemplateID transaction completed successfully. Number of records found: " + resultReports.RecordsCount;
                logger.Debug(resultReports.Message);
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                resultReports.ReturnCode = -2;
                resultReports.Message = e.Message;
                var baseException = e.GetBaseException();
                resultReports.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GetReportByCustomerAndTemplateID Method ...");
            return resultReports;
        }
        /// <summary>
        /// Update an existing report o create a new one if the Report does not exist
        /// The Method will check if the Customer ID , and Template ID exist
        /// If the Report's Parameters that are given are not valid, they will be ignored.
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateReport(GlobalVars.Report report)
        {
            string reportType = "";
            Boolean continueProcessing = true;
            GlobalVars.ResultReportsTemplate resultReportTemplates = new GlobalVars.ResultReportsTemplate();
            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into UpdateReport Method ...");

                // Check if Report Template ID exist
                resultReportTemplates = SQLFunctionsReports.GetReportTemplateByID(report.TemplateID);
                if (resultReportTemplates.RecordsCount != 0)
                {
                    reportType = resultReportTemplates.ReturnValue[0].Type;
                    if (reportType.Contains("Customer"))
                    {
                        // Only for Customer Reports
                        // Check if the Customer ID given is in the Database
                        result = SQLFunctionsCustomers.ExistCustomerID(report.CustomerID);
                        if (result.RecordsCount != 0)
                        {
                            // Continue processing
                        }
                        else
                        {
                            // Given Customer ID could not be found, so Report cannot be updated
                            result.Message = "Customer ID " + report.CustomerID + " Does not exist, so Report cannot be updated";
                            result.ReturnCode = -1;
                            continueProcessing = false;
                        }
                    }
                    else
                    {
                        // This is a System Report, so Customer ID to be used is "0"
                        report.CustomerID = 0;
                    }
                    if (continueProcessing)
                    {
                        string scheduleJS = JsonConvert.SerializeObject(report.Schedule, Newtonsoft.Json.Formatting.Indented);
                        using (ScanningDBContext DB = new ScanningDBContext())
                        {
                            // AT this point, we know that he customer and the Report template are both in the Database, so
                            // the report can be created or updated
                            Reports Matching_Result = DB.Reports.FirstOrDefault(x => x.CustomerId == Convert.ToInt32(report.CustomerID) & x.TemplateId == Convert.ToInt32(report.TemplateID));
                            Reports record = new Reports();
                            record.TemplateId = report.TemplateID;
                            record.CustomerId = report.CustomerID;
                            record.EmailRecipients = report.EmailRecipients;
                            record.EmailSubject = report.EmailSubject;
                            record.EnableFlag = report.EnableFlag.ToString();
                            record.ReportId = report.ReportID;
                            record.TitleContent1 = report.TitleContent1;
                            record.TitleContent2 = report.TitleContent2;
                            record.TitleContent3 = report.TitleContent3;
                            record.TitleFontBoldFlag1 = report.TitleFontBoldFlag1.ToString();
                            record.TitleFontBoldFlag2 = report.TitleFontBoldFlag2.ToString();
                            record.TitleFontBoldFlag3 = report.TitleFontBoldFlag3.ToString();
                            record.TitleFontColor1 = report.TitleFontColor1;
                            record.TitleFontColor2 = report.TitleFontColor2;
                            record.TitleFontColor3 = report.TitleFontColor3;
                            record.TitleFontSize1 = report.TitleFontSize1;
                            record.TitleFontSize2 = report.TitleFontSize2;
                            record.TitleFontSize3 = report.TitleFontSize3;
                            record.TableHeaderFontColor = report.TableHeaderFontColor;
                            record.TableHeaderBackColor = report.TableHeaderBackColor;
                            record.TableHeaderFontBoldFlag = report.TableHeaderFontBoldFlag.ToString();
                            record.TableHeaderFontSize = report.TableHeaderFontSize;
                            record.TableColumnNamesBackColor = report.TableColumnNamesBackColor;
                            record.TableColumnNamesFontBoldFlag = report.TableColumnNamesFontBoldFlag.ToString();
                            record.TableColumnNamesFontColor = report.TableColumnNamesFontColor;
                            record.TableColumnNamesFontSize = report.TableColumnNamesFontSize;
                            record.ScheduleTime = scheduleJS;

                            if (Matching_Result == null)
                            {
                                // Means --> the Report given was not found in the Database so a new record will be created
                                DB.Reports.Add(record);
                                DB.SaveChanges();
                                // Need to get now the Report ID which is going to be used while creating the parameters list
                                Reports Matching_Result_1 = DB.Reports.FirstOrDefault(x => x.CustomerId == Convert.ToInt32(report.CustomerID) & x.TemplateId == Convert.ToInt32(report.TemplateID));
                                if (Matching_Result_1 != null)
                                {
                                    if (report.Parameters != null)
                                    {
                                        foreach (GlobalVars.ReportParameter parameter in report.Parameters)
                                        {
                                            parameter.ReportID = Matching_Result_1.ReportId;
                                            UpdateReportParameter(parameter);
                                        }
                                    }
                                }                                                                  
                                result.Message = "There was not information associated to the given Report, so new records was created successfully.";
                            }
                            else
                            {
                                // Means --> table has a record and it will be updated
                                Matching_Result.EmailRecipients = report.EmailRecipients;
                                Matching_Result.EmailSubject = report.EmailSubject;
                                Matching_Result.EnableFlag = report.EnableFlag.ToString();
                                //Matching_Result.ReportId = report.ReportID;
                                Matching_Result.TitleContent1 = report.TitleContent1;
                                Matching_Result.TitleContent2 = report.TitleContent2;
                                Matching_Result.TitleContent3 = report.TitleContent3;
                                Matching_Result.TitleFontBoldFlag1 = report.TitleFontBoldFlag1.ToString();
                                Matching_Result.TitleFontBoldFlag2 = report.TitleFontBoldFlag2.ToString();
                                Matching_Result.TitleFontBoldFlag3 = report.TitleFontBoldFlag3.ToString();
                                Matching_Result.TitleFontColor1 = report.TitleFontColor1;
                                Matching_Result.TitleFontColor2 = report.TitleFontColor2;
                                Matching_Result.TitleFontColor3 = report.TitleFontColor3;
                                Matching_Result.TitleFontSize1 = report.TitleFontSize1;
                                Matching_Result.TitleFontSize2 = report.TitleFontSize2;
                                Matching_Result.TitleFontSize3 = report.TitleFontSize3;
                                Matching_Result.TableHeaderFontColor = report.TableHeaderFontColor;
                                Matching_Result.TableHeaderBackColor = report.TableHeaderBackColor;
                                Matching_Result.TableHeaderFontBoldFlag = report.TableHeaderFontBoldFlag.ToString();
                                Matching_Result.TableHeaderFontSize = report.TableHeaderFontSize;
                                Matching_Result.TableColumnNamesBackColor = report.TableColumnNamesBackColor;
                                Matching_Result.TableColumnNamesFontBoldFlag = report.TableColumnNamesFontBoldFlag.ToString();
                                Matching_Result.TableColumnNamesFontColor = report.TableColumnNamesFontColor;
                                Matching_Result.TableColumnNamesFontSize = report.TableColumnNamesFontSize;
                                Matching_Result.ScheduleTime = scheduleJS;

                                DB.SaveChanges();
                                if (report.Parameters != null)
                                {
                                    foreach (GlobalVars.ReportParameter parameter in report.Parameters)
                                    {
                                        UpdateReportParameter(parameter);
                                    }
                                }
                                result.Message = "Report Information was updated successfully.";
                            }
                        }
                    }
                }
                else
                {
                    // Template ID does not exist in the Database. Cannot continue the Update Operation
                    // Given Customer ID could not be found, so Report cannot be updated
                    result.Message = "Termplate ID " + report.TemplateID + " Does not exist, so Report cannot be updated";
                    result.ReturnCode = -1;
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
            logger.Trace("Leaving UpdateReport Method ...");
            return result;
        }

        /// <summary>
        /// Check if a given Report ID exist in the Database
        /// If Report ID Exist, 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistReportID(int reportID)
        {
            List<GlobalVars.Report> reports = new List<GlobalVars.Report>();

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistReportID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.Reports.Where(x => x.ReportId == reportID);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Report ID " + reportID + " already exist.";
                    }
                    else
                    {
                        result.Message = "Report ID " + reportID + " doest not exist.";
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
            logger.Trace("Leaving ExistReportID Method ...");
            return result;
        }

        /// <summary>
        /// Check if a Temlate ID exist in the Database
        /// Note: Report templates does not depend on Customer or Reports, but a Customer or System Report relays on
        /// a predefined Report Template
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistReportTemplateID(int templateID)
        {
            List<GlobalVars.Report> reports = new List<GlobalVars.Report>();

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistReportTemplateID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ReportsTemplate.Where(x => x.TemplateId == templateID);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Template ID " + templateID + " already exist.";
                    }
                    else
                    {
                        result.Message = "Template ID " + templateID + " doest not exist.";
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
            logger.Trace("Leaving ExistReportTemplateID Method ...");
            return result;
        }
        
        /// <summary>
        /// Check if a given Parameter ID, within a template ID,  exist in the Database
        /// Note: This methos is used to check of the parameter for a given Reports match with the definition of the
        /// Parameters defined for an existing Template ID. Reports must honor the fields that were defined for the 
        /// associated Template, so that is why the ID for the Template and the Parameters are required for  this Method.
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric ExistReportParameterID(int templateID, int parameterID)
        {
            List<GlobalVars.Report> reports = new List<GlobalVars.Report>();

            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into ExistReportParameterID Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    var results = DB.ReportsTemplateParameters.Where(x => x.ParameterId == parameterID & x.TemplateId == templateID);
                    result.RecordsCount = results.Count();
                    if (results.Count() >= 1)
                    {
                        result.Message = "Report Parameter ID " + parameterID + " already exist.";
                    }
                    else
                    {
                        result.Message = "Report Parameter ID " + parameterID + " doest not exist for Template ID " + templateID;
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
            logger.Trace("Leaving ExistReportParameterID Method ...");
            return result;
        }


        /// <summary>
        /// This method generate work orders reports
        /// The Method will send an email per Job Type x Customer 
        /// </summary>
        /// <param name="workOrders">This is a list of work orders numbers separated by comma</param>
        /// <param name="sendEmail"></param>
        /// <param name="attachPDF"></param>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric GenerateWorkOrdersReport(string workOrders, Boolean sendEmail, Boolean attachPDF)
        {
            List<GlobalVars.WorkOrderCustomer> workOrderCompanies = new List<GlobalVars.WorkOrderCustomer>();
            Boolean showNumberOfDocuments = false;
            Boolean showNumberOfScannedImages = false;
            Boolean showNumberOfKeystrokes = false;
            Boolean showNumberOfBlankImages = false;
            Boolean showPrepTime = false;
            Boolean showPageSizeCategories = false;

            GlobalVars.ResultBatches resultBatches = new GlobalVars.ResultBatches();
            GlobalVars.ResultReportsTemplate reportsTemplate = new GlobalVars.ResultReportsTemplate();
            GlobalVars.ResultCustomers resultCustomers = new GlobalVars.ResultCustomers();
            GlobalVars.ResultReports resultReports = new GlobalVars.ResultReports();
            GlobalVars.Report report = new GlobalVars.Report();
            GlobalVars.Customer customer = new GlobalVars.Customer();
            GlobalVars.EMAIL email = new GlobalVars.EMAIL();

            List<GlobalVars.Batch> batches = new List<GlobalVars.Batch>();
            List<GlobalVars.Batch> batchesSorted = new List<GlobalVars.Batch>();
            string currentCustomer = "";
            string currentJobType = "";
            string body = "";
            string customerBody = "";
            int templateID = 0;
            List<string> workList = new List<string>();


            GlobalVars.ResultGeneric result = new GlobalVars.ResultGeneric()
            {
                ReturnCode = 0,
                Message = "",
                RecordsCount = 0,
                HttpStatusCode = ""
            };
            try
            {
                logger.Trace("Entering into GenerateWorkOrdesReport Method ...");

                // Split the Work Orders into a List
                if (workOrders.Contains(","))
                {
                    // More that one Work Orders was detected
                    workList = workOrders.Split(',').ToList();
                }
                else
                {
                    // One work order detected
                    workList.Add(workOrders);
                }

                Boolean newCustomer;
                Boolean newJobType;
                using (ScanningDBContext DB = new ScanningDBContext())
                {
                    // Merger all Batches contained in Work Orders so they can be Sorted by Customer Name
                    foreach (string workOrder in workList)
                    {                        
                        resultBatches = SQLFunctionsBatches.GetBatchesInformation("LotNumber =\"" + workOrder + "\"", "");
                        if (resultBatches.RecordsCount > 0)
                        {
                            // Add Batches to the List
                            foreach (GlobalVars.Batch batch in resultBatches.ReturnValue)
                            {
                                batches.Add(batch);
                            }
                        }
                    }
                    if (batches.Count != 0)
                    {
                        // Sort Batches by Customer, and LotNumber
                        batchesSorted = batches.OrderBy(s => s.Customer).ThenBy(x => x.JobType).ThenBy(x => x.LotNumber).ToList();
                    }

                    // Build the List so the reports can be easly created
                    // Feed Customer Info
                    foreach (GlobalVars.Batch batchSorted in batchesSorted)
                    {
                        if (currentCustomer != batchSorted.Customer)
                        {
                            currentCustomer = batchSorted.Customer;
                            newCustomer = true;
                            foreach (GlobalVars.WorkOrderCustomer woCustomer in workOrderCompanies)
                            {
                                if (woCustomer.CustomerName == currentCustomer)
                                {
                                    newCustomer = false;
                                    break;
                                }
                            }
                            if (newCustomer)
                            {
                                GlobalVars.WorkOrderCustomer workOrderCompany = new GlobalVars.WorkOrderCustomer();
                                workOrderCompany.CustomerName = currentCustomer;
                                workOrderCompanies.Add(workOrderCompany);
                                
                            }                            
                        }
                    }
                    //Feed Job Type info
                    foreach (GlobalVars.Batch batchSorted in batchesSorted)
                    {
                        if (currentJobType != batchSorted.JobType)
                        {
                            currentJobType = batchSorted.JobType;
                            newJobType = true;
                            foreach (GlobalVars.WorkOrderCustomer woCustomer in workOrderCompanies)
                            {
                                //newJobType = true;
                                if (woCustomer.JobTypeNames is null)
                                {
                                    GlobalVars.WorkOrderJobType workOrderJobType = new GlobalVars.WorkOrderJobType();
                                    woCustomer.JobTypeNames = new List<GlobalVars.WorkOrderJobType>();
                                    workOrderJobType.JobName = currentJobType;
                                    woCustomer.JobTypeNames.Add(workOrderJobType);
                                    break;
                                }
                                else
                                {
                                    foreach (GlobalVars.WorkOrderJobType woJobType in woCustomer.JobTypeNames)
                                    {
                                        if (woJobType.JobName == currentJobType)
                                        {
                                            newJobType = false;
                                            break;
                                        }
                                    }
                                    if (newJobType)
                                    {
                                        GlobalVars.WorkOrderJobType workOrderJobType = new GlobalVars.WorkOrderJobType();
                                        workOrderJobType.JobName = currentJobType;
                                        woCustomer.JobTypeNames.Add(workOrderJobType);
                                        break;
                                    }
                                }                                
                            }
                        }
                    }

                    //Feed Batches info                    
                    foreach (GlobalVars.Batch batchSorted in batchesSorted)
                    {
                        foreach (GlobalVars.WorkOrderCustomer woCustomer in workOrderCompanies)
                        {
                            if (woCustomer.CustomerName == batchSorted.Customer)
                            {
                                foreach (GlobalVars.WorkOrderJobType woJobType in woCustomer.JobTypeNames)
                                {
                                    if (woJobType.JobName == batchSorted.JobType)
                                    {
                                        // Add Batch
                                        if (woJobType.batches is null)
                                        {
                                            woJobType.batches = new List<GlobalVars.Batch>();
                                        }                                            
                                        woJobType.batches.Add(batchSorted);
                                        break;
                                    }
                                }
                                break;
                            }                            
                        }
                    }

                    // Build Work Order Report Summary Information
                    foreach (GlobalVars.WorkOrderCustomer woCustomer in workOrderCompanies)
                    {
                        foreach (GlobalVars.WorkOrderJobType woJobType in woCustomer.JobTypeNames)
                        {
                            foreach (GlobalVars.Batch batch in woJobType.batches)
                            {
                                woJobType.NumberBoxes = woJobType.NumberBoxes + 1;
                                //woJobType.NumberBlankImages = 
                                woJobType.NumberDocs = woJobType.NumberDocs + batch.NumberOfDocuments;
                                woJobType.NumberKeystrokes = woJobType.NumberKeystrokes + batch.KeysStrokes;
                                woJobType.NumberScannedImages = woJobType.NumberScannedImages + batch.NumberOfScannedPages;
                                woJobType.PrepTime = woJobType.PrepTime + batch.PrepTime;
                            }
                            woCustomer.NumberBoxes = woCustomer.NumberBoxes + woJobType.NumberBoxes;
                            //woJobType.NumberBlankImages = 
                            woCustomer.NumberDocs = woCustomer.NumberDocs + woJobType.NumberDocs;
                            woCustomer.NumberKeystrokes = woCustomer.NumberKeystrokes + woJobType.NumberKeystrokes;
                            woCustomer.NumberScannedImages = woCustomer.NumberScannedImages + woJobType.NumberScannedImages;
                            woCustomer.PrepTime = woCustomer.PrepTime + woJobType.PrepTime;
                        } 
                    }

                    // Get the Work Order Report Template ID
                    reportsTemplate = SQLFunctionsReports.GetReportsTemplate("Customer");
                    foreach (GlobalVars.ReportTemplate template in reportsTemplate.ReturnValue)
                    {
                        if (template.Name == "WORK_ORDER_REPORT")
                        {
                            templateID = template.TemplateID;
                        }
                    }

                    // ---------------------------------------------------------------------------------------------------------------
                    // Build Work Order Report
                    // One Email Report by Customner's Job TYpe
                    foreach (GlobalVars.WorkOrderCustomer woCustomer in workOrderCompanies)
                    {
                        showNumberOfDocuments = false;
                        showNumberOfScannedImages = false;
                        showNumberOfKeystrokes = false;
                        showNumberOfBlankImages = false;
                        showPrepTime = false;
                        showPageSizeCategories = false;

                        // Need Customer ID, so Get Customer Information
                        resultCustomers = SQLFunctionsCustomers.GetCustomerByName(woCustomer.CustomerName);
                        customer = resultCustomers.ReturnValue[0];
                        customerBody = ""; 

                        // Need Report Information to decide which columns must be considered in the Report
                        resultReports = SQLFunctionsReports.GetReportByCustomerAndTemplateID(customer.CustomerID, templateID);
                        report = resultReports.ReturnValue[0];

                        // Find out which one of the report columns needs to be presented for this Job Type in the Work Order Report
                        foreach (GlobalVars.ReportParameter parameter in report.Parameters)
                        {
                            switch (parameter.ParameterName)
                            {
                                case "ShowNumberOfDocuments":
                                    if (string.IsNullOrEmpty(parameter.Value))
                                        showNumberOfDocuments = false;
                                    else
                                        showNumberOfDocuments = Convert.ToBoolean(parameter.Value);
                                    break;
                                case "ShowNumberOfScannedImages":
                                    if (string.IsNullOrEmpty(parameter.Value))
                                        showNumberOfScannedImages = false;
                                    else
                                        showNumberOfScannedImages = Convert.ToBoolean(parameter.Value);
                                    break;
                                case "ShowNumberOfKeystrokes":
                                    if (string.IsNullOrEmpty(parameter.Value))
                                        showNumberOfKeystrokes = false;
                                    else                                        
                                        showNumberOfKeystrokes = Convert.ToBoolean(parameter.Value);
                                    break;
                                case "ShowNumberBlankImages":
                                    if (string.IsNullOrEmpty(parameter.Value))
                                        showNumberOfBlankImages = false;
                                    else
                                        showNumberOfBlankImages = Convert.ToBoolean(parameter.Value);
                                    break;
                                case "ShowPrepTime":
                                    if (string.IsNullOrEmpty(parameter.Value))
                                        showPrepTime = false;
                                    else
                                        showPrepTime = Convert.ToBoolean(parameter.Value);
                                    break;

                                case "ShowPageSizeCategories":
                                    if (string.IsNullOrEmpty(parameter.Value))
                                        showPageSizeCategories = false;
                                    else
                                        showPageSizeCategories = Convert.ToBoolean(parameter.Value);
                                    break;
                            }
                        }

                        // Build the Header of the email
                        customerBody = "<!DOCTYPE HTML PUBLIC \"-//W3C/DTD HTML 4.0 Transitional//EN\">";
                        customerBody += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                        customerBody += "</HEAD>";
                        customerBody += "<BODY>";
                        customerBody += "<TABLE border='0' cellpadding='1' cellspacing='1' width='800'>";
                        customerBody += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                        if (report.TitleContent1.Length > 0)
                        {
                            customerBody += "<tr>";
                            if (report.TitleFontBoldFlag1)
                                customerBody += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                        "<b>" + report.TitleContent1 + "</b>" + "</font></td>";
                            else
                                customerBody += "<td align='center'><font color='" + report.TitleFontColor1 + "' size='" + report.TitleFontSize1 + "'>" +
                                        report.TitleContent1 + "</font></td>";
                            customerBody += "</tr>";
                        }
                        if (report.TitleContent2.Length > 0)
                        {
                            customerBody += "<tr>";
                            if (report.TitleFontBoldFlag2)
                                customerBody += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2  + "'>" +
                                        "<b>" + report.TitleContent2 + "</b>" + "</font></td>";
                            else
                                customerBody += "<td align='center'><font color='" + report.TitleFontColor2 + "' size='" + report.TitleFontSize2 + "'>" +
                                        report.TitleContent2 + "</font></td>";
                            customerBody += "</tr>";
                        }
                        if (report.TitleContent3.Length > 0)
                        {
                            customerBody += "<tr>";
                            if (report.TitleFontBoldFlag3)
                                customerBody += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                        "<b>" + report.TitleContent3 + "</b>" + "</font></td>";
                            else
                                customerBody += "<td align='center'><font color='" + report.TitleFontColor3 + "' size='" + report.TitleFontSize3 + "'>" +
                                        report.TitleContent3 + "</font></td>";
                            customerBody += "</tr>";
                        }
                        customerBody += "</tbody>";
                        customerBody += "</TABLE>";
                        customerBody += "<br/>";
                        // Enf ofthe Email Header
                        
                        // Build JobType Subtable
                        foreach (GlobalVars.WorkOrderJobType woJobType in woCustomer.JobTypeNames)
                        {
                            //customerBody = customerBody + WorkOrderReportHelper(report, woJobType);
                            email.Body = customerBody + WorkOrderReportHelper(report, woJobType);
                            if (sendEmail)
                            {
                                //email.Body = customerBody;
                                email.RecipientsEmailAddress = report.EmailRecipients;
                                email.Subject = report.EmailSubject;
                                if (attachPDF)
                                {
                                    StringReader sr = new StringReader(email.Body);
                                    //Document pdfDoc = new Document(PageSize.LEDGER, 10f, 10f, 60f, 30f);
                                    Document pdfDoc = new Document(PageSize.A3, 10f, 10f, 30f, 30f); // PageSize.A4, 10f, 10f, 10f, 0f);

                                    HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                        pdfDoc.Open();
                                        //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                                        htmlparser.Parse(sr);
                                        pdfDoc.Close();
                                        byte[] bytes = memoryStream.ToArray();
                                        memoryStream.Close();
                                        email.attachment = bytes;
                                        email.HasAttachment = true;
                                    }
                                }
                                SQLFunctionsSMTP.SendEmail(email);
                            }
                        }

                        // Build Grant Total section
                        // Creating the JobType Summary section
                        //customerBody += "<TABLE border='1' cellpadding='0' cellspacing='0' width='800'>";
                        //customerBody += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                        //customerBody += "<tr bgcolor='" + report.TableHeaderBackColor + "'>";
                        //customerBody += "<td align='center'><font color='" + report.TableHeaderFontColor + "' size='" + report.TableHeaderFontSize + "'> FINAL SUMMARY REPORT FOR " + woCustomer.CustomerName.ToUpper() + "</font></td>";
                        //customerBody += "</tr>";
                        //customerBody += "</tbody>";
                        //customerBody += "</TABLE>";

                        //customerBody += "<TABLE border='1' cellpadding='1' cellspacing='1' width='800'>";
                        //customerBody += "<tbody align='center' style='font-family:verdana; color:'black'; background-color:'silver'>";
                        //customerBody += "<tr bgcolor='" + report.TableColumnNamesBackColor + "'><b>";

                        //customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Boxes</td>";
                        //if (showNumberOfDocuments)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Docs</td>";
                        //if (showNumberOfScannedImages)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Scanned Images</td>";
                        //if (showNumberOfBlankImages)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'># Blanc Images</td>";
                        //if (showNumberOfKeystrokes)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Indexing</td>";
                        //if (showPrepTime)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>Prep Time</td>";
                        //customerBody += "</b></tr>";

                        //customerBody += "<tr>";
                        //customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woCustomer.NumberBoxes.ToString("0,0") + "</td>";
                        //if (showNumberOfDocuments)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woCustomer.NumberDocs.ToString("0,0") + "</td>";
                        //if (showNumberOfScannedImages)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woCustomer.NumberScannedImages.ToString("0,0") + "</td>";
                        //if (showNumberOfBlankImages)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woCustomer.NumberBlankImages.ToString("0,0") + "</td>";
                        //if (showNumberOfKeystrokes)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woCustomer.NumberKeystrokes.ToString("0,0") + "</td>";
                        //if (showPrepTime)
                        //    customerBody += "<td align='left' width='100'><font color='" + report.TableColumnNamesFontColor + "' size='" + report.TableColumnNamesFontSize + "'>" + woCustomer.PrepTime.ToString("0,0") + "</td>";
                        //customerBody += "</tr>";

                        //customerBody += "</tbody>";
                        //customerBody += "</TABLE>";

                        // Sennd Email with the Attached PDF
                        //if (sendEmail)
                        //{
                        //    email.Body = customerBody;
                        //    email.RecipientsEmailAddress = report.EmailRecipients;
                        //    email.Subject = report.EmailSubject;
                        //    if (attachPDF)
                        //    {
                        //        StringReader sr = new StringReader(customerBody);
                        //        //Document pdfDoc = new Document(PageSize.LEDGER, 10f, 10f, 60f, 30f);
                        //        Document pdfDoc = new Document(PageSize.A3, 10f, 10f, 30f, 30f); // PageSize.A4, 10f, 10f, 10f, 0f);

                        //        HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                        //        using (MemoryStream memoryStream = new MemoryStream())
                        //        {
                        //            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                        //            pdfDoc.Open();
                        //            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        //            htmlparser.Parse(sr);
                        //            pdfDoc.Close();
                        //            byte[] bytes = memoryStream.ToArray();
                        //            memoryStream.Close();
                        //            email.attachment = bytes;
                        //            email.HasAttachment = true;
                        //        }
                        //    }
                        //    SQLFunctionsSMTP.SendEmail(email);
                        //}
                    }                    
                }
            }
            catch (Exception e)
            {
                logger.Error("Error:" + e.Message + "\n" + "Exception: " + e.InnerException);
                result.ReturnCode = -2;
                result.Message = e.Message;
                var baseException = e.GetBaseException();
                result.Exception = baseException.ToString();
            }
            logger.Trace("Leaving GenerateWorkOrdesReport Method ...");
            return result;
        }


        /// <summary>
        /// Check if a Temlate ID exist in the Database
        /// Note: Report templates does not depend on Customer or Reports, but a Customer or System Report relays on
        /// a predefined Report Template
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric GenerateReport(int reportID, int templateID, Boolean sendEmail, Boolean attachPDF)
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
                logger.Trace("Entering into GenerateReport Method ...");
                using (ScanningDBContext DB = new ScanningDBContext())
                {          
                    // Get the Template Name, then, based on this name, invoque the corressponding report
                    ReportsTemplate reportTemplate = DB.ReportsTemplate.FirstOrDefault(x => x.TemplateId == templateID);
                    if (reportTemplate != null)
                    {                        
                        switch (reportTemplate.Name.Trim())
                        {
                            case "QC_OPERATOR_REPORT":
                                //result.Message = "This Report has not been implemented yet.";
                                // Get report Information
                                result = SQLFunctionsReports.QCDailyOperationReport(reportID);
                                if (sendEmail)
                                {
                                    // Get report Information
                                    GlobalVars.ResultReports reports = SQLFunctionsReports.GetReportByID(reportID);
                                    if (reports.RecordsCount > 0)
                                    {
                                        GlobalVars.EMAIL email = new GlobalVars.EMAIL();
                                        email.Body = result.StringReturnValue;
                                        email.RecipientsEmailAddress = reports.ReturnValue[0].EmailRecipients;
                                        email.Subject = reports.ReturnValue[0].EmailSubject;
                                        SQLFunctionsSMTP.SendEmail(email);
                                    }
                                    else
                                    {
                                        // Report ID not found
                                        result.Message = "Report ID " + reportID + " doest not exist.";
                                    }
                                }
                                break;

                            case "OVERALL_BATCH_STATUS_NOTIFICATION":
                                result = SQLFunctionsReports.OverallBatchStatusReport(reportID);
                                if (sendEmail)
                                {
                                    // Get report Information
                                    GlobalVars.ResultReports reports = SQLFunctionsReports.GetReportByID(reportID);
                                    if (reports.RecordsCount > 0)
                                    {
                                        GlobalVars.EMAIL email = new GlobalVars.EMAIL();
                                        email.Body = result.StringReturnValue;

                                        if (attachPDF)
                                        {
                                            StringReader sr = new StringReader(result.StringReturnValue);
                                            Document pdfDoc = new Document(PageSize.Letter, 60f, 30f, 60f, 30f); // PageSize.A4, 10f, 10f, 10f, 0f);
                                            HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                                            using (MemoryStream memoryStream = new MemoryStream())
                                            {
                                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                                pdfDoc.Open();
                                                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                                                htmlparser.Parse(sr);
                                                pdfDoc.Close();
                                                byte[] bytes = memoryStream.ToArray();
                                                memoryStream.Close();
                                                email.attachment = bytes;
                                                email.HasAttachment = true;
                                            }
                                        }
                                        else
                                        {
                                            email.HasAttachment = false;
                                        }                                       

                                        email.RecipientsEmailAddress = reports.ReturnValue[0].EmailRecipients;
                                        email.Subject = reports.ReturnValue[0].EmailSubject;
                                        SQLFunctionsSMTP.SendEmail(email);
                                    }
                                    else
                                    {
                                        // Report ID not found
                                        result.Message = "Report ID " + reportID + " doest not exist.";
                                    }
                                }
                                break;

                            case "SSS_BATCH_REPORT_BY_STATUS":
                                result = SQLFunctionsReports.BatchByStatusReport(reportID);
                                if (sendEmail && result.BooleanReturnValue)
                                {
                                    // Get report Information
                                    GlobalVars.ResultReports reports = SQLFunctionsReports.GetReportByID(reportID);
                                    if (reports.RecordsCount > 0)
                                    {
                                        GlobalVars.EMAIL email = new GlobalVars.EMAIL();
                                        email.Body = result.StringReturnValue;

                                        StringReader sr = new StringReader(result.StringReturnValue);
                                        Document pdfDoc = new Document(PageSize.Letter,30f,30f,30f,30f); // PageSize.A4, 10f, 10f, 10f, 0f);
                                        HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                                        using (MemoryStream memoryStream = new MemoryStream())
                                        {
                                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                                            pdfDoc.Open();
                                            htmlparser.Parse(sr);
                                            pdfDoc.Close();
                                            byte[] bytes = memoryStream.ToArray();
                                            memoryStream.Close();
                                            email.attachment = bytes;
                                            email.HasAttachment = true;
                                        }

                                        email.RecipientsEmailAddress = reports.ReturnValue[0].EmailRecipients;
                                        email.Subject = reports.ReturnValue[0].EmailSubject;
                                        result = SQLFunctionsSMTP.SendEmail(email);
                                    }
                                    else
                                    {
                                        // Report ID not found
                                        result.Message = "Report ID " + reportID + " doest not exist.";
                                    }
                                }
                                break;

                            case "SCANNED_BATCHES_WEEKLY_REPORT":
                                result.Message = "This Report has not been implemented yet.";
                                Console.WriteLine("Case 2");
                                break;

                            case "SSS_VFR_DOCS_REPORT":
                                result.Message = "This Report has not been implemented yet.";
                                Console.WriteLine("Case 2");
                                break;
                               
                            case "VFR_UPLOAD_REPORT":
                                // This report shows a list of Batches that arer ready for customer approval
                                result.Message = "This Report has not been implemented yet.";
                                Console.WriteLine("Case 2");
                                break;

                            default:
                                //Console.WriteLine("Default case");
                                result.Message = "This Report has not been implemented yet.";
                                break;
                        }
                    }
                    else
                    {
                        result.Message = "Template ID " + templateID + " doest not exist.";
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
            logger.Trace("Leaving GenerateReport Method ...");
            return result;
        }

        /// <summary>
        /// This Method will update the value of a Report's Parameter in the Database
        /// In order to update a Parameter Value for a given Report, the following condidions apply:
        /// - The Given Report ID must exist in the Database
        /// - The Given Template ID must exist in the Database
        /// - The Given Report must have the given Template associated to it
        /// - The given Parameter ID for a particular Template ID must Exist in the Datbase
        /// - 
        /// </summary>
        /// <returns></returns>
        static public GlobalVars.ResultGeneric UpdateReportParameter(GlobalVars.ReportParameter parameter)
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
                logger.Trace("Entering into UpdateReportParameter Method ...");

                // Checkin if the given Report ID Exist in the Databse
                result = SQLFunctionsReports.ExistReportID(parameter.ReportID);
                if (result.RecordsCount != 0)
                {
                    // Check if the Template ID exist in the Database
                    result = SQLFunctionsReports.ExistReportTemplateID(parameter.TemplateID);
                    if (result.RecordsCount != 0)
                    {
                        // Check if the Given Parameter ID for a particular Temnplate ID is valid
                        result = SQLFunctionsReports.ExistReportParameterID(parameter.TemplateID,parameter.ParameterID);
                        if (result.RecordsCount != 0)
                        {
                            // Now we should check if the Report's parameter ID exist in the Database
                            using (ScanningDBContext DB = new ScanningDBContext())
                            {
                                ReportParameters Matching_Result = DB.ReportParameters.FirstOrDefault(x => x.ParameterId == parameter.ParameterID & x.ReportId == parameter.ReportID);
                                ReportParameters record = new ReportParameters();
                                record.ParameterId = parameter.ParameterID;
                                record.ReportId = parameter.ReportID;
                                record.TemplateId = parameter.TemplateID;
                                record.Value = parameter.Value;

                                if (Matching_Result == null)
                                {
                                    // Means --> the Report's parameter does not exist, so create parameter
                                    DB.ReportParameters.Add(record);
                                    DB.SaveChanges();
                                    result.Message = "There was not information associated to the given Report's Parameter, so new records was created successfully.";
                                }
                                else
                                {
                                    // Means --> table has a record and it will be updated
                                    Matching_Result.Value = parameter.Value;
                                    DB.SaveChanges();
                                    result.Message = "Report's parameter Information was updated successfully.";
                                }
                            }
                        }
                        else
                        {
                            // Given Parameter ID Does not exist, so Report cannot be updated
                            result.Message = "Parameter ID " + parameter.ParameterID + " for Template ID " + parameter.TemplateID + "Does not exist, so Report's Parameter cannot be updated";
                            result.ReturnCode = -1;
                        }
                    }
                    else
                    {
                        // Given Template ID Does not exist, so Report cannot be updated
                        result.Message = "Template ID "+ parameter.TemplateID + " Does not exist, so Report's Parameter cannot be updated";
                        result.ReturnCode = -1;
                    }
                }
                else
                {
                    // Given Report ID Does not exist, so Report cannot be updated
                    result.Message = "Report ID "+ parameter.ReportID + " Does not exist, so Report's Parameter cannot be updated";
                    result.ReturnCode = -1;
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
            logger.Trace("Leaving UpdateReportParameter Method ...");
            return result;
        }
    }
}
