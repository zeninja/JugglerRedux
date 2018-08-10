using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightController : MonoBehaviour {
	
	SpriteRenderer m_Sprite;
	NewBall ball;
	Transform target;

	public float animationDuration = .25f;

	public AnimationCurve xCurve;
	public AnimationCurve yCurve;

	public Gradient colorOverTime;

	public float scalar = 3f;
	public float scaleSmoothing = 1;

	// Vector2 targetScale; 

	// Use this for initialization
	void Start () {
		ball = GetComponentInParent<NewBall>();
		m_Sprite = GetComponent<SpriteRenderer>();

		m_Sprite.enabled = false;
		EventManager.StartListening("BallCaught", DoCatch);
		EventManager.StartListening("BallThrown", DoThrow);
		EventManager.StartListening("BallDied", OnGameOver);

		target = transform.parent;
	}
	 
	public float squashAmount = 1.75f;

	// Update is called once per frame
	void Update () {
		

		if(!growing && ball.m_IsHeld) {
			float squash = ball.currentThrowVector.magnitude;

			squash = Extensions.mapRangeMinMax(0, 50, 0, 1, squash);
			squash *= squashAmount;

			Vector2 targetScale = new Vector2( 1 + squash, 1 - squash ) * scalar;
			LookAt2D();

			transform.localScale = Vector2.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSmoothing);

		// transform. localScale += (targetScale - transform.localScale) * .1f;
		}
	}

	void DoCatch() {
		transform.position = target.transform.position;
		transform.parent = target;
		// transform.localPosition = Vector3.zero;
		if(ball.CaughtJustNow()) {
			StartCoroutine(CatchEffect());
		}
	}

	void DoThrow() {
		transform.parent = null;
		StartCoroutine(RecedeEffect());
	}


	bool growing;

	IEnumerator CatchEffect() {
		// Debug.Log("Catch effect");
		m_Sprite.enabled = true;

		float elapsed = 0;
		float percent = 0;

		while (elapsed < animationDuration) {
			growing = true;
			elapsed += Time.deltaTime;
			percent = elapsed/animationDuration;

			float x = scalar * xCurve.Evaluate(percent);// * targetScale.x;
			float y = scalar * yCurve.Evaluate(percent);// * targetScale.y;

			transform.localScale = new Vector3(x, y, 0);

			// m_Sprite.color = colorOverTime.Evaluate(percent);
			yield return new WaitForEndOfFrame();
		}
		growing = false;
	}

	IEnumerator RecedeEffect() {
		if(growing) {
			yield return new WaitForEndOfFrame();
		}

		while(transform.localScale.x > .001f) {
	


			transform.localScale *= .9f;
			
			// transform.localScale = Mathf.Max(0, transform.localScale.x) * Vector3.one;
			yield return new WaitForEndOfFrame();
		}
	}

	void LookAt2D() {
		Vector3 vectorToTarget = target.transform.position - transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = q;//Quaternion.Slerp(transform.rotation, , Time.deltaTime * );
	}


	void OnGameOver() {
		Destroy(gameObject);
	}
}
