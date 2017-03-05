using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotInformation : MonoBehaviour {

	public Camera _cam;
	public string _hotSpotName = "DefaultName";
	public string _hotSpotDescription = "Lorem ipsum";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// check the visibility of the hotspot in current cast orientation
	public bool CheckVisibility() {
		RaycastHit hit;
		float sizeFactor = 0.1f;

		Vector3[] edgePoints = new Vector3[] {
			new Vector3(transform.position.x + sizeFactor, transform.position.y + sizeFactor, transform.position.z),
			new Vector3(transform.position.x + sizeFactor, transform.position.y - sizeFactor, transform.position.z),
			new Vector3(transform.position.x - sizeFactor, transform.position.y - sizeFactor, transform.position.z),
			new Vector3(transform.position.x - sizeFactor, transform.position.y + sizeFactor, transform.position.z),
		};

		// Debug.Log(direction);
		foreach (Vector3 e in edgePoints) {
			if(Physics.Raycast(e, _cam.transform.position - e, out hit)) {
				if (hit.collider.tag != "HotSpotCamera") {
					return false;
				};
			} else {
				return false;
			}
		}

		return true;
	}
}
