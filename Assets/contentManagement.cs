using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using Assets.SimpleAndroidNotifications;

public class contentManagement : MonoBehaviour {

    GameObject plan;
    GameObject ScreenShotImage;
    GameObject RotateView;
    GameObject HotSpotDes;
    GameObject FunctionView;
    GameObject ShootButton;
    
    // Use this for initialization
    void Start () {
        CallExampleNotification();
        InitializeScreen("Content");
        InitializeScreen("ScreenShot");
        InitializeScreen("RotateView");
        InitializeScreen("HotSpotDes");
        InitializeScreen("FunctionView");
        InitializeScreen("ShootButton");
        InitializeScreen("MessageBox");
        InitializeScreen("EmailBox");
        InitializeScreen("StoryView");
        LoadData();
    }

    void LoadData() {
        AssetsLoader assetsLoader = GameObject.Find("AssetsLoader").GetComponent<AssetsLoader>();
        assetsLoader.Load();
    }

    
    GameObject InitializeScreen(string screenTag) {
        GameObject returnObject = GameObject.FindGameObjectWithTag(screenTag);
        if (returnObject != null) {
            returnObject.SetActive(false);
            typeof(GlobalManagement).GetField(screenTag).SetValue(null, returnObject);
        }
        return null;
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

    
}
