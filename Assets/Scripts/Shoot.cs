using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class Shoot : MonoBehaviour, IPointerClickHandler
{

    private string imagePath;
    private Image image;
    private Texture2D m_Tex;

    public GameObject MainFunction;

    GameObject ScreenShotImage;
    GameObject FunctionView;
    GameObject ShootButton;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        MainFunction.GetComponent<MainFunctions>().Shoot();
    }
}
