using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour {

	public GameObject ballPrefab;
	public float launchForce = 8;

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
			balls[i].GetComponent<Ball>().zDepth++;

			if (balls[i] == topBall) {
				balls[i].GetComponent<Ball>().zDepth = 0;
			}

			balls[i].GetComponent<Ball>().SetDepth();
		}
	}

	public static bool startNextExplosion = false;

	public IEnumerator HandleGameOver() {
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

	/*public static int numBallsExploded = 0;

//	public void HandleBallExplosion() {
//		if (numBallsExploded < numBalls) {
//			balls[numBallsExploded].GetComponent<Ball>().HandleDeath();
//		}
//	}

	public IEnumerator HandleGameOver() {
//		for (int i = 0; i < numBalls; i++) {
//			balls [i].GetComponent<Ball> ().HandleDeath ();
//		}

//		balls[0].GetComponent<Ball>().HandleDeath();
		yield return StartCoroutine(ExplodeBalls());

		numBallsExploded = 0;
		yield return 0;
	}

	IEnumerator ExplodeBalls() {
		while (numBallsExploded < numBalls) {	
			for (int i = 0; i < balls.Count; i++) {
				balls[i].GetComponent<Ball>().HandleDeath();
			}
			yield return new WaitForEndOfFrame();
		}
	}

	/*public static int numBallsImploded = 0;

	public IEnumerator CleanUpBalls() {
		for (int i = 0; i < numBalls; i++) {
			balls [i].GetComponent<Ball> ().CleanUp ();
			yield return new WaitForSeconds(.25f);
		}

		while (numBallsImploded < numBalls) {
			yield return new WaitForEndOfFrame();
		}

		for (int i = 0; i < numBalls; i++) {
			Destroy (balls [i]);
		}

		numBalls = 0;
		balls.Clear ();
		firstBallSpawned = false;
		numBallsImploded = 0;
		yield return 0;
	}*/
}
