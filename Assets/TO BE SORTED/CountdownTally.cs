using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTally : MonoBehaviour {

	public float popInDuration = .15f;
	public float popOutDuration = .15f;
	[System.NonSerialized]
	public bool nextTally = false;
	float startScale;
	Transform mask;
	

	// Use this for initialization
	void Start () {
		startScale = transform.localScale.x;
		mask = transform.GetChild(0);

		StartCoroutine(PopIn());
	}

	IEnumerator PopIn() {
		float t = 0;
        float d = popInDuration;

		transform.localScale = Vector2.zero;


        while (t < d)
        {
            t += Time.fixedDeltaTime;
            transform.localScale = Vector2.one * startScale * EZEasings.SmoothStart3(t / d);
            yield return new WaitForFixedUpdate();
        }
	}

	public void StartPopOutProcess() {
		StartCoroutine(PopOut());
	}

	IEnumerator PopOut() {
		float t = 0;
		float d = popOutDuration;

		while (t < d) {
			t += Time.fixedDeltaTime;
			mask.localScale = Vector2.one * EZEasings.SmoothStart3(t / d);
			yield return new WaitForFixedUpdate();
		}
		Destroy(gameObject);
	}

	public void SetColor(int colorIndex) {
		GetComponent<Image>().color = NewBallManager.GetInstance().m_BallColors[colorIndex];
	}
}
