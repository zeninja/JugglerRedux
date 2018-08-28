using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioClip catchSound;
	public AudioClip throwSound;
	public AudioClip gameOver;

	AudioSource source;

	public static bool muted;
	string muteKey = "mute";

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(instance != this) {
				Destroy(gameObject);
			}
		}

		InitMute();
	}

	// Use this for initialization
	void Start () {

		EventManager.StartListening("BallCaught", PlayCatch);
		EventManager.StartListening("BallThrown", PlayThrow);
		EventManager.StartListening("BallDied", PlayGameOver);
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

	void InitMute() {
		if (!PlayerPrefs.HasKey(muteKey)) {
			PlayerPrefs.SetInt(muteKey, 0);
		} else {
			muted = PlayerPrefs.GetInt(muteKey) == 1;
		}

		source = GetComponent<AudioSource>();
		source.mute = muted;
	}

	public void ToggleMute() {
		muted = !muted;
		source.mute = muted;

		int muteVal = muted ? 1 : 0;
		PlayerPrefs.SetInt(muteKey, muteVal);
	}
}
