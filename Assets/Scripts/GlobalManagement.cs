using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalManagement
{
    public static GameObject HomeView;
    public static GameObject ScreenShot;
    public static GameObject RotateView;
    public static List<GameObject> Markers;
    public static GameObject Building;
    public static GameObject FunctionView;

    public static GameObject OnBoardingView;
    public static GameObject ShootButton;
    public static GameObject MessageBox;
    public static GameObject EmailBox;

    public static GameObject NavigationView;

    public static GameObject StoryView;

    // public static GameObject GuidingLine;

    public static GameObject BuildingAppearFX;

    public static GameObject MapView;

    public static GameObject GuideView;

    public static GameObject BuildingView;

    public static int SceneIndex = (int) Configs.SceneIndex.Onboarding;

    public static int currentMarkerID = -1;

    public static bool developerMode = false;

    public static int appearMode = (int) Configs.AppearMode.Grow;

}
