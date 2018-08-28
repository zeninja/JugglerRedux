using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinePredictor : MonoBehaviour
{
    public int m_PreviewLineSegments = 120;

    NewHand m_Hand;

    Vector3 m_LineStartPosition;
    public List<Vector3> m_FinalPositionList;

    // bool m_GotThrown = false;

    LineRenderer previewLine;

    // Use this for initialization
    void Awake()
    {
        previewLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindPositionList();


        // m_PreviewLineRenderer.positionCount = m_PreviewLineSegments;
        // m_PreviewLineRenderer.SetPositions(linePositions.ToArray());
    }

    public void FindPositionList() {
        Vector3 currentVelocity = GetCurrentShotVelocity();
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

    public List<Vector3> FindLaunchList() {
        m_LineStartPosition = transform.position;
        
        Vector3 currentVelocity = NewBallManager.GetInstance().GetLaunchVelocity();
        List<Vector3> launchList = new List<Vector3>();
        Vector3 currentLinePoint = Vector2.zero;

        const float dragPerFrame = -0.1f;
        Vector3 gravity = (Physics2D.gravity * Time.fixedDeltaTime);

        for(int i = 0; i < 500; i++) {
            launchList.Add(m_LineStartPosition + currentLinePoint);

            //Add Drag
            Vector3 dragForce = dragPerFrame * currentVelocity.normalized * currentVelocity.sqrMagnitude;
            currentVelocity += dragForce * Time.fixedDeltaTime;

            //Add Gravity
            currentVelocity += gravity;

            currentLinePoint += currentVelocity * Time.fixedDeltaTime;

            if(currentVelocity.y < 0) {
                break;
            }
        }

        // Debug.Log("4. SET LAUNCH LIST. LENGTH IS: " + launchList.Count);
        return launchList;
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

    public List<Vector3> GetPointList()
    {
        if(m_FinalPositionList.Count == 0) {
            FindPositionList();
        }
        return m_FinalPositionList;
    }
}
