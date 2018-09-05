using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerDot : MonoBehaviour {

	public void SetAnchorPos(Vector2 anchor) {
	}

	public void SetTargetRadius(float r) {
		GetComponent<ProceduralCircle>().radius = r;
	}

	public void SetInfo(Vector2 anchor, Color dotColor, int depth) {
		GetComponent<ProceduralCircle>().anchorPos = anchor;
		GetComponent<ProceduralCircle>().color = dotColor;
		GetComponent<ProceduralCircle>().depth = depth;
	}
}
