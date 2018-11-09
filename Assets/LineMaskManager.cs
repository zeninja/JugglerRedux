using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMaskManager : MonoBehaviour {

	// int layersPerLine = 2;

	LineRenderer line, mask;

	int layersPerLine = 2;
	int renderQueue;

	public Material lineMat, maskMat;

	// Use this for initialization
	void Awake () {
		mask = transform.Find("Mask").GetComponent<LineRenderer>();
		line = transform.Find("Line").GetComponent<LineRenderer>();

		mask.material = maskMat;
		line.material = lineMat;

		renderQueue = lineMat.renderQueue;
	}

	public void UpdateMaskIndex(int index) {
		mask.material.renderQueue = renderQueue + index * layersPerLine;
		line.material.renderQueue = renderQueue + index * layersPerLine + 1;
	}
}
