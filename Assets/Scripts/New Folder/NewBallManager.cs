using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<NewBall> balls = new List<NewBall>();
    public float ballLaunchForce = 10;

    public Color[] m_BallColors;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SpawnBall", SpawnBall);
        EventManager.StartListening("BallCaught", CheckBallLaunch);
        EventManager.StartListening("BallSlapped", CheckBallLaunch);
        EventManager.StartListening("BallDied", OnBallDied);
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

    void SpawnBall()
    {
        Vector2 ballSpawnPos = new Vector2(Random.Range(-2.25f, 2.25f), -6);
        NewBall ball = Instantiate(m_BallPrefab);

        ball.transform.position = ballSpawnPos;
        ball.m_Launching = true;
        ball.canBeCaught = false;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.up * ballLaunchForce;
        ball.GetComponent<NewBall>().SetColor(m_BallColors[_ballCount]);

        balls.Add(ball);
        _ballCount++;
    }

    int scoreIndex;
    int[] ballSpawnScores = new int[] { 5, 10, 15, 20, 25, 30, 35 };

    void CheckBallLaunch()
    {
        if(scoreIndex < ballSpawnScores.Length) {
            if (NewScoreManager._catchCount == ballSpawnScores[scoreIndex])
            {
                scoreIndex++;
                EventManager.TriggerEvent("SpawnBall");
            }
        } 
        // else {
        //     foreach(NewBall b in balls) {
                
        //     }
        // }

    }

    void OnBallDied()
    {
        NewGameManager.GetInstance().SetState(GameState.gameOver);
    }

    IEnumerator KillBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].GetComponent<NewBall>().Die();
            yield return new WaitForSeconds(.2f);
        }

        balls.Clear();
        _ballCount = 0;
    }
}
