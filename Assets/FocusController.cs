using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusController : MonoBehaviour {

	public Sprite _focusSprite;
	public Sprite _idleSprite;

	public bool _isFocusing;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
 		if (_isFocusing) {
			GetComponent<Image>().sprite = _focusSprite;
		} else {
			GetComponent<Image>().sprite = _idleSprite;
		} 
		
	}

	

}
