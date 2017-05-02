using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

using UnityEngine;
using Assets.SimpleAndroidNotifications;

public class AssetsLoader : MonoBehaviour {
	
	public MarkerManager markerManager;

    public int remainingConnection = 0;

    public bool connStarted = false;

    public string ipAddress = "128.2.238.5";

    public int port = 6689;

	void Start() {
        AccessData();
		
		// StartCoroutine(LoadAssets("markerb"));
	}

	// Use this for initialization
	IEnumerator LoadAssets (string url) {
		// Debug.Log(bundleName);
		// string url = "http://128.2.238.176:3000/assets/" + bundleName;
        if(!connStarted) {
            connStarted = true;
        }
		remainingConnection++;
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
			// markerManager.Init();
		} else {
			// load asset from bundle
			AssetBundle bundle = www.assetBundle;
			AssetBundleRequest request = bundle.LoadAssetAsync("Marker", typeof(GameObject));
			yield return request;
			// create cast model gameObject
			GameObject MarkersPrefab = request.asset as GameObject;
			GameObject Markers = Instantiate(MarkersPrefab) as GameObject;
			Markers.transform.parent = markerManager.gameObject.transform.GetChild(0).transform;
			// markerManager.Init();
			// clear cache
			bundle.Unload(false);
			www.Dispose();
		}
		// Debug.Log(remainingConnection);
        remainingConnection--;
        if (remainingConnection == 0 && connStarted) {
            markerManager.Init();
            connStarted = false;
        }

	}

	private void AccessData()
        {

            DataTable t = new System.Data.DataTable();
            string con = "Data Source=128.2.238.5;Initial Catalog=plastar;User ID=plastar;Password=husiyuan";
            try
            {
                using (SqlConnection myCon = new SqlConnection(con))
                {
                    myCon.Open();
                    string sql = "select * from dbo.AssetsBundle";
                    using (SqlCommand cm = new SqlCommand(sql, myCon))
                    {
                        SqlDataAdapter a = new SqlDataAdapter(cm);
                        a.Fill(t);
                        Debug.Log(t.Rows.Count);
                        foreach (DataRow r in t.Rows)
                        {
                            string url = "http://" + ipAddress + ":" + port.ToString() + "/" + (string) r["Path"];
                            StartCoroutine(LoadAssets(url));
                        }

                    }
                    myCon.Close();
                }

                
            }
            catch (Exception ex)
            {
                remainingConnection = 0;
                connStarted = false;
            }
        }


	
	public void Load() {
		// StartCoroutine(LoadAssets());
		// markerManager.Init();

	}

	

}
