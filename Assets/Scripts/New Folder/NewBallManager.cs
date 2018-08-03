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

    int scoreIndex;
    int[] ballSpawnScores;
    int[] slowBallSpawnScores   = new int[] { 5, 10, 25, 50, 75, 100, 125 };
    int[] normalBallSpawnScores = new int[] { 5, 15, 25, 40, 55, 70, 99 };
    int[] fastBallSpawnScores   = new int[] { 5, 10, 15, 20, 25, 30, 35 };
    public Toggle ballSpeedToggle;
    public Text ballSpeedDisplay;

    public enum BallSpawnSpeed { slow, med, fast };
    public BallSpawnSpeed ballSpawnSpeed;

    public static bool allowSlaps;
    public Toggle slapToggle;

    public int juggleThreshold = 1;
    public Text ui_numBallsToTriggerSlow;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SpawnBall", SpawnBall);
        EventManager.StartListening("BallCaught", CheckBallLaunch);
        EventManager.StartListening("BallSlapped", CheckBallLaunch);
        EventManager.StartListening("BallDied", OnBallDied);

        SetBallLaunchScores();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerEvent("SpawnBall");
        }
    }
    
    public bool AnyBallBeingThrown() {
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

    public bool JuggleThresholdReached() {
        int numBallsThrowing = 0;

        foreach (NewBall n in balls)
        {
            if (n.m_BallThrown)
            {
                numBallsThrowing++;
                if(numBallsThrowing >= juggleThreshold) {
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
        // ball.GetComponent<NewBallArtManager>().SetColor(_ballCount);
        
        // ball.GetComponent<NewBall>().SetColor(m_BallColors[_ballCount]);
        // ball.GetComponentInChildren<NewBallArtManager>().spriteSortIndex = (_ballCount);

        balls.Add(ball);
        _ballCount++;

        ballsSortedByDepth.Add(ball.GetComponent<NewBallArtManager>());
    }

    // public void MoveBallToFront(NewBallArtManager b) {
    //     ballsSortedByDepth.Remove(b);
    //     ballsSortedByDepth.Insert(0, b);
        
    //     for(int i  = 0; i < ballsSortedByDepth.Count; i++) {
    //         ballsSortedByDepth[i].SetDepth(i);
    //     }
    // }

    // Util
    void SetBallLaunchScores() {
        string ballSpeedText = "UNSET";

        switch(ballSpawnSpeed) {
            case BallSpawnSpeed.slow:
                ballSpeedText = "Ball Spawn Speed:\nSlow";
                ballSpawnScores = slowBallSpawnScores;
                break;

            case BallSpawnSpeed.med:
                ballSpeedText = "Ball Spawn Speed:\nMed";
                ballSpawnScores = normalBallSpawnScores;
                break;

            case BallSpawnSpeed.fast:
                ballSpawnScores = fastBallSpawnScores;
                ballSpeedText = "Ball Spawn Speed:\nFast";
                break;
        }
        ballSpeedDisplay.text = ballSpeedText;
    }

    public void SwitchBallLaunchSpeed() {
        if(NewBallManager._ballCount > 0) { return; }

        int ballSpeedIndex = (int)ballSpawnSpeed;
        ballSpeedIndex = (ballSpeedIndex + 1) % 3;
        ballSpawnSpeed = (BallSpawnSpeed)ballSpeedIndex;
        SetBallLaunchScores();
    }

    public void SwitchSlapsAllowed() {
        if(NewBallManager._ballCount > 0) { return; }

        allowSlaps = slapToggle.isOn;
    }

    void CheckBallLaunch()
    {
        if(scoreIndex < ballSpawnScores.Length) {
            if (NewScoreManager._catchCount == ballSpawnScores[scoreIndex])
            {
                scoreIndex++;
                EventManager.TriggerEvent("SpawnBall");
            }
        }
    }

    public void UpdateEndgame(NewBall nb) {
        int endgameIndex = 8;

        if(_ballCount == endgameIndex) {
            if ( !AllBallsUnitedAtIndex(endgameIndex)) {
                if (nb.ballColorIndex < endgameIndex) {
                    nb.ballColorIndex++;
                    nb.UpdateColor();
                }
            }
        }
    }

    void OnBallDied()
    {
        NewGameManager.GetInstance().SetState(GameState.gameOver);
        scoreIndex = 0;
    }

    public void HandleGameOver() {
        scoreIndex = 0;
    }

    IEnumerator KillBalls()
    {
        foreach(NewBall b in balls) {
            b.FreezeBall();
        }

        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].GetComponent<NewBall>().Die();
            yield return new WaitForSeconds(.2f);
        }

        balls.Clear();
        _ballCount = 0;
    }

    public bool AllBallsUnitedAtIndex(int index) {
        foreach(NewBall b in balls) {
            if(b.ballColorIndex != index) {
                return false;
            }
        }        

        return true;
    }

    public void AdjustJuggleThreshold(int adj) {
        juggleThreshold += adj;
        juggleThreshold = Mathf.Max(1, juggleThreshold);
    }
}
