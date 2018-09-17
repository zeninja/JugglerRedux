using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLine : MonoBehaviour {

	public float scrollSpeed;
	public int   scrollDirection;
	public float width;
	public float lineWidth;

	[System.NonSerialized] public float offset;

	public Material inside, outside;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		offset += scrollSpeed * scrollDirection * Time.fixedDeltaTime;
		// Debug.Log(offset);
		offset = Mathf.Clamp01(offset);
		if(offset == 1) {
			offset = 0;
		}

		transform.position = new Vector2(offset * width - width / 2, 0);

		GetComponent<LineRenderer>().startWidth = lineWidth;
		GetComponent<LineRenderer>().endWidth   = lineWidth;
	}   
}
