using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RectangleLine : MonoBehaviour
{
    LineRenderer line;

    public int lineResolution = 13;
    public float lineWidth;

    public float easeRate = 1f;
    float t = 0;

    bool ready;

    public List<Vector3> rectanglePositions;
    public List<Vector3> ringPositions;

    public GameObject debug;

    GameObject s;
	Vector2 ringAnchorPos;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        FindRingPositions();
        FindRectanglePositions();

		// originalRingPositions = ringPositions;

        ready = true;
    }

    void Update()
    {
        FindRingPositions();
        FindRectanglePositions();

        if (lineResolution % 2 != 0)
        {
            lineResolution--;
        }
    }

    void FixedUpdate()
    {
        EaseValues();
    }


    void EaseValues()
    {
        if (!ready) { return; }


        if (Input.GetMouseButton(0))
        {
            t += Time.fixedDeltaTime * easeRate;
        }
        else
        {
            t -= Time.fixedDeltaTime * easeRate;
        }

        t = Mathf.Clamp(t, 0, 1);

        List<Vector3> diffs = new List<Vector3>();

        for (int i = 0; i < lineResolution; i++)
        {
            diffs.Add(ringPositions[i] - rectanglePositions[i]);
        }

        easedPositions = new List<Vector3>();

        for (int i = 0; i < lineResolution; i++)
        {
            easedPositions.Add(rectanglePositions[i] + diffs[i] * t);
        }

		line.positionCount = easedPositions.Count;
        line.SetPositions(easedPositions.ToArray());
    }

    public List<Vector3> easedPositions;

    // public Vector2 topLeftPos;
    // public Vector2 topRightPos;
    // public Vector2 bottomRightPos;
    // public Vector2 bottomLeftPos;

    // public float width, height;

	// public int startIndex = 0;
	// List<Vector3> originalRingPositions;
	public float rectBuffer;

	public float radius = .5f;

	public bool useOffset;

    void FindRectanglePositions()
    {
		Vector2 offset = Vector2.zero;


		if(useOffset) {
			offset = new Vector2(Screen.width / 2, 0);
			// Debug.Log(offset);
			// offset = Extensions.ScreenToWorld(offset);
			// Debug.Log(offset);
		}

        // // Points for the corners of the rectangle in terms of the screen
        Vector2 screenTL = new Vector2(0, 0);
        Vector2 screenTR = new Vector2(Screen.width / 2, 0);
        Vector2 screenBR = new Vector2(Screen.width / 2, Screen.height);
        Vector2 screenBL = new Vector2(0, Screen.height);

		screenTL += offset;
		screenTR += offset;
		screenBR += offset;
		screenBL += offset;

        Vector2 topLeftPos     = Extensions.ScreenToWorld(screenTL);
        Vector2 topRightPos    = Extensions.ScreenToWorld(screenTR);
        Vector2 bottomRightPos = Extensions.ScreenToWorld(screenBR);
        Vector2 bottomLeftPos  = Extensions.ScreenToWorld(screenBL);
		
		


		topLeftPos     += new Vector2( rectBuffer,  rectBuffer);
		topRightPos    += new Vector2(-rectBuffer,  rectBuffer);
		bottomRightPos += new Vector2(-rectBuffer, -rectBuffer);
		bottomLeftPos  += new Vector2( rectBuffer, -rectBuffer);

        rectanglePositions = new List<Vector3>();

        float perimeter = Vector2.Distance(topLeftPos, topRightPos) + Vector2.Distance(topRightPos, bottomRightPos);
        perimeter *= 2;

        // // w = (p - 2h) / 2
        float width = (perimeter - 2 * Vector2.Distance(topLeftPos, bottomLeftPos)) / 2;
        float height = (perimeter - 2 * Vector2.Distance(topLeftPos, topRightPos)) / 2;

        int half = lineResolution / 2;
        int pointsPerLongSide = Mathf.FloorToInt(half * height / (width + height));
        int pointsPerShortSide = half - pointsPerLongSide;

        Vector2 lastPoint = Vector2.zero;

        // Top Left to Top Right
        for (int i = 0; i <= pointsPerShortSide; i++)
        {
            Vector2 rectPoint = topLeftPos + (topRightPos - topLeftPos) * i / pointsPerShortSide;
            rectanglePositions.Add(rectPoint);
        }

        // Start the next 3 loops at 1 because they continue from the last loop, the corner points are shared
        for (int i = 1; i <= pointsPerLongSide; i++)
        {
            Vector2 rectPoint = topRightPos + (bottomRightPos - topRightPos) * i / pointsPerLongSide;
            rectanglePositions.Add(rectPoint);
        }

        for (int i = 1; i <= pointsPerShortSide; i++)
        {
            Vector2 rectPoint = bottomRightPos + (bottomLeftPos - bottomRightPos) * i / pointsPerShortSide;
            rectanglePositions.Add(rectPoint);
        }

        for (int i = 1; i < pointsPerLongSide - 1; i++)
        {
            Vector2 rectPoint = bottomLeftPos + (topLeftPos - bottomLeftPos) * i / pointsPerLongSide;
            rectanglePositions.Add(rectPoint);
        }

		Vector2 capPoint = topLeftPos;
		rectanglePositions.Add(capPoint);
    }

    void FindRingPositions()
    {
        ringPositions = new List<Vector3>();

		ringAnchorPos = Extensions.MouseScreenToWorld();

        float x;
        float y;
        float z = 0;

        float angle = 225f;

        for (int i = 0; i < lineResolution; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Vector3 ringPoint = (Vector3)ringAnchorPos + new Vector3(x, y, z);
            ringPositions.Add(ringPoint);

            angle -= (360f / lineResolution);
        }
    }

    void DrawRectangle()
    {
        line.positionCount = rectanglePositions.Count;
        line.SetPositions(rectanglePositions.ToArray());
    }

    void DrawRing()
    {
        line.positionCount = ringPositions.Count;
        line.SetPositions(ringPositions.ToArray());
    }
}