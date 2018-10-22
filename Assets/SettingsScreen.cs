using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : MonoBehaviour {

	#region instance
    private static SettingsScreen instance;
    public static SettingsScreen GetInstance()
    {
        return instance;
    }
    #endregion

	public SettingsButton music, sfx, invertThrows, rails, contact, removeAds, tipJar, exit, settings;

	public UISlider ballSizeSlider;

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
		// EventManager.StartListening("ShowSettings", ShowSettings);
		// EventManager.StartListening("HideSettings", HideSettings);

		InitSettings();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBallScale();

		removeAds.gameObject.SetActive(!NewAdManager.adsDisabled);
		tipJar.gameObject.SetActive(NewAdManager.adsDisabled);
	}

	void InitSettings() {
		music		.SetButtonState(!AudioManager.m_mute);
		sfx			.SetButtonState(!AudioManager.sfx_mute);
		invertThrows.SetButtonState(GlobalSettings.Settings.invertThrows);
		rails		.SetButtonState(NewBallManager.useRails);

		contact.InitBounce();
		removeAds.InitBounce();
		exit.InitBounce();
		settings.InitBounce();

		float p = Extensions.mapRange(.5f, 1.5f, 0f, 1f, GlobalSettings.Settings.ballScale);
		ballSizeSlider.SetScale(p);
	}

	void UpdateBallScale() {
		if (NewBallManager.GetInstance().ballScale != ballSizeSlider.value) {
			NewBallManager.GetInstance().UpdateBallScale(ballSizeSlider.value);
			NewUIManager.UpdateBallScale();
		}
	}

	public void ShowSettings() {
		AudioManager.PlaySettingsTheme();
		StartCoroutine(SettingsIn());
	}

	IEnumerator SettingsIn() {
		float d = settings.moveToClickDuration * 2f;
		yield return StartCoroutine(Extensions.Wait(d));
		GetComponent<Animation>().Play("SettingsIn");
		NewGameManager.GetInstance().EnterSettings();
	}

	public void HideSettings() {
		AudioManager.StopSettingsTheme();
		StartCoroutine(SettingsOut());
	}

	IEnumerator SettingsOut() {
		// Wait for the button to play
		float d = settings.moveToClickDuration * 2f;
		yield return StartCoroutine(Extensions.Wait(d));
		
		// Play settings out
		GetComponent<Animation>().Play("SettingsOut");
	
		// Wait for settings animation
		float d2 = GetComponent<Animation>().GetClip("SettingsOut").length;
		yield return StartCoroutine(Extensions.Wait(d2));
		
		settings.InvertValue();
		NewGameManager.GetInstance().ExitSettings();
	}

	public void PurchaseGame() {
		GetComponent<Purchaser>().MakePurchase();
	}
}

