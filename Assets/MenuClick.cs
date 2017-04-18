using UnityEngine;
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

    public GameObject GuidingLine;

    public GameObject MapView;

    public GameObject GuideView;

    GameObject DebugConsole;

    public void Start() {
        DebugConsole = GameObject.Find("DebugConsole");
        DebugConsole.SetActive(false);
        RefreshView();
        // toggle save button based on developer option
        
    }

    

    void RefreshView(){
        LandingView = GlobalManagement.Markers;
        HomeView = GlobalManagement.HomeView;
        CastView = GlobalManagement.RotateView;
        BuildingView = GlobalManagement.HotSpotDes;
        FunctionView = GlobalManagement.FunctionView;
        Building = GlobalManagement.Building;
        StoryView = GlobalManagement.StoryView;
        GuidingLine = GlobalManagement.GuidingLine;
        OnBoardingView = GlobalManagement.OnBoardingView;
        GuideView = GlobalManagement.GuideView;
        // sub buttons
        MapView = GlobalManagement.MapView;

    }

    public void ToggleMap() {
        RefreshView();
        Debug.Log(GlobalManagement.MapView);
        Debug.Log(MapView);
        MapView.SetActive(!MapView.activeSelf);
    }

    

    public void ToBuiding()
    {
        // switch scene index
        
        
        RefreshView();
        
        // disable active screen overlay
        try {
            foreach(GameObject m in LandingView) {
                m.SetActive(false);
            }
        } catch {
            Debug.Log("No Marker Present");
        }
        HomeView.SetActive(false);
        CastView.SetActive(false);
        FunctionView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(true);
        Destroy(Building);
        GlobalManagement.Building = null;
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Building;
        GuideView.SetActive(false);
        

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

    }

    public void ToLanding(){
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Landing;

        RefreshView();

        try {
            foreach(GameObject m in LandingView) {
                m.SetActive(true);
            }
        } catch {
            Debug.Log("No Marker Present");
        }
        HomeView.SetActive(false);
        CastView.SetActive(false);
        StoryView.SetActive(false);
        BuildingView.SetActive(false);
        FunctionView.SetActive(false);
        OnBoardingView.SetActive(false);
        GuideView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(false);

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
            default:
                ToLanding();
                break;
        }
    }

    public void ToggleSave() {
        GlobalManagement.developerMode = !GlobalManagement.developerMode;
        DebugConsole.SetActive(GlobalManagement.developerMode);
        FunctionView.transform.GetChild(0).gameObject.SetActive(GlobalManagement.developerMode);
    }
}
