using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallManager : MonoBehaviour {
	
	#region instance
	private static NewBallManager instance;
	public static NewBallManager GetInstance() {
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

	public GameObject ballPrefab;
	public static int _ballCount;
	
	List<GameObject> balls = new List<GameObject>();

	Vector2 ballSpawnPos;
	public float ballLaunchForce = 10;

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

		ballSpawnPos = new Vector2(Random.Range(-2.25f, 2.25f), -6);
		ball.transform.position = ballSpawnPos;
		ball.GetComponent<NewBall>().launching = true;
		ball.GetComponent<NewBall>().canBeCaught = false;
		ball.GetComponent<Rigidbody2D>().velocity = Vector2.up * ballLaunchForce;
		Debug.Log("Launching");

		balls.Add(ball);
		_ballCount++;
	}

	void OnBallCaught() {
		if(NewScoreManager._catchCount % 5 == 0) {
			EventManager.TriggerEvent("SpawnBall");
		}
	}

	void OnBallDied() {
		NewGameManager.GetInstance().SetState(GameState.gameOver);
	}

	IEnumerator KillBalls() {
		for(int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<NewBall>().Die();
			yield return new WaitForSeconds(.2f);
		}

		balls.Clear();
		_ballCount = 0;
	}
}
