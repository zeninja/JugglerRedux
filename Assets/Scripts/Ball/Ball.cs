using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	Rigidbody2D rb;
	GameObject artManager;
	BallArtManager ballArtManager;
	DepthManager depthManager;
	BallInfo ballInfo;

	[System.NonSerialized]
	public bool launching = false;
	[System.NonSerialized]
	public bool isCaught;

	[System.NonSerialized] public BallManager ballManager;
	[System.NonSerialized] public int zDepth = 0;

	void Awake() {
		rb = GetComponent<Rigidbody2D> ();
		artManager = transform.GetChild (0).gameObject;
		ballArtManager = artManager.GetComponent<BallArtManager> ();
		depthManager = artManager.GetComponent<DepthManager> ();
		ballInfo = GetComponent<BallInfo> ();
	}

	// Use this for initialization
	void Start () {
		SetDepth ();
		launching = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (launching && rb.velocity.y <= .001f) {
			launching = false;
		}
	}

	public void HandleCatch() {
		isCaught = true;
		rb.velocity = ballInfo.caughtInfo.velocity;
		rb.gravityScale = ballInfo.caughtInfo.gravityScale;
		ScoreManager.GetInstance ().IncreaseScore ();

		ballManager.UpdateBallDepths (gameObject);
		ballArtManager.HandleCatch ();

		ScoreAnimation.GetInstance ().HandleCatch ();
	}

	public void SetDepth() {
		depthManager.UpdateDepth (zDepth);
	}

	public void HandleThrow(Vector2 throwForce) {
		isCaught = false;
		rb.velocity = throwForce;
		rb.gravityScale = ballInfo.defaultInfo.gravityScale;

		ballArtManager.HandleThrow();
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
