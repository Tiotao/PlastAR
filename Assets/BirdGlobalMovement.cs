using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGlobalMovement : MonoBehaviour {

	public GameObject _routes;

	public BirdRandomMovement[] _birds;
	public List<Transform> _waypoints;

	// Use this for initialization
	void Start () {
		foreach (Transform waypoint in _routes.transform) {
			_waypoints.Add(waypoint);
		}
		
		_birds = GetComponentsInChildren<BirdRandomMovement>();

		foreach (BirdRandomMovement bird in _birds) {
			float time = Random.Range(16f, 20f);
			iTween.MoveTo(bird.transform.parent.gameObject, iTween.Hash(
				"path", _waypoints.ToArray(),
				"orienttopath", true,
				// "movetopath", true,
				"time", time,
				"looptype", iTween.LoopType.loop,
				"easetype", iTween.EaseType.linear
			));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
