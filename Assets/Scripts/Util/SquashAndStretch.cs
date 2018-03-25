using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour {

	float currentSquashValue, targetSquashValue;

	[System.NonSerialized]
	public Vector2 throwDirection;

	Vector2 position, velocity, acceleration;
	Vector2 lastPosition, lastVelocity;
	Vector2 smoothedVelocity, smoothedAcceleration;

	public bool velocityBased;
	public bool velocitySmoothed;
	public bool accelerationSmoothed;

	public float velSquashFactor = 10f;
	public float accSquashFactor = 10f;

	public float velocitySmoothing, accelerationSmoothing;

	// Use this for initialization
	void Start () {
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

//		float squashValue = accSquashFactor * Mathf.Abs(acceleration.y);

		float squashValue = 0;
//		float speed = 0;
//		float squashFactor = 1;

//		if (velocityBased) {
//			squashFactor = velSquashFactor;
//			speed = Mathf.Abs(velocity.y);
//
//			if (velocitySmoothed) {
//				smoothedVelocity = Vector2.Lerp (smoothedVelocity, velocity, Time.deltaTime * velocitySmoothing);
//				speed = Mathf.Abs(smoothedVelocity.y);
//			}
//		} else {
//			squashFactor = accSquashFactor;
//			speed = Mathf.Abs (acceleration.y);
//
//			if (accelerationSmoothed) {
//				smoothedAcceleration = Vector2.Lerp (smoothedAcceleration, acceleration, Time.deltaTime * accelerationSmoothing);
//				speed = Mathf.Abs(smoothedAcceleration.y);
//			}
//
//		}
//		squashValue = squashFactor * speed;

		// This is not too bad (but doesn't have the squashing on catch)

//		squashValue = accSquashFactor * Mathf.Abs(smoothedAcceleration.y);

		if(!GetComponentInParent<Ball>().isCaught) {
			squashValue = Mathf.Sin (Mathf.Abs(velocity.y));

			ApplySmoothing ();
			transform.localScale = new Vector2(1 - squashValue, 1 + squashValue);
		} else {
			squashValue = throwDirection.magnitude;
			squashValue = Extensions.mapRangeMinMax(0, 6f, 0, .9f, squashValue);
			transform.localScale = new Vector2(1 + squashValue, 1 - squashValue);
		}
	}

	Vector2 sVel;
	Vector2 sAcc;
	float maxSpeed = 10f;

	void ApplySmoothing() {
		smoothedVelocity = Vector2.SmoothDamp(smoothedVelocity, velocity, ref sVel, Time.fixedDeltaTime * velocitySmoothing, maxSpeed, Time.deltaTime);
		smoothedAcceleration = Vector2.SmoothDamp(smoothedAcceleration, acceleration, ref sAcc, Time.fixedDeltaTime * accelerationSmoothing, maxSpeed, Time.deltaTime);
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