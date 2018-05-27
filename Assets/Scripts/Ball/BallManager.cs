using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BallManager : MonoBehaviour {

	public GameObject ballPrefab;
	public float launchForce = 8;

	[SerializeField]
	public List<GameObject> balls = new List<GameObject>();
	public static int numBalls;

	public Vector3 spawnPos = new Vector3(0, -1.75f, 0);

//	float startTime;
//	float holdDuration = .5f;
//	float elapsedTime;

//	[System.NonSerialized]
//	public float timerProgress;

//	bool firstBallSpawned = false;

	private static BallManager instance;
	private static bool instantiated;

	public static BallManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(BallManager)) as BallManager;
			if (!instance)
				Debug.Log("No BallManager!!");
		}
		return instance;
	}

	// Use this for initialization
	void Start () {
//		SpawnFirstBall ();
	}

	public void SpawnFirstBall() {
		GameObject firstBall = Instantiate (ballPrefab) as GameObject;
		firstBall.transform.position = spawnPos;
		firstBall.GetComponent<Ball> ().firstBall = true;
		Debug.Log ("Added first ball");
	}
	
	// Update is called once per frame
	void Update () {
		#region Cheats
		if(Input.GetKeyDown(KeyCode.Space)) {
			LaunchBall();
		}
		#endregion

//		SpawnFirstBall();
	}

//	void SpawnFirstBall() {
//		if (GameManager.GetInstance ().state == GameManager.GameState.mainMenu /*&& Hand.instance.FindHandPos().y < UIBoundary.position.y*/) {
//
//
//
//			#region timer
////			if (Input.GetMouseButtonDown (0)) {
////				startTime = Time.time;
////				ReplayManager.GetInstance().HandleFingerDown (Hand.instance.FindHandPos());
////			}
////			elapsedTime = Time.time - startTime;
////			if (Input.GetMouseButton (0)) {
////
////				if (!firstBallSpawned) {
////					timerProgress = elapsedTime / holdDuration;
////					UIManager.instance.ballTimer.fillAmount = timerProgress;
////
////					if (elapsedTime >= holdDuration) {
////						//GameObject firstBall = ObjectPool.instance.GetObjectForType ("Ball", false);
////						GameObject firstBall = Instantiate(ballPrefab) as GameObject;
////						firstBall.transform.position = new Vector2 (0, 3);
////						firstBall.GetComponent<Ball> ().ballManager = this;
////						balls.Add (firstBall);
////						numBalls = balls.Count;
////						firstBallSpawned = true;
//////						GameManager.GetInstance ().HandleGameStart ();
//////						ReplayManager.GetInstance().HandleBallLaunched (firstBall.transform.position, true);
////
////					}
////				}
////			} else {
////				startTime = Time.time;
////				timerProgress = 0;
////				UIManager.instance.ballTimer.fillAmount = 0;
////			}
////		} else {
////			UIManager.instance.ballTimer.fillAmount = 0;
////			elapsedTime = 0;
////			timerProgress = 0;
////			startTime = Time.time;
//			#endregion
//		}
//	}

	public void LaunchBall() {
		Debug.Log ("Launching ball");
		GameObject ball = Instantiate(ballPrefab) as GameObject;
		ball.transform.position = new Vector2 (Random.Range (-2f, 2f), -6f);
		ball.GetComponent<Rigidbody2D> ().velocity = Vector2.up * launchForce;
		ball.GetComponent<Ball> ().ballManager = this;
		ball.GetComponent<Ball> ().firstBall = false;
		balls.Add (ball);
		numBalls = balls.Count;

//		ReplayManager.GetInstance().HandleBallLaunched (ball.transform.position);
	}

	public void LaunchBall(Vector2 launchPos, bool firstBall) {
		Debug.Log ("Launching first ball");

		GameObject ball = Instantiate(ballPrefab) as GameObject;
		ball.transform.position = launchPos;
		ball.GetComponent<Ball> ().ballManager = this;
		balls.Add (ball);
		numBalls = balls.Count;
	}

	public void UpdateBallDepths(GameObject caughtBall) {
		if (GameManager.GetInstance ().state != GameManager.GameState.gameOver) {
			for (int i = 0; i < balls.Count; i++) {
				Ball currentBall = balls [i].GetComponent<Ball> ();

				if (!currentBall.launching) {
					// Increment the depths of all balls
					currentBall.ballArtManager.zDepth++;

					// Place the "top" ball at the front
					if (balls [i] == caughtBall) {
						currentBall.ballArtManager.zDepth = -100;
					}

					currentBall.ballArtManager.SetDepth ();
				}
			}
		}
	}

	public void RemoveBall(GameObject ball) {
		balls.Remove (ball);
	}

	public static bool startNextExplosion = false;

	public IEnumerator HandleGameOver() {
		Debug.Log("Calling handle game over");
//		SortBallsByDepth();
//		yield return new WaitForEndOfFrame();

		for (int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<Ball>().FreezeBall();
		}
		Debug.Log (balls.Count);

		for (int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<Ball>().HandleDeath();

			while(!startNextExplosion) {
				yield return new WaitForEndOfFrame();
			}
			startNextExplosion = false;
		}

		balls.Clear();
//		firstBallSpawned = false;
		yield return new WaitForEndOfFrame();
	}

	void SortBallsByDepth() {
		Dictionary<int, GameObject> depths = new Dictionary<int, GameObject>();

		for (int i = 0; i < balls.Count; i++) {
			depths.Add(balls[i].GetComponent<BallArtManager>().zDepth, balls[i]);
		}

//		var items = from pair in depths
//                    orderby pair.Value ascending
//                    select pair;

        balls = depths.Values.ToList();

		for (int i = 0; i < balls.Count; i++) {
//			Debug.Log (balls [i].GetComponent<BallArtManager> ().zDepth);
		}
	}

	void OnGUI() {
		
		for (int i = 0; i < balls.Count; i++) {
			if (balls [i] != null) {
				Vector3 ballPos = new Vector3 (balls [i].transform.position.x, -balls [i].transform.position.y, 0);
				Vector3 camPos = Camera.main.WorldToScreenPoint (ballPos);
				Vector3 offset = new Vector3 (camPos.x - 10, camPos.y - 10, camPos.z);
				Vector3 size = Vector3.one * 20;
				string depth = balls [i].GetComponent<BallArtManager> ().zDepth.ToString ();

				GUI.Label (new Rect (offset, size), depth);
			}
		}
			
	}
}