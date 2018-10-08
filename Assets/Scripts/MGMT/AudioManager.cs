using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	public AudioClip catchSound;
	public AudioClip throwSound;
	public AudioClip gameOver;

	public AudioClip peakSound;
	public AudioClip selectSound;
	public AudioClip undoSound;


	public AudioSource sfxSource;
	public AudioSource musicSource;

	public static bool m_mute = false;
	string m_muteKey = "m_mute";
	public static bool sfx_mute = false;
	string sfx_muteKey = "sfx_mute";

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
		EventManager.StartListening("BallDied",   PlayGameOver);
		EventManager.StartListening("BallPeaked", PlayPeak);
		EventManager.StartListening("Select",     PlaySelect);
		EventManager.StartListening("Undo",       PlayUndo);
	}

	public static void PlayCatch() {
		instance.sfxSource.PlayOneShot (instance.catchSound);
	}

	public static void PlayThrow() {
		instance.sfxSource.PlayOneShot (instance.throwSound);
	}

	public static void PlayPeak() {
		instance.sfxSource.PlayOneShot (instance.peakSound);
	}

	public static void PlaySelect() {
		instance.sfxSource.PlayOneShot (instance.selectSound);
	}

	public static void PlayUndo() {
		instance.sfxSource.PlayOneShot (instance.undoSound);
	}

	public static void PlayGameOver() {
		instance.sfxSource.PlayOneShot (instance.gameOver);
	}

	void InitMute() {
		// if (!PlayerPrefs.HasKey(m_muteKey)) {
		// 	PlayerPrefs.SetInt(m_muteKey, 0);
		// } else {
		// 	m_mute = PlayerPrefs.GetInt(m_muteKey) == 1;
		// }

		// musicSource.mute = m_mute;

		// if (!PlayerPrefs.HasKey(sfx_muteKey)) {
		// 	PlayerPrefs.SetInt(sfx_muteKey, 0);
		// } else {
		// 	sfx_mute = PlayerPrefs.GetInt(sfx_muteKey) == 1;
		// }

		// sfxSource.mute = sfx_mute;
	}

	public void ToggleMute() {
		m_mute = !m_mute;
		musicSource.mute = m_mute;

		GlobalSettings.Settings.musicOn = !m_mute;
		GlobalSettings.UpdateSavedValues();
	}

	public void ToggleSFX() {
		sfx_mute = !sfx_mute;
		sfxSource.mute = sfx_mute;

		GlobalSettings.Settings.sfxOn = !sfx_mute;
		GlobalSettings.UpdateSavedValues();
	}
}
