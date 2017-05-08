using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingOnboardingController : MonoBehaviour {

	// Use this for initialization
	public GameObject _onboardingA;
	public GameObject _onboardingB;

	public GameObject _onboardingC;

	public GameObject _renderToggle;

	void OnEnable () {
		_onboardingA.SetActive(true);
		_onboardingB.SetActive(false);
		_onboardingC.SetActive(false);
		_renderToggle.SetActive(false);

	}

	public void SkipOnBoarding() {
		_onboardingA.SetActive(false);
		_onboardingB.SetActive(false);
		_onboardingC.SetActive(false);
		_renderToggle.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
