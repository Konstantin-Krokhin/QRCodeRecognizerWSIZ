using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace QRCodeSample
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public class Role
        {
            public string login { get; set; }
            public string role { get; set; }
        }

        public string conString = "Data Source=10.250.12.235\\SQLEXPRESS;Initial Catalog=CHECKER;Persist Security Info=True;User ID=sa;Password=sh23#t4";
        public List<Role> allRoles = new List<Role>();
        public string role = "";

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            allRecordsLoad();
                    if (ModelState.IsValid)
                    {
                        DirectoryEntry de = null;
                        de = new DirectoryEntry(
                                $"LDAP://{ConfigurationManager.AppSettings["ldap.host.teacher"].ToString()}/{ConfigurationManager.AppSettings["ldap.path.teacher"].ToString()}", TextBox1.Text, TextBox2.Text);

                        try
                        {
                            SearchResult sr = SearchAD(de, TextBox1.Text);
                            if (sr != null)
                            {
                                System.DirectoryServices.DirectoryEntry sde = sr.GetDirectoryEntry();
                                if (sde.Properties["sAMAccountName"].Value != null)
                                {
                                    try
                                    {

                                        findRole();

                                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                                if (role == "admin")
                                {
                                    sb.Append(@"<script language='javascript'>");
                                    sb.Append(@"loadAdmin();");
                                    sb.Append(@"</script>");
                                }

                                if (role == "print")
                                {
                                    sb.Append(@"<script language='javascript'>");
                                    sb.Append(@"loadPrint();");
                                    sb.Append(@"</script>");
                                }

                                if (role == "user")
                                {
                                    sb.Append(@"<script language='javascript'>");
                                    sb.Append(@"loadUser();");
                                    sb.Append(@"</script>");
                                }

                                        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);
                                    }
                                    catch (Exception x)
                                    {

                                        sde.Invoke("ChangePassword", new object[] { TextBox2.Text, TextBox2.Text });
                                        sde.CommitChanges();
                                        ModelState.AddModelError("PasswordNew", "Hasło nie zostało zmienione! Podane hasło nie spełnia ustawionej polityki zabezpieczeń (powinno składać się z co najmniej 7 znaków, w tym przynajmniej 2 cyfr)");
                                    }
                                }
                            }
                            else
                            {
                                   MessageBox.Show("Not Found!");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Wrong credentials!");
                        }
                    }

        }
        private SearchResult SearchAD(DirectoryEntry de, string login)
        {
            DirectorySearcher ds = new DirectorySearcher(de);

            ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            ds.Filter = String.Format(ConfigurationManager.AppSettings["ldap.filter"].ToString(), login);
            ds.PropertiesToLoad.Add("samaccountname");

            return ds.FindOne();
        }

        public void findRole()
        {
            for (int i = 0; i < allRoles.Count(); i++)
            {
                if (TextBox1.Text == allRoles[i].login) role = allRoles[i].role;
            }
        }

        public void allRecordsLoad()
        {
            string sql = @"SELECT Login, Role
               FROM UserRoles";
            SqlConnection con = new SqlConnection(conString);
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<Role>();
                    while (reader.Read())
                        list.Add(new Role
                        {
                            login = reader.GetString(0),
                            role = reader.GetString(1)
                        });
                    allRoles = list;
                }
            }
            con.Close();
        }
    }
}