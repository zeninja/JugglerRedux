using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPathOutline : MonoBehaviour {

	LineRenderer[] lines;
	int sortIndex;
	public int layers = 2;

	void Start() {
		EventManager.StartListening("BallCaught", AdjustDepth);

		lines = GetComponentsInChildren<LineRenderer>();

		for(int i = 0; i < lines.Length; i++) {
			lines[i].enabled = false;
		}

		if(!NewBallManager.useRails) {
			lines[0].gameObject.SetActive(false);
			lines[1].gameObject.SetActive(false);
		}
	}

	public void DrawBallPath(Vector2 startPos, Vector2 throwVector) {
		lines[1].material.color = GetComponentInParent<NewBallArtManager>().myColor;
		Vector3[] ballPath = GetComponent<BallPredictor>().GetPositionList(startPos, throwVector).ToArray();

		// Find line width
		float width = NewBallManager.GetInstance().ballScale;
		lines[0].startWidth = width * .8f;
		lines[0].endWidth   = width * .8f;
		lines[1].startWidth = width;
		lines[1].endWidth   = width;



		for(int i = 0; i < lines.Length; i++) {

			lines[i].positionCount = ballPath.Length;
			lines[i].SetPositions(ballPath);
			lines[i].enabled = true;
		}

		lines[0].sortingOrder = 0;
		lines[1].sortingOrder = 1;
	}

	void AdjustDepth() {
		sortIndex++;
		lines[0].sortingOrder = sortIndex * layers;
		lines[1].sortingOrder = sortIndex * layers + 1;
	}

	// Called via BroadcastMessage
	void HandlePeak() {
		for(int i = 0; i < lines.Length; i++) {
			lines[i].enabled = false;
			// Debug.Log("Disabling lines via peak");
		}
	}

	void HandleBallDeath() {
		for(int i = 0; i < lines.Length; i++) {
			lines[i].enabled = false;
			// Debug.Log("Disabling lines via death");
		}
	}
}
