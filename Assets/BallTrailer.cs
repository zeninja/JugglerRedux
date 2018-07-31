using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrailer : MonoBehaviour {

	public float shrinkRate = .05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.localScale.x > 0) {
			transform.localScale -= Vector3.one * Time.deltaTime * shrinkRate;

		}
	}
}
