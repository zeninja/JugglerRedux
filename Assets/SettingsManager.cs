using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

	public GameObject[] hiddenObjects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		UpdateHiddenObjects (false);
		GameManager.GetInstance ().GoToSettings ();
	}

	void OnDisable() {
		UpdateHiddenObjects (true);
		GameManager.GetInstance ().ReturnToMainMenu ();
	}

	void UpdateHiddenObjects(bool hidden) {
		for (int i = 0; i < hiddenObjects.Length; i++) {
			hiddenObjects [i].SetActive (hidden);
		}
	}
}
