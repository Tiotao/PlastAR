using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

	GameObject _building;
	GameObject _cast;

	GameObject _marker;

	GameObject _story;

	public Sprite _icon;

	public string _castName;

	[TextArea(3,10)]
	public string _castDescription;

	public string _castLocationTime;

	public int _frameCount = 19;

	public int _startingPoint = 0;

	public Sprite _buildingMap;

	public bool _isVisited;


	enum DataType {
		Marker,
		Building,
		Cast,
		Story
	}

	void Awake() {
		_building = transform.GetChild((int) DataType.Building).gameObject;
		_cast = transform.GetChild((int) DataType.Cast).gameObject;
		_marker = transform.GetChild((int) DataType.Marker).gameObject;
		_story = transform.GetChild((int) DataType.Story).gameObject;
		
	}

	// Use this for initialization
	public GameObject GetMarkerModel() {
		return _marker;
	}

	public GameObject GetBuilding() {
		return _building;
	}

	public GameObject GetStory() {
		return _story;
	}

	public GameObject GetBuildingModel() {
		return _building;
	}

	

	public GameObject GetCast() {
		return _cast;
	}

	public Hotspot3D[] GetBuildingHotspot() {
		return _building.GetComponentsInChildren<Hotspot3D>();
	}

	public Hotspot2D[] GetCastHotspot() {
		return _cast.GetComponentsInChildren<Hotspot2D>();
	}
}
