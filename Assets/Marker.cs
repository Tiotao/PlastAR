using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

	GameObject _building;
	GameObject _cast;

	GameObject _marker;

	public string _castName;
	public string _castDescription;


	enum DataType {
		Marker,
		Building,
		Cast
	}

	void Start() {
		_building = transform.GetChild((int) DataType.Building).gameObject;
		_cast = transform.GetChild((int) DataType.Cast).gameObject;
		_marker = transform.GetChild((int) DataType.Marker).gameObject;
	}

	// Use this for initialization
	public GameObject GetMarkerModel() {
		return _marker;
	}

	public GameObject GetBuilding() {
		return _building;
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
