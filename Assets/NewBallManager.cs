﻿using System.Collections;
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

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SpawnBall", SpawnBall);
        EventManager.StartListening("BallCaught", OnBallCaught);
        EventManager.StartListening("BallDied", OnBallDied);
    }

    public bool anyBallThrowing = false;
    public float targetTimeScale = 1;
    public float currentTimeScale;
    public float m_SlowTimeScale = .25f;
    float m_NormalTimeScale = 1.0f;

    public float m_TimeSmoothing = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerEvent("SpawnBall");
        }

        SlowTimeBasedOnThrows();
    }

    void SlowTimeBasedOnThrows()
    {
        anyBallThrowing = false;

        foreach (NewBall n in balls)
        {
            if (n.m_BeingThrown)
            {
                anyBallThrowing = true;
                break;
            }
        }

        if (anyBallThrowing)
        {
            targetTimeScale = m_SlowTimeScale;
        }
        else
        {
            targetTimeScale = m_NormalTimeScale;
        }

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.deltaTime * m_TimeSmoothing);
        currentTimeScale = Time.timeScale;
    }

    void SpawnBall()
    {
        Vector2 ballSpawnPos = new Vector2(Random.Range(-2.25f, 2.25f), -6);
        NewBall ball = Instantiate(m_BallPrefab);

        ball.transform.position = ballSpawnPos;
        ball.m_Launching = true;
        ball.canBeCaught = false;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.up * ballLaunchForce;

        balls.Add(ball);
        _ballCount++;
    }

    void OnBallCaught()
    {
        if (NewScoreManager._catchCount % 5 == 0)
        {
            EventManager.TriggerEvent("SpawnBall");
        }
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
