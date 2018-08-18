using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorEffect : MonoBehaviour {

	public bool useStartColor;
	public Color startColor;
	public Color endColor;

	// Use this for initialization
	void Start () {
		// startColor = Camera.main.backgroundColor;
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.backgroundColor = Color.Lerp(startColor, endColor, TimeManager.timeScalePercent);
	}
}
