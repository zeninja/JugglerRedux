using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager instance;
	public Image ballTimer;
	public Toggle mute;

	void Awake() {
		instance = this;
	}

	void Start() {

	}

	public void UpdateMute() {
		mute.isOn = AudioManager.muted;
	}
}
