using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallManager : MonoBehaviour {

	public GameObject ballPrefab;
	public static int _ballCount;

	// Use this for initialization
	void Start () {
		EventManager.StartListening("SpawnBall", SpawnBall);
		EventManager.StartListening("BallCaught", OnBallCaught);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			EventManager.TriggerEvent("SpawnBall");
		}
	}

	void SpawnBall() {
		GameObject ball = Instantiate(ballPrefab) as GameObject;
		_ballCount++;
	}

	void OnBallCaught() {
		if(NewScoreManager._catchCount % 5 == 0) {
			EventManager.TriggerEvent("SpawnBall");
		}
	}
}
