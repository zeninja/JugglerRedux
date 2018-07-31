using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrail : MonoBehaviour {

	public BallTrailer m_BallTrailPrefab;
	public ParticleSystem particleSystem;

	List<GameObject> trailParts = new List<GameObject>();

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

	public int frameInterval = 5;
	

	IEnumerator ShowBallTrail() {
		int iterations = 0;

		if(GetComponent<Rigidbody2D>().velocity.y > 0) {
			BallTrailer ballTrailer = Instantiate(m_BallTrailPrefab);
			ballTrailer.transform.position = transform.position;

			int elapsedFrames = 0;
			while(elapsedFrames < frameInterval) {
				yield return new WaitForEndOfFrame();
				elapsedFrames++;
			}

			StartCoroutine("ShowBallTrail");
			iterations++;
		}
	}
}
