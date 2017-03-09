﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class MenuClick : MonoBehaviour
{

    // Use this for initialization
    public GameObject LandingView;
    public GameObject MenuView;
    public GameObject CastView;

    public GameObject BuildingView;

    public GameObject FunctionView;

    public GameObject Building;

    public void Start() {
        RefreshView();
    }

    void RefreshView(){
        LandingView = GlobalManagement.Marker;
        MenuView = GlobalManagement.Content;
        CastView = GlobalManagement.RotateView;
        BuildingView = GlobalManagement.HotSpotDes;
        FunctionView = GlobalManagement.FunctionView;
        Building = GlobalManagement.Building;
    }

    public void ToBuiding()
    {
        // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Building;
        
        RefreshView();
        
        // disable active screen overlay
        try {
            LandingView.SetActive(false);
        } catch {
            Debug.Log("No Marker Present");
        }
        MenuView.SetActive(false);
        CastView.SetActive(false);
        FunctionView.SetActive(true);
        Building.SetActive(true);
        

    }

    public void ToCast()
    {
        // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Cast;

        RefreshView();
        
        // disable active screen overlay
        CastView.SetActive(true);
        MenuView.SetActive(false);
        try {
            LandingView.SetActive(false);
        } catch {
            Debug.Log("No Marker Present");
        }
        BuildingView.SetActive(false);
        FunctionView.SetActive(true);
        Building.SetActive(false);

    }

    public void ToStory()
    {
        // // switch scene index
        // GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Cast;
        
        // // disable active screen overlay
        // GameObject marker = GlobalManagement.Marker;
        // GameObject plan = GlobalManagement.Content;
        // marker.SetActive(false);
        // plan.SetActive(false);

    }

    public void ToLanding(){
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Landing;

        RefreshView();

        try {
            LandingView.SetActive(true);
        } catch {
            Debug.Log("No Marker Present");
        }
        MenuView.SetActive(false);
        CastView.SetActive(false);
        BuildingView.SetActive(false);
        FunctionView.SetActive(false);
        Building.SetActive(false);
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
}
