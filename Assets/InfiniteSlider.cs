using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSlider : MonoBehaviour {

	float screenWidth;
	
	public int numTicks;
	public float slideSpeed;
	public float startValue;

	Vector2 mouseDelta;
	Vector2 mousePos;
	Vector2 lastPosition;
	Vector2 startPos;
	Vector2 drag;

	float value;

	// Use this for initialization
	void Start () {
		screenWidth = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {
		mousePos = Input.mousePosition;

		if(Input.GetMouseButtonDown(0)) {
			startPos = mousePos;
		}

		if (Input.GetMouseButton(0)) {
			if (Input.mousePosition.y < Screen.height / 2) {

				// mouseDelta = (Vector2)Input.mousePosition - lastPosition;
				// lastPosition = Input.mousePosition;

				drag = mousePos - startPos;

				value += EZEasings.SmoothStart3(drag.x);
			}
		}
	}
}
