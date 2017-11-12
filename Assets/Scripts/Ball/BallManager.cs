using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BallManager : MonoBehaviour {

	public GameObject ballPrefab;
	public float launchForce = 8;

	[SerializeField]
	List<GameObject> balls = new List<GameObject>();
	public static int numBalls;

	float startTime;
	float holdDuration = .5f;
	float elapsedTime;

	[System.NonSerialized]
	public float timerProgress;

	bool firstBallSpawned = false;

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
		
	}
	
	// Update is called once per frame
	void Update () {
		#region Cheats
		if(Input.GetKeyDown(KeyCode.Space)) {
			LaunchBall();
		}
		#endregion

		SpawnFirstBall();
	}

	void SpawnFirstBall() {
		if (GameManager.GetInstance ().state == GameManager.GameState.mainMenu) {
			if (Input.GetMouseButtonDown (0)) {
				startTime = Time.time;
			}

			elapsedTime = Time.time - startTime;

			if (Input.GetMouseButton (0)) {

				if (!firstBallSpawned) {
					timerProgress = elapsedTime / holdDuration;
					UIManager.instance.ballTimer.fillAmount = timerProgress;


					if (elapsedTime >= holdDuration) {
						//GameObject firstBall = ObjectPool.instance.GetObjectForType ("Ball", false);
						GameObject firstBall = Instantiate(ballPrefab) as GameObject;
						firstBall.transform.position = new Vector2 (0, 4);
						firstBall.GetComponent<Ball> ().ballManager = this;
						balls.Add (firstBall);
						numBalls = balls.Count;
						firstBallSpawned = true;
						GameManager.GetInstance ().HandleGameStart ();
					}
				}
			} else {
				startTime = Time.time;
				UIManager.instance.ballTimer.fillAmount = 0;
			}
		} else {
			UIManager.instance.ballTimer.fillAmount = 0;
			elapsedTime = 0;
			startTime = Time.time;
		}
	}

	public void LaunchBall() {
		GameObject ball = Instantiate(ballPrefab) as GameObject;
		//GameObject ball = ObjectPool.instance.GetObjectForType ("Ball", false);
		ball.transform.position = new Vector2 (Random.Range (-2f, 2f), -6f);
		ball.GetComponent<Rigidbody2D> ().velocity = Vector2.up * launchForce;
		ball.GetComponent<Ball> ().ballManager = this;


		balls.Add (ball);
		numBalls = balls.Count;
	}

	public void UpdateBallDepths(GameObject topBall) {
		for (int i = 0; i < balls.Count; i++) {
			Ball currentBall = balls [i].GetComponent<Ball> ();

			if(!currentBall.launching) {
				currentBall.zDepth++;

				if (balls [i] == topBall) {
					currentBall.zDepth = 0;
				}

				currentBall.SetDepth ();
			}
		}
	}

	public static bool startNextExplosion = false;

	public IEnumerator HandleGameOver() {
		SortBallsByDepth();
		yield return new WaitForEndOfFrame();

		for (int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<Ball>().FreezeBall();
		}

		for (int i = 0; i < balls.Count; i++) {
			balls[i].GetComponent<Ball>().HandleDeath();

			while(!startNextExplosion) {
				yield return new WaitForEndOfFrame();
			}
			startNextExplosion = false;
		}

		balls.Clear();
		firstBallSpawned = false;
		yield return new WaitForEndOfFrame();
	}

	void SortBallsByDepth() {
		Dictionary<int, GameObject> depths = new Dictionary<int, GameObject>();

		for (int i = 0; i < balls.Count; i++) {
			depths.Add(balls[i].GetComponent<Ball>().zDepth, balls[i]);
		}

		var items = from pair in depths
                    orderby pair.Value ascending
                    select pair;

        balls = depths.Values.ToList();
	}
}
