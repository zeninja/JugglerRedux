using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    static GameOverManager instance;
    public static GameOverManager GetInstance()
    {
        return instance;
    }

    SpriteRenderer target;
    NewScoreManager scoreManger;

    public SpriteRenderer demo;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        scoreManger = GetComponent<NewScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (demo != null)
            {
                target = demo;
                NewScoreManager._numBalls = 6;
                NewScoreManager._catchCount = 66;
                StartGameOver();
            }
        }
    }

    public void SetTargetBall(SpriteRenderer s) {
        target = s;
        target.sortingOrder = 100;
    }

    public void StartGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        NewBallManager.GetInstance().FreezeBalls();
        
        yield return StartCoroutine(Explode());
        NewBallManager.GetInstance().KillAllBalls();

        yield return StartCoroutine(CountdownScore());

        yield return StartCoroutine(Implode());

        ScoreMaskEffect.GetInstance().Reset();
        NewGameManager.GetInstance().ResetGame();

        Destroy(target);
    }

    public float explodeDuration = 1.47f;

    IEnumerator Explode()
    {
        float t = 0;
        float targetScale = 40;

        while (t < explodeDuration)
        {
            // Debug.Log("growing");
            t += Time.fixedDeltaTime;
            target.transform.localScale = Vector3.one + Vector3.one * targetScale * EZEasings.SmoothStart5(t / explodeDuration);
            yield return new WaitForFixedUpdate();
        }

    }
    public float implodeDuration = 1.0f;

    IEnumerator Implode()
    {
        Vector3 startScale = target.transform.localScale;
        float t = 0;

        while (t < implodeDuration)
        {
            // Debug.Log("imploding");
            t += Time.fixedDeltaTime;
            target.transform.localScale = startScale - startScale * EZEasings.SmoothStop5(t / implodeDuration);

            yield return new WaitForFixedUpdate();
        }

    }

    // public Property shortDelayRange;
    // public Property longDelayRange;

    // [System.Serializable]
    // public class Property
    // {
    //     public float start;
    //     public float end;
    // }

    float percent;

    public float countdownDuration = 1;

    IEnumerator CountdownScore()
    {
        yield return StartCoroutine(ScoreMaskEffect.GetInstance().PrepEffect(target));


        float shortDelay = countdownDuration / NewScoreManager._catchCount;

        while (NewScoreManager._catchCount > 0)
        {
            NewScoreManager._catchCount--;
            yield return new WaitForSeconds(shortDelay);
        }

        yield return new WaitForSeconds(.5f);

        float longDelay = countdownDuration / NewScoreManager._numBalls;

        while (NewScoreManager._numBalls > 0)
        {
            NewScoreManager._numBalls--;
            yield return new WaitForSeconds(longDelay);
        }
    }

    // float GetSmoothStepRange(Property p)
    // {
    //     return p.start + (p.end - p.start) * EZEasings.SmoothStart3(percent);
    // }
}
