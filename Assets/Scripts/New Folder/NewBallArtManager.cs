using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    List<Vector3> m_LinePointList;
    List<Vector3> predictedLine;
    // List<Vector3> m_LineSegment;
    public int m_LineLength = 5;

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


    public SquashAndStretch squashStats;

    [System.Serializable]
    public class SquashAndStretch
    {
        public float throwMagnitude;	// doesnt need to be public
        public float maxThrowMagnitude;
        public float lineLength = 1f;
        public float squashAmount = .5f;

    }

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

    void GetPredictedLine() {
        predictedLine = GetComponentInParent<LinePredictor>().GetPointList();
    }

    public float risingSquash;

    int index = 0;
    public float t;

    public float smoothingSpeed = 100;

    void DrawTrail()
    {
        // Rising
        if (VelocityPositive())
        {
            if (m_Ball.m_BallThrown || m_Ball.m_Launching)
            {
                trail.enabled = true;

                // 1. Add the most recent position;
                m_LinePointList.Add(transform.position);

                // 2. Set the line length;
                int maxLineLength = predictedLine.Count / 2;
                index++;
                t = (float)index / (float)predictedLine.Count;
                t = Mathf.Clamp01(t);
                m_LineLength = (int)(EZEasings.Arch2(t) * maxLineLength);
                Debug.Log(m_LineLength);

                // 3. Trim the line
                if (m_LinePointList.Count > m_LineLength)
                {
                    m_LinePointList.RemoveAt(0);
                }

                // 4. Set the line
                trail.positionCount = m_LinePointList.Count;
                trail.SetPositions(m_LinePointList.ToArray());

                // trail.startWidth = defaultScale;
                // trail.startWidth = defaultScale;
                // float t = m_LinePointList.Count /  m_LineLength;
                trail.startWidth = defaultScale * (1 - risingSquash * t);
                trail.endWidth   = defaultScale * (1 - risingSquash * t);

                // SquishLine(percent);
            }
        }
        else
        {
            // if(m_Ball.IsHeld()) {
            //     // Held
            //     squashStats.throwMagnitude = m_Ball.currentThrowVector.magnitude;
            //     percent = squashStats.throwMagnitude / squashStats.maxThrowMagnitude;
            //     percent = Mathf.Clamp01(percent);
            //     SquishLine(percent);

            // } else {
            //     // Falling
            //     m_LinePointList.Clear();
            //     trail.startWidth = defaultScale;
            //     trail.endWidth   = defaultScale;
            //     trail.positionCount = 2;
            //     trail.SetPosition(0, transform.position);
            //     trail.SetPosition(1, transform.position);
            // }

            // line.enabled = false;


            m_LinePointList.Clear();
            trail.positionCount = 2;
            trail.SetPosition(0, transform.position);
            trail.SetPosition(1, transform.position);
            index = 0;
        }
    }

    // public float squashAmount;

    // void SquishLine(float t)
    // {

    //     Vector2 lineWidthRange = new Vector2(1, .5f);
    //     trail.startWidth = defaultScale * (1 - squashAmount * t);
    //     trail.endWidth = defaultScale * (1 - squashAmount * t);

    //     // float angle = Vector3.Angle(transform.position, (Vector3)transform.position + (Vector3)m_Ball.currentThrowVector);
    //     // Debug.Log(angle);
    //     // float leftAngle = angle - 90;
    //     // float rightAngle = angle + 90;

    //     Vector2 startPos, endPos;

    //     // startPos = new Vector2(Mathf.Sin(Mathf.Deg2Rad * leftAngle),  Mathf.Cos(Mathf.Deg2Rad * leftAngle));
    //     // endPos   = new Vector2(Mathf.Sin(Mathf.Deg2Rad * rightAngle), Mathf.Cos(Mathf.Deg2Rad * rightAngle));

    //     startPos = (Vector2)transform.position + new Vector2(-squashStats.lineLength * t, 0);
    //     endPos = (Vector2)transform.position + new Vector2(squashStats.lineLength * t, 0);

    //     trail.SetPosition(0, startPos);
    //     trail.SetPosition(1, endPos);
    // }

    // void UpdateLineWidth(float t)
    // {
    // }

    // public float squashAmount = .76f;

    // void AdjustLineWidth() {
    //     float t = (float)m_LinePointList.Count / (float)m_LineLength;
    //     trail.startWidth = NewBallManager.GetInstance().ballScale * (1 - squashAmount * t);
    // }

    public AnimationCurve popInAnimation;
    public float popInDuration;

    IEnumerator PopIn()
    {
        float t = 0;
        float d = popInDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            transform.localScale = Vector2.one * popInAnimation.Evaluate(t / d);
            yield return new WaitForFixedUpdate();
        }
    }

    public bool VelocityPositive()
    {
        // Debug.Log(m_Rigidbody.velocity.y);
        return m_Rigidbody.velocity.y > 0;
    }

    public void KillTrail()
    {
        trail.enabled = false;
    }
}
