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

	string scoreText;

	public TextMeshPro text;

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
		_progress = Mathf.Min((float)_catchCount / 99f, 1.0f) ;
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

	public IEnumerator HandleGameOver() {
		_catchCount = 0;
		_numBalls = 0;
		yield return 0;
	}

	public static float GetProgressPercent() {
		return _progress;
	}
}
