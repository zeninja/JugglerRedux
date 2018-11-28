using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interstitial : MonoBehaviour {

	private static Interstitial instance;
	public  static Interstitial GetInstance()
    {
        return instance;
	}

	public GameObject button;


	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	public void Update() {
		if(Input.GetKeyDown(KeyCode.S)) {
			StartCoroutine(ShowInterstitial());
		}
	}

	public float hangDuration = 3f;

	public AnimationClip inAnim, outAnim;

	public IEnumerator ShowInterstitial() {
		GetComponent<Animation>().Play(inAnim.name);
		yield return StartCoroutine(Extensions.Wait(inAnim.length));

		button.SetActive(true);

		yield return StartCoroutine(Extensions.Wait(hangDuration));

		button.SetActive(false);

		GetComponent<Animation>().Play(outAnim.name);
		yield return StartCoroutine(Extensions.Wait(outAnim.length));
	}
}
