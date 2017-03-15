using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsLoader : MonoBehaviour {

	private string url = "http://128.2.238.176:3000/assets/test";
	MarkerManager markerManager;

	void Start() {
		markerManager = GameObject.Find("MarkerManager").GetComponent<MarkerManager>();; 
	}

	// Use this for initialization
	IEnumerator LoadAssets () {
		
		Caching.CleanCache ();
		// load asset bundle from remote
		WWW www = WWW.LoadFromCacheOrDownload(url, 1);
		yield return www;
		if (www.error != null) {
			markerManager.Init();
			Debug.Log(www.error);
		} else {
			// load asset from bundle
			AssetBundle bundle = www.assetBundle;
			AssetBundleRequest request = bundle.LoadAssetAsync("Markers", typeof(GameObject));
			yield return request;
			// create cast model gameObject
			GameObject MarkersPrefab = request.asset as GameObject;
			GameObject Markers = Instantiate(MarkersPrefab) as GameObject;
			Markers.transform.parent = markerManager.gameObject.transform;
			
			markerManager.Init();
			// clear cache
			bundle.Unload(false);
			www.Dispose();
		}
		

	}


	public void Load() {
		StartCoroutine(LoadAssets());
	}

	

}
