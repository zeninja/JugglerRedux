using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CatchRing : MonoBehaviour {

	LineRenderer line;
	int NUM_SEGMENTS = 1000;
	public Extensions.Property lineWidth;
	public Extensions.Property radius;

	float currentRadius;
	float currentLineWidth;
	public float duration;

	public Color defaultColor;


	// Use this for initialization
	void Awake () {
		line = GetComponent<LineRenderer>();
		line.useWorldSpace = false;
		line.sortingLayerName = "Default";
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(NewGameManager.GameOver()) { return; }

		HandleInput();
	}

	void HandleInput() {
		if(Input.GetKeyDown(KeyCode.C)) {
			TriggerRing(defaultColor);
		}
	}

	public void TriggerRing(Color ballColor) {
		line.material.color = ballColor;
		line.enabled = true;
		StartCoroutine(AnimateRing(duration));
	}

	IEnumerator AnimateRing(float d) {
		float t = 0;

		while(t < d) {
			float p = t / d;

			UpdateLineWidthAndRadius(p);
			DrawRing();

			t += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
		
		line.enabled = false;
		Destroy(gameObject);
	}

	void UpdateLineWidthAndRadius(float percent) {
		currentRadius    = Extensions.GetSmoothStepRange(radius, percent);
		currentLineWidth = Extensions.GetSmoothStepRange(lineWidth, percent);
	}

	void DrawRing()
    {
        float x;
        float y;
        float z = 10f;

        float angle = 20f;

        for (int i = 0; i < (NUM_SEGMENTS + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * currentRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * currentRadius;

			Vector3 linePos = new Vector3(x, y, z);

            line.positionCount = (NUM_SEGMENTS + 1);
            line.SetPosition(i, linePos);

            angle += (360f / NUM_SEGMENTS);
        }

        line.startWidth = currentLineWidth;
        line.endWidth   = currentLineWidth;
    }
}
