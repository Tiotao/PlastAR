﻿using UnityEngine;
using System.Collections;

public class recognize : MonoBehaviour {

    GameObject plan;
    public bool seen = false;
    
    GameObject MarkerManager;

    ParticleSystem[] hotspots;
    // Use this for initialization
    void Start()
    {
        MarkerManager = GameObject.FindGameObjectWithTag("MarkerManager");
        plan = GlobalManagement.HomeView;
        hotspots = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in hotspots) {
            ParticleSystem.EmissionModule em = p.emission;
            em.enabled = true;
        }
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>()) {
            m.enabled = true;
        }


        //plan = GameObject.FindGameObjectWithTag("Content");
        //plan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        seen = CouldBeSeen();
        // if (CouldBeSeen() && GlobalManagement.SceneIndex == (int) Configs.SceneIndex.Landing)
        // {
        //     plan.SetActive(true);
        //     // MarkerManager.GetComponent<MarkerManager>().SetCurrentMarker(this.GetComponent<ARMarker>().GetID());
        //     MarkerManager.GetComponent<MarkerManager>().Refresh(this.GetComponent<ARMarker>().GetID());
        //     Debug.Log(GetComponent<ARMarker>().GetID() + " : could be seen.");
        // }
        // else
        // {
        //     plan.SetActive(false);
        // }

        //if (seen)
        //{
        //    seen = false;
        //    //plan.GetComponent<MeshRenderer>().material.color = Color.red;
        //    plan.SetActive(true);
        //}
        //else
        //{
        //    plan.SetActive(false);
        //}
    }

    void OnWillRenderObject()
    {
        seen = true;
    }

    bool CouldBeSeen()
    {
        float angleThresh = Camera.main.fieldOfView / 4;
        float distThresh = Camera.main.farClipPlane;

        Vector3 vc = Camera.main.transform.forward;
        Vector3 vt = this.gameObject.transform.position - Camera.main.transform.position;

        float angle = Vector3.Angle(vc, vt);

        float forwardDistance = Mathf.Cos(angle) * Vector3.Distance(this.gameObject.transform.position, Camera.main.transform.position);

        if (angle < angleThresh && forwardDistance < distThresh) {
            Debug.Log(forwardDistance);
            Debug.Log(angle);
            return true;
        } else {
            return false;
        }
    }
}
