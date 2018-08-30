using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameTrail : MonoBehaviour {

	public bool drawTrail = false;

	public float refreshInterval = 10;

	public GameObject dot;

	// public float fadeDuration = .245f;

	public Color dotColor;

	public float defaultScale;

	public int numDots = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	float nextDrawTime;

	// Update is called once per frame
	void Update () {
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
		d.transform.localScale = Vector2.one * defaultScale;
		// d.transform.localScale = transform.parent.localScale;
		d.GetComponent<SpriteRenderer>().color = dotColor;

		d.GetComponent<Dot>().fadeDuration = (float)refreshInterval * (float)numDots;
	}
}
