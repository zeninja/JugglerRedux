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
	Vector2 anchorPos;

    void Awake()
    {

        s = Instantiate(debug) as GameObject;

        line = GetComponent<LineRenderer>();
        FindRingPositions();
        FindRectanglePositions();

		originalRingPositions = ringPositions;

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


        if(Time.time > nextMoveTime) {
        	s.transform.position = easedPositions[sIndex];
        	nextMoveTime = Time.time + delay;
        	sIndex = (sIndex + 1) % easedPositions.Count;
        }

        // DrawRectangle();
        // DrawRing();
    }

    float nextMoveTime = 0;
    public float delay = .25f;
    int sIndex;

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
			// easedPositions.Add(rectanglePositions[i]);
        }

		line.positionCount = easedPositions.Count;
        line.SetPositions(easedPositions.ToArray());
    }

    public List<Vector3> easedPositions;

    public Vector2 topLeftPos;
    public Vector2 topRightPos;
    public Vector2 bottomRightPos;
    public Vector2 bottomLeftPos;

    public float width, height;

	public int startIndex = 0;
	List<Vector3> originalRingPositions;
	public float rectBuffer;


    void FindRectanglePositions()
    {
        // // Points for the corners of the rectangle in terms of the screen
        Vector2 screenTL = new Vector2(0, 0);
        Vector2 screenTR = new Vector2(Screen.width, 0);
        Vector2 screenBR = new Vector2(Screen.width, Screen.height);
        Vector2 screenBL = new Vector2(0, Screen.height);

        Vector2 topLeftPos     = Extensions.ScreenToWorld(screenTL);
        Vector2 topRightPos    = Extensions.ScreenToWorld(screenTR);
        Vector2 bottomRightPos = Extensions.ScreenToWorld(screenBR);
        Vector2 bottomLeftPos  = Extensions.ScreenToWorld(screenBL);

		topLeftPos += new Vector2(-rectBuffer, -rectBuffer);
		topLeftPos += new Vector2(-rectBuffer, -rectBuffer);
		topLeftPos += new Vector2(-rectBuffer, -rectBuffer);
		topLeftPos += new Vector2(-rectBuffer, -rectBuffer);

        rectanglePositions = new List<Vector3>();

        float perimeter = Vector2.Distance(topLeftPos, topRightPos) + Vector2.Distance(topRightPos, bottomRightPos);
        perimeter *= 2;

        // // w = (p - 2h) / 2
        width = (perimeter - 2 * Vector2.Distance(topLeftPos, bottomLeftPos)) / 2;
        height = (perimeter - 2 * Vector2.Distance(topLeftPos, topRightPos)) / 2;

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

	public float radius = .5f;

    void FindRingPositions()
    {
        ringPositions = new List<Vector3>();

		anchorPos = Extensions.MouseScreenToWorld();

        float x;
        float y;
        float z = 0;

        float angle = 225f;

        for (int i = 0; i < lineResolution; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Vector3 ringPoint = (Vector3)anchorPos + new Vector3(x, y, z);
            ringPositions.Add(ringPoint);

            angle -= (360f / lineResolution);
        }
    }




























    /*
        #region
        void FindRectanglePositions()
        {
            // // Points for the corners of the rectangle in terms of the screen
            Vector2 screenTL = new Vector2(0, 0);
            Vector2 screenTR = new Vector2(Screen.width, 0);
            Vector2 screenBR = new Vector2(Screen.width, Screen.height);
            Vector2 screenBL = new Vector2(0, Screen.height);

            // Vector2 topLeftPos     = Extensions.ScreenToWorld(screenTL);
            // Vector2 topRightPos    = Extensions.ScreenToWorld(screenTR);
            // Vector2 bottomRightPos = Extensions.ScreenToWorld(screenBR);
            // Vector2 bottomLeftPos  = Extensions.ScreenToWorld(screenBL);

            rectanglePositions = new List<Vector3>();

            float perimeter = Vector2.Distance(topLeftPos, topRightPos) + Vector2.Distance(topRightPos, bottomRightPos);
            perimeter *= 2;

            // // w = (p - 2h) / 2
            width = (perimeter - 2 * Vector2.Distance(topLeftPos, bottomLeftPos)) / 2;
            height = (perimeter - 2 * Vector2.Distance(topLeftPos, topRightPos)) / 2;

            spaceBetweenPoints = perimeter / lineResolution;

            pointsPerShortSide = (width / spaceBetweenPoints);
            pointsPerLongSide = (height / spaceBetweenPoints);

            // // // w = (p - 2h) / 2
            // float width = (perimeter - 2 * Vector2.Distance(topLeftPos, bottomLeftPos)) / 2;
            // float height = (perimeter - 2 * Vector2.Distance(topLeftPos, topRightPos)) / 2;

            // float spaceBetweenPoints = perimeter / lineResolution;

            // float pointsPerShortSide = (width / spaceBetweenPoints);
            // float pointsPerLongSide = (height / spaceBetweenPoints);

            Vector2 lastPoint = Vector2.zero;

            // Top Left to Top Right
            for (int i = 0; i <= pointsPerShortSide; i++)
            {
                Vector2 rectPoint = topLeftPos + (topRightPos - topLeftPos) * i / pointsPerShortSide;
                rectanglePositions.Add(rectPoint);

                // GameObject d = Instantiate(debug);
                // d.transform.position = rectPoint;

                // yield return new WaitForSeconds(.2f);
            }

            for (int i = 1; i <= pointsPerLongSide; i++)
            {
                Vector2 rectPoint = topRightPos + (bottomRightPos - topRightPos) * i / pointsPerLongSide;
                rectanglePositions.Add(rectPoint);

                // GameObject d = Instantiate(debug);
                // d.transform.position = rectPoint;

                // yield return new WaitForSeconds(.2f);
            }

            for (int i = 1; i <= pointsPerShortSide; i++)
            {
                Vector2 rectPoint = bottomRightPos + (bottomLeftPos - bottomRightPos) * i / pointsPerShortSide;
                rectanglePositions.Add(rectPoint);

                //             GameObject d = Instantiate(debug);
                // d.transform.position = rectPoint;

                //             yield return new WaitForSeconds(.2f);

            }

            for (int i = 1; i < pointsPerLongSide; i++)
            {
                Vector2 rectPoint = bottomLeftPos + (topLeftPos - bottomLeftPos) * i / pointsPerLongSide;
                rectanglePositions.Add(rectPoint);

                //             GameObject d = Instantiate(debug);
                // d.transform.position = rectPoint;

                //             yield return new WaitForSeconds(.2f);



            }
        }

        void FindRingPositions()
        {
            ringPositions = new List<Vector3>();

            float radius = 1f;

            float x;
            float y;
            float z = 10f;

            float angle = 20f;

            for (int i = 0; i < lineResolution; i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                Vector3 ringPoint = new Vector3(x, y, z);
                ringPositions.Add(ringPoint);

                angle += (360f / lineResolution);
            }
        }
        #endregion
        */


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