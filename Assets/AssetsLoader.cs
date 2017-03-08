using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsLoader : MonoBehaviour {

	private string url = "http://128.2.238.176:3000/assets/test";

	public RotatableSprites casts;

	// Use this for initialization
	IEnumerator Start () {
		
		Caching.CleanCache ();
		// load asset bundle from remote
		WWW www = WWW.LoadFromCacheOrDownload(url, 1);
		yield return www;
		// load asset from bundle
		AssetBundle bundle = www.assetBundle;
		AssetBundleRequest request = bundle.LoadAssetAsync("CastModels", typeof(GameObject));
		yield return request;
		// create cast model gameObject
		GameObject CastModels = request.asset as GameObject;
		Instantiate(CastModels);
		casts.InitializeContent(true);
		// clear cache
		bundle.Unload(false);
		www.Dispose();

	}
	

}
