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

	public float velSquashFactor = 10f;
	public float accSquashFactor = 10f;

	public float velocitySmoothing, accelerationSmoothing;

	// Use this for initialization
	void Start () {
		ball = transform.GetComponentInParent<Ball> ();
		position = transform.position;
		lastPosition = position;
		velocity = Vector2.zero;
	}

	void FixedUpdate() {
		RotateToFaceMovementDirection ();

		position = (Vector2)transform.position;
		velocity = (Vector2)transform.position - lastPosition;
		acceleration = velocity - lastVelocity;

		ApplySmoothing();

		Squash ();

		lastPosition = position;
		lastVelocity = velocity;
	}

	void Squash() {
//		squashFactor = Mathf.Sin(Mathf.Abs(velocity.y));
//		float squashValue = velSquashFactor * Mathf.Abs(velocity.y);

		float squashValue = accSquashFactor * Mathf.Abs(acceleration.y);

		transform.localScale = new Vector2(1 - squashValue, 1 + squashValue);
	}

	Vector2 sVel;
	float maxSpeed = 10f;

	void ApplySmoothing() {
		smoothedAcceleration = Vector2.SmoothDamp(smoothedAcceleration, acceleration, ref sVel, Time.fixedDeltaTime * accelerationSmoothing, maxSpeed, Time.deltaTime);
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