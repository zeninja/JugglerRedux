using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {

	Selectable item;

	// Use this for initialization
	void Start () {
		item = GetComponent<Selectable>();
	}
	
	// Update is called once per frame
	void Update () {

		// Can change this to just "New game manager in menu state"
		item.interactable = NewGameManager.gameState == GameState.preGame;
	}
}
