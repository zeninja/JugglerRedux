using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		GUI.color = Color.black;
		foreach(Touch t in Input.touches) {
			GUI.Label(new Rect(t.position.x, t.position.y, 100, 100), t.position.ToString());
		}
	}
}