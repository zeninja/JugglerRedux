using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {

	SpriteRenderer s;
	Color myColor;
	
	public float fadeDuration;
	public float threshold = .005f;
	public float startAlpha;

	public bool fade;
	float startScale;

	// Use this for initialization
	void Start () {
		s = GetComponent<SpriteRenderer>();
		startScale = transform.localScale.x;

		if(fade) {
			StartCoroutine(FadeOut());
		} else {
			StartCoroutine(Shrink());
		}

		// EventManager.StartListening("CleanUp", HandleDeath);
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

	IEnumerator Shrink() {
		if(fadeDuration < threshold) { Destroy(gameObject); }

		float t = 0; 
		while (t < fadeDuration) {
			t += Time.fixedDeltaTime;
			float percent = t / fadeDuration;

			transform.localScale = Vector2.one * (startScale - startScale * percent);
			yield return new WaitForFixedUpdate();
		}
		Destroy(gameObject);
	}

	void HandleDeath() {
		Destroy(gameObject);
	}
}
