using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotatableSprites : MonoBehaviour {

    public GameObject _ControlSlider;                           // rotation slider
    
    private GameObject _CastModels;                              // container that stores models of the casts
    
    public Camera _cam;                                         // fake camera that handles hotspot placement projection on 2D canvas from 3D space

    public Text _HotSpotDescription;                            // Text field that displays the description of the hotspot

    public GameObject _HotSpotsPanel;                           // Panel that contains hotspot description

    public int _frameAmount;                                   // amount of rotation frames

    public Hotspot2D[] _hotspotsInfo;

    public Transform[] _HotSpotsSpritesTransform;              // Transform of the list of HotSpotsSprites that are displayed on UI

    private int _heightOffset = 150;                            // height offset due to the placement of the cast

    public Sprite[][] BG;                                         // container that stores frames of rotation of the current cast model

    private int _currentHotspot = -1;

    public GameObject _currentCastModel;

    public int _currentFrame = 0;


    public float minSwipeDistY;
 
    public float minSwipeDistX;
         
    private Vector2 startPos;

    // Use this for initialization
    void Awake()
    {
        _cam = GameObject.FindGameObjectWithTag("HotSpotCamera").GetComponent<Camera>();
    }

    // enable cast model and hotspots

    void Update() {
        // if (Input.touchCount > 0) {
        //     Touch touch = Input.touches[0];
        //     switch (touch.phase) {
        //         case TouchPhase.Began:
        //             startPos = touch.position;
        //             break;
        //         case TouchPhase.Ended:
        //             float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
        //             float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
        //             if (swipeDistHorizontal > minSwipeDistX) {
        //                 float swipeValue = Mathf.Sign(touch.position.x - startPos.x);    
        //                 if (swipeValue > 0) {
        //                     SetFrame((_currentFrame + 1) % _frameAmount);
        //                 } else if (swipeValue < 0) {
        //                     SetFrame((_currentFrame - 1) % _frameAmount);
        //                 }
        //             }
        //             break;
        //     }
        // }
    }

    public void InitializeContent(bool isRemote) {
        string objectName;
        if (isRemote) {
            objectName = "CastModels(Clone)";
        } else {
            objectName = "CastModels";
        }
        _CastModels = GameObject.Find(objectName);
        
        // _CastInfos = _CastModels.GetComponentsInChildren<CastInformation>();
        SetFrame();
        ToggleHotSpotInfo(0);
        
        Image defaultSprite = GetComponentsInChildren<Image>()[0];
        Color color = defaultSprite.color;
        color.a = 1;
        defaultSprite.color = color;
    }


    // frame control when rotating the model
	public void SetFrame(int frame) {
        _currentFrame = frame;
		if (frame < _frameAmount && frame >= 0) {
            for (int i = 0; i < _hotspotsInfo.Length; i++) {
                transform.GetChild(i).gameObject.GetComponent<Image>().sprite = BG[i][frame];
            }
            RotateModel(frame);
            UpdateHotSpot();
		} else {
            Debug.Log("request frame exceeds total frame count");
        }
		
	}

    public void SetFrame() {
        int frame = 18 - (int) (_ControlSlider.GetComponent<Slider>().value);
        _currentFrame = frame;
		if (frame < _frameAmount && frame >= 0) {
            for (int i = 0; i < _hotspotsInfo.Length; i++) {
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
        for (int i=1; i < _hotspotsInfo.Length; i++) {
            Vector3 screenPos = _cam.WorldToScreenPoint(_hotspotsInfo[i].gameObject.transform.position);
            screenPos.y = screenPos.y + _heightOffset;
            _HotSpotsSpritesTransform[i].position = screenPos;
            // enable the hotspot of it is visiable
            _HotSpotsSpritesTransform[i].gameObject.GetComponent<Image>().enabled = _hotspotsInfo[i].CheckVisibility(_cam);
        }
       
    }

    // rotate model
    void RotateModel(int factor) {
        float rotation = factor * 20f;
        _currentCastModel.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    // toggle display of the hotspot information, update inforamtion
    public void ToggleHotSpotInfo(int hotspotID){
        
        if (hotspotID < _hotspotsInfo.Length) {
            if (_HotSpotsPanel.activeSelf && hotspotID == _currentHotspot) {
                // _HotSpotsPanel.SetActive(false);
                _HotSpotDescription.text = _hotspotsInfo[0]._description;
                _currentHotspot = 0;
                Image[] fragmentSprites = transform.GetComponentsInChildren<Image>();
                foreach (Image sprites in fragmentSprites) {
                    float targetAlpha = 0f;
                    if (sprites.transform.GetSiblingIndex() == 0) {
                        targetAlpha = 1f;
                    }
                    Color color = sprites.color;
                    color.a = targetAlpha;
                    sprites.color = color;
                }
                
            } else {
                _HotSpotDescription.text = _hotspotsInfo[hotspotID]._description;
                // highlight fragments

                Image[] fragmentSprites = transform.GetComponentsInChildren<Image>();
                foreach (Image sprites in fragmentSprites) {
                    float targetAlpha = 0f;
                    if (sprites.transform.GetSiblingIndex() == hotspotID) {
                        targetAlpha = 0.8f;
                    }
                    if (sprites.transform.GetSiblingIndex() == 0) {
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
