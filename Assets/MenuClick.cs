using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class MenuClick : MonoBehaviour
{

    // Use this for initialization
    GameObject LandingView = GlobalManagement.Marker;
    GameObject MenuView = GlobalManagement.Content;
    GameObject CastView = GlobalManagement.RotateView;

    GameObject BuildingView = GlobalManagement.HotSpotDes;

    public void ToBuiding()
    {
        // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Building;
        
        // disable active screen overlay
        LandingView.SetActive(false);
        MenuView.SetActive(false);
        CastView.SetActive(false);
        

    }

    public void ToCast()
    {
        // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Cast;
        
        // disable active screen overlay
        CastView.SetActive(true);
        MenuView.SetActive(false);
        LandingView.SetActive(false);
        BuildingView.SetActive(false);

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
        LandingView.SetActive(true);
        MenuView.SetActive(false);
        CastView.SetActive(false);
        BuildingView.SetActive(false);
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
