using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class manipulate : MonoBehaviour {

    private Touch oldTouch1;
    private Touch oldTouch2;

    ParticleSystem[] hotspots;

    MaterialChanger matChanger;

    public Material overrideMat;

    public Material highlightMat;

    public bool isTransparent = false;

    public GameObject scaleSlider;

    GameObject buildingOnBoardingC;



	// Use this for initialization
	void Start () {

        buildingOnBoardingC = GlobalManagement.BuildingView.GetComponent<BuildingOnboardingController>()._onboardingC;

        highlightMat = (Material)Resources.Load("Highlight", typeof(Material));
        overrideMat  = (Material)Resources.Load("Ghost", typeof(Material));

        if (gameObject.transform.GetChild(0).FindChild("cast")) {
            gameObject.transform.GetChild(0).FindChild("cast").gameObject.tag = "CastModel";
        }
        
        hotspots = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in hotspots) {
            ParticleSystem.EmissionModule em = p.GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = true;
        }
        // Debug.Log(gameObject.transform.FindChild("BuildingModel").gameObject.transform.parent.FindChild("Ground"));
        Debug.Log(gameObject.transform.FindChild("BuildingModel").gameObject);
        Debug.Log("apply mat changer");
        Debug.Log(overrideMat);
        Debug.Log(highlightMat);
        
        matChanger = new MaterialChanger(gameObject.transform.FindChild("BuildingModel").gameObject, overrideMat, highlightMat);

    }

    

    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.R)) {
            ToggleRendering();
        }

        if (Input.touchCount <= 0)
        {
            return;
        }

        if (1 == Input.touchCount)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 deltaPos = touch.deltaPosition;

            transform.Rotate(Vector3.down * deltaPos.x, Space.World);
  
            //transform.Rotate(Vector3.right * deltaPos.y, Space.World);
        }

        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);

        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
            if (buildingOnBoardingC.activeSelf) {
                buildingOnBoardingC.SetActive(false);
            }

            return;
        }

        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

        float offset = newDistance - oldDistance;

        float scaleFactor = offset / 3000f;
        Vector3 localScale = transform.localScale;
        Vector3 scale = new Vector3(localScale.x + scaleFactor,
                                    localScale.y + scaleFactor,
                                    localScale.z + scaleFactor);

        //if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
        if (scale.x > 1f && scale.y > 1f && scale.z > 1f && scale.x < 4f && scale.y < 4f && scale.z < 4f)
        {
            transform.localScale = scale;
            

            // update slider
            scaleSlider.GetComponent<Slider>().value = scale.x;

            Debug.Log("update scale");

            
            
        }

        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
    }

    public void UpdateScale() {


        
        float scaleVal = scaleSlider.GetComponent<Slider>().value;
        Debug.Log("update scale to: " + scaleVal.ToString());
        Vector3 scale = new Vector3(scaleVal,
                                    scaleVal,
                                    scaleVal);
                                    
        //if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
        if (scaleVal > 0f)
        {
            transform.localScale = scale;
            
        }
    }

    public bool ToggleRendering() {
        Debug.Log("Toggle Rendering");
       
        
        if (isTransparent) {
            Debug.Log("Set back to texture mode");
            matChanger.Revert<MeshRenderer>();
        } else {
            Debug.Log("Set to transparent mode");
            matChanger.Change<MeshRenderer>();
        }
        isTransparent = !isTransparent;
        return isTransparent;
    }
}
