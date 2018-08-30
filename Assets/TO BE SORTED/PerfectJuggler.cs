using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectJuggler : MonoBehaviour {

	List<NewBall> allBalls;

	public float triggerHeight = -2f;

	public Vector2 throwVector = new Vector2(0, 5f);

	// Use this for initialization
	void Start () {
		allBalls = new List<NewBall>();
	}
	
	// Update is called once per frame
	void Update () {
		FindBalls();
		Juggle();
	}

	void FindBalls() {
		NewBall[] list = GameObject.FindObjectsOfType<NewBall>();

		allBalls.Clear();
		for(int i = 0; i < list.Length; i++) {
			allBalls.Add(list[i]);
		}
	}

	void Juggle() {
		for (int i = 0; i < allBalls.Count; i++) {
			if(allBalls[i].transform.position.y < triggerHeight) {
				allBalls[i].GetCaught();
				allBalls[i].GetThrown(throwVector);
			}
		}
	}

	public void SetBallList(List<NewBall> balls) {

	}
}
