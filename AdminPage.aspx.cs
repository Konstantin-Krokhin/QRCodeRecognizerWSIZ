using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace QRCodeSample
{
    public partial class AdminPage : System.Web.UI.Page
    {
        private SqlConnection conn = new SqlConnection("Data Source=10.250.12.235\\SQLEXPRESS;Initial Catalog=CHECKER;Persist Security Info=True;User ID=sa;Password=sh23#t4");
        DataSet ds2 = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvbind();
                //this.BindGrid();
            }
        }
        protected void gvbind()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select * from UserRoles", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            SqlCommand cmd2 = new SqlCommand("Select Role from UserRoles", conn);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(ds2);


            DataSet ds = new DataSet();

            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                GridView1.DataSource = ds;
                GridView1.DataBind();
                int columncount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
                GridView1.Rows[0].Cells[0].Text = "No Records Found";
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var t = e.Row.RowType;

            if (t == DataControlRowType.DataRow)
            {
                if (this.GridView1.EditIndex >= 0 && e.Row.RowIndex == this.GridView1.EditIndex)
                {
                    var d = e.Row.FindControl("ddlRoles") as DropDownList;
                    //var em = e.Row.DataItem as Employee;

                    d.DataSource = ds2;
                    d.DataBind();

                    d.SelectedValue = ds2.ToString();
                }
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("lblID");
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete FROM detail where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbind();
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            gvbind();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var userid = GridView1.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("lblID");
            TextBox textName = (TextBox)row.Cells[0].Controls[0];
            GridView1.EditIndex = -1;
            conn.Open();

            var row1 = this.GridView1.Rows[e.RowIndex];
            var c = (DropDownList)row1.FindControl("ddlRoles");
            var textadd = c.SelectedValue;

            SqlCommand cmd = new SqlCommand("update UserRoles set Login='" + textName.Text + "',Role='" + textadd + "'where Login='" + userid + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbind();
  
        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            gvbind();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            gvbind();
        }
    }
}