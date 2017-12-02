using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum GameState { mainMenu, gameOn, gameOver };
	public GameState state = GameState.mainMenu;

	public BallManager ballManager;

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
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetState(GameState newState) {
		state = newState;

		ButtonManager.GetInstance().UpdateButtons();

		switch (state) {
			case GameState.mainMenu:
				break;
			case GameState.gameOn:
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

	public void RestartGame() {
		SetState (GameState.mainMenu);
	}

	IEnumerator GameOverProcedure() {
		Hand.instance.HandleGameOver ();
		yield return StartCoroutine (BallManager.GetInstance ().HandleGameOver ());
		yield return new WaitForSeconds(.5f);
		yield return StartCoroutine (ScoreManager.GetInstance ().HandleGameOver ());
//		yield return StartCoroutine (AdManager.GetInstance ().HandleGameOver ());
		RestartGame ();
	}
}