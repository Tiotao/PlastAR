using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancelButton: MonoBehaviour, IPointerClickHandler
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalManagement.FunctionView.SetActive(true);
        GlobalManagement.ShootButton.SetActive(true);

        GlobalManagement.ScreenShot.SetActive(false);

        GlobalManagement.EmailBox.SetActive(false);
    }
}
