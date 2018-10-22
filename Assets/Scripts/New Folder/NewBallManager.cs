using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBallManager : MonoBehaviour
{
    #region instance
    private static NewBallManager instance;
    public static NewBallManager GetInstance()
    {
        return instance;
    }
    #endregion

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
    }

    public NewBall m_BallPrefab;
    public static int _ballCount;
    public static bool useRails = false;
    public float ballScale = .4f;

    NewBall firstBall;
    List<NewBall> balls = new List<NewBall>();
    // List<NewBallArtManager> ballsSortedByDepth = new List<NewBallArtManager>();
    public float ballLaunchForce = 10;

    public Color[] m_BallColors;

    public int ballSpeedIndex;  // used to choose a SET, ie: slow, normal, fast
    int scoreIndex;             // used to trigger the ball spawn, the index of the score WITHIN one set
    public int[] ballSpawnScores;
    int[] slowBallSpawnScores   = new int[] { 5, 10, 25, 50, 75, 100, 125, 150 };
    int[] normalBallSpawnScores = new int[] { 5, 15, 25, 40, 55, 70,   99, 125 };
    int[] fastBallSpawnScores   = new int[] { 5, 10, 20, 35, 50, 65,   80, 99 };

    public enum BallSpawnSpeed { slow, med, fast };
    public BallSpawnSpeed ballSpawnSpeed = BallSpawnSpeed.med;

    public int juggleThreshold = 3;
    // public static float unheldBallCount;

    public Vector2 ballSpawnPos;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SpawnBall", SpawnBall);
        EventManager.StartListening("BallPeaked", CheckBallLaunch);
        EventManager.StartListening("BallDied", OnBallDied);

        // ballSpawnScores = fastBallSpawnScores;
        SetBallLaunchScores();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_ballCount < endgameBallCount) { 
                EventManager.TriggerEvent("SpawnBall");
            }
        }
    }

    public float GetUnheldBallCount() {
        int unheldBalls = 0;
        for(int i = 0; i < balls.Count; i++) {
            if(!balls[i].IsHeld()) {
                unheldBalls++;
            }
        }
        return unheldBalls;
    }

    public bool InJuggleTime()
    {
        foreach(NewBall n in balls) {
            if(n.IsFalling()) {
                return true;
            }
        }

        int numBallsThrowing = 0;

        foreach (NewBall n in balls)
        {
            if (n.m_BallThrown)
            {
                numBallsThrowing++;
                if (numBallsThrowing >= juggleThreshold)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    int xSwitcher = 1;
    public Vector2 spawnPos;
    public IEnumerator SpawnFirstBall() {
        if(firstBall != null) {
            Debug.Log("First ball already spawned. Stopping Ball Spawn.");
            StopCoroutine(SpawnFirstBall());
        }
        Debug.Log("Spawning First Ball.");

        xSwitcher *= -1;

        if (GlobalSettings.Settings.offsetXSpawnPosition) {
            ballSpawnPos = new Vector2(xSwitcher * spawnPos.x, spawnPos.y);
        } else {
            ballSpawnPos = new Vector2(0, spawnPos.y);
        }
        NewBall ball = Instantiate(m_BallPrefab);

        ball.transform.position = ballSpawnPos;
        ball.firstBall   = true;
        ball.GetComponent<Rigidbody2D>().velocity  = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().gravityScale = 0;
        ball.GetComponentInChildren<NewBallArtManager>().SetInfo(_ballCount);
        
        firstBall = ball;

        balls.Add(ball);
        _ballCount++;
        NewScoreManager._ballCount = _ballCount;


        // Play the ball pop-in animation and set the game to ready
        yield return StartCoroutine(ball.GetComponentInChildren<BallLine>().PopIn());
    }

    void SpawnBall()
    {
        if(_ballCount < endgameBallCount) { 

            // Debug.Log("1. Spawning ball.");
            Vector2 ballSpawnPos = new Vector2(Random.Range(-2.25f, 2.25f), -6);
            NewBall ball = Instantiate(m_BallPrefab);

            ball.transform.position = ballSpawnPos;
            ball.SetBallState(NewBall.BallState.launching);
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.up * ballLaunchForce;
            ball.GetComponentInChildren<NewBallArtManager>().SetInfo(_ballCount);
            // ball.GetComponentInChildren<NewBallArtManager>().HandleLaunch();

            balls.Add(ball);
            _ballCount++;

            // ballsSortedByDepth.Add(ball.GetComponent<NewBallArtManager>());

            if(scoreIndex < ballSpawnScores.Length) {
                // BallCountdownManager.GetInstance().SetCountdownNumber(ballSpawnScores[scoreIndex] - NewScoreManager._peakCount);
            }
        }
    }

    public void UpdateBallScale(float s) {
        ballScale = s;
        foreach(NewBall b in balls) {
            b.SetScale();
        }
    } 

    public void SetBallLaunchScores()
    {
        switch (ballSpawnSpeed)
        {
            case BallSpawnSpeed.slow:
                ballSpawnScores = slowBallSpawnScores;
                break;

            case BallSpawnSpeed.med:
                ballSpawnScores = normalBallSpawnScores;
                break;

            case BallSpawnSpeed.fast:
                ballSpawnScores = fastBallSpawnScores;
                break;
        }
    }

    void CheckBallLaunch()
    {
        if (scoreIndex < ballSpawnScores.Length)
        {
            if (NewScoreManager._peakCount == ballSpawnScores[scoreIndex])
            {
                scoreIndex++;
                EventManager.TriggerEvent("SpawnBall");
            }
        }
    }

    public static int endgameBallCount = 9;

    public void UpdateEndgame(NewBall b)
    {
        if (_ballCount == endgameBallCount)
        {
            b.ProcessStageTransition();
        }
    }

    void OnBallDied()
    {
        scoreIndex = 0;
        firstBall = null;
    }

    public void FreezeBalls()
    {
        foreach (NewBall b in balls)
        {
            b.FreezeBall();
        }
    }

    public void KillAllBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            // if (!balls[i].dead)
            // {
                balls[i].GetComponent<NewBall>().DestroyMe();
            // }
        }

        balls.Clear();
        _ballCount = 0;
    }

    public void HideFirstBall() {
        // Called when the settings screen appears
        StartCoroutine(firstBall.gameObject.GetComponentInChildren<BallLine>().HideBall());
        _ballCount = 0;
        balls.Clear();
    }

    bool keepChecking;

    public bool AllBallsNormal() {
        if(!keepChecking) { return true; }

        foreach (NewBall b in balls) {
            if(b.stage != NewBall.BallStage.normal) {
                return false;
            }
        }
        keepChecking = false;
        return true;
    }

    public Vector2 GetLaunchVelocity() {
        return Vector2.up * ballLaunchForce;
    }
}
