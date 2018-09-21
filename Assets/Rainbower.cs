using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbower : MonoBehaviour {

	public List<StackerDot> dots;
	public Color[] colors;
	public float waveProgressionRate = .125f;


	void Start() {
		PrepWave();
	}

	// Update is called once per frame
	void Update () {
		completion = Mathf.Clamp01(completion);


		if(Input.GetKeyDown(KeyCode.R)) {
			// StartRainbow();
			// StartCoroutine(StartRainbow());

			StartCoroutine(Wave(waveDuration));
		}

		PrepWave();

		if(Input.GetKeyDown(KeyCode.D)) {
			StartCoroutine(DoTheWave(numBalls, GetTideDuration((float)numBalls / (float)9)));
		}

	}

	public int numBalls;

	public float completion;

	public Extensions.Property tideDuration;

	float GetTideDuration(float completionPercentage) {
		float calculatedDuration = tideDuration.start + (tideDuration.end - tideDuration.start) * completionPercentage;
		Debug.Log(calculatedDuration);
		return calculatedDuration;
	}

	public void SetDots(List<StackerDot> a_dots) {
		dots = a_dots;
	}

	void PrepWave() {
		if(NewBallManager.GetInstance() != null) {
			colors = new Color[NewBallManager._ballCount];

			for(int i = 0; i < NewBallManager._ballCount; i++) {
				colors[i] = BallColorManager.GetInstance().ballColors[i];
			}
		} else {
			colors = new Color[numBalls];

			for(int i = 0; i < numBalls; i++) {
				colors[i] = BallColorManager.GetInstance().ballColors[i];
			}
		}
	}

	public IEnumerator DoTheWave(int numWaves, float duration) {
		float waveDelay = (float)duration / (float)numWaves;

		Debug.Log(waveDelay);

		for(int i = 0; i < numWaves; i++) {
			StartCoroutine(Wave(waveDuration));
			yield return StartCoroutine(Extensions.Wait(waveDelay));
		}
	}

	public float waveDuration = .3f;
	IEnumerator Wave(float duration) {
		int waveRings = colors.Length;
		int iterations = dots.Count + waveRings;
		float split = duration / (float)iterations;

		for(int index = 0; index < iterations; index++) {
			for(int i = 0; i < dots.Count; i++) {
				// Adjust the set of rings in the wave
				for (int j = 0; j < waveRings; j++) {
					if (index - j >= 0  && index - j < dots.Count) {
						dots[index - j].SetColor(colors[j]);
					}
				}

				// Reset rings that have been passed in the wave
				if(i <= index - waveRings) {
					dots[i].ReturnToDefaultColor();
				}
			}
			yield return StartCoroutine(Extensions.Wait(split));
		}
	}







	// IEnumerator Wave() {
	// 	int index = 0;
	// 	int waveRings = colors.Length;
	// 	while ( index < dots.Count + waveRings) {
	// 		// Progress through every dot in the stack
	// 		for(int i = 0; i < dots.Count; i++) {
	// 			// int waveIndex = index - waveRings;
	// 			// Adjust the set of rings in the wave
	// 			for (int j = 0; j < waveRings; j++) {
	// 				if (index - j >= 0  && index - j < dots.Count) {
	// 					dots[index - j].SetColor(colors[j]);
	// 				}
	// 			}
	// 			Debug.Log(index - waveRings);
	// 			// Reset rings that have been passed in the wave
	// 			if(i <= index - waveRings) {
	// 				dots[i].ReturnToDefaultColor();
	// 			}
	// 		}
	// 		yield return StartCoroutine(Extensions.Wait(waveProgressionRate));
	// 		index++;
	// 	}
	// }
}
