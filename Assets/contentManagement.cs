using UnityEngine;
using System.Collections;

public class contentManagement : MonoBehaviour {

    GameObject plan;
    GameObject ScreenShotImage;
    // Use this for initialization
    void Start () {
        plan = GameObject.FindGameObjectWithTag("Content");
        if (plan != null) {
            plan.SetActive(false);
            GlobalManagement.content = plan;
        } else {
            GlobalManagement.content = null;
        }

        ScreenShotImage = GameObject.FindGameObjectWithTag("ScreenShot");
        if (ScreenShotImage != null) {
            ScreenShotImage.SetActive(false);
            GlobalManagement.ScreenShotImage = ScreenShotImage;
        } else {
            GlobalManagement.ScreenShotImage = null;
        }

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void ExitContent() {
        if (GlobalManagement.content && GlobalManagement.content.activeSelf) {
            GlobalManagement.content.SetActive(false);
        }
    }
}
