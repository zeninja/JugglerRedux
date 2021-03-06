using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAndStretch1 : MonoBehaviour {
	/*
	Ball ball;
	float currentSquashValue, targetSquashValue;

	[System.NonSerialized]
	public Vector2 throwDirection;

	//public float currentTime = 0;
	//float totalTime = .3f;
	//public float catchVelocity;

	public AnimationCurve curve;

	// Use this for initialization
	void Start () {
		ball = transform.GetComponentInParent<Ball> ();
	}

	Vector2 smoothingRef;
	public float velocitySmoothing = 10;

	// Update is called once per frame
	void FixedUpdate () {
		CalculateMovementInfo ();
		Squash ();
	}

	void CalculateMovementInfo() {
		velocity = (Vector2)transform.position - lastPosition;
		acceleration = velocity - lastVelocity;

		lastPosition = transform.position;
		lastVelocity = velocity;

		smoothedVelocity = Vector2.Lerp (smoothedVelocity, velocity, Time.fixedDeltaTime * velocitySmoothing);
	}

	void Squash() {
		RotateToFaceMovementDirection();

		/*#region previous implementation
		if (ball.isCaught) {
			// Animate the ball wobbling while it's being caught using an animation curve
			currentTime += Time.fixedDeltaTime;
			squashValue = catchVelocity * curve.Evaluate (currentTime / totalTime);

			if (currentTime > totalTime) {
				squashValue = -throwDirection.magnitude;
				squashValue = -throwDirection.magnitude.Remap(0, 15, 0, .8f);
			}

			// Math Squash
			// a * sin (bx)
			// a = amplitude (should decrease over time)
			// b = period of the wave (doesn't necessarily need to change??)

//			float progress;
//	float amount = 0;
//	float total = 1f;

//			while (amount < total) {
//				amount += Time.fixedDeltaTime;
//				progress = amount / total;
//				squashValue = (total - progress) * Mathf.Cos (progress);
//			}
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
		#endregion*/

		/*switch (ball.state) {
			case Ball.BallState.BeingCaught:
				currentTime += Time.fixedDeltaTime;
				targetSquashValue = catchVelocity * curve.Evaluate (currentTime / totalTime);

				if (currentTime > totalTime) {
					ball.SetState (Ball.BallState.BeingHeld);
				}
				break;
			case Ball.BallState.BeingHeld:
				targetSquashValue = -throwDirection.magnitude;
				targetSquashValue = -throwDirection.magnitude.Remap(0, 15, 0, .8f);
				break;
			case Ball.BallState.InAir:
				targetSquashValue = Mathf.Sin(Mathf.Abs(velocity.y));
				SetCatchVelocity ();
				break;
		}*

//		currentSquashValue = Mathf.Lerp (currentSquashValue, targetSquashValue, Time.deltaTime * 10);

		// THIS ACTUALLY SEEMS PRETTY OKAY? BUT IT GETS OUT OF HAND DURING THE HIGHER SPEEDS
		//currentSquashValue = Mathf.Pow(Mathf.Sin(velocity.magnitude) * 10, 2)/10;
//		amountToSquashBy = squashFactor * Mathf.Sin(smoothedVelocity.magnitude);

		transform.localScale = new Vector2(1 - Mathf.Abs(smoothedVelocity.y), 1 + Mathf.Abs(smoothedVelocity.y));

		lastPosition = transform.position;
		lastVelocity = velocity;
	}

	void SetCatchVelocity() {
		// Used to handle the catch wobble animation
		currentTime = 0;
		//catchVelocity = Mathf.Abs(lastVelocity.y);
	}

	void RotateToFaceMovementDirection() {
		Vector3 diff = velocity;

		if((Vector3)velocity != Vector3.zero) {
			diff = velocity.normalized;
		} else {
			diff = throwDirection.normalized;
		}

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		//transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.fixedDeltaTime * 1000);
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}
	*/
}