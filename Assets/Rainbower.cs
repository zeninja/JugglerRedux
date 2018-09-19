using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbower : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
			StartRainbow();
			// StartCoroutine(StartRainbow());
		}
	}
	
	List<StackerDot> dots;

	public void SetDots(List<StackerDot> a_dots) {
		dots = a_dots;
	}

	public Color[] colors;
	public float timeBetweenBows = .125f;

	public void StartRainbow() {

		if(NewBallManager.GetInstance() != null) {
			Debug.Log("Setting colors");
			colors = new Color[NewBallManager._ballCount];
			colors = NewBallManager.GetInstance().m_BallColors;
		}

		for(int i = 0; i < dots.Count; i++) {
			StartCoroutine(dots[i].StartRainbow(colors, i * timeBetweenBows));

			// dots[i].StartRainbow(colors, i)
			// yield return StartCoroutine(Extensions.Wait(timeBetweenBows));
		}

		// for(int i = 0; i < dots.Count; i++) {
		// 	dots[i].EndRainbow();
		// 	yield return StartCoroutine(Extensions.Wait(timeBetweenBows));
		// }
	}
}
