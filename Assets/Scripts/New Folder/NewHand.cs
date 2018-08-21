using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    Transform m_Transform;

    Vector2 m_LastFramePos;
    Vector2 m_MostRecentMoveDir;
    Vector2 m_SlapThrowVector;
    Vector2 m_GrabMoveDir;
    Vector2 m_GrabThrowVector;
    Vector2 m_CatchPosition;

    NewBall m_Ball;

    [System.NonSerialized] public bool useMouse = false;
    public int m_FingerID;

    private bool m_BallGrabbedFirstFrame;

    List<Vector2> m_PositionHistory;

    [System.NonSerialized] public float grabThrowForce = 4;
    [System.NonSerialized] public float slapThrowForce = 10;

    void Awake()
    {
        InitHand();
    }

    void InitHand()
    {
        m_Transform = transform;

        if (useMouse)
        {
            grabThrowForce = NewHandManager.GetInstance().mouseGrabThrowForce;
            slapThrowForce = NewHandManager.GetInstance().mouseSlapThrowForce;
        }
        else
        {
            grabThrowForce = NewHandManager.GetInstance().touchGrabThrowForce;
            slapThrowForce = NewHandManager.GetInstance().touchSlapThrowForce;
        }

        m_PositionHistory = new List<Vector2>();
    }

    void Start() {
        EventManager.StartListening("GameOver", HandleDeath);
    }

    void FixedUpdate()
    {
        // CheckForBall();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // if(NewGameManager.GetInstance().gameState == GameState.gameOver) { return; }

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

        // Check num positions so that first frame catches do not throw the ball immediately
        if (m_LastFramePos != null)
            m_MostRecentMoveDir = (Vector2)m_Transform.position - m_LastFramePos;

        // Set the last frame position (for next frame)
        m_LastFramePos = m_Transform.position;

        if (m_Ball != null)
        {
            if (FirstFrame())
            {
                // The finger tapped directly on the ball, catch it and drag to throw it
                GrabBall();
            }
            else
            {
                if (m_BallGrabbedFirstFrame)
                {
                    // Find Grab/Drag throw vector
                    FindGrabThrowVector();

                    if (Input.GetMouseButtonUp(0))
                    {
                        // Debug.Log("Mouse up");
                        ThrowBall();
                    }
                }
                else
                {
                    SlapBall();
                    // GrabBall();
                }
            }
        }

        m_PositionHistory.Add(m_Transform.position);

        if(Input.GetMouseButtonUp(0)) {
            HandleDeath();
        }

    }

    void HandleTouchInput()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.fingerId == m_FingerID)
            {

                m_Transform.position = Extensions.TouchScreenToWorld(t);

                // Find slap throw vector
                m_MostRecentMoveDir = t.deltaPosition;

                if (m_Ball != null)
                {
                    // Circlecast found a ball under the finger
                    if (FirstFrame() && !m_BallGrabbedFirstFrame)
                    {
                        // Grab the ball
                        GrabBall();
                    }
                    else
                    {
                        if (m_BallGrabbedFirstFrame)
                        {
                            FindGrabThrowVector();

                            if (t.phase == TouchPhase.Ended)
                            {
                                // Debug.Log("Throwing. Throw vector: " + m_GrabThrowVector);
                                ThrowBall();
                            }
                        }
                        else if (t.phase != TouchPhase.Ended)
                        {
                            SlapBall();
                        }
                    }
                }
                else
                {
                    if (t.phase == TouchPhase.Ended)
                    {
                        // Debug.Log("Finger: " + t.fingerId + " Touch Phase Ended");
                        HandleDeath();
                    }
                }

                m_PositionHistory.Add(m_Transform.position);
            }
        }
    }

    // void CheckForBall()
    // {
    //     float radius = GetComponent<CircleCollider2D>().radius;

    //     RaycastHit2D hit = Physics2D.CircleCast(m_Transform.position, radius, Vector2.zero);

    //     // Only catch one ball at a time
    //     if (hit && m_Ball == null)
    //     {
    //         if (hit.collider.gameObject.CompareTag("Ball"))
    //         {
    //             // Debug.Log("Hit a ball");
    //             NewBall ballToJuggle = hit.collider.gameObject.GetComponent<NewBall>();

    //             // only catch balls that are not launching
    //             if (!ballToJuggle.m_Launching)
    //             {
    //                 // Debug.Log("Setting ball");
    //                 SetBall(ballToJuggle);
    //             }
    //         }
    //     }
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Ball == null)
        {
            if (other.CompareTag("Ball"))
            {
                NewBall ballToJuggle = other.gameObject.GetComponent<NewBall>();
                if (!ballToJuggle.m_Launching) { 
                    // Debug.Log("Set a new ball");
                    SetBall(ballToJuggle);
                }
            }
        }
    }

    void SetBall(NewBall caughtBall)
    {
        m_Ball = caughtBall;
        // Debug.Break();
    }

    void FindGrabThrowVector()
    {
        m_GrabMoveDir = (Vector2)m_Transform.position - m_CatchPosition;
        m_GrabThrowVector = m_GrabMoveDir * grabThrowForce;
        m_Ball.currentThrowVector = m_GrabThrowVector;
    }

    void GrabBall()
    {
        if(!m_BallGrabbedFirstFrame) {
            // Debug.Log("Ball grabbed");
            m_Ball.GetCaught();
            GetComponentInChildren<CatchRing>().SetColor(m_Ball.GetComponentInChildren<NewBallArtManager>().myColor);
            m_CatchPosition = m_Transform.position;
            m_BallGrabbedFirstFrame = true;
        }
    }

    void SlapBall()
    {
        // if(!NewBallManager.allowSlaps) { return; }

        // // Debug.Log("Ball slapped");
        // m_SlapThrowVector = m_MostRecentMoveDir * slapThrowForce;
        // // Debug.Log("Slapping. Slap vector: " + m_SlapThrowVector);
        // m_Ball.GetComponent<NewBall>().GetCaughtAndThrown(m_SlapThrowVector);
        // HandleDeath();
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
        return m_PositionHistory.Count <= 2;
    }

    void HandleDeath()
    {
        NewHandManager.GetInstance().RemoveID(m_FingerID);
        Destroy(gameObject);
    }
}
