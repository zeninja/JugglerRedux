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

    public void SetTargetBall(NewBall ball) {
        deadBall = ball;
        // target = deadBall.ballArtManager.m_BallSprite;
        // target.enabled = true;
        // target.sortingOrder = 100;

        deadBallPos = ball.transform.position;
    }

    public void SwitchState(int index) {
        switch(index) {
            case 0:
                StartCoroutine(GameOverIn());
                break;
            case 1:
                StartCoroutine(GameOverOut());
                break;
        }
    }

    NewBall deadBall;
    Vector2 deadBallPos;

    IEnumerator GameOverIn()
    {
        NewBallManager.GetInstance().FreezeBalls();
        NewScoreManager.GetInstance().CheckHighscore();

        yield return StartCoroutine(GameOverStacker.GetInstance().HandleGameOver(deadBallPos, deadBall.ballArtManager.myColor));
        
        NewBallManager.GetInstance().KillAllBalls();
        EventManager.TriggerEvent("CleanUp");

        yield return new WaitForSeconds(.165f);
        yield return StartCoroutine(ScoreMaskEffect.GetInstance().PopInScoreMask());
        // yield return new WaitForSeconds(.15f);
        yield return StartCoroutine(NewScoreManager.GetInstance().HighscoreProcess());

        // Make this infinite
        yield return StartCoroutine(Rainbower.GetInstance().LotsOfSwooshes(NewScoreManager._ballCount));

        NewGameManager.GetInstance().GameOverInComplete();
    }

    public IEnumerator GameOverOut() {
        yield return StartCoroutine(ScoreMaskEffect.GetInstance().PlayMaskOut());

        NewScoreManager.GetInstance().Reset();
        NewScoreManager.GetInstance().EnableScore(false);

        yield return StartCoroutine(InterstitalAd());
        // yield return StartCoroutine(ShowLogo());
        
        yield return new WaitForSeconds(.15f);
        yield return StartCoroutine(GameOverStacker.GetInstance().HideCircles());
        
        // Destroy(target.transform.root.gameObject);

        ScoreMaskEffect.GetInstance().Reset();
        NewGameManager.GetInstance().ResetGame();
    }

    public float explodeDuration = 1.47f;

    // IEnumerator Explode()
    // {
    //     float t = 0;
    //     float targetScale = 40;

    //     while (t < explodeDuration)
    //     {
    //         t += Time.fixedDeltaTime;
    //         target.transform.localScale = Vector3.one + Vector3.one * targetScale * EZEasings.SmoothStop3(t / explodeDuration);
    //         yield return new WaitForFixedUpdate();
    //     }

    // }
    public float implodeDuration = 1.0f;

    IEnumerator Implode()
    {
        Vector3 startScale = target.transform.localScale;
        float t = 0;

        while (t < implodeDuration)
        {
            t += Time.fixedDeltaTime;
            target.transform.localScale = startScale - startScale * EZEasings.SmoothStop5(t / implodeDuration);

            yield return new WaitForFixedUpdate();
        }

        Destroy(target.transform.root.gameObject);
    }

    IEnumerator InterstitalAd() {
        NewAdManager.GetInstance().ShowVideoAd();
        while(NewAdManager.GetInstance().ShowingAd()) {
            yield return new WaitForEndOfFrame();
        }
    }
}
