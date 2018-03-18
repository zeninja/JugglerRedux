using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

//	[System.NonSerialized]
	public Transform anchor;
//	[System.NonSerialized]
	public Hand hand;
	LineRenderer line = new LineRenderer();

	[System.NonSerialized]
	public float throwForce;

	public float lineLengthMultiplier = 1.5f;
	public float distanceFromAnchor   =  .5f;

	public Color lineColor = new Color(0, 255, 234, 1);

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();

		line.startColor = lineColor;
		line.endColor   = lineColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (hand.HoldingBall() && GameManager.GetInstance().state != GameManager.GameState.gameOver) {
			Vector3 startPoint = anchor.transform.position + anchor.transform.up * distanceFromAnchor;

//			lineLengthMultiplier = hand.timedThrowForce;

			line.SetPosition(0, startPoint);
			line.SetPosition(1, startPoint + (Vector3)hand.throwDirection * lineLengthMultiplier);
			line.enabled = true;
		} else {
			line.enabled = false;
		}
	}
}
