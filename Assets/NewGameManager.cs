using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : MonoBehaviour {

	#region instance
	private static NewGameManager instance;
	public static NewGameManager GetInstance() {
		return instance;
	}
	#endregion

	public bool spawnBallsByTouchCount;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {
		EventManager.TriggerEvent("SpawnBall");
	}
}
