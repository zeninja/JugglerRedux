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
	 
	// Update is called once per frame
	void Update () {

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
		while(transform.localScale.x > .001f) {
			if(growing) {
				yield return new WaitForEndOfFrame();
			}

			transform.localScale *= .9f;
			
			// transform.localScale = Mathf.Max(0, transform.localScale.x) * Vector3.one;
			yield return new WaitForEndOfFrame();
		}
	}

	void OnGameOver() {
		Destroy(gameObject);
	}
}
