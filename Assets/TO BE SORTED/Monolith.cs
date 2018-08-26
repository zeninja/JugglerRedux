using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Monolith : MonoBehaviour
{
    LineRenderer line;

    public int lineResolution = 13;

    public float duration = .15f;

    public List<Vector3> rectanglePositions;
    public List<Vector3> ringPositions;
    public List<Vector3> easedPositions;
    public Vector2 ringAnchorPos;

    public float cornerOffset;
    public float radius = .5f;

    float t = 0;
    bool  ready;

    [System.NonSerialized]
    public bool  useOffset;
    public Vector2 myCenter;

    BoxCollider2D myCollider;

    public Color startColor;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        myCollider = GetComponent<BoxCollider2D>();

        if (lineResolution % 2 != 0)
        {
            lineResolution--;
        }

        GetComponent<MeshRenderer>().material.color = startColor;
        // FindRectanglePositions();
    }

    void Update()
    {
        FindRectanglePositions();
        FindRingPositions();
       
        ready = true;
    }

    void FixedUpdate()
    {
        EaseValues();
        UpdateMesh();
    }

    void EaseValues()
    {
        if (!ready) { return; }


        if (fingerActive || Input.GetMouseButton(0))
        {
            t += Time.fixedDeltaTime;
        }
        else
        {
            t -= Time.fixedDeltaTime;
        }
        fingerActive = false;

        t = Mathf.Clamp(t, 0, duration);
        float percent = t/duration;
        percent = Mathf.Clamp(percent, 0, 1);

        List<Vector3> diffs = new List<Vector3>();

        for (int i = 0; i < lineResolution; i++)
        {
            diffs.Add(ringPositions[i] - rectanglePositions[i]);
        }

        easedPositions = new List<Vector3>();

        for (int i = 0; i < lineResolution; i++)
        {
            easedPositions.Add(rectanglePositions[i] + diffs[i] * percent);
        }

        line.positionCount = easedPositions.Count;
        line.SetPositions(easedPositions.ToArray());
    }

    void UpdateMesh() {
        if(!ready) { return; }

        if(easedPositions != null) {
            if(fingerActive || Input.GetMouseButton(0)) {
                Debug.Log("Updating mesh live");
                GetComponent<MonolithMesh>().UpdateValues(ringAnchorPos, easedPositions);
            } else {
                Debug.Log("Updating mesh 0");
                GetComponent<MonolithMesh>().UpdateValues(myCenter, easedPositions);
            }
        }
    }

    bool fingerActive;

    public void GetFingerHover(Vector2 a_position)
    {
        ringAnchorPos = a_position;
        fingerActive = true;
    }

    // public Vector2 m_TopLeft;
    // public Vector2 m_TopRight;
    // public Vector2 m_BotLeft;
    // public Vector2 m_BotRight;  

    void FindRectanglePositions()
    {
        Vector2 offset = Vector2.zero;

        if (useOffset)
        {
            offset = new Vector2(ScreenInfo.w / 2, 0);
        }
        
        SetColliderPosition(offset);

        Vector2 m_TopLeft  = new Vector2( 0                , -ScreenInfo.h / 2);
        Vector2 m_TopRight = new Vector2( -ScreenInfo.w / 2, -ScreenInfo.h / 2);
        Vector2 m_BotLeft  = new Vector2( 0                ,  ScreenInfo.h / 2);
        Vector2 m_BotRight = new Vector2( -ScreenInfo.w / 2,  ScreenInfo.h / 2);

        m_TopLeft  += offset;
        m_TopRight += offset;
        m_BotLeft  += offset;
        m_BotRight += offset;

        m_TopLeft  += new Vector2( cornerOffset, -cornerOffset);
        m_TopRight += new Vector2(-cornerOffset, -cornerOffset);
        m_BotLeft  += new Vector2( cornerOffset,  cornerOffset);
        m_BotRight += new Vector2(-cornerOffset,  cornerOffset);

        rectanglePositions = new List<Vector3>();

        float perimeter = Vector2.Distance(m_TopLeft, m_TopRight) + Vector2.Distance(m_TopRight, m_BotRight);
        perimeter *= 2;

        // // w = (p - 2h) / 2
        float width = (perimeter - 2 * Vector2.Distance(m_TopLeft, m_BotRight)) / 2;
        float height = (perimeter - 2 * Vector2.Distance(m_TopLeft, m_TopRight)) / 2;

        int half = lineResolution / 2;
        int pointsPerLongSide = Mathf.FloorToInt(half * height / (width + height));
        int pointsPerShortSide = half - pointsPerLongSide;

        Vector2 lastPoint = Vector2.zero;

        // Top Left to Top Right
        for (int i = 0; i <= pointsPerShortSide; i++)
        {
            Vector2 rectPoint = m_TopLeft + (m_TopRight - m_TopLeft) * i / pointsPerShortSide;
            rectanglePositions.Add(rectPoint);
        }

        // Start the next 3 loops at 1 because they continue from the last loop, the corner points are shared
        for (int i = 1; i <= pointsPerLongSide; i++)
        {
            Vector2 rectPoint = m_TopRight + (m_BotRight - m_TopRight) * i / pointsPerLongSide;
            rectanglePositions.Add(rectPoint);
        }

        for (int i = 1; i <= pointsPerShortSide; i++)
        {
            Vector2 rectPoint = m_BotRight + (m_BotLeft - m_BotRight) * i / pointsPerShortSide;
            rectanglePositions.Add(rectPoint);
        }

        for (int i = 1; i < pointsPerLongSide - 1; i++)
        {
            Vector2 rectPoint = m_BotLeft + (m_TopLeft - m_BotLeft) * i / pointsPerLongSide;
            rectanglePositions.Add(rectPoint);
        }

        Vector2 capPoint = m_TopLeft;
        rectanglePositions.Add(capPoint);
    }

    void FindRingPositions()
    {
        ringPositions = new List<Vector3>();

        ringAnchorPos = Extensions.MouseScreenToWorld();

        float x;
        float y;
        float z = 0;

        float angle = 180;

        for (int i = 0; i < lineResolution; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Vector3 ringPoint = (Vector3)ringAnchorPos + new Vector3(x, y, z);
            ringPositions.Add(ringPoint);

            angle += (360f / lineResolution);
        }
    }

    void SetColliderPosition(Vector2 offset)
    {
        Vector2 center = new Vector2(-ScreenInfo.w / 4, 0);
        Vector2 size = new Vector2(ScreenInfo.w / 2, ScreenInfo.h);

        myCollider.offset = offset + center;
        myCollider.size = size;
        myCenter = center + offset;
    }

    public bool TransitionComplete() {
        return t >= duration;
    }

    public GameObject debug;
}