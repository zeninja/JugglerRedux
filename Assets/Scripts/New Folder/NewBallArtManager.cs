using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    LinePredictor m_LinePredictor;
    List<Vector3> m_LinePointList;
    List<Vector3> m_LineSegment;
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

    int lineIndex = 0;

    public AnimationCurve m_TrailCurve;

    // Use this for initialization
    void Start()
    {
        m_Ball = GetComponentInParent<NewBall>();
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();
        m_LinePredictor = GetComponentInParent<LinePredictor>();

        m_LinePointList = new List<Vector3>();

        line.sortingLayerName = "Default";
        line.startWidth = transform.root.localScale.x;
        line.endWidth   = transform.root.localScale.y;

        cap.transform.position = transform.position;
        cap.transform.localScale = Vector3.one * transform.localScale.x;

        // cap.enabled = false;
        // cap.GetComponent<BallCap>().bam = this;
        // cap.GetComponent<BallCap>().target = ball.transform;
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

                


                // m_LinePointList = m_LinePredictor.GetPointList();

                // for(int i = 0; i < m_LinePointList.Count; i++) {
                //     if(m_Ball.transform.position == m_LinePointList[i]) {
                //         lineIndex = i;
                //         break;
                //     }
                // }

                // 





                // // GET THE SUB-SEGMENT OF THE COMPLETE LINE AND ASSIGN IT TO M_LINESEGMENT
                // if(m_LinePointList.Count < m_LineLength) {
                //     m_LineSegment = GetLineSegment(0);
                // } else {
                //     lineIndex++;

                //     if(lineIndex + m_LineLength * 2 > m_LinePointList.Count) {
                //         lineIndex++;

                //         m_LineSegment = GetLineSegment(lineIndex);
                //     } else {
                //         m_LineSegment = GetLineSegment(lineIndex);
                //     }
                // }

                // // THEN. UHH... CHECK HOW FAR ALONG THE LINE WE ARE WHEN SETTING THE CAP'S POSITION
                // // AND SET IT ACCORDING TO HOW LONG THE LINE SHOULD BE
                
                // // AND THIS IS THE MOST IMPORTANT PART!!!! 
                // // WHEN THE BALL IS PEAKING, SET THE CAP POSITION SO THAT IT CATCHES UP *AS* THE BALL PEAKS!!!
                // SetCapPosition();

            }
        }
        else
        {
            line.enabled = false;

            // if (m_LinePointList.Count > 0)
            // {
            //     m_LinePointList.RemoveAt(m_LinePointList.Count - 1);
            //     // m_EndDot.transform.position = transform.position;
            //     cap.enabled = false;
            // }
        }

        // line.positionCount = Mathf.Min(m_LinePointList.Count, m_LineLength);
        // line.SetPositions(m_LinePointList.ToArray());
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
        cap.transform.position = m_LineSegment[0];
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
