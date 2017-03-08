using UnityEngine;
using System.Collections;

public class contentManagement : MonoBehaviour {

    GameObject plan;
    GameObject ScreenShotImage;
    GameObject RotateView;
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

        RotateView = GameObject.FindGameObjectWithTag("RotateView");
        if (RotateView != null)
        {
            RotateView.SetActive(false);
            GlobalManagement.RotateView = RotateView;
        }
        else
        {
            GlobalManagement.RotateView = null;
        }

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void ExitContent() {
        if (GlobalManagement.RotateView && GlobalManagement.RotateView.activeSelf) {
            GlobalManagement.RotateView.SetActive(false);
        }

        // currently in building scene
        if (GlobalManagement.SceneIndex == 2)
        {
            Destroy(GlobalManagement.Building);
            GlobalManagement.Marker.SetActive(true);
        }

        GlobalManagement.SceneIndex = 0;
    }
    
}
