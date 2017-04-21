using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class point : MonoBehaviour {

    public Transform canvasCenter;
    Plane frustumPlane;
    
    GameObject markerFinder;

    GameObject[] path;

    const int slice = 20;

    float FOV;

    int screenWidth = 800;
    int screenHeight = 500;

    float scalingFactor = 0;

    float horizontalRange;

    float verticalRange;

    float farPlane;

    int selectedMarkerId = -1;

    public Sprite[] icons;

    public Sprite visitedIcon;


	// Use this for initialization
	void Start () {

        markerFinder = GameObject.FindGameObjectWithTag("VirtualMarkerFinder");
        if (Camera.main != null) {
            farPlane = Camera.main.farClipPlane;
            FOV = Camera.main.fieldOfView;
            verticalRange = Mathf.Tan(Mathf.Deg2Rad * FOV / 2) * farPlane;
            horizontalRange = verticalRange / screenHeight * screenWidth;
            scalingFactor = screenHeight / (verticalRange * 2);
        }
        
        // path = new GameObject[slice];
        // markerFinder = GameObject.FindGameObjectWithTag("VirtualMarkerFinder");
        // for (int i = 0; i < slice; i++)
        //     path[i] = Instantiate(sp);
    }
	
	// Update is called once per frame
	void Update () {

        if (markerFinder == null) {
            markerFinder = GameObject.FindGameObjectWithTag("VirtualMarkerFinder");
            return;
        }


        if (Camera.main == null) {
            Debug.Log("[Guide] No Camera Found");
            return;
        }

        if (scalingFactor == 0) {
            farPlane = Camera.main.farClipPlane;
            FOV = Camera.main.fieldOfView;
            verticalRange = Mathf.Tan(Mathf.Deg2Rad * FOV / 2) * farPlane;
            horizontalRange = verticalRange / screenHeight * screenWidth;
            scalingFactor = screenHeight / (verticalRange * 2);
        }

        
        // Debug.Log(scalingFactor);

        

        // GameObject[] list = GameObject.FindGameObjectsWithTag("VirtualMarker");
        List<GameObject> list = GlobalManagement.Markers;
        double dist = double.MaxValue;
        GameObject nearestMarker = null;

        foreach (var marker in list)
        {
            if (marker.transform.position == new Vector3(0, 0, 0))
                continue;
            if (Vector3.Distance(Camera.main.transform.position, marker.transform.position) < dist)
            {
                dist = Vector3.Distance(Camera.main.transform.position, marker.transform.position);
                nearestMarker = marker;
            }
        }

        Vector3 startPoint = Camera.main.transform.position;

        // override nearestMarker selection

        if (selectedMarkerId > -1) {
            markerFinder.SetActive(true);
            nearestMarker = GlobalManagement.Markers[selectedMarkerId];
            markerFinder.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = icons[selectedMarkerId];

        } else {
            markerFinder.SetActive(false);
        }


        if (nearestMarker)
        {
            
            frustumPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main)[5];
            
            Vector3 planeCenter = Camera.main.transform.position + Camera.main.transform.forward * farPlane;
            

            Ray ray = new Ray(startPoint, nearestMarker.transform.position - startPoint);
            // Debug.DrawRay(startPoint, nearestMarker.transform.position - startPoint, Color.green, Time.deltaTime, false);
            
            float rayDistance;

#if UNITY_EDITOR
            DrawLine(planeCenter, Camera.main.transform.position, Color.black);
            DrawLine(startPoint, nearestMarker.transform.position, Color.green);
#endif
            
            if (frustumPlane.Raycast(ray, out rayDistance)) {
                Vector3 intersection = ray.GetPoint(rayDistance);
                Vector3 offset = intersection - planeCenter;

                
                float verticalOffset = Vector3.Dot(offset, Camera.main.transform.up);
                float horizontalOffset = Vector3.Dot(offset, Camera.main.transform.right);
                // if ( (horizontalOffset < 5 && horizontalOffset > -5) || (verticalOffset < 3.5f && verticalOffset > -3.5f) ) {
                //     // markerFinder.SetActive(false);
                //     return;
                // }
#if UNITY_EDITOR
                DrawLine(planeCenter, intersection, Color.red);
                DrawLine(intersection, intersection + Camera.main.transform.up, Color.red);
                DrawLine(intersection, intersection + Camera.main.transform.right, Color.yellow);
#endif
                
                 

                if (horizontalOffset > horizontalRange) {
                    horizontalOffset = horizontalRange;
                    
                } else if (horizontalOffset < -horizontalRange) {
                    horizontalOffset = -horizontalRange;

                }

                if (verticalOffset > verticalRange) {
                    verticalOffset = verticalRange;

                } else if (verticalOffset < -verticalRange) {
                    verticalOffset = -verticalRange;
                    
                }

                

                if (verticalOffset <  verticalRange && verticalOffset > -verticalRange && horizontalOffset < horizontalRange && horizontalOffset > -horizontalRange) {
                    markerFinder.SetActive(false);
                } else {
                    markerFinder.SetActive(true);
                }


                markerFinder.GetComponent<RectTransform>().LookAt(canvasCenter);
                
                markerFinder.GetComponent<RectTransform>().localPosition= new Vector3(horizontalOffset * scalingFactor,  verticalOffset * scalingFactor, 0);

            }
            
        }

        
    }

    public void SetCurrentNavigation(int id) {
        if (selectedMarkerId == id) {
            id = -1;
        }

        selectedMarkerId = id;
        
        // selectedMarkerId = id;
        // int i = 0;
        // foreach (Button child in GlobalManagement.NavigationView.GetComponentsInChildren<Button>()) {
        //     if (i == id) {
        //         child.gameObject.GetComponent<Image>().sprite = visitedIcon;
        //         // child.gameObject.GetComponent<Image>().color = new Color(167f/255f, 0, 1f, 1f);
        //     } else {
        //         // child.gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
        //     }
        //     i++;
        // }

    }

    public void SetVisitedIcon(int id) {
        GlobalManagement.NavigationView.GetComponentsInChildren<Button>()[id].gameObject.GetComponent<Image>().sprite = visitedIcon;
    }


    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.02f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
