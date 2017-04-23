using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotspotShine : MonoBehaviour {


	// Use this for initialization
	void Start () {
		Shine();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Resume(){

	}
	public void Shine() {

		iTween.Stop(gameObject);
		gameObject.transform.localScale = new Vector3(1, 1, 1);
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0f,
			"to", 0.5f,
			"time", 1f,
			"onupdate", "UpdateAlpha",
			"onupdatetarget", gameObject));
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0.5f,
			"to", 0f,
			"time", 1f,
			"delay", 1f,
			"onupdate", "UpdateAlpha",
			"onupdatetarget", gameObject));
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", new Vector3(1.5f, 1.5f, 1.5f),
			"time", 2f,
			"oncomplete", "Shine",
			"oncompletetarget", gameObject
		));
	}

	void UpdateAlpha(float alpha){
		Color c = gameObject.GetComponent<Image>().color;
		gameObject.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
	}

	public void StopShine() {
		iTween.Stop(gameObject);
		gameObject.transform.localScale = new Vector3(1, 1, 1);
		Color c = gameObject.GetComponent<Image>().color;
		gameObject.GetComponent<Image>().color = new Color(c.r, c.g, c.b, 0);

		
	}
}
