using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

//	[System.NonSerialized]
	public Transform anchor;
//	[System.NonSerialized]
	public Hand hand;
	LineRenderer line = new LineRenderer();

	public float velocity;
	public float angle;
	public int resolution;

	float g;
	float radianAngle;

	[System.NonSerialized]
	public float throwForce;

//	public float lineLengthMultiplier = 1.5f;
	public float distanceFromAnchor   =  .5f;

	public Color lineColor = new Color(0, 255, 234, 1);

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();

		line.startColor = lineColor;
		line.endColor   = lineColor;

		g = Mathf.Abs (Physics2D.gravity.y);

		RenderArc ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hand.HoldingBall() && GameManager.GetInstance().state != GameManager.GameState.gameOver && hand.ball != null) {
			Vector3 startPoint = anchor.transform.position + anchor.transform.up * distanceFromAnchor * GameManager.throwDirection;

//			lineLengthMultiplier = hand.timedThrowForce;

//			line.SetPosition(0, startPoint);
//			line.SetPosition(1, startPoint + (Vector3)hand.throwDirection * lineLengthMultiplier * GameManager.throwDirection);

//			DrawLineArc ();

			ConvertVectorToAngle ();
			RenderArc ();
//			line.enabled = true;
		} else {
//			line.enabled = false;
		}
	}

	void DrawLineArc() {
		for (int i = 0; i < 120; i++) {
			Vector2 positionAlongArc =  new Vector2(i, .5f * Mathf.Asin( 9.81f * 120 / Mathf.Pow(hand.throwDirection.magnitude, 2)));
			line.SetPosition (i, positionAlongArc);
		}
	}

	void RenderArc() {
		line.positionCount = resolution + 1;
		line.SetPositions (CalculateArcArray ());
	}

	void ConvertVectorToAngle() {
		velocity = hand.throwDirection.magnitude * hand.throwForceModifier;
		angle = Vector2.Angle ((Vector2)hand.transform.position, (Vector2)hand.transform.position + (Vector2)hand.throwDirection * hand.throwForceModifier) - 90;
	}
		
	Vector3[] CalculateArcArray(){
		Vector3[] arcArray = new Vector3[resolution + 1];

		radianAngle = Mathf.Deg2Rad * angle;
		float maxDistance = (velocity * velocity * Mathf.Sin (2 * radianAngle)) / g;

		for (int i = 0; i <= resolution; i++) {
			float t = (float)i / (float)resolution;
			arcArray [i] = CalculateArcPoint (t, maxDistance);
		}

		return arcArray;
	}

	Vector3 CalculateArcPoint(float t, float maxDistance) {
		float x = t * maxDistance;
		float y = x * Mathf.Tan (radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos (radianAngle) * Mathf.Cos (radianAngle)));
		return new Vector3 (x, y);
	}
}
