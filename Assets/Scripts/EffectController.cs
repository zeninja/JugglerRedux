using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

	// public CircleEffect circleEffect;
	// public LineEffect lineEffect;

	// public Color ringColor;

	#region Screenshake
	[Range(0f, 1f)]
	float trauma;

	// damage and stress increases trauma (+.2, +.5 etc);
	float angle;
	float maxAngle;
	float shake;

	float offsetX, maxOffsetX;
	float offsetY, maxOffsetY;
	#endregion

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C)) {
			// SpawnGrowingRing(transform.position);
		}

		// Screenshake
		// angle = maxAngle * shake * GetRandomFloatNegOneToOne();
	}

	public float circleGrowSpeed = .5f;

	// public void SpawnGrowingRing(Vector2 position) {
	// 	CircleEffect newCircle = Instantiate(circleEffect);
		
	// 	ringColor = GetComponentInChildren<NewBallArtManager>().myColor;
	// 	// ringColor.a = .75f;

	// 	newCircle.transform.position = position; 
	// 	newCircle.spreadSpeed = circleGrowSpeed;
	// 	newCircle.GetComponent<LineRenderer>().material.color = ringColor;
	// 	newCircle.GetComponent<LineRenderer>().material.color = ringColor;
	// }

	// float GetRandomFloatNegOneToOne() {
	// 	return Random.Range(-1f, 1f);
	// }

	// float GetPerlinNoise() {
	// 	// hey maybe actually fill this in at some point
	// 	return 0;
	// }
}
