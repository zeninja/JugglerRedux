﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    #region Instance 
    private static NewHand instance;

    public static NewHand GetInstance()
    {
        return instance;
    }
    #endregion

    Transform m_Transform;

    Vector2 m_LastFramePos;
    Vector2 m_MostRecentMoveDir;
    Vector2 m_SlapThrowVector;
    Vector2 m_GrabMoveDir;
    Vector2 m_GrabThrowVector;
    Vector2 m_CatchPosition;

    NewBall m_Ball;

    public bool useMouse = false;
    public int fingerId;

    private bool m_BallGrabbedFirstFrame;

    List<Vector2> m_PositionHistory;

    [System.NonSerialized] public float grabThrowForce = 4;
    [System.NonSerialized] public float m_SlapThrowForce = 10;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }

        InitHand();

    }

    void InitHand()
    {
        m_Transform = transform;

        grabThrowForce = NewHandManager.GetInstance().heldThrowForce;
        m_SlapThrowForce = NewHandManager.GetInstance().immediateThrowForce;

        m_PositionHistory = new List<Vector2>();
    }

    void FixedUpdate()
    {
        CheckForBall();
    }

    bool d_BallSeenThisFrame;

    void Update()
    {
        d_BallSeenThisFrame = m_Ball != null;

        // dEBUG
        m_SlapThrowForce = NewHandManager.GetInstance().immediateThrowForce;

        HandleInput();
    }

    void HandleInput()
    {
        if (useMouse)
        {
            HandleMouseInput();
        }
        else
        {
            HandleTouchInput();
        }
    }

    void HandleMouseInput()
    {
        // Set the position to be equal to the current mouse position
        m_Transform.position = Extensions.MouseScreenToWorld();

        if (m_LastFramePos != null)
            m_MostRecentMoveDir = (Vector2)m_Transform.position - m_LastFramePos;

        // Set the last frame position (for next frame)
        m_LastFramePos = m_Transform.position;

        if (m_Ball != null)
        {
            if (FirstFrame())
            {
                Debug.Log("First frame");

                // The finger tapped directly on the ball, catch it and drag to throw it
                m_Ball.GetComponent<NewBall>().GetCaught();

                m_CatchPosition = m_Transform.position;
                m_BallGrabbedFirstFrame = true;
            }
            else
            {
                Debug.Log("Not first frame");

                if (m_BallGrabbedFirstFrame)
                {
                    // Find Grab/Drag throw vector
                    FindMouseGrabThrowVector();

                    if (Input.GetMouseButtonUp(0))
                    {
                        ThrowBall();
                    }
                }
                else
                {
                    Debug.Log(m_PositionHistory.Count);
                    m_SlapThrowVector = m_MostRecentMoveDir * m_SlapThrowForce;
                    SlapBall();
                }
            }
        }

        m_PositionHistory.Add(m_Transform.position);
    }

    void HandleTouchInput()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.fingerId == fingerId)
            {
                m_Transform.position = Camera.main.ScreenToWorldPoint(t.position);
                m_MostRecentMoveDir = t.deltaPosition;

                if (m_Ball != null)
                {
                    if (FirstFrame())
                    {
                        Debug.Log("First frame");

                        // The finger tapped directly on the ball, catch it and drag to throw it
                        m_Ball.GetComponent<NewBall>().GetCaught();
                        m_CatchPosition = m_Transform.position;
                        m_BallGrabbedFirstFrame = true;
                    }
                    else
                    {
                        Debug.Log("Not first frame");

                        if (m_BallGrabbedFirstFrame)
                        {
                            // Find Grab/Drag throw vector
                            FindTouchGrabThrowVector();

                            if(t.phase == TouchPhase.Ended) {
                                ThrowBall();
                            }
                        }
                        else
                        {
                            if(t.phase != TouchPhase.Ended) {
                                Debug.Log(m_PositionHistory.Count);
                                m_SlapThrowVector = m_MostRecentMoveDir * m_SlapThrowForce;
                                SlapBall();
                            }
                        }
                    }
                }
            }
        }
    }

    int physicsFramesBallWasSeen = 0;

    void CheckForBall()
    {
        float radius = 1f;
        LayerMask mask = 1 << LayerMask.NameToLayer("Ball");
        // RaycastHit2D hit = Physics2D.CircleCast(m_Transform.position, radius, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(m_Transform.position, Vector2.zero, 0, mask);


        Debug.Log(hit.collider);

        // Only catch one ball at a time
        if (hit && m_Ball == null)
        {
            physicsFramesBallWasSeen++;
            Debug.Log("Hit a ball");
            NewBall ballToJuggle = hit.collider.gameObject.GetComponent<NewBall>();

            // only catch balls that are not launching
            if (!ballToJuggle.launching)
            {
                // Debug.Log("Setting ball");
                SetBall(ballToJuggle);
            }
        }

        // Holding a ball, update the grab throw direction
        if (m_Ball != null)
        {
            m_GrabThrowVector = m_GrabMoveDir * grabThrowForce;
        }
    }

    void SetBall(NewBall caughtBall)
    {
        m_Ball = caughtBall;
        m_Ball.GetComponent<LineDrawer>().SetHand(this);
    }

    void FindMouseGrabThrowVector()
    {
        if (Input.GetMouseButton(0))
        {
            m_GrabMoveDir = (Vector2)m_Transform.position - m_CatchPosition;
            m_GrabThrowVector = m_GrabMoveDir * grabThrowForce;
        }
    }

    void FindTouchGrabThrowVector() {
        m_GrabMoveDir = (Vector2)m_Transform.position - m_CatchPosition;
        m_GrabThrowVector = m_GrabMoveDir * grabThrowForce;
    }

    void SlapBall()
    {
        m_Ball.GetComponent<NewBall>().GetCaughtAndThrown(m_SlapThrowVector);
        HandleDeath();
    }

    void ThrowBall()
    {
        m_Ball.GetThrown(m_GrabThrowVector);
        m_Ball = null;
        HandleDeath();
    }

    public Vector2 GetThrowVector()
    {
        return m_GrabThrowVector;
    }

    bool FirstFrame()
    {
        return m_PositionHistory.Count <= 1;
    }

    void HandleDeath()
    {
        NewHandManager.GetInstance().RemoveID(fingerId);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_Transform.position, .5f);
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), ((Vector2)m_Transform.position).ToString());
    }
}
