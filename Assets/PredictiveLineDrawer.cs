using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictiveLineDrawer : MonoBehaviour {

	// public NewBall m_Ball;
	public bool drawLine;
	public LineRenderer predictiveLine;
	public SpriteMask peakPoint;

	public Color lineColor;

	// Use this for initialization
	void Start () {
		predictiveLine = GetComponent<LineRenderer>();
		predictiveLine.material.color = lineColor;
		// WarningLines.GetInstance().SetWarningMask(peakPoint);
	}
	
	// Update is called once per frame
	void Update () {
		if(NewGameManager.GameOver()) { 
			Destroy(peakPoint);
			return; 
		}
		// if(peakPoint == null) { return; }

		predictiveLine.enabled = false;
		// peakPoint.enabled      = drawLine;
	}

	public void EnableLine(bool isDrawing) {
		drawLine = isDrawing;
	}

	public void DrawLine(Vector3 anchorPos, Vector3 velocity) {
		Vector3[] positions = GetComponent<BallPredictor>().GetPositionList(anchorPos, velocity).ToArray();
		predictiveLine.positionCount = positions.Length;
		predictiveLine.SetPositions(positions);
		peakPoint.transform.position = positions[positions.Length - 1];
		// WarningLines.GetInstance().
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
