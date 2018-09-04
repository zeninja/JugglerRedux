using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteMask))]
public class ScoreMaskEffect : MonoBehaviour {

	#region Instance
	static ScoreMaskEffect instance;
	public static ScoreMaskEffect GetInstance() {
		return instance;
	}

	// Use this for initialization
	void Awake () {
		if(instance == null) {
			instance = this;
		} else {
			if (this != instance) {
				Destroy(gameObject);
			}
		}
	}
	#endregion
	
	GameObject mask;

	public float baseScale = 3;
	// public float maxScale = 22;
	public float maskInDuration = .4f;
	public float maskOutDuration;

	// bool canBounce = false;

	public float waitPreMask = .15f;
	public AnimationCurve maskInAnimation;


	void Start() {
		mask = gameObject;
		transform.localScale = Vector3.zero;
	}

	public IEnumerator PopInScoreMask(SpriteRenderer deadBall) {
		deadBall.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

		float t = 0;

		while(t < waitPreMask) {
			t += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}

		t = 0;

		while(t < maskInDuration) {
			t += Time.fixedDeltaTime;
			mask.transform.localScale = Vector3.one * baseScale * maskInAnimation.Evaluate(t / maskInDuration);
			yield return new WaitForFixedUpdate();
		}
	}

	public IEnumerator PlayMaskOut() {
		float t = 0;
		while (t < maskOutDuration) {
			t += Time.fixedDeltaTime;
			mask.transform.localScale = Vector3.one * baseScale * (1 - EZEasings.SmoothStart3(t / maskOutDuration));
			yield return new WaitForFixedUpdate();
		}
	}

	public void Reset() {
		mask.transform.localScale = Vector3.zero;
	}
}