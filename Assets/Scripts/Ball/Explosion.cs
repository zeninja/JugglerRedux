using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public Vector3 finalScale = new Vector3(10, .3f, 0);

	public AnimationCurve xCurve;
	public AnimationCurve yCurve;

	public float animationDuration = .3f;

	/// ~~~~~~~

	Transform[] animationTargets;

	float startTime;
	float elapsedTime;

	// Use this for initialization
	void Awake () {
		animationTargets = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			animationTargets [i] = transform.GetChild (i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.E)) {	
			TriggerAnimation ();
		}
	}

	public void TriggerAnimation() {
		startTime = Time.time;
		StartCoroutine ("Animation");
	}

	IEnumerator Animation() {
		elapsedTime = Time.time - startTime;

		while (elapsedTime < animationDuration) {
			elapsedTime = Time.time - startTime;

			float progress = elapsedTime / animationDuration;

			Vector3 newScale = new Vector3 (xCurve.Evaluate (progress) * finalScale.x, yCurve.Evaluate (progress) * finalScale.y, 0);

			for (int i = 0; i < animationTargets.Length; i++) {
				Transform temp = animationTargets [i].transform;
				temp.localScale = newScale;
				animationTargets [i] = temp;
			}
			yield return new WaitForEndOfFrame ();
		}
		yield return 0;
	}
}
