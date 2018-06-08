using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extensions : MonoBehaviour {

	// Args: input min, input max, map to range min, range max, number to input
	// Inputs outside a1 to a2 will fall outside the output range
	public static float mapRange(float a1, float a2, float b1, float b2, float s)
	{
		return b1 + (s - a1) * (b2 - b1) / (a2 - a1);

	}

	// Output is clamped to range: b1 to b2
	public static float mapRangeMinMax(float a1, float a2, float b1, float b2, float s)
	{
		float value =  b1 + (s - a1) * (b2 - b1) / (a2 - a1);
		value = Mathf.Clamp(value, b1, b2);
		return value;
	}

	public static Vector3 ScreenToWorld(Vector3 input) {
		input = new Vector3(input.x, input.y, Camera.main.nearClipPlane);
		Vector3 output = Camera.main.ScreenToWorldPoint(input);
		return output;
	}

	public static Vector3 MouseScreenToWorld() {
		Vector3 input = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
		Vector3 output = Camera.main.ScreenToWorldPoint(input);
		return output;
	}
}