using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinePredictor : MonoBehaviour
{

    // LineRenderer m_PreviewLineRenderer;
    public int m_PreviewLineSegments = 120;

    Vector3 m_ShotPower;
    float m_ShotPowerMultiplier;

    NewBall m_Ball;
    NewHand m_Hand;

    Vector3 m_LineStartPosition;
    List<Vector3> m_FinalPositionList;

    bool m_GotThrown = false;

    // Use this for initialization
    void Start()
    {
        // m_PreviewLineRenderer = GetComponent<LineRenderer>();
        m_Ball = GetComponent<NewBall>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // m_PreviewLineRenderer.enabled = EnableLine() && currentVelocity != Vector3.zero;
        // if (m_Ball.m_IsHeld)
        // {
        //     m_LineStartPosition = transform.position;
        // }

        bool updateLine = !m_Ball.m_BallThrown;

        if (updateLine) 
        {
            Vector3 currentVelocity = GetCurrentShotVelocity();
            List<Vector3> completeLinePositionList = new List<Vector3>(m_PreviewLineSegments);
            Vector3 currentLinePoint = Vector2.zero;

            const float dragPerFrame = -0.1f;
            Vector3 gravity = (Physics2D.gravity * Time.fixedDeltaTime);
            for (int i = 0; i < m_PreviewLineSegments; ++i)
            {
                completeLinePositionList.Add(m_LineStartPosition + currentLinePoint);

                //Add Drag
                Vector3 dragForce = dragPerFrame * currentVelocity.normalized * currentVelocity.sqrMagnitude;
                currentVelocity += dragForce * Time.fixedDeltaTime;

                //Add Gravity
                currentVelocity += gravity;

                currentLinePoint += currentVelocity * Time.fixedDeltaTime;

                if (currentLinePoint.y < -20)
                {
                    break;
                }
            }

            m_FinalPositionList = completeLinePositionList;
        }  


        // m_PreviewLineRenderer.positionCount = m_PreviewLineSegments;
        // m_PreviewLineRenderer.SetPositions(linePositions.ToArray());
    }

    // bool EnableLine()
    // {
    //     return m_Ball.m_IsHeld || m_gotThrown || m_Ball.m_BallThrown;
    // }

    public void SetHand(NewHand hand)
    {
        m_Hand = hand;
        m_LineStartPosition = transform.position;
    }

    Vector2 GetCurrentShotVelocity()
    {
        if (m_Hand != null)
        {
            return m_Hand.GetThrowVector();
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void HandleThrow()
    {
        m_LineStartPosition = transform.position;
        m_GotThrown = true;
    }

    public List<Vector3> GetPointList()
    {
        return m_FinalPositionList;
    }
}
