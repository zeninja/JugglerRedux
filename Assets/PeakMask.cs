using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakMask : MonoBehaviour {

	LineRenderer line;
	public float peakDuration;
	public float peakScale = .8f;

    BallArt ballLine;


	void Start() {
		line = GetComponent<LineRenderer>();
		line.startWidth = 0;
		line.endWidth   = 0;

		ballLine = GetComponentInParent<BallArt>();
	}

	void LateUpdate()
    {
        SetMaskPositions(ballLine.GetTrailPositions());
    }

    public void SetMaskPositions(Vector3[] positions)
    {
		for(int i = 0; i < positions.Length; i++) {
			positions[i] += Vector3.back;
		}
        if (positions != null)
        {
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }

	void HandlePeak() {
		// Debug.Log("Handling peak");
		StartCoroutine(Peak());
	}

	public IEnumerator Peak() {
		// Debug.Log("Peaking");

		float t = 0;
		float d = peakDuration / 2;

		while(t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;

			line.startWidth = peakScale * EZEasings.SmoothStart3(p);
			line.endWidth   = peakScale * EZEasings.SmoothStart3(p);
	
			yield return new WaitForFixedUpdate();
		}

		t = 0;
		d = peakDuration / 2;

		while(t < d) {
			t += Time.fixedDeltaTime;
			float p = t / d;

			line.startWidth = peakScale - peakScale * EZEasings.SmoothStart3(p);
			line.endWidth   = peakScale - peakScale * EZEasings.SmoothStart3(p);

			yield return new WaitForFixedUpdate();
		}

	}
}
