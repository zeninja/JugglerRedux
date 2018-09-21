using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimator : MonoBehaviour {
 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)) {
			ShowLogo();
		}
	}

	public void ShowLogo() {
		GetComponent<Animation>().Play("New Animation");
	}
}
