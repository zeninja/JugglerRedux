using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour {

	Vector3 throwDirection, throwPos, lastThrowPos;

	public float throwForce = 4;

	public float smoothingSpeed = 2;
	public float throwDirectionLerpSpeed = 2;

	public int handIndex;

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate() {
		transform.position = Input.GetTouch(handIndex).position;

		// transform.position = GetMousePos();    //Vector3.Lerp(transform.position, GetMousePos(), Time.deltaTime * mouseSmoothing);
		
		throwPos = Vector3.Lerp(throwPos, transform.position, Time.deltaTime * throwDirectionLerpSpeed);
		throwDirection = throwPos - lastThrowPos;

		lastThrowPos = throwPos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		other.GetComponent<NewBall>().HandleCatch(throwDirection.normalized * throwForce);
	}

	Vector3 GetMousePos() {
		return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
	}
}
