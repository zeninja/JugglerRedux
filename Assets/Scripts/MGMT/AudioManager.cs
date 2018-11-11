using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	public enum ButtonSound { select, undo, bounce };

	public static AudioManager instance;

	public AudioClip catchSound;
	public AudioClip throwSound;
	public AudioClip gameOver;

	public AudioClip peakSound;
	public AudioClip selectSound;
	public AudioClip undoSound;
	public AudioClip bounceSound;


	public AudioSource sfxSource;
	public AudioSource mainThemeSource;
	public AudioSource settingsThemeSource;

	public static bool m_mute   = false;
	public static bool sfx_mute = false;

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
		EventManager.StartListening("BallCaught", PlayCatch);
		EventManager.StartListening("BallThrown", PlayThrow);
		EventManager.StartListening("BallDied",   PlayGameOver);
		EventManager.StartListening("BallPeaked", PlayPeak);
		EventManager.StartListening("Select",     PlaySelect);
		EventManager.StartListening("Undo",       PlayUndo);

		SetMutes();
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

	public static void PlayBounce() {
		instance.sfxSource.PlayOneShot(instance.bounceSound);
	}

	public static void PlayGameOver() {
		instance.sfxSource.PlayOneShot (instance.gameOver);
	}

	public static void PlaySettingsTheme() {
		instance.SettingsThemeIn();
	}

	public static void StopSettingsTheme() {
		instance.SettingsThemeOut();
	}

	void SettingsThemeIn() {
		StartCoroutine(FadeSourceOut(mainThemeSource));
		StartCoroutine(FadeSourceIn(settingsThemeSource));
		settingsThemeSource.Play();
	}

	void SettingsThemeOut() {
		StartCoroutine(FadeSourceIn(mainThemeSource));
		StartCoroutine(FadeSourceOut(settingsThemeSource));
	}

	IEnumerator FadeSourceIn(AudioSource source) {
		float t = 0;
		float d = .5f;

		while (t < d) {
			float p = t / d;
			t += Time.fixedDeltaTime;
			source.volume = EZEasings.SmoothStop3(p);
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator FadeSourceOut(AudioSource source) {
		float t = 0;
		float d = .5f;

		while (t < d) {
			float p = t / d;
			t += Time.fixedDeltaTime;
			source.volume = 1 - EZEasings.SmoothStop3(p);
			yield return new WaitForFixedUpdate();
		}
		source.Stop();
	}

	void SetMutes() {
		mainThemeSource.mute 	 = GlobalSettings.Settings.muteMusic;
		settingsThemeSource.mute = GlobalSettings.Settings.muteMusic;
		sfxSource.mute 	 		 = GlobalSettings.Settings.muteSfx;
	}

	public void ToggleMute() {
		m_mute = !m_mute;
		SetMutes();

		GlobalSettings.Settings.muteMusic = m_mute;
		GlobalSettings.UpdateSavedValues();
	}

	public void ToggleSFX() {
		sfx_mute = !sfx_mute;
		SetMutes();

		GlobalSettings.Settings.muteSfx = sfx_mute;
		GlobalSettings.UpdateSavedValues();
	}
}
