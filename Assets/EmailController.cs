using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailController : MonoBehaviour {

	public GameObject _emailer;
	public InputField _input;

	public GameObject _statusPill;

	public GameObject _successPill;

	public GameObject _sendPill;

	RectTransform _imageTransform;


	// Use this for initialization
	void Start () {
		_successPill.SetActive(false);
		_sendPill.SetActive(true);
		_statusPill.SetActive(false);
		_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		
	}

	public void ResizeScreenshot() {
		if (!_imageTransform) {
			_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		}
		Vector2 targetPosition = new Vector2(40, -100);
		Vector3 targetScale = new Vector3(0.5f, 0.5f, 1f);
		
		iTween.ValueTo(_imageTransform.gameObject, iTween.Hash(
			"from", _imageTransform.anchoredPosition,
			"to", targetPosition,
			"time", 1f,
			"onupdatetarget", gameObject, 
			"onupdate", "MoveScreenshot")
		);

		iTween.ValueTo(_imageTransform.gameObject, iTween.Hash(
			"from", _imageTransform.localScale,
			"to", targetScale,
			"time", 1f,
			"onupdatetarget", gameObject, 
			"onupdate", "ScaleScreenshot"));
	}

	public void MoveScreenshot(Vector2 position) {
		Debug.Log("move");
		_imageTransform.anchoredPosition = position;
	}

	public void ScaleScreenshot(Vector3 scale) {
		_imageTransform.localScale = scale;
	}


	public void SendEmail() {
		string emailAddress = _input.text;
        
        StartCoroutine(SendEmailAsync(emailAddress));

        // GlobalManagement.FunctionView.SetActive(true);
        // GlobalManagement.ShootButton.SetActive(true);
        // GlobalManagement.ScreenShot.SetActive(false);
        // GlobalManagement.EmailBox.SetActive(false);
	}

	IEnumerator SendEmailAsync(string emailAddress) {
		_sendPill.SetActive(false);
		_statusPill.SetActive(true);
        yield return null;
        _emailer.GetComponent<MainFunctions>().SendEmail(emailAddress);
        
    }

	public void SendEmailSuccess() {
		_successPill.SetActive(true);
		if (!_imageTransform) {
			_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		}
		_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		_imageTransform.localScale = new Vector3(1f, 1f, 1f);
		_imageTransform.anchoredPosition = new Vector2(40, -25);
	}

	public void SendEmailFail() {

	}

	public void Cancel() {
		if (!_imageTransform) {
			_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		}
		_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		_imageTransform.localScale = new Vector3(1f, 1f, 1f);
		_imageTransform.anchoredPosition = new Vector2(40, -25);

		GlobalManagement.FunctionView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(true);
        GlobalManagement.ScreenShot.SetActive(false);
        GlobalManagement.EmailBox.SetActive(false);
	}
}
