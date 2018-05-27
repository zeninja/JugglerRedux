using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewScoreManager : MonoBehaviour {

	public static int _numBalls;
	public static int _catchCount;

	string scoreText;

	TextMeshPro text;

	// Use this for initialization
	void Start () {
		EventManager.StartListening("BallCaught", OnBallCaught);
	}
	
	// Update is called once per frame
	void Update () {
		scoreText = _numBalls.ToString() + "." + _catchCount.ToString();
	}

	void OnBallCaught() {
		text.text = scoreText;
	}
}
