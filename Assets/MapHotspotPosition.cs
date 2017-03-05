using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHotspotPosition : MonoBehaviour {

	public GameObject _HotSpot;
	public GameObject _HotSpotSprite;
	public Camera _cam;
	public int _heightOffset = 150;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPos = _cam.WorldToScreenPoint(_HotSpot.transform.position);
		screenPos.y = screenPos.y + _heightOffset;
		_HotSpotSprite.transform.position = screenPos;
	}
}
