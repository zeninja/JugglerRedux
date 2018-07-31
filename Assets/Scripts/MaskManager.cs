using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskManager : MonoBehaviour {

	Ball ball;
	SpriteRenderer renderer;
	SpriteMask mask;

	// Use this for initialization
	void Start () {
		enabled = false;
		ball = GetComponentInParent<Ball> ();
		renderer = GetComponent<SpriteRenderer> ();
		mask = GetComponent<SpriteMask> ();
	}
	
	// Update is called once per frame
	void Update () {
		mask.enabled = ball.GetComponent<Ball> ().state == Ball.BallState.Rising;
		renderer.enabled = !mask.enabled;
	}
}
