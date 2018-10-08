using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { /*monolith,*/ ballSpawn, preGame, gameOn, gameOver, settings };

public class NewGameManager : MonoBehaviour {

	public static GameState gameState = GameState.ballSpawn;
	public GameState debugState;

	#region instance
	private static NewGameManager instance;
	public static NewGameManager GetInstance() {
		return instance;
	}
	#endregion

	// PregameTrailSpawner pregameTrail;

	void Awake() {
		Application.targetFrameRate = 120;

		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}

		// pregameTrail = GetComponent<PregameTrailSpawner>();

		EventManager.StartListening("BallDied", HandleGameOver);
	}

	public bool useStartState;
	public GameState startState;

	void Start() {
		if(useStartState) {
			SetState(startState);
		} else {
			SetState(gameState);
		}
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
			case GameState.ballSpawn:
				NewBallManager.GetInstance().SpawnFirstBall();
				break;

			case GameState.preGame:
				// NewBallManager.GetInstance().SpawnFirstBall();
				// pregameTrail.SetPosition();
				// pregameTrail.EnableTrail(true);
				break;

			case GameState.gameOn:
				// pregameTrail.EnableTrail(false);
				// BallCountdownManager.GetInstance().SetUpCountdown();
				
				break;

			case GameState.gameOver:
				GameOverManager.GetInstance().StartGameOver();
				break;
			case GameState.settings:
				break;
		}
	}

	public void StartGame() {
		SetState(GameState.gameOn);
		BallCountdownManager.GetInstance().SetUpCountdown();
	}

	public void EnterSettings() {
		SetState(GameState.settings);
	}

	public void ExitSettings() {
		SetState(GameState.preGame);
	}

	void HandleGameOver() {
		SetState(GameState.gameOver);
	}

	public static bool PreGame() {
		return NewGameManager.gameState == GameState.preGame;
	}

	public static bool GameOver() {
		return NewGameManager.gameState == GameState.gameOver;
	}

	public void ResetGame() {
		NewScoreManager.GetInstance().Reset();
		// BallCountdownManager.GetInstance().Reset();
		SetState(GameState.ballSpawn);
	}

	public void PrepGame() {
		SetState(GameState.preGame);
	}

	bool CanSpawnBall() {
		return gameState == GameState.preGame;
	}

	public static bool CanGrabBalls() {
		return gameState != GameState.settings && gameState != GameState.gameOver;
	}

	public void HandlePurchaseMade() {
        NewAdManager.GetInstance().HandlePurchaseMade();
	}

	public void HandleTip() {

	}
}
