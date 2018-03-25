using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowDirectionUtil : MonoBehaviour {

	public float animationDuration = .3f;
	public AnimationCurve spinCurve;
	public float spinAmount = 180;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AnimateSwitch() {
		StartCoroutine("SpinIcon");
	}

	IEnumerator SpinIcon() {
		GetComponent<Button>().interactable = false;
		float elapsedTime = 0;
		float startRotation = transform.rotation.z;
		startRotation = Mathf.Round(startRotation);
		startRotation *= 180;

		while(elapsedTime < animationDuration) {
			elapsedTime += Time.deltaTime;

			transform.localRotation = Quaternion.Euler(new Vector3(0, 0, startRotation + spinCurve.Evaluate(elapsedTime/animationDuration) * -180));
			yield return new WaitForEndOfFrame();
		}

		GetComponent<Button>().interactable = true;
	}
}
