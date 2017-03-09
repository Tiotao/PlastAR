using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class BuildingClick : MonoBehaviour, IPointerClickHandler
{

    GameObject marker;
    GameObject plan;
    // Use this for initialization
    void Start () {
        plan = GlobalManagement.Content;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        marker = GlobalManagement.Marker;
        GlobalManagement.SceneIndex = 2;
        marker.SetActive(false);
        plan.SetActive(false);

    }
}
