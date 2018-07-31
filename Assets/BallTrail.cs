using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrail : MonoBehaviour {

	public GameObject m_BallTrailPrefab;
	public ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		// EventManager.StartListening("BallThrown", ShowTrail);
	}


	
	// Update is called once per frame
	void Update () {
		
	}

	void ShowTrail() {
		Debug.Log("Checking ball thrown");
		if (GetComponent<NewBall>().m_BeingThrown) {
			Debug.Log("ball was thrown");
			StartCoroutine("ShowBallTrail");
		}
	}

	public int numFramesToWait = 10;
	
	IEnumerator ShowBallTrail() {
		// if(numFramesToWait > 0) {
			GameObject ballTrailer = Instantiate(m_BallTrailPrefab) as GameObject;
			ballTrailer.transform.position = transform.position;

			int elapsedFrames = 0;
			while(elapsedFrames < numFramesToWait) {
				yield return new WaitForEndOfFrame();
				elapsedFrames++;
			}

			// numFramesToWait--;
			StartCoroutine("ShowBallTrail");
		// }
	}
}
