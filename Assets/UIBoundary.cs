﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoundary : MonoBehaviour {

	public static Vector3 position;

	// Use this for initialization
	void Start () {
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos() {
		GUI.color = Color.red;
		Gizmos.DrawLine(transform.position + Vector3.left * 10,  transform.position + Vector3.right * 10);
	}
}
