﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

	LineManager[] lines;

	Vector2 handPos;
	Vector2 dragStart, dragEnd;

	[System.NonSerialized]
	public Vector2 throwDirection;

	Touch myTouch;

	[System.NonSerialized]
	public GameObject ball;
	bool holdingBall;

	public float throwForceModifier = 4;
	Vector2 throwVelocity, currentPos, lastPos;


//	public float baseThrowModifier = 5;
//	public float timedThrowForce;
//	public float totalChargeTime = .6f;
//	public float maxThrowForce = 10;

	Vector2 idlePos = new Vector2(0, 50);

	public static Hand instance;

	// Use this for initialization
	void Start () {
		instance = this;

		lines = GetComponentsInChildren<LineManager>();
		for (int i = 0; i < lines.Length; i++) {
			lines[i].hand = this;
		}
	}

	void Update() {
		if(GameManager.GetInstance().state == GameManager.GameState.gameOn || 
		   GameManager.GetInstance().state == GameManager.GameState.mainMenu) {

		   	currentPos = (Vector2)transform.position;


			throwDirection = (dragEnd - dragStart);
//			throwDirection = (dragEnd - dragStart).normalized;
//			timedThrowForce = (Time.time - dragStartTime) / totalChargeTime;
//			timedThrowForce = Mathf.Min(timedThrowForce, 1);
//			timedThrowForce *= maxThrowForce;

			if (Input.touchCount > 0) {
				myTouch = Input.GetTouch(0);
			}
				
			#if !UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
			ManageInput();
			#else 
			ManageTouchInput();
			#endif

			handPos = FindHandPos();

			if (holdingBall && ball != null) {
				ball.GetComponentInChildren<SquashAndStretch> ().throwDirection = throwDirection;
			}

			throwVelocity = currentPos - lastPos;
			lastPos = currentPos;
		}
	}

	public Vector2 FindHandPos() {
		Vector2 currentHandPos;

		#if UNITY_EDITOR
		currentHandPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		#elif UNITY_IOS
		currentHandPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
		#endif

		return currentHandPos;
	}

//	public void ReplayFingerDown(Vector2 replayPos) {
//		if (!holdingBall) {
//			dragStart = handPos;
//			dragEnd = dragStart;
//
//			transform.position = dragStart;
//		} else {
//			dragEnd = handPos;
//		}	
//	}
//
//	public void ReplayFingerUp(Vector2 replayDir) {
//		if (holdingBall) {
//			// Throw the ball (if we're holding one) when we let go of the screen
//			throwDirection = replayDir;
//			ThrowBall ();
//		}
//		HandleDeath();
//	}


	float dragStartTime, dragEndTime;

	void ManageInput() {
		if (Input.GetMouseButton (0)) {
			if (!holdingBall) {
				dragStart = handPos;
				dragEnd = dragStart;

				transform.position = dragStart;
//				ReplayManager.GetInstance().HandleFingerDown (dragStart);
			} else {
				dragEnd = handPos;
			}
		}

		if (!Input.GetMouseButton (0)) { 
			HandleDeath ();
		}

		if (Input.GetMouseButtonUp(0)) {
			if (holdingBall) {
				// Throw the ball (if we're holding one) when we let go of the screen
				ThrowBall ();
			}
			HandleDeath();

//			ReplayManager.GetInstance().HandleFingerUp (dragEnd);
		}
	}

	void ManageTouchInput() {
		if (myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary) {
			if (!holdingBall) {
				dragStart = handPos;
				dragEnd = dragStart;

				transform.position = dragStart;
			} else {
				dragEnd = handPos;
			}
		}

		if (myTouch.phase == TouchPhase.Ended) {
			// Throw the ball (if we're holding one) when we let go of the screen
			if (holdingBall) {
				ThrowBall();
			}

			HandleDeath();
		}
	}

	// Handle Catch
	void OnTriggerEnter2D(Collider2D other) {
		int layer = 1 << LayerMask.NameToLayer ("Ball");
		Collider2D[] balls = Physics2D.OverlapCircleAll (transform.position, .5f, layer);

		GameObject targetBall = null;

		if (balls.Length > 1) {
			// Grab the ball "closest" to the player (has the highest z depth) as long as we're not already holding a ball
			int closestBall = -1000;
			for (int i = 0; i < balls.Length; i++) {
				if(balls[i].GetComponent<BallArtManager>().zDepth > closestBall) { 
					targetBall = balls [i].gameObject; 
					closestBall = targetBall.GetComponent<BallArtManager>().zDepth;
				}
			}
			GrabBall (targetBall);
//			ThrowBall();
		} else {
			GrabBall (other.gameObject);
//			ThrowBall();
		}
	}

	void GrabBall(GameObject targetBall) {
		if(!holdingBall) {
			ball = targetBall;
			if (ball.GetComponent<Ball>().CanBeCaught()) {
				ball.GetComponent<Ball> ().HandleCatch (this);

				for(int i = 0; i < lines.Length; i++) {
					lines[i].anchor = ball.transform.GetChild(0);
				}
				holdingBall = true;

				dragStartTime = Time.time;
			}
		}
	}

	void ThrowBall() {
		// Throw the ball
		if (ball != null) {
//			ball.GetComponent<Ball> ().HandleThrow (throwVelocity * throwForceModifier);
			ball.GetComponent<Ball> ().HandleThrow (throwDirection * throwForceModifier * GameManager.dragDirectionModifier);
//			ball.GetComponent<Ball> ().HandleThrow (throwDirection * timedThrowForce * baseThrowModifier);
			holdingBall = false;
			ball = null;
		}
	}

	void OnDrawGizmos() {
		if(enabled) {
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(dragStart, .5f);

			Gizmos.color = Color.red;
			Gizmos.DrawLine(dragStart, dragEnd);

			Gizmos.DrawWireSphere(dragEnd, .5f);
		}
	}

	public void HandleDeath() {
		transform.position = idlePos;
	}

	public void HandleGameOver() {
		ball = null;
		holdingBall = false;
		transform.position = idlePos;
	}

	public bool HoldingBall() {
		return holdingBall;
	}
}
