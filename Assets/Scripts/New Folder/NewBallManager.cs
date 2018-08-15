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

    List<NewBall> balls = new List<NewBall>();
    List<NewBallArtManager> ballsSortedByDepth = new List<NewBallArtManager>();
    public float ballLaunchForce = 10;

    public Color[] m_BallColors;
    public Color deadBallColor;

    public int ballSpeedIndex;  // used to choose a SET, ie: slow, normal, fast
    int scoreIndex;             // used to trigger the ball spawn, the index of the score WITHIN one set
    int[] ballSpawnScores;
    int[] slowBallSpawnScores = new int[] { 5, 10, 25, 50, 75, 100, 125 };
    int[] normalBallSpawnScores = new int[] { 5, 15, 25, 40, 55, 70, 99 };
    int[] fastBallSpawnScores = new int[] { 5, 10, 15, 20, 25, 30, 35 };

    public enum BallSpawnSpeed { slow, med, fast };
    public BallSpawnSpeed ballSpawnSpeed = BallSpawnSpeed.med;

    public static bool allowSlaps;

    public int juggleThreshold = 3;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SpawnBall", SpawnBall);
        EventManager.StartListening("BallCaught", CheckBallLaunch);
        EventManager.StartListening("BallSlapped", CheckBallLaunch);
        EventManager.StartListening("BallDied", OnBallDied);
        EventManager.StartListening("Reset", KillAllBalls);

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

    public bool AnyBallBeingThrown()
    {
        bool anyBallThrowing = false;

        foreach (NewBall n in balls)
        {
            if (n.m_BallThrown)
            {
                anyBallThrowing = true;
                break;
            }
        }
        return anyBallThrowing;
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

    void SpawnBall()
    {
        Vector2 ballSpawnPos = new Vector2(Random.Range(-2.25f, 2.25f), -6);
        NewBall ball = Instantiate(m_BallPrefab);

        ball.transform.position = ballSpawnPos;
        ball.m_Launching = true;
        ball.canBeCaught = false;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.up * ballLaunchForce;
        ball.GetComponentInChildren<NewBallArtManager>().SetInfo(_ballCount);

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
    }

    IEnumerator FreezeBalls()
    {
        foreach (NewBall b in balls)
        {
            b.FreezeBall();
        }

        yield return null;
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

    public bool FirstBallGotThrown() {
        
        return false;
    }
}
