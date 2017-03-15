using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotspot3D : MonoBehaviour {

	public string _name;
	public string _description;

	GameObject HotSpotDes;
    private bool flag = false;
    private bool looseFinger = true;

    GameObject ScreenShotImage;

    // Use this for initialization
    void Start()
    {
        HotSpotDes = GlobalManagement.HotSpotDes;
        ScreenShotImage = GlobalManagement.ScreenShot;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && looseFinger)
        //if (Input.touchCount > 0 && looseFinger)// && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    HotSpotDes.transform.GetChild(0).GetComponent<Text>().text = _name;
                    HotSpotDes.SetActive(!flag);
                    flag = !flag;
                    looseFinger = false;
                }
            }
        }

        if (Input.touchCount == 0)
            looseFinger = true;
    }
}
