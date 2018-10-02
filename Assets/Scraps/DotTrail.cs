using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotTrail : MonoBehaviour {

	public float refreshInterval = 10;

	public GameObject dot;

	public float fadeDuration = .245f;
	
	float nextDrawTime;

	// Update is called once per frame
	void FixedUpdate () {
		if(DrawTrail()) {
			if(Time.time > nextDrawTime) {
				DrawDot();
				nextDrawTime = Time.time + refreshInterval;
			}
		}
	}

	bool DrawTrail() {
		bool velocityPositive = GetComponent<NewBallArtManager>().VelocityPositive();
		bool drawTrail = !velocityPositive && TimeManager.TimeSlowing() && NewBallManager._ballCount >= 1 && !GetComponentInParent<NewBall>().IsHeld();
		return drawTrail;
	}

	void DrawDot() {
		GameObject d = Instantiate(dot) as GameObject;
		d.transform.position = transform.position;
		d.transform.localScale = transform.parent.localScale;
		d.GetComponent<SpriteRenderer>().color = GetComponent<NewBallArtManager>().myColor;
		d.GetComponent<Dot>().fadeDuration = fadeDuration;

		d.GetComponent<Dot>().SetSprite(endgameTriggered);
	}

	bool endgameTriggered = false;

	public void TriggerEndgame() {
		endgameTriggered = true;
	}
}
