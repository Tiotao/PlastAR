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
        // CallExampleNotification();
        InitializeScreen("HomeView");
        InitializeScreen("ScreenShot");
        InitializeScreen("RotateView");
        InitializeScreen("FunctionView");
        InitializeScreen("ShootButton");
        InitializeScreen("MessageBox");
        InitializeScreen("EmailBox");
        InitializeScreen("StoryView");
        InitializeScreen("GuidingLine");
        InitializeScreen("BuildingAppearFX");
        InitializeScreen("OnBoardingView");
        InitializeScreen("MapView");
        InitializeScreen("GuideView");
        InitializeScreen("BuildingView");
        InitializeScreen("NavigationView");
        // LoadData();



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
            Debug.Log("[" + screenTag.ToUpper() + "] initialized.");
        } else {
            Debug.Log("[" + screenTag.ToUpper() + "] not found.");
        }
        
        return null;
    }

    // Update is called once per frame
    void Update () {
	
	}

    
   
}
