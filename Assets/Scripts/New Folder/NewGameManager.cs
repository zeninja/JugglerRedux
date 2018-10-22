using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { /*monolith,*/ /*ballSpawn, preGame,*/ STARTSCREEN, PREGAME, GAMEON, GAMEOVER, SETTINGS };

public class NewGameManager : MonoBehaviour {

	public static GameState gameState = GameState.STARTSCREEN;
	public GameState debugState;

	#region instance
	private static NewGameManager instance;
	public static NewGameManager GetInstance() {
		return instance;
	}
	#endregion

	GameState lastState;

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

		ExitLoopedScreens();

		debugState = gameState;
	}

	void SetState(GameState newState) {
		lastState = gameState;
		gameState = newState;

		switch(gameState) {
			case GameState.STARTSCREEN:
				LogoAnimator.GetInstance().PlayLogoIn();
				receiveInput = true;
				break;

			case GameState.PREGAME:
				StartCoroutine(PregameProcess());
				break;

			case GameState.GAMEON:

				break; 

			case GameState.GAMEOVER:
				GameOverManager.GetInstance().SwitchState(0);
				break;
			case GameState.SETTINGS:
				NewBallManager.GetInstance().HideFirstBall();
				break;
		}
	}

	bool receiveInput = true;

	void ExitLoopedScreens() {
		if(receiveInput) {
			if(Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
				if(gameState == GameState.STARTSCREEN) {
					SetState(GameState.PREGAME);
					receiveInput = false;
				}

				if(gameState == GameState.GAMEOVER) {
					GameOverManager.GetInstance().SwitchState(1);
					receiveInput = false;
				}
			}
		}
	}

	IEnumerator PregameProcess() {
		if(lastState == GameState.STARTSCREEN) {
			yield return StartCoroutine(LogoAnimator.GetInstance().HideLogo());
		}
        NewScoreManager.GetInstance().EnableScore(true);
		yield return StartCoroutine(NewBallManager.GetInstance().SpawnFirstBall());
	}

	public void StartGame() {
		// Triggered by a finger "catching" the first ball
		SetState(GameState.GAMEON);
	}

	public void EnterSettings() {
		SetState(GameState.SETTINGS);
	}

	public void ExitSettings() {
		SetState(GameState.PREGAME);
	}

	void HandleGameOver() {
		SetState(GameState.GAMEOVER);
	}

	public void GameOverInComplete() {
		receiveInput = true;
	}

	public static bool InPreGame() {
		return NewGameManager.gameState == GameState.PREGAME;
	}

	public static bool InSettings() {
		return NewGameManager.gameState == GameState.SETTINGS;
	}

	public static bool GameOver() {
		return NewGameManager.gameState == GameState.GAMEOVER;
	}

	public void ResetGame() {
		SetState(GameState.STARTSCREEN);
	}

	public static bool CanGrabBalls() {
		return gameState != GameState.SETTINGS && gameState != GameState.GAMEOVER;
	}

	public void HandlePurchaseMade() {
        NewAdManager.GetInstance().HandlePurchaseMade();
	}

	public void HandleTip() {

	}

	// Debug
	bool CanSpawnBall() {
		return gameState == GameState.PREGAME;
	}
}
