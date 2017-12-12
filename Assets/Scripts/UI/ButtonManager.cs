using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

	public static ButtonManager instance;

	public static ButtonManager GetInstance() {
		return instance;
	}

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateButtons() {
		gameObject.SetActive(GameManager.GetInstance().state == GameManager.GameState.mainMenu);
	}
}
