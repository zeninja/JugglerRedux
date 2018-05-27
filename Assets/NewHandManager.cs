using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHandManager : MonoBehaviour {

	public GameObject handPrefab;

	// finger index : hand
	Dictionary<int, GameObject> hands = new Dictionary<int,GameObject>();
	static int _handCount;

	// Use this for initialization
	void Start () {
		
	}

	void OnFingerDown(FingerEvent e) {
		Debug.Log("Finger down");
		SpawnHand(e.Finger.Index);
	}

	void OnFingerUp(FingerEvent e) {
		GameObject targetHand = GetHandByIndex(e.Finger.Index);
		hands.Remove(e.Finger.Index);
		Destroy(targetHand);
	}

	void SpawnHand(int fingerIndex) {
		GameObject hand = Instantiate(handPrefab) as GameObject;
		hand.transform.position = Extensions.ScreenToWorld(Input.mousePosition);
		hands.Add(fingerIndex, hand);
		hand.GetComponent<NewHand>().handIndex = fingerIndex;
		_handCount++;

		if(NewGameManager._spawnBallsByTouchCount) {
			if(NewBallManager._ballCount < Input.touchCount) {
				EventManager.TriggerEvent("SpawnBall");
			}
		}
	}

	GameObject GetHandByIndex(int fingerIndex) {
		return hands[fingerIndex];
	}
}
