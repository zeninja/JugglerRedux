using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour {

	public GameObject line1;
	public GameObject line2;

	int lineCount = 0;

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			GameObject l1 = Instantiate(line1) as GameObject;
			GameObject l2 = Instantiate(line2) as GameObject;
		}
	}
}
