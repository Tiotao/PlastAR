using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger {

	public List<Material> _materials;

	public GameObject _obj;

	public Material _overrideMat;

	public Material _highlightMat;

	GameObject _ground;

	GameObject _animation;

	public MaterialChanger(GameObject obj, Material overrideMat, Material highlightMat) {
		this._materials = new List<Material>();
		this._obj = obj;
		this._overrideMat = overrideMat;
		this._highlightMat = highlightMat;
		Debug.Log(obj.transform.parent.transform);
		this._ground = obj.transform.parent.transform.FindChild("Ground").gameObject;
		this._animation = obj.transform.parent.transform.FindChild("Animation").gameObject;
	}

	public void Change<T>() where T: Renderer{
		Debug.Log(this._obj.GetComponentsInChildren<T>().Length);
		_ground.SetActive(false);
		_animation.SetActive(false);
		foreach (T m in this._obj.GetComponentsInChildren<T>()) {
            Material[] mats = new Material[m.materials.Length];
            for (int j = 0; j < m.materials.Length; j++) {
            	_materials.Add(m.materials[j]);
				if (m.gameObject.tag == "CastModel") {
					mats[j] = this._highlightMat;
				} else {
					mats[j] = this._overrideMat;
				}
            }
            m.materials = mats;
        }
	}

	public void Revert<T>() where T: Renderer{
		_ground.SetActive(true);
		_animation.SetActive(true);
		Debug.Log(this._obj.GetComponentsInChildren<T>().Length);
		int index = 0;
		foreach (T m in this._obj.GetComponentsInChildren<T>()) {
            Material[] mats = new Material[m.materials.Length];
            for (int j = 0; j < m.materials.Length; j++) {
            	_materials.Add(mats[j]);
				mats[j] = _materials[index++];
            }
            m.materials = mats;
        }
	}

}
