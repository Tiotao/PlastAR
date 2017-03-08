using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotatableSprites : MonoBehaviour {

    public GameObject _ControlSlider;                           // rotation slider
    
    private GameObject _CastModels;                              // container that stores models of the casts
    
    private Camera _cam;                                         // fake camera that handles hotspot placement projection on 2D canvas from 3D space

    public GameObject _HotSpotsSprites;                         // container that stores current hotspots sprites on UI

    public GameObject _HotSpotSpritePrefab;                     // prefab that used to generate hotspot sprites on UI

    public Text _CastTitle;                                     // Text field that displays the name of the cast

    public Text _HotSpotDescription;                            // Text field that displays the description of the hotspot

    public GameObject _HotSpotsPanel;                           // Panel that contains hotspot description

    private int _frameAmount;                                   // amount of rotation frames

    private int _currentCast = 0;                               // ID of the current cast being selected

    private HotSpotInformation[] _HotSpotsInfo;                 // array of HotSpotsInfo, which contains description of each hotspot

    private Transform[] _HotSpotsSpritesTransform;              // Transform of the list of HotSpotsSprites that are displayed on UI

    private int _heightOffset = 0;                            // height offset due to the placement of the cast

    private CastInformation[] _CastInfos;                       // array of CastInformation, which stores inforamtion of each cast model
    public Sprite[][] BG;                                         // container that stores frames of rotation of the current cast model

    private int _currentHotspot = -1;

    public GameObject _FragmentPrefab;
    // Use this for initialization
    void Start()
    {
        // _CastInfos = _CastModels.GetComponentsInChildren<CastInformation>();
        // EnableCast(0);
        if (GameObject.Find("AssetsLoader") == null) {
            InitializeContent(false);
        }
    }

    // enable cast model and hotspots

    public void InitializeContent(bool isRemote) {
        string objectName;
        if (isRemote) {
            objectName = "CastModels(Clone)";
        } else {
            objectName = "CastModels";
        }
        _CastModels = GameObject.Find(objectName);
        _cam = GameObject.FindGameObjectWithTag("HotSpotCamera").GetComponent<Camera>();
        _CastInfos = _CastModels.GetComponentsInChildren<CastInformation>();
        EnableCast(1);
    }

    void EnableCast(int castID) {
        if (castID < _CastInfos.Length) {
            _currentCast = castID;
            
            CastInformation currentCast = _CastInfos[_currentCast];
            // read all cast model sprites
            
            // update cast name
            _CastTitle.text = (currentCast._castName).ToUpper();
            // add hotspots and rotation control
            SetHotSpotsCorrespondance();
            SetFragments();
            SetFrame();
        } else {
            Debug.Log("castID invalid");
        }
       
    }

    // create part of the model that may be highlighted when hotspot is featured
    void SetFragments() {
        // clear previous fragments
        foreach (Transform child in gameObject.transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        CastInformation currentCast = _CastInfos[_currentCast];
        
        BG = new Sprite[_HotSpotsInfo.Length][];
        
        for (int i = 0; i < _HotSpotsInfo.Length; i++) { 
            GameObject fragment = Instantiate(_FragmentPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
            fragment.transform.parent = gameObject.transform;
            // string spriteFolderName = currentCast._spriteFolderName + "/HotSpot" + i.ToString();
            // Sprite[] fragmentSprites = Resources.LoadAll<Sprite>(spriteFolderName);
            Sprite[] fragmentSprites = _HotSpotsInfo[i]._sprites;
            fragment.GetComponent<Image>().sprite = fragmentSprites[0];
            _frameAmount = fragmentSprites.Length;
            BG[i] = fragmentSprites;
        }


    }

    // manages hotspot movement when cast model is moving
    void SetHotSpotsCorrespondance() {
        if (_currentCast < _CastInfos.Length) {
            _HotSpotsInfo = _CastInfos[_currentCast].gameObject.GetComponentsInChildren<HotSpotInformation>();
            _HotSpotsSpritesTransform = new Transform[_HotSpotsInfo.Length];
            // remove hotspots that belongs to previous selected cast
            foreach (Transform child in _HotSpotsSprites.transform) {
                GameObject.Destroy(child.gameObject);
            }
            // attach hotspot sprites in UI
            for (int i = 0; i < _HotSpotsInfo.Length; i++) {
                GameObject sprite = Instantiate(_HotSpotSpritePrefab, _HotSpotsSprites.transform.position, Quaternion.identity) as GameObject;
                sprite.transform.parent = _HotSpotsSprites.transform;
                int hotspotID = i;
                // add listener on the hotspot sprite which enables toggle function
                sprite.GetComponent<Button>().onClick.AddListener(delegate  { ToggleHotSpotInfo(hotspotID);});
                _HotSpotsSpritesTransform[i] = sprite.transform;
            }   
        } else {
            Debug.Log("castID invalid");
        }
        
    }

    // frame control when rotating the model
	public void SetFrame() {
        int frame = (int) (_ControlSlider.GetComponent<Slider>().value);
		if (frame < _frameAmount && frame >= 0) {
            for (int i = 0; i < _HotSpotsInfo.Length; i++) {
                transform.GetChild(i).gameObject.GetComponent<Image>().sprite = BG[i][frame];
            }
            RotateModel(frame);
            UpdateHotSpot();
		} else {
            Debug.Log("request frame exceeds total frame count");
        }
		
	}

    // update hotspot inforamtion when a new cast is enabled
    void UpdateHotSpot() {
        for (int i=0; i < _HotSpotsInfo.Length; i++) {
            Vector3 screenPos = _cam.WorldToScreenPoint(_HotSpotsInfo[i].gameObject.transform.position);
            screenPos.y = screenPos.y + _heightOffset;
            _HotSpotsSpritesTransform[i].position = screenPos;
            // enable the hotspot of it is visiable
            _HotSpotsSpritesTransform[i].gameObject.GetComponent<Image>().enabled = _HotSpotsInfo[i].CheckVisibility();
        }
       
    }

    // rotate model
    void RotateModel(int factor) {
        float rotation = factor * 20f;
        _CastInfos[_currentCast].gameObject.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    // toggle display of the hotspot information, update inforamtion
    public void ToggleHotSpotInfo(int hotspotID){
        if (hotspotID < _HotSpotsInfo.Length) {
            if (_HotSpotsPanel.activeSelf && hotspotID == _currentHotspot) {
                _HotSpotsPanel.SetActive(false);
                _currentHotspot = -1;
                Image[] fragmentSprites = transform.GetComponentsInChildren<Image>();
                foreach (Image sprites in fragmentSprites) {
                    float targetAlpha = 1f;
                    Color color = sprites.color;
                    color.a = targetAlpha;
                    sprites.color = color;
                }
            } else {
                _HotSpotDescription.text = _HotSpotsInfo[hotspotID]._hotSpotDescription;
                // highlight fragments

                Image[] fragmentSprites = transform.GetComponentsInChildren<Image>();
                foreach (Image sprites in fragmentSprites) {
                    float targetAlpha = 0.2f;
                    if (sprites.transform.GetSiblingIndex() == hotspotID) {
                        targetAlpha = 1f;
                    }
                    Color color = sprites.color;
                    color.a = targetAlpha;
                    sprites.color = color;
                }


                _currentHotspot = hotspotID;
                _HotSpotsPanel.SetActive(true);
                
            }
           
        } else {
            Debug.Log("hotspotID invalid");
        }
    }

    
    
    
}
