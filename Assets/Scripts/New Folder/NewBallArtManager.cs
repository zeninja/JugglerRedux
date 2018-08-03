using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    LineRenderer m_TrailLineRenderer;
    public GameObject m_Dot;
    List<Vector3> m_LinePointList;
    int m_LineLength = 5;

    GameObject m_EndDot;
    NewBall m_Ball;
    Rigidbody2D m_Rigidbody;

    Color myColor;

    [System.NonSerialized]
    public int spriteSortIndex;

    // Use this for initialization
    void Start()
    {
        m_TrailLineRenderer = GetComponent<LineRenderer>();
        m_TrailLineRenderer.sortingLayerName = "Default";
        m_TrailLineRenderer.startWidth = transform.localScale.x;
        m_TrailLineRenderer.endWidth   = transform.localScale.y;

        m_LinePointList = new List<Vector3>();

        m_Ball = GetComponent<NewBall>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

        m_EndDot = Instantiate(m_Dot) as GameObject;
        m_EndDot.transform.position = transform.position;
        m_EndDot.transform.localScale = Vector3.one * transform.localScale.x;
        m_EndDot.SetActive(false);

        SetColor();
        SetDepth(spriteSortIndex);
    }

    // Update is called once per frame
    void Update()
    {
        DrawTrail();
    }

    public void SetDepth(int newIndex) {
        int numLayersPerBall = 3;
        spriteSortIndex = newIndex;

        SpriteRenderer ball = GetComponent<SpriteRenderer>();
        LineRenderer line = m_TrailLineRenderer;
        SpriteRenderer endDot = m_EndDot.GetComponent<SpriteRenderer>();

        ball.sortingOrder   = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        line.sortingOrder   = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
        endDot.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 1); // Furthest back
    }

    void StartTrail()
    {
        m_EndDot = Instantiate(m_Dot) as GameObject;
        m_EndDot.transform.position = transform.position;
    }

    void DrawTrail()
    {
        // Debug.Log(m_Rigidbody.velocity.y);

        if (VelocityPositive())
        {
            if (m_Ball.m_BallThrown || m_Ball.m_Launching)
            {
                m_TrailLineRenderer.enabled = true;

                m_LinePointList.Add(transform.position);

                if (m_LinePointList.Count > m_LineLength)
                {
                    m_LinePointList.RemoveAt(0);
                }

                m_EndDot.transform.position = m_LinePointList[0];
                m_EndDot.SetActive(true);
            }
        }
        else
        {
            m_TrailLineRenderer.enabled = false;

            if (m_LinePointList.Count > 0)
            {
                m_LinePointList.RemoveAt(m_LinePointList.Count - 1);
                // m_EndDot.transform.position = transform.position;
                m_EndDot.SetActive(false);
            }
        }

        m_TrailLineRenderer.positionCount = Mathf.Min(m_LinePointList.Count, m_LineLength);
        m_TrailLineRenderer.SetPositions(m_LinePointList.ToArray());
    }

    bool VelocityPositive()
    {
        return m_Rigidbody.velocity.y > 0;
    }

    bool EnableTrail()
    {
        return m_Ball.m_BallThrown;
    }

    public void SetColor() {
        myColor = m_Ball.GetComponent<SpriteRenderer>().color;
        m_TrailLineRenderer.material.color = myColor;
        m_EndDot.GetComponent<SpriteRenderer>().color = myColor;
    }

    void OnDestroy()
    {
        Destroy(m_EndDot);
    }
}
