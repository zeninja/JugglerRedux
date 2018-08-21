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

	float t;
	public float duration;
	float percent;

	public Color ringColor;

	// Use this for initialization
	void Start () {
		startTime = Time.time;

		line = GetComponent<LineRenderer>();
		line.useWorldSpace = false;
		line.sortingLayerName = "Default";
		// SetColor(ringColor);


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(NewGameManager.GameOver()) { return; }

		HandleInput();

		DrawRing();
	}

	float startTime;

	void HandleInput() {
		if(t < duration) {
			t += Time.fixedDeltaTime;
		}

		percent = t / duration;
		percent = Mathf.Clamp(percent, 0, 1);
	}	

	void DrawRing()
    {
		currentRadius = Extensions.GetSmoothStepRange(radius, percent);
		currentLineWidth = Extensions.GetSmoothStepRange(lineWidth, percent);

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
        line.endWidth = currentLineWidth;
        line.enabled = true;
    }

	public bool hasSetColor = false;

	public void SetColor(Color newColor) {
		if(!hasSetColor) {
			line.material.color = newColor;
			hasSetColor = true;
		}
	}
}
