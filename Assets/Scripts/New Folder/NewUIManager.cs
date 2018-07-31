using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUIManager : MonoBehaviour {

	public Text slapForceText;
	public Text grabForceText;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		slapForceText.text = NewHandManager.GetInstance().touchSlapThrowForce.ToString("F2");
		grabForceText.text = NewHandManager.GetInstance().touchGrabThrowForce.ToString("F2");
	}
}
