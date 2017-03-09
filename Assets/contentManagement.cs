using UnityEngine;
using System.Collections;
using System;
using Assets.SimpleAndroidNotifications;

public class contentManagement : MonoBehaviour {

    GameObject plan;
    GameObject ScreenShotImage;
    GameObject RotateView;
    GameObject HotSpotDes;
    // Use this for initialization
    void Start () {

        CallExampleNotification();

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

        HotSpotDes = GameObject.FindGameObjectWithTag("HotSpotDes");
        if (HotSpotDes != null)
        {
            HotSpotDes.SetActive(false);
            GlobalManagement.HotSpotDes = HotSpotDes;
        }
        else
        {
            GlobalManagement.HotSpotDes = null;
        }

    }

    // Update is called once per frame
    void Update () {
	
	}

    
    void CallExampleNotification() {
        var notificationParams = new NotificationParams
                {
                    Id = UnityEngine.Random.Range(0, int.MaxValue),
                    Delay = TimeSpan.FromSeconds(5),
                    Title = "Hello World",
                    Message = "Message",
                    Ticker = "Ticker",
                    Sound = true,
                    Vibrate = true,
                    Light = true,
                    SmallIcon = NotificationIcon.Heart,
                    SmallIconColor = new Color(0, 0.5f, 0),
                    LargeIcon = "app_icon"
                };

                NotificationManager.SendCustom(notificationParams);
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
            GlobalManagement.HotSpotDes.SetActive(false);
        }

        GlobalManagement.SceneIndex = 0;
    }
    
}
