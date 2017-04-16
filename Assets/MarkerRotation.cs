using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerRotation : MonoBehaviour {

	public bool _startRotating = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_startRotating) {
			Rotate();
			_startRotating = false;
		}
	}

	void Rotate() {
		Debug.Log("rotate");
		Debug.Log(gameObject);
		iTween.RotateBy(gameObject, iTween.Hash(
			"time", 3f,
			"y", 1f,
			"x", 1f,
			"z", -1f, 
			"easetype", "linear",
			"oncomplete", "Rotate",
			"oncompletetarget", gameObject
		));
	}

}
