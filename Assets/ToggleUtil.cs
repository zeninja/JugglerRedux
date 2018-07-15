using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUtil : MonoBehaviour {

	public Toggle myToggle;

	public Image target;

	public Sprite onSprite;
	public Sprite offSprite;

	public void OnToggleValueChanged() {
		target.sprite = myToggle.isOn ? onSprite : offSprite;
	}
}
