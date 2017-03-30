using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SendButton : MonoBehaviour, IPointerClickHandler
{

    public GameObject MainFunction;
    public InputField InputBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        string emailAddress = InputBox.text;
        MainFunction.GetComponent<MainFunctions>().SendEmail(emailAddress);

        GlobalManagement.FunctionView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(true);

        GlobalManagement.ScreenShot.SetActive(false);

        GlobalManagement.EmailBox.SetActive(false);
    }
}
