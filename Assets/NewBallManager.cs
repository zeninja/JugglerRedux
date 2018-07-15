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
		EventManager.StartListening("BallDied", OnBallDied);
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
		StartCoroutine("KillBalls");
		balls.Clear();
		_ballCount = 0;

		NewGameManager.GetInstance().SetState(GameState.GameOver);
	}

	IEnumerator KillBalls() {
		for(int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<NewBall>().Die();
			yield return new WaitForSeconds(.2f);
		}
	}
}
