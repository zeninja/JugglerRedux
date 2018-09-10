using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDirectionSprite : MonoBehaviour {

	public void UpdateThrowSprite() {
		int i = NewHandManager.dragUpToThrow ? 1 : 0;
		Vector3 targetRotation = new Vector3(0, 0, 180 * i);

		// StartCoroutine("RotateSprite", targetRotation);
		transform.rotation = Quaternion.Euler(targetRotation);
	}

	IEnumerator RotateSprite(Vector3 targetRotation) {
		float t = 0;
		float d = .015f;

		Vector3 diff = transform.rotation.eulerAngles - targetRotation;
		Vector3 startRotation = transform.rotation.eulerAngles;

		while(t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;

			transform.rotation = Quaternion.Euler(startRotation + diff * p);

			yield return new WaitForFixedUpdate();
		}
	}

}
