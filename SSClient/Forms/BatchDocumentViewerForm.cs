using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ScanningServicesDataObjects.GlobalVars;

namespace ScanningServicesAdmin.Forms
{
    public partial class BatchDocumentViewerForm : Form
    {
        public int currentDocID;
        public List<BatchDocs> batchDocs = new List<BatchDocs>();
        private static NLog.Logger nlogger = LogManager.GetCurrentClassLogger();

        private int intCurrPage = 0; // defining the current page (its some sort of a counter)
        bool tiffOpened = false; // if an image was tiffOpened
        public string currentImagePage;
        public string totalImagePages;
        public string imageFileName;

        public BatchDocumentViewerForm()
        {
            InitializeComponent();
        }

        private void PopulateMetadata(BatchDocs doc)
        {
            Metadata metadata = new Metadata();
            ListViewItem listViewItem;
            try
            {
                if (!string.IsNullOrEmpty(doc.CustonmerField1))
                {
                    metadata = BuildMetadata(doc.CustonmerField1);
                    string[] row1 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row1);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField2))
                {
                    metadata = BuildMetadata(doc.CustonmerField2);
                    string[] row2 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row2);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField3))
                {
                    metadata = BuildMetadata(doc.CustonmerField3);
                    string[] row3 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row3);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField4))
                {
                    metadata = BuildMetadata(doc.CustonmerField4);
                    string[] row4 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row4);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField5))
                {
                    metadata = BuildMetadata(doc.CustonmerField5);
                    string[] row5 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row5);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField6))
                {
                    metadata = BuildMetadata(doc.CustonmerField6);
                    string[] row6 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row6);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField7))
                {
                    metadata = BuildMetadata(doc.CustonmerField7);
                    string[] row7 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row7);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField8))
                {
                    metadata = BuildMetadata(doc.CustonmerField8);
                    string[] row8 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row8);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField9))
                {
                    metadata = BuildMetadata(doc.CustonmerField9);
                    string[] row9 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row9);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField10))
                {
                    metadata = BuildMetadata(doc.CustonmerField10);
                    string[] row10 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row10);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField11))
                {
                    metadata = BuildMetadata(doc.CustonmerField11);
                    string[] row11 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row11);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField12))
                {
                    metadata = BuildMetadata(doc.CustonmerField12);
                    string[] row12 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row12);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField13))
                {
                    metadata = BuildMetadata(doc.CustonmerField13);
                    string[] row13 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row13);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField14))
                {
                    metadata = BuildMetadata(doc.CustonmerField14);
                    string[] row14 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row14);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                if (!string.IsNullOrEmpty(doc.CustonmerField15))
                {
                    metadata = BuildMetadata(doc.CustonmerField15);
                    string[] row15 = { metadata.Scope.ToString(), metadata.FieldName, metadata.FieldValue };
                    listViewItem = new ListViewItem(row15);
                    if (metadata.Scope.ToString().ToUpper() == "DOCUMENT")
                    {
                        DocsMetadataListView.Items.Add(listViewItem);
                    }
                    else
                    {
                        BatchMetadataListView.Items.Add(listViewItem);
                    }
                }

                Cursor.Current = Cursors.WaitCursor;
                nlogger.Trace("Launching Document: " + doc.DocumentFileNameWithFullPath);
                if (!File.Exists(doc.DocumentFileNameWithFullPath))
                {
                    MessageBox.Show("Warning:" + "\r\n" + "Cannot find document: " + doc.DocumentFileNameWithFullPath, "Document Viewer ...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (Path.GetExtension(doc.DocumentFileNameWithFullPath).ToUpper() == ".PDF")
                    {
                        PDFFile.LoadFile(doc.DocumentFileNameWithFullPath);
                        ImageControlGroupBox.Visible = false;
                        TIFFPanel.Visible = false;
                        PDFPanel.Visible = true;
                    }
                    else
                    {
                        imageFileName = doc.DocumentFileNameWithFullPath;
                        ImageControlGroupBox.Visible = true;
                        TIFFPanel.Visible = true;
                        PDFPanel.Visible = false;
                        RefreshImage();
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void BatchDocumentViewerForm_Load(object sender, EventArgs e)
        {
            try
            {
                // DocsMetadataListView.LabelEdit = true;
                ResultBatchDocs resultBatchDocs = new ResultBatchDocs();
                BatchNameLabel.Text = Data.GlovalVariables.currentBatchName;
                JobTypeLabel.Text = Data.GlovalVariables.currentJobName;
                resultBatchDocs = DBTransactions.GetBatchDocuments("BatchName=\"" + Data.GlovalVariables.currentBatchName + "\"");
                batchDocs = resultBatchDocs.ReturnValue;
                //Data.GlovalVariables.currentBatchName = BatchList.CurrentRow.Cells["BatchNumber"].Value.ToString();

                PDFPanel.Dock = DockStyle.Fill;
                TIFFPanel.Dock = DockStyle.Fill;
                ImageControlGroupBox.Visible = false;
                TIFFPanel.Visible = false;
                PDFPanel.Visible = false;


                if (batchDocs.Count != 0)
                {
                    // Sort Docs by Document ID
                    batchDocs = batchDocs.OrderBy(s => Convert.ToInt32(s.DocumentID)).ToList();
                }

                foreach (BatchDocs doc in batchDocs)
                {
                    currentDocID = doc.DocumentID;
                    DocNumberTextBox.Text = currentDocID.ToString() + " / " + batchDocs.Count().ToString();
                    PopulateMetadata(doc);
                    break; // Load one the first Documents of the List
                }
            }
            catch (Exception ex)
            {
                nlogger.Fatal(General.ErrorMessage(ex));
                MessageBox.Show(General.ErrorMessage(ex), "Error ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInformation"></param>
        /// <returns></returns>
        static public Metadata BuildMetadata(string fieldInformation)
        {
            var tokens = fieldInformation.Split('|');
            Metadata metadata = new Metadata();
            metadata.FieldName = tokens[0];
            metadata.FieldValue = tokens[2];
            if (tokens[1] == "D")
                metadata.Scope = MetadataScope.Document;
            else
                metadata.Scope = MetadataScope.Batch;
            return metadata;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            foreach (BatchDocs doc in batchDocs)
            {
                if (doc.DocumentID == currentDocID + 1)
                {
                    DocsMetadataListView.Items.Clear();
                    BatchMetadataListView.Items.Clear();
                    currentDocID = doc.DocumentID;
                    DocNumberTextBox.Text = currentDocID.ToString() + " / " + batchDocs.Count().ToString();
                    PopulateMetadata(doc);
                    break;
                }
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            foreach (BatchDocs doc in batchDocs)
            {
                if (doc.DocumentID == currentDocID - 1)
                {
                    DocsMetadataListView.Items.Clear();
                    BatchMetadataListView.Items.Clear();
                    currentDocID = doc.DocumentID;
                    DocNumberTextBox.Text = currentDocID.ToString() + " / " + batchDocs.Count().ToString();
                    PopulateMetadata(doc);
                    break;
                }
            }
        }

        private void LastButton_Click(object sender, EventArgs e)
        {

            BatchDocs doc = batchDocs[batchDocs.Count -1];     
            DocsMetadataListView.Items.Clear();
            BatchMetadataListView.Items.Clear();
            currentDocID = doc.DocumentID;
            DocNumberTextBox.Text = currentDocID.ToString() + " / " + batchDocs.Count().ToString();
            PopulateMetadata(doc);
        }

        private void FirstButton_Click(object sender, EventArgs e)
        {
            BatchDocs doc = batchDocs[0];
            DocsMetadataListView.Items.Clear();
            BatchMetadataListView.Items.Clear();
            currentDocID = doc.DocumentID;
            DocNumberTextBox.Text = currentDocID.ToString() + " / " + batchDocs.Count().ToString();
            PopulateMetadata(doc);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrviousPageButton_Click(object sender, EventArgs e)
        {
            //if (tiffOpened) // the button works if the file is tiffOpened. you could go with button.enabled
            //{
                if (intCurrPage == 0) // it stops here if you reached the bottom, the first page of the tiff
                { intCurrPage = 0; }
                else
                {
                    intCurrPage--; // if its not the first page, then go to the previous page
                    RefreshImage(); // refresh the image on the selected page
                }
            //}
        }

        private void NextPageButton_Click(object sender, EventArgs e)
        {
            //if (tiffOpened) // the button works if the file is tiffOpened. you could go with button.enabled
            //{
                if (intCurrPage == Convert.ToInt32(totalImagePages)) // if you have reached the last page it ends here
                                                                      // the "-1" should be there for normalizing the number of pages
                { intCurrPage = Convert.ToInt32(totalImagePages); }
                else
                {
                    intCurrPage++;
                    RefreshImage();
                }
            //}
        }

        public void RefreshImage()
        {
            Image myImg; // setting the selected tiff
            Image myBmp; // a new occurance of Image for viewing

            myImg = System.Drawing.Image.FromFile(imageFileName); // setting the image from a file

            int intPages = myImg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page); // getting the number of pages of this tiff
            intPages--; // the first page is 0 so we must correct the number of pages to -1
            totalImagePages = Convert.ToString(intPages); // showing the number of pages
            currentImagePage = Convert.ToString(intCurrPage); // showing the number of page on which we're on
            ImagePageTextBox.Text = Convert.ToString(intCurrPage + 1) + " / " + Convert.ToString(intPages + 1);
            myImg.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, intCurrPage); // going to the selected page
            myBmp = FixedSize(myImg, TIFFPanel.Width, TIFFPanel.Height); // setting the new page as an image

            PictureBox.Image = myBmp; // showing the page in the pictureBox1

        }

        private void BatchDocumentViewerForm_Resize(object sender, EventArgs e)
        {
            if (TIFFPanel.Visible)
            {
                RefreshImage();
            }
        }

        static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.LightGray);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private void LastImagePageButton_Click(object sender, EventArgs e)
        {
            intCurrPage = Convert.ToInt32(totalImagePages);
            RefreshImage();
        }

        private void FirstImagePageButton_Click(object sender, EventArgs e)
        {
            intCurrPage = 0;
            RefreshImage();
        }
    }
}
