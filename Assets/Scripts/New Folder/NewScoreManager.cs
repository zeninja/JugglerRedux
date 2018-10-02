using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewScoreManager : MonoBehaviour
{

    #region instance
    private static NewScoreManager instance;
    public static NewScoreManager GetInstance()
    {
        return instance;
    }

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
    #endregion

    public static int _ballCount;
    public static int _peakCount;
    public static float _progress;
    public static float _lastPeakCount;

    string currentScoreString;
    string highScoreString;

    public TextMeshPro scoreText;
    public TextMeshPro highScoreText;

    decimal currentScore;
    decimal highscore;

    int m_SavedBallCount;
    int m_SavedPeakCount;

    static string ballKey = "ballKey";
    static string peakKey = "peakKey";
    static string highScoreKey = "highScore";

    public Gradient highscoreGradient;
    public Color scoreColor;

    // public float pauseBeforeCountdown = .25f;
    public float inOutDuration;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("SpawnBall", OnBallSpawned);
        EventManager.StartListening("BallPeaked", OnBallPeaked);

        InitHighscore();
    }

    int GetScore(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        else
        {
            return 0;
        }
    }

    void InitHighscore()
    {
        m_SavedBallCount = GetScore(ballKey);
        m_SavedPeakCount = GetScore(peakKey);
        highScoreString = m_SavedBallCount.ToString() + "." + m_SavedPeakCount.ToString();
        SetHighscoreText();
    }

    void SetHighscoreText()
    {
        highScoreText.text = highScoreString;
    }

    // Update is called once per frame
    void Update()
    {
        currentScoreString = _ballCount.ToString() + "." + _peakCount.ToString();
        scoreText.text = currentScoreString;

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearHighscore();
        }
    }

    public void ClearHighscore()
    {
        PlayerPrefs.DeleteAll();
        highScoreString = "0.0";
        SetHighscoreText();
    }

    void OnBallSpawned()
    {
        _ballCount++;
    }

    void OnBallPeaked()
    {
        scoreText.text = currentScoreString;
        _peakCount++;
    }

    public void Reset()
    {
        _peakCount = 0;
        _ballCount = 0;

        currentScoreString = _ballCount.ToString() + "." + _peakCount.ToString();
        newHighscore = false;
    }

    public static float GetProgressPercent()
    {
        return _progress;
    }

    public static bool newHighscore;

    public void CheckHighscore()
    {
        currentScore = decimal.Parse(string.Format("{0}.{1}", _ballCount.ToString(), _peakCount.ToString()));
        highscore = (decimal)PlayerPrefs.GetFloat(highScoreKey);
        newHighscore = currentScore > highscore;

        // Debug.Log(currentScore + " | " + highscore);
    }

    public IEnumerator HighscoreProcess()
    {
        highscore = (decimal)PlayerPrefs.GetFloat(highScoreKey);
        _lastPeakCount = _peakCount;

        if (newHighscore)
        {

            highscore = currentScore;
            highScoreString = highscore.ToString();

            SetHighscoreText();

            PlayerPrefs.SetInt(ballKey, _ballCount);
            PlayerPrefs.SetInt(peakKey, _peakCount);

            PlayerPrefs.SetFloat(highScoreKey, (float)highscore);

            // Debug.Log("REPORTING HIGH SCORE. VALUE IS: " + highscore);
            #if UNITY_IOS && !UNITY_EDITOR
            GameCenter.GetInstance().SetHighScore(highscore);
            #else
            #endif
        }
        else
        {
            yield return new WaitForSeconds(.1f);
        }
    }

    public IEnumerator RainbowText()
    {
        // yield return new WaitForSeconds(pauseBeforeCountdown);

        float t = 0;
        inOutDuration = .15f;

        while (t < inOutDuration)
        {
            t += Time.fixedDeltaTime;
            float percent = t / inOutDuration;

            scoreText.color = Color.Lerp(scoreColor, highscoreGradient.Evaluate(0), percent);
            yield return new WaitForFixedUpdate();
        }

        t = 0;
        float duration = 1f;

        while (t < duration)
        {
            t += Time.fixedDeltaTime;
            float percent = t / duration;

            scoreText.color = highscoreGradient.Evaluate(percent);

            yield return new WaitForFixedUpdate();
        }

        t = 0;

        while (t < inOutDuration)
        {
            t += Time.fixedDeltaTime;
            float percent = t / inOutDuration;

            scoreText.color = Color.Lerp(highscoreGradient.Evaluate(0), scoreColor, percent);
            yield return new WaitForFixedUpdate();
        }

        scoreText.color = scoreColor;
    }

    public void EnableScore(bool val)
    {
        scoreText.gameObject.SetActive(val);
    }
}
