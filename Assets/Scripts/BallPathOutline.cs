using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPathOutline : MonoBehaviour {

	LineRenderer line, mask;
	int sortIndex;

	void Start() {
		EventManager.StartListening("BallCaught", IncrementDepth);
		mask = transform.GetChild(0).GetComponent<LineRenderer>();
		line = transform.GetChild(1).GetComponent<LineRenderer>();

		line.enabled = false;
		mask.enabled = false;

		if(!NewBallManager.useRails) {
			line.gameObject.SetActive(false);
			mask.gameObject.SetActive(false);
		}
	}

	public void DrawBallPath(Vector2 startPos, Vector2 throwVector) {
		// Set outline color
		line.material.color = GetComponentInParent<NewBallArtManager>().myColor;
		
		// Find ball path
		Vector3[] ballPath = GetComponent<BallPredictor>().GetPositionList(startPos, throwVector).ToArray();
		
		// Set line width
		float width = NewBallManager.GetInstance().ballScale;
		mask.startWidth = width * .8f;
		mask.endWidth   = width * .8f;
		line.startWidth = width;
		line.endWidth   = width;

		UpdateAndEnableLine(mask, ballPath, 0);
		UpdateAndEnableLine(line, ballPath, 1);

		ResetDepth();
	}

	// void Update() {
	// 	if(NewBallManager.useRails) {

	// 	}
	// }

	void UpdateAndEnableLine(LineRenderer line, Vector3[] path, float zDepth) {
		for(int i = 0; i < path.Length; i++) {
			path[i] = new Vector3(path[i].x, path[i].y, zDepth);
		}

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
	void HandleCatch() {
		DisableLines();
	}

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
