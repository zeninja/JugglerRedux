using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    LinePredictor m_LinePredictor;
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
    // public SpriteRenderer cap;

    int lineIndex = 0;

    public AnimationCurve m_TrailCurve;

    // Use this for initialization
    void Start()
    {
        m_Ball = GetComponentInParent<NewBall>();
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();
        m_LinePredictor = GetComponentInParent<LinePredictor>();

        m_LinePointList = new List<Vector3>();
        m_LineSegment = new List<Vector3>();

        line.sortingLayerName = "Default";
        line.startWidth = transform.root.localScale.x;
        line.endWidth   = transform.root.localScale.y;
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
        // cap.color           = myColor;
    }

    public void SetColor(Color newColor) {
        // overload method for setting the color directly
        myColor             = newColor;
        ball.color          = myColor;
        line.material.color = myColor;
        // cap.color           = myColor;
    }

    public void SetDepth() {
        int numLayersPerBall = 3;
        
        ball.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        line.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
        // cap.sortingOrder  = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 1); // Furthest back
    }

    public void SetDepth(int newIndex) {
        spriteSortIndex = newIndex;
        int numLayersPerBall = 3;
        
        ball.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        line.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
        // cap.sortingOrder  = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 1); // Furthest back
    }

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
                if(m_LinePointList.Count > m_LineLength) {
                    m_LinePointList.RemoveAt(0);
                }

                // 4. Set the line
                line.positionCount = m_LinePointList.Count;
                line.SetPositions(m_LinePointList.ToArray());

                // 5. Set the cap
                // if(m_LinePointList.Count >= 1) {
                    // cap.transform.position = m_LinePointList[0];
                    // cap.enabled = true;
                // }

            }
        }
        else
        {
            // Debug.Log("False");
            // cap.enabled = false;
            m_LinePointList.Clear();
            line.positionCount = 1;        
            // line.SetPosition(0, transform.position);
            line.enabled = false;
            // Debug.Break();
        }
    }

    public void HandleDeath() {

    }

    public bool VelocityPositive()
    {
        return m_Rigidbody.velocity.y > 0;
    }

    List<Vector3> GetLineSegment(int startingIndex) {
        int lineLength = m_LineLength;

        List<Vector3> lineSeg = new List<Vector3>();
        
        for(int i = 0; i < lineLength; i++) {
            lineSeg.Add(m_LinePointList[i + startingIndex]);
        }

        return lineSeg;
    }

    void SetCapPosition() {
        // cap.transform.position = m_LineSegment[0];
    }

    bool EnableTrail()
    {
        return m_Ball.m_BallThrown;
    }

    void OnDestroy()
    {
        // Destroy(cap);
    }
}
