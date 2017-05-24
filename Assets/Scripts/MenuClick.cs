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

    public GameObject EmailBox;

    public GameObject MessageBox;

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

    GameObject MenuButton;

    GameObject ScanButton;

    public Sprite[] renderingHandles;

    public Sprite[] hotspotHandles;

    bool isScrollingLeft;

    bool isScrollingRight;

    Transform navContent;


    public void Start() {
        DebugConsole = GameObject.Find("DebugConsole");
        DebugConsole.SetActive(false);
        RefreshView();
        ScanButton = FunctionView.transform.GetChild(1).gameObject;
        MenuButton = FunctionView.transform.GetChild(2).gameObject;
        navContent = NavigationView.transform.FindChild("Scroll View/Viewport/Content");
        SaveButton.SetActive(false);
        

        // toggle save button based on developer option
        
    }

    public void ToggleBuildingRendering() {
        // GlobalManagement.Building = currentBuilding;
        Image toggle = BuildingRenderToggle.transform.FindChild("Slider/Handle Slide Area/Handle").GetComponent<Image>();
        Debug.Log(toggle);
        
        if (GlobalManagement.Building != null) {
            bool isTransparentRenderMode = GlobalManagement.Building.GetComponent<manipulate>().ToggleRendering();
            Debug.Log(isTransparentRenderMode);
            if (isTransparentRenderMode) {
                 toggle.sprite = renderingHandles[1];
            } else {
                 toggle.sprite =  renderingHandles[0];
            }
            
        }
    }

    public void ScaleBuilding () {
        Debug.Log(GlobalManagement.Building.GetComponent<manipulate>());
        GlobalManagement.Building.GetComponent<manipulate>().UpdateScale();
    }

    void RefreshView(){
        LandingView = GlobalManagement.Markers;
        HomeView = GlobalManagement.HomeView;
        CastView = GlobalManagement.RotateView;
        BuildingView = GlobalManagement.BuildingView;
        FunctionView = GlobalManagement.FunctionView;
        Building = GlobalManagement.Building;
        StoryView = GlobalManagement.StoryView;
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

    
    public void ToggleMenuButton(bool isVisible) {
        if (isVisible) {
            MenuButton.SetActive(true);
            ScanButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-100, -15, 0);
        } else {
            MenuButton.SetActive(false);
            ScanButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-15, -15, 0);
        }
       
    }


    

    public void ToBuiding()
    {
        // switch scene index
        BuildingView.SetActive(false);

        RefreshView();

        if (!Building && GlobalManagement.SceneIndex == (int) Configs.SceneIndex.Building) {
            ToLanding();
            return;
        }

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
        ToggleMenuButton(true);

    }

    public void ToCast()
    {
        // switch scene index
        GlobalManagement.SceneIndex = (int) Configs.SceneIndex.Cast;

        RefreshView();
        
        // disable active screen overlay
        CastView.SetActive(true);
        Debug.Log(CastView);
        Debug.Log(CastView.activeSelf);
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


        GuideView.SetActive(false);
        GlobalManagement.ShootButton.SetActive(false);
        NavigationView.SetActive(false);
        ToggleMenuButton(true);

        Debug.Log(CastView);
        Debug.Log(CastView.activeSelf);

        CastView.SetActive(true);

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


        GuideView.SetActive(false);
        GlobalManagement.ShootButton.SetActive(false);
        NavigationView.SetActive(false);
        ToggleMenuButton(true);
        

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

        ToggleMenuButton(false);
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

    public void NavScrollLeft() {
        isScrollingLeft = true;
        isScrollingRight = false;
    }

    public void NavScrollRight() {
        isScrollingLeft = false;
        isScrollingRight = true;
    }

    public void NavScrollEnd() {
        isScrollingLeft = false;
        isScrollingRight = false;
    }

    public void OnNavScrollChange() {
        Button leftButton = NavigationView.transform.FindChild("Scroller/Left").GetComponent<Button>();
        Button rightButton = NavigationView.transform.FindChild("Scroller/Right").GetComponent<Button>();
        RectTransform content = NavigationView.transform.FindChild("Scroll View/Viewport/Content").GetComponent<RectTransform>();
        if (content.anchoredPosition.x < 600f - content.sizeDelta.x) {
            leftButton.interactable = false;
            rightButton.interactable = true;
        } else if (content.anchoredPosition.x > 0) {
            leftButton.interactable = true;
            rightButton.interactable = false;
        } else {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    void Update(){
        if (isScrollingLeft) {
            Vector2 originalPos = navContent.GetComponent<RectTransform>().anchoredPosition;
            navContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(originalPos.x - 200 * Time.deltaTime, originalPos.y);
        }

        if (isScrollingRight) {
            Vector2 originalPos = navContent.GetComponent<RectTransform>().anchoredPosition;
            navContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(originalPos.x + 200 * Time.deltaTime, originalPos.y);

        }
    }


    // public void ReadBuildingOnboarding() {
    //     // BuildingOnboarding.SetActive(false);
    //     BuildingRenderToggle.SetActive(true);
    // }

}
