using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTrailDrawer : MonoBehaviour {

	public PregameTrail trailer;
	PregameTrail trailObj;

	public float duration;
	public float height;
	
	Vector2 startPos;
	public float dotScale = .08f;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		trailObj = Instantiate(trailer);
		trailObj.defaultScale = dotScale;
	}

	float t = 0;

	void FixedUpdate() {
		if (t < duration) {
			t += Time.fixedDeltaTime;
			float percent = t / duration;
			
			trailObj.transform.position = startPos + Vector2.up * height * percent;

		} else {
			t = 0;
			// Destroy(trailer);
		}
	}
}
