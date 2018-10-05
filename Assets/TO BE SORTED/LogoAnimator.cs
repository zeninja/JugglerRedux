using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimator : MonoBehaviour {
 
	public Transform easy, juggling, mask, container;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)) {
			ShowLogo();
		}
	}

	public void ShowLogo() {
		GetComponent<Animation>().Play("LogoMask");
	}

	void LateUpdate() {

		// Rather than using 3 separate variables in the animation curve, using one variable ensures consistency across axes
		// I'm sure there's a better way to do this but 
		float easyX = easy.transform.localScale.x;
		easy.transform.localScale = new Vector3(easyX, easyX, 0);

		float jugglingX = juggling.transform.localScale.x;
		juggling.transform.localScale = new Vector3(jugglingX, jugglingX, 0);
		
		float maskX = mask.transform.localScale.x;
		mask.transform.localScale = new Vector3(maskX, maskX, 0);

		float containerX = container.transform.localScale.x;
		container.transform.localScale = new Vector3(containerX, containerX, 0);
	}

	// public float animationDuration;
	// public Extensions.Property yHeight;
	// public float targetScale;
	// public float logoHangDuration;

	// IEnumerator AnimateLogo() {
	// 	yield return StartCoroutine(LogoIn());
	// 	yield return StartCoroutine(Extensions.Wait(logoHangDuration));
	// 	yield return StartCoroutine(LogoOut());
	// }

	// IEnumerator LogoIn() {
	// 	float t = 0;
	// 	float d = animationDuration;

	// 	while (t < d) {
	// 		float p = t / d;

	// 		// Debug.Log(EZEasings.Arch2(p));


	// 		easy.localScale 	= Vector2.one * targetScale * EZEasings.SmoothStart4(p);
	// 		juggling.localScale = Vector2.one * targetScale * EZEasings.SmoothStart4(p);

	// 		t += Time.fixedDeltaTime;
	// 		yield return new WaitForFixedUpdate();
	// 	}

	// }

	// IEnumerator LogoOut() {
	// 	float t = 0;
	// 	float d = animationDuration;

	// 	while (t < d) {
	// 		float p = t / d;

	// 		easy.transform.localPosition = 

	// 		easy.localScale 	= Vector2.one * targetScale * EZEasings.SmoothStart4(1) -
	// 						  	  Vector2.one * targetScale * EZEasings.SmoothStart4(p);
	// 		juggling.localScale = Vector2.one * targetScale * EZEasings.SmoothStart4(1) -
	// 						  	  Vector2.one * targetScale * EZEasings.SmoothStart4(p);

	// 		t += Time.fixedDeltaTime;
	// 		yield return new WaitForFixedUpdate();
	// 	}

	// }
}
