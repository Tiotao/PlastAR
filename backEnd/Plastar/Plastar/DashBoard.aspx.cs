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
            string con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\TangoProject\\sample\\recognize\\backEnd\\Plastar\\Plastar\\App_Data\\PlastarDB.mdf;Integrated Security=True";
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
                        foreach (DataRow r in t.Rows)
                        {
                            TextBox txt = new TextBox();
                            txt.Width = 100;
                            txt.Height = 100;
                            txt.Text = (r["name"].ToString());
                            holder.Controls.Add(txt);

                            Image img = new Image();
                            img.Width = 100;
                            img.Height = 100;
                            //img.ImageUrl = (r["Snapshot"].ToString());
                            img.ImageUrl = "~\\AssetsBundle\\AssetsBundle\\"+ (r["Snapshot"].ToString());
                            holder.Controls.Add(img);

                            holder.Controls.Add();

                        }

                    }
                }

                
            }
            catch (Exception ex)
            {
            }
        }


    }
}