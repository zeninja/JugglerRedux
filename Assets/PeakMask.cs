﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakMask : MonoBehaviour {

	LineRenderer line;
	public float peakDuration;
	public float peakScale = .8f;

	void Start() {
		EventManager.StartListening("HandlePeak", DoPeak);
		line = GetComponent<LineRenderer>();
		line.startWidth = 0;
		line.endWidth   = 0;
	}

	void DoPeak() {
		StartCoroutine(Peak());
	}

	public IEnumerator Peak() {
		float t = 0;
		float d = peakDuration / 2;

		while(t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;

			line.startWidth = peakScale * EZEasings.SmoothStart3(p);
			line.endWidth   = peakScale * EZEasings.SmoothStart3(p);

			yield return new WaitForFixedUpdate();
		}

		t = 0;
		d = peakDuration / 2;

		while(t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;

			line.startWidth = peakScale * EZEasings.SmoothStart3(p);
			line.endWidth   = peakScale * EZEasings.SmoothStart3(p);

			yield return new WaitForFixedUpdate();
		}

	}
}