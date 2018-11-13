using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance;
	public Image ballTimer;
	// public Toggle mute;
	public Toggle settings;
	public GameObject buttons;

	public GameObject settingsPanel;
	public GameObject scoreDisplay;

	public static UIManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(UIManager)) as UIManager;
			if (!instance)
				Debug.Log("No UIManager!!");
		}
		return instance;
	}

	void Awake() {
		instance = this;
	}

	void Start() {

	}

	// public void UpdateMute() {
	// 	mute.isOn = AudioManager.m_mute;
	// }

	public void ToggleSettings() {
		settingsPanel.SetActive (settings.isOn);
	}

	public void ScoreActive(bool active) {
		scoreDisplay.SetActive (active);
	}

	public void EnableUI(bool enabled) {
		buttons.SetActive(enabled);
	}
}
