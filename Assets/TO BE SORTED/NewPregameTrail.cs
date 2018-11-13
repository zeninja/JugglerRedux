using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPregameTrail : MonoBehaviour {

	public Vector2 startPos;
	public Vector2 target;

	public float height;


	// Use this for initialization
	void Start () {
		startPos = transform.position;
		target   = startPos + Vector2.up * height;

		StartCoroutine(MoveBall());
	}
	
	// Update is called once per frame
	void Update () {
		// target = startPos + Vector2.up * height;
	}

	IEnumerator MoveBall() {
		float t = 0;
		float d = 1;

		while(t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;

			target = startPos + Vector2.up * height * EZEasings.SmoothStart2(p);
			transform.position = target;
			yield return new WaitForFixedUpdate();
		}
	
		StartCoroutine(MoveBall());
	}
}
