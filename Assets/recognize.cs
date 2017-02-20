using UnityEngine;
using System.Collections;

public class recognize : MonoBehaviour {

    GameObject plan;
    private bool seen = false;
    // Use this for initialization
    void Start()
    {
        plan = GlobalManagement.content;
        //plan = GameObject.FindGameObjectWithTag("Content");
        //plan.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (seen)
        {
            seen = false;
            //plan.GetComponent<MeshRenderer>().material.color = Color.red;
            plan.SetActive(true);
        }
        else
            plan.SetActive(false);
            //plan.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    void OnWillRenderObject()
    {
        seen = true;
    }
}
