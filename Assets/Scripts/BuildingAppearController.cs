using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAppearController : MonoBehaviour {

	// Use this for initialization

	float w = 0f;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(w);
		// if (GetComponentsInChildren<MeshRenderer>().Length > 0) {
		// 	if (w < 1) {
		// 		w = w + 0.03f * Time.deltaTime;
		// 		foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>()) {
		// 			m.material.SetVector("_section", new Vector4(0, -1, 0, w));
		// 		}
		// 	}
		// }
	}
}
