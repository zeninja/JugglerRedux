using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewScoreManager : MonoBehaviour {

	#region instance
	private static NewScoreManager instance;
	public static NewScoreManager GetInstance() {
		return instance;
	}
	#endregion

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	public class Score {
		public int balls;
		public int peaks;
		public float airtime;
	}

	public static int _numBalls;
	// public static int _catchCount;
	public static int _peakCount;
	public static float _progress;
	public static float _lastPeakCount;

	public static int _maxCatchCount = 99;

	string currentScoreString;
	string highScoreString;

	public TextMeshPro scoreText;
	public TextMeshPro highScoreText;

	float currentScore;
	float highscore;
	static string highScoreKey = "highScore";

	public Gradient highscoreGradient;
	public Color scoreColor;

	public float pauseBeforeCountdown = .25f;
	public float inOutDuration;

	// Use this for initialization
	void Start () {
		EventManager.StartListening("SpawnBall", OnBallSpawned);
		EventManager.StartListening("BallCaught", OnBallCaught);
		EventManager.StartListening("BallPeaked", OnBallPeaked);
		EventManager.StartListening("BallSlapped", OnBallSlapped);
		
		InitHighscore();
	}

	void InitHighscore() {
		if(PlayerPrefs.HasKey(highScoreKey)) {
			highscore = PlayerPrefs.GetFloat(highScoreKey);
			highScoreString = highscore.ToString();
		} else {
			highscore = 0.0f;
			highScoreString = "0.0";
		}
		SetHighscoreText();
	}

	void SetHighscoreText() {
		highScoreText.text = highScoreString;
	}
	
	// Update is called once per frame
	void Update () {
		currentScoreString = _numBalls.ToString() + "." + _peakCount.ToString();
		scoreText.text = currentScoreString;

		if(Input.GetKeyDown(KeyCode.C)) {
			PlayerPrefs.DeleteAll();
			SetHighscoreText();
		}
	}

	void OnBallSpawned() {
		_numBalls++;	
	}

	void OnBallCaught() {
		// Debug.Log("caught");
		// scoreText.text = currentScoreString;
		// _catchCount++;
		// _progress = Mathf.Min((float)_catchCount / (float)_maxCatchCount, 1.0f);
	}

	void OnBallSlapped() {
		// Debug.Log("slapped");
		// scoreText.text = currentScoreString;
		// _catchCount++;
	}

	void OnBallPeaked() {
		scoreText.text = currentScoreString;
		_peakCount++;
	}

	public void Reset() {
		_peakCount = 0;
		_numBalls = 0;

		currentScoreString = _numBalls.ToString() + "." + _peakCount.ToString();
		newHighscore = false;
	}

	public static float GetProgressPercent() {
		return _progress;
	}

	public static bool newHighscore;

	public void CheckHighscore() {
		currentScore = float.Parse(string.Format("{0}.{1}", _numBalls.ToString(), _peakCount.ToString()));
		highscore = PlayerPrefs.GetFloat(highScoreKey);
		newHighscore = currentScore > highscore;

		Debug.Log(currentScore + " | " +  highscore);
	}

	public IEnumerator HighscoreProcess() {
		currentScore = float.Parse(string.Format("{0}.{1}", _numBalls.ToString(), _peakCount.ToString()));
		highscore = PlayerPrefs.GetFloat(highScoreKey);

		_lastPeakCount = _peakCount;

		if(newHighscore) {
			// StartCoroutine(RainbowText());

			highscore = currentScore;
			highScoreString = highscore.ToString();
			SetHighscoreText();

			PlayerPrefs.SetFloat(highScoreKey, highscore);

			Debug.Log("REPORTING HIGH SCORE. VALUE IS: " +  highscore);
			GameCenter.GetInstance().SetHighScore(highscore);
			
			yield return StartCoroutine(Rainbower.GetInstance().MakeWaves(_numBalls));

		} else {
			yield return new WaitForSeconds(.1f);
		}
	}

	public IEnumerator RainbowText() {
		yield return new WaitForSeconds(pauseBeforeCountdown);

		float t = 0;
		inOutDuration = .15f;

		while(t < inOutDuration) {
			t += Time.fixedDeltaTime;
			float percent = t / inOutDuration;

			scoreText.color = Color.Lerp(scoreColor, highscoreGradient.Evaluate(0), percent);
			yield return new WaitForFixedUpdate();
		}

		t = 0;
		float duration = 1f;

		while(t < duration) {
			t += Time.fixedDeltaTime;
			float percent = t / duration;

			scoreText.color = highscoreGradient.Evaluate(percent);

			yield return new WaitForFixedUpdate();
		}

		t = 0;
		
		while(t < inOutDuration) {
			t += Time.fixedDeltaTime;
			float percent = t / inOutDuration;

			scoreText.color = Color.Lerp(highscoreGradient.Evaluate(0), scoreColor, percent);
			yield return new WaitForFixedUpdate();
		}

		scoreText.color = scoreColor;
	}

	public void EnableScore(bool val) {
		scoreText.gameObject.SetActive(val);
	}
}
