using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum GameState { mainMenu, gameOn, gameOver, replay, settings };
	public GameState state = GameState.mainMenu;

	public BallManager ballManager;

	public static int throwDirection = 1;

	private static GameManager instance;
	private static bool instantiated;

	public static GameManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(GameManager)) as GameManager;
			if (!instance)
				Debug.Log("No GameManager!!");
		}
		return instance;
	}

	void Awake() {
		// Not sure why, but this seems to be messing with the squash and stretching???
		Application.targetFrameRate = 120;
	}

	// Use this for initialization
	void Start () {
		SetState (GameState.mainMenu);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetState(GameState newState) {
		state = newState;

//		ButtonManager.GetInstance().UpdateButtons();

		switch (state) {
			case GameState.mainMenu:
				UIManager.GetInstance ().ScoreActive (false);
				BallManager.GetInstance ().SpawnFirstBall ();
				break;
			case GameState.gameOn:
				UIManager.GetInstance ().ScoreActive (true);
				UIManager.GetInstance().EnableUI(false);
				break;
			case GameState.gameOver:
				StartCoroutine ("GameOverProcedure");
					
				break;
		}
	}

	public void HandleGameStart() {
		SetState (GameState.gameOn);
	}

	public void HandleGameOver() {
		SetState (GameState.gameOver);
	}

	public void GoToSettings() {
		SetState (GameState.settings);
	}

	public void SwitchThrowDirection() {
		throwDirection *= -1;
	}

	public void HandleAdShown() {

	}

	public void ReturnToMainMenu() {
		SetState(GameState.mainMenu);
		UIManager.GetInstance().EnableUI(true);
	}

	IEnumerator GameOverProcedure() {
		Hand.instance.HandleGameOver ();
		yield return StartCoroutine (BallManager.GetInstance ().HandleGameOver ());
		yield return new WaitForSeconds(.5f);
		yield return StartCoroutine (ScoreManager.GetInstance ().HandleGameOver ());
//		AdManager.GetInstance().CheckAd();
//		yield return StartCoroutine(ReplayManager.GetInstance().PlayReplay());
		ReturnToMainMenu ();
	}

	public static bool GameOver() {
		return GameManager.GetInstance ().state == GameState.gameOver;
	}
}