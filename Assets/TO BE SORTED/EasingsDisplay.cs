using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingsDisplay : MonoBehaviour {

	LineRenderer line;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		DrawEasing();
	}

	void DrawEasing() {
		Vector3[] positions = new Vector3[100];
		for(int i = 0; i < 100; i++) {
			positions[i] = new Vector3((float)i / 100f, EZEasings.SmoothStep3((float)i / 100f), 0);
		}
		line.SetPositions(positions);
		line.positionCount = positions.Length;
	}
}
