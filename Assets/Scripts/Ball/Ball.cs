using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	
	int numCatches = 4;

	public enum BallState { Rising, Falling, Caught, GameOver };
	public BallState state;

	Rigidbody2D rb;
	BallInfo ballInfo;

	[System.NonSerialized]
	public Hand hand;

	public bool firstBall;

//	[System.NonSerialized]
	public BallArtManager ballArtManager;
//	[System.NonSerialized]
	public bool launching = false;
//	[System.NonSerialized]
	public bool isCaught;

	// Debug
	public int indexInManagerCollection;

	[System.NonSerialized] public BallManager ballManager;

	void Awake() {
		ballManager = BallManager.GetInstance ();
		rb = GetComponent<Rigidbody2D> ();
		ballArtManager = GetComponent<BallArtManager> ();
		ballInfo = GetComponent<BallInfo> ();
		rb.gravityScale = 0;
	}

	// Use this for initialization
	void Start () {
		if (!firstBall) {
			launching = true;
			rb.gravityScale = 1;
		} else {
			BallManager.GetInstance ().balls.Add (gameObject);
			rb.gravityScale = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckBallState ();
	}

	void CheckBallState() {

		if (isCaught) {
			state = BallState.Caught;
		} else {
			if (rb.velocity.y > 0) {
				state = BallState.Rising;
			} else {
				state = BallState.Falling;
				launching = false;
			}
		}
	}

	void CheckBallIndex() {
		for (int i = 0; i < ballManager.balls.Count; i++) {
			if (ballManager.balls [i] == gameObject) {
				indexInManagerCollection = ballManager.balls [i].GetComponent<BallArtManager> ().zDepth;
			}
		}
	}

	bool startGame = false;

	public void HandleCatch(Hand hand) {
		isCaught = true;

		rb.velocity = ballInfo.caughtInfo.velocity;
		rb.gravityScale = ballInfo.caughtInfo.gravityScale;

		ballArtManager.HandleCatch ();
		// GetComponent<CatchCountHandler> ().HandleCatch ();


		if (GameManager.GetInstance ().state == GameManager.GameState.gameOn) {
			ScoreManager.GetInstance ().IncreaseScore ();
			ScoreAnimation.GetInstance ().HandleCatch ();
			if (numCatches <= 0) {
				Explode ();
			}
		}

		if (firstBall) {
			firstBall = false;
			startGame = true;
		}
	}

	void Explode() {
		BallManager.GetInstance ().RemoveBall (gameObject);
		// Do a cool explosion
//		Destroy (gameObject);
	}

	public void HandleThrow(Vector2 throwForce) {
		isCaught = false;
		rb.velocity = throwForce;
		rb.gravityScale = ballInfo.defaultInfo.gravityScale;

		ballManager.UpdateBallDepths (gameObject);
		ballArtManager.HandleThrow();

		if (startGame) {
			GameManager.GetInstance ().HandleGameStart ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("KillTrigger") && !launching && GameManager.GetInstance().state != GameManager.GameState.gameOver) {
//			ballArtManager.SetGameOverColor();
			GameManager.GetInstance().HandleGameOver();
			ballManager.UpdateBallDepths (gameObject);
		}
	}

	public void FreezeBall() {
		rb.velocity = GetComponent<BallInfo> ().deadInfo.velocity;
		rb.gravityScale = GetComponent<BallInfo> ().deadInfo.gravityScale;
	}

	public void HandleDeath() {
		Debug.Log ("Handling death");
		StartCoroutine (Die());
	}

	public bool CanBeCaught() {
		return !launching && rb.velocity.y <= 0;
	}

	IEnumerator Die() {
		yield return StartCoroutine(ballArtManager.CompleteExplosion());
		Destroy(gameObject);
	}
}
