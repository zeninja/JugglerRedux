using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BouncingCircleEffect : MonoBehaviour
{
    LineRenderer line;
    const int NUM_SEGMENTS = 100;

    float radius;
	float lineWidth;
	float t;
	public float maxRadius;
	public float animationDuration;
	public AnimationCurve circleRadius;

    void Awake()
    {
        // startTime = Time.time;
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
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
        CreatePoints();
    }

    void AnimateCircle()
    {
        if (NewGameManager.GameOver()) { return; }
		lineWidth = radius / 10;
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;

        float angle = 20f;

		t += Time.deltaTime;
		Debug.Log(t);

		if (t < animationDuration) {
			radius = maxRadius * circleRadius.Evaluate(t / animationDuration);
		} else {
			t = 0;
		}


        for (int i = 0; i < NUM_SEGMENTS; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.positionCount = NUM_SEGMENTS;
            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / NUM_SEGMENTS);
        }

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = true;
    }
}