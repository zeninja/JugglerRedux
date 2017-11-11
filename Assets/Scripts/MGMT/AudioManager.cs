using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioClip catchSound;
	public AudioClip throwSound;
	public AudioClip startSound;
	public AudioClip gameOver;
	public AudioClip gameStart;
	public AudioClip gameTimer;

	AudioSource source;
	AudioSource timerSource;

	// Use this for initialization
	void Start () {
		instance = this;
		source = GetComponent<AudioSource> ();
		timerSource = transform.GetComponentInChildren<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		PlayTimer ();
	}

	public static void PlayCatch() {
		instance.source.PlayOneShot (instance.catchSound);
	}

	public static void PlayThrow() {
		instance.source.PlayOneShot (instance.throwSound);
	}

	public static void PlayGameOver() {
		instance.source.PlayOneShot (instance.gameOver);
	}

	public static void PlayGameStart() {
		instance.source.PlayOneShot (instance.gameStart);
	}

	public void PlayTimer() {
		if (BallManager.GetInstance ().timerProgress > 0) {
			timerSource.volume = 1;
			timerSource.pitch = 1 + BallManager.GetInstance ().timerProgress;
		} else {
			timerSource.volume = 0;
			timerSource.pitch = 1;
		}
	}
}
