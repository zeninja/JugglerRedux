using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

	[System.NonSerialized]
	public Transform anchor;
	[System.NonSerialized]
	public Hand hand;
	LineRenderer lineRenderer;

	[System.NonSerialized]
	public float throwForce;

	public float lineLengthMultiplier = 1.5f;
	public float distanceFromAnchor   =  .5f;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();

		Color lineColor = new Color(0, 255, 234, 1);

		lineRenderer.startColor = lineColor;
		lineRenderer.endColor   = lineColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (hand.HoldingBall()) {
			Vector3 startPoint = anchor.transform.position + anchor.transform.up * distanceFromAnchor;

			lineRenderer.SetPosition(0, startPoint);
			lineRenderer.SetPosition(1, startPoint + (Vector3)hand.throwDirection * lineLengthMultiplier);
			lineRenderer.enabled = true;
		} else {
			lineRenderer.enabled = false;
		}
	}
}
