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
    [HideInInspector]
    public bool m_Launching;
    [HideInInspector]
    public Vector2 currentThrowVector;

    public bool m_IsHeld;

    [HideInInspector]
    public bool m_BallThrown = false;

    // Endgame
    int ballCatchCount;
    public int ballColorIndex;

    NewBallArtManager ballArtManager;

    int framesSinceCatch = 0;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;
        transform.localScale = Vector2.one * NewBallManager.GetInstance().ballScale;

        ballArtManager = GetComponentInChildren<NewBallArtManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }

        if (rb.velocity.y < 0)
        {
            EventManager.TriggerEvent("BallPeaked");
            m_Launching = false;
            m_BallThrown = false;
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

        // Debug.Log("Getting caught and thrown");
        rb.velocity = Vector2.zero;
        rb.AddForce(throwVector * rb.mass, ForceMode2D.Impulse);
        rb.gravityScale = defaultGravity;
        GetComponent<LinePredictor>().HandleThrow();
        EventManager.TriggerEvent("BallSlapped");
    }

    int catchFrame;

    public void GetCaught()
    {
        if (m_Launching || NewGameManager.GameOver()) { return; }
        // Debug.Log("Got caught");

        m_IsHeld = true;
        framesSinceCatch = 0;
        
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        EventManager.TriggerEvent("BallCaught");

        // NewBallManager.GetInstance().MoveBallToFront(ballArtManager);
    }

    public void GetThrown(Vector2 throwVector)
    {
        if (m_Launching || NewGameManager.GameOver()) { return; }

        m_IsHeld = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(throwVector * rb.mass, ForceMode2D.Impulse);
        rb.gravityScale = defaultGravity;
        GetComponent<LinePredictor>().HandleThrow();

        m_BallThrown = true;
        EventManager.TriggerEvent("BallThrown");

        // Debug.Log("Ball Thrown");

        // IncrementEndgame();

        NewBallManager.GetInstance().UpdateEndgame(this);
    }

    void CheckBounds()
    {
        Vector3 converted = Camera.main.WorldToScreenPoint(transform.position);

        if (converted.y < 0 || converted.y > Screen.height || converted.x < 0 || converted.x > Screen.width)
        {
            if (!NewGameManager.GameOver())
            {
                HandleDeath();
            }
        }
    }

    public bool CaughtJustNow() {
        return framesSinceCatch < 1;
    }

    public void HandleDeath()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        EventManager.TriggerEvent("BallDied");
        // EventManager.TriggerEvent("GameOver");
    }

    public void FreezeBall() {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    public void Die()
    {
        StartCoroutine(DeathProcess());
    }

    IEnumerator DeathProcess() {
        yield return StartCoroutine(ballArtManager.Explode());
        Destroy(gameObject);
    }

    public void UpdateColor() {
        GetComponent<NewBallArtManager>().SetColor(NewBallManager.GetInstance().m_BallColors[ballColorIndex]);
    }

    // public void SetColor(Color newColor)
    // {
    //     m_BallSprite.GetComponent<SpriteRenderer>().color = newColor;

    //     ballColorIndex = NewBallManager._ballCount - 1;
    // }

    // public void SetColor()
    // {
    //     // GetComponent<SpriteRenderer>().color = NewBallManager.GetInstance().m_BallColors[ballColorIndex];
    //     GetComponent<NewBallArtManager>().SetColor(NewBallManager.GetInstance().m_BallColors[ballColorIndex]);
    // }
}