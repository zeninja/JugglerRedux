using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour {

	// track positions of ball spawns
	// track positions of inputs in
	// track positions of inputs out
	// track time between in and out (in frames)

	[System.Serializable]
	public class InputInfo {
		public int frameCountDown;
		public int frameCountUp;
		public Vector2 positionDown;
		public Vector2 positionUp;

		public int ballFrameCount;
		public Vector2 ballLaunchLocation;
		public bool firstBall;
	}

	public List<InputInfo> InputStorage;
	
	private static ReplayManager instance;
	private static bool instantiated;

	public static ReplayManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(ReplayManager)) as ReplayManager;
			if (!instance)
				Debug.Log("No ReplayManager!!");
		}
		return instance;
	}
	// Use this for initialization
	void Start () {
		instance = this;

		InputStorage = new List<InputInfo> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
//			StartCoroutine(PlayReplay());
		}
	}

	public void HandleFingerDown(Vector2 newPos) {
		InputInfo newInfo = new InputInfo ();
		newInfo.frameCountDown = Time.frameCount;
		newInfo.positionDown = newPos;
		newInfo.ballFrameCount = -1;

		InputStorage.Add (newInfo);
		Debug.Log ("Adding finger down");
	}

	public void HandleFingerUp(Vector2 newPos) {
		InputInfo newInfo = new InputInfo ();
		newInfo.frameCountUp = Time.frameCount;
		newInfo.positionUp = newPos;
		newInfo.ballFrameCount = -1;

		InputStorage.Add (newInfo);
		Debug.Log ("Adding finger up");
	}

	public void HandleBallLaunched(Vector2 newPos) {
		InputInfo newInfo = new InputInfo ();
		newInfo.ballFrameCount = Time.frameCount;
		newInfo.ballLaunchLocation = newPos;
		InputStorage.Add (newInfo);
		Debug.Log ("Adding launched ball");
	}

	public void HandleBallLaunched(Vector2 newPos, bool firstBall) {
		InputInfo newInfo = new InputInfo ();
		newInfo.ballFrameCount = Time.frameCount;
		newInfo.ballLaunchLocation = newPos;
		newInfo.firstBall = true;
		InputStorage.Add (newInfo);

		Debug.Log ("adding first ball: " + newPos + "; " + firstBall);
	}

	public IEnumerator PlayReplay() {
		Debug.Log ("Playing replay");

		int frameCountToNow = Time.frameCount;
		int replayFrameCount = Time.frameCount - frameCountToNow;

		int[] frames = new int[] {
			InputStorage [InputStorage.Count - 1].ballFrameCount,
			InputStorage [InputStorage.Count - 1].frameCountDown,
			InputStorage [InputStorage.Count - 1].frameCountUp
		};

		int framesToPlay = Mathf.Max(frames);

		int replayIndex = 0;

		while (replayFrameCount < framesToPlay) {
			if (InputStorage [replayIndex].frameCountDown == replayFrameCount) {
				Vector2 pos = InputStorage [replayFrameCount].positionDown;
//				Hand.instance.ReplayFingerDown (pos);
				replayIndex++;

			}

			if (InputStorage [replayIndex].frameCountUp == replayFrameCount) {
				Vector2 dir = InputStorage [replayFrameCount].positionUp - InputStorage [replayFrameCount].positionDown;
//				Hand.instance.ReplayFingerUp (dir);
				replayIndex++;

			}

			if (InputStorage [replayIndex].ballFrameCount == replayFrameCount) {
//				Debug.Log (InputStorage [replayIndex].ballFrameCount);
//				Debug.Log (InputStorage [replayFrameCount].ballLaunchLocation);
//				Debug.Log (InputStorage [replayFrameCount].firstBall);
				BallManager.GetInstance ().LaunchBall (InputStorage [replayFrameCount].ballLaunchLocation, InputStorage [replayFrameCount].firstBall);
				replayIndex++;
			}

			replayFrameCount++;

			Debug.Log (replayIndex);
			Debug.Log (replayFrameCount);
		}
		yield return 0;
	}
}
