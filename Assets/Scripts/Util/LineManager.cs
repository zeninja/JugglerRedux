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
	}
	
	// Update is called once per frame
	void Update () {
		if(CheckEnabled()) {		
			Vector3 startPoint = anchor.transform.position + anchor.transform.up * distanceFromAnchor * GameManager.dragDirectionModifier;

			ConvertVectorToAngle ();
			RenderArc ();
		}
	}

	bool CheckEnabled() {
		bool active;
		active = hand.HoldingBall();
		active = active && GameManager.GetInstance().state != GameManager.GameState.gameOver;
		active = active && hand.ball != null;
		line.enabled  = active;
		return active;
	}

	void RenderArc() {
		line.positionCount = resolution + 1;
		line.SetPositions (CalculateArcArray ());
	}

	void ConvertVectorToAngle() {
		velocity = hand.throwDirection.magnitude * hand.throwForceModifier * GameManager.dragDirectionModifier;
		angle = Vector2.Angle ((Vector2)hand.transform.position, (Vector2)hand.transform.position + (Vector2)hand.throwDirection * hand.throwForceModifier);
		angle -= 90;
	}
		
	Vector3[] CalculateArcArray(){
		Vector3[] arcArray = new Vector3[resolution + 1];

		radianAngle = Mathf.Deg2Rad * angle;
		float maxDistance = (velocity * velocity * Mathf.Sin (2 * radianAngle)) / g;

		for (int i = 0; i <= resolution; i++) {
			float t = (float)i / (float)resolution;
			arcArray [i] = CalculateArcPoint (t, maxDistance);
			arcArray [i] += transform.position;
		}

		return arcArray;
	}

	Vector3 CalculateArcPoint(float t, float maxDistance) {
		float x = t * maxDistance;
		float y = x * Mathf.Tan (radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos (radianAngle) * Mathf.Cos (radianAngle)));
		return new Vector3 (x, y);
	}
}
