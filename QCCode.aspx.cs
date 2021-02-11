using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data.SqlClient;
using ZXing;
using System.Collections;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using DocumentFormat.OpenXml;
using System.Windows;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Windows.Documents;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using System.Data;
using System.Web.UI.WebControls;

namespace QRCodeSample
{
    public class Employee
    {
        public Decimal id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }

    public partial class QCCode : System.Web.UI.Page
    {
        public string conString = "Data Source=10.250.12.235\\SQLEXPRESS;Initial Catalog=CHECKER;Persist Security Info=True;User ID=sa;Password=sh23#t4";
        public List<Employee> allRecords = new List<Employee>();
        public Employee[] allRecords_pre = null;
        public List<int> employeeNum = new List<int>();
        public void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void GenerateMyQCCode()
        {
            CleanFile();
            AddTemplate();
            SelectedEmployees();
            allRecordsLoad();
            SelectedEmployeesExtract();
            for (int record = 0; record < allRecords.Count(); record++)
            {
                WordprocessingDocument wordDoc = WordprocessingDocument.Open(@"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QRCodeSample\QR.docx", true);
                string QCText = "PRAC-" + allRecords[record].id;
                var QCwriter = new BarcodeWriter();
                QCwriter.Format = BarcodeFormat.QR_CODE;
                var result = QCwriter.Write(QCText);
                string path = Server.MapPath("~/images/MyQRImage.jpg");
                var barcodeBitmap = new Bitmap(result);

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(path,
                       FileMode.Create, FileAccess.ReadWrite))
                    {
                        barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }

                ReplaceMergeFieldWithImage(wordDoc, "qrcode", @"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QRCodeSample\images\MyQRImage.jpg");
                ReplaceMergeFieldWithText(wordDoc, "name", allRecords[record].name);
                ReplaceMergeFieldWithText(wordDoc, "surname", allRecords[record].surname);

                wordDoc.MainDocumentPart.Document.Save();
                wordDoc.Close();

                if (record != allRecords.Count() - 1)
                {
                    AddTemplate();
                }

            }

            // Delete annoying Spire trial warning text
            using (WordprocessingDocument doc = WordprocessingDocument.Open(@"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QRCodeSample\QR.docx", true))
            {
                foreach (Text element in doc.MainDocumentPart.Document.Body.Descendants<Text>())
                {
                    if (element.Text == "Evaluation Warning: The document was created with Spire.Doc for .NET.") element.Parent.Remove();
                    if (element.Text == BreakType.LineBreak.ToString()) element.Parent.Remove();
                }
            }
            MessageBox.Show("Dane zapisane do pliku QR.docx!");
        }

        public void SelectedEmployeesExtract()
        {
            int j = 0;
            for (int i = 0; i < allRecords_pre.Count(); i++)
            {
                if (j < employeeNum.Count())
                {
                    if (i == employeeNum[j])
                    {
                        allRecords.Add(allRecords_pre[i]);
                        j++;
                    }
                }
                
            }
        }

        public void SelectedEmployees()
        {
            if (GridView.Rows.Count > 0)
            {
                foreach (GridViewRow row in GridView.Rows)
                {
                    //find checkbox in GridView
                    var checkbox = row.FindControl("CheckBox1") as System.Web.UI.WebControls.CheckBox;
                    //CheckBox not null
                    if (checkbox != null)
                    {
                        //if CheckBox Checked
                        if (checkbox.Checked)
                        {
                            employeeNum.Add(row.RowIndex);
                        }
                    }
                }
            }
        }

        public void CleanFile()
        {
            //Instantiate a Document object
            Spire.Doc.Document document = new Spire.Doc.Document();
            //Load the Word document
            document.LoadFromFile(@"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QRCodeSample\QR.docx");

            //Remove paragraphs from every section in the document
            foreach (Spire.Doc.Section section in document.Sections)
            {
                section.Paragraphs.Clear();
            }

            //Save the document
            document.SaveToFile(@"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QRCodeSample\QR.docx", FileFormat.Docx2013);
        }

        //Copies mail merge template into .docx and adds page break
        public void AddTemplate()
        {
            Spire.Doc.Document sourceDoc = new Spire.Doc.Document(@"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QR.docx");
            Spire.Doc.Document destinationDoc = new Spire.Doc.Document(@"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QRCodeSample\QR.docx");
            foreach (Spire.Doc.Section sec in sourceDoc.Sections)
            {
                foreach (DocumentObject obj in sec.Body.ChildObjects)
                {
                    destinationDoc.Sections[0].Body.ChildObjects.Add(obj.Clone());

                    Spire.Doc.Documents.Paragraph newParagraph = new Spire.Doc.Documents.Paragraph(destinationDoc);
                    newParagraph.AppendBreak(BreakType.PageBreak);
                    destinationDoc.LastSection.Paragraphs.Add(newParagraph);
                }
            }
            destinationDoc.SaveToFile(@"C:\Users\Konstantin\Desktop\WSIiZ\QRCodeSample\QRCodeSample\QR.docx", FileFormat.Docx2013);
        }

