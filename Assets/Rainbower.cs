using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbower : MonoBehaviour
{
    static Rainbower instance;
    public static Rainbower GetInstance()
    {
        return instance;
    }

	List<StackerDot> dots;
    Color[] colors;

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

    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(MakeWaves());
        }

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
    
    public IEnumerator MakeWaves() {
        Debug.Log("makin waves");
        int balls = NewScoreManager._numBalls;
        float waveTime = GetTideDuration((float)balls / 9f);
        yield return StartCoroutine(DoTheWave(balls, waveTime));
    }

    public float timeBetweenWaves = .15f;

    public IEnumerator DoTheWave(int numWaves, float duration)
    {
        PrepWave();

        // float timeBetweenWaves = (float)duration / (float)numWaves;
        for (int i = 0; i < numWaves; i++)
        {
            StartCoroutine(Wave(waveDuration));
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
            AdjustRings(p);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        AdjustRings(1);
	}

    void AdjustRings(float p)
    {
        int waveRings = colors.Length;
        float tunnelLength = dots.Count + waveRings;
		int index = Mathf.CeilToInt(tunnelLength * EZEasings.SmoothStart2(p));

        for (int i = 0; i < dots.Count; i++)
        {
            // Adjust the set of rings in the wave
            for (int j = 0; j < waveRings; j++)
            {
                if (index - j >= 0 && index - j < dots.Count)
                {
                    dots[index - j].SetColor(colors[j]);
                }
            }

            // Reset rings that have been passed in the wave
            if (i <= index - waveRings)
            {
                dots[i].ReturnToDefaultColor();
            }
        }
    }


	// public float timeBetweenWaves;

	// public IEnumerator DoTheWave(int numWaves)
    // {
    //     for (int i = 0; i < numWaves; i++)
    //     {
    //         StartCoroutine(Wave(waveDuration));
    //         yield return StartCoroutine(Extensions.Wait(timeBetweenWaves));
    //     }
    // }

	// IEnumerator Wave(float duration)
    // {
    //     int waveRings = colors.Length;
    //     int iterations = dots.Count + waveRings;
    //     float split = duration / (float)iterations;

    //     for (int index = 0; index < iterations; index++)
    //     {
    //         for (int i = 0; i < dots.Count; i++)
    //         {
    //             // Adjust the set of rings in the wave
    //             for (int j = 0; j < waveRings; j++)
    //             {
    //                 if (index - j >= 0 && index - j < dots.Count)
    //                 {
    //                     dots[index - j].SetColor(colors[j]);
    //                 }
    //             }

    //             // Reset rings that have been passed in the wave
    //             if (i <= index - waveRings)
    //             {
    //                 dots[i].ReturnToDefaultColor();
    //             }
    //         }
    //         yield return StartCoroutine(Extensions.Wait(split));
    //     }
    // }
}
