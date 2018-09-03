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

	public static Vector3 TouchScreenToWorld(Touch t) {
		Vector3 input  = Camera.main.ScreenToWorldPoint(t.position);
		Vector3 output = new Vector3(input.x, input.y, 0);
		return output;
	}

	[System.Serializable]
	public class Property {
		public float start;
		public float end;
	}

	[System.Serializable]
	public class ColorProperty {
		public Color start;
		public Color end;
	}

	public static float GetSmoothStepRange(Property p, float t)
    {
        return p.start + (p.end - p.start) * EZEasings.SmoothStep3(t);
    }

	// Range Map
	// (in, inStart, inEnd, outStart, outEnd) 
	// {
	//	out = in - inStart // Puts in [0, inEnd - inStart]
	//  out /= (inEnd - inStart); // Puts in [0,1]
	//  out = ApplyEasing(out); // in [0,1]
	//  out *= (outEnd - outStart); // puts in [0, outRange]
	//  return out + outStart; 
	// }
	
}