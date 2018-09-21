using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FirstBallPreview : MonoBehaviour
{
    public int m_PreviewLineSegments = 120;

    public NewHand m_Hand;

    Vector3 m_LineStartPosition;
    public List<Vector3> m_FinalPositionList;

    // bool m_GotThrown = false;

    LineRenderer previewLine;

    // Use this for initialization
    void Awake()
    {
        previewLine = GetComponent<LineRenderer>();
        previewLine.material.color = GetComponentInChildren<NewBallArtManager>().myColor;
        previewLine.positionCount = 0;

        m_LineStartPosition = transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindPositionList();

        // if(GetComponent<NewBall>().firstBall) {
        //     // previewLine.positionCount = m_PreviewLineSegments;
        //     previewLine.SetPositions(m_FinalPositionList.ToArray());
        // }

    }

    public void FindPositionList() {
        
        Vector3 currentVelocity = GetCurrentShotVelocity();
        // Debug.Log(GetCurrentShotVelocity());
        List<Vector3> completeLinePositionList = new List<Vector3>();
        Vector3 currentLinePoint = Vector2.zero;

        const float dragPerFrame = -0.1f;
        Vector3 gravity = (Physics2D.gravity * Time.fixedDeltaTime);
        // for (int i = 0; i < m_PreviewLineSegments; ++i)
        // {
        //     completeLinePositionList.Add(m_LineStartPosition + currentLinePoint);

        //     //Add Drag
        //     Vector3 dragForce = dragPerFrame * currentVelocity.normalized * currentVelocity.sqrMagnitude;
        //     currentVelocity += dragForce * Time.fixedDeltaTime;

        //     //Add Gravity
        //     currentVelocity += gravity;

        //     currentLinePoint += currentVelocity * Time.fixedDeltaTime;

        //     if (currentLinePoint.y < -20)
        //     {
        //         break;
        //     }
        // }


        int numIterations = 0;

        for(int i = 0; i < 500; i++) {
            completeLinePositionList.Add(m_LineStartPosition + currentLinePoint);

            //Add Drag
            Vector3 dragForce = dragPerFrame * currentVelocity.normalized * currentVelocity.sqrMagnitude;
            currentVelocity += dragForce * Time.fixedDeltaTime;

            //Add Gravity
            currentVelocity += gravity;
            numIterations++;

            currentLinePoint += currentVelocity * Time.fixedDeltaTime;

            if(currentVelocity.y < 0) {
                break;
            }
            // Debug.Log(currentVelocity);

        }
        // if(numIterations > 500) {
        //     Debug.Break();
        // }

        m_FinalPositionList = completeLinePositionList;

        previewLine.positionCount = m_FinalPositionList.Count;
        previewLine.SetPositions(m_FinalPositionList.ToArray());
    }

   

    Vector2 GetCurrentShotVelocity()
    {
        if (m_Hand != null)
        {
            // Hand exists, getting thrown
            return m_Hand.GetThrowVector();
        }
        else
        {
            // Launching
            return (Vector2)transform.position + NewBallManager.GetInstance().GetLaunchVelocity();
        }
    }

    public void HandleCatch(NewHand hand) {
        m_Hand = hand;
        m_LineStartPosition = transform.position;
    }

    public void HandleThrow()
    {
        m_Hand = null;
        // m_LineStartPosition = transform.position;
    }
}
