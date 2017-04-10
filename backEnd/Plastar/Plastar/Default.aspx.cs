using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

            var si = new ProcessStartInfo();
            si.CreateNoWindow = true;
            si.FileName = Server.MapPath("AssetsBundle") + "\\script.sh";
            si.UseShellExecute = true;
            Process.Start(si);
        }

        private void Submit1_ServerClick(object sender, System.EventArgs e)
        {
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
                string fn = System.IO.Path.GetFileName(File1.PostedFile.FileName);
                string SaveLocation = Server.MapPath("AssetsBundle") + "\\resource\\" + fn;
                try
                {
                    File1.PostedFile.SaveAs(SaveLocation);
                    Response.Write("The file has been uploaded.");
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                    //Note: Exception.Message returns a detailed message that describes the current exception. 
                    //For security reasons, we do not recommend that you return Exception.Message to end users in 
                    //production environments. It would be better to return a generic error message. 
                }
            }
            else
            {
                Response.Write("Please select a file to upload.");
            }
        }
    }
}