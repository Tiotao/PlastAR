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
    public GameObject MenuView;
    public GameObject CastView;

    public GameObject StoryView;

    public GameObject BuildingView;

    public GameObject FunctionView;

    public GameObject Building;

    public GameObject GuidingLine;

    public void Start() {
        RefreshView();
        // toggle save button based on developer option
        
    }

    

    void RefreshView(){
        LandingView = GlobalManagement.Markers;
        MenuView = GlobalManagement.Content;
        CastView = GlobalManagement.RotateView;
        BuildingView = GlobalManagement.HotSpotDes;
        FunctionView = GlobalManagement.FunctionView;
        Building = GlobalManagement.Building;
        StoryView = GlobalManagement.StoryView;
        GuidingLine = GlobalManagement.GuidingLine;
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
        MenuView.SetActive(false);
        CastView.SetActive(false);
        FunctionView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(true);
        Destroy(Building);
        GlobalManagement.Building = null;
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Building;
        // try {
        //     Building.SetActive(true);
        // } catch {
        //     Debug.Log("No Building Present");
        // }

        
        

    }

    public void ToCast()
    {
        // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Cast;

        RefreshView();
        
        // disable active screen overlay
        CastView.SetActive(true);
        CastView.GetComponentInChildren<RotatableSprites>().InitializeContent(false);
        MenuView.SetActive(false);
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

    }

    public void ToStory()
    {
         // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Story;

        RefreshView();

        StoryView.SetActive(true);
        
        // disable active screen overlay
        CastView.SetActive(false);
        MenuView.SetActive(false);
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
        MenuView.SetActive(false);
        CastView.SetActive(false);
        StoryView.SetActive(false);
        BuildingView.SetActive(false);
        FunctionView.SetActive(false);
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
        FunctionView.transform.GetChild(0).gameObject.SetActive(GlobalManagement.developerMode);
    }
}
