using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.SimpleAndroidNotifications;

public class AssetsLoader : MonoBehaviour {

	private string url = "http://128.2.238.176:3000/assets/test";
	public MarkerManager markerManager;

	void Start() {
		
	}

	// Use this for initialization
	IEnumerator LoadAssets () {
		Debug.Log("LoadData");
		Caching.CleanCache ();
		// load asset bundle from remote
		WWW www = WWW.LoadFromCacheOrDownload(url, 1);
		yield return www;
		if (www.error != null) {
			var notificationParams = new NotificationParams
                {
                    Id = UnityEngine.Random.Range(0, int.MaxValue),
                    Delay = TimeSpan.FromSeconds(5),
                    Title = "Connection Failed",
                    Message = "Message",
                    Ticker = "Ticker",
                    Sound = true,
                    Vibrate = true,
                    Light = true,
                    SmallIcon = NotificationIcon.Heart,
                    SmallIconColor = new Color(0, 0.5f, 0),
                    LargeIcon = "app_icon"
                };

            NotificationManager.SendCustom(notificationParams);
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
		// StartCoroutine(LoadAssets());
		markerManager.Init();

	}

	

}
