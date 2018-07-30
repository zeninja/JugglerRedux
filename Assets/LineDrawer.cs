using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{

    LineRenderer m_PreviewLineRenderer;
    public int m_PreviewLineSegments = 120;

    Vector3 m_ShotPower;
    float m_ShotPowerMultiplier;

    NewBall m_Ball;
    NewHand m_Hand;

    Vector3 lineStartPosition;

    bool m_gotThrown = false;

    // Use this for initialization
    void Start()
    {
        m_PreviewLineRenderer = GetComponent<LineRenderer>();
        m_Ball = GetComponent<NewBall>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // m_ShotPower.x = m_Player.GetAxis("Horizontal") + m_InitialShotPower.x;
        //         m_ShotPower.y = m_Player.GetAxis("Vertical") + m_InitialShotPower.y;
        // m_PreviewLineRenderer.transform.position = m_Basketball.transform.position;

        // Vector2 currentVelocity = m_ShotPower * m_ShotPowerMultiplier;

        Vector3 currentVelocity = GetCurrentShotVelocity();

        m_PreviewLineRenderer.enabled = EnableLine() && currentVelocity != Vector3.zero;

        List<Vector3> linePositions = new List<Vector3>(m_PreviewLineSegments);
        Vector3 currentLinePoint = Vector2.zero;

        const float dragPerFrame = -0.1f;
        Vector3 gravity = (Physics2D.gravity * Time.fixedDeltaTime);
        for (int i = 0; i < m_PreviewLineSegments; ++i)
        {
            if (m_Ball.isHeld) { lineStartPosition = transform.position; }
            linePositions.Add(lineStartPosition + currentLinePoint);

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

        m_PreviewLineRenderer.positionCount = m_PreviewLineSegments;
        m_PreviewLineRenderer.SetPositions(linePositions.ToArray());

    }

    bool EnableLine()
    {
        return m_Ball.isHeld || m_gotThrown;
    }

    public void SetHand(NewHand hand)
    {
        m_Hand = hand;
    }

    Vector2 GetCurrentShotVelocity()
    {
        if(m_Hand != null) {
            return m_Hand.GetThrowVector();
        } else {
            return Vector3.zero;
        }
    }

    public void HandleThrow()
    {
        lineStartPosition = transform.position;
        m_gotThrown = true;
    }
}
