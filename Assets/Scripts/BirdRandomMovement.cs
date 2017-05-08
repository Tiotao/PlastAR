using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdRandomMovement : MonoBehaviour {

	Vector3 randomTarget;
    public float _moveRange;
    public float _moveRangeMin = 0.5f;
    public float _moveRangeMax = 1f;
    public float _moveTimeLowRange = 0.5f;
    public float _moveTimeHighRange = 1f;

	public List<Vector3> _waypoints;

	public GameObject _routes;


    // Use this for initialization
    void Start () {
        // StartAnimation();
    }
	
	public void StartAnimation() {
		


        // transform.localPosition = new Vector3(Random.Range(-_moveRange * 2, _moveRange * 2), Random.Range(-_moveRange * 2, _moveRange * 2), Random.Range(-_moveRange * 2, 0f));
        moveCompleted();
		GlobalMovement();
	}

	// Update is called once per frame
	void Update () {
        // if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        // {
        //     _moveRange = _moveRangeMin;
        // } else
        // {
        //     _moveRange = _moveRangeMax;
        // }
        
	}

	void GlobalMovement() {
        _waypoints.Clear();
		foreach (Transform waypoint in _routes.transform) {
			_waypoints.Add(waypoint.localPosition);
            // Debug.Log(waypoint.localPosition);
		}

        
        
		float time = Random.Range(15f, 20f);
		iTween.MoveTo(transform.parent.gameObject, iTween.Hash(
            "path", _waypoints.ToArray(),
            "orienttopath", true,
            "islocal", true,
            "movetopath", false,
            "time", time,
            "oncomplete", "GlobalMovement",
            "oncompletetarget", gameObject,
            "easetype", iTween.EaseType.linear,
            "axis", "y"
        ));
	}

    void moveCompleted()
    {
        int position = Random.Range(0, 3);
        float moveTime = Random.Range(_moveTimeLowRange, _moveTimeHighRange);

        switch (position)
        {
            case 0:
                randomTarget = new Vector3(Random.Range(-_moveRange, _moveRange), Random.Range(-_moveRange / 4, _moveRange / 4), Random.Range(-_moveRange * 2, 0f));
                break;
            case 1:
                randomTarget = new Vector3(Random.Range(-_moveRange, _moveRange), Random.Range(-_moveRange / 4, _moveRange / 4), Random.Range(-_moveRange * 2, 0f));
                break;
            case 2:
                randomTarget = new Vector3(Random.Range(-_moveRange, _moveRange), Random.Range(-_moveRange / 4, _moveRange / 4), Random.Range(-_moveRange * 2, 0f));
                break;
            default:
                break;
        }
        // randomTarget = new Vector3 (Random.Range (-RANGE*2, RANGE*2), Random.Range (-RANGE, -0.5f), Random.Range (-RANGE*2, 0.2f));
        iTween.MoveTo(gameObject, iTween.Hash("position", randomTarget, "isLocal", true, "time", moveTime, "orientToPath", false, "easetype", "easeInOutSine", "oncomplete", "moveCompleted", "oncompletetarget", gameObject));
    }
}
