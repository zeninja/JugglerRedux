using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public enum BallState { InAir, BeingCaught, BeingHeld };
	public BallState state;

	Rigidbody2D rb;
	GameObject ballSprite;
	BallArtManager ballArtManager;
	DepthManager depthManager;
	BallInfo ballInfo;

	[System.NonSerialized]
	public bool launching = false;
	[System.NonSerialized]
	public bool isCaught;

	[System.NonSerialized] public BallManager ballManager;
	/*[System.NonSerialized]*/ public int zDepth = -100;

	void Awake() {
		rb = GetComponent<Rigidbody2D> ();
		ballSprite = transform.GetChild (0).gameObject;
		ballArtManager = ballSprite.GetComponent<BallArtManager> ();
		depthManager = ballSprite.GetComponent<DepthManager> ();
		ballInfo = GetComponent<BallInfo> ();
	}

	// Use this for initialization
	void Start () {
		SetDepth ();
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

	public void SetDepth() {
		depthManager.UpdateDepth (zDepth);
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
		if (other.gameObject.CompareTag("KillTrigger") && !launching) {
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

	IEnumerator Die() {
		yield return StartCoroutine(ballArtManager.CompleteExplosion());
		Destroy(gameObject);
	}
}
