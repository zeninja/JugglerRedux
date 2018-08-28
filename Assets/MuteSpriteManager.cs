using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteSpriteManager : MonoBehaviour {

	public Sprite muted;
	public Sprite unmuted;

	void Update() {
		if(NewGameManager.gameState == GameState.preGame) {
			GetComponent<Image>().sprite = AudioManager.muted ? muted : unmuted;
		}
	}
}
