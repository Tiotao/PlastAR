using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class HotSpotClick : MonoBehaviour {


    GameObject plan;
    private bool flag = false;
    private bool looseFinger = true;

    private string imagePath;
    private Image image;
    private Texture2D m_Tex;

    GameObject ScreenShotImage;

    // Use this for initialization
    void Start()
    {
        plan = GlobalManagement.content;
        ScreenShotImage = GlobalManagement.ScreenShotImage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && looseFinger)
        //if (Input.touchCount > 0 && looseFinger)// && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            //hit用来存储碰撞物体的信息
            RaycastHit hit;
            //ray表示射线，hit存储物体的信息,1000为设定射线发射的距离
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    plan.SetActive(!flag);
                    flag = !flag;
                    looseFinger = false;
                }
            }
        }

        if (Input.touchCount == 0)
            looseFinger = true;
    }

}
