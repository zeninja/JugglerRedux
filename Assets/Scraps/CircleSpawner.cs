using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour {

	public CircleEffect circle;

	public float minRadius;
	public float maxRadius;
	float radiusDiff;

	public float minWidthPercent = .01f;
	public float maxWidthPercent = 10;
	float widthDiff;

	public float minTime = .3f;
	public float maxTime = 1.3f;
	float timeDiff;

	[Range(0f, 1f)]
	public float percent = .05f;

	Color myColor;

	void Start() {
		myColor = GetComponentInChildren<SpriteRenderer>().color;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.C)) {
			SpawnCircle(transform.position);
		}
	}

	void SpawnCircle(Vector2 position) {
			Vector3 targetPos = position;
			targetPos.z = -1;
		
			CircleEffect f = Instantiate(circle);
			f.transform.position = targetPos;

			radiusDiff = maxRadius - minRadius;
			widthDiff  = maxWidthPercent - minWidthPercent;
			timeDiff   = maxTime - minTime;

			float circleRadius = minRadius + radiusDiff * EZEasings.SmoothStart3(percent);
			Debug.Log(radiusDiff * EZEasings.SmoothStart3(percent));
			float lineWidth    = minWidthPercent + widthDiff * EZEasings.SmoothStart3(percent);
			float duration     = minTime + timeDiff * EZEasings.SmoothStart3(percent);

			f.TriggerCircle(circleRadius, lineWidth, myColor, duration, true);
	}
}
