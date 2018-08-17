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
	
	float trauma;
	public float smallTrauma;
	public float largeTrauma;
	public float traumaReductionRate = 1;


	GameObject mask;

	public float baseScale = 3;
	public float maxScale = 22;
	public float maskInDuration = .4f;

	bool canBounce = false;


	void Start() {
		mask = gameObject;
		transform.localScale = Vector3.zero;
	}

	void Update() {
		// if(Input.GetKeyDown(KeyCode.D))
	}

	// Update is called once per frame
	void FixedUpdate () {
		trauma -= Time.fixedDeltaTime * traumaReductionRate;
		trauma = Mathf.Clamp(trauma, 0, 1);

		float juiceValue = trauma * trauma * trauma;

		if(canBounce) {
			BounceMask();
		}
	}

	public float waitPreMask = .15f;
	public float waitPreBounce = 1.5f;

	public AnimationCurve maskInAnimation;

	public IEnumerator PrepEffect(SpriteRenderer deadBall) {
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

		t = 0;

		while(t < waitPreBounce) {
			t += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}

		canBounce = true;
		trauma = 0;
	}

	public void PlaySmallEffect() {
		trauma += smallTrauma;
	}

	public void PlayLargeEffect() {
		trauma += largeTrauma;
	}

	void BounceMask() {
		float diff = maxScale - baseScale;
		mask.transform.localScale = Vector3.one * baseScale + Vector3.one * diff * trauma;
	}

	public void Reset() {
		mask.transform.localScale = Vector3.zero;
	}
}
