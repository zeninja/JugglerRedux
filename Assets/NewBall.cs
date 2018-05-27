using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBall : MonoBehaviour {

	public float scale = .25f;
	public bool freezeOnCatch;


	// Use this for initialization
	void Start () {
		transform.localScale = Vector2.one * scale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleCatch(Vector3 throwDirection) {
		// GetComponent<Rigidbody2D>().AddForce(throwDirection, ForceMode2D.Impulse);
		EventManager.TriggerEvent("BallCaught");
	}
}
