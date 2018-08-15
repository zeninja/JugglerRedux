using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FadingCircle : MonoBehaviour
{
    const int NUM_SEGMENTS = 100;

    float radius;
    public float maxRadius = 2f;
    public float lineWidth = .1f;
    LineRenderer line;

    float startTime;

    void Awake()
    {
        startTime = Time.time;
        radius = 0;
        line = GetComponent<LineRenderer>();
        line.positionCount = (NUM_SEGMENTS + 1);
        line.useWorldSpace = false;
        line.enabled = false;
    }

    void Start()
    {
        CreatePoints();
    }

    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(AnimateCircle());
		}

        CreatePoints();
    }

	public float animationDuration = .4f;
	public AnimationCurve alphaOverTime;

    IEnumerator AnimateCircle()
    {
		float t = 0;

		while (t < animationDuration) {
			t += Time.deltaTime;
			
			float percent = t / animationDuration;
			
			radius = maxRadius * EZEasings.SmoothStart3(percent);

			float alpha = alphaOverTime.Evaluate(percent);
			Color lineColor = GetComponent<LineRenderer>().material.color;
			lineColor.a = alpha;
			line.material.color = lineColor;
			line.startColor = lineColor;
			line.endColor = lineColor;

			yield return new WaitForEndOfFrame();
		}
    }

	public void TriggerCircle(float targetMax, float targetLineWidth, float duration = .4f) {
        maxRadius = targetMax;
        lineWidth = targetLineWidth;
        animationDuration = duration;

        StartCoroutine(AnimateCircle());
	}

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;
        float angle = 20f;

        for (int i = 0; i < (NUM_SEGMENTS + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.positionCount = (NUM_SEGMENTS + 1);
            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / NUM_SEGMENTS);
        }

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = true;
    }
}