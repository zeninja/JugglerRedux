using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

	LineManager line;

	Vector2 handPos;
	Vector2 dragStart, dragEnd;

	[System.NonSerialized]
	public Vector2 throwDirection;

	[System.NonSerialized]
	public int id;
	Touch myTouch;

	bool holdingBall;
	public GameObject ball;

	public float throwForceModifier = 4;

	Vector2 idlePos = new Vector2(0, 50);

	public static Hand instance;

	// Use this for initialization
	void Start () {
		instance = this;

		line = GetComponent<LineManager>();
		line.hand = this;
	}

	void Update() {
		if(GameManager.GetInstance().state == GameManager.GameState.gameOn) {
			throwDirection = dragEnd - dragStart;
			FindTouch();
			handPos = FindHandPos();


			#if !UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE_OSX
			ManageInput();
			#else 
			ManageTouchInput();
			#endif

			if (holdingBall) {
				ball.GetComponentInChildren<SquashAndStretch> ().throwDirection = throwDirection;
			}
		}
	}

	void FindTouch() {
		#if UNITY_IOS
		for(int i = 0; i < Input.touchCount; i++) {
			if (Input.touches[i].fingerId == id) {
				myTouch = Input.GetTouch(i);
			}
		}
		#endif
	}

	public Vector2 FindHandPos() {
		Vector2 currentHandPos;

		#if UNITY_IOS
		currentHandPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
		#endif

		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		currentHandPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		#endif

		return currentHandPos;
	}

	void ManageInput() {
		if (Input.GetMouseButton (0)) {
			if (!holdingBall) {
				dragStart = handPos;
				dragEnd = dragStart;

				transform.position = dragStart;
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
			int closestBall = -1000;
			for (int i = 0; i < balls.Length; i++) {
				if(balls[i].GetComponent<Ball>().zDepth > closestBall) { targetBall = balls [i].gameObject; }
			}
			GrabBall (targetBall);
		} else {
			GrabBall (other.gameObject);
		}
	}

	void GrabBall(GameObject targetBall) {
		// Grab the ball "closest" to the player (has the highest z depth) as long as we're not already holding a ball
		if(!holdingBall) {
			ball = targetBall;
			if (!ball.GetComponent<Ball> ().launching) {
				ball.GetComponent<Ball> ().HandleCatch ();
				line.anchor = ball.transform.GetChild(0);
				holdingBall = true;
			}
		}
	}

	void ThrowBall() {
		// Throw the ball
		if (ball != null) {
			ball.GetComponent<Ball> ().HandleThrow (throwDirection * throwForceModifier);
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
