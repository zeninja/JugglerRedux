using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trails : MonoBehaviour {

	int[,] colorIndexGrid = new int[Screen.width, Screen.height];
	public Texture2D background;


	// Use this for initialization
	void Start () {
		background = new Texture2D (Screen.width, Screen.height);
		GetComponent<Renderer>().material.mainTexture = background;
	}
	
	// Update is called once per frame
	void Update () {
		for (int x = 0; x < Screen.width; x++) {
			for (int y = 0; y < Screen.height; y++) {
				
				if (Physics2D.Raycast (new Vector2 (x, y), Vector2.zero, 0)) {
					colorIndexGrid [x, y]++;
				}
			}
		}

		for (int x = 0; x < Screen.width; x++) {
			for (int y = 0; y < Screen.height; y++) {
				if (colorIndexGrid [x, y] > 0) {
					
				}
			}
		}
	}
}
