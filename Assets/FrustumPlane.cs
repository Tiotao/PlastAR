using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumPlane : MonoBehaviour {

	// Use this for initialization
	public Camera cam;
    private Plane[] planes;
    void Start() {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        int i = 0;
        while (i < planes.Length) {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
            i++;
        }
    }
}
