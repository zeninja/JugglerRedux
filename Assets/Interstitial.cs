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

	public AnimationClip removeAdsInterstitial;

	public IEnumerator ShowInterstitial() {
		GetComponent<Animation>().Play(removeAdsInterstitial.name);
		button.SetActive(true);
		yield return StartCoroutine(Extensions.Wait(removeAdsInterstitial.length));
		button.SetActive(false);
	}
}
