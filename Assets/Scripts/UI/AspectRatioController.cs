using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioController : MonoBehaviour {

	public UISlider slider;
	UnityEngine.UI.CanvasScaler scaler;

	public float landscapeAspectRatio;
	public float portraitAspectRatio;


	// THIS IS TEMPORARY AND NOT VERY GOOD
	// SHOULD CONVERT THIS TO A UI IMAGE SO IT SCALES PROPERLY
	public GameObject plate;

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

	[Range(0, 1)]
	public float matchPCT = 0;
	public float widthModifier;

	public float tallX;
	public float wideX;

	void UpdateAspectRatios() {
		if(portraitAspectRatio > 2) {
			// 19.5:9 (iPhone X and other very tall screens)
			scaler.matchWidthOrHeight = matchPCT;
			slider.widthModifier = widthModifier;

			plate.transform.localPosition = new Vector2(tallX, plate.transform.localPosition.y);
		} else {
			// Everything else
			scaler.matchWidthOrHeight = 1;
			slider.widthModifier = 1;

			plate.transform.localPosition = new Vector2(wideX, plate.transform.localPosition.y);
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
