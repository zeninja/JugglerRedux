using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public UnityEngine.UI.Image ballTimer; 

	public static UIManager instance;

	void Awake() {
		instance = this;
	}
}
