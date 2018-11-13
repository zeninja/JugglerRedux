using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowLine : MonoBehaviour {

	public LineRenderer line;

	public float startWidth;
	public float endWidth;

	public void SetLinePositions(Vector2 start, Vector2 end) {
		Vector3[] linePositions = new Vector3[] { start, end };
		line.SetPositions(linePositions);
		line.positionCount = linePositions.Length;

		line.SetWidth(startWidth, endWidth);

	}
}
