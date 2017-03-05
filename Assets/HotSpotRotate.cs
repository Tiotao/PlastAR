using UnityEngine;
using System.Collections;

public class HotSpotRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.Rotate(new Vector3(1, 1, 0), 5);
	}
}
