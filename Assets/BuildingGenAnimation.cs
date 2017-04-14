using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenAnimation : MonoBehaviour {

	bool _isResizing = false;
	float _radius = 0.3f;

	public float _buildingHeight = 0.6f;

	public float _buildingRadius = 0.3f;
	ParticleSystem _ps;

	GameObject _wrapper;

	public void SetRange(float height, float radius) {
		_buildingHeight = height;
		_buildingRadius = radius;
		var sh = _ps.shape;
		sh.radius = _radius;
	}

	// Use this for initialization
	void Start () {
		_wrapper = transform.GetChild(0).gameObject;
		_ps = GetComponentInChildren<ParticleSystem>();
		var sh = _ps.shape;
		sh.radius = _radius;
	}
	
	// Update is called once per frame
	void Update () {
		if (_isResizing) {
			if (_radius > 0.04) {
				_radius = _radius - 0.3f * Time.deltaTime;
				var sh = _ps.shape;
				sh.radius = _radius;
			} else {
				_isResizing = false;
			}
			
		}

		if (_wrapper.transform.localPosition.y >= _buildingHeight) {
			var em = _ps.emission;
			em.enabled = false;
			iTween.Stop();
			StartCoroutine(DestroyEffect());
		}
	}

	public void StartAnimation() {
		Resize();
		MoveToSide();
	}

	IEnumerator DestroyEffect(){
		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}

	void MoveToSide() {
		iTween.MoveBy(_ps.gameObject, iTween.Hash(
			"amount", new Vector3(0, _buildingRadius, 0),
			"speed", 0.3f,
			"islocal", true,
			"easetype", iTween.EaseType.easeInSine,
			"oncomplete", "PlayRotate",
			"oncompletetarget", gameObject
		));
	}

	void PlayRotate() {
		iTween.RotateBy(_wrapper, iTween.Hash(
			"y", 1.0f, 
			"time", 1f,
			"easetype", "linear",
			"looptype", iTween.LoopType.loop
        ));
		iTween.MoveBy(_wrapper, iTween.Hash(
			"amount", new Vector3(0, _buildingHeight + 0.4f, 0),
			"speed", 0.2f,
			"easetype", iTween.EaseType.easeOutSine,
			"oncompletetarget", gameObject
		));
	}

	void Resize() {
		_ps.simulationSpace =  ParticleSystemSimulationSpace.World;
		
		_isResizing = true;
	}
}
