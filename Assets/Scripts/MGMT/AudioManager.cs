using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioClip catchSound;
	public AudioClip throwSound;
	// public AudioClip startSound;
	public AudioClip gameOver;
	// public AudioClip gameStart;
	// public AudioClip gameTimer;

	// public AudioClip[] scoreSounds;

	AudioSource source;

	public static bool muted;
	string muteKey = "Muted";

	// public Slider volumeSlider;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(instance != this) {
				Destroy(gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();

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

	// public static void PlayGameStart() {
	// 	instance.source.PlayOneShot (instance.gameStart);
	// }

	// public static void PlayScore() {
	// 	instance.source.PlayOneShot(instance.scoreSounds[Random.Range(0, instance.scoreSounds.Length)]);
	// }

	// IEnumerator VolumeOn(AudioSource source) {
	// 	StopAllCoroutines();

	// 	int frameCount = 3;

	// 	for(int i = 0; i < frameCount; i++) {
	// 		source.volume = i/frameCount;
	// 		yield return new WaitForEndOfFrame();
	// 	}
	// 	yield return 0;
	// }

	// IEnumerator VolumeOff(AudioSource source) {
	// 	StopAllCoroutines();

	// 	int frameCount = 3;

	// 	for(int i = 0; i < frameCount; i++) {
	// 		source.volume = 1 - i/frameCount;
	// 		yield return new WaitForEndOfFrame();
	// 	}
	// 	yield return 0;
	// }

	// public void UpdateMute() {
	// 	muted = !UIManager.instance.mute.isOn;
	// 	source.mute = muted;

	// 	int storedValue = muted ? 1 : 0;
	// 	PlayerPrefs.SetInt(muteKey, storedValue);
	// }

	// void CheckMute() {
	// 	if (!PlayerPrefs.HasKey(muteKey)) {
	// 		PlayerPrefs.SetInt(muteKey, 0);
	// 	} else {
	// 		muted = PlayerPrefs.GetInt(muteKey) == 1;
	// 		UIManager.instance.UpdateMute();
	// 	}
	// }

	// public void UpdateVolume() {
	// 	source.volume = volumeSlider.value;
	// }
}
