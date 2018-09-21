using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSprite : MonoBehaviour {

	NewBall m_Ball;
	SpriteRenderer m_Sprite;

	// Use this for initialization
	void Start () {
		m_Ball = GetComponentInParent<NewBall>();
		m_Sprite = GetComponent<SpriteRenderer>();

		peakSprite.color = GetComponentInParent<NewBallArtManager>().myColor;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_Sprite.enabled = m_Ball.m_State == NewBall.BallState.rising  ||
						   m_Ball.m_State == NewBall.BallState.falling ||
						   m_Ball.m_State == NewBall.BallState.firstBall;
		
		if(m_Sprite.enabled == false && m_Ball.m_State != NewBall.BallState.caught && m_Ball.m_State != NewBall.BallState.launching) {
			Debug.Log("breaking");
			Debug.Break();
		}
	}

	public void HandlePeak() {
		StartCoroutine(Peak());
	}

	public SpriteMask peakMask;
	public SpriteRenderer peakSprite;

	IEnumerator Peak() {
		peakMask.transform.localScale   = Vector2.zero;
		peakSprite.transform.localScale = Vector2.zero;
		
		peakMask.enabled   = true;
		peakSprite.enabled = true;

		float t = 0;
		float d = .125f;

		while (t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;
			peakMask.transform.localScale = Vector2.one * p;
			yield return new WaitForFixedUpdate();
		}

		t = 0;
		while (t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;
			peakSprite.transform.localScale = Vector2.one * p;
			yield return new WaitForFixedUpdate();
		}

		peakMask.enabled = false;
		peakSprite.enabled = false;

	}
}
