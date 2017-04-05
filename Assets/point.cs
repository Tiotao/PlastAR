using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour {

    public GameObject sp;

    GameObject[] path;

    const int slice = 20;

	// Use this for initialization
	void Start () {
        path = new GameObject[slice];
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
            for (int i = 0; i < slice; i++)
            {
                path[i].transform.position = startPoint + (nearestMarker.transform.position - startPoint) * i / slice;
            }
            this.gameObject.transform.position = Camera.main.transform.position - Camera.main.transform.forward.normalized * 10;
        }

        //if (nearestMarker)
        //    this.gameObject.transform.up = nearestMarker.transform.position - this.gameObject.transform.position;
        //else
        //    this.gameObject.transform.position = Camera.main.transform.position - Camera.main.transform.forward.normalized * 10;
    }
}
