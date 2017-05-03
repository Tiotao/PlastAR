using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Plastar
{
    public partial class DashBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Response.Redirect("~/");
                AccessData();
            }
            else
            {
                AccessData();
            }
        }

        override protected void OnInit(EventArgs e)
        {
            // 
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            // 
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.Submit1.ServerClick += new System.EventHandler(this.Submit1_ServerClick);
            this.AddBundle.Click += new System.EventHandler(this.AddBundle_Click);
            //this.Load += new System.EventHandler(this.Page_Load);
        }

        private void AddBundle_Click(object sender, System.EventArgs e)
        {
            //AccessData();
            Response.Redirect("~/");
        }


        private void AccessData()
        {
            //TextBox1.Text = hdna.Value.ToString();

            DataTable t = new System.Data.DataTable();
            //string con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\TangoProject\\sample\\recognize\\backEnd\\Plastar\\Plastar\\App_Data\\PlastarDB.mdf;Integrated Security=True";
            string con = "Data Source=128.2.238.5;Initial Catalog=plastar;User ID=plastar;Password=husiyuan";
            try
            {
                using (SqlConnection myCon = new SqlConnection(con))
                {
                    myCon.Open();
                    string sql = "select * from dbo.AssetsBundle";
                    using (SqlCommand cm = new SqlCommand(sql, myCon))
                    {
                        SqlDataAdapter a = new SqlDataAdapter(cm);
                        a.Fill(t);
                        holder.Controls.Add(new LiteralControl("<div class='row cast-list'>"));

                        foreach (DataRow r in t.Rows)
                        {
                            holder.Controls.Add(new LiteralControl("<div class='col-md-4 col-sm-6 holder'><div class='thumbnail'>"));
                            
                            // thumbnail image
                            Image img = new Image();
                            //img.ImageUrl = (r["Snapshot"].ToString());
                            img.ImageUrl = "~\\AssetsBundle\\AssetsBundle\\" + (r["Snapshot"].ToString());
                            holder.Controls.Add(img);
                           

                            // caption
                            holder.Controls.Add(new LiteralControl("<div class='caption'>"));
                            string txt = (r["name"].ToString());
                            holder.Controls.Add(new LiteralControl("<h3>" + txt + "</h3>"));
                            holder.Controls.Add(new LiteralControl("<p>"));
                            Button btn = new Button();
                            btn.CssClass = "btn btn-danger";
                            btn.Text = "delete";
                            btn.CommandArgument = (r["Snapshot"].ToString());
                            btn.Click += new System.EventHandler(this.ButtonClicked);
                            holder.Controls.Add(btn);
                            holder.Controls.Add(new LiteralControl("</p>"));
                            holder.Controls.Add(new LiteralControl("</div>"));

                            holder.Controls.Add(new LiteralControl("</div>"));
                            holder.Controls.Add(new LiteralControl("</div>"));
                            // holder.Controls.Add(new LiteralControl("<br />"));
                        }
                        holder.Controls.Add(new LiteralControl("</div>"));

                    }
                    myCon.Close();
                }

                
            }
            catch (Exception ex)
            {
            }
        }

        protected void ButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            string snapshot = button.CommandArgument;

            string con = "Data Source=128.2.238.5;Initial Catalog=plastar;User ID=plastar;Password=husiyuan";
            SqlConnection myCon = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("delete from dbo.AssetsBundle where Snapshot = @snapshot", myCon);
            cmd.Parameters.AddWithValue(@"snapshot", snapshot);

            myCon.Open();
            cmd.ExecuteNonQuery();
            myCon.Close();

            Response.Redirect(Request.RawUrl);

            //DeleteFileFromFolder("131370224948511349");

            // need to delete the file physically
        }

        public void DeleteFileFromFolder(string StrFilename)
        {
            string strPhysicalFolder = "C:\\TangoProject\\sample\\recognize\\backEnd\\Plastar\\Plastar\\AssetsBundle\\AssetsBundle\\";


            //string strPhysicalFolder = "~\\AssetsBundle\\AssetsBundle\\";

            string strFileFullPath = strPhysicalFolder + StrFilename;
            //System.IO.File.Delete(strFileFullPath);
            if (System.IO.File.Exists(strFileFullPath))
            {
                test("1");
                System.IO.File.Delete(strFileFullPath);
            }
            else
                test(strFileFullPath);

        }


        private void test(string str)
        {
            Response.Write("<script>alert('" + str + "')</script>");
        }


    }
}