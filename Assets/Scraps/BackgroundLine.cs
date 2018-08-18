using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLine : MonoBehaviour
{

    LineRenderer line;

	Color lineColor;

    float lineWidth;

    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.startColor = lineColor;
        line.endColor = lineColor;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
    }

	float t;
	int sampleIndex;

	float scrollSpeed = .05f;
	float a, b;

    int lineResolution = 100;

    // Update is called once per frame
    void FixedUpdate()
    {
		t += scrollSpeed;
        List<Vector3> linePositions = new List<Vector3>();

        float worldScreenWidth  = ResolutionCompensation.WorldUnitsInCamera.x;
		float worldScreenHeight = ResolutionCompensation.WorldUnitsInCamera.y;

		float xOffset = -worldScreenWidth  / 2;
		float yOffset = -worldScreenHeight / 2;

        for (int i = 0; i <= lineResolution; i++)
        {
			float percent = (float) i / (float) lineResolution;

            float xPos = percent * worldScreenWidth + xOffset;
            float yPos = b * Mathf.Sin(a * (percent + t * Time.fixedDeltaTime)) + yOffset;

            Vector3 linePos = new Vector3(xPos, yPos, 0);
            linePositions.Add(linePos);
        }

        line.positionCount = linePositions.Count;
        line.SetPositions(linePositions.ToArray()); 

		line.startWidth = lineWidth;
		line.endWidth   = lineWidth;                                                    
    }

	public void SetValues(float s, float newA, float newB, float newWidth, int newResolution = 100) {
		 scrollSpeed = s;
		 a = newA;
		 b = newB;
		 lineWidth = newWidth;
		 lineResolution = newResolution;
	}

	public void SetColor(Color newColor) {
		lineColor = newColor;
		line.startColor = lineColor;
        line.endColor = lineColor;
	}
}