        static void ReplaceMergeFieldWithText(WordprocessingDocument wordDoc, string key, string value)
        {
            string FieldDelimeter = " MERGEFIELD ";

            foreach (FieldCode field in wordDoc.MainDocumentPart.RootElement.Descendants<FieldCode>())
            {
                var fieldNameStart = field.Text.LastIndexOf(FieldDelimeter, System.StringComparison.Ordinal);
                if (field.Text.Length <= fieldNameStart + FieldDelimeter.Length)
                {
                    continue;
                }

                var fieldName = field.Text.Substring(fieldNameStart + FieldDelimeter.Length).Trim();

                foreach (Run run in wordDoc.MainDocumentPart.Document.Descendants<Run>())
                {
                    foreach (Text txtFromRun in run.Descendants<Text>().Where(a => a.Text == "«" + key.Trim() + "»"))
                    {
                        txtFromRun.Text = (value ?? "").Replace("\r\n", "###LB###");
                    }
                }
            }
        }

        static void ReplaceMergeFieldWithImage(WordprocessingDocument wordDoc, string key, string filePath)
        {
            if(File.Exists(filePath) == false)
            {
                return;
            }

            string FieldDelimeter = " MERGEFIELD ";

            foreach (FieldCode field in wordDoc.MainDocumentPart.RootElement.Descendants<FieldCode>())
            {
                var fieldNameStart = field.Text.LastIndexOf(FieldDelimeter, System.StringComparison.Ordinal);
                if (field.Text.Length <= fieldNameStart + FieldDelimeter.Length)
                {
                    continue;
                }

                var fieldName = field.Text.Substring(fieldNameStart + FieldDelimeter.Length).Trim();

                long iWidth = 0;
                long iHeight = 0;
                using (var bmp = new System.Drawing.Bitmap(filePath))
                {
                    iWidth = (long)bmp.Width * 9525L;
                    iHeight = (long)bmp.Height * 9525L;
                }

                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    foreach (Run run in wordDoc.MainDocumentPart.Document.Descendants<Run>())
                    {
                        foreach (Text txtFromRun in run.Descendants<Text>().Where(a => a.Text == "«" + key.Trim() + "»"))
                        {
                            ImagePart imagePart = wordDoc.MainDocumentPart.AddImagePart(ImagePartType.Png);
                            imagePart.FeedData(stream);

                            var drawing = AddImageToBody(wordDoc.MainDocumentPart.GetIdOfPart(imagePart), iWidth, iHeight);
                            if (drawing != null)
                            {
                                txtFromRun.Parent.InsertAfter<Drawing>(drawing, txtFromRun);
                                txtFromRun.Text = String.Empty;
                            }
                        }
                    }
                }
            }
        }

        private static Drawing AddImageToBody(string relationshipId, long width, long height)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = width, Cy = height },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new DocumentFormat.OpenXml.Drawing.GraphicFrameLocks() { NoChangeAspect = true }),
                         new DocumentFormat.OpenXml.Drawing.Graphic(
                             new DocumentFormat.OpenXml.Drawing.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new DocumentFormat.OpenXml.Drawing.Blip(
                                             new DocumentFormat.OpenXml.Drawing.BlipExtensionList(
                                                 new DocumentFormat.OpenXml.Drawing.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print
                                         },
                                         new DocumentFormat.OpenXml.Drawing.Stretch(
                                             new DocumentFormat.OpenXml.Drawing.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new DocumentFormat.OpenXml.Drawing.Transform2D(
                                             new DocumentFormat.OpenXml.Drawing.Offset() { X = 0L, Y = 0L },
                                             new DocumentFormat.OpenXml.Drawing.Extents() { Cx = width, Cy = height }),
                                         new DocumentFormat.OpenXml.Drawing.PresetGeometry(
                                             new DocumentFormat.OpenXml.Drawing.AdjustValueList()
                                         )
                                         { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            return element;
        }

        protected void btnQCGenerate_Click(object sender, EventArgs e)
        {
            GenerateMyQCCode();
        }

        protected void btnload_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT id, surname, name
               FROM Employees";
            SqlConnection con = new SqlConnection(conString);
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    GridView.DataSource = dt;
                    GridView.DataBind();
                }
            }
            con.Close();
        }

        public void allRecordsLoad()
        {
            string sql = @"SELECT id, surname, name
               FROM Employees";
            SqlConnection con = new SqlConnection(conString);
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<Employee>();
                    while (reader.Read())
                        list.Add(new Employee
                        {
                            id = reader.GetDecimal(0),
                            surname = reader.GetString(1),
                            name = reader.GetString(2)
                        });
                    allRecords_pre = list.ToArray();
                }
            }
            con.Close();
        }
    }
}