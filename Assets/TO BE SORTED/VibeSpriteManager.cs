using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibeSpriteManager : MonoBehaviour {

	public Sprite vibeOn, vibeOff;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(NewGameManager.gameState == GameState.preGame) {
			GetComponent<Image>().sprite = Vibrator.vibeEnabled ? vibeOn : vibeOff;
		}
	}
}
