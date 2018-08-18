using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    List<Vector3> m_LinePointList;
    List<Vector3> m_LineSegment;
    int m_LineLength = 5;

    NewBall m_Ball;
    Rigidbody2D m_Rigidbody;

    [System.NonSerialized]
    public Color myColor;

    [System.NonSerialized]
    public int spriteSortIndex;

    public SpriteRenderer ball;
    public LineRenderer line;


    // Use this for initialization
    void Start()
    {
        m_Ball = GetComponentInParent<NewBall>();
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();

        m_LinePointList = new List<Vector3>();
        m_LineSegment = new List<Vector3>();

        line.sortingLayerName = "Default";
        line.useWorldSpace = true;
        line.startWidth = transform.root.localScale.x;
        line.endWidth = transform.root.localScale.y;
    }


    // Update is called once per frame
    void Update()
    {
        DrawTrail();
    }

    #region util
    public void SetInfo(int newIndex)
    {
        spriteSortIndex = newIndex;

        SetColor();
        SetDepth();
    }

    public void SetColor()
    {
        spriteSortIndex = Mathf.Clamp(spriteSortIndex, 0, 8);
        myColor = NewBallManager.GetInstance().m_BallColors[spriteSortIndex];
        ball.color = myColor;
        line.material.color = myColor;
        line.material.color = myColor;
        
        GetComponent<SpriteCircleEffectSpawner>().effectColor = myColor;
    }

    public void SetColor(Color newColor)
    {
        // overload method for setting the color directly
        myColor = newColor;
        ball.color = myColor;
        line.material.color = myColor;
    }

    public void SetDepth()
    {
        int numLayersPerBall = 3;

        ball.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        line.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
    }

    public void SetDepth(int newIndex)
    {
        spriteSortIndex = newIndex;
        int numLayersPerBall = 3;

        ball.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        line.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
    }
    #endregion

    void DrawTrail()
    {
        if (VelocityPositive())
        {
            if (m_Ball.m_BallThrown || m_Ball.m_Launching)
            {
                line.enabled = true;

                // 1. Add the most recent position;
                m_LinePointList.Add(transform.position);

                // 2. Set the line length;
                // m_LineLength = Mathf.CeilToInt()

                // 3. Trim the line
                if (m_LinePointList.Count > m_LineLength)
                {
                    m_LinePointList.RemoveAt(0);
                }

                // 4. Set the line
                line.positionCount = m_LinePointList.Count;
                line.SetPositions(m_LinePointList.ToArray());
            }
        }
        else
        {
            m_LinePointList.Clear();
            line.positionCount = 1;
            line.enabled = false;
        }
    }

    // GAME OVER!!
    public void HandleDeath()
    {
        transform.localScale = Vector3.one;
        myColor = NewBallManager.GetInstance().deadBallColor;
        SetColor(myColor);
        // GetComponentInChildren<GameOverEffect>().HandleDeath();
    }

    public bool VelocityPositive()
    {
        // Debug.Log(m_Rigidbody.velocity.y);
        return m_Rigidbody.velocity.y > 0;
    }
}
