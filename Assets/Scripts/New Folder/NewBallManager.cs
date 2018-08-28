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
    public float ballScale = .4f;

    NewBall firstBall;
    List<NewBall> balls = new List<NewBall>();
    List<NewBallArtManager> ballsSortedByDepth = new List<NewBallArtManager>();
    public float ballLaunchForce = 10;

    public Color[] m_BallColors;

    public int ballSpeedIndex;  // used to choose a SET, ie: slow, normal, fast
    int scoreIndex;             // used to trigger the ball spawn, the index of the score WITHIN one set
    int[] ballSpawnScores;
    int[] slowBallSpawnScores = new int[] { 5, 10, 25, 50, 75, 100, 125 };
    int[] normalBallSpawnScores = new int[] { 5, 15, 25, 40, 55, 70, 99 };
    int[] fastBallSpawnScores = new int[] { 5, 10, 15, 20, 25, 30, 35 };

    public enum BallSpawnSpeed { slow, med, fast };
    public BallSpawnSpeed ballSpawnSpeed = BallSpawnSpeed.med;

    // public static bool allowSlaps;

    public int juggleThreshold = 3;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SpawnBall", SpawnBall);
        EventManager.StartListening("BallCaught", CheckBallLaunch);
        EventManager.StartListening("BallSlapped", CheckBallLaunch);
        EventManager.StartListening("BallDied", OnBallDied);

        ballSpawnScores = normalBallSpawnScores;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerEvent("SpawnBall");
        }
    }

    public bool JuggleThresholdReached()
    {
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

    public void SpawnFirstBall() {
        if(firstBall) { return; }

        xSwitcher *= -1;

        Vector2 ballSpawnPos = new Vector2(xSwitcher, -2);
        NewBall ball = Instantiate(m_BallPrefab);

        ball.transform.position = ballSpawnPos;
        ball.m_Launching = false;
        ball.canBeCaught = false;
        ball.firstBall   = true;
        ball.GetComponent<Rigidbody2D>().velocity  = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().gravityScale = 0;
        ball.GetComponentInChildren<NewBallArtManager>().SetInfo(_ballCount);
        ball.GetComponentInChildren<NewBallArtManager>().PopInAnimation();
        firstBall = ball;

        balls.Add(ball);
        _ballCount++;
        NewScoreManager._numBalls = _ballCount;

        ballsSortedByDepth.Add(ball.GetComponent<NewBallArtManager>());
    }

    void SpawnBall()
    {
        // Debug.Log("1. Spawning ball.");
        Vector2 ballSpawnPos = new Vector2(Random.Range(-2.25f, 2.25f), -6);
        NewBall ball = Instantiate(m_BallPrefab);

        ball.transform.position = ballSpawnPos;
        ball.m_Launching = true;
        ball.canBeCaught = false;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.up * ballLaunchForce;
        ball.GetComponentInChildren<NewBallArtManager>().SetInfo(_ballCount);
        ball.GetComponentInChildren<NewBallArtManager>().HandleLaunch();

        balls.Add(ball);
        _ballCount++;

        ballsSortedByDepth.Add(ball.GetComponent<NewBallArtManager>());
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
            if (NewScoreManager._catchCount == ballSpawnScores[scoreIndex])
            {
                scoreIndex++;
                EventManager.TriggerEvent("SpawnBall");
            }
        }
    }

    public void UpdateEndgame(NewBall nb)
    {
        int endgameIndex = 8;

        if (_ballCount == endgameIndex)
        {
            if (!AllBallsUnitedAtIndex(endgameIndex))
            {
                if (nb.ballColorIndex < endgameIndex)
                {
                    nb.ballColorIndex++;
                    nb.UpdateColor();
                }
            }
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
            if (!balls[i].dead)
            {
                balls[i].GetComponent<NewBall>().DestroyMe();
            }
        }

        balls.Clear();
        _ballCount = 0;
    }

    public bool AllBallsUnitedAtIndex(int index)
    {
        foreach (NewBall b in balls)
        {
            if (b.ballColorIndex != index)
            {
                return false;
            }
        }

        return true;
    }

    public Vector2 GetLaunchVelocity() {
        return Vector2.up * ballLaunchForce;
    }
}
