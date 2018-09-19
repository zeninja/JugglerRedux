using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingSquash : MonoBehaviour {

	NewBall m_Ball;
	public float squashAmount;

	Vector2 catchPos;
	Vector2 peakPos;
	float throwMagnitude;

	// Use this for initialization
	void Start () {
		m_Ball = GetComponentInParent<NewBall>();
		// m_BallPredictor = GetComponentInParent<BallPredictor>();
	}

	void HandleCatch() {
		catchPos = transform.position;
		Debug.Log("got caught");
	}

	public void SetPeakPos(Vector2 a_peakPos) {
		throwMagnitude = (a_peakPos - catchPos).magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		// if(m_Ball.m_State == NewBall.BallState.rising) {
		// 	float progressAlongLine = ((Vector2)transform.position - catchPos).magnitude / throwMagnitude;
		// 	float x = 1 - squashAmount * progressAlongLine;
		// 	float y = 1 + squashAmount * progressAlongLine;

		// 	squashAmount = Mathf.Clamp01(squashAmount);	

		// 	transform.localScale = new Vector3(x, y, 0);
		// }
	}
}
