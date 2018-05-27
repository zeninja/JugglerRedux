using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : MonoBehaviour {

	public bool spawnBallsByTouchCount;
	public static bool _spawnBallsByTouchCount;

	// Use this for initialization
	void Start () {
		EventManager.TriggerEvent("SpawnBall");

		_spawnBallsByTouchCount = spawnBallsByTouchCount;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
