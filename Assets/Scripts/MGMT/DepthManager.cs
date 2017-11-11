using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthManager : MonoBehaviour {

	Transform ring;

	// Use this for initialization
	void Awake () {
		ring = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDepth(int newDepth) {
		GetComponent<SpriteRenderer> ().sortingOrder = newDepth * 2;
		ring.GetComponent<SpriteRenderer> ().sortingOrder = newDepth * 2 - 1;
	}
}
