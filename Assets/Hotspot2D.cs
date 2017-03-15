using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotspot2D : MonoBehaviour {

	public string _name;
	public string _description;

	public Sprite[] _sprites;

	public bool CheckVisibility(Camera cam) {
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
			if(Physics.Raycast(e, cam.transform.position - e, out hit)) {
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
