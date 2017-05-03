using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerManager : MonoBehaviour {

	public Marker[] _markers;

	int _currentMarkerID;

	int _updateMarkerID;

	int _currentHotspotID;

	// stores virtual camera and virtual cast model
	GameObject _castBuffer;

	GameObject _storyView;
	GameObject _castRotateView;

	GameObject _homeView;

	GameObject _buildingView;

	GameObject _mapView;


	public GameObject _HotspotSpritePrefab;
	public GameObject _FragmentPrefab;
	public GameObject _StoryPrefab;
	public GameObject _PostcardPrefab;
	public GameObject _PostcardHotspotPrefab;



	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		// Init();
		#endif
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Refresh(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Refresh(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			Refresh(2);
		}
	}

	public void Init() {
		_markers = GetComponentsInChildren<Marker>();
		_castBuffer = GameObject.Find("CastModels");
		_castRotateView = GlobalManagement.RotateView;
		_storyView = GlobalManagement.StoryView;
		_homeView = GlobalManagement.HomeView;
		_mapView = GlobalManagement.MapView;
		_buildingView = GlobalManagement.BuildingView;
		updateMarkerIcon();

		

		SetCurrentMarker(-1);
		Refresh(0);
		SetCurrentMarker(-1);

		
		
		// TEMP: replace building model  
		// GlobalManagement.Building = GetBuildingModel();
	}

	public void updateMarkerIcon() {
		List<Sprite> icons = new List<Sprite>();
		foreach (Marker m in _markers) {
			icons.Add(m._icon);
		}
		GameObject.FindGameObjectWithTag("Navigator").GetComponent<point>().icons = icons.ToArray();
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
			ActiveBuildingView();
		}
	}

	public void ActiveStoryView() {

		GameObject storyContent = GetMarker().GetStory();

		

		Text castName = GlobalManagement.StoryView.transform.GetChild(1).GetComponent<Text>();

		castName.text = GetMarker()._castName;

		GameObject story = InitStoryContent(storyContent) as GameObject;


		story.transform.parent = GlobalManagement.StoryView.transform;
		story.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
		story.transform.localScale = new Vector3(1, 1, 1);
	}

	GameObject InitStoryContent(GameObject storyContent) {
		GameObject story = Instantiate(_StoryPrefab);
		Postcard[] postcardsInfo = storyContent.GetComponentsInChildren<Postcard>();

		int postcardId = 0;

		foreach (Postcard pInfo in postcardsInfo) {

			GameObject postcard = Instantiate(_PostcardPrefab);
			postcard.transform.parent = story.transform;
			postcard.transform.FindChild("Back/Content").GetComponent<Image>().sprite = pInfo._frontImage;
			postcard.transform.FindChild("Back/Description/Year").GetComponent<Text>().text = pInfo._year;
			GameObject hotspots = postcard.transform.FindChild("Back/Hotspots").gameObject;
			
			int hotspotId = 0;

			Vector2 gridPos = new Vector2((int) postcardId % 3 * 240, -100 - 180 * (int) (postcardId / 3));

			postcard.GetComponent<RectTransform>().anchoredPosition = gridPos;

			PostcardController controller = postcard.GetComponent<PostcardController>();
			
			foreach (HotspotStory hotspotInfo in pInfo.GetComponentsInChildren<HotspotStory>()) {
				
				GameObject hotspot = Instantiate(_PostcardHotspotPrefab);
				hotspot.transform.parent = hotspots.transform;
				HotspotStory content = hotspot.GetComponent<HotspotStory>();
				content._coolFact = hotspotInfo._coolFact;
				content._description = hotspotInfo._description;
				content._sprite = hotspotInfo._sprite;
				hotspot.GetComponent<RectTransform>().anchoredPosition = hotspotInfo._position;
				int tempHotspotId = hotspotId;
				hotspot.GetComponent<Button>().onClick.AddListener(()=> controller.ToggleSides(tempHotspotId));
				hotspotId++;
			}
			postcardId++;
		}

		return story;
		// GameObject postcard = Instantiate(_PostcardPrefab);
		// GameObject hotspots = postcard.transform.FindChild("Hotspots").gameObject;
		
	}

	// public GameObject[] GetMarkerModels() {
	// 	GameObject[] markerModels = new GameObject[_markers.Length];
	// 	for (int i = 0; i < _markers.Length; i++) {
	// 		markerModels[i] = GetMarkerModel(i);
	// 	}
	// 	return markerModels;
	// }

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
			Destroy(GlobalManagement.StoryView.transform.GetChild(4).gameObject);
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

	private void ActiveBuildingView() {
		_buildingView.transform.Find("CastName").GetComponent<Text>().text = GetMarker()._castName;
	}

	private void ActiveCastView() {


		// update building 

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

	// private GameObject GetMarkerModel() {
	// 	return GetMarker().GetMarkerModel();
	// }

	// private GameObject GetMarkerModel(int markerID) {
	// 	return GetMarker(markerID).GetMarkerModel();
	// }

	public void SetCurrentMarker(int markerID) {
		GlobalManagement.currentMarkerID = markerID;
		_currentMarkerID = markerID;
	}

	// building information getter
	// private string GetBuildingHotspotName(int hotspotID) {
	// 	Hotspot3D hotspot = GetMarker().GetBuilding().GetComponentsInChildren<Hotspot3D>()[hotspotID];
	// 	return hotspot._name;
	// }

	// private string GetBuildingHotspotDescription(int hotspotID) {
	// 	Hotspot3D hotspot = GetMarker().GetBuilding().GetComponentsInChildren<Hotspot3D>()[hotspotID];
	// 	return hotspot._description;
	// }

	
	

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
