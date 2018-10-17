using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrail : MonoBehaviour {

	TrailRenderer trail;
	NewBall ball;

	// Use this for initialization
	void Start () {
		ball = GetComponentInParent<NewBall>();
		trail = GetComponent<TrailRenderer>();

		trail.material.color = GetComponentInParent<NewBallArtManager>().myColor;
		// trail.endColor   = GetComponentInParent<NewBallArtManager>().myColor;
		trail.startWidth = NewBallManager.GetInstance().ballScale;
		trail.endWidth   = NewBallManager.GetInstance().ballScale;

	}
	
	// Update is called once per frame
	void Update () {
		if (ball.m_State == NewBall.BallState.launching) {
			trail.enabled = true;
			trail.time = .1f;
		} else {
			trail.enabled = false;
			trail.time = 0;
		}
	}
}
