using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class MessageBoxButton : MonoBehaviour, IPointerClickHandler
{

	RectTransform imageTransform;
	// Use this for initialization
	void Start () {
		Debug.Log(imageTransform);
		imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		imageTransform.localScale = new Vector3(1f, 1f, 1f);
		imageTransform.anchoredPosition = new Vector2(40, -25);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalManagement.EmailBox.SetActive(true);

        GlobalManagement.EmailBox.GetComponent<EmailController>().ResizeScreenshot();

		// RectTransform imageTransform = GlobalManagement.ScreenShot.transform.GetChild(0).gameObject.GetComponent<RectTransform>();

		

		GlobalManagement.MessageBox.SetActive(false);
	}


	
}
