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

	NewBall ball;

	Vector3 lineStartPosition;

	bool m_gotThrown = false;

    // Use this for initialization
    void Start()
    {
		
		m_PreviewLineRenderer = GetComponent<LineRenderer>();
		ball = GetComponent<NewBall>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // m_ShotPower.x = m_Player.GetAxis("Horizontal") + m_InitialShotPower.x;
        //         m_ShotPower.y = m_Player.GetAxis("Vertical") + m_InitialShotPower.y;
        // m_PreviewLineRenderer.transform.position = m_Basketball.transform.position;

        m_PreviewLineRenderer.enabled = EnableLine();

        List<Vector3> linePositions = new List<Vector3>(m_PreviewLineSegments);
        Vector3 currentLinePoint = Vector2.zero;
    
	    // Vector2 currentVelocity = m_ShotPower * m_ShotPowerMultiplier;
        Vector3 currentVelocity = GetCurrentShotVelocity();

        const float dragPerFrame = -0.1f;
        Vector3 gravity = (Physics2D.gravity * Time.fixedDeltaTime);
        for (int i = 0; i < m_PreviewLineSegments; ++i)
        {
			if(ball.isHeld) { lineStartPosition = transform.position; }
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

	bool EnableLine() {
		return ball.isHeld || m_gotThrown;
	}

	Vector2 GetCurrentShotVelocity() {
		return ball.currentThrowVector;
	}

	public void HandleThrow() {
		lineStartPosition = transform.position;
		m_gotThrown = true;
	}
}
