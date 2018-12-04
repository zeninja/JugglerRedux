using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerDot : MonoBehaviour {

	// ProceduralCircle pc;

	LineRenderer circle;

	void Start() {
		// GetComponent<MeshRenderer>().sortingLayerName = "GameOver";
		circle = GetComponent<LineRenderer>();
	}

	public float radius;

	public void SetTargetRadius(float r) {
		circle.startWidth = r;
		circle.endWidth = r;
	}

	public void SetInfo(Vector2 anchor, Color dotColor, int depth) {
		transform.position = anchor;
		
		circle.material.color = dotColor;
		circle.sortingOrder = depth;
	}

	public void SetColor(Color dotColor) {
		circle.material.color = dotColor;
	}

	public IEnumerator HideDot(float d, float endRadius) {
		float t = 0;
		
		while(t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;

			SetTargetRadius(radius - (radius - endRadius) * p);

			yield return new WaitForFixedUpdate();
		}
		gameObject.SetActive(false);
	}

	Color startColor;

	public void ReturnToDefaultColor() {
		SetColor(startColor);
	}
}
