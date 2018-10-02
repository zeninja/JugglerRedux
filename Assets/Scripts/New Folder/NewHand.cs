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
    // CatchRingSpawner m_CatchRingSpawner;
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
        // m_CatchRingSpawner = GetComponentInChildren<CatchRingSpawner>();
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
        // Make sure we can't catch two balls with one finger
        if (m_Ball == null)
        {
            // Check all possible balls
            int layerMask = 1 << LayerMask.NameToLayer("Ball");
            float radius = GetComponent<CircleCollider2D>().radius;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, layerMask);

            float[] distances = new float[hits.Length];
            float[] heights  = new float[hits.Length];
            // int[] depths     = new int[hits.Length];

            for(int i = 0; i < hits.Length; i++) {
                Debug.Log(hits[i].transform.gameObject.name);
                heights[i]    = hits[i].transform.position.y;
                distances [i] = (transform.position - hits[i].transform.position).magnitude;
                // depths[i]     = hits[i].transform.gameObject.GetComponentInChildren<NewBallArtManager>().currentDepth;
            }

            float minHeight   = Mathf.Min(heights);
            float minDistance = Mathf.Min(distances);
            // int   maxDepth    = Mathf.Max(depths);


            int ballIndex   = 0;
            int heightIndex = FindTargetValue(heights, minHeight);
            int distIndex   = FindTargetValue(distances, minDistance);
            // int depthIndex  = FindTargetValue(depths, maxDepth);

            ballIndex = heightIndex;

            NewBall targetBall = hits[ballIndex].transform.GetComponent<NewBall>();
            SetBall(targetBall);

            // Old Approach
            // Make sure we're catching a ball
            // if (other.CompareTag("Ball"))
            // {
            //     NewBall ballToJuggle = other.gameObject.GetComponent<NewBall>();
            //     if (!ballToJuggle.IsLaunching()) { 
            //         SetBall(ballToJuggle);
            //     }
            // }
        }
    }



    int FindTargetValue(float[] array, float target) {
        for(int i = 0; i < array.Length; i++) {
            if(array[i] == target) {
                return i;
            }
        }
        return 0;
    }

     int FindTargetValue(int[] array, int target) {
        for(int i = 0; i < array.Length; i++) {
            if(array[i] == target) {
                return i;
            }
        }
        return 0;
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

        if (m_GrabThrowVector == Vector2.zero) {
            m_GrabThrowVector = new Vector2(0, -.001f);
        }

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
            // m_CatchRingSpawner.SpawnRing(ballColor);
            m_FingerRing.TriggerRing(ballColor);
            
            Vibrator.Vibrate(vibeDuration);
            // CatchAndDragView.GetInstance().SetCatchPosition(transform.position);
        }
    }

    void ThrowBall()
    {

        m_Ball.GetThrown(m_GrabThrowVector);
        m_Ball = null;
        // CatchAndDragView.GetInstance().SetThrowPosition(transform.position);
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
