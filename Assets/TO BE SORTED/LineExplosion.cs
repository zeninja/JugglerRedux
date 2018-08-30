using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineExplosion : MonoBehaviour
{

    public LineEffect lineEffect;
    public float startWidth;
	public float endWidth;

    public int numLines = 10;

    public float innerRadius;
    public float outerRadius;

    public Color lineColor;

	public float duration = .25f;
	public float hangDuration = .25f;

	public float rotationSpeed = .25f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Space)) {
			SpawnLines(Vector2.zero);
		}

		transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.Self);

        outerRadius = Mathf.Clamp(outerRadius, innerRadius, outerRadius);
    }

    public void SpawnLines(Vector2 anchorPoint)
    {
		// transform.position = anchorPoint;

		Debug.Log("spawning lines");
        float angle = (float)360 / (float)numLines;

        for (int i = 0; i < numLines; i++)
        {
            LineEffect l = Instantiate(lineEffect);

            float x;
            float y;
            
            x = Mathf.Sin(Mathf.Deg2Rad * (angle * i));
            y = Mathf.Cos(Mathf.Deg2Rad * (angle * i));

			Vector2 startPos = new Vector2(x * innerRadius, y * innerRadius);
			Vector2 endPos   = new Vector2(x * outerRadius, y * outerRadius);

			startPos += anchorPoint;
			endPos   += anchorPoint;
			
			// l.lineExplosion = this;
			l.SetColor(lineColor);
            l.PlayLine(startPos, endPos, duration, startWidth, endWidth, hangDuration);
			// l.transform.parent = transform;
        }
    }
}
