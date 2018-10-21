using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBall : MonoBehaviour
{
    public enum BallState { rising, falling, caught, launching, dead, firstBall }
    public BallState m_State;

    public enum BallStage { easy, normal, hard }
    public BallStage stage;

    [System.NonSerialized] public NewBallArtManager ballArtManager;
    [System.NonSerialized] public PredictiveLineDrawer predictiveLine;

    Rigidbody2D rb;

    public float defaultGravity = 20;
    public float drag = -0.1f;

    [HideInInspector] public Vector2 currentThrowVector;
    [HideInInspector] public bool m_BallThrown = false;

    bool m_IsHeld;


    // Endgame
    int ballCatchCount;
    public int ballColorIndex;
    //

    bool canPeak = false;

    int framesSinceCatch = 0;
    [System.NonSerialized] public bool dead;

    public bool firstBall;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;

        ballArtManager = GetComponentInChildren<NewBallArtManager>();
        predictiveLine = GetComponentInChildren<PredictiveLineDrawer>();

    }

    void Start()
    {
        ballColorIndex = NewBallManager._ballCount - 1;

        if(firstBall) {
            gameObject.layer = LayerMask.NameToLayer("Ball");
            // predictiveLine.EnableLine(true);
        }

        SetScale();
    }

    public void SetScale() {
        BroadcastMessage("UpdateScale", NewBallManager.GetInstance().ballScale);
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        #endif

        if (rb.velocity.y < 0)
        {
            SetBallState(BallState.falling);

            if (m_BallThrown && canPeak)
            {
                Peak();
            }
        }

        if (!IsLaunching())
        {
            CheckBounds();
        }

        framesSinceCatch++;
    }

    void FixedUpdate()
    {
        // QUADRATIC DRAG
        Vector2 force = drag * rb.velocity.normalized * rb.velocity.sqrMagnitude;
        rb.AddForce(force);
    }

    public void SetBallState(BallState newState)
    {

        BallState oldState = m_State;
        m_State = newState;

        // Nothing has changed. break out early.
        if (oldState != m_State) { 
            if(newState == BallState.falling) {
                // Peaked for the first time
                gameObject.layer = LayerMask.NameToLayer("Ball");
            }
         }
    }

    public void UpdateThrowInfo(Vector2 throwVector)
    {
        currentThrowVector = throwVector;
        // predictiveLine.DrawLine(transform.position, currentThrowVector);
    }

    public void GetCaught()
    {
        if (IsLaunching() || NewGameManager.GameOver()) { return; }
        if (firstBall)
        {
            NewGameManager.GetInstance().StartGame();
            firstBall = false;
        }

        m_IsHeld = true;
        framesSinceCatch = 0;

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        EventManager.TriggerEvent("BallCaught");

        BroadcastMessage("HandleCatch", SendMessageOptions.DontRequireReceiver);
        SetBallState(BallState.caught);
    }

    public void GetThrown(Vector2 throwVector)
    {
        if (IsLaunching() || NewGameManager.GameOver()) { return; }

        m_IsHeld = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(throwVector * rb.mass, ForceMode2D.Impulse);
        rb.gravityScale = defaultGravity;

        m_BallThrown = true;
        EventManager.TriggerEvent("BallThrown");
        BroadcastMessage("HandleThrow", SendMessageOptions.DontRequireReceiver);

        currentThrowVector = Vector2.zero;

        GetComponentInChildren<BallPathOutline>().DrawBallPath(transform.position, throwVector);

        if (throwVector.y > 0)
        {
            canPeak = true;
            SetBallState(BallState.rising);
        }
        else
        {
            canPeak = false;
            SetBallState(BallState.falling);
        }
    }

    void Peak()
    {
        BroadcastMessage("HandlePeak", SendMessageOptions.DontRequireReceiver);
        EventManager.TriggerEvent("BallPeaked");
        // Debug.Log("Peakin");
        m_BallThrown = false;
        NewBallManager.GetInstance().UpdateEndgame(this);
    }

    void CheckBounds()
    {
        Vector3 converted = Camera.main.WorldToScreenPoint(transform.position);

        if (converted.y < 0 || converted.y > Screen.height || converted.x < 0 || converted.x > Screen.width)
        {
            if (!NewGameManager.GameOver() && !dead)
            {
                KillThisBall();
                dead = true;
            }
        }
    }

    public bool CaughtJustNow()
    {
        return framesSinceCatch < 1;
    }

    void KillThisBall()
    {
        FreezeBall();
        GameOverManager.GetInstance().SetTargetBall(this);
        EventManager.TriggerEvent("BallDied");
    }

    public void FreezeBall()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        BroadcastMessage("HandleBallDeath");
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    void SetBallStage(BallStage newStage)
    {
        stage = newStage;
        switch (stage)
        {
            case BallStage.easy:

                break;

            case BallStage.normal:
                ballArtManager.UpdateToNormal();
                break;
        
            case BallStage.hard:
                ballArtManager.UpdateToHard();
                break;
        }

    }

    public void ProcessStageTransition()
    {

        switch (stage)
        {
            case BallStage.easy:
                if (ballColorIndex < NewBallManager.endgameBallCount)
                {
                    ballArtManager.SetColor(NewBallManager.GetInstance().m_BallColors[ballColorIndex]);
                    ballColorIndex++;
                } else {
                    SetBallStage(BallStage.normal);
                }

                break;

            case BallStage.normal:
                if(NewBallManager.GetInstance().AllBallsNormal()) {
                    SetBallStage(BallStage.hard);
                }
                break;
        
            case BallStage.hard:
                ballArtManager.UpdateToHard();
                break;
        }

    }

    public bool IsHeld()
    {
        return m_IsHeld;
    }

    public bool IsFalling()
    {
        return m_State == BallState.falling;
    }

    public bool IsLaunching()
    {
        return m_State == BallState.launching;
    }

    public bool IsRising()
    {
        return m_State == BallState.rising;
    }
}