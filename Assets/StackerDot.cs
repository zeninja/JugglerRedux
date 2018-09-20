using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerDot : MonoBehaviour {

	void Start() {
		GetComponent<MeshRenderer>().sortingLayerName = "GameOver";
	}

	public void SetTargetRadius(float r) {
		GetComponent<ProceduralCircle>().radius = r;
	}

	public void SetInfo(Vector2 anchor, Color dotColor, int depth) {
		GetComponent<ProceduralCircle>().anchorPos = anchor;
		GetComponent<ProceduralCircle>().color = dotColor;
		GetComponent<ProceduralCircle>().depth = depth;

		startColor = dotColor;
	}

	public void SetColor(Color dotColor) {
		GetComponent<ProceduralCircle>().color = dotColor;
	}

	public IEnumerator StartRainbow(Color[] a_colors, float delay) {
		colors = a_colors;
		yield return new WaitForSeconds(delay);
		StartCoroutine(Rainbow());
	}

	public void EndRainbow() {
		StopAllCoroutines();
	}

	float d = .125f;

	Color[] colors;
	int colorIndex = 0;

	Color startColor;

	IEnumerator Rainbow() {
		SetColor(colors[colorIndex]);
		yield return StartCoroutine(Extensions.Wait(d));
		colorIndex = (colorIndex + 1 ) % colors.Length;

		StartCoroutine(Rainbow());
	}

	public void ReturnToDefaultColor() {
		Debug.Log("Resturning to start");
		SetColor(startColor);
	}
}
