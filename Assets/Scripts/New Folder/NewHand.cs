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
    CatchRing m_CatchRing;
    FingerRing m_FingerRing;

    NewBall m_Ball;

    [System.NonSerialized] public bool useMouse = false;
    public int m_FingerID;

    private bool m_BallGrabbedFirstFrame;

    List<Vector2> m_PositionHistory;

    [System.NonSerialized] public float grabThrowForce = 4;

    public float vibeDuration = .05f;

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
        }
        else
        {
            grabThrowForce = NewHandManager.GetInstance().touchGrabThrowForce;
        }

        m_PositionHistory = new List<Vector2>();
        m_CatchRing = GetComponentInChildren<CatchRing>();
        m_FingerRing = GetComponentInChildren<FingerRing>();
    }

    void Start() {
        EventManager.StartListening("GameOver", HandleDeath);
    }

    void FixedUpdate()
    {
        // CheckForBall();

        Hover();
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

    void Hover() {
        int layerMask = 1 << LayerMask.NameToLayer("Monolith");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0, layerMask);

        if(hit) {
            hit.transform.GetComponent<Monolith>().GetFingerHover(transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Ball == null)
        {
            if (other.CompareTag("Ball"))
            {
                NewBall ballToJuggle = other.gameObject.GetComponent<NewBall>();
                if (!ballToJuggle.IsLaunching()) { 
                    SetBall(ballToJuggle);
                }
            }
        }
    }

    void SetBall(NewBall caughtBall)
    {
        m_Ball = caughtBall;
    }

    void FindGrabThrowVector()
    {
        int throwDirectionModifier = NewHandManager.dragUpToThrow ? 1 : -1;
        m_GrabMoveDir = ((Vector2)m_Transform.position - m_CatchPosition) * throwDirectionModifier;
        m_GrabThrowVector = m_GrabMoveDir * grabThrowForce;

        m_Ball.UpdateThrowInfo(m_GrabThrowVector);
    }

    void GrabBall()
    {
        if(!m_BallGrabbedFirstFrame) {
            // Debug.Log("Ball grabbed");
            m_Ball.GetCaught();
            m_CatchPosition = m_Transform.position;
            m_BallGrabbedFirstFrame = true;
            Color ballColor = m_Ball.GetComponentInChildren<NewBallArtManager>().myColor;
            m_CatchRing.TriggerRing(ballColor);
            m_FingerRing.TriggerRing(ballColor);
            
            Vibrator.Vibrate(vibeDuration);
        }
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
