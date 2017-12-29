using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioClip catchSound;
	public AudioClip throwSound;
	public AudioClip startSound;
	public AudioClip gameOver;
	public AudioClip gameStart;
	public AudioClip gameTimer;

	public AudioClip[] scoreSounds;

	AudioSource source;
	AudioSource timerSource;

	public AnimationCurve pitchModifier;

	public static bool muted;
	string muteKey = "Muted";

	// Use this for initialization
	void Start () {
		instance = this;
		source = GetComponent<AudioSource> ();
		timerSource = transform.Find("TimerAudio").GetComponent<AudioSource> ();
		timerSource.Play();

		UIManager.instance.mute.onValueChanged.AddListener( delegate { UpdateMute(); } );

		CheckMute();
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

	public static void PlayScore() {
		instance.source.PlayOneShot(instance.scoreSounds[Random.Range(0, instance.scoreSounds.Length)]);
	}

	public void PlayTimer() {
		if(GameManager.GetInstance().state == GameManager.GameState.mainMenu) {
			if (BallManager.GetInstance ().timerProgress > 0) {
				timerSource.volume = 1.0f;
				timerSource.pitch = 1 + pitchModifier.Evaluate (BallManager.GetInstance ().timerProgress);
			} else {
				timerSource.volume = 0;
				timerSource.pitch = 1;
			}
		} else {
			timerSource.volume = 0;
			timerSource.pitch = 1;
		}
	}

	public void UpdateMute() {
		muted = !UIManager.instance.mute.isOn;
		source.mute = muted;

		int storedValue = muted ? 1 : 0;
		PlayerPrefs.SetInt(muteKey, storedValue);
	}

	void CheckMute() {
		if (!PlayerPrefs.HasKey(muteKey)) {
			PlayerPrefs.SetInt(muteKey, 0);
		} else {
			muted = PlayerPrefs.GetInt(muteKey) == 1;
			UIManager.instance.UpdateMute();
		}
	}
}
