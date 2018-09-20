using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSprite : MonoBehaviour {

	NewBall m_Ball;
	SpriteRenderer m_Sprite;

	public SpriteMask peakMask;
	public SpriteRenderer peakSprite;

	public Sprite hardSprite;

	// Use this for initialization
	void Start () {
		m_Ball   = GetComponentInParent<NewBall>();
		m_Sprite = GetComponent<SpriteRenderer>();

		peakSprite.color = GetComponentInParent<NewBallArtManager>().myColor;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_Sprite.enabled = m_Ball.m_State == NewBall.BallState.rising  ||
						   m_Ball.m_State == NewBall.BallState.falling ||
						   m_Ball.m_State == NewBall.BallState.firstBall ||
						   m_Ball.m_State == NewBall.BallState.launching;
	}

	// Called via Broadcast message
	public void HandlePeak() {
		peakSprite.color = m_Sprite.color;
		StartCoroutine(Peak());
	}

	// public void UpdateToNormal() {
		
	// }

	public void UpdateToHard() {
		GetComponent<SpriteRenderer>().sprite = hardSprite;
	}

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

	public void AdjustDepth() {
		m_Sprite.sortingOrder = GetComponentInParent<NewBallArtManager>().currentDepth;
	}
}
