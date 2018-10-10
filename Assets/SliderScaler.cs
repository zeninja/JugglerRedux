using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SliderScaler : MonoBehaviour {
	
	public Vector3 inversePoint;
	public Vector3 inverseDirection;

		public Vector3 transformPoint;
	public Vector3 transformDirection;

	// Update is called once per frame
	void Update () {
		inversePoint = transform.InverseTransformPoint(transform.localScale);
		inverseDirection = transform.InverseTransformDirection(transform.localScale);
		
		transformPoint = transform.TransformPoint(transform.localScale);
		transformDirection = transform.TransformDirection(transform.localScale);
		transform.localScale = inversePoint;
	}
}
