using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Spire.Doc;
using Spire.Doc.Documents;
using Syncfusion.XlsIO;

namespace QRCodeSample
{
    public partial class UserPage : System.Web.UI.Page
    {
        public string conString = "Data Source=10.250.12.235\\SQLEXPRESS;Initial Catalog=CHECKER;Persist Security Info=True;User ID=sa;Password=sh23#t4";
        public string[] allRecords = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand("select * from EmployeeEntries"))
                {
                    SqlDataAdapter dt = new SqlDataAdapter();
                    try
                    {
                        cmd.Connection = con;
                        con.Open();
                        dt.SelectCommand = cmd;

                        DataTable dTable = new DataTable();
                        dt.Fill(dTable);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        GridView2.DataSource = dTable;
                        GridView2.DataBind();
                    }
                    catch (Exception)
                    {
                         
                    }
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        protected void btnQCGenerate_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "Lista_Obecnosci" + DateTime.Now.Date + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Lista_Obecnosci", "attachment;filename=" + FileName);
            GridView2.GridLines = GridLines.Both;
            GridView2.HeaderStyle.Font.Bold = true;
            GridView2.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
    }
}