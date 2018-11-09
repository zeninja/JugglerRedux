using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour {

	public GameObject line1;
	public GameObject line2;

	int lineCount = 0;

	// public int maskIndex, lineIndex;

	public int stencil1 = 1, stencil2 = 2;

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			GameObject l1 = Instantiate(line1) as GameObject;
			GameObject l2 = Instantiate(line2) as GameObject;

			l1.GetComponent<LineMaskManager>().UpdateMaskIndex(stencil1);
			l2.GetComponent<LineMaskManager>().UpdateMaskIndex(stencil2);

			// l1.GetComponent<LineMaskManager>().index = 1;
			// l2.GetComponent<LineMaskManager>().index = 2;
		}
	}
}
