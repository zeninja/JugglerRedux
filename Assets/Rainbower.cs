using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbower : MonoBehaviour
{
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

    static Rainbower instance;
    public static Rainbower GetInstance()
    {
        return instance;
    }

    List<StackerDot> dots;
    public Color[] colors;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(MakeWaves(numBalls));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            numBalls++;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            numBalls--;
        }
        numBalls = Mathf.Clamp(numBalls, 1, 9);

    }

    public int numBalls;

    public Extensions.Property tideDuration;

    float GetTideDuration(float completionPercentage)
    {
        float calculatedDuration = tideDuration.start + (tideDuration.end - tideDuration.start) * completionPercentage;
        return calculatedDuration;
    }

    public void SetDots(List<StackerDot> a_dots)
    {
        dots = a_dots;
    }

    void PrepWave()
    {
        if (NewBallManager.GetInstance() != null)
        {
            colors = new Color[NewScoreManager._numBalls];

            for (int i = 0; i < NewScoreManager._numBalls; i++)
            {
                colors[i] = BallColorManager.GetInstance().ballColors[i];
            }
        }
        else
        {
            colors = new Color[numBalls];

            for (int i = 0; i < numBalls; i++)
            {
                colors[i] = BallColorManager.GetInstance().ballColors[i];
            }
        }
    }

    public int numWaves;

    public IEnumerator MakeWaves(int ballCount)
    {
        int balls = ballCount;
        float tDuration = GetTideDuration((float)balls / 9f);

        PrepWave();
        yield return StartCoroutine(DoTheWave(numWaves, tDuration));
    }

    public float timeBetweenWaves = .15f;

    public IEnumerator DoTheWave(int numWaves, float duration)
    {
        // int totalWaveTunnelLength = numWaves * bandsPerColor;

        for (int i = 0; i < numWaves; i++)
        {
            yield return StartCoroutine(Wave(waveDuration));
            yield return StartCoroutine(Extensions.Wait(timeBetweenWaves));
        }
    }

    public float waveDuration = .3f;
    IEnumerator Wave(float duration)
    {
        float t = 0;
        float d = duration;

        while (t < d)
        {
            float p = t / d;
            UpdateAllBands(p);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        UpdateAllBands(1);
    }

    public int colorsInWave;
    public int bandsPerColor = 5;


    void UpdateAllBands(float p)
    {
        colorsInWave = Mathf.Min(colorsInWave, colors.Length);
        int tunnelLength = dots.Count + colorsInWave * bandsPerColor;
        int index = Mathf.CeilToInt(tunnelLength * EZEasings.SmoothStart5(p));

        for (int i = 0; i < dots.Count; i++)
        {
            for (int j = 0; j < colorsInWave; j++)
            {
                for (int k = 0; k < bandsPerColor; k++)
                {
                    if (index - k >= 0 && index - k < dots.Count)
                    {
                        dots[index - k].SetColor(colors[j]);
                    }

                    // Reset rings that have been passed in the wave
                    if (i <= index - colorsInWave)
                    {
                        dots[i].ReturnToDefaultColor();
                    }
                }
            }


        }
    }
}
