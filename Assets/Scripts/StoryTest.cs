using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryTest : MonoBehaviour {

	public GameObject _story;

	public GameObject _storyPrefab;

	public GameObject _postcardPrefab;

	public GameObject _hotspotPrefab;

	// Use this for initialization
	void Start () {
		InitStory();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitStory() {
		GameObject story = Instantiate(_storyPrefab);
		Postcard[] postcardsInfo = _story.GetComponentsInChildren<Postcard>();

		int postcardId = 0;

		foreach (Postcard pInfo in postcardsInfo) {

			GameObject postcard = Instantiate(_postcardPrefab);
			postcard.transform.parent = story.transform;
			postcard.transform.FindChild("Back/Content").GetComponent<Image>().sprite = pInfo._frontImage;
			postcard.transform.FindChild("Back/Description/Year").GetComponent<Text>().text = pInfo._year;
			GameObject hotspots = postcard.transform.FindChild("Back/Hotspots").gameObject;
			
			int hotspotId = 0;

			Vector2 gridPos = new Vector2((int) postcardId % 3 * 240, -100 - 180 * (int) (postcardId / 3));

			postcard.GetComponent<RectTransform>().anchoredPosition = gridPos;

			PostcardController controller = postcard.GetComponent<PostcardController>();
			
			foreach (HotspotStory hotspotInfo in pInfo.GetComponentsInChildren<HotspotStory>()) {
				
				GameObject hotspot = Instantiate(_hotspotPrefab);
				hotspot.transform.parent = hotspots.transform;
				HotspotStory content = hotspot.GetComponent<HotspotStory>();
				content._coolFact = hotspotInfo._coolFact;
				content._description = hotspotInfo._description;
				content._sprite = hotspotInfo._sprite;
				hotspot.GetComponent<RectTransform>().anchoredPosition = hotspotInfo._position;
				int tempHotspotId = hotspotId;
				hotspot.GetComponent<Button>().onClick.AddListener(()=> controller.ToggleSides(tempHotspotId));
				hotspotId++;
			}
			postcardId++;
		}
		// GameObject postcard = Instantiate(_postcardPrefab);
		// GameObject hotspots = postcard.transform.FindChild("Hotspots").gameObject;
		
	}
}
