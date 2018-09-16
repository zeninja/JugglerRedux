using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakEffect : MonoBehaviour {

	SpriteRenderer sprite;
	SpriteMask mask;

	public void StartPeak() {
		StartCoroutine(Peak());
	}

	IEnumerator Peak() {
		float t = 0;
		float d = .125f;

		while(t < d) {
			t += Time.fixedDeltaTime;

			

			yield return new WaitForFixedUpdate();
		}
	}
}
