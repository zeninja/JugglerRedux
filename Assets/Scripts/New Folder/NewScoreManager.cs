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

	string scoreText;

	public TextMeshPro text;

	float currentScore;
	float savedHighScore;
	string highScoreKey;

	// Use this for initialization
	void Start () {
		EventManager.StartListening("SpawnBall", OnBallSpawned);
		EventManager.StartListening("BallCaught", OnBallCaught);
		EventManager.StartListening("BallPeaked", OnBallPeaked);
		EventManager.StartListening("BallSlapped", OnBallSlapped);
		EventManager.StartListening("Reset", Reset);
	}
	
	// Update is called once per frame
	void Update () {
		scoreText = _numBalls.ToString() + "." + _catchCount.ToString();
		text.text = scoreText;

	}

	void OnBallSpawned() {
		_numBalls++;	
	}

	void OnBallCaught() {
		// Debug.Log("caught");
		text.text = scoreText;
		_catchCount++;
		_progress = Mathf.Min((float)_catchCount / (float)_maxCatchCount, 1.0f) ;
	}

	void OnBallSlapped() {
		// Debug.Log("slapped");
		text.text = scoreText;
		_catchCount++;
	}

	void OnBallPeaked() {
		text.text = scoreText;
	}

	void Reset() {
		_catchCount = 0;
		_numBalls = 0;

		scoreText = _numBalls.ToString() + "." + _catchCount.ToString();
	}

	public static float GetProgressPercent() {
		return _progress;
	}

	public IEnumerator HighscoreProcess() {

		currentScore = float.Parse(string.Format("{0}.{1}", _numBalls.ToString(), _catchCount.ToString()));

		if(currentScore > savedHighScore) {
			PlayerPrefs.SetFloat(highScoreKey, currentScore);
			// GameCenter.GetInstance().SetHighScore(currentScore);
			yield return StartCoroutine(UpdateHighScore());
		}
	}

	public IEnumerator UpdateHighScore() {
		float t = 0;
		float duration = 1f;

		while(t < duration) {
			t += Time.fixedDeltaTime;
			float percent = t / duration;

			// Add an effect for the high score

			yield return new WaitForFixedUpdate();
		}
	}
}
