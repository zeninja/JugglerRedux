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
        target = deadBall.ballArtManager.m_BallSprite;
        target.enabled = true;
        target.sortingOrder = 100;

        deadBallPos = ball.transform.position;
    }
    
    NewBall deadBall;
    Vector2 deadBallPos;

    public void StartGameOver()
    {
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        NewBallManager.GetInstance().FreezeBalls();
        NewScoreManager.GetInstance().CheckHighscore();

        yield return StartCoroutine(GameOverStacker.GetInstance().HandleGameOver(deadBallPos, deadBall.ballArtManager.myColor));
        
        NewBallManager.GetInstance().KillAllBalls();
        EventManager.TriggerEvent("CleanUp");

        yield return StartCoroutine(ScoreMaskEffect.GetInstance().PopInScoreMask(target));
        yield return new WaitForSeconds(.15f);
        yield return StartCoroutine(NewScoreManager.GetInstance().HighscoreProcess());
        yield return StartCoroutine(ScoreMaskEffect.GetInstance().PlayMaskOut());

        NewScoreManager._peakCount = 0;
        NewScoreManager._ballCount  = 0;

        NewScoreManager.GetInstance().EnableScore(false);
        
        yield return StartCoroutine(InterstitalAd());
        yield return StartCoroutine(ShowLogo());

        NewScoreManager.GetInstance().EnableScore(true);
        
        yield return new WaitForSeconds(.15f);
        yield return StartCoroutine(GameOverStacker.GetInstance().HideCircles());
        
        Destroy(target.transform.root.gameObject);

        ScoreMaskEffect.GetInstance().Reset();
        NewGameManager.GetInstance().ResetGame();

    }  

    public float explodeDuration = 1.47f;

    IEnumerator Explode()
    {
        float t = 0;
        float targetScale = 40;

        while (t < explodeDuration)
        {
            t += Time.fixedDeltaTime;
            target.transform.localScale = Vector3.one + Vector3.one * targetScale * EZEasings.SmoothStop3(t / explodeDuration);
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

    public LogoAnimator logoAnimator;

    IEnumerator ShowLogo() {
        logoAnimator.ShowLogo();
        while(logoAnimator.GetComponent<Animation>().isPlaying) {
            yield return new WaitForEndOfFrame();
        }
    }
}
