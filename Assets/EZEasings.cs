using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZEasings : MonoBehaviour {

	// float RangeMap(float in, float inStart, float inEnd, float outStart, float outEnd) {
		
	// }

	public static float Linear(float t) {
		return t;
	}

	// Smooth Start
	public static float SmoothStart2(float t) {
		return t*t;
	}

	public static float SmoothStart3(float t) {
		return t*t*t;
	}

	public static float SmoothStart4(float t) {
		return t*t*t*t;
	}
	
	public static float SmoothStart5(float t) {
		return t*t*t*t*t;
	}

	// Smooth Stop
	public static float SmoothStop2(float t) {
		return 1 - ((1 - t) * (1 - t));
	}
	
	public static float SmoothStop3(float t) {
		return 1 - ((1 - t) * (1 - t) * (1 - t));
	}

	public static float SmoothStop4(float t) {
		return 1 - ((1 - t) * (1 - t) * (1 - t) * (1 - t));
	}

	public static float SmoothStop5(float t) {
		return 1 - ((1 - t) * (1 - t) * (1 - t) * (1 - t) * (1 - t));
	}



	// Techniques
	public static float Square(float t) {
		return t*t;
	}

	public static float Flip(float t) {
		return 1 - t;
	}

	public static float Mix(float a, float b, float weightB, float t) {
		return (1 - weightB ) * a + ( weightB ) * b;
	}
}
