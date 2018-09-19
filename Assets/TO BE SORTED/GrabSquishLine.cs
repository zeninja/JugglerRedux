﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSquishLine : MonoBehaviour
{

    public LineRenderer squishLine;
    NewBall ball;

    float defaultScale;


    // Use this for initialization
    void Start()
    {
        ball = GetComponentInParent<NewBall>();
        squishLine.material.color = GetComponent<NewBallArtManager>().myColor;
        defaultScale = NewBallManager.GetInstance().ballScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset();
        squishLine.enabled = ShowLine();
        
        SquishLine(ball.currentThrowVector, defaultScale, GetThrowMagnitudePortion());
    }

    public float lineLength = 1f;

    public float squashAmount;

    public void SquishLine(Vector2 throwVector, float startScale, float t)
    {
        if(throwVector == null) {
            Debug.Log("returning");
            squishLine.positionCount = 0;
            return;
        }

        squishLine.startWidth = startScale * (1 - squashAmount * t);
        squishLine.endWidth = startScale * (1 - squashAmount * t);

        float angle = -Vector2.SignedAngle(Vector2.up, (Vector3)throwVector);

        float leftAngle = angle - 90;
        if (leftAngle < 0)
        {
            leftAngle += 360;
        }

        float rightAngle = angle + 90;
        if (rightAngle > 360)
        {
            rightAngle = rightAngle % 360;
        }

        // Debug.Log(angle + " | " + leftAngle + " | " + rightAngle);

        Vector2 startPos, endPos;

        float x1 = Mathf.Sin(Mathf.Deg2Rad * leftAngle) * lineLength * t;
        float y1 = Mathf.Cos(Mathf.Deg2Rad * leftAngle) * lineLength * t;

        startPos = transform.position + new Vector3(x1, y1, 0);

        float x2 = Mathf.Sin(Mathf.Deg2Rad * rightAngle) * lineLength * t;
        float y2 = Mathf.Cos(Mathf.Deg2Rad * rightAngle) * lineLength * t;

        endPos = transform.position + new Vector3(x2, y2, 0);

        squishLine.positionCount = 2;
        squishLine.SetPosition(0, startPos);
        squishLine.SetPosition(1, endPos);
    }

    float GetThrowMagnitudePortion() {
        // Throws don't have a max throw speed, but the graphics need one to avoid getting insane??

        float throwMagnitudePortion = 0;
        float throwMagnitude = ball.currentThrowVector.magnitude;
        float maxThrowMagnitude = NewHandManager.GetInstance().maxThrowMagnitude;
        throwMagnitudePortion = throwMagnitude / maxThrowMagnitude;
        throwMagnitudePortion = Mathf.Clamp01(throwMagnitudePortion);
        
        return throwMagnitudePortion;
    }

    bool ShowLine()
    {
        return ball.IsHeld();
    }

    public void Reset() {
        squishLine.positionCount = 0;
    }

    // void UpdateLineWidth(float t)
    // {
    // }

    // public float squashAmount = .76f;

    // void AdjustLineWidth() {
    //     float t = (float)m_LinePointList.Count / (float)m_LineLength;
    //     trail.startWidth = NewBallManager.GetInstance().ballScale * (1 - squashAmount * t);
    // }
}
