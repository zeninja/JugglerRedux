﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {

	SpriteRenderer s;
	Color myColor;
	
	public float fadeDuration;
	public float threshold = .005f;
	public float startAlpha;

	// Use this for initialization
	void Start () {
		s = GetComponent<SpriteRenderer>();
		StartCoroutine(FadeOut());

		EventManager.StartListening("CleanUp", HandleDeath);
	}

	IEnumerator FadeOut() {
		if(fadeDuration < threshold) { Destroy(gameObject); }

		float t = 0; 
		while (t < fadeDuration) {
			t += Time.fixedDeltaTime;

			myColor = s.color;
			myColor.a = startAlpha * (1 - EZEasings.SmoothStart3(t / fadeDuration));
			s.color = myColor;
			yield return new WaitForFixedUpdate();
		}
		Destroy(gameObject);
	}

	void HandleDeath() {
		Destroy(gameObject);
	}
}
