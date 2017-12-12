using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSpriteManager : MonoBehaviour {

	Button muteButton;
	Image mySprite;

	public Sprite muted;
	public Sprite notMuted;

	// Use this for initialization
	void Start () {
		muteButton = GetComponent<Button> ();
		mySprite = GetComponent<Image>();
		muteButton.onClick.AddListener (UpdateImage);
	}

	public void UpdateImage() {
		Sprite targetSprite;

		targetSprite = AudioManager.muted ? muted : notMuted;
		mySprite.sprite = targetSprite;
	}
}
