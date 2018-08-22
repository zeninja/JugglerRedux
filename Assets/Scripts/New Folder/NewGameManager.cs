using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { /*monolith,*/ preGame, inGame, gameOver };

public class NewGameManager : MonoBehaviour {

	public static GameState gameState = GameState.preGame;

	#region instance
	private static NewGameManager instance;
	public static NewGameManager GetInstance() {
		return instance;
	}
	#endregion

	void Awake() {
		Application.targetFrameRate = 120;

		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}

		EventManager.StartListening("BallDied", HandleGameOver);
	}

	void Start() {
		SetState(gameState);
	}

	void Update() {
		if( Input.touchCount == 2 && NewBallManager._ballCount == 0 && CanSpawnBall()) {
			EventManager.TriggerEvent("SpawnBall");
		}
	}

	public void SetState(GameState newState) {
		gameState = newState;

		// trigger one-time effects

		switch(gameState) {
			// case GameState.monolith:
			// 	MonolithManager.GetInstance().Initialize();
			// 	break;
			case GameState.preGame:
				NewBallManager.GetInstance().SpawnFirstBall();
				break;

			case GameState.gameOver:
				GameOverManager.GetInstance().StartGameOver();
				break;
		}
	}

	void HandleGameOver() {
		SetState(GameState.gameOver);
	}

	public static bool GameOver() {
		return NewGameManager.gameState == GameState.gameOver;
	}

	public void ResetGame() {
		NewBallManager.GetInstance().KillAllBalls();
		NewScoreManager.GetInstance().Reset();
		SetState(GameState.preGame);
	}

	bool CanSpawnBall() {
		return gameState == GameState.preGame;
	}
}
