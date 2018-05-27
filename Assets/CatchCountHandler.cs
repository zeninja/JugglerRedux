using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchCountHandler : MonoBehaviour {

	public GameObject counterPrefab;
	public int startCatchCount = 5;
	public int currentCatchCount;
	public float distanceFromCenter = .6f;

	List<GameObject> catchCounter;


	// Use this for initialization
	void Awake () {
		currentCatchCount = startCatchCount + BallManager.numBalls;
		catchCounter = new List<GameObject> ();
		SpawnCounter();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleCatch() {
		if (!enabled)
			return;
		if (GameManager.GetInstance ().state == GameManager.GameState.gameOn) {
			currentCatchCount--;
			UpdateCatchCounter ();
			if (currentCatchCount <= 0) {
				BallManager.GetInstance ().RemoveBall (gameObject);
				Explode ();
			}
		}
	}

	void SpawnCounter() {
		for (int i = 0; i < currentCatchCount; i++) {
			GameObject counter = Instantiate (counterPrefab) as GameObject;
			counter.transform.parent = transform;
			counter.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0,  i * (360 / currentCatchCount)));
			counter.transform.localPosition = counter.transform.up * distanceFromCenter;
			counter.transform.localScale = Vector3.one * .2f;
			catchCounter.Add (counter);
		}
	}

	void UpdateCatchCounter() {
		GameObject deadCounter = catchCounter [0];
		catchCounter.RemoveAt (0);
		Destroy (deadCounter);

		for (int i = 0; i < currentCatchCount; i++) {
			LeanTween.rotate (catchCounter [i], new Vector3 (0, 0, i * (360 / currentCatchCount)), .1f);
		}
	}

	void Explode() {
		Destroy (gameObject);
	}
}
