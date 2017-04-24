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

	public static string _errorMessageContent;

	public GameObject _errorMessage;

	RectTransform _imageTransform;

	public static bool isSuccess = false;

	public static bool isFailed = false;


	// Use this for initialization
	void Start () {
		_sendPill.SetActive(true);
		_successPill.SetActive(false);
		_statusPill.SetActive(false);
		_errorMessage.SetActive(false);
		
		// _imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {
		if (!_imageTransform) {
			_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		}

		if (EmailController.isSuccess) {
			ShowSuccess ();
			EmailController.isSuccess = false;
			
		}

		if (EmailController.isFailed) {
			Debug.Log("failed");
			
			ShowFail();
			EmailController.isFailed = false;
		}
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
			"time", 0.5f,
			"onupdatetarget", gameObject, 
			"onupdate", "MoveScreenshot")
		);

		iTween.ValueTo(_imageTransform.gameObject, iTween.Hash(
			"from", _imageTransform.localScale,
			"to", targetScale,
			"time", 0.5f,
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
		
		emailAddress = "tiotaocn@gmail.com";
        
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
		EmailController.isSuccess = true;
		// Invoke("ShowSuccess", 0);
	}

	public void ShowSuccess (){
		_statusPill.SetActive(false);
		_successPill.SetActive(true);
		_errorMessage.SetActive(false);
		
	}

	public void ShowFail (){
		Debug.Log("showed failed");
		_statusPill.SetActive(false);
		_sendPill.SetActive(true);
		_errorMessage.GetComponent<Text>().text = EmailController._errorMessageContent;
		_errorMessage.SetActive(true);

	}

	public void SendEmailFail(string message) {
		Debug.Log("fail");
		EmailController.isFailed = true;
		EmailController._errorMessageContent = message;
		Debug.Log(EmailController.isFailed);
		// Invoke("ShowFail", 0);
		// if (!_imageTransform) {
		// 	_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		// }
		// _imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		// _imageTransform.localScale = new Vector3(1f, 1f, 1f);
		// _imageTransform.anchoredPosition = new Vector2(40, -25);
	}

	public void Cancel() {
		if (!_imageTransform) {
			_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		}

		_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		_imageTransform.localScale = new Vector3(1f, 1f, 1f);
		_imageTransform.anchoredPosition = new Vector2(40, -25);
		GlobalManagement.BuildingView.SetActive(true);
		GlobalManagement.BuildingView.GetComponent<BuildingOnboardingController>().SkipOnBoarding();
		GlobalManagement.FunctionView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(true);
		GlobalManagement.ScreenShot.SetActive(false);
        GlobalManagement.EmailBox.SetActive(false);
		_errorMessage.SetActive(false);
	}
}
