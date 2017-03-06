using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsLoader : MonoBehaviour {

	private string url = "http://128.2.238.176:3000/assets/test";

	public RotatableSprites casts;

	public string[] assetNames;

	// Use this for initialization
	IEnumerator Start () {
		Caching.CleanCache ();
		WWW www = WWW.LoadFromCacheOrDownload(url, 1);
		yield return www;
		AssetBundle bundle = www.assetBundle;

		assetNames  = bundle.GetAllAssetNames();

		AssetBundleRequest request = bundle.LoadAssetAsync("CastModels", typeof(GameObject));

		yield return request;

		GameObject CastModels = request.asset as GameObject;

		Instantiate(CastModels);

		// foreach (HotSpotInformation h in CastModels.GetComponentsInChildren<HotSpotInformation>()) {
		// 	h._sprites = new Sprite[h._spriteCount];
		// 	for (int i = 0; i < h._spriteCount; i++) {
		// 		bundle.LoadAsset("")
		// 	}
		// }

		casts.InitializeContent();

		// bundle.Unload(false);
		// www.Dispose();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
