using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour {

	Ball ball;
	public float squashValue;

	Vector2 velocity, acceleration;
	Vector2 lastPosition, lastVelocity;

	[System.NonSerialized]
	public Vector2 throwDirection;

	float currentTime = 0;
	float totalTime = .3f;
	float catchVelocity;

	public List<Vector2> accelerationValues = new List<Vector2>();
	public AnimationCurve curve;

	// Use this for initialization
	void Start () {
		ball = transform.GetComponentInParent<Ball> ();
		velocity = Vector2.zero;
		lastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		velocity = (Vector2)transform.position - lastPosition;
		acceleration = velocity - lastVelocity;

		lastPosition = transform.position;
		lastVelocity = velocity;

		Squash ();
	}

	void Squash() {

		RotateToFaceMovementDirection();

		if (ball.isCaught) {
			// Animate the ball wobbling while it's being caught using an animation curve
			currentTime += Time.deltaTime;
			squashValue = catchVelocity * curve.Evaluate (currentTime / totalTime);

			if (currentTime > totalTime) {
				squashValue = -throwDirection.magnitude;
				squashValue = -throwDirection.magnitude.Remap(0, 15, 0, .8f);
			}
		} else {
			squashValue = Mathf.Abs(velocity.y);
			SetCatchVelocity ();
		}

//		if (velocity.magnitude > .0001f) {
//			squashValue = velocity.magnitude;
//			SetCatchVelocity();
//		} else {
//
//		}

//		Debug.Log(velocity.magnitude/acceleration.y);

		transform.localScale = new Vector2(1 - squashValue, 1 + squashValue);

		lastPosition = transform.position;
		lastVelocity = velocity;
	}

	void SetCatchVelocity() {
		// Used to handle the catch wobble animation
		currentTime = 0;
		catchVelocity = lastVelocity.magnitude;
	}

	void RotateToFaceMovementDirection() {
		Vector3 diff = velocity;

		if((Vector3)velocity != Vector3.zero) {
			diff = velocity.normalized;
		} else {
			diff = throwDirection.normalized;
		}

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.fixedDeltaTime * 1000);

	}
}
