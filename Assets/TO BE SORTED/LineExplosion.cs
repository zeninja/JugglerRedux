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

	public bool isLocal;

    // Use this for initialization
    void Start()
    {

    }

	Vector2 targetPos;


    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Space)) {
			SpawnLines(targetPos);
		}

		if(Input.GetMouseButtonDown(0)) {
			targetPos = Extensions.MouseScreenToWorld();
		}

		transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime), Space.World);

        outerRadius = Mathf.Clamp(outerRadius, innerRadius, outerRadius);
    }

    public void SpawnLines(Vector2 anchorPoint)
    {
		if(isLocal) {
			transform.position = anchorPoint;
		}

        float angle = (float)360 / (float)numLines;

        for (int i = 0; i < numLines; i++)
        {
            LineEffect l = Instantiate(lineEffect);
			l.transform.position = transform.position;
			l.transform.rotation = transform.rotation;

            float x;
            float y;
            
            x = Mathf.Sin(Mathf.Deg2Rad * (angle * i));
            y = Mathf.Cos(Mathf.Deg2Rad * (angle * i));

			Vector2 startPos = new Vector2(x * innerRadius, y * innerRadius);
			Vector2 endPos   = new Vector2(x * outerRadius, y * outerRadius);

			if(!isLocal) {
				startPos += anchorPoint;
				endPos   += anchorPoint;
			} else {
				l.transform.parent = transform;
			}

			// l.lineExplosion = this;
			l.SetColor(lineColor);
            l.PlayLine(startPos, endPos, duration, startWidth, endWidth, hangDuration);
        }
    }
}
