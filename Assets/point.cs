using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour {

    public GameObject sp;

    public Camera cam;

    Plane frustumPlane;

    GameObject markerFinder;

    GameObject[] path;

    const int slice = 20;

	// Use this for initialization
	void Start () {
        
        


        // path = new GameObject[slice];
        markerFinder = GameObject.FindGameObjectWithTag("VirtualMarkerFinder");
        for (int i = 0; i < slice; i++)
            path[i] = Instantiate(sp);
    }
	
	// Update is called once per frame
	void Update () {
        if (Camera.main == null)
            return;

        //this.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward.normalized * 10;

        GameObject[] list = GameObject.FindGameObjectsWithTag("VirtualMarker");
        double dist = double.MaxValue;
        GameObject nearestMarker = null;
        foreach (var marker in list)
        {
            if (marker.transform.position == new Vector3(0, 0, 0))
                continue;
            //if (marker.GetComponent<recognize>().seen)
            //    break;
            if (Vector3.Distance(Camera.main.transform.position, marker.transform.position) < dist)
            {
                dist = Vector3.Distance(Camera.main.transform.position, marker.transform.position);
                nearestMarker = marker;
            }
        }

        Vector3 startPoint = Camera.main.transform.position + Camera.main.transform.forward.normalized * 2;

        if (nearestMarker)
        {
            frustumPlane = GeometryUtility.CalculateFrustumPlanes(cam)[5];
            Vector3 planeCenter = -frustumPlane.normal * frustumPlane.distance;

            Ray ray = new Ray(startPoint, nearestMarker.transform.position - startPoint);
            float rayDistance;
            if (frustumPlane.Raycast(ray, out rayDistance)) {
                Vector3 intersectPos = ray.GetPoint(rayDistance);
                Vector3 centerVector = intersectPos - planeCenter;
                Vector3 upVector = planeCenter + cam.transform.up.normalized;
                float horizontalOffset = Mathf.Sqrt(centerVector.sqrMagnitude - upVector.sqrMagnitude);
                Debug.Log("(" +  upVector + ", " + centerVector + ")");

                float verticalOffset = Vector3.Dot(centerVector.normalized, upVector.normalized);
                Debug.Log("(" +  horizontalOffset + ", " + verticalOffset + ")");

            }
            // for (int i = 0; i < slice; i++)
            // {
            //     path[i].transform.position = startPoint + (nearestMarker.transform.position - startPoint) * i / slice;
            // }
            // this.gameObject.transform.position = Camera.main.transform.position - Camera.main.transform.forward.normalized * 10;
        }

        //if (nearestMarker)
        //    this.gameObject.transform.up = nearestMarker.transform.position - this.gameObject.transform.position;
        //else
        //    this.gameObject.transform.position = Camera.main.transform.position - Camera.main.transform.forward.normalized * 10;
    }
}
