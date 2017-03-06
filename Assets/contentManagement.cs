using UnityEngine;
using System.Collections;

public class contentManagement : MonoBehaviour {

    GameObject plan;
    GameObject ScreenShotImage;
    // Use this for initialization
    void Start () {
        plan = GameObject.FindGameObjectWithTag("Content");
        plan.SetActive(false);
        GlobalManagement.content = plan;

        ScreenShotImage = GameObject.FindGameObjectWithTag("ScreenShot");
        ScreenShotImage.SetActive(false);
        GlobalManagement.ScreenShotImage = ScreenShotImage;
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
