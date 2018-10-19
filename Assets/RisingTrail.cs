using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingTrail : MonoBehaviour {

	LineRenderer line;

	List<Vector3> trailPositions;

	// Use this for initialization
	void Start () {
		trailPositions = new List<Vector3>();
		line = GetComponent<LineRenderer>();
		line.startWidth = NewBallManager.GetInstance().ballScale;
		line.endWidth   = NewBallManager.GetInstance().ballScale;

	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponentInParent<NewBallArtManager>().VelocityPositive()) {

			trailPositions.Add(transform.position);
			DrawTrail();

			line.material.color = GetComponentInParent<NewBallArtManager>().myColor;
			line.material.color = GetComponentInParent<NewBallArtManager>().myColor;
			line.enabled = true;
		} else {
			line.enabled = false;
			trailPositions.Clear();
		}
	}

	public int trailLength;

	void DrawTrail() {
		 
		Vector3[] trail = new Vector3[trailLength];
		for(int i = 0; i < trailLength; i++) {
			int index = trailPositions.Count - i - 1;
			index = Mathf.Max(index, 0);
			trail[i] = trailPositions[index];
		}

		 line.positionCount = trail.Length;
		 line.SetPositions(trail);
	}
}
