using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : MonoBehaviour {

	// bool music, sfx, invertThrows;
	public SettingsButton music, sfx, invertThrows;

	public UISlider ballSizeSlider;

	void Awake() {
	}

	// Use this for initialization
	void Start () {
		EventManager.StartListening("ShowSettings", ShowSettings);
		EventManager.StartListening("HideSettings", HideSettings);

		InitSettings();
	}
	
	// Update is called once per frame
	void Update () {
		if(ballSizeSlider != null) {
			NewBallManager.GetInstance().ballScale = ballSizeSlider.value;
		}
	}

	void InitSettings() {
		

		// music.startState = AudioManager.m_mute;
	}

	public void ShowSettings() {

	}

	public void HideSettings() {

	}

	public void PurchaseGame() {
		GetComponent<Purchaser>().MakePurchase();
	}
}
