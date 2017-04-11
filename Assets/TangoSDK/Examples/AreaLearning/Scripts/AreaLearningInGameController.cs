//-----------------------------------------------------------------------
// <copyright file="AreaLearningInGameController.cs" company="Google">
//
// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Tango;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.SimpleAndroidNotifications;

/// <summary>
/// AreaLearningGUIController is responsible for the main game interaction.
/// 
/// This class also takes care of loading / save persistent data(marker), and loop closure handling.
/// </summary>
public class AreaLearningInGameController : MonoBehaviour, ITangoPose, ITangoEvent, ITangoDepth
{
    /// <summary>
    /// Prefabs of different colored markers.
    /// </summary>
    public GameObject[] m_markPrefabs;

    /// <summary>
    /// The point cloud object in the scene.
    /// </summary>
    public TangoPointCloud m_pointCloud;

    /// <summary>
    /// The canvas to place 2D game objects under.
    /// </summary>
    public Canvas m_canvas;

    /// <summary>
    /// The touch effect to place on taps.
    /// </summary>
    public RectTransform m_prefabTouchEffect;

    /// <summary>
    /// Saving progress UI text.
    /// </summary>
    public UnityEngine.UI.Text m_savingText;

    /// <summary>
    /// The Area Description currently loaded in the Tango Service.
    /// </summary>
    [HideInInspector]
    public AreaDescription m_curAreaDescription;

    public GameObject MarkerManager;

    public GameObject buildingSymbolPrefab;

    private GameObject buildingSymbol;

    public Material allowPlaceMat;
    public Material disallowPlaceMat;
    public Material appearMat;

    

#if UNITY_EDITOR
    /// <summary>
    /// Handles GUI text input in Editor where there is no device keyboard.
    /// If true, text input for naming new saved Area Description is displayed.
    /// </summary>
    private bool m_displayGuiTextInput;

    /// <summary>
    /// Handles GUI text input in Editor where there is no device keyboard.
    /// Contains text data for naming new saved Area Descriptions.
    /// </summary>
    private string m_guiTextInputContents;

    /// <summary>
    /// Handles GUI text input in Editor where there is no device keyboard.
    /// Indicates whether last text input was ended with confirmation or cancellation.
    /// </summary>
    private bool m_guiTextInputResult;
#endif

    /// <summary>
    /// If set, then the depth camera is on and we are waiting for the next depth update.
    /// </summary>
    private bool m_findPlaneWaitingForDepth;

    /// <summary>
    /// A reference to TangoARPoseController instance.
    /// 
    /// In this class, we need TangoARPoseController reference to get the timestamp and pose when we place a marker.
    /// The timestamp and pose is used for later loop closure position correction. 
    /// </summary>
    private TangoARPoseController m_poseController;

    /// <summary>
    /// List of markers placed in the scene.
    /// </summary>
    private List<GameObject> m_markerList = new List<GameObject>();

    /// <summary>
    /// Reference to the newly placed marker.
    /// </summary>
    private GameObject newMarkObject = null;

    /// <summary>
    /// Current marker type.
    /// </summary>
    private int m_currentMarkType = (int) Configs.MarkerType.Marker;

    /// <summary>
    /// If set, this is the selected marker.
    /// </summary>
    private ARMarker m_selectedMarker;

    /// <summary>
    /// If set, this is the rectangle bounding the selected marker.
    /// </summary>
    private Rect m_selectedRect;

    /// <summary>
    /// If the interaction is initialized.
    /// 
    /// Note that the initialization is triggered by the relocalization event. We don't want user to place object before
    /// the device is relocalized.
    /// </summary>
    private bool m_initialized = false;

    /// <summary>
    /// A reference to TangoApplication instance.
    /// </summary>
    private TangoApplication m_tangoApplication;

    private Thread m_saveThread;

    private bool _isLookingForPlane;

    private bool _isPlacingBuilding;

    private Vector3 _planeCenter;

    private GameObject _selectedMarker;

    // how building appears
    private int _appearMode;
    


