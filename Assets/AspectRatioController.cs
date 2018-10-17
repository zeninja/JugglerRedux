using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioController : MonoBehaviour {

	public UISlider slider;
	UnityEngine.UI.CanvasScaler scaler;

	public float landscapeAspectRatio;
	public float portraitAspectRatio;

	// Use this for initialization
	void Start () {
		scaler = GetComponent<CanvasScaler>();
	}
	
	// Update is called once per frame
	void Update () {
		landscapeAspectRatio = Camera.main.aspect;
		portraitAspectRatio  = 1 / landscapeAspectRatio;

		UpdateAspectRatios();
	}

	// [Range(0, 1)]
	// public float float21, float17, float15, float13;

	void UpdateAspectRatios() {
		if(portraitAspectRatio > 2) {
			// 19.5:9 (iPhone X and other very tall screens)
			scaler.matchWidthOrHeight = .5f;
			slider.widthModifier = .85f;
		} else {
			// Everything else
			scaler.matchWidthOrHeight = 1;
			slider.widthModifier = 1;
		}

		// if(portraitAspectRatio >= 1.7f) {
		// 	//   16:9 (Reasonable screens)
		// 	scaler.matchWidthOrHeight = 1;
		// } else 
		// if(portraitAspectRatio >= 1.49f) {
		// 	//    3:2 (Old iPhones)
		// 	scaler.matchWidthOrHeight = 1;
		// } else
		// if(portraitAspectRatio < 1.5f) {
		// 	//    4:3 screens
		// 	scaler.matchWidthOrHeight = 1;
		// }
	}
}
