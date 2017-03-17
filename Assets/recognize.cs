using UnityEngine;
using System.Collections;

public class recognize : MonoBehaviour {

    GameObject plan;
    private bool seen = false;
    GameObject MarkerManager;
    // Use this for initialization
    void Start()
    {
        MarkerManager = GameObject.FindGameObjectWithTag("MarkerManager");
        plan = GlobalManagement.Content;
        //plan = GameObject.FindGameObjectWithTag("Content");
        //plan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (CouldBeSeen() && GlobalManagement.SceneIndex == (int) Configs.SceneIndex.Landing)
        {
            plan.SetActive(true);
            MarkerManager.GetComponent<MarkerManager>().SetCurrentMarker(this.GetComponent<ARMarker>().GetID());
        }
        else
        {
            plan.SetActive(false);
        }

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
        float angleThresh = 25;
        float distThresh = 50;

        Vector3 vc = Camera.main.transform.forward;
        Vector3 vt = this.gameObject.transform.position - Camera.main.transform.position;

        if (Vector3.Angle(vc, vt) < angleThresh && Vector3.Distance(this.gameObject.transform.position, Camera.main.transform.position) < distThresh)
            return true;
        else
            return false;
    }
}
