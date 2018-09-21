﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    List<Vector3> m_LinePointList;
    List<Vector3> predictedPointList;
    List<Vector3> launchPointList = new List<Vector3>();
    int m_LineLength = 5;

    NewBall m_Ball;
    Rigidbody2D m_Rigidbody;

    GrabSquishLine grabSquishLine;

    public SpriteRenderer m_BallSprite;
    public LineRenderer trail;
    
    [System.NonSerialized] public Color myColor;
    [System.NonSerialized] public int spriteSortIndex;
    [System.NonSerialized] public BallSpriteMask ballMask;
    public BallPredictor m_BallPredictor;

    public float risingSquash;
    public float peakPercent = .95f;
    public float lineLengthPercent = .15f;
    public float currentWidth;
    public int launchLineLength = 40;


    int indexAlongLine = 0;
    float throwMagnitudePortion = -1;

    float defaultScale;
    bool initialized;

    // Use this for initialization
    void Start()
    {
        grabSquishLine  = GetComponent<GrabSquishLine>();
        m_Ball          = GetComponentInParent<NewBall>();
        m_Rigidbody     = GetComponentInParent<Rigidbody2D>();
        m_BallSprite    = GetComponentInChildren<SpriteRenderer>();
        ballMask        = GetComponentInChildren<BallSpriteMask>();
        m_BallPredictor = GetComponentInChildren<BallPredictor>();

        m_LinePointList    = new List<Vector3>();
        predictedPointList = new List<Vector3>();

        defaultScale = transform.root.localScale.x;

        // trail.sortingLayerName = "Default";
        // trail.useWorldSpace = true;
        // trail.startWidth = defaultScale;
        // trail.endWidth = defaultScale;

        EventManager.StartListening("CleanUp", HandleDeath);

        CheckLaunch();

        initialized = true;
    }

    void CheckLaunch() {
        if(m_Ball.IsLaunching()) {
            GetLaunchLine();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!initialized) { return; }

        // CheckBallState();
    }

    void ActivateSprite()
    {
        // m_BallSprite.enabled = (!m_Ball.IsHeld() && !VelocityPositive()) || BallPeaking();
    }

    #region util
    public void SetInfo(int newIndex)
    {
        spriteSortIndex = newIndex;

        SetColor();
        SetDepth();
    }

    public void PopInAnimation()
    {
        StartCoroutine(PopIn());
    }

    public void SetColor()
    {
        spriteSortIndex = Mathf.Clamp(spriteSortIndex, 0, 8);
        myColor = NewBallManager.GetInstance().m_BallColors[spriteSortIndex];
        m_BallSprite.color = myColor;
        // trail.material.color = myColor;

        GetComponent<SpriteCircleEffectSpawner>().effectColor = myColor;
    }

    public void SetColor(Color newColor)
    {
        // overload method for setting the color directly
        myColor = newColor;
        m_BallSprite.color = myColor;
        // trail.material.color = myColor;
    }

    public void SetDepth()
    {
        int numLayersPerBall = 3;

        m_BallSprite.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        // trail.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
    }

    public void SetDepth(int newIndex)
    {
        spriteSortIndex = newIndex;
        int numLayersPerBall = 3;

        m_BallSprite.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 3); // Up front
        // trail.sortingOrder = spriteSortIndex * numLayersPerBall - (numLayersPerBall - 2);
    }
    #endregion

    bool ballDead = false;

    public void HandleDeath()
    {
        ballDead = true;
    }

    void GetPredictedLine(Vector2 throwVector)
    {
        Vector2 anchorPos = transform.position;
        predictedPointList = m_BallPredictor.GetPositionList(anchorPos, throwVector);
    }

    void GetLaunchLine()
    {
        Vector2 launchPos    = transform.position;
        Vector2 launchVector = Vector2.up * NewBallManager.GetInstance().ballLaunchForce;

        // if(m_BallPredictor == null) {
        //     m_BallPredictor = GetComponentInChildren<BallPredictor>();
        // }
        // Debug.Log(launchPointList);

        launchPointList = m_BallPredictor.GetPositionList(launchPos, launchVector);
    }

    void DrawTrail()
    {
        if (DisableTrail())
        {
            // trail.enabled = false;
            return;
        }

        // Rising
        if (VelocityPositive())
        {
            
        }
        else
        {
            if (m_Ball.IsHeld())
            {
                // Held
                float throwMagnitude = m_Ball.currentThrowVector.magnitude;
                float maxThrowMagnitude = NewHandManager.GetInstance().maxThrowMagnitude;
                throwMagnitudePortion = throwMagnitude / maxThrowMagnitude;
                throwMagnitudePortion = Mathf.Clamp01(throwMagnitudePortion);

                // grabSquishLine.SquishLine(m_Ball.currentThrowVector, defaultScale, throwMagnitudePortion);
                // trail.enabled = false;

            }
            else
            {
                // Falling
                m_LinePointList.Clear();
                // trail.startWidth = defaultScale;
                // trail.endWidth = defaultScale;
                // trail.positionCount = 2;
                // trail.SetPosition(0, transform.position);
                // trail.SetPosition(1, transform.position);
                // trail.enabled = true;
                // grabSquishLine.Reset();
            }
            indexAlongLine = 0;
        }

        // if(trail.positionCount < 1) {
        //     Debug.Log(m_Rigidbody.velocity.y);
        // }
    }

    // void TrimLine()
    // {
    //     if (m_LinePointList.Count > m_LineLength)
    //     {
    //         int iter = 0;
    //         while (m_LinePointList.Count > m_LineLength && iter < 100)
    //         {
    //             m_LinePointList.RemoveAt(0);
    //             iter++;
    //         }
    //     }
    // }

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

        NewGameManager.GetInstance().PrepGame();
    }

    public bool VelocityPositive()
    {
        if (m_Rigidbody == null) { return false; }
        return m_Rigidbody.velocity.y > 0;
    }

    bool BallPeaking() {
        // return false;
        // return trail.positionCount < 1;
        return trail.positionCount < 1 && !m_Ball.IsHeld();
    }

    public bool trailOff = true;

    bool DisableTrail()
    {
        // Debug.Log(ballDead + " | " + !popInDone);
        if(trailOff) {
            return false;
        }
        return ballDead || !popInDone;
    }

    // public void PrepGameOver() {
    //     trail.sortingOrder =trail 
    // }

    void DrawRisingTrailOld() {
                    // if (m_Ball.m_BallThrown)
            // {
            //     // Debug.Log("Thrown");

            //     // 1. Add the most recent position;
            //     m_LinePointList.Add(transform.position);

            //     // 2. Set the line length;
            //     int maxLineLength = (int)(predictedPointList.Count * lineLengthPercent);
            //     indexAlongLine++;
            //     float t = (float)indexAlongLine / ((float)predictedPointList.Count * peakPercent);
            //     t = Mathf.Clamp01(t);

            //     float percent = EZEasings.Arch2(t);
            //     m_LineLength = (int)(percent * maxLineLength);

            //     // 3. Trim the line
            //     TrimLine();

            //     // 4. Set the line
            //     trail.positionCount = m_LinePointList.Count;
            //     trail.SetPositions(m_LinePointList.ToArray());

            //     // 5. Adjust trail width
            //     // float trailWidth = defaultScale * (1 - risingSquash * (1 - EZEasings.SmoothStart3(t))); // * throwMagnitudePortion);
            //     float squashAmount = risingSquash * (1 - EZEasings.SmoothStop2(t));
            //     // Debug.Log(squashAmount);
            //     currentWidth = defaultScale - squashAmount;
            //     float trailWidth = currentWidth;
            //     // Debug.Log(trailWidth);

            //     trail.startWidth = trailWidth;
            //     trail.endWidth   = trailWidth;

            //     Debug.Log(trail.positionCount);
            // }
            // else if (m_Ball.IsLaunching())
            // {
            //     // Debug.Log("Launching: " + launchPointList.Count);

            //     // 1. Add the most recent position;
            //     m_LinePointList.Add(transform.position);

            //     // 2. Set the line length;
            //     int maxLineLength = launchLineLength;

            //     indexAlongLine++;
            //     float t = (float)indexAlongLine / ((float)launchPointList.Count * peakPercent);
            //     t = Mathf.Clamp01(t);

            //     float percent = EZEasings.Arch2(t);
            //     m_LineLength = (int)(percent * maxLineLength);

            //     // float percent = 0;
            //     // m_LineLength = maxLineLength;

            //     // 3. Trim the line
            //     TrimLine();

            //     // 4. Set the line
            //     trail.positionCount = m_LinePointList.Count;
            //     trail.SetPositions(m_LinePointList.ToArray());

            //     // Adjust the width
            //     // trail.startWidth = defaultScale * (1 - risingSquash * percent);
            //     // trail.endWidth   = defaultScale * (1 - risingSquash * percent);
            // }

            // trail.enabled = true;
    }
}
