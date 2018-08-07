using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { preGame, inGame, gameOver };

public class NewGameManager : MonoBehaviour {

	public GameState gameState = GameState.preGame;

	#region instance
	private static NewGameManager instance;
	public static NewGameManager GetInstance() {
		return instance;
	}
	#endregion

	// public bool spawnBallsByTouchCount;

	void Awake() {
		Application.targetFrameRate = 120;

		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	void Update() {
		if( Input.touchCount == 2 && NewBallManager._ballCount == 0) {
			EventManager.TriggerEvent("SpawnBall");
		}	
	}

	public void SetState(GameState newState) {
		gameState = newState;

		// trigger one-time effects

		switch(gameState) {
			case GameState.gameOver:
				StartCoroutine("GameOverProcedure");
				break;
		}

	}

	IEnumerator GameOverProcedure() {
		yield return NewBallManager.GetInstance().StartCoroutine("KillBalls");
		yield return NewScoreManager.GetInstance().StartCoroutine("HandleGameOver");
		SetState(GameState.preGame);
	}

	public static bool GameOver() {
		return NewGameManager.GetInstance().gameState == GameState.gameOver;
	}

	public void OnGUI() {
		GUI.color = Color.black;
		GUI.Label(new Rect(0, 0, 150, 150), gameState.ToString());
	}
}
