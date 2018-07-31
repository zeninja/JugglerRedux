using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewScoreManager : MonoBehaviour {

	public static int _numBalls;
	public static int _catchCount;
	public static int _peakCount;

	string scoreText;

	public TextMeshPro text;

	// Use this for initialization
	void Start () {
		EventManager.StartListening("SpawnBall", OnBallSpawned);
		EventManager.StartListening("BallCaught", OnBallCaught);
		EventManager.StartListening("BallPeaked", OnBallPeaked);
		EventManager.StartListening("BallSlapped", OnBallSlapped);
		EventManager.StartListening("BallDied", OnBallDied);
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
		text.text = scoreText;
		_catchCount++;
	}

	void OnBallSlapped() {
		text.text = scoreText;
		_catchCount++;
	}

	void OnBallPeaked() {
		text.text = scoreText;
	}

	void OnBallDied() {
		_catchCount = 0;
		_numBalls = 0;

		scoreText = _numBalls.ToString() + "." + _catchCount.ToString();
	}
}
