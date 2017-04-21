using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingOnboardingController : MonoBehaviour {

	// Use this for initialization
	public GameObject _onboardingA;
	public GameObject _onboardingB;

	void Start () {
		Debug.Log("building started: ");
		Debug.Log(_onboardingA.activeSelf);
		Debug.Log(_onboardingB.activeSelf);
		_onboardingA.SetActive(true);
		_onboardingB.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
