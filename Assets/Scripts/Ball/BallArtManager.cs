using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallArtManager : MonoBehaviour {

	SpriteRenderer sprite;
	GameObject ring;

	public float explosionDuration = .5f;
	public float implosionDuration = .25f;

	public AnimationCurve explosionCurve;
	public AnimationCurve implosionCurve;

	public float explosionScale = 30;

	float startTime;

	public Color defaultColor;
	public Color catchColor;
	public Color gameOverColor;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		ring = transform.GetChild (0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleCatch() {
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
		sprite.color = defaultColor;
	}

	public void Reset() {
		//transform.localScale = Vector3.one;
		transform.GetChild (0).gameObject.SetActive(true);
	}

	public IEnumerator CompleteExplosion() {
		yield return StartCoroutine(Explode());
		BallManager.startNextExplosion = true;
		yield return StartCoroutine(Implode());
	}

	public IEnumerator Explode() {
		GetComponent<SquashAndStretch> ().enabled = false;
		GetComponent<SpriteRenderer> ().sortingLayerName = "Balls";

		ring.SetActive (false);
		Vector3 startScale = transform.localScale;
		startTime = Time.time;

		bool startedImplosion = false;

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

	public IEnumerator Implode() {
		Vector3 startScale = transform.localScale * 1.1f;
		ring.transform.localScale = Vector3.zero;
		startTime = Time.time;
		ring.SetActive (true);
		ring.transform.parent = null;
		ring.GetComponent<SpriteRenderer> ().sortingOrder += 2;

		float currentProgress = Time.time - startTime;
		while (currentProgress < explosionDuration) {
			currentProgress = Time.time - startTime;
			ring.transform.localScale = startScale * explosionCurve.Evaluate ( currentProgress / explosionDuration );
			yield return new WaitForFixedUpdate ();
		}

		yield return 0;
		ring.transform.parent = transform;
	}

	public void SetGameOverColor() {
		sprite.color = gameOverColor;
	}
}