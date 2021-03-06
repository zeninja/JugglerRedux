using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallArtManager : MonoBehaviour {

	Ball ball;

	SpriteRenderer sprite;
	GameObject ring;

	public int zDepth = -100;

	public float explosionDuration = .5f;
	public float implosionDuration = .25f;

	public AnimationCurve explosionCurve;
	public AnimationCurve implosionCurve;

	public float explosionScale = 30;

	float startTime;

	public Color catchColor;
	public Color risingColor;
	public Color fallingColor;
	public Color gameOverColor;

//	public Color defaultColor;
//	public Color risingColor;
//	public Color catchColor;
//	public Color gameOverColor;

	// Use this for initialization
	void Start () {
		ball = GetComponent<Ball> ();
		sprite = transform.Find("Art").GetComponent<SpriteRenderer> ();
		ring = transform.Find("Art/Ring").gameObject;
		SetDepth ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateColor ();
	}

	void UpdateColor() {
		switch (ball.state) {
			case Ball.BallState.Caught:
				sprite.color = catchColor;
				break;
			case Ball.BallState.Falling:
				sprite.color = fallingColor;
				break;
			case Ball.BallState.Rising:
				sprite.color = risingColor;
				break;
			case Ball.BallState.GameOver:
				sprite.color = gameOverColor;
				break;
		}
	}

	public void SetDepth() {
		sprite.GetComponent<SpriteRenderer> ().sortingOrder = zDepth * 2;
		ring.GetComponent<SpriteRenderer> ().sortingOrder = zDepth * 2 - 1;
	}

	public void HandleCatch() {
		zDepth = 1;
		SetDepth ();
		StopCoroutine  ("FlashBall");
		StartCoroutine ("FlashBall");
	}

	IEnumerator FlashBall() {
		float flashDuration = .03f;

		sprite.enabled = false;
		sprite.color   = catchColor;
		yield return new WaitForSeconds (flashDuration);
		sprite.enabled = true;
	}

	public void HandleThrow() {
//		sprite.color = defaultColor;
	}

	public void Reset() {
		//transform.localScale = Vector3.one;
		transform.GetChild (0).gameObject.SetActive(true);
	}

	public IEnumerator CompleteExplosion() {
		Vector3 startScale = Vector3.one;
		startScale = sprite.transform.localScale;
		yield return StartCoroutine(Explode(startScale));
		BallManager.startNextExplosion = true;
//		yield return StartCoroutine(Implode(startScale));
	}

	public IEnumerator Explode(Vector2 startScale) {
		GetComponentInChildren<SquashAndStretch> ().enabled = false;

		startTime = Time.time;
		// ring.SetActive (false);

		float currentProgress = Time.time - startTime;
		while (currentProgress < explosionDuration) {
			currentProgress = Time.time - startTime;
			transform.localScale = startScale * explosionScale * explosionCurve.Evaluate ( currentProgress / explosionDuration );
			yield return new WaitForFixedUpdate ();
		}

		yield return 0;
	}

//	public IEnumerator Implode() {
//		Vector3 startScale = transform.localScale;
//		startTime = Time.time;
//
//		float currentProgress = Time.time - startTime;
//		while (currentProgress < implosionDuration) {
//			currentProgress = Time.time - startTime;
//			transform.localScale = startScale * implosionCurve.Evaluate ( currentProgress / implosionDuration );
//			yield return new WaitForEndOfFrame ();
//		}
//
//		yield return 0;
//
//		BallManager.numBallsImploded++;
//	}

	public IEnumerator Implode(Vector2 startScale) {
		// ring.transform.localScale = Vector3.one;
		startTime = Time.time;
		// ring.SetActive (true);
		ring.GetComponent<SpriteRenderer> ().sortingOrder += 2;

		float currentProgress = Time.time - startTime;
		while (currentProgress < explosionDuration) {
			currentProgress = Time.time - startTime;
			// ring.transform.localScale = Vector3.one * 1.1f * explosionCurve.Evaluate ( currentProgress / explosionDuration );
			yield return new WaitForFixedUpdate ();
		}

		//yield return 0;
		// ring.transform.parent = transform;
	}

//	public void SetGameOverColor() {
//		sprite.color = gameOverColor;
//	}
}