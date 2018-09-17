using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningManager : MonoBehaviour {

	public GameObject text1;

	public GameObject warningLine;
	List<LineRenderer> warningLines;

	public float width1;
	public float width2;


	// Use this for initialization
	void Start () {
		SpawnWarningLines();
	}
	
	// Update is called once per frame
	void Update () {
		AdjustWarningLines();
	}

	IEnumerator FlashWarning() {
		float  on = .5f;
		float off = .4f;

		text1.SetActive(true);
		yield return new WaitForSeconds(on);

		text1.SetActive(false);
		yield return new WaitForSeconds(off);
	}

	public int numLines = 20;

	void SpawnWarningLines() {

	}

	void AdjustWarningLines() {

	}
}
