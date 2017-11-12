using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnimation : MonoBehaviour {

	[System.Serializable]
	public class HighScoreAnimationInfo {
		public float highScoreDuration = 0;
		public float flashDuration = .15f;
		public int timesToFlash = 5;
	}
		
	[System.Serializable]
	public class ScoreAnimInfo {
		public Vector3 defaultScale = new Vector3 (.25f, .25f, .25f);
		public float bumpMagnitude = 1.2f;
		public float animationDuration = .15f;
	}

	[System.Serializable]
	public class CountdownInfo {
		public float countdownDuration = .5f;
	}

	[System.Serializable]
	public class FlashInfo {
		public int timesToFlash = 3;
		public float flashDuration = .15f;
	}

	[SerializeField]
	HighScoreAnimationInfo highScoreAnimationInfo = new HighScoreAnimationInfo();
	[SerializeField]
	ScoreAnimInfo scoreAnimInfo = new ScoreAnimInfo();
	[SerializeField]
	CountdownInfo countdownInfo = new CountdownInfo();
	[SerializeField]
	FlashInfo flashInfo = new FlashInfo();

	public GameObject score;
	public GameObject highScore;
	public GameObject highscoreText;

	public Gradient highScoreGradient;

	public static bool flashingScore;

	private static ScoreAnimation instance;
	private static bool instantiated;

	public static ScoreAnimation GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(ScoreAnimation)) as ScoreAnimation;
			if (!instance)
				Debug.Log("No ScoreAnimation!!");
		}
		return instance;
	}

	void Awake() {
		if (!instantiated) {
			instance = this;
			instantiated = true;
		}
	}

	// Use this for initialization
	void Start () {
		scoreAnimInfo.defaultScale = score.transform.localScale;
		highScoreAnimationInfo.highScoreDuration = highScoreAnimationInfo.flashDuration * highScoreAnimationInfo.timesToFlash * 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleCatch() {
		StopCoroutine ("BumpScore");
		StartCoroutine ("BumpScore");
	}

	public IEnumerator BumpScore () {
		// emphasize each catch/score increase
		if (!flashingScore) {
			score.ScaleTo (scoreAnimInfo.defaultScale * scoreAnimInfo.bumpMagnitude, scoreAnimInfo.animationDuration, 0, EaseType.easeOutCirc);
			yield return new WaitForSeconds (scoreAnimInfo.animationDuration);
			score.ScaleTo (scoreAnimInfo.defaultScale, scoreAnimInfo.animationDuration, 0, EaseType.easeOutCirc);
		}
	}

	public IEnumerator FlashScore() {
		StopCoroutine ("BumpScore");												// emphasize the milestone scores

		int numFlashes = 0;
		flashingScore = true;

		while (numFlashes < flashInfo.timesToFlash) {
			score.SetActive (false);
			yield return new WaitForSeconds (flashInfo.flashDuration);
			score.SetActive (true);
			yield return new WaitForSeconds(flashInfo.flashDuration);
			numFlashes++;
		}
		flashingScore = false;
		yield return 0;
	}

	public IEnumerator ShowNewHighScore() {
		StartCoroutine(RainbowText());

		int numFlashes = 0;

		while (numFlashes < highScoreAnimationInfo.timesToFlash) {
			highscoreText.SetActive (false);
			score.SetActive (false);
			yield return new WaitForSeconds (highScoreAnimationInfo.flashDuration);
			highscoreText.SetActive (true);
			score.SetActive (true);
			yield return new WaitForSeconds(highScoreAnimationInfo.flashDuration);
			numFlashes++;

			yield return new WaitForEndOfFrame();
		}
//		highscoreText.SetActive (false);
		yield return 0;
	}

	IEnumerator RainbowText() {
		// Could take this out since it also happens in ShowNewHighSCore
		float startTime = Time.time;
		float elapsedTime = Time.time - startTime;
		while (elapsedTime < highScoreAnimationInfo.highScoreDuration) {
			float t = elapsedTime / highScoreAnimationInfo.highScoreDuration;
			elapsedTime = Time.time - startTime;
			highscoreText.GetComponent<UnityEngine.UI.Text>().color = highScoreGradient.Evaluate(t);
			score.GetComponent<UnityEngine.UI.Text>().color = highScoreGradient.Evaluate(t);
			yield return new WaitForEndOfFrame();
		}
		highscoreText.SetActive(false);
		score.GetComponent<UnityEngine.UI.Text>().color = Color.black;
		yield return 0;
	}

	public IEnumerator CountdownScore() {
		float timeBetweenCounts = countdownInfo.countdownDuration / ScoreManager.score;

		while (ScoreManager.score > 0) {
			ScoreManager.score--;
			ScoreManager.GetInstance().scoreDisplay.text = ScoreManager.score.ToString ();
			yield return new WaitForSeconds (timeBetweenCounts);
		}
		yield return 0;
	}

	public void HandleGameOver() {
		if (flashingScore) {
			StopCoroutine(FlashScore());
			flashingScore = false;

			score.SetActive(true);
		}
	}
}