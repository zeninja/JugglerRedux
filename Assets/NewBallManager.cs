using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallManager : MonoBehaviour {

	public GameObject ballPrefab;
	public static int _ballCount;
	List<GameObject> balls = new List<GameObject>();

	// Use this for initialization
	void Start () {
		EventManager.StartListening("SpawnBall", SpawnBall);
		EventManager.StartListening("BallCaught", OnBallCaught);
		EventManager.StartListening("BallDeath", OnBallDied);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			EventManager.TriggerEvent("SpawnBall");
		}
	}

	void SpawnBall() {
		GameObject ball = Instantiate(ballPrefab) as GameObject;
		balls.Add(ball);
		_ballCount++;
	}

	void OnBallCaught() {
		if(NewScoreManager._catchCount % 5 == 0) {
			EventManager.TriggerEvent("SpawnBall");
		}
	}

	void OnBallDied() {
		for(int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<NewBall>().HandleDeath();
		}
		balls.Clear();
	}
}
