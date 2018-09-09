using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBall : MonoBehaviour
{
    Rigidbody2D rb;

    // public float scale = .25f;
    public float defaultGravity = 20;
    public float drag = -0.1f;

    [HideInInspector]
    public bool canBeCaught = false;
    // [HideInInspector]
    public bool m_Launching;
    [HideInInspector]
    public Vector2 currentThrowVector;

    public bool m_IsHeld;

    [HideInInspector]
    public bool m_BallThrown = false;

    // Endgame
    int ballCatchCount;
    public int ballColorIndex;
    //

    bool canPeak = false;

    [System.NonSerialized]
    public NewBallArtManager ballArtManager;

    int framesSinceCatch = 0;
    [System.NonSerialized]
    public bool dead;

    public bool firstBall;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;

        ballArtManager = GetComponentInChildren<NewBallArtManager>();
        if(firstBall){
            
        }
    }

    void Start() {
        transform.localScale = Vector2.one * NewBallManager.GetInstance().ballScale;
        ballColorIndex = NewBallManager._ballCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITYEDITOR
        if (Input.GetMouseButtonDown(1))
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        #endif

        if (rb.velocity.y < 0)
        {
            m_Launching = false;

            if(m_BallThrown && canPeak) {
                EventManager.TriggerEvent("BallPeaked");
                m_BallThrown = false;
                // GetComponentInChildren<SpriteCircleEffectSpawner>().SpawnRing(transform.position);
            }
        }

        if (!m_Launching)
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

    public void GetCaughtAndThrown(Vector2 throwVector)
    {
        if (m_Launching || NewGameManager.GameOver()) { return; }

        rb.velocity = Vector2.zero;
        rb.AddForce(throwVector * rb.mass, ForceMode2D.Impulse);
        rb.gravityScale = defaultGravity;
        EventManager.TriggerEvent("BallSlapped");
    }

    public void GetCaught()
    {
        if (m_Launching || NewGameManager.GameOver()) { return; }

        if(firstBall) { NewGameManager.GetInstance().StartGame(); }

        m_IsHeld = true;
        framesSinceCatch = 0;
        
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        EventManager.TriggerEvent("BallCaught");
        
        ballArtManager.HandleCatch();

        GetComponentInChildren<SpriteCircleEffectSpawner>().SpawnRing(transform.position);
    }

    public void GetThrown(Vector2 throwVector)
    {
        if (m_Launching || NewGameManager.GameOver()) { return; }

        m_IsHeld = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(throwVector * rb.mass, ForceMode2D.Impulse);
        rb.gravityScale = defaultGravity;

        m_BallThrown = true;
        EventManager.TriggerEvent("BallThrown");

        ballArtManager.HandleThrow();

        currentThrowVector = Vector2.zero;

        if(throwVector.y > 0) {
            canPeak = true;
        } else {
            canPeak = false;
        }

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

    public bool CaughtJustNow() {
        return framesSinceCatch < 1;
    }

    void KillThisBall() {
        FreezeBall();
        ballArtManager.HandleDeath();
        GameOverManager.GetInstance().SetTargetBall(this);
        EventManager.TriggerEvent("BallDied");
        // GameOverManager.GetInstance().StartGameOver(ballArtManager.ball);
    }

    public void FreezeBall() {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void UpdateColor() {
        if(ballColorIndex < NewBallManager.endgameBallCount) {
            // Debug.Log( ballColorIndex);
            ballColorIndex++;
            ballArtManager.SetColor(NewBallManager.GetInstance().m_BallColors[ballColorIndex]);
        }
    }

    public bool IsHeld() {
        return m_IsHeld;
    }
}