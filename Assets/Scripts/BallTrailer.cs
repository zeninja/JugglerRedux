using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrailer : MonoBehaviour {

	public bool shrink;
	public float shrinkRate = .05f;

	public bool colorshift;
	public Gradient colorGradient;

	float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(shrink) {
			if(transform.localScale.x > 0) {
				transform.localScale -= Vector3.one * Time.deltaTime * shrinkRate;
			}
		}

		if(colorshift) {
			GetComponent<SpriteRenderer>().color = colorGradient.Evaluate(Time.time - startTime);
		}
	}
}
