using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColorManager : MonoBehaviour {

	#region instance
	private static BallColorManager instance;
	public static BallColorManager GetInstance() {
		return instance;
	}
	#endregion

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
		
		ballColors = defaultColors;
	}

	public Color[] ballColors = new Color[9];
	public Color[] defaultColors = new Color[9];
}
