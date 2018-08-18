using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTrail : MonoBehaviour {

	public bool drawTrail = false;

	public float refreshInterval = 10;

	public GameObject dot;

	float fadeDuration = .125f;

	// Use this for initialization
	void Start () {
		
	}
	
	float nextDrawTime;

	// Update is called once per frame
	void Update () {
		bool velocityPositive = GetComponent<NewBallArtManager>().VelocityPositive();
		Debug.Log(velocityPositive);
		drawTrail = !velocityPositive && TimeManager.TimeSlowing();
		// drawTrail = !velocityPositive;
		if(drawTrail) {
			if(Time.time > nextDrawTime) {
				DrawDot();
				nextDrawTime = Time.time + refreshInterval;
			}
		}
	}

	void DrawDot() {
		GameObject d = Instantiate(dot) as GameObject;
		d.transform.position = transform.position;
		d.transform.localScale = transform.parent.localScale;
		d.GetComponent<SpriteRenderer>().color = GetComponent<NewBallArtManager>().myColor;
		// float t = ;
		// d.GetComponent<Dot>().fadeDuration = Extensions.GetSmoothStepRange(fadeDuration, TimeManager.timeScalePercent);
		d.GetComponent<Dot>().fadeDuration = fadeDuration;
	}
}
