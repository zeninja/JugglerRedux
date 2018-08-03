using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    List<Vector3> m_LinePointList;
    int m_LineLength = 5;

    // GameObject m_EndDot;
    NewBall m_Ball;
    Rigidbody2D m_Rigidbody;

    Color myColor;

    [System.NonSerialized]
    public int spriteSortIndex;


    public SpriteRenderer ball;
    public LineRenderer line;
    public SpriteRenderer cap;


    // Use this for initialization
    void Start()
    {
        m_Ball = GetComponentInParent<NewBall>();
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();

        m_LinePointList = new List<Vector3>();

        line.sortingLayerName = "Default";
        line.startWidth = transform.root.localScale.x;
        line.endWidth   = transform.root.localScale.y;


        cap.transform.position = transform.position;
        cap.transform.localScale = Vector3.one * transform.localScale.x;
        cap.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        DrawTrail();
    }

    public void SetInfo(int newIndex) {
        spriteSortIndex = newIndex;

        SetColor();
        SetDepth();
    }

    public void SetColor() {
        myColor = NewBallManager.GetInstance().m_BallColors[spriteSortIndex];
        ball.color          = myColor;
        line.material.color = myColor;
        line.material.color = myColor;
        cap.color           = myColor;
    }

    public void SetDepth() {
        int numLayersPerBall = 3;
        
        ball.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        line.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
        cap.sortingOrder  = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 1); // Furthest back
    }

    public void SetDepth(int newIndex) {
        spriteSortIndex = newIndex;
        int numLayersPerBall = 3;
        
        ball.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        line.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
        cap.sortingOrder  = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 1); // Furthest back
    }

    void DrawTrail()
    {
        if (VelocityPositive())
        {
            if (m_Ball.m_BallThrown || m_Ball.m_Launching)
            {
                line.enabled = true;

                m_LinePointList.Add(transform.position);

                if (m_LinePointList.Count > m_LineLength)
                {
                    m_LinePointList.RemoveAt(0);
                }

                cap.transform.position = m_LinePointList[0];
                cap.enabled = true;
            }
        }
        else
        {
            line.enabled = false;

            if (m_LinePointList.Count > 0)
            {
                m_LinePointList.RemoveAt(m_LinePointList.Count - 1);
                // m_EndDot.transform.position = transform.position;
                cap.enabled = false;
            }
        }

        line.positionCount = Mathf.Min(m_LinePointList.Count, m_LineLength);
        line.SetPositions(m_LinePointList.ToArray());
    }

    bool VelocityPositive()
    {
        return m_Rigidbody.velocity.y > 0;
    }

    bool EnableTrail()
    {
        return m_Ball.m_BallThrown;
    }

    public void SetColor(Color myColor) {
        ball.color = myColor;
        line.material.color = myColor;
        cap.GetComponent<SpriteRenderer>().color = myColor;
    }

    void OnDestroy()
    {
        Destroy(cap);
    }
}
