using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;


public class test : MonoBehaviour
{

    private string imagePath;
    private Image image;
    private Texture2D m_Tex;


    void Start()
    {
        LoadImage();
    }

    private void LoadImage()
    {
        imagePath = "./Screenshot.png";
        //imagePath = Application.persistentDataPath + "/tackPhoto/1.jpg";
        Debug.Log(imagePath);
        image = this.gameObject.GetComponent<Image>();

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

        byte[] binary = new byte[fileStream.Length]; //创建文件长度的buffer   
        fileStream.Read(binary, 0, (int)fileStream.Length);

        fileStream.Close();

        fileStream.Dispose();

        fileStream = null;

        return binary;
    }


}