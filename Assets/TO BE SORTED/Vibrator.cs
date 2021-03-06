﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrator : MonoBehaviour {

	void Update() {
		if(Input.touchCount == 1) {
			Vibrate();
		}
	}

	public static void Vibrate() {
		Handheld.Vibrate();
	}

	public static IEnumerator Vibrate(float d) {
		float t = 0;
		while (t < d) {
			t += Time.fixedDeltaTime;
			Handheld.Vibrate();
			yield return new WaitForFixedUpdate();
		}
	}
}
