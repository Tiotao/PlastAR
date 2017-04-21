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
