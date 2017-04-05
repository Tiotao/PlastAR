using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Camera.main == null)
            return;

        this.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward.normalized * 5;

        GameObject[] list = GameObject.FindGameObjectsWithTag("VirtualMarker");
        double dist = double.MaxValue;
        GameObject nearestMarker = null;
        foreach (var marker in list)
        {
            if (Vector3.Distance(Camera.main.transform.position, marker.transform.position) < dist)
            {
                dist = Vector3.Distance(Camera.main.transform.position, marker.transform.position);
                nearestMarker = marker;
            }

            this.gameObject.transform.forward = nearestMarker.transform.position - Camera.main.transform.position;
        }
    }
}
