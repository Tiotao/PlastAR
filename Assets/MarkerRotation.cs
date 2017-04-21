using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerRotation : MonoBehaviour {

	public bool _startRotating = false;

	MeshRenderer _renderer;

	public bool _isFocusing = false;

	public bool _isFocused = false;

	
	

	
	

	float _progress = 0f;

	// Use this for initialization
	void Start () {
		_renderer = GetComponent<MeshRenderer>();
		// Rotate();
	}
	
	// Update is called once per frame
	void Update () {

		bool inMenu = GlobalManagement.HomeView.activeSelf;

		if (_startRotating) {
			Rotate();
			_startRotating = false;
		}

		if (_isFocusing) {
			if (_progress < 1.0f) {
				_progress = _progress + Time.deltaTime / 3;
				_renderer.material.SetFloat("_threshold", _progress);
			} else {
				if (!_isFocused) {
					_isFocused = true;
					
				} else {
					_isFocused = false;
				}
			}

		} else {
			
			if (_progress > 0f && !inMenu) {
				_progress = _progress - Time.deltaTime;
				_renderer.material.SetFloat("_threshold", _progress);
			}

			if (_isFocused) {
				_isFocused = false;
			}
			
		}

		_isFocusing = false;

		
	}

	public void Reset() {
		_isFocusing = false;
		_isFocused = false;
		_progress = 0f;
		_renderer.material.SetFloat("_threshold", _progress);
	}

	void Rotate() {
		iTween.RotateBy(gameObject, iTween.Hash(
			"time", 3f,
			"y", 1f,
			"x", 0f,
			"z", 0f, 
			"easetype", "linear",
			"oncomplete", "Rotate",
			"oncompletetarget", gameObject
		));
	}

	public void Pause() {
		iTween.Pause(gameObject);
	}

	public void Resume() {
		iTween.Resume(gameObject);
	}
	
	public void StartFocusing() {
		_isFocusing = true;
		
	}

	public void StopFocusing() {
		_isFocusing = false;
	}

}
