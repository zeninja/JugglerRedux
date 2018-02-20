using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public enum BallState { InAir, BeingCaught, BeingHeld };
	public BallState state;

	Rigidbody2D rb;
	BallInfo ballInfo;

	[System.NonSerialized]
	public BallArtManager ballArtManager;
	[System.NonSerialized]
	public bool launching = false;
	[System.NonSerialized]
	public bool isCaught;

	public int indexInManagerCollection;

	[System.NonSerialized] public BallManager ballManager;

	void Awake() {
		rb = GetComponent<Rigidbody2D> ();
		ballArtManager = GetComponent<BallArtManager> ();
		ballInfo = GetComponent<BallInfo> ();
	}

	// Use this for initialization
	void Start () {
		launching = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (launching) {
			ballArtManager.UseLaunchColor(true);

			if(rb.velocity.y <= .001f) {
				launching = false;
				ballArtManager.UseLaunchColor(false);
			}
		}

		//Debug
		CheckBallIndex();
	}

	void CheckBallIndex() {
		for (int i = 0; i < ballManager.balls.Count; i++) {
			if (ballManager.balls [i] == gameObject) {
				indexInManagerCollection = ballManager.balls [i].GetComponent<BallArtManager> ().zDepth;
			}
		}
	}

	public void SetState(BallState newState) {
		state = newState;
	}

	public void HandleCatch() {
		isCaught = true;
		rb.velocity = ballInfo.caughtInfo.velocity;
		rb.gravityScale = ballInfo.caughtInfo.gravityScale;
		ScoreManager.GetInstance ().IncreaseScore ();

		ballArtManager.HandleCatch ();
		ScoreAnimation.GetInstance ().HandleCatch ();

		SetState (BallState.BeingCaught);
	}

	public void HandleThrow(Vector2 throwForce) {
		isCaught = false;
		rb.velocity = throwForce;
		rb.gravityScale = ballInfo.defaultInfo.gravityScale;

		ballManager.UpdateBallDepths (gameObject);
		ballArtManager.HandleThrow();

		SetState (BallState.InAir);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("KillTrigger") && !launching && GameManager.GetInstance().state != GameManager.GameState.gameOver) {
			ballArtManager.SetGameOverColor();
			GameManager.GetInstance().HandleGameOver();
			ballManager.UpdateBallDepths (gameObject);
		}
	}

	public void FreezeBall() {
		rb.velocity = GetComponent<BallInfo> ().deadInfo.velocity;
		rb.gravityScale = GetComponent<BallInfo> ().deadInfo.gravityScale;
	}

	public void HandleDeath() {
		StartCoroutine (Die());
	}

	public bool CanBeCaught() {
		return !launching && rb.velocity.y < 0;
	}

	IEnumerator Die() {
		yield return StartCoroutine(ballArtManager.CompleteExplosion());
		Destroy(gameObject);
	}
}
