using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeSpriteManager : MonoBehaviour {

	UnityEngine.UI.Image mySprite;

	public Sprite muted;
	public Sprite notMuted;

	// Use this for initialization
	void Start () {
		mySprite = GetComponent<UnityEngine.UI.Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateImage() {
		Sprite targetSprite;
		Debug.Log("updating sprite");

		targetSprite = AudioManager.muted ? muted : notMuted;
		mySprite.sprite = targetSprite;

	}
}
