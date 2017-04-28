using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using Ionic.Zip;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Plastar
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        override protected void OnInit(EventArgs e)
        {
            // 
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            // 
            ViewState["uploaded"] = false;
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Submit1.ServerClick += new System.EventHandler(this.Submit1_ServerClick);
            this.Build1.Click += new System.EventHandler(this.Build1_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }

        private void Build1_Click(object sender, System.EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
            //ProcessStartInfo psi = new ProcessStartInfo();
            //psi.FileName = Server.MapPath("AssetsBundle") + "\\script.sh";
            //psi.UseShellExecute = true;
            //psi.RedirectStandardOutput = true;

            //psi.Arguments = "test";
            //Process p = Process.Start(psi);
            //string strOutput = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();
            //Console.WriteLine(strOutput);

            //Console.WriteLine("hehe");

            if (!(bool)ViewState["uploaded"])
            {
                Response.Write("<script>alert('Upload your cast first!')</script>");
                return;
            }

            string argument = DateTime.Now.ToFileTime().ToString();

            var si = new ProcessStartInfo();
            si.CreateNoWindow = true;
            si.Arguments = argument;
            si.FileName = Server.MapPath("AssetsBundle") + "\\script.sh";
            si.UseShellExecute = true;
            var process = new Process { StartInfo = si };
            process.Start();

            Thread.Sleep(20000);
            //process.WaitForExit(10000);
            //process.Exited += new EventHandler(captureExit);
            //process.WaitForExit();

            string name = "husiyuan";
            string path = Server.MapPath("AssetsBundle") + "\\AssetsBundle\\" + argument;
            string snapshot = argument + "\\000.png";

            string con = "Data Source=128.2.238.5;Initial Catalog=plastar;User ID=plastar;Password=husiyuan";
            SqlConnection myCon = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("insert into dbo.AssetsBundle(Name,Path,Snapshot) values (@name,@path,@snapshot)", myCon);
            cmd.Parameters.AddWithValue(@"name", ViewState["name"].ToString());
            cmd.Parameters.AddWithValue(@"path", path);
            cmd.Parameters.AddWithValue(@"snapshot", snapshot);

            myCon.Open();
            cmd.ExecuteNonQuery();
            myCon.Close();

            ViewState["uploaded"] = false;
            //Response.Write("<script>alert('"+"done!!"+"')</script>");
        }

        private void captureExit(object sender, EventArgs e)
        {
            Response.Write("<script>alert('" + "done!!!" + "')</script>");

        }

        private void Submit1_ServerClick(object sender, System.EventArgs e)
        {
            //if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            //{
            //    string fn = System.IO.Path.GetFileName(File1.PostedFile.FileName);
            //    string SaveLocation = Server.MapPath("AssetsBundle") + "\\resource\\" + fn;
            //    try
            //    {
            //        File1.PostedFile.SaveAs(SaveLocation);
            //        Response.Write("The file has been uploaded.");
            //    }
            //    catch (Exception ex)
            //    {
            //        Response.Write("Error: " + ex.Message);
            //        //Note: Exception.Message returns a detailed message that describes the current exception. 
            //        //For security reasons, we do not recommend that you return Exception.Message to end users in 
            //        //production environments. It would be better to return a generic error message. 
            //    }
            //}
            //else
            //{
            //    Response.Write("Please select a file to upload.");
            //}

            string extractPath = Server.MapPath("AssetsBundle") + "\\resource\\";
            if (File1.PostedFile.FileName != "cast.zip")
            {
                Response.Write("<script>alert('File structure not valid!')</script>");
                return;
            }
            using (ZipFile zip = ZipFile.Read(File1.PostedFile.InputStream))
            {
                //Response.Write("<script>alert('"+zip.EntryFileNames.ElementAt(0)+"')</script>");
                for (int i = 0; i < zip.EntryFileNames.Count; i++)
                {
                    //Response.Write("<script>alert('" + zip.EntryFileNames.ElementAt(i) + "')</script>");
                    if (zip.EntryFileNames.ElementAt(i).Substring(0, 4) != "cast")
                    {
                        Response.Write("<script>alert('File structure not valid!')</script>");
                        return;
                    }
                    if (zip.EntryFileNames.ElementAt(i).Substring(0, 10) != "cast/model")
                    {
                        string str = zip.EntryFileNames.ElementAt(i).Substring(5);
                        str = str.Substring(0, str.Length - 1);
                        ViewState["name"] = str;
                    }
                }
                zip.ExtractAll(extractPath, ExtractExistingFileAction.DoNotOverwrite);
                ViewState["uploaded"] = true;
            }
        }
    }
}