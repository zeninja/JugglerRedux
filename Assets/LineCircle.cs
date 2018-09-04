using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineCircle : MonoBehaviour {

	const int NUM_SEGMENTS = 100;
	LineRenderer line;

	public float circleRadius;
	float lineWidth;

    void Awake()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.positionCount = (NUM_SEGMENTS + 1);
        line.useWorldSpace = true;
        line.enabled = false;
    }

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		CreatePoints();	
	}

    void CreatePoints()
    {
        float x;
        float y;
        float z = -1f;

        float angle = 0f;

        for (int i = 0; i < (NUM_SEGMENTS + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * circleRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * circleRadius;

            line.positionCount = (NUM_SEGMENTS + 1);
            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / NUM_SEGMENTS);
        }

		lineWidth = circleRadius * 2;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = true;
    }
}
