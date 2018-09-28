using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSprite : MonoBehaviour {

	NewBall m_Ball;
	public SpriteRenderer m_Sprite;

	public SpriteMask peakMask;
	public SpriteRenderer peakSprite;

	public Sprite hardSprite;

	public float modifier = .5f;

	// Use this for initialization
	void Start () {
		m_Ball   = GetComponentInParent<NewBall>();
		// m_Sprite = GetComponent<SpriteRenderer>();

		// peakSprite.color = GetComponentInParent<NewBallArtManager>().myColor * modifier;
		peakSprite.color = GetComponent<SpriteRenderer>().color * .5f;


		EventManager.StartListening("CleanUp", DisableSprite);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_Sprite.enabled = m_Ball.m_State == NewBall.BallState.rising  ||
						   m_Ball.m_State == NewBall.BallState.falling ||
						   m_Ball.m_State == NewBall.BallState.firstBall ||
						   m_Ball.m_State == NewBall.BallState.launching;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.P)) {
			StartCoroutine(Peak());
		}
				peakSprite.color = GetComponent<SpriteRenderer>().color * modifier;

	}

	// Called via Broadcast message
	public void HandlePeak() {
		// peakSprite.color = m_Sprite.color;
		StartCoroutine(Peak());
	}

	void DisableSprite() {
		gameObject.SetActive(false);
	}

	public void UpdateToHard() {
		m_Sprite.sprite = hardSprite;
		peakSprite.sprite = hardSprite;
	}

	public float peakDuration;

	IEnumerator Peak() {
		peakMask.transform.localScale   = Vector2.zero;
		peakMask.enabled   = true;

		peakSprite.transform.localScale = Vector2.zero;
		peakSprite.enabled = true;

		float t = 0;
		float d = peakDuration / 2;

		t = 0;
		while (t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;
			peakSprite.transform.localScale = Vector2.one * p;
			yield return new WaitForFixedUpdate();
		}

		t = 0;
		while (t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;
			peakMask.transform.localScale = Vector2.one * p;
			yield return new WaitForFixedUpdate();
		}

		peakMask.enabled = false;
		peakSprite.enabled = false;

	}

	public void AdjustDepth() {
		m_Sprite.sortingOrder = GetComponentInParent<NewBallArtManager>().currentDepth * NewBallManager._ballCount;
		peakSprite.sortingOrder = GetComponentInParent<NewBallArtManager>().currentDepth * NewBallManager._ballCount + 1;
	}
}
