using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour {

    public Transform canvasCenter;
    Plane frustumPlane;
    
    GameObject markerFinder;

    GameObject[] path;

    const int slice = 20;

	// Use this for initialization
	void Start () {

        markerFinder = GameObject.FindGameObjectWithTag("VirtualMarkerFinder");
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

        

        GameObject[] list = GameObject.FindGameObjectsWithTag("VirtualMarker");
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

        if (nearestMarker)
        {
            
            frustumPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main)[5];
            Vector3 planeCenter = -frustumPlane.normal * frustumPlane.distance;

            Ray ray = new Ray(startPoint, nearestMarker.transform.position - startPoint);
            float rayDistance;
            if (frustumPlane.Raycast(ray, out rayDistance)) {
                Vector3 intersection = ray.GetPoint(rayDistance);
                Vector3 offset = intersection - planeCenter;
                float verticalOffset = Vector3.Dot(offset, Camera.main.transform.up.normalized);
                float horizontalOffset = Vector3.Dot(offset, Camera.main.transform.right.normalized);
                // if ( (horizontalOffset < 5 && horizontalOffset > -5) || (verticalOffset < 3.5f && verticalOffset > -3.5f) ) {
                //     // markerFinder.SetActive(false);
                //     return;
                // }
                
                

                if (horizontalOffset > 5) {
                    horizontalOffset = 5;
                    
                } else if (horizontalOffset < -5) {
                    horizontalOffset = -5;

                }

                if (verticalOffset > 3.5f) {
                    verticalOffset = 3.5f;

                } else if (verticalOffset < -3.5f) {
                    verticalOffset = -3.5f;
                    
                }

                if (verticalOffset <  3.5f && verticalOffset > -3.5f && horizontalOffset < 5f && horizontalOffset > -5f) {
                    markerFinder.SetActive(false);
                } else {
                    markerFinder.SetActive(true);
                }

                markerFinder.GetComponent<RectTransform>().LookAt(canvasCenter);
                
                markerFinder.GetComponent<RectTransform>().localPosition= new Vector3(horizontalOffset * 80, verticalOffset * 70, 0);

            }
            
        }

        
    }
}
