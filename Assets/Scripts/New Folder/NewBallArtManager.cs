using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    List<Vector3> m_LinePointList;
    List<Vector3> predictedLine;
    // List<Vector3> m_LineSegment;
    int m_LineLength = 5;

    NewBall m_Ball;
    Rigidbody2D m_Rigidbody;

    [System.NonSerialized]
    public Color myColor;

    [System.NonSerialized]
    public int spriteSortIndex;

    // public SpriteRenderer ballSprite;
    public LineRenderer trail;

    float defaultScale;
    public SpriteRenderer gameOverBallSprite;

    GrabSquishLine grabSquishLine;

    // Use this for initialization
    void Start()
    {
        m_Ball = GetComponentInParent<NewBall>();
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();

        m_LinePointList = new List<Vector3>();
        predictedLine = new List<Vector3>();
        // m_LineSegment = new List<Vector3>();

        defaultScale = transform.root.localScale.x;

        trail.sortingLayerName = "Default";
        trail.useWorldSpace = true;
        trail.startWidth = defaultScale;
        trail.endWidth = defaultScale;

        grabSquishLine = GetComponent<GrabSquishLine>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        DrawTrail();
    }

    #region util
    public void SetInfo(int newIndex)
    {
        spriteSortIndex = newIndex;

        SetColor();
        SetDepth();

        StartCoroutine(PopIn());
    }

    public void SetColor()
    {
        spriteSortIndex = Mathf.Clamp(spriteSortIndex, 0, 8);
        myColor = NewBallManager.GetInstance().m_BallColors[spriteSortIndex];
        gameOverBallSprite.color = myColor;
        trail.material.color = myColor;
        trail.material.color = myColor;

        GetComponent<SpriteCircleEffectSpawner>().effectColor = myColor;
    }

    public void SetColor(Color newColor)
    {
        // overload method for setting the color directly
        myColor = newColor;
        // ballSprite.color = myColor;
        trail.material.color = myColor;
    }

    public void SetDepth()
    {
        int numLayersPerBall = 3;

        // ballSprite.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        trail.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
    }

    public void SetDepth(int newIndex)
    {
        spriteSortIndex = newIndex;
        int numLayersPerBall = 3;

        // ballSprite.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        trail.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
    }
    #endregion

    public void HandleCatch()
    {
        // ballSprite.enabled = false;
    }

    public void HandleThrow()
    {
        GetPredictedLine();

        // ballSprite.enabled = true;
    }

    bool ballDead = false;

    public void HandleDeath() {
        ballDead = true;
    }

    void GetPredictedLine() {
        predictedLine = GetComponentInParent<LinePredictor>().GetPointList();
    }

    public float risingSquash;

    int index = 0;
    public float t;

    public float lineSegmentPercent = .35f;
    public float peakPercent = .95f;

   float throwMagnitudePortion;


    void DrawTrail()
    {
        if(DisableTrail()) {
            trail.enabled = false;
            return;
        }

        // Rising
        if (VelocityPositive())
        {
            if (m_Ball.m_BallThrown || m_Ball.m_Launching)
            {
                trail.enabled = true;

                // 1. Add the most recent position;
                m_LinePointList.Add(transform.position);

                // 2. Set the line length;
                int maxLineLength = (int)(predictedLine.Count * lineSegmentPercent);
                index++;
                t = (float)index / ((float)predictedLine.Count * peakPercent);
                t = Mathf.Clamp01(t);

                float percent = EZEasings.Arch2(t);
                m_LineLength = (int)(percent * maxLineLength);

                // 3. Trim the line
                if (m_LinePointList.Count > m_LineLength)
                {
                    int iter = 0;
                    while(m_LinePointList.Count > m_LineLength && iter < 100) {
                        m_LinePointList.RemoveAt(0);
                        iter++;
                    }
                }

                // Debug.Log(t + " | " + percent + " | ");


                // 4. Set the line
                trail.positionCount = m_LinePointList.Count;
                trail.SetPositions(m_LinePointList.ToArray());

                // trail.startWidth = defaultScale;
                // trail.startWidth = defaultScale;
                // float t = m_LinePointList.Count /  m_LineLength;
                trail.startWidth = defaultScale * (1 - risingSquash * percent * throwMagnitudePortion);
                trail.endWidth   = defaultScale * (1 - risingSquash * percent * throwMagnitudePortion);

                // SquishLine(percent);
                trail.enabled = true;
            }
        }
        else
        {
            if(m_Ball.IsHeld()) {
                // Held
                float throwMagnitude = m_Ball.currentThrowVector.magnitude;
                float maxThrowMagnitude = NewHandManager.GetInstance().maxThrowMagnitude;
                throwMagnitudePortion = throwMagnitude / maxThrowMagnitude;
                throwMagnitudePortion = Mathf.Clamp01(throwMagnitudePortion);
                // SquishLine(throwMagnitudePortion);
                grabSquishLine.SquishLine(m_Ball.currentThrowVector, defaultScale, throwMagnitudePortion);
                trail.enabled = false;
    
            } else {
                // Falling
                m_LinePointList.Clear();
                trail.startWidth = defaultScale;
                trail.endWidth   = defaultScale;
                trail.positionCount = 2;
                trail.SetPosition(0, transform.position);
                trail.SetPosition(1, transform.position);
                trail.enabled = true;
            }

            index = 0;
        }
    }

    public AnimationCurve popInAnimation;
    public float popInDuration;

    bool popInDone = true;

    IEnumerator PopIn()
    {
        float t = 0;
        float d = popInDuration;
        popInDone = false;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            transform.localScale = Vector2.one * popInAnimation.Evaluate(t / d);
            yield return new WaitForFixedUpdate();
        }
        popInDone = true;
    }

    public bool VelocityPositive()
    {
        if(m_Rigidbody == null) { return false; }
        // Debug.Log(m_Rigidbody.velocity.y);
        return m_Rigidbody.velocity.y > 0;
    }

    bool DisableTrail() {
        // Debug.Log(ballDead + " | " + !popInDone);
        return ballDead || !popInDone;
    }
}
