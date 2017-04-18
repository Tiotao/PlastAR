using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
 public class HumanMovement : MonoBehaviour {
     
	SkinnedMeshRenderer[] _rends;
	
	public GameObject _waypointGroup;
	List<Transform> _waypoints;

	public float _randomFactor;

	

	void Start() {
		_waypoints = new List<Transform>();

		foreach (Transform w in _waypointGroup.transform) {
			_waypoints.Add(w);
		}
		_rends = GetComponentsInChildren<SkinnedMeshRenderer>();
		// Debug.Log(_rends.Length);
		// StartAnimation();
	}

	void Update() {
		
	}

	Vector3 Randomize(Vector3 pos) {
		pos = new Vector3(
			pos.x + Random.Range(-_randomFactor, _randomFactor),
			pos.y,
			pos.z + Random.Range(-_randomFactor, _randomFactor));
		return pos;
	}

	public void StartWalking() {
		List<Vector3> route = new List<Vector3>();
		foreach (Transform t in _waypoints) {
			route.Add(Randomize(t.position));
		}
		iTween.MoveTo(gameObject, iTween.Hash(
			"path", route.ToArray(),
			"orienttopath", true,
			"movetopath", false,
			"speed", Random.Range(0.04f, 0.045f),
			"oncomplete", "ResetPosition",
			"oncompletetarget", gameObject,
			"easetype", iTween.EaseType.linear
		));
	}

	void ResetPosition() {
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut() {
		float i = 0.3f;
		while(i > 0f) {
			i = i - Time.deltaTime;
			foreach(SkinnedMeshRenderer m in _rends) {
				Color c = m.material.GetColor("_Color"); 
				m.material.SetColor("_Color", new Color(c.r, c.g, c.b, i)); 
			}
			yield return null;
		}

		StartCoroutine(FadeIn());

	}

	IEnumerator FadeIn() {
		if (Random.Range(0f, 1f) < 0.5) {
			_waypoints.Reverse();
		}
		yield return new WaitForSeconds(Random.Range(0, 10f));
		StartWalking();
		float i = 0;
		while(i < 0.3f) {
			i = i + Time.deltaTime;
			foreach(SkinnedMeshRenderer m in _rends) {
				Color c = m.material.GetColor("_Color"); 
				m.material.SetColor("_Color", new Color(c.r, c.g, c.b, i)); 
			}
			yield return null;
		}

	}

	public void StartAnimation(){
		StartCoroutine(FadeIn());
	}
 }