using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Runtime.InteropServices;

namespace ScanningServicesAdmin.Forms
{
    public partial class BatchTrackingForm : Form
    {
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        public BatchTrackingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BatchTrackingForm_Load(object sender, EventArgs e)
        {
            try
            {
                //  Data.GlovalVariables.currentBatchName
                ResultBatchTrackingExtended batchTrackingResult = new ResultBatchTrackingExtended();
                batchTrackingResult = DBTransactions.GetBatchTrackingEvents("BatchNumber = \"" + Data.GlovalVariables.currentBatchName.Trim() + "\"");

                if (batchTrackingResult.RecordsCount > 0)
                {
                    ReportButton.Enabled = true;
                    // Fill out the grid
                    foreach (BatchTrackingExtended trackingEvent in batchTrackingResult.ReturnValue)
                    {
                        string[] row = new string[]{ trackingEvent.ID.ToString(), trackingEvent.BatchNumber, trackingEvent.Date.ToString(),
                                                        trackingEvent.InitialStatus, trackingEvent.FinalStatus,
                                                        trackingEvent.OperatorName, trackingEvent.StationName, trackingEvent.Event };
                        BatchEventsList.Rows.Add(row);
                    }
                }
                else
                {
                    ReportButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReportButton_Click(object sender, EventArgs e)
        {
            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(BatchEventsList.ColumnCount);
            pdfTable.DefaultCell.Padding =  6;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            foreach (DataGridViewColumn column in BatchEventsList.Columns)
            {
                if (!string.IsNullOrEmpty(column.HeaderText))
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    pdfTable.AddCell(cell);
                }         
            }

            //Adding DataRow
            foreach (DataGridViewRow row in BatchEventsList.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdfTable.AddCell(cell.Value.ToString());
                }
            }

            //Exporting to PDF
            string folderPath = Application.StartupPath + "\\PDFs\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!ScanningServicesAdmin.General.IsFileLocked(folderPath + "BatchTraceReport.pdf"))
            {
                using (FileStream stream = new FileStream(folderPath + "BatchTraceReport.pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.LEDGER, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
                if (!ScanningServicesAdmin.General.IsFileLocked(folderPath + "BatchTraceReport.pdf"))
                {
                    System.Diagnostics.Process.Start(folderPath + "BatchTraceReport.pdf");
                }
            }
            else
            {
                MessageBox.Show("The BatchTraceReport.pdf is open by another application." + Environment.NewLine +
                                "You need to close the PDF viewer in order to generate the PDF File.", "Message ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }            
        }
    }
}
