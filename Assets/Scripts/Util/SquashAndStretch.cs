using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour {
	
	Ball ball;
	float currentSquashValue, targetSquashValue;

	[System.NonSerialized]
	public Vector2 throwDirection;

	Vector2 position, velocity, acceleration;
	Vector2 lastPosition, lastVelocity;
	Vector2 smoothedVelocity, smoothedAcceleration;

	public float squashFactor = 10f;

	public float velocitySmoothing, accelerationSmoothing;

	// Use this for initialization
	void Start () {
		ball = transform.GetComponentInParent<Ball> ();
		position = transform.position;
		lastPosition = position;
		velocity = Vector2.zero;

	}

	private Vector2 yVelocity;
	private Vector2 sVelocity;

	void FixedUpdate() {
		RotateToFaceMovementDirection ();

		position = (Vector2)transform.position;
		velocity = (Vector2)transform.position - lastPosition;

		smoothedVelocity = Vector2.SmoothDamp (smoothedVelocity, velocity, ref sVelocity, velocitySmoothing, Mathf.Infinity, Time.fixedDeltaTime);

		lastPosition = position;
		lastVelocity = velocity;

		Squash ();
	}

	void Squash() {
//		squashFactor = Mathf.Sin(Mathf.Abs(velocity.y));
		float squashValue = squashFactor * Mathf.Abs(velocity.y);
		transform.localScale = new Vector2(1 - squashValue, 1 + squashValue);
	}

	void RotateToFaceMovementDirection() {
		//Rotate the ball so that it faces the direction it's moving and can just scale X and Y to look like it's squashing properly
		Vector3 diff = velocity;

		if((Vector3)velocity != Vector3.zero) {
			diff = velocity.normalized;
		} else {
			diff = throwDirection.normalized;
		}

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}

}