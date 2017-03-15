using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerManager : MonoBehaviour {

	Marker[] _markers;

	int _currentMarkerID;
	int _currentHotspotID;

	// stores virtual camera and virtual cast model
	GameObject _castBuffer;
	GameObject _castRotateView;

	public GameObject _HotspotSpritePrefab;
	public GameObject _FragmentPrefab;



	// Use this for initialization
	void Start () {
		_markers = GetComponentsInChildren<Marker>();
		_currentMarkerID = 0;
		_castBuffer = GameObject.Find("CastModels");
		_castRotateView = GameObject.Find("CastRotateView");
		// ClearPastData();
		ActiveCastView();
	}

	void ClearPastData() {
		Transform hotspotsView = _castRotateView.transform.FindChild("HotSpotSprites");
		foreach (Transform child in hotspotsView) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in _castRotateView.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	void ActiveCastView() {
		Transform fragmentsContainer = _castRotateView.transform.FindChild("RotatableCast");
		RotatableSprites rotator = fragmentsContainer.GetComponent<RotatableSprites>();

		// copy cast model information
		GameObject castModel = Instantiate(GetCast(), Vector3.zero, Quaternion.identity, _castBuffer.transform) as GameObject;
		castModel.transform.localPosition = new Vector3(0, 0, 0);
		// update canvas UI
		Text castNameView = _castRotateView.transform.FindChild("CastNamePanel/CastName").GetComponent<Text>();
		castNameView.text = GetMarker()._castName;
		// TODO update description
		
		rotator._currentCastModel = castModel;

		// update 2D hotspots
		// remove hotspots that belongs to previous selected cast
		Transform hotspotsView = _castRotateView.transform.FindChild("HotSpotSprites");
		Hotspot2D[] hotspotsInfo = castModel.GetComponentsInChildren<Hotspot2D>();
		Transform[] hotspotsTransform = new Transform[hotspotsInfo.Length];

		for (int i = 0; i < hotspotsInfo.Length; i++) {
			GameObject sprite = Instantiate(_HotspotSpritePrefab, hotspotsView.position, Quaternion.identity) as GameObject;
			sprite.transform.parent = hotspotsView;
			int hotspotID = i;
			// add listener on the hotspot sprite which enables toggle function

			rotator._hotspotsInfo = hotspotsInfo;

			sprite.GetComponent<Button>().onClick.AddListener(delegate  { rotator.ToggleHotSpotInfo(hotspotID);});
			hotspotsTransform[i] = sprite.transform;
		}

		rotator._HotSpotsSpritesTransform = hotspotsTransform;

		// update cast fragments
		
		Sprite[][] BG = new Sprite[hotspotsInfo.Length][];
		
		
		int minFrameAoumt = 999;

		for (int i = 0; i < hotspotsInfo.Length; i++) {
			GameObject fragment = Instantiate(_FragmentPrefab, fragmentsContainer.position, Quaternion.identity) as GameObject;
			fragment.transform.parent = fragmentsContainer.transform;
			Sprite[] fragmentSprites = hotspotsInfo[i]._sprites;
			fragment.GetComponent<Image>().sprite = fragmentSprites[0];
			BG[i] = fragmentSprites;
			if (fragmentSprites.Length < minFrameAoumt) {
				minFrameAoumt = fragmentSprites.Length;
			}
		}
		rotator.BG = BG;
		rotator._frameAmount = minFrameAoumt;
		
		rotator.InitializeContent(false);
		

	}


	

	// marker getter
	
	public Marker GetMarker(int markerID) {
		return _markers[markerID];
	}

	public Marker GetMarker() {
		return _markers[_currentMarkerID];
	}

	public GameObject GetMarkerModel() {
		return GetMarker().GetMarkerModel();
	}

	public void SetCurrentMarker(int markerID) {
		_currentMarkerID = markerID;
	}

	// building information getter

	public GameObject GetBuildingModel() {
		return GetMarker().GetBuildingModel();
	}

	public string GetBuildingHotspotName(int hotspotID) {
		Hotspot3D hotspot = GetMarker().GetBuilding().GetComponentsInChildren<Hotspot3D>()[hotspotID];
		return hotspot._name;
	}

	public string GetBuildingHotspotDescription(int hotspotID) {
		Hotspot3D hotspot = GetMarker().GetBuilding().GetComponentsInChildren<Hotspot3D>()[hotspotID];
		return hotspot._description;
	}
	

	// cast information getter

	public GameObject GetCast() {
		return GetMarker().GetCast();
	}

	public string GetCastName() {
		return GetMarker()._castName;
	}

	public string GetCastDescription() {
		return GetMarker()._castDescription;
	}




}
