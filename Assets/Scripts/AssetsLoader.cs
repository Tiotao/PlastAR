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

    public bool localContent = true;

    public List<string> urls;

	void Start() {
        // AccessData();
        
        if (localContent) {
            markerManager.Init();
        } else {
            AccessData();
            StartCoroutine(LoadAssets());
            
        }
		// StartCoroutine(LoadAssets("markerb"));
	}

	// Use this for initialization
	IEnumerator LoadAssets () {
		// Debug.Log(bundleName);
		// string url = "http://128.2.238.176:3000/assets/" + bundleName;
        // url = "http://128.2.238.5:6689/AssetsBundle/AssetsBundle/marker.jpg";
        if(!connStarted) {
            connStarted = true;
        }
		
        // urls.Clear();
        // urls.Add("http://128.2.238.176:3000/assets/marker");
		
		// load asset bundle from remote
        foreach(string url in urls) {
            Caching.CleanCache ();
            remainingConnection++;

            WWW www = WWW.LoadFromCacheOrDownload(url, 1);
            yield return www;
            if (www.error != null) {
                
                // markerManager.Init();
            } else {
                // load asset from bundle
                AssetBundle bundle = www.assetBundle;
                AssetBundleRequest request = bundle.LoadAssetAsync("Marker", typeof(GameObject));
                yield return request;
                // create cast model gameObject
                GameObject MarkersPrefab = request.asset as GameObject;
                GameObject Marker = Instantiate(MarkersPrefab) as GameObject;
                Marker.transform.parent = markerManager.gameObject.transform.GetChild(0).transform;
                Marker.transform.localPosition = Vector3.zero;
                Marker.transform.localRotation = Quaternion.identity;
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
        markerManager.Init();

	}

	private void AccessData()
        {
            
            Debug.Log("start access data");
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
                            
                            string url = "http://" + ipAddress + ":" + port.ToString() + "" + (string) r["Path"];
                            urls.Add(url);
                        }

                    }
                    myCon.Close();
                }

                
            }
            catch (Exception ex)
            {
                remainingConnection = 0;
                connStarted = false;
                Debug.Log(ex.Message);
                // markerManager.Init();
            }
        }


	
	public void Load() {
		// StartCoroutine(LoadAssets());
		// markerManager.Init();

	}

	

}
