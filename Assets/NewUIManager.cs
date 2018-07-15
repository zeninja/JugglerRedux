using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUIManager : MonoBehaviour {

	public Text throwSens;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		throwSens.text = NewHandManager.GetInstance().immediateThrowForce.ToString();
	}
}
