using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class MessageBoxNoButton : MonoBehaviour, IPointerClickHandler
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

        GlobalManagement.MessageBox.SetActive(false);
    }
}