    /// <summary>
    /// Unity Start function.
    /// 
    /// We find and assign pose controller and tango application, and register this class to callback events.
    /// </summary>
    public void Start()
    {
        Debug.Log("In Game Controller Online");
        m_poseController = FindObjectOfType<TangoARPoseController>();
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        m_markPrefabs = MarkerManager.GetComponent<MarkerManager>().GetMarkerModels();
        _appearMode = Configs.appearMode;
        
        if (m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
        }
    }

    // set material of a structured gameobject (gameobject with multiple child and renderer)
    private void SetMaterial<T> (GameObject obj, Material mat) where T: Renderer 
    {
        foreach (T m in obj.GetComponentsInChildren<T>()) {
            Material[] mats = new Material[m.materials.Length];
            for (int j = 0; j < m.materials.Length; j++) {
                mats[j] = mat; 
            }
            m.materials = mats;
        }
    }

    private void SetRendererActive<T> (GameObject obj, bool isActive) where T: Renderer  {
        foreach (T m in obj.GetComponentsInChildren<T>()) {
             m.enabled = isActive;
        }
    }

    /// <summary>
    /// Unity Update function.
    /// 
    /// Mainly handle the touch event and place mark in place.
    /// </summary>

    IEnumerator _BuildingAppearEffect(GameObject buildingSymbol) {
        
        if (_appearMode == (int) Configs.AppearMode.Grow) {
            Debug.Log("Start placing buildings");
            _isPlacingBuilding = true;
            SetMaterial<MeshRenderer>(buildingSymbol, appearMat);
            float w = 0;
            while (w < 1) {
                foreach (MeshRenderer m in buildingSymbol.GetComponentsInChildren<MeshRenderer>()) {
                    m.material.SetFloat("_Offset", w);
                }
                w = w + Time.deltaTime * 0.3f;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        Destroy(buildingSymbol);
        buildingSymbol = null;
        SetRendererActive<MeshRenderer>(newMarkObject, true);
        SetRendererActive<SkinnedMeshRenderer>(newMarkObject, true);
        _isPlacingBuilding = false;
        Debug.Log("End placing buildings");


    }

    

    public void Update()
    {
        if (m_saveThread != null && m_saveThread.ThreadState != ThreadState.Running)
        {
            // After saving an Area Description or mark data, we reload the scene to restart the game.
            _UpdateMarkersForLoopClosures();
            _SaveMarkerToDisk();
            #pragma warning disable 618
            Application.LoadLevel(Application.loadedLevel);
            #pragma warning restore 618
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            #pragma warning disable 618
            Application.LoadLevel(Application.loadedLevel);
            #pragma warning restore 618
        }


        if (!m_initialized)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(0) || GUIUtility.hotControl != 0)
        {
            return;
        }


        if (GlobalManagement.SceneIndex == (int) Configs.SceneIndex.Building) {

            
            // control input
            if (Input.touchCount == 1) {

                if (_isPlacingBuilding) {
                    Debug.Log("Placing Building, ignore touch");
                    return;
                }

                
                Touch t = Input.GetTouch(0);
                Vector2 guiPosition = new Vector2(t.position.x, Screen.height - t.position.y);
                Camera cam = Camera.main;
                RaycastHit hitInfo;
                if (t.phase != TouchPhase.Began)
                {
                    return;
                }
                if (m_selectedRect.Contains(guiPosition))
                {
                // do nothing, the button will handle it
                    Debug.Log("Touch on gui objects");
                } else if (Physics.Raycast(cam.ScreenPointToRay(t.position), out hitInfo)) {
                // Found a marker, select it (so long as it isn't disappearing)!
                    Debug.Log("Touch on existing object");
                } else if (GlobalManagement.Building == null) {
                    GameObject ObjectToInstant;
                    SetCurrentMarkType((int) Configs.MarkerType.Building);
                    ObjectToInstant = MarkerManager.GetComponent<MarkerManager>().GetBuildingModel();
                    // _planeCenter will be zero if no plane found
                    if (_planeCenter != Vector3.zero) {
                        Debug.Log("Plane is good and placing buildings");
                        _InstantiateBuilding(ObjectToInstant, _planeCenter);
                        // add into global management
                        GlobalManagement.Building = newMarkObject;
                        // destroy guide lines and transparent symbol
                        GlobalManagement.GuidingLine.SetActive(false);
                        Debug.Log("Guiding Line Status: " + GlobalManagement.GuidingLine.activeSelf);
                        // show instruction
                        GlobalManagement.ShootButton.transform.GetChild(0).gameObject.SetActive(false);
                        GlobalManagement.ShootButton.transform.GetChild(1).gameObject.SetActive(true);
                        StartCoroutine(_BuildingAppearEffect(buildingSymbol));

                    }
                    
                }
                return;
            }

            // display
            if (GlobalManagement.Building == null) {
                if (buildingSymbol == null) {
                    Debug.Log("create Building Symbol");
                    Debug.Log(_isLookingForPlane);
                    buildingSymbol = Instantiate(MarkerManager.GetComponent<MarkerManager>().GetBuildingModel()) as GameObject;
                    if (_appearMode == (int) Configs.AppearMode.Grow) {
                        SetMaterial<MeshRenderer>(buildingSymbol, disallowPlaceMat);
                    }
                    
                    SetRendererActive<MeshRenderer>(buildingSymbol, true);
                    StartCoroutine(_WaitForDepthAndFindPlane());
                }

                if (!_isLookingForPlane) {
                    StartCoroutine(_WaitForDepthAndFindPlane());
                }
            }

            return;    
        }

        if (GlobalManagement.SceneIndex == (int) Configs.SceneIndex.Landing) {

            // remove building symbol
            if (buildingSymbol != null) {
                Destroy(buildingSymbol);
                buildingSymbol = null;
                GlobalManagement.Building = null;
                Debug.Log("Destroy building symbol");
            }
            
            if (Input.touchCount == 1) {
                Touch t = Input.GetTouch(0);
                Vector2 guiPosition = new Vector2(t.position.x, Screen.height - t.position.y);
                Camera cam = Camera.main;
                RaycastHit hitInfo;
                
                if (t.phase != TouchPhase.Began)
                {
                    return;
                }
                if (m_selectedRect.Contains(guiPosition))
                {
                // do nothing, the button will handle it
                } else if (Physics.Raycast(cam.ScreenPointToRay(t.position), out hitInfo)) {
                    
                    
                } else {
                    if (GlobalManagement.developerMode) {
                        Debug.Log("find plane...");
                        StartCoroutine(_WaitForDepthAndFindPlane(t.position));
                    }
                }

                // animation effect
                RectTransform touchEffectRectTransform = Instantiate(m_prefabTouchEffect) as RectTransform;
                touchEffectRectTransform.transform.SetParent(m_canvas.transform, false);
                Vector2 normalizedPosition = t.position;
                normalizedPosition.x /= Screen.width;
                normalizedPosition.y /= Screen.height;
                touchEffectRectTransform.anchorMin = touchEffectRectTransform.anchorMax = normalizedPosition;
            }
            foreach (GameObject m in m_markerList) {
                if(m.GetComponent<recognize>().seen ) {
                    MarkerManager.GetComponent<MarkerManager>().Refresh(m.GetComponent<ARMarker>().GetID());
                    _selectedMarker = m;
                    GlobalManagement.Content.SetActive(true);
                    return;
                }
            }
            // no marker being seen, set menu to inactive
            if (GlobalManagement.Content.activeSelf) {
                GlobalManagement.Content.SetActive(false);
            }

            
            return;
        }


    }

    /// <summary>
    /// Application onPause / onResume callback.
    /// </summary>
    /// <param name="pauseStatus"><c>true</c> if the application about to pause, otherwise <c>false</c>.</param>
    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && m_initialized)
        {
            // When application is backgrounded, we reload the level because the Tango Service is disconected. All
            // learned area and placed marker should be discarded as they are not saved.
            #pragma warning disable 618
            Application.LoadLevel(Application.loadedLevel);
            #pragma warning restore 618
        }
    }

    /// <summary>
    /// Unity OnGUI function.
    /// 
    /// Mainly for removing markers.
    /// </summary>
    public void OnGUI()
    {
        if (m_selectedMarker != null)
        {
            Renderer selectedRenderer = m_selectedMarker.GetComponent<Renderer>();
            
            // GUI's Y is flipped from the mouse's Y
            Rect screenRect = _WorldBoundsToScreen(Camera.main, selectedRenderer.bounds);
            float yMin = Screen.height - screenRect.yMin;
            float yMax = Screen.height - screenRect.yMax;
            screenRect.yMin = Mathf.Min(yMin, yMax);
            screenRect.yMax = Mathf.Max(yMin, yMax);
            
            if (GUI.Button(screenRect, "<size=30>Hide</size>"))
            {
                m_markerList.Remove(m_selectedMarker.gameObject);
                m_selectedMarker.SendMessage("Hide");
                m_selectedMarker = null;
                m_selectedRect = new Rect();
            }
            else
            {
                m_selectedRect = screenRect;
            }
        }
        else
        {
            m_selectedRect = new Rect();
        }

#if UNITY_EDITOR
        // Handle text input when there is no device keyboard in the editor.
        if (m_displayGuiTextInput)
        {
            Rect textBoxRect = new Rect(100,
                                        Screen.height - 200,
                                        Screen.width - 200,
                                        100);

            Rect okButtonRect = textBoxRect;
            okButtonRect.y += 100;
            okButtonRect.width /= 2;

            Rect cancelButtonRect = okButtonRect;
            cancelButtonRect.x = textBoxRect.center.x;

            GUI.SetNextControlName("TextField");
            GUIStyle customTextFieldStyle = new GUIStyle(GUI.skin.textField);
            customTextFieldStyle.alignment = TextAnchor.MiddleCenter;
            m_guiTextInputContents = 
                GUI.TextField(textBoxRect, m_guiTextInputContents, customTextFieldStyle);
            GUI.FocusControl("TextField");

            if (GUI.Button(okButtonRect, "OK")
                || (Event.current.type == EventType.keyDown && Event.current.character == '\n'))
            {
                m_displayGuiTextInput = false;
                m_guiTextInputResult = true;
            }
            else if (GUI.Button(cancelButtonRect, "Cancel"))
            {
                m_displayGuiTextInput = false;
                m_guiTextInputResult = false;
            }
        }
#endif
    }

    /// <summary>
    /// Set the marker type.
    /// </summary>
    /// <param name="type">Marker type.</param>
    public void SetCurrentMarkType(int type)
    {
        if (type != m_currentMarkType)
        {
            m_currentMarkType = type;
            Debug.Log("Set current marker to " + type.ToString());
        }
    }

    /// <summary>
    /// Save the game.
    /// 
    /// Save will trigger 3 things:
    /// 
    /// 1. Save the Area Description if the learning mode is on.
    /// 2. Bundle adjustment for all marker positions, please see _UpdateMarkersForLoopClosures() function header for 
    ///     more details.
    /// 3. Save all markers to xml, save the Area Description if the learning mode is on.
    /// 4. Reload the scene.
    /// </summary>
    public void Save()
    {
        StartCoroutine(_DoSaveCurrentAreaDescription());
    }

    public void AdjustMarker(int mode) {

        float distance = 0.01f;
        float newScale;

        switch(mode) {
            case (int) Configs.AdjustMode.Forward:
                _selectedMarker.transform.Translate(Vector3.forward * distance, Camera.main.transform);
                _selectedMarker.transform.SetParent(null, true);
                break;
            case (int) Configs.AdjustMode.Backward:
                _selectedMarker.transform.Translate(Vector3.back * distance, Camera.main.transform);
                _selectedMarker.transform.SetParent(null, true);
                break;
            case (int) Configs.AdjustMode.Left:
                _selectedMarker.transform.Translate(Vector3.left * distance, Camera.main.transform);
                _selectedMarker.transform.SetParent(null, true);
                break;
            case (int) Configs.AdjustMode.Right:
                _selectedMarker.transform.Translate(Vector3.right * distance, Camera.main.transform);
                _selectedMarker.transform.SetParent(null, true);
                break;
            case (int) Configs.AdjustMode.Up:
                _selectedMarker.transform.Translate(Vector3.up * distance, Camera.main.transform);
                _selectedMarker.transform.SetParent(null, true);
                break;
            case (int) Configs.AdjustMode.Down:
                _selectedMarker.transform.Translate(Vector3.down * distance, Camera.main.transform);
                _selectedMarker.transform.SetParent(null, true);
                break;
            case (int) Configs.AdjustMode.ScaleUp:
                newScale = _selectedMarker.transform.localScale.x + 0.1f;
                _selectedMarker.transform.localScale = new Vector3(newScale, newScale, newScale);
                break;
            case (int) Configs.AdjustMode.ScaleDown:
                newScale = _selectedMarker.transform.localScale.x - 0.1f;
                _selectedMarker.transform.localScale = new Vector3(newScale, newScale, newScale);
                break;


        }
    }

    /// <summary>
    /// This is called each time a Tango event happens.
    /// </summary>
    /// <param name="tangoEvent">Tango event.</param>
    public void OnTangoEventAvailableEventHandler(Tango.TangoEvent tangoEvent)
    {
        // We will not have the saving progress when the learning mode is off.
        if (!m_tangoApplication.m_areaDescriptionLearningMode)
        {
            return;
        }

        if (tangoEvent.type == TangoEnums.TangoEventType.TANGO_EVENT_AREA_LEARNING
            && tangoEvent.event_key == "AreaDescriptionSaveProgress")
        {
            m_savingText.text = "Saving. " + (float.Parse(tangoEvent.event_value) * 100) + "%";
        }
    }

    /// <summary>
    /// OnTangoPoseAvailable event from Tango.
    /// 
    /// In this function, we only listen to the Start-Of-Service with respect to Area-Description frame pair. This pair
    /// indicates a relocalization or loop closure event happened, base on that, we either start the initialize the
    /// interaction or do a bundle adjustment for all marker position.
    /// </summary>
    /// <param name="poseData">Returned pose data from TangoService.</param>
    public void OnTangoPoseAvailable(Tango.TangoPoseData poseData)
    {
        // This frame pair's callback indicates that a loop closure or relocalization has happened. 
        //
        // When learning mode is on, this callback indicates the loop closure event. Loop closure will happen when the
        // system recognizes a pre-visited area, the loop closure operation will correct the previously saved pose 
        // to achieve more accurate result. (pose can be queried through GetPoseAtTime based on previously saved
        // timestamp).
        // Loop closure definition: https://en.wikipedia.org/wiki/Simultaneous_localization_and_mapping#Loop_closure
        //
        // When learning mode is off, and an Area Description is loaded, this callback indicates a
        // relocalization event. Relocalization is when the device finds out where it is with respect to the loaded
        // Area Description. In our case, when the device is relocalized, the markers will be loaded because we
        // know the relatvie device location to the markers.
        if (poseData.framePair.baseFrame == 
            TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION &&
            poseData.framePair.targetFrame ==
            TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE &&
            poseData.status_code == TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID)
        {
            // When we get the first loop closure/ relocalization event, we initialized all the in-game interactions.
            if (!m_initialized)
            {
                m_initialized = true;
                if (m_curAreaDescription == null)
                {
                    Debug.Log("AndroidInGameController.OnTangoPoseAvailable(): m_curAreaDescription is null");
                    return;
                }

                _LoadMarkerFromDisk();
            }
        }
    }

    /// <summary>
    /// This is called each time new depth data is available.
    /// 
    /// On the Tango tablet, the depth callback occurs at 5 Hz.
    /// </summary>
    /// <param name="tangoDepth">Tango depth.</param>
    public void OnTangoDepthAvailable(TangoUnityDepth tangoDepth)
    {
        // Don't handle depth here because the PointCloud may not have been updated yet.  Just
        // tell the coroutine it can continue.
        m_findPlaneWaitingForDepth = false;
    }

    /// <summary>
    /// Actually do the Area Description save.
    /// </summary>
    /// <returns>Coroutine IEnumerator.</returns>
    private IEnumerator _DoSaveCurrentAreaDescription()
    {
#if UNITY_EDITOR
        // Work around lack of on-screen keyboard in editor:
        if (m_displayGuiTextInput || m_saveThread != null)
        {
            yield break;
        }

        m_displayGuiTextInput = true;
        m_guiTextInputContents = "Unnamed";
        while (m_displayGuiTextInput)
        {
            yield return null;
        }

        bool saveConfirmed = m_guiTextInputResult;
#else
        if (TouchScreenKeyboard.visible || m_saveThread != null)
        {
            yield break;
        }
        
        TouchScreenKeyboard kb = TouchScreenKeyboard.Open("Unnamed");
        while (!kb.done && !kb.wasCanceled)
        {
            yield return null;
        }

        bool saveConfirmed = kb.done;
#endif
        if (saveConfirmed)
        {
            // Disable interaction before saving.
            m_initialized = false;
            m_savingText.gameObject.SetActive(true);
            if (m_tangoApplication.m_areaDescriptionLearningMode)
            {
                m_saveThread = new Thread(delegate()
                {
                    // Start saving process in another thread.
                    m_curAreaDescription = AreaDescription.SaveCurrent();
                    AreaDescription.Metadata metadata = m_curAreaDescription.GetMetadata();
#if UNITY_EDITOR
                    metadata.m_name = m_guiTextInputContents;
#else
                    metadata.m_name = kb.text;
#endif
                    m_curAreaDescription.SaveMetadata(metadata);
                });
                m_saveThread.Start();
            }
            else
            {
                _SaveMarkerToDisk();
                #pragma warning disable 618
                Application.LoadLevel(Application.loadedLevel);
                #pragma warning restore 618
            }
        }
    }

    /// <summary>
    /// Correct all saved marks when loop closure happens.
    /// 
    /// When Tango Service is in learning mode, the drift will accumulate overtime, but when the system sees a
    /// preexisting area, it will do a operation to correct all previously saved poses
    /// (the pose you can query with GetPoseAtTime). This operation is called loop closure. When loop closure happens,
    /// we will need to re-query all previously saved marker position in order to achieve the best result.
    /// This function is doing the querying job based on timestamp.
    /// </summary>
    private void _UpdateMarkersForLoopClosures()
    {
        // Adjust mark's position each time we have a loop closure detected.
        foreach (GameObject obj in m_markerList)
        {
            ARMarker tempMarker = obj.GetComponent<ARMarker>();
            if (tempMarker.m_timestamp != -1.0f)
            {
                TangoCoordinateFramePair pair;
                TangoPoseData relocalizedPose = new TangoPoseData();

                pair.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION;
                pair.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;
                PoseProvider.GetPoseAtTime(relocalizedPose, tempMarker.m_timestamp, pair);

                Matrix4x4 uwTDevice = m_poseController.m_uwTss
                                      * relocalizedPose.ToMatrix4x4()
                                      * m_poseController.m_dTuc;

                Matrix4x4 uwTMarker = uwTDevice * tempMarker.m_deviceTMarker;

                obj.transform.position = uwTMarker.GetColumn(3);
                obj.transform.rotation = Quaternion.LookRotation(uwTMarker.GetColumn(2), uwTMarker.GetColumn(1));
            }
        }
    }

    /// <summary>
    /// Write marker list to an xml file stored in application storage.
    /// </summary>
    private void _SaveMarkerToDisk()
    {
        // Compose a XML data list.
        List<MarkerData> xmlDataList = new List<MarkerData>();
        foreach (GameObject obj in m_markerList)
        {
            // Add marks data to the list, we intentionally didn't add the timestamp, because the timestamp will not be
            // useful when the next time Tango Service is connected. The timestamp is only used for loop closure pose
            // correction in current Tango connection.
            MarkerData temp = new MarkerData();
            temp.m_type = obj.GetComponent<ARMarker>().m_type;
            temp.m_position = obj.transform.position;
            temp.m_orientation = obj.transform.rotation;
            xmlDataList.Add(temp);
        }

        string path = Application.persistentDataPath + "/" + m_curAreaDescription.m_uuid + ".xml";
        var serializer = new XmlSerializer(typeof(List<MarkerData>));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, xmlDataList);
        }
    }

    /// <summary>
    /// Load marker list xml from application storage.
    /// </summary>
    private void _LoadMarkerFromDisk()
    {
        // Attempt to load the exsiting markers from storage.
        string path = Application.persistentDataPath + "/" + m_curAreaDescription.m_uuid + ".xml";

        var serializer = new XmlSerializer(typeof(List<MarkerData>));
        var stream = new FileStream(path, FileMode.Open);

        List<MarkerData> xmlDataList = serializer.Deserialize(stream) as List<MarkerData>;

        if (xmlDataList == null)
        {
            Debug.Log("AndroidInGameController._LoadMarkerFromDisk(): xmlDataList is null");
            return;
        }

        m_markerList.Clear();

       

        for (int i = 0; i < xmlDataList.Count; i++)
        {
            MarkerData mark = xmlDataList[i];
            // Instantiate all markers' gameobject.
            GameObject temp = Instantiate(m_markPrefabs[i],
                                          mark.m_position,
                                          mark.m_orientation) as GameObject;
            // Set marker ID
            temp.GetComponent<ARMarker>().SetID(i);
            temp.GetComponent<recognize>().enabled = true;
            
            
            m_markerList.Add(temp);
        }

    }

    /// <summary>
    /// Convert a 3D bounding box represented by a <c>Bounds</c> object into a 2D 
    /// rectangle represented by a <c>Rect</c> object.
    /// </summary>
    /// <returns>The 2D rectangle in Screen coordinates.</returns>
    /// <param name="cam">Camera to use.</param>
    /// <param name="bounds">3D bounding box.</param>
    private Rect _WorldBoundsToScreen(Camera cam, Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        Bounds screenBounds = new Bounds(cam.WorldToScreenPoint(center), Vector3.zero);
        
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, +extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, +extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, -extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(+extents.x, -extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, +extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, +extents.y, -extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, -extents.y, +extents.z)));
        screenBounds.Encapsulate(cam.WorldToScreenPoint(center + new Vector3(-extents.x, -extents.y, -extents.z)));
        return Rect.MinMaxRect(screenBounds.min.x, screenBounds.min.y, screenBounds.max.x, screenBounds.max.y);
    }

    private void _SetUpARScript(GameObject obj) {
        ARMarker markerScript = obj.GetComponent<ARMarker>();

        markerScript.m_type = m_currentMarkType;
        markerScript.m_timestamp = (float)m_poseController.m_poseTimestamp;
        
        Matrix4x4 uwTDevice = Matrix4x4.TRS(m_poseController.m_tangoPosition,
                                            m_poseController.m_tangoRotation,
                                            Vector3.one);
        Matrix4x4 uwTMarker = Matrix4x4.TRS(obj.transform.position,
                                            obj.transform.rotation,
                                            Vector3.one);
        markerScript.m_deviceTMarker = Matrix4x4.Inverse(uwTDevice) * uwTMarker;
    }


    private void _InstantiateBuilding (GameObject ObjectToInstant, Vector3 planeCenter) {

        Vector3 buildingPos;

        if (_appearMode == (int) Configs.AppearMode.Float) {
            buildingPos = planeCenter + new Vector3(0, 1.5f, 0);
        } else {
            buildingPos = planeCenter;
        }
        
        newMarkObject = Instantiate(ObjectToInstant, buildingPos, Quaternion.identity) as GameObject;
        
        _SetUpARScript(newMarkObject);

        newMarkObject.GetComponent<manipulate>().enabled = true;

        m_selectedMarker = null;
    }

    private void _PlaceMarker(GameObject ObjectToInstant, Vector3 planeCenter, Vector3 forward, Vector3 up) {
        Debug.Log("Placing Marker");
        newMarkObject = Instantiate(ObjectToInstant,
                                    planeCenter,
                                    Quaternion.identity) as GameObject;

        _SetUpARScript(newMarkObject);
        
        newMarkObject.GetComponent<ARMarker>().SetID(m_markerList.Count);
        newMarkObject.GetComponent<recognize>().enabled = true;

        m_markerList.Add(newMarkObject);
        GlobalManagement.Markers = m_markerList;
            
        m_selectedMarker = null;

       
    }

    private IEnumerator _WaitForDepthAndFindPlane() {
        // start of the process, only one process at a time

        _isLookingForPlane = true;
        
        m_findPlaneWaitingForDepth = true;
    
        // Turn on the camera and wait for a single depth update.
        m_tangoApplication.SetDepthCameraRate(TangoEnums.TangoDepthCameraRate.MAXIMUM);
        while (m_findPlaneWaitingForDepth)
        {
            yield return null;
        }

        m_tangoApplication.SetDepthCameraRate(TangoEnums.TangoDepthCameraRate.DISABLED);
        
        // Find the plane.
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;
        bool hasPlane = m_pointCloud.FindPlane(cam, new Vector2(cam.pixelWidth/2, cam.pixelHeight/2), out planeCenter, out plane);
        GameObject GuidingLine = GlobalManagement.GuidingLine;

        if (GlobalManagement.SceneIndex == (int) Configs.SceneIndex.Building && !_isPlacingBuilding) {
            if (hasPlane && plane.normal.y < 1.0f && plane.normal.y > 0.95f)
            {
                

                if (_appearMode == (int) Configs.AppearMode.Float) {
                    buildingSymbol.transform.position = planeCenter + new Vector3(0, 1.5f, 0);
                } else {
                    buildingSymbol.transform.position = planeCenter;
                }
                
                if (_appearMode == (int) Configs.AppearMode.Grow) {
                    SetMaterial<MeshRenderer>(buildingSymbol, allowPlaceMat);
                    GuidingLine.GetComponent<Bezier>().controlPoints = new Transform[] {cam.transform, buildingSymbol.transform};
                    GuidingLine.SetActive(true);
                }

                _planeCenter = planeCenter;
                
            } else {

                buildingSymbol.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;

                if (_appearMode == (int) Configs.AppearMode.Grow) {
                    SetMaterial<MeshRenderer>(buildingSymbol, disallowPlaceMat);
                    GuidingLine.SetActive(false);
                }

                if (_appearMode == (int) Configs.AppearMode.Float) {


                }

                _planeCenter = Vector3.zero;
            }
        }

        _isLookingForPlane = false;
        
        // end of the process
        
    }

    /// <summary>
    /// Wait for the next depth update, then find the plane at the touch position.
    /// </summary>
    /// <returns>Coroutine IEnumerator.</returns>
    /// <param name="touchPosition">Touch position to find a plane at.</param>
    private IEnumerator _WaitForDepthAndFindPlane(Vector2 touchPosition)
    {
        m_findPlaneWaitingForDepth = true;
        
        // Turn on the camera and wait for a single depth update.
        m_tangoApplication.SetDepthCameraRate(TangoEnums.TangoDepthCameraRate.MAXIMUM);
        while (m_findPlaneWaitingForDepth)
        {
            yield return null;
        }

        m_tangoApplication.SetDepthCameraRate(TangoEnums.TangoDepthCameraRate.DISABLED);
        
        // Find the plane.
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;
        if (!m_pointCloud.FindPlane(cam, touchPosition, out planeCenter, out plane))
        {
            yield break;
        }
        

        GameObject ObjectToInstant;
        Vector3 forward = Vector3.zero;
        Vector3 up = Vector3.zero;

        if (m_markerList.Count < m_markPrefabs.Length)
        {
            // main scene
            
            SetCurrentMarkType((int) Configs.MarkerType.Marker);
            ObjectToInstant = m_markPrefabs[m_markerList.Count];
            _PlaceMarker(ObjectToInstant, planeCenter, forward, up);
        } 
        
    
    }

    /// <summary>
    /// Data container for marker.
    /// 
    /// Used for serializing/deserializing marker to xml.
    /// </summary>
    [System.Serializable]
    public class MarkerData
    {
        /// <summary>
        /// Marker's type.
        /// 
        /// Red, green or blue markers. In a real game scenario, this could be different game objects
        /// (e.g. banana, apple, watermelon, persimmons).
        /// </summary>
        [XmlElement("type")]
        public int m_type;
        
        /// <summary>
        /// Position of the this mark, with respect to the origin of the game world.
        /// </summary>
        [XmlElement("position")]
        public Vector3 m_position;
        
        /// <summary>
        /// Rotation of the this mark.
        /// </summary>
        [XmlElement("orientation")]
        public Quaternion m_orientation;
    }
}
