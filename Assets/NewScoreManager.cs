using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewScoreManager : MonoBehaviour {

	public static int _numBalls;
	public static int _catchCount;

	string scoreText;

	public TextMeshPro text;

	// Use this for initialization
	void Start () {
		EventManager.StartListening("BallSpawned", OnBallSpawned);
		EventManager.StartListening("BallCaught", OnBallCaught);
		EventManager.StartListening("BallDeath", OnBallDied);
	}
	
	// Update is called once per frame
	void Update () {
		scoreText = _numBalls.ToString() + "." + _catchCount.ToString();
	}

	void OnBallSpawned() {
		_numBalls++;	
	}

	void OnBallCaught() {
		text.text = scoreText;
		_catchCount++;
	}

	void OnBallDied() {
		_catchCount = 0;
		_numBalls = 0;
	}
}
