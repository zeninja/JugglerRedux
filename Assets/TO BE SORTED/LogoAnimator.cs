using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimator : MonoBehaviour {

	#region instance
	private static LogoAnimator instance;
	public static LogoAnimator GetInstance() {
		return instance;
	}
	#endregion

	public StackerDot initialBg;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	void Start() {
		anim = GetComponent<Animation>();
	}

	public Transform easy, juggling, mask, container;
	
	Animation anim;
	public AnimationClip logoIn;
	public AnimationClip logoOut;

	// Update is called once per frame
	void Update () {
		// if(Input.GetKeyDown(KeyCode.L)) {
		// 	ShowLogo();
		// }
	}

	public void PlayLogoIn() {
		StartCoroutine(ShowLogo());
	}

	public IEnumerator ShowLogo() {
		GetComponent<Animation>().Play("LogoIn");
		yield return new WaitForSeconds(logoIn.length);
	}

	public IEnumerator HideLogo() {
		GetComponent<Animation>().Play("LogoOut");
		yield return new WaitForSeconds(logoOut.length);
	}

	// Late Update so that the slider updates its position properly since it is not a UI element
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
}
