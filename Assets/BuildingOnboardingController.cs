using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingOnboardingController : MonoBehaviour {

	// Use this for initialization
	public GameObject _onboardingA;
	public GameObject _onboardingB;

	public GameObject _renderToggle;

	void Start () {
		_onboardingA.SetActive(true);
		_onboardingB.SetActive(false);
		_renderToggle.SetActive(false);

	}

	public void SkipOnBoarding() {
		_onboardingA.SetActive(false);
		_onboardingB.SetActive(false);
		_renderToggle.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
