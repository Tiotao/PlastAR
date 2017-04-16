using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerManager : MonoBehaviour {

	Marker[] _markers;

	int _currentMarkerID;

	int _updateMarkerID;

	int _currentHotspotID;

	// stores virtual camera and virtual cast model
	GameObject _castBuffer;

	GameObject _storyView;
	GameObject _castRotateView;

	GameObject _homeView;

	GameObject _mapView;

	// TEMP
	public GameObject _post1;
	public GameObject _post0;



	public GameObject _HotspotSpritePrefab;
	public GameObject _FragmentPrefab;



	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		// Init();
		#endif
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.K)) {
			Refresh(0);
		}
	}

	public void Init() {
		_markers = GetComponentsInChildren<Marker>();
		_castBuffer = GameObject.Find("CastModels");
		_castRotateView = GlobalManagement.RotateView;
		_storyView = GlobalManagement.StoryView;
		_homeView = GlobalManagement.HomeView;
		_mapView = GlobalManagement.MapView;
		SetCurrentMarker(-1);
		Refresh(1);
		SetCurrentMarker(-1);
		
		// TEMP: replace building model  
		// GlobalManagement.Building = GetBuildingModel();
	}
	

	public void Refresh(int markerID) {
		// Debug.Log("REFRESH: " + markerID);
		if (markerID != _currentMarkerID) {
			SetCurrentMarker(markerID);
			ClearPastData();
			ActiveStoryView();
			ActiveCastView();
			ActiveHomeView();
			ActiveMapView();
		}
	}

	public void ActiveStoryView() {

		GameObject storyContent = GetMarker().GetStory();

		GameObject story = Instantiate(storyContent, Vector3.zero, Quaternion.identity) as GameObject;
		story.transform.parent = GlobalManagement.StoryView.transform;
		story.transform.localPosition = Vector3.zero;
		story.transform.localScale = new Vector3(1, 1, 1);
	}

	public GameObject[] GetMarkerModels() {
		GameObject[] markerModels = new GameObject[_markers.Length];
		for (int i = 0; i < _markers.Length; i++) {
			markerModels[i] = GetMarkerModel(i);
		}
		return markerModels;
	}

	public GameObject GetBuildingModel() {
		return GetMarker().GetBuildingModel();
	}

	


	private void ClearPastData() {
		Transform hotspotsView = _castRotateView.transform.FindChild("HotSpotSprites");
		Transform fragmentsContainer = _castRotateView.transform.FindChild("RotatableCast");
		GlobalManagement.Building = null;

		try {
			Destroy(_castBuffer.transform.GetChild(1).gameObject);
		} catch {

		}

		foreach (Transform child in hotspotsView) {
			GameObject.Destroy(child.gameObject);
		}

		try {
			Destroy(GlobalManagement.StoryView.transform.GetChild(1).gameObject);
		} catch {

		}

		foreach (Transform child in fragmentsContainer) {
			GameObject.Destroy(child.gameObject);
		}
	}

	private void ActiveHomeView() {
		Text name = _homeView.transform.FindChild("MarkerName").GetComponent<Text>();
		Text description = _homeView.transform.FindChild("MarkerDescription").GetComponent<Text>();
		Text location = _homeView.transform.FindChild("MarkerLocationTime").GetComponentInChildren<Text>();
		
		name.text = GetMarker()._castName;
		description.text = GetMarker()._castDescription;
		location.text = GetMarker()._castLocationTime;
	}

	private void ActiveMapView() {
		_mapView.transform.Find("Map").GetComponent<Image>().sprite = GetMarker()._buildingMap;
	}


	private void ActiveCastView() {
		Transform fragmentsContainer = _castRotateView.transform.FindChild("RotatableCast");
		RotatableSprites rotator = fragmentsContainer.GetComponent<RotatableSprites>();
		// copy cast model information
		
		GameObject castModel = Instantiate(GetCast(), Vector3.zero, Quaternion.identity, _castBuffer.transform) as GameObject;
		castModel.transform.localPosition = new Vector3(0, 0, 0);
		// update canvas UI
		Text castNameView = _castRotateView.transform.FindChild("CastName").GetComponent<Text>();
		castNameView.text = GetCastName();
		Text location = _castRotateView.transform.FindChild("MarkerLocationTime").GetComponentInChildren<Text>();
		location.text = GetMarker()._castLocationTime;
		//
		// TODO update description
		
		rotator._currentCastModel = castModel;
		rotator._startFrame = GetMarker()._startingPoint;

		// update 2D hotspots
		// remove hotspots that belongs to previous selected cast
		Transform hotspotsView = _castRotateView.transform.FindChild("HotSpotSprites");
		Hotspot2D[] hotspotsInfo = castModel.GetComponentsInChildren<Hotspot2D>();
		Transform[] hotspotsTransform = new Transform[hotspotsInfo.Length];

		for (int i = 1; i < hotspotsInfo.Length; i++) {
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
			GameObject fragment = Instantiate(_FragmentPrefab) as GameObject;
			
			fragment.transform.parent = fragmentsContainer.transform;
			fragment.GetComponent<RectTransform>().anchoredPosition  = new Vector3(-30, -25, 0);
			fragment.GetComponent<RectTransform>().localScale  = new Vector3(1, 1, 1);
			Sprite[] fragmentSprites = hotspotsInfo[i]._sprites;
			fragment.GetComponent<Image>().sprite = fragmentSprites[0];
			BG[i] = fragmentSprites;
			if (fragmentSprites.Length < minFrameAoumt) {
				minFrameAoumt = fragmentSprites.Length;
			}
		}
		rotator.BG = BG;
		rotator._frameAmount = minFrameAoumt;
		// rotator.InitializeContent(false);
	}



	// marker getter

	
	private Marker GetMarker(int markerID) {
		return _markers[markerID];
	}

	private Marker GetMarker() {
		return _markers[_currentMarkerID];
	}

	private GameObject GetMarkerModel() {
		return GetMarker().GetMarkerModel();
	}

	private GameObject GetMarkerModel(int markerID) {
		return GetMarker(markerID).GetMarkerModel();
	}

	public void SetCurrentMarker(int markerID) {
		_currentMarkerID = markerID;
	}

	// building information getter
	private string GetBuildingHotspotName(int hotspotID) {
		Hotspot3D hotspot = GetMarker().GetBuilding().GetComponentsInChildren<Hotspot3D>()[hotspotID];
		return hotspot._name;
	}

	private string GetBuildingHotspotDescription(int hotspotID) {
		Hotspot3D hotspot = GetMarker().GetBuilding().GetComponentsInChildren<Hotspot3D>()[hotspotID];
		return hotspot._description;
	}

	
	

	// cast information getter

	private GameObject GetCast() {
		return GetMarker().GetCast();
	}

	private string GetCastName() {
		return GetMarker()._castName;
	}

	private string GetCastDescription() {
		return GetMarker()._castDescription.ToString();
	}

	private string GetCastLocationTime() {
		return GetMarker()._castLocationTime.ToString();
	}




}
