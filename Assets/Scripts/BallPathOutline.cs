using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPathOutline : MonoBehaviour {

	// LineRenderer[] lines;
	LineRenderer line, mask;
	int sortIndex;
	int layers = 2;

	void Start() {
		EventManager.StartListening("BallCaught", IncrementDepth);
		mask = transform.GetChild(0).GetComponent<LineRenderer>();
		line = transform.GetChild(1).GetComponent<LineRenderer>();

		// lines = GetComponentsInChildren<LineRenderer>();

		// for(int i = 0; i < lines.Length; i++) {
		// 	lines[i].enabled = false;
		// }

		line.enabled = false;
		mask.enabled = false;

		if(!NewBallManager.useRails) {
			line.gameObject.SetActive(false);
			line.gameObject.SetActive(false);
		}
	}

	public void DrawBallPath(Vector2 startPos, Vector2 throwVector) {
		// Set outline color
		line.material.color = GetComponentInParent<NewBallArtManager>().myColor;
		// lines[1].material.color = GetComponentInParent<NewBallArtManager>().myColor;
		
		// Find ball path
		Vector3[] ballPath = GetComponent<BallPredictor>().GetPositionList(startPos, throwVector).ToArray();

		// Set line width
		float width = NewBallManager.GetInstance().ballScale;
		mask.startWidth = width * .8f;
		mask.endWidth   = width * .8f;
		line.startWidth = width;
		line.endWidth   = width;

		UpdateAndEnableLine(mask, ballPath);
		UpdateAndEnableLine(line, ballPath);

		ResetDepth();

		// lines[0].sortingOrder = 0;
		// lines[1].sortingOrder = 1;
	}

	void UpdateAndEnableLine(LineRenderer line, Vector3[] path) {
		line.positionCount = path.Length;
		line.SetPositions(path);
		line.enabled = true;
	}

	void IncrementDepth() {
		sortIndex++;
		GetComponent<LineMaskManager>().UpdateMaskIndex(sortIndex);
	}

	void ResetDepth() {
		sortIndex = 0;
		GetComponent<LineMaskManager>().UpdateMaskIndex(sortIndex);
	}

	// Called via BroadcastMessage
	void HandlePeak() {
		DisableLines();
	}

	void HandleBallDeath() {
		DisableLines();
	}

	void DisableLines() {
		line.enabled = false;
		mask.enabled = false;
	}
}
