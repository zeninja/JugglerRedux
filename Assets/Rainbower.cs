using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbower : MonoBehaviour
{
    #region
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
    #endregion

    List<StackerDot> dots;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(MakeWaves(numWaves));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
        }
        PrepWave();
    }

    Color[] colors;
    public int numWaves;
    public int colorsInWave;
    public int bandsPerColor = 5;
    public float waveDuration = .3f;
    public float timeBetweenWaves = .15f;

    public Extensions.Property waveIntervalRange;

    public void SetDots(List<StackerDot> a_dots)
    {
        dots = a_dots;
    }

    void PrepWave()
    {
        if (NewBallManager.GetInstance() != null)
        {
            colors = new Color[NewScoreManager._ballCount];
            for (int i = 0; i < NewScoreManager._ballCount; i++)
            {
                colors[i] = BallColorManager.GetInstance().ballColors[i];
            }
        }
        else
        {
            colors = new Color[colorsInWave];
            for (int i = 0; i < colorsInWave; i++)
            {
                colors[i] = BallColorManager.GetInstance().ballColors[i];
            }
        }
    }



    public IEnumerator MakeWaves(int ballCount)
    {
        int ballsScored = ballCount;
        numWaves = ballsScored;
        colorsInWave = ballsScored;

        timeBetweenWaves = Extensions.GetSmoothStart3Range(waveIntervalRange, (float)ballsScored / 9f);

        PrepWave();
        yield return StartCoroutine(DoTheWave(ballsScored));
    }

    public IEnumerator DoTheWave(int numWaves)
    {
        for (int i = 0; i < numWaves; i++)
        {
            StartCoroutine(Wave(waveDuration, i));
            yield return StartCoroutine(Extensions.Wait(timeBetweenWaves));
        }
    }

    IEnumerator Wave(float duration, int index)
    {
        float t = 0;
        float d = duration;

        while (t < d)
        {
            float p = t / d;
            UpdateAllBands(p, index);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        UpdateAllBands(1, index);
    }

    void UpdateAllBands(float p, int colorIndex)
    {
        colorsInWave = Mathf.Min(colorsInWave, colors.Length);
        int tunnelLength = dots.Count + colorsInWave * bandsPerColor;
        int index = Mathf.CeilToInt(tunnelLength * EZEasings.SmoothStart5(p));

        for (int i = 0; i < dots.Count; i++)
        {
            for(int k = 0; k < bandsPerColor; k++) {
                if(index - k >= 0 && index - k < dots.Count) {
                    dots[index - k].SetColor(colors[colorIndex]);
                    // ParticleRingSpawner.GetInstance().TriggerRing(index - k, colors[colorIndex]);
                }

                // Reset rings that have been passed in the wave
                if (i <= index - bandsPerColor)
                {
                    dots[i].ReturnToDefaultColor();
                }
            }
        }

            // for (int j = 0; j < colorsInWave; j++)
            // {

            //     for (int k = 0; k < bandsPerColor; k++)
            //     {
            //         if (index - k >= 0 && index - k < dots.Count)
            //         {
            //             dots[index - k].SetColor(colors[j]);
            //         }

            //         // Reset rings that have been passed in the wave
            //         if (i <= index - bandsPerColor)
            //         {
            //             dots[i].ReturnToDefaultColor();
            //         }
            //     }
            // }
    }

    // public ParticleRing p;

    // void SpawnParticleRings() {

    // }
}
