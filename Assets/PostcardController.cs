using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostcardController : MonoBehaviour {


	GameObject _Front;
	GameObject _Back;

	GameObject _Hotspots;

	public bool _rotating;
	public bool _expanding;

	public bool _isFocused = false;

	public bool _isBack = true;

	public Vector3 _initialLocation;

	float _ZOOM_TIME = 1f;
	float _ROTATE_TIME = 0.5f;
	

	// Use this for initialization
	void Start () {
		_Front = transform.GetChild(0).gameObject;
		_Back = transform.GetChild(1).gameObject;
		_Hotspots = _Back.transform.FindChild("Hotspots").gameObject;
		_initialLocation = transform.localPosition;
		_Front.transform.rotation = Quaternion.Euler(new Vector3 (0f, 180f, 0f));
		_Hotspots.SetActive(false);
		// ToggleFocus();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowFront() {
		StartCoroutine(RotateToFront());
	}

	

	public void ToggleFocus() {
		
		if (_isFocused) {
			StartCoroutine(Focus());
			
		} else {
			StartCoroutine(Defocus());
		}
		_isFocused = !_isFocused;
	} 
	

	public void ToggleSides(int hotspotIndex) {
		
		// update hotspot content
		if (_isBack) {
			StartCoroutine(RotateToFront());
			UpdateFrontInfo(hotspotIndex);
		} else {
			Debug.Log(hotspotIndex);
			
			StartCoroutine(RotateToBack());
		}
		_isBack = !_isBack;
	}

	public void FadeOut(GameObject target, float time) {
		Debug.Log("fadeout");
        iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", 0.0f, "time", time, "easetype", "linear","onupdate", (Action<object>) (newAlpha => {
			Image[] images = target.GetComponentsInChildren<Image>();
         	foreach (Image mObj in images) {
				mObj.color = new Color(
            	mObj.color.r, mObj.color.g, 
            	mObj.color.b, (float) newAlpha);
			}
		})));
    }
     public void FadeIn(GameObject target, float time) {
         iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 1.0f, "time", time, "easetype", "linear","onupdate", (Action<object>) (newAlpha => {
			Image[] images = target.GetComponentsInChildren<Image>();
         	foreach (Image mObj in images) {
				mObj.color = new Color(
            	mObj.color.r, mObj.color.g, 
            	mObj.color.b, (float) newAlpha);
			}
		})));
     }

	void UpdateFrontInfo(int hotspotIndex) {
		// get hotspot information
		Transform Hotspots = _Back.transform.FindChild("Hotspots");
		HotspotStory storyInfo = Hotspots.GetChild(hotspotIndex).GetComponent<HotspotStory>();
		// update front page
		Text description = _Front.transform.FindChild("HotspotDescription/Text").GetComponent<Text>();
		Text coolfact = _Front.transform.FindChild("HotspotDescription/CoolFact").GetComponent<Text>();
		Image image = _Front.transform.FindChild("HotspotImage").GetComponent<Image>(); 

		description.text = storyInfo._description;
		coolfact.text = storyInfo._coolFact;
		image.sprite = storyInfo._sprite;

	}

	IEnumerator Focus() {
		_expanding = true;
		iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0.8f, 0.8f, 0.8f), "time", _ZOOM_TIME, "easetype", iTween.EaseType.easeInOutBack));
		iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(0f, 0f, 0f),"islocal", true, "time", _ZOOM_TIME, "easetype", iTween.EaseType.easeInOutBack));
		gameObject.transform.SetAsLastSibling();
		_Hotspots.SetActive(true);
		FadeIn(_Hotspots, _ZOOM_TIME);
		yield return new WaitForSeconds(_ZOOM_TIME);
		_expanding = false;
	} 

	IEnumerator Defocus() {
		_expanding = true;
		iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0.2f, 0.2f, 0.2f), "time", _ZOOM_TIME, "easetype", iTween.EaseType.easeInOutBack));
		iTween.MoveTo(gameObject, iTween.Hash("position", _initialLocation, "islocal", true, "time", _ZOOM_TIME, "easetype", iTween.EaseType.easeInOutBack));
		FadeOut(_Hotspots, _ZOOM_TIME);
		yield return new WaitForSeconds(_ZOOM_TIME);
		_expanding = false;
		_Hotspots.SetActive(false);
	} 

	IEnumerator RotateToFront() {
		_rotating = true;
		iTween.RotateTo(_Front, iTween.Hash("rotation", new Vector3(0f, 90f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeInBack));
		iTween.RotateTo(_Back, iTween.Hash("rotation", new Vector3(0f, -90f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeInBack));
		yield return new WaitForSeconds(_ROTATE_TIME);
		_Back.transform.SetAsFirstSibling();
		iTween.RotateTo(_Front, iTween.Hash("rotation", new Vector3(0f, 180f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeOutBack));
		iTween.RotateTo(_Back, iTween.Hash("rotation", new Vector3(0f, -180f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeOutBack));
		yield return new WaitForSeconds(_ROTATE_TIME);
		_rotating = false;
	}

	IEnumerator RotateToBack() {
		_rotating = true;
		iTween.RotateTo(_Front, iTween.Hash("rotation", new Vector3(0f, 90f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeInBack));
		iTween.RotateTo(_Back, iTween.Hash("rotation", new Vector3(0f, -90f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeInBack));
		yield return new WaitForSeconds(_ROTATE_TIME);
		_Front.transform.SetAsFirstSibling();
		iTween.RotateTo(_Front, iTween.Hash("rotation", new Vector3(0f, 0f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeOutBack));
		iTween.RotateTo(_Back, iTween.Hash("rotation", new Vector3(0f, 0f, 0f), "time", _ROTATE_TIME, "easetype", iTween.EaseType.easeOutBack));
		yield return new WaitForSeconds(_ROTATE_TIME);
		_rotating = false;
	}
}
