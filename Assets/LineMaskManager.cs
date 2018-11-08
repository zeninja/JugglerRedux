using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMaskManager : MonoBehaviour {

	public int index = 0;
	int layersPerLine = 2;

	GameObject line;
	GameObject mask;

	// Use this for initialization
	void Start () {
		line = transform.GetChild(0).gameObject;
		mask = transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		mask.GetComponent<LineRenderer>().material.renderQueue = 3000 + index * layersPerLine;
		line.GetComponent<LineRenderer>().material.renderQueue = 3000 + index * layersPerLine + 1;
	}
}
