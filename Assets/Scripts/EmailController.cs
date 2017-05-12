using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

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

	AudioSource _audio;

	public const string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";


	// Use this for initialization
	void OnEnable () {
		_sendPill.SetActive(true);
		_successPill.SetActive(false);
		_statusPill.SetActive(false);
		_errorMessage.SetActive(false);
		_audio = GetComponent<AudioSource>();
		
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

	public void OnEndEdit() {
		string emailAddress = _input.text;
#if UNITY_EDITOR
		emailAddress = "tiotaocn@gmail.com";
#endif
		if (emailAddress == null || !Regex.IsMatch(emailAddress, MatchEmailPattern)) {
            SendEmailFail("Invalid Email Address.");
			_sendPill.GetComponent<Button>().interactable = false;
        } else {
			_sendPill.GetComponent<Button>().interactable = true;
		}
	}

	public void MoveScreenshot(Vector2 position) {
		_imageTransform.anchoredPosition = position;
	}

	public void ScaleScreenshot(Vector3 scale) {
		_imageTransform.localScale = scale;
	}


	public void SendEmail() {
		string emailAddress = _input.text;
#if UNITY_EDITOR
		emailAddress = "tiotaocn@gmail.com";
#endif
        StartCoroutine(SendEmailAsync(emailAddress));
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
		StartCoroutine(SuccessEffect());
	}

	IEnumerator SuccessEffect() {
		_statusPill.SetActive(false);
		_successPill.SetActive(true);
		_errorMessage.SetActive(false);
		if (!_audio.isPlaying) {
			_audio.Play();
		}
		yield return new WaitForSeconds(1f);
		ResetScreenshotTransform();
		ResumeBuildingView();
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

	}

	public void Cancel() {
		ResetScreenshotTransform();
		ResumeBuildingView();
		
		
	}

	public void ResetScreenshotTransform() {
		if (!_imageTransform) {
			_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		}

		_imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		_imageTransform.localScale = new Vector3(1f, 1f, 1f);
		_imageTransform.anchoredPosition = new Vector2(40, -25);
	}

	public void ResumeBuildingView() {
		GlobalManagement.BuildingView.SetActive(true);
		GlobalManagement.BuildingView.GetComponent<BuildingOnboardingController>().SkipOnBoarding();
		GlobalManagement.FunctionView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(true);
		GlobalManagement.ScreenShot.SetActive(false);
        GlobalManagement.EmailBox.SetActive(false);
		_errorMessage.SetActive(false);
	}
}
