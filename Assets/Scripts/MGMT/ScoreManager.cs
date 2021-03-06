using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public static int score;
	static int highScore;

	public UnityEngine.UI.Text scoreDisplay;
	public UnityEngine.UI.Text highScoreDisplay;

	private static ScoreManager instance;
	private static bool instantiated;

	Vector3 highScoreStartPos;

	public static ScoreManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
			if (!instance)
				Debug.Log("No ScoreManager!!");
		}
		return instance;
	}

	void Awake() {
		InitHighScore ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.C)) {
			ClearHighScore();
		}

		if(Input.GetKeyDown(KeyCode.H)) {
			ClearHighScore();
			score = 10;

			StartCoroutine(HandleGameOver());
		}


		if (!ScoreAnimation.flashingScore) {
			scoreDisplay.text = score.ToString ();
		}
	}

	public void IncreaseScore() {
		// AudioManager.PlayScore ();
		score++;
		if (!ScoreAnimation.flashingScore) {
			scoreDisplay.text = score.ToString ();
		}

		if (score == 5  ||
			score == 15 ||
			score % 25 == 0) {
			BallManager.GetInstance().LaunchBall();		
			StartCoroutine(ScoreAnimation.GetInstance().FlashScore());
		}
	}

	public IEnumerator HandleGameOver() {
		ScoreAnimation.GetInstance().HandleGameOver();

		if (score > highScore) {
			StartCoroutine(UpdateHighScore());
			yield return StartCoroutine(ScoreAnimation.GetInstance().ShowNewHighScore());
		}
		yield return new WaitForSeconds (.5f);
		yield return StartCoroutine (ScoreAnimation.GetInstance ().CountdownScore ());
	}

	IEnumerator UpdateHighScore() {
		highScore = score;
		PlayerPrefs.SetInt ("HighScore", highScore);

		#if UNITY_IOS && !UNITY_EDITOR
		GameCenter.GetInstance().SetHighScore(highScore);
		#endif

		//TODO: This seems like the kind of thing that should just be in ScoreAnimation??
		Vector2 highScorePos   = highScoreDisplay.transform.position;
		Vector2 highScoreScale = highScoreDisplay.transform.localScale;

		// highScoreDisplay.gameObject.MoveTo(highScorePos * 1.5f, .5f, 0, EaseType.easeInBack);
		// highScoreDisplay.gameObject.ScaleTo(Vector2.one * .25f, .5f, 0, EaseType.easeInBack);

		yield return new WaitForSeconds(.5f + Time.deltaTime);

		highScoreDisplay.text = highScore.ToString ();
		// highScoreDisplay.gameObject.MoveTo(highScorePos, .5f, 0, EaseType.easeOutBack);
		// highScoreDisplay.gameObject.ScaleTo(highScoreScale, .5f, 0, EaseType.easeOutBack);
	}

	void InitHighScore() {
		if (PlayerPrefs.HasKey ("HighScore")) {
			highScore = PlayerPrefs.GetInt ("HighScore");
			highScoreDisplay.text = highScore.ToString ();
		} else {
			PlayerPrefs.SetInt ("HighScore", 0);
		}
	}

	void ClearHighScore() {
		PlayerPrefs.SetInt("HighScore", 0);
		highScore = PlayerPrefs.GetInt ("HighScore");
		highScoreDisplay.text = highScore.ToString ();
	}
}
