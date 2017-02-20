using UnityEngine;
using System.Collections;

public class contentManagement : MonoBehaviour {

    GameObject plan;
    // Use this for initialization
    void Start () {
        plan = GameObject.FindGameObjectWithTag("Content");
        plan.SetActive(false);
        GlobalManagement.content = plan;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
