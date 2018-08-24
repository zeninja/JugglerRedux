using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore : MonoBehaviour {

	Extensions.Property score;
	float lastHighscore;

	IEnumerator UpdateHighscore() {
		float t = 0;
		float d = .25f;



		while(t < d) {
			t += Time.fixedDeltaTime;
			
			yield return new WaitForFixedUpdate();

		}
	}
}
