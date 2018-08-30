using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { /*monolith,*/ preGame, gameOn, gameOver };

public class NewGameManager : MonoBehaviour {

	public static GameState gameState = GameState.preGame;
	public GameState debugState;

	#region instance
	private static NewGameManager instance;
	public static NewGameManager GetInstance() {
		return instance;
	}
	#endregion

	PregameTrailSpawner pregameTrail;

	void Awake() {
		Application.targetFrameRate = 120;

		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}

		pregameTrail = GetComponent<PregameTrailSpawner>();

		EventManager.StartListening("BallDied", HandleGameOver);
	}

	void Start() {
		SetState(gameState);
	}

	void Update() {
		if( Input.touchCount == 2 && NewBallManager._ballCount == 0 && CanSpawnBall()) {
			EventManager.TriggerEvent("SpawnBall");
		}

		debugState = gameState;
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
				pregameTrail.EnableTrail(true);
				pregameTrail.SetPosition();
				break;

			case GameState.gameOn:
				pregameTrail.EnableTrail(false);
				break;

			case GameState.gameOver:
				GameOverManager.GetInstance().StartGameOver();
				break;
		}
	}

	public void StartGame() {
		SetState(GameState.gameOn);
	}

	void HandleGameOver() {
		SetState(GameState.gameOver);
	}

	public static bool GameOver() {
		return NewGameManager.gameState == GameState.gameOver;
	}

	public void ResetGame() {
		NewScoreManager.GetInstance().Reset();
		SetState(GameState.preGame);
	}

	bool CanSpawnBall() {
		return gameState == GameState.preGame;
	}
}
