using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class MainFunctions : MonoBehaviour {

    private string imagePath;
    private Image image;
    private Texture2D m_Tex;

    public GameObject MainFunction;

    GameObject ScreenShotImage;
    GameObject FunctionView;
    GameObject ShootButton;

    public EmailController Emailer;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {
        ScreenShotImage = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject;
        FunctionView = GlobalManagement.FunctionView;
        ShootButton = GlobalManagement.ShootButton;

        FunctionView.SetActive(false);
        GlobalManagement.ShootButton.SetActive(false);

        StartCoroutine("CaptureScreen");
    }

    IEnumerator CaptureScreen()
    {
        print("save");
        //Application.CaptureScreenshot("Screenshot.png", 0);
        yield return new WaitForSeconds(0.1f);
        CaptureScreenshot2(new Rect(0, 0, Screen.width, Screen.height));

        //yield return new WaitForSeconds(1);

        GlobalManagement.ScreenShot.SetActive(true);
        LoadImage();

        GlobalManagement.MessageBox.SetActive(true);
    }

    void CaptureScreenshot2(Rect rect)
    {
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.persistentDataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);

        //return screenShot;
    }

    private void LoadImage()
    {
        imagePath = Application.persistentDataPath + "/Screenshot.png";
        //imagePath = Application.persistentDataPath + "/tackPhoto/1.jpg";
        Debug.Log(imagePath);
        image = ScreenShotImage.GetComponent<Image>();

        // WWW www = new WWW(imagePath);  
        // yield return www;  
        //byte[] by= www.texture.EncodeToJPG();  
        LoadFromFile(imagePath);
        Sprite tempSprite = new Sprite();
        tempSprite = Sprite.Create(m_Tex, new Rect(0, 0, m_Tex.width, m_Tex.height), new Vector2(0, 0));
        image.sprite = tempSprite;
    }


    private void LoadFromFile(string path)
    {
        m_Tex = new Texture2D(1, 1);
        m_Tex.LoadImage(ReadPNG(path));

    }

    private byte[] ReadPNG(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);

        fileStream.Seek(0, SeekOrigin.Begin);

        byte[] binary = new byte[fileStream.Length];
        fileStream.Read(binary, 0, (int)fileStream.Length);

        fileStream.Close();

        fileStream.Dispose();

        fileStream = null;

        return binary;
    }

    // Send email
    public void SendEmail(string emailAddress)
    {
        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(@Application.persistentDataPath + "/Screenshot.png");
        var thread = new System.Threading.Thread(() => SendEmailWithSubThread(emailAddress, attachment));
        thread.Start();
    }

    public void SendEmailWithSubThread(string emailAddress, System.Net.Mail.Attachment attachment)
    {
        try
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("etcplastar@gmail.com");
            mail.To.Add(emailAddress);
            mail.Subject = "Plastar Photo";
            mail.Body = "This is a gift from Plastar";
            //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(@Application.persistentDataPath + "/Screenshot.png");
            //mail.Attachments.Add(attachment);

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("etcplastar@gmail.com", "husiyuan") as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            smtpServer.Send(mail);
            Debug.Log("success");


            Emailer.SendEmailSuccess();
        }
        catch
        {
            Debug.Log("fail");
            Emailer.SendEmailFail();
        }
    }
}
