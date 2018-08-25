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

	public static int _numBalls;
	public static int _catchCount;
	public static int _peakCount;
	public static float _progress;
	public static float _lastCatchCount;

	public static int _maxCatchCount = 99;

	string currentScoreString;

	public TextMeshPro scoreText;
	public TextMeshPro highScoreText;

	float currentScore;
	float highscore;
	static string highScoreKey = "highScore";

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
		} else {
			highscore = 0.0f;
		}

		SetHighscoreText();
	}

	void SetHighscoreText() {
		highScoreText.text = currentScoreString;
	}
	
	// Update is called once per frame
	void Update () {
		currentScoreString = _numBalls.ToString() + "." + _catchCount.ToString();
		scoreText.text = currentScoreString;

		if(Input.GetKeyDown(KeyCode.C)) {
			PlayerPrefs.DeleteAll();
		}
	}

	void OnBallSpawned() {
		_numBalls++;	
	}

	void OnBallCaught() {
		// Debug.Log("caught");
		scoreText.text = currentScoreString;
		_catchCount++;
		_progress = Mathf.Min((float)_catchCount / (float)_maxCatchCount, 1.0f) ;
	}

	void OnBallSlapped() {
		// Debug.Log("slapped");
		scoreText.text = currentScoreString;
		_catchCount++;
	}

	void OnBallPeaked() {
		scoreText.text = currentScoreString;
	}

	public void Reset() {
		_catchCount = 0;
		_numBalls = 0;

		currentScoreString = _numBalls.ToString() + "." + _catchCount.ToString();
	}

	public static float GetProgressPercent() {
		return _progress;
	}

	public IEnumerator HighscoreProcess() {
		currentScore = float.Parse(string.Format("{0}.{1}", _numBalls.ToString(), _catchCount.ToString()));
		highscore = PlayerPrefs.GetFloat(highScoreKey);

		Debug.Log("HIGH SCORE PROCESS");
		Debug.Log(currentScore + " | " +  highscore);

		if(currentScore > highscore) {
			Debug.Log("UPDATING HIGH SCORE");

			highscore = currentScore;
			PlayerPrefs.SetFloat(highScoreKey, highscore);
			SetHighscoreText();

			// GameCenter.GetInstance().SetHighScore(highscore);
			
			yield return StartCoroutine(UpdateHighScore());
		}
	}

	public Gradient highscoreGradient;

	public Color scoreColor;

	public float inOutDuration;

	public IEnumerator UpdateHighScore() {
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
}
