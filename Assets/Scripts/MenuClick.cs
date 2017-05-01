﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class MenuClick : MonoBehaviour
{

    // Use this for initialization
    public List<GameObject> LandingView;
    public GameObject HomeView;
    public GameObject CastView;

    public GameObject StoryView;

    public GameObject BuildingView;

    public GameObject FunctionView;

    public GameObject OnBoardingView;

    public GameObject Building;

    public GameObject EmailBox;

    public GameObject MessageBox;

    public GameObject GuidingLine;

    public GameObject MapView;

    public GameObject GuideView;

    public GameObject Screenshot;

    public GameObject ShootButton;

    // public GameObject BuildingOnboarding;

    public GameObject BuildingRenderToggle;

    public GameObject NavigationView;

    // public GameObject currentBuilding;

    

    GameObject DebugConsole;

    GameObject SaveButton;



    public void Start() {
        DebugConsole = GameObject.Find("DebugConsole");
        DebugConsole.SetActive(false);
        
        RefreshView();
        
        
        // toggle save button based on developer option
        
    }

    public void ToggleBuildingRendering() {
        // GlobalManagement.Building = currentBuilding;
        RectTransform toggleTransform = BuildingRenderToggle.transform.GetChild(3).GetComponent<RectTransform>();
        if (GlobalManagement.Building != null) {
            bool isTransparentRenderMode = GlobalManagement.Building.GetComponent<manipulate>().ToggleRendering();
            Debug.Log(isTransparentRenderMode);
            if (isTransparentRenderMode) {
                 toggleTransform.anchoredPosition = new Vector3(-56, -110, 0);
            } else {
                 toggleTransform.anchoredPosition = new Vector3(-56, -25, 0);
            }
            
        }
    }

    void RefreshView(){
        LandingView = GlobalManagement.Markers;
        HomeView = GlobalManagement.HomeView;
        CastView = GlobalManagement.RotateView;
        BuildingView = GlobalManagement.BuildingView;
        FunctionView = GlobalManagement.FunctionView;
        Building = GlobalManagement.Building;
        StoryView = GlobalManagement.StoryView;
        GuidingLine = GlobalManagement.GuidingLine;
        OnBoardingView = GlobalManagement.OnBoardingView;
        GuideView = GlobalManagement.GuideView;
        NavigationView = GlobalManagement.NavigationView;
        EmailBox = GlobalManagement.EmailBox;
        MessageBox = GlobalManagement.MessageBox;
        Screenshot = GlobalManagement.ScreenShot;
        ShootButton = GlobalManagement.ShootButton;
        // sub buttons
        MapView = GlobalManagement.MapView;
        SaveButton = FunctionView.transform.GetChild(0).gameObject;

    }

    public void ToggleMap() {
        RefreshView();
        MapView.SetActive(!MapView.activeSelf);
    }


    

    public void ToBuiding()
    {
        // switch scene index
        
        
        RefreshView();

        // BuildingOnboarding.SetActive(true);
        // BuildingRenderToggle.SetActive(false);
        
        // disable active screen overlay
        try {
            foreach(GameObject m in LandingView) {
                m.SetActive(false);
            }
        } catch {
            Debug.Log("No Marker Present");
        }
        HomeView.SetActive(false);
        BuildingView.SetActive(true);
        CastView.SetActive(false);
        FunctionView.SetActive(true);
        
        // GlobalManagement.ShootButton.SetActive(true);
        Destroy(Building);
        GlobalManagement.Building = null;
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Building;
        GuideView.SetActive(false);
        // BuildingOnboarding.SetActive(true);
        NavigationView.SetActive(false);

    }

    public void ToCast()
    {
        // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Cast;

        RefreshView();
        
        // disable active screen overlay
        CastView.SetActive(true);
        CastView.GetComponentInChildren<RotatableSprites>().InitializeContent(false);
        HomeView.SetActive(false);
        try {
            foreach(GameObject m in LandingView) {
                m.SetActive(false);
            }
        } catch {
            Debug.Log("No Marker Present");
        }
        BuildingView.SetActive(false);
        FunctionView.SetActive(true);
        try {
            Building.SetActive(false);
        } catch {
            Debug.Log("No Building Present");
        }

        GuidingLine.SetActive(false);
        GuideView.SetActive(false);
        GlobalManagement.ShootButton.SetActive(false);
        NavigationView.SetActive(false);

    }

    public void ToStory()
    {
         // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Story;

        RefreshView();

        StoryView.SetActive(true);
        
        // disable active screen overlay
        CastView.SetActive(false);
        HomeView.SetActive(false);
        try {
            foreach(GameObject m in LandingView) {
                m.SetActive(false);
            }
        } catch {
            Debug.Log("No Marker Present");
        }
        BuildingView.SetActive(false);
        FunctionView.SetActive(true);
        try {
            Building.SetActive(false);
        } catch {
            Debug.Log("No Building Present");
        }

        GuidingLine.SetActive(false);
        GuideView.SetActive(false);
        GlobalManagement.ShootButton.SetActive(false);
        NavigationView.SetActive(false);

    }

    public void ToScanning() {
        ToLanding(false, true);
        FunctionView.SetActive(false);
    }

    public void ToLanding(bool maintainMenu = true, bool fromMenu = false){
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Landing;

        RefreshView();

        try {
            foreach(GameObject m in LandingView) {
                m.SetActive(true);
            }
        } catch {
            Debug.Log("No Marker Present");
        }


        HomeView.SetActive(maintainMenu);
        
        CastView.SetActive(false);
        StoryView.SetActive(false);
        BuildingRenderToggle.SetActive(false);
        BuildingView.SetActive(false);
        
        FunctionView.SetActive(true);
        OnBoardingView.SetActive(false);
        GuideView.SetActive(!maintainMenu);
        GlobalManagement.ShootButton.SetActive(false);
        NavigationView.SetActive(!maintainMenu);

        EmailBox.SetActive(false);
        MessageBox.SetActive(false);
        Screenshot.SetActive(false);
        ShootButton.SetActive(false);
        MapView.SetActive(false);
        
        if (fromMenu) {
            int id = GlobalManagement.currentMarkerID;
            if (id >= 0) {
                Debug.Log("CURRENT ID" + id);
                GlobalManagement.Markers[id].GetComponentInChildren<MarkerRotation>().Reset();
            }
            
        }
        

        try {
            Building.SetActive(false);
        } catch {
            Debug.Log("No Building Present");
        }

        GuidingLine.SetActive(false);
    }
    
    
    public void Exit() {
        int currentIndex = GlobalManagement.SceneIndex;

        switch (currentIndex)
        {
            case (int) Configs.SceneIndex.Building:
                ToLanding();
                break;
            case (int) Configs.SceneIndex.Cast:
                ToLanding();
                break;
            case (int) Configs.SceneIndex.Story:
                ToLanding();
                break;
            case (int) Configs.SceneIndex.Onboarding:
                ToLanding(false);
                FunctionView.SetActive(false);
                break;
            case (int) Configs.SceneIndex.Landing:
                ToLanding(false, true);
                break;
            default:
                ToLanding(false);
                break;
        }
    }

    public void ToggleSave() {
        GlobalManagement.developerMode = !GlobalManagement.developerMode;
        DebugConsole.SetActive(GlobalManagement.developerMode);
        SaveButton.SetActive(GlobalManagement.developerMode);
    }

    // public void ReadBuildingOnboarding() {
    //     // BuildingOnboarding.SetActive(false);
    //     BuildingRenderToggle.SetActive(true);
    // }

}
