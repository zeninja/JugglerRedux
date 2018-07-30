using System.Collections;
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

    [System.NonSerialized] public bool useMouse = false;
    public int m_FingerID;

    private bool m_BallGrabbedFirstFrame;

    List<Vector2> m_PositionHistory;

    [System.NonSerialized] public float grabThrowForce = 4;
    [System.NonSerialized] public float slapThrowForce = 10;

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

    void FixedUpdate()
    {
        CheckForBall();
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
                Debug.Log("First frame");
                // Debug.Log("Finger tapped directly");
                // The finger tapped directly on the ball, catch it and drag to throw it
                GrabBall();
            }
            else
            {
                Debug.Log("Not first frame");

                if (m_BallGrabbedFirstFrame)
                {

                    // Find Grab/Drag throw vector
                    FindGrabThrowVector();

                    if (Input.GetMouseButtonUp(0))
                    {
                        ThrowBall();
                    }
                }
                else
                {
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
            Debug.Log(Input.touchCount);
            if (t.fingerId == m_FingerID)
            {
                Debug.Log("Checking " + t.fingerId);

                m_Transform.position = Extensions.TouchScreenToWorld(t);

                // Find slap throw vector
                m_MostRecentMoveDir = t.deltaPosition;

                if (m_Ball != null)
                {
                    if (FirstFrame() && !m_BallGrabbedFirstFrame)
                    {
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
                        Debug.Log("Finger: " + t.fingerId + " Touch Phase Ended");
                        HandleDeath();
                    }
                }

                m_PositionHistory.Add(m_Transform.position);
            }
        }
    }

    void CheckForBall()
    {
        float radius = 0.5f;
        RaycastHit2D hit = Physics2D.CircleCast(m_Transform.position, radius, Vector2.zero);

        // Only catch one ball at a time
        if (hit && m_Ball == null)
        {
            if (hit.collider.gameObject.CompareTag("Ball"))
            {
                // Debug.Log("Hit a ball");
                NewBall ballToJuggle = hit.collider.gameObject.GetComponent<NewBall>();

                // only catch balls that are not launching
                if (!ballToJuggle.launching)
                {
                    // Debug.Log("Setting ball");
                    SetBall(ballToJuggle);
                }
            }
        }

        // Holding a ball, update the grab throw direction
        // if (m_Ball != null)
        // {
        //     m_GrabThrowVector = m_GrabMoveDir * grabThrowForce;
        // }
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
    }

    void GrabBall()
    {
        Debug.Log("Ball grabbed");
        m_Ball.GetComponent<NewBall>().GetCaught();
        m_Ball.GetComponent<LineDrawer>().SetHand(this);
        m_CatchPosition = m_Transform.position;
        m_BallGrabbedFirstFrame = true;
    }

    void SlapBall()
    {
        m_SlapThrowVector = m_MostRecentMoveDir * slapThrowForce;
        // Debug.Log("Slapping. Slap vector: " + m_SlapThrowVector);
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
        return m_PositionHistory.Count < 2;
    }

    void HandleDeath()
    {
        // Debug.Log("Hand dying " + m_FingerID);
        // NewHandManager.GetInstance().RemoveID(m_FingerID);
        // Destroy(gameObject);
    }

    void OnGUI()
    {
        // GUI.color = Color.black;
        // GUI.Label(new Rect(0, 100 * m_FingerID, Screen.width, Screen.height), ((Vector3)m_Transform.position).ToString());

        // Vector2 startPos = Camera.main.WorldToScreenPoint(transform.position);
        // startPos.x += 100;
        // startPos.y += 50;
        // startPos.y = Screen.height - startPos.y;

        // string handInfo = "FingerID: " + m_FingerID + "\n" +
        //                   "Num fingers: " + NewHandManager.GetCurrentFingerIDCount().ToString() + "\n" + 
        //                   "touchCount: " + Input.touchCount;

        // GUI.Label(new Rect(startPos.x, startPos.y, 100, 500), handInfo);
    }
}
