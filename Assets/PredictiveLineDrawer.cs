using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictiveLineDrawer : MonoBehaviour {

	// public NewBall m_Ball;
	public bool drawLine;
	public LineRenderer predictiveLine;
	public SpriteRenderer peakPoint;

	public Color lineColor;

	// Use this for initialization
	void Start () {
		// m_Ball = GetComponent<NewBall>();
		predictiveLine = GetComponent<LineRenderer>();

		predictiveLine.material.color = lineColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(NewGameManager.GameOver()) { return; }
		// if(peakPoint == null) { return; }

		predictiveLine.enabled = drawLine;
		peakPoint.enabled      = drawLine;
	}

	public void EnableLine(bool isDrawing) {
		Debug.Log("Enabling line");
		drawLine = isDrawing;
	}

	public void DrawLine(Vector3 anchorPos, Vector3 velocity) {
		Vector3[] positions = GetComponent<BallPredictor>().GetPositionList(anchorPos, velocity).ToArray();
		predictiveLine.positionCount = positions.Length;
		predictiveLine.SetPositions(positions);
		peakPoint.transform.position = positions[positions.Length - 1];
	}

	// Don't believe VSCode's lies
	// These are called via BroadcastMessage in NewBall
	void HandleThrow() {
		peakPoint.transform.parent = null;
	}

	void HandlePeak() {
		drawLine = false;
		// Destroy(peakPoint);
	}
}
