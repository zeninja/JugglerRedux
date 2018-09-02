using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStacker : MonoBehaviour {

	public SpriteRenderer dot;

	public int circleCount = 5;
	public Extensions.Property circleRadius;


	IEnumerator StackCircles() {

		for(int i = 0; i < circleCount; i++) {
			yield return StartCoroutine(SpawnCircle(duration));
		}

	}

	public float duration;

	IEnumerator SpawnCircle(float d) {
			float t = 0;
			d = 1;

			SpriteRenderer s = Instantiate (dot);

			while(t < d) {	
				t += Time.fixedDeltaTime;
				float percent = t / d;

				s.transform.localScale = Vector2.one * GetScale(percent);

				yield return new WaitForFixedUpdate();
			}
	}

	float GetScale(float t) {
		return EZEasings.SmoothStart4(t);
	}
}
